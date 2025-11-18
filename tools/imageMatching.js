const Jimp = require('jimp');
const cv = require('./opencv.js');
const path = require('path')


const 图片匹配 = async (bigPath, smallPath, threshold = 0.8) => {
    try {
        // 1. 使用 Jimp 加载大图和小图
        const bigImage = await Jimp.Jimp.read(bigPath);
        const smallImage = await Jimp.Jimp.read(smallPath);

        const bigWidth = bigImage.bitmap.width;
        const bigHeight = bigImage.bitmap.height;
        const smallWidth = smallImage.bitmap.width;
        const smallHeight = smallImage.bitmap.height;

        // 检查小图是否大于大图
        if (smallWidth > bigWidth || smallHeight > bigHeight) {
            console.log('小图尺寸超过大图，无法匹配');
            return null;
        }

        // 2. 将 Jimp 图片转换为 OpenCV Mat
        const bigMat = new cv.Mat(bigHeight, bigWidth, cv.CV_8UC4);
        bigMat.data.set(bigImage.bitmap.data);

        const smallMat = new cv.Mat(smallHeight, smallWidth, cv.CV_8UC4);
        smallMat.data.set(smallImage.bitmap.data);

        // 3. 将 RGBA 转换为 BGR（OpenCV 使用 BGR 顺序）
        const bigBgr = new cv.Mat();
        const smallBgr = new cv.Mat();
        cv.cvtColor(bigMat, bigBgr, cv.COLOR_RGBA2BGR);
        cv.cvtColor(smallMat, smallBgr, cv.COLOR_RGBA2BGR);

        // 4. 转换为灰度图（忽略颜色，只关注形状和亮度）
        const bigGray = new cv.Mat();
        const smallGray = new cv.Mat();
        cv.cvtColor(bigBgr, bigGray, cv.COLOR_BGR2GRAY);
        cv.cvtColor(smallBgr, smallGray, cv.COLOR_BGR2GRAY);

        // 5. 检查小图是否有透明像素，创建掩码
        let smallMask;
        const alphaChannel = smallMat.channels() === 4 ? 3 : -1; // RGBA 中 alpha 在第4个通道
        if (alphaChannel >= 0) {
            // 从 RGBA 图片中提取 alpha 通道作为掩码
            const channels = new cv.MatVector();
            cv.split(smallMat, channels);
            const alpha = channels.get(3); // 获取 alpha 通道
            smallMask = new cv.Mat();
            cv.threshold(alpha, smallMask, 1, 255, cv.THRESH_BINARY); // 将 alpha > 0 的像素设为 255，其他为 0
            alpha.delete();
            channels.delete();
        } else {
            // 如果没有 alpha 通道，创建一个全白掩码（所有像素都参与匹配）
            smallMask = cv.Mat.ones(smallMat.rows, smallMat.cols, cv.CV_8U).mul(255);
        }

        // 6. 使用灰度图进行模板匹配（使用掩码）
        const result = new cv.Mat();
        cv.matchTemplate(bigGray, smallGray, result, cv.TM_CCOEFF_NORMED, smallMask);

        // 7. 找到最佳匹配点
        const minMax = cv.minMaxLoc(result);
        let maxValue = minMax.maxVal;
        const maxLoc = minMax.maxLoc;

        // 检查并处理异常数值
        if (!isFinite(maxValue)) {
            console.warn('检测到异常匹配值:', maxValue);
            maxValue = 0;
        }

        // 确保匹配值在合理范围内 [0, 1]
        maxValue = Math.max(0, Math.min(1, maxValue));

        console.log('匹配相似度:', maxValue);

        // 8. 检查是否达到阈值
        if (maxValue >= threshold) {
            const x = maxLoc.x;
            const y = maxLoc.y;

            // 清理内存
            bigMat.delete();
            smallMat.delete();
            bigBgr.delete();
            smallBgr.delete();
            bigGray.delete();
            smallGray.delete();
            result.delete();
            smallMask.delete();

            // 返回匹配位置（返回小图左上角坐标）
            return { x, y };
        } else {
            // 清理内存
            bigMat.delete();
            smallMat.delete();
            bigBgr.delete();
            smallBgr.delete();
            bigGray.delete();
            smallGray.delete();
            result.delete();
            smallMask.delete();

            return null;
        }

    } catch (error) {
        console.error('图片匹配错误:', error);
        return null;
    }
}



// async function main() {
//     const result = await 图片匹配(
//         path.resolve(__dirname, '../resource', `bbb.bmp`),
//         path.resolve(__dirname, '../resource', `vvv.png`),
//         0.7
//     );
//     console.log('匹配结果:', result);
// }

// setTimeout(() => {
//     main();
// }, 1000);

module.exports = {
    图片匹配
}