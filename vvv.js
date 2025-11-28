const ort = require('onnxruntime-node');
const sharp = require('sharp');
const path = require('path');

// 模型缓存，避免重复加载同一个模型文件
const modelCache = new Map();

/**
 * 加载ONNX模型
 * @param {string} modelPath - 模型文件路径
 * @returns {Promise} ONNX推理会话对象
 */
async function loadModel(modelPath) {
    // 如果模型已加载过，直接返回缓存的会话对象
    if (modelCache.has(modelPath)) return modelCache.get(modelPath);
    // 创建新的推理会话并缓存
    const session = await ort.InferenceSession.create(modelPath);
    modelCache.set(modelPath, session);
    return session;
}

/**
 * 预处理图片：将图片resize到640x640并转换为模型输入格式
 * @param {string} imagePath - 图片文件路径
 * @returns {Promise<object>} 返回预处理后的tensor数据和缩放信息
 */
async function preprocessImage(imagePath) {
    const image = sharp(imagePath);
    const metadata = await image.metadata();
    
    // 计算缩放比例（letterbox方式：保持宽高比，短边缩放到640）
    const scale = Math.min(640 / metadata.width, 640 / metadata.height);
    const newWidth = Math.round(metadata.width * scale);
    const newHeight = Math.round(metadata.height * scale);
    
    // 计算填充位置（居中填充）
    const padLeft = Math.round((640 - newWidth) / 2);
    const padTop = Math.round((640 - newHeight) / 2);
    
    // 1. 先按比例resize图片
    // 2. 然后用灰色(114,114,114)填充到640x640（YOLO标准预处理方式）
    // 3. 确保有alpha通道
    // 4. 转换为原始像素数据
    const resized = await image
        .resize(newWidth, newHeight, { fit: 'fill' })
        .extend({ top: padTop, bottom: 640 - newHeight - padTop, left: padLeft, right: 640 - newWidth - padLeft, background: { r: 114, g: 114, b: 114 } })
        .ensureAlpha()
        .raw()
        .toBuffer();
    
    // 将RGBA格式的像素数据转换为NCHW格式的Float32Array
    // NCHW格式：(batch=1, channel=3, height=640, width=640)
    // sharp的raw输出是RGBA格式，每4个字节代表一个像素
    const tensorData = new Float32Array(3 * 640 * 640);
    for (let i = 0; i < 640 * 640; i++) {
        // 归一化到[0,1]范围，并分离RGB三个通道
        tensorData[i] = resized[i * 4] / 255.0;                    // R通道
        tensorData[i + 640 * 640] = resized[i * 4 + 1] / 255.0;  // G通道
        tensorData[i + 2 * 640 * 640] = resized[i * 4 + 2] / 255.0; // B通道
    }
    
    // 返回tensor数据和缩放信息（用于后续将坐标转换回原图尺寸）
    return { tensorData, scale, padLeft, padTop };
}

/**
 * 后处理模型输出：提取检测框并转换坐标
 * @param {object} output - 模型输出tensor
 * @param {object} preprocessInfo - 预处理信息（包含scale、padLeft、padTop）
 * @param {number} confThreshold - 置信度阈值，默认0.25
 * @param {number} iouThreshold - IOU阈值（用于NMS），默认0.45
 * @returns {Array} 检测框数组
 */
function postprocess(output, preprocessInfo, confThreshold = 0.25, iouThreshold = 0.45) {
    const boxes = [];
    const { data, dims } = output;
    const { scale, padLeft, padTop } = preprocessInfo;
    
    // 判断输出格式：
    // - (1, 5, 8400) 或 (1, 84, 8400)：数据按列存储，dims[1] < dims[2]
    // - (1, 8400, 5) 或 (1, 8400, 84)：数据按行存储，dims[1] > dims[2]
    const isColumnFormat = dims[1] < dims[2];
    const numDetections = isColumnFormat ? dims[2] : dims[1];  // 检测框数量（通常是8400）
    const numClasses = (isColumnFormat ? dims[1] : dims[2]) - 4; // 类别数量（5-4=1 或 84-4=80）
    
    // 遍历所有检测框
    for (let i = 0; i < numDetections; i++) {
        let x, y, w, h, maxConf = 0, maxClass = 0;
        
        if (isColumnFormat) {
            // 列格式：(1, 5, 8400) - 数据按列存储
            // 第0列是所有检测框的x坐标，第1列是y坐标，以此类推
            x = data[0 * numDetections + i];
            y = data[1 * numDetections + i];
            w = data[2 * numDetections + i];
            h = data[3 * numDetections + i];
            
            // 获取置信度（单类别模型直接取第4列，多类别模型取最大值）
            if (numClasses === 1) {
                maxConf = data[4 * numDetections + i];
            } else {
                // 多类别：找到置信度最高的类别
                for (let j = 0; j < numClasses; j++) {
                    const conf = data[(4 + j) * numDetections + i];
                    if (conf > maxConf) { maxConf = conf; maxClass = j; }
                }
            }
        } else {
            // 行格式：(1, 8400, 5) - 数据按行存储
            // 每一行代表一个检测框：[x, y, w, h, conf1, conf2, ...]
            const offset = i * dims[2];
            x = data[offset];
            y = data[offset + 1];
            w = data[offset + 2];
            h = data[offset + 3];
            
            // 获取置信度
            if (numClasses === 1) {
                maxConf = data[offset + 4];
            } else {
                // 多类别：找到置信度最高的类别
                for (let j = 0; j < numClasses; j++) {
                    const conf = data[offset + 4 + j];
                    if (conf > maxConf) { maxConf = conf; maxClass = j; }
                }
            }
        }
        
        // 只保留置信度超过阈值的检测框
        if (maxConf > confThreshold) {
            // 将坐标从640x640空间转换回原图空间
            // YOLO输出的是中心点坐标(x,y)和宽高(w,h)
            boxes.push({
                xywh: [(x - padLeft) / scale, (y - padTop) / scale, w / scale, h / scale],
                confidence: maxConf,
                class: maxClass
            });
        }
    }
    
    // 应用NMS（非极大值抑制）去除重复检测框
    return applyNMS(boxes, iouThreshold);
}

