// const dm = require('../damo.js')
const Jimp = require('jimp');
const cv = require('../opencv.js');
const path = require('path')


// 获取图片四个角的颜色
const getImageCorners = (image) => {
    const width = image.bitmap.width;
    const height = image.bitmap.height;

    return [
        { x: 0, y: 0 },                    // 左上角
        { x: width - 1, y: 0 },           // 右上角
        { x: 0, y: height - 1 },          // 左下角
        { x: width - 1, y: height - 1 }   // 右下角
    ].map(corner => {
        const color = Jimp.intToRGBA(image.getPixelColor(corner.x, corner.y));
        return {
            x: corner.x,
            y: corner.y,
            r: color.r,
            g: color.g,
            b: color.b,
            a: color.a
        };
    });
}

// 检查四个角颜色是否相同
const hasSameCornerColors = (corners) => {
    if (corners.length < 4) return false;

    const firstCorner = corners[0];
    for (let i = 1; i < corners.length; i++) {
        const corner = corners[i];
        if (corner.r !== firstCorner.r ||
            corner.g !== firstCorner.g ||
            corner.b !== firstCorner.b ||
            corner.a !== firstCorner.a) {
            return false;
        }
    }
    return true;
}

// 创建透明色掩码
const createTransparencyMask = (templMat, transparentColor) => {
    const mask = new cv.Mat(templMat.rows, templMat.cols, cv.CV_8UC1);

    // 将透明色区域设为0，其他区域设为255
    for (let i = 0; i < templMat.rows; i++) {
        for (let j = 0; j < templMat.cols; j++) {
            const pixel = templMat.ptr(i, j);
            if (pixel[0] === transparentColor.b && // OpenCV使用BGR格式
                pixel[1] === transparentColor.g &&
                pixel[2] === transparentColor.r &&
                pixel[3] === transparentColor.a) {
                mask.ucharPtr(i, j)[0] = 0; // 透明色区域不参与匹配
            } else {
                mask.ucharPtr(i, j)[0] = 255; // 非透明色区域参与匹配
            }
        }
    }


    return mask;
}
class Mhxy {
    flag = true
    hwnd = 0
    constructor(hwnd, changeProp) {
        this.hwnd = hwnd
        this.changeProp = changeProp;
    }
    stop() {
        this.flag = false;
    }
    start() { }

    // bind() {
    //     const res = dm.reg("xf30557fc317f617eead33dfc8de3bdd4ab9043", "x4lpdhpht2zgnl7")
    //     if (res == 1) {
    //         const res2 = dm.bindWindow(this.hwnd, 'dx2', 'windows', 'windows', 0)
    //         if (res2 != 1) {
    //             this.changeProp('status', `dm后台绑定失败: ${res}`);
    //             return false
    //         }
    //     } else {
    //         this.changeProp('status', `dm注册失败: ${res}`);
    //         return false
    //     }
    //     return true;
    // }

    // time延时, offsetTime上下浮动
    async sleep(time, offsetTime = 2) {
        // 生成±2秒范围内的随机浮动值
        const offset = (Math.random() * (offsetTime * 2) - offsetTime).toFixed(2); // -2到+2秒之间
        const actualSeconds = Math.max(0, parseFloat(time) + parseFloat(offset));

        // console.log(`基础等待: ${time}s | 实际等待: ${actualSeconds}s (浮动: ${offset}s)`);
        return new Promise(resolve => setTimeout(resolve, actualSeconds * 1000));
    }

