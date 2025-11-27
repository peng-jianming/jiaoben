const Jimp = require('jimp')
const { 调用ADB } = require('../touping.js')
const cv = require('./opencv.js');
const fs = require('fs');
const path = require('path');

// 文件读取锁管理器，防止并发读取同一文件导致流冲突
const fileReadLocks = new Map();

/**
 * 安全读取图片文件，避免并发冲突
 * @param {string} imagePath 图片路径
 * @param {number} maxRetries 最大重试次数
 * @param {number} retryDelay 重试延迟（毫秒）
 * @returns {Promise<Jimp>}
 */
async function safeReadImage(imagePath, maxRetries = 3, retryDelay = 100) {
    // 获取或创建文件锁队列
    if (!fileReadLocks.has(imagePath)) {
        fileReadLocks.set(imagePath, Promise.resolve());
    }
    
    // 等待之前的操作完成
    const previousLock = fileReadLocks.get(imagePath);
    await previousLock;
    
    // 创建新的锁 Promise
    let resolveLock;
    const lockPromise = new Promise(resolve => {
        resolveLock = resolve;
    });
    fileReadLocks.set(imagePath, lockPromise);
    
    try {
        // 先复制文件到临时位置，避免并发读取冲突
        const tempPath = imagePath + '.tmp.' + Date.now() + '.' + Math.random().toString(36).substr(2, 9);
        
        let retries = 0;
        while (retries < maxRetries) {
            try {
                // 复制文件到临时位置
                fs.copyFileSync(imagePath, tempPath);
                
                // 从临时文件读取
                const image = await Jimp.Jimp.read(tempPath);
                
                // 清理临时文件
                try {
                    if (fs.existsSync(tempPath)) {
                        fs.unlinkSync(tempPath);
                    }
                } catch (e) {
                    // 忽略清理错误
                }
                
                return image;
            } catch (error) {
                retries++;
                if (retries >= maxRetries) {
                    // 如果临时文件读取失败，尝试直接读取原文件（作为最后手段）
                    try {
                        const image = await Jimp.Jimp.read(imagePath);
                        return image;
                    } catch (directError) {
                        // 清理临时文件
                        try {
                            if (fs.existsSync(tempPath)) {
                                fs.unlinkSync(tempPath);
                            }
                        } catch (e) {
                            // 忽略清理错误
                        }
                        throw directError;
                    }
                }
                // 清理失败的临时文件
                try {
                    if (fs.existsSync(tempPath)) {
                        fs.unlinkSync(tempPath);
                    }
                } catch (e) {
                    // 忽略清理错误
                }
                // 等待后重试（指数退避）
                await new Promise(resolve => setTimeout(resolve, retryDelay * Math.pow(2, retries - 1)));
            }
        }
    } finally {
        // 释放锁
        resolveLock();
        // 如果这是最后一个操作，清理锁
        if (fileReadLocks.get(imagePath) === lockPromise) {
            // 延迟清理，给其他可能正在等待的操作时间
            setTimeout(() => {
                if (fileReadLocks.get(imagePath) === lockPromise) {
                    fileReadLocks.delete(imagePath);
                }
            }, 1000);
        }
    }
}


const 获取图片宽高 = async (imagePath) => {
    try {
        const image = await safeReadImage(imagePath);
        return {
            width: image.bitmap.width,
            height: image.bitmap.height
        };
    } catch (error) {
        console.error('读取图片失败:', error);
        throw error;
    }
}

const 随机区间位置 = (start, end) => {
    return Math.floor(Math.random() * (end - start)) + start;
}

const 随机区间时间 = (startMs, endMs) => {
    if (startMs > endMs) {
        [startMs, endMs] = [endMs, startMs];
    }
    // 生成包含两端点的随机整数
    return Math.floor(Math.random() * (endMs - startMs + 1)) + startMs;
}

const 随机坐标 = (x1, y1, width, height) => {
    return {
        x: 随机区间位置(x1, x1 + width),
        y: 随机区间位置(y1, y1 + height)
    }
}

const 延时 = (time) => {
    return new Promise((resolve) => setTimeout(resolve, time));
}

const 随机延时 = (startMs, endMs) => {
    return 延时(随机区间时间(startMs, endMs));
}

const ADB左键点击 = async (result) => {
    if (result) {
        // 按下
        await 调用ADB(global.hwnd, `input motionevent DOWN ${result.x} ${result.y}`)

        await 随机延时(100, 500)
        // 弹起
        await 调用ADB(global.hwnd, `input motionevent UP ${result.x} ${result.y}`)
    } else {
        console.log('左键点击坐标为空');
    }
}

