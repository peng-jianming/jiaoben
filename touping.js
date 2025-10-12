const WebSocket = require('ws');
const path = require('path');
const { log } = require('console');
const fs = require('fs');
const {Jimp} = require('jimp');
// 连接到上游 WebSocket 服务（ws://127.0.0.1:33332）
let upstreamWS = null;

let currentResolve = () => { }

function connectUpstream() {
    upstreamWS = new WebSocket('ws://127.0.0.1:33332');

    

    upstreamWS.on('message', (data) => {
        const res = JSON.parse(data)
        console.log("极限投屏返回值", res);
        
        currentResolve(res.result)
    });
}

connectUpstream()

const getList = () => {
    return new Promise(resolve => {
        upstreamWS.send('{ "action":"list" }')
        currentResolve = resolve
    })
}

const getScreen = (deviceIds) => {
    console.log(path.resolve(__dirname, 'resource/cache'), "===");
    
    return new Promise(resolve => {
        const data = {
            "action": "screen",
            "comm": {
                "deviceIds": deviceIds,
                "savePath": path.resolve(__dirname, 'resource/cache'),
                "onlyDeviceName": 1
            }
        }
        upstreamWS.send(JSON.stringify(data))
        currentResolve = resolve
    })
}

/**
 * 将 PNG 图片转换为 BMP 格式
 * @param {string} pngPath - PNG 图片的路径
 * @param {string} bmpPath - 输出 BMP 图片的路径（可选，默认替换原文件扩展名）
 * @returns {Promise<string>} 返回转换后的 BMP 文件路径
 */
const convertPngToBmp = async (pngPath, bmpPath = null) => {
    try {
        // 检查输入文件是否存在
        if (!fs.existsSync(pngPath)) {
            throw new Error(`PNG 文件不存在: ${pngPath}`);
        }

        // 如果没有指定输出路径，则使用原文件名替换扩展名
        if (!bmpPath) {
            const ext = path.extname(pngPath);
            const nameWithoutExt = path.basename(pngPath, ext);
            const dir = path.dirname(pngPath);
            bmpPath = path.join(dir, `${nameWithoutExt}.bmp`);
        }

        console.log(`开始转换: ${pngPath} -> ${bmpPath}`);

        // 使用 Jimp 读取 PNG 图片
        const image = await Jimp.read(pngPath);
        
        // 转换为 BMP 格式并保存
        await image.write(bmpPath);
        
        console.log(`转换完成: ${bmpPath}`);
        return bmpPath;
        
    } catch (error) {
        console.error('PNG 转 BMP 转换失败:', error.message);
        throw error;
    }
}

/**
 * 批量转换指定目录下的所有 PNG 图片为 BMP 格式
 * @param {string} directoryPath - 目录路径
 * @param {boolean} deleteOriginal - 是否删除原始 PNG 文件（默认 false）
 * @returns {Promise<Array>} 返回转换结果数组
 */
const convertPngsToBmpInDirectory = async (directoryPath, deleteOriginal = false) => {
    try {
        // 检查目录是否存在
        if (!fs.existsSync(directoryPath)) {
            throw new Error(`目录不存在: ${directoryPath}`);
        }

        // 读取目录中的所有文件
        const files = fs.readdirSync(directoryPath);
        const pngFiles = files.filter(file => path.extname(file).toLowerCase() === '.png');
        
        if (pngFiles.length === 0) {
            console.log('目录中没有找到 PNG 文件');
            return [];
        }

        console.log(`找到 ${pngFiles.length} 个 PNG 文件，开始批量转换...`);
        
        const results = [];
        
        // 逐个转换文件
        for (const pngFile of pngFiles) {
            try {
                const pngPath = path.join(directoryPath, pngFile);
                const bmpPath = await convertPngToBmp(pngPath);
                
                // 如果选择删除原始文件
                if (deleteOriginal) {
                    fs.unlinkSync(pngPath);
                    console.log(`已删除原始文件: ${pngPath}`);
                }
                
                results.push({
                    original: pngPath,
                    converted: bmpPath,
                    success: true
                });
                
            } catch (error) {
                console.error(`转换文件失败 ${pngFile}:`, error.message);
                results.push({
                    original: path.join(directoryPath, pngFile),
                    converted: null,
                    success: false,
                    error: error.message
                });
            }
        }
        
        const successCount = results.filter(r => r.success).length;
        console.log(`批量转换完成: ${successCount}/${pngFiles.length} 个文件转换成功`);
        
        return results;
        
    } catch (error) {
        console.error('批量转换失败:', error.message);
        throw error;
    }
}

module.exports = {
    getList,
    getScreen,
    convertPngToBmp,
    convertPngsToBmpInDirectory
};

