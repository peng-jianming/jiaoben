// 透明图制作, 对比源图片和结果图片, 如果结果图片的像素点和源图片的像素点不同,则设置为透明,已经设为透明的像素点,则不处理,保持透明
// imagePath 需要处理的源图片
// resultPath 处理后的图片, 如果文件不存在,则创建文件,如果存在,就作为上次的结果继续处理
// CROP_CONFIG 源图片裁剪配置,
// tolerance 处理时的容差值

const Jimp = require('jimp');
const fs = require('fs');
const path = require('path');

// 配置路径
// 源图片路径（全屏截图）
const imagePath = path.join(__dirname, 'resource/cache/9a8de478.png');
const resultPath = path.join(__dirname, 'resource/cache/toumingtu.png');
// 源图片裁剪配置,
const CROP_CONFIG = {
    enabled: true, // 是否启用裁剪,让配置生效
    x: 1959, // 裁剪的x坐标
    y: 272, // 裁剪的y坐标
    w: 73, // 裁剪的宽度
    h: 41 // 裁剪的高度
};

let baseImage = null;
let isProcessing = false;
let lastMtime = 0;

// 主函数
async function processImage(tolerance = 0) {
    if (isProcessing) return;
    isProcessing = true;

    try {
        // 检查文件是否存在
        if (!fs.existsSync(imagePath)) {
            console.log('等待图片文件...');
            return;
        }

        // 检查文件修改时间，避免重复处理同一张图片
        const stats = fs.statSync(imagePath);
        if (stats.mtimeMs === lastMtime) {
            // 文件未更新，跳过
            return;
        }

        // 等待文件写入完成（简单的防抖，防止读取到写入一半的文件）
        // 在实际高频写入场景可能需要更复杂的锁机制，这里简单假设读取时文件已就绪

        let currentImage = await Jimp.Jimp.read(imagePath);

        // 如果启用了裁剪，先对图片进行裁剪
        if (CROP_CONFIG.enabled) {
            // console.log(`正在裁剪: x=${CROP_CONFIG.x}, y=${CROP_CONFIG.y}, w=${CROP_CONFIG.w}, h=${CROP_CONFIG.h}`);
            currentImage.crop({
                x: CROP_CONFIG.x,
                y: CROP_CONFIG.y,
                w: CROP_CONFIG.w,
                h: CROP_CONFIG.h
            });
        }

        lastMtime = stats.mtimeMs;

        if (!baseImage) {
            if (fs.existsSync(resultPath)) {
                console.log('发现 aaa_processed.png，将其作为基准图片');
                baseImage = await Jimp.Jimp.read(resultPath);
            } else {
                // 第一张图片，作为基准
                baseImage = currentImage;
                console.log('已初始化基准图片');
                await baseImage.write(resultPath);
            }
        } else {
            // 确保尺寸一致，如果不一致可能需要重置或调整，这里假设尺寸不变
            if (baseImage.width !== currentImage.width || baseImage.height !== currentImage.height) {
                console.log('图片尺寸发生变化，重置基准图片');
                baseImage = currentImage;
                await baseImage.write(resultPath);
                isProcessing = false;
                return;
            }

            let diffCount = 0;
            // 遍历所有像素进行对比
            baseImage.scan(0, 0, baseImage.bitmap.width, baseImage.bitmap.height, function (x, y, idx) {
                // 如果基准图片这个点已经是透明的，就不用比了
                if (this.bitmap.data[idx + 3] === 0) return;

                const r1 = this.bitmap.data[idx + 0];
                const g1 = this.bitmap.data[idx + 1];
                const b1 = this.bitmap.data[idx + 2];
                const a1 = this.bitmap.data[idx + 3];

                const r2 = currentImage.bitmap.data[idx + 0];
                const g2 = currentImage.bitmap.data[idx + 1];
                const b2 = currentImage.bitmap.data[idx + 2];
                const a2 = currentImage.bitmap.data[idx + 3];

                // 对比两个像素是否相同
                const rDiff = Math.abs(r1 - r2);
                const gDiff = Math.abs(g1 - g2);
                const bDiff = Math.abs(b1 - b2);
                const aDiff = Math.abs(a1 - a2);

                if (rDiff > tolerance || gDiff > tolerance || bDiff > tolerance || aDiff > tolerance) {
                    // 不一样，设置为透明
                    this.bitmap.data[idx + 3] = 0;
                    // 颜色也可以清空，不过alpha设为0也就看不见了
                    this.bitmap.data[idx + 0] = 0;
                    this.bitmap.data[idx + 1] = 0;
                    this.bitmap.data[idx + 2] = 0;
                    diffCount++;
                }
            });

            if (diffCount > 0) {
                console.log(`发现 ${diffCount} 个不同像素，已更新结果图片`);
                await baseImage.write(resultPath);
            } else {
                console.log('图片内容一致，无变化');
            }
        }

    } catch (err) {
        console.error('处理出错:', err.message);
    } finally {
        isProcessing = false;
    }
}

// 每秒执行一次
console.log('开始监控图片变化...');
const tolerance = 30; // 设置容差值
setInterval(() => processImage(tolerance), 1000);