const 随机点击 = async (x, y, w, h) => {
    await ADB左键点击(随机坐标(x, y, w, h))
}



const 滑动方向 = {
    向上: 'UP',
    向下: 'DOWN',
    向左: 'LEFT',
    向右: 'RIGHT'
}

/**
 * 获取拟人化滑动坐标
 * @param {Object} startRegion 起始区域 {x, y, width, height}
 * @param {Object} endRegion 结束区域 {x, y, width, height}
 * @param {string} direction 滑动方向 (使用 滑动方向 枚举)
 * @returns {Object} { start: {x,y}, end: {x,y} }
 */
// 注意上下左右由起始区域和结束区域控制,这里的direction只控制精度,哪边需要间隔小,例如上下,那么左右就间隔小
const 获取滑动坐标 = (startRegion, endRegion, direction) => {
    // 1. 在起始区域生成随机起始点
    const startP = 随机坐标(startRegion.x, startRegion.y, startRegion.width, startRegion.height);
    
    let endP = { x: 0, y: 0 };
    const maxDeviation = 30; // 非主方向的最大偏移量(像素)，模拟手指滑动不完全直

    // 辅助函数：限制数值在范围内
    const clamp = (val, min, max) => Math.max(min, Math.min(val, max));

    if (direction === 滑动方向.向上 || direction === 滑动方向.向下) {
        // === 上下滑动 ===
        // 规则：上下间隔大(由区域位置决定)，左右间隔小(需要计算)
        
        // 计算结束点X：在起始点X基础上微调，确保"左右间隔小"
        let targetX = startP.x + 随机区间位置(-maxDeviation, maxDeviation);
        
        // 确保结束点X在 endRegion 的宽范围内
        targetX = clamp(targetX, endRegion.x, endRegion.x + endRegion.width);
        
        endP.x = targetX;
        // 结束点Y在 endRegion 高度范围内随机
        endP.y = 随机区间位置(endRegion.y, endRegion.y + endRegion.height);
        
    } else {
        // === 左右滑动 ===
        // 规则：左右间隔大(由区域位置决定)，上下间隔小(需要计算)
        
        // 计算结束点Y：在起始点Y基础上微调，确保"上下间隔小"
        let targetY = startP.y + 随机区间位置(-maxDeviation, maxDeviation);
        
        // 确保结束点Y在 endRegion 的高范围内
        targetY = clamp(targetY, endRegion.y, endRegion.y + endRegion.height);
        
        endP.y = targetY;
        // 结束点X在 endRegion 宽度范围内随机
        endP.x = 随机区间位置(endRegion.x, endRegion.x + endRegion.width);
    }

    return { start: startP, end: endP };
}





const 百分之十随机用户操作 = async () => {
    if (Math.random() < 0.1) {
        const result = 随机坐标(200, 800, 600, 1000)
        console.log("🖱 触发随机事件 随机点击: (" + result.x + "," + result.y + ")");
        await ADB左键点击(result)
    }
}


/**
 * 在大图中查找小图，忽略小图的透明部分
 * @param {string} 大图路径 大图路径
 * @param {string} 小图路径 小图路径 (包含透明区域)
 * @param {number} 容差 容差值 (对应 processImage 的容差)
 * @param {number} 相似度 相似度阈值 (0-1)，默认为 0.9
 * @returns {Promise<{x: number, y: number}|null>} 返回坐标或 null
 */