    async findPic(aPath, bPaths, threshold = 0.8) {
        const templatePaths = Array.isArray(bPaths) ? bPaths : [bPaths];
    
        try {
            const aImage = await Jimp.Jimp.read(aPath);
            const srcMat = cv.matFromImageData(aImage.bitmap);
            const srcGray = new cv.Mat();
            cv.cvtColor(srcMat, srcGray, cv.COLOR_RGBA2GRAY);
    
            for (let i = 0; i < templatePaths.length; i++) {
                const bPath = templatePaths[i];
                let bImage = null;
                let templMat = null;
                let templGray = null;
                let mask = null;
    
                try {
                    bImage = await Jimp.Jimp.read(bPath);
                    templMat = cv.matFromImageData(bImage.bitmap);
                    templGray = new cv.Mat();
                    cv.cvtColor(templMat, templGray, cv.COLOR_RGBA2GRAY);
    
                    // 检查图像尺寸，避免模板图比原图大
                    if (templGray.rows > srcGray.rows || templGray.cols > srcGray.cols) {
                        console.warn(`模板图片 ${bPath} 尺寸大于原图，跳过匹配`);
                        continue;
                    }
    
                    const corners = getImageCorners(bImage);
                    const hasTransparentColor = hasSameCornerColors(corners);
    
                    if (hasTransparentColor) {
                        const transparentColor = corners[0];
                        mask = await createTransparencyMask(templMat, transparentColor, false);
                    }
    
                    const result = new cv.Mat();
                    const method = cv.TM_CCOEFF_NORMED;
    
                    if (mask) {
                        cv.matchTemplate(srcGray, templGray, result, method, mask);
                    } else {
                        cv.matchTemplate(srcGray, templGray, result, method);
                    }
    
                    const minMax = cv.minMaxLoc(result);
                    let maxValue = minMax.maxVal;
    
                    // 关键修复：检查并处理异常数值
                    if (!isFinite(maxValue)) {
                        console.warn(`检测到异常匹配值: ${maxValue}，将其设置为0`);
                        maxValue = 0;
                    }
    
                    // 确保匹配值在合理范围内 [0, 1]
                    maxValue = Math.max(0, Math.min(1, maxValue));
    
                    result.delete();
                    if (mask) {
                        mask.delete();
                    }
                    console.log(maxValue, '000000');
                    
                    if (maxValue >= threshold) {
                        srcMat.delete();
                        srcGray.delete();
    
                        return {
                            found: true,
                            confidence: maxValue,
                            location: {
                                x: maxLoc.x,
                                y: maxLoc.y,
                                width: bImage.bitmap.width,
                                height: bImage.bitmap.height
                            },
                            index: i,
                        };
                    }
                } catch (error) {
                    console.error(`处理模板图片 ${bPath} 时出错:`, error);
                } finally {
                    if (templMat) templMat.delete();
                    if (templGray) templGray.delete();
                }
            }
    
            srcMat.delete();
            srcGray.delete();
    
            return {
                found: false,
                confidence: 0,
                location: null,
                index: -1,
            };
        } catch (error) {
            console.error('图像处理出错:', error);
            throw error;
        }
    }


    /**
     * 使用Jimp进行高效颜色匹配（Node.js环境）
     * @param {Array} coordArray - 坐标像素数组
     * @param {string|Buffer} image - 图片路径或Buffer数据
     * @returns {Promise<boolean>}
     */
    async findMultiColor(coordArray, image) {
        let jimpImage = null;

        try {
            // 使用Jimp加载图片
            jimpImage = await Jimp.read(image);

            // 获取图片尺寸
            const width = jimpImage.bitmap.width;
            const height = jimpImage.bitmap.height;

            // 检查所有坐标是否在范围内
            for (const coord of coordArray) {
                if (coord.x >= width || coord.y >= height || coord.x < 0 || coord.y < 0) {
                    console.warn(`坐标点 (${coord.x}, ${coord.y}) 超出图片范围 (${width}x${height})`);
                    return false;
                }
            }

            // 检查每个坐标点的颜色
            for (const coord of coordArray) {
                // 获取像素颜色（Jimp返回的是十六进制数字）
                const pixelColor = jimpImage.getPixelColor(coord.x, coord.y);

                // 将颜色转换为十六进制字符串（Jimp返回的是ARGB格式）
                const rgba = Jimp.intToRGBA(pixelColor);
                const actualColor = '#' +
                    rgba.r.toString(16).padStart(2, '0') +
                    rgba.g.toString(16).padStart(2, '0') +
                    rgba.b.toString(16).padStart(2, '0');

                // 比较颜色（忽略大小写）
                if (actualColor.toLowerCase() !== coord.color.toLowerCase()) {
                    console.log(`坐标 (${coord.x}, ${coord.y}) 颜色不匹配: 期望 ${coord.color}, 实际 ${actualColor}`);
                    return false;
                }
            }

            return true;

        } catch (error) {
            console.error('Jimp处理错误:', error);
            return false;
        }
    }
}

module.exports = Mhxy

