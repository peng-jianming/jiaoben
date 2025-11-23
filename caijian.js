const Jimp = require('jimp');
const path = require('path');

/**
 * 根据坐标裁剪图片
 * @param {string} inputPath - 输入图片路径
 * @param {string} outputPath - 输出图片路径
 * @param {number} x1 - 左上角 x 坐标
 * @param {number} y1 - 左上角 y 坐标
 * @param {number} x2 - 右下角 x 坐标
 * @param {number} y2 - 右下角 y 坐标
 * @returns {Promise<boolean>} - 是否成功
 */
async function cropImage(inputPath, outputPath, x1, y1, x2, y2) {
    try {
        // 兼容处理：根据项目中的 usage (toumingtu.js) 看起来可能是 Jimp.Jimp.read
        // 但标准用法通常是 Jimp.read。这里做一个简单的探测。
        const J = Jimp.Jimp || Jimp;

        console.log(`正在读取图片: ${inputPath}`);
        const image = await J.read(inputPath);

        // 计算宽度和高度
        const width = Math.abs(x2 - x1);
        const height = Math.abs(y2 - y1);

        // 确保起始点是左上角
        const startX = Math.min(x1, x2);
        const startY = Math.min(y1, y2);

        if (width === 0 || height === 0) {
            throw new Error('裁剪宽度或高度不能为0');
        }

        console.log(`执行裁剪: x=${startX}, y=${startY}, w=${width}, h=${height}`);

        // 裁剪
        image.crop({ x: startX, y: startY, h: height, w: width });

        // 保存
        // 优先使用 writeAsync (Jimp v0.5+), 如果没有则封装 write
        if (typeof image.writeAsync === 'function') {
            await image.writeAsync(outputPath);
        } else {
            await new Promise((resolve, reject) => {
                image.write(outputPath, (err) => {
                    if (err) reject(err);
                    else resolve();
                });
            });
        }

        console.log(`图片已保存到: ${outputPath}`);
        return true;
    } catch (error) {
        console.error('裁剪失败:', error.message);
        return false;
    }
}

// 导出函数
module.exports = cropImage;

// 测试调用 (当直接运行此文件时)
if (require.main === module) {
    const fs = require('fs');

    // 默认测试路径
    const testInput = path.join(__dirname, 'resource/cache/9a8de478.png');
    const testOutput = path.join(__dirname, 'resource/cache/ooo.png');

    if (fs.existsSync(testInput)) {
        setInterval(() => cropImage(testInput, testOutput, 1941, 246, 1941 + 429, 246 + 544), 1000);
        // 裁剪左上角 200x200 的区域

    } else {
        console.log(`测试图片不存在: ${testInput}，请手动调用函数测试。`);
    }
}