async function opencv找图(大图路径, 小图路径, 容差 = 0, 相似度 = 0.9) {
    try {
        // 1. 加载图片
        if (!fs.existsSync(大图路径) || !fs.existsSync(小图路径)) {
            console.error("Image files not found.");
            return null;
        }

        const largeJimp = await safeReadImage(大图路径);
        const smallJimp = await safeReadImage(小图路径);

        // 2. 转换为 OpenCV Mat (RGBA)
        const largeMat = new cv.Mat(largeJimp.bitmap.height, largeJimp.bitmap.width, cv.CV_8UC4);
        largeMat.data.set(largeJimp.bitmap.data);

        const smallMat = new cv.Mat(smallJimp.bitmap.height, smallJimp.bitmap.width, cv.CV_8UC4);
        smallMat.data.set(smallJimp.bitmap.data);

        // 3. 提取小图的 Alpha 通道作为掩码
        const mask = new cv.Mat();
        const channels = new cv.MatVector();
        cv.split(smallMat, channels);
        const alpha = channels.get(3); // Alpha channel
        // 创建二值掩码：Alpha > 0 的部分为 255 (参与匹配)，否则为 0
        cv.threshold(alpha, mask, 0, 255, cv.THRESH_BINARY);

        // 4. 转换为 BGR 用于匹配 (matchTemplate 同时也支持带掩码的查找)
        const largeBGR = new cv.Mat();
        const smallBGR = new cv.Mat();
        cv.cvtColor(largeMat, largeBGR, cv.COLOR_RGBA2BGR);
        cv.cvtColor(smallMat, smallBGR, cv.COLOR_RGBA2BGR);

        // 5. 模板匹配
        // 使用 TM_CCORR_NORMED 配合掩码效果较好，结果接近 1.0 表示匹配
        const result = new cv.Mat();
        cv.matchTemplate(largeBGR, smallBGR, result, cv.TM_CCORR_NORMED, mask);

        // 6. 获取最佳匹配位置
        const minMax = cv.minMaxLoc(result);
        const { maxLoc, maxVal } = minMax;
        // console.log(`OpenCV match score: ${maxVal} at ${maxLoc.x},${maxLoc.y}`);

        // 释放 OpenCV 内存
        largeMat.delete(); smallMat.delete();
        largeBGR.delete(); smallBGR.delete();
        smallAlpha = null; // alias to alpha
        alpha.delete(); channels.delete();
        result.delete(); mask.delete();

        // 7. 二次校验：根据 容差 逐像素比对
        // 这一步确保满足用户的 "processImage 相同的容差" 要求
        const startX = maxLoc.x;
        const startY = maxLoc.y;

        let totalPixels = 0;
        let matchedPixels = 0;

        // 遍历小图的每一个像素
        smallJimp.scan(0, 0, smallJimp.bitmap.width, smallJimp.bitmap.height, function (x, y, idx) {
            // 获取小图当前像素
            const sA = this.bitmap.data[idx + 3];

            // 如果小图该像素是透明的，忽略比对
            if (sA === 0) return;

            totalPixels++;

            const sR = this.bitmap.data[idx + 0];
            const sG = this.bitmap.data[idx + 1];
            const sB = this.bitmap.data[idx + 2];

            // 获取大图对应位置像素
            // 检查边界
            if (startX + x >= largeJimp.bitmap.width || startY + y >= largeJimp.bitmap.height) {
                return;
            }

            const lIdx = largeJimp.getPixelIndex(startX + x, startY + y);
            const lR = largeJimp.bitmap.data[lIdx + 0];
            const lG = largeJimp.bitmap.data[lIdx + 1];
            const lB = largeJimp.bitmap.data[lIdx + 2];

            // 计算差异
            const rDiff = Math.abs(sR - lR);
            const gDiff = Math.abs(sG - lG);
            const bDiff = Math.abs(sB - lB);

            // 如果所有通道差异都在容差范围内，则认为该像素匹配
            if (rDiff <= 容差 && gDiff <= 容差 && bDiff <= 容差) {
                matchedPixels++;
            }
        });

        const matchRate = totalPixels > 0 ? matchedPixels / totalPixels : 0;
        // console.log(`Match rate: ${matchRate.toFixed(4)} (${matchedPixels}/${totalPixels})`);
        // console.log('matchRate', matchRate);

        if (matchRate >= 相似度) {
            return { x: startX, y: startY };
        } else {
            // console.log(`Found candidate at ${startX},${startY} but 相似度 ${matchRate.toFixed(4)} < ${相似度}`);
            return null;
        }

    } catch (error) {
        console.error("findImage error:", error);
        return null;
    }
}


const 裁剪图片 = async (裁剪图片路径, 裁剪区域) => {
    try {
        // 使用安全读取避免并发冲突
        const image = await safeReadImage(裁剪图片路径);
        const x = Math.floor(裁剪区域.x);
        const y = Math.floor(裁剪区域.y);
        const width = Math.floor(裁剪区域.width);
        const height = Math.floor(裁剪区域.height);
        const cropped = image.clone().crop({ x, y, w: width, h: height });
        await cropped.write(裁剪图片路径);
    } catch (error) {
        console.error('裁剪图片失败:', error);
        throw error;
    }
}

module.exports = {
    获取图片宽高,
    随机区间位置,
    随机区间时间,
    随机坐标,
    延时,
    随机延时,
    ADB左键点击,
    百分之十随机用户操作,
    opencv找图,
    裁剪图片,
    随机点击,
    获取滑动坐标
}