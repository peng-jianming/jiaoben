const Jimp = require('jimp');
const cv = require('./tools/opencv.js');
const path = require('path');

/**
 * 在大图中查找小图，忽略小图的透明部分
 * @param {string} largeImagePath 大图路径
 * @param {string} smallImagePath 小图路径 (包含透明区域)
 * @param {number} tolerance 容差值 (对应 processImage 的容差)
 * @param {number} similarity 相似度阈值 (0-1)，默认为 0.9
 * @returns {Promise<{x: number, y: number}|null>} 返回坐标或 null
 */
async function findImage(largeImagePath, smallImagePath, tolerance, similarity = 0.9) {
    try {
        // 1. 加载图片
        if (!require('fs').existsSync(largeImagePath) || !require('fs').existsSync(smallImagePath)) {
            console.error("Image files not found.");
            return null;
        }

        const largeJimp = await Jimp.Jimp.read(largeImagePath);
        const smallJimp = await Jimp.Jimp.read(smallImagePath);

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

        // 7. 二次校验：根据 tolerance 逐像素比对
        // 这一步确保满足用户的 "processImage 相同的容差" 要求
        const startX = maxLoc.x;
        const startY = maxLoc.y;
        
        let totalPixels = 0;
        let matchedPixels = 0;

        // 遍历小图的每一个像素
        smallJimp.scan(0, 0, smallJimp.bitmap.width, smallJimp.bitmap.height, function(x, y, idx) {
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
            if (rDiff <= tolerance && gDiff <= tolerance && bDiff <= tolerance) {
                matchedPixels++;
            }
        });

        const matchRate = totalPixels > 0 ? matchedPixels / totalPixels : 0;
        // console.log(`Match rate: ${matchRate.toFixed(4)} (${matchedPixels}/${totalPixels})`);
        // console.log('matchRate', matchRate);
        
        if (matchRate >= similarity) {
            return { x: startX, y: startY };
        } else {
            // console.log(`Found candidate at ${startX},${startY} but similarity ${matchRate.toFixed(4)} < ${similarity}`);
            return null;
        }

    } catch (error) {
        console.error("findImage error:", error);
        return null;
    }
}

module.exports = { findImage };

// 测试代码 (如果直接运行此文件)
if (require.main === module) {
    const largePath = path.join(__dirname, 'resource/cache/aaa.png');
    const smallPath = path.join(__dirname, 'resource/cache/shimenxunqu.png');
    const tolerance = 30;
    const similarity = 0.8;

    console.log("Searching...");
    findImage(largePath, smallPath, tolerance, similarity).then(res => {
        console.log("Result:", res);
    });
}