/**
 * NMS（Non-Maximum Suppression）非极大值抑制
 * 去除重叠度高的重复检测框，只保留置信度最高的
 * @param {Array} boxes - 检测框数组
 * @param {number} iouThreshold - IOU阈值，超过此值的框会被认为是重复的
 * @returns {Array} 过滤后的检测框数组
 */
function applyNMS(boxes, iouThreshold) {
    // 按置信度从高到低排序
    boxes.sort((a, b) => b.confidence - a.confidence);
    const selected = [];
    const used = new Set(); // 记录已被抑制的框的索引
    
    for (let i = 0; i < boxes.length; i++) {
        if (used.has(i)) continue; // 跳过已被抑制的框
        
        // 保留当前框（置信度最高的）
        selected.push(boxes[i]);
        used.add(i);
        
        // 检查后续所有框，如果与当前框重叠度高，则抑制它们
        for (let j = i + 1; j < boxes.length; j++) {
            if (!used.has(j) && calculateIOU(boxes[i].xywh, boxes[j].xywh) > iouThreshold) {
                used.add(j); // 标记为已抑制
            }
        }
    }
    return selected;
}

/**
 * 计算两个检测框的IOU（Intersection over Union）交并比
 * @param {Array<number>} box1 - 第一个框 [x, y, w, h]（中心点坐标和宽高）
 * @param {Array<number>} box2 - 第二个框 [x, y, w, h]
 * @returns {number} IOU值，范围[0,1]，值越大表示重叠度越高
 */
function calculateIOU(box1, box2) {
    const [x1, y1, w1, h1] = box1;
    const [x2, y2, w2, h2] = box2;
    
    // 将中心点坐标转换为左上角和右下角坐标
    const x1Min = x1 - w1 / 2, y1Min = y1 - h1 / 2, x1Max = x1 + w1 / 2, y1Max = y1 + h1 / 2;
    const x2Min = x2 - w2 / 2, y2Min = y2 - h2 / 2, x2Max = x2 + w2 / 2, y2Max = y2 + h2 / 2;
    
    // 计算交集区域的左上角和右下角坐标
    const interXMin = Math.max(x1Min, x2Min), interYMin = Math.max(y1Min, y2Min);
    const interXMax = Math.min(x1Max, x2Max), interYMax = Math.min(y1Max, y2Max);
    
    // 计算交集面积
    const interArea = Math.max(0, interXMax - interXMin) * Math.max(0, interYMax - interYMin);
    // 计算并集面积 = 两个框的面积之和 - 交集面积
    const unionArea = w1 * h1 + w2 * h2 - interArea;
    
    // IOU = 交集面积 / 并集面积
    return unionArea > 0 ? interArea / unionArea : 0;
}

/**
 * YOLO模型推理主函数
 * @param {string} modelPath - ONNX模型文件路径
 * @param {string} imagePath - 图片文件路径
 * @param {object} options - 可选参数
 * @param {number} options.confThreshold - 置信度阈值，默认0.25
 * @param {number} options.iouThreshold - IOU阈值（NMS），默认0.45
 * @returns {Promise<object>} 返回识别结果
 * @returns {number} returns.count - 识别到的数量
 * @returns {Array} returns.detections - 检测结果数组
 * @returns {Array<number>} returns.detections[].xywh - 坐标位置 [x, y, w, h] (中心点坐标和宽高)
 * @returns {number} returns.detections[].confidence - 相似度/置信度 (0-1)
 * @returns {number} returns.detections[].class - 类别ID
 */
async function detect(modelPath, imagePath, options = {}) {
    const { confThreshold = 0.25, iouThreshold = 0.45 } = options;
    
    try {
        // 1. 加载模型
        const session = await loadModel(modelPath);
        
        // 2. 预处理图片：resize到640x640并转换为tensor格式
        const preprocessResult = await preprocessImage(imagePath);
        
        // 3. 创建输入tensor：格式为 (batch=1, channel=3, height=640, width=640)
        const inputTensor = new ort.Tensor('float32', preprocessResult.tensorData, [1, 3, 640, 640]);
        
        // 4. 运行模型推理
        const results = await session.run({ [session.inputNames[0]]: inputTensor });
        
        // 5. 后处理：提取检测框、转换坐标、应用NMS
        const boxes = postprocess(results[session.outputNames[0]], preprocessResult, confThreshold, iouThreshold);
        
        // 6. 格式化返回结果
        return {
            count: boxes.length,
            detections: boxes.map(box => ({ xywh: box.xywh, confidence: box.confidence, class: box.class }))
        };
    } catch (error) {
        throw new Error(`YOLO detection failed: ${error.message}`);
    }
}

// 导出函数供其他模块使用
module.exports = { detect };

// 如果直接运行此文件，执行示例代码
if (require.main === module) {
    (async () => {
        try {
            const result = await detect(path.join(__dirname, 'best.onnx'), path.join(__dirname, 'hhhhhhh.png'));
            console.log(result.detections);
        } catch (error) {
            console.error('错误:', error.message);
        }
    })();
}
