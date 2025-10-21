const Jimp = require('jimp');
const cv = require('../opencv.js');

const 获取图片四个角的颜色 = (image) => {
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

const 检查四个角颜色是否相同 = (corners) => {
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


const 创建透明色掩码 = (templMat, transparentColor) => {
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



const 图片匹配 = async (aPath, bPaths, threshold = 0.8) => {
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
                    // console.warn(`模板图片 ${bPath} 尺寸大于原图，跳过匹配`);
                    continue;
                }

                const corners = 获取图片四个角的颜色(bImage);
                const hasTransparentColor = 检查四个角颜色是否相同(corners);

                if (hasTransparentColor) {
                    const transparentColor = corners[0];
                    mask = await 创建透明色掩码(templMat, transparentColor, false);
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
                    // console.warn(`检测到异常匹配值: ${maxValue}，将其设置为0`);
                    maxValue = 0;
                }

                // 确保匹配值在合理范围内 [0, 1]
                maxValue = Math.max(0, Math.min(1, maxValue));

                result.delete();
                if (mask) {
                    mask.delete();
                }

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

module.exports = {
    图片匹配
}