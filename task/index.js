const dm = require('../damo.js')
const { Jimp } = require('jimp');
const cv = require('../opencv.js');
const path = require('path')
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

    bind() {
        const res = dm.reg("xf30557fc317f617eead33dfc8de3bdd4ab9043", "x4lpdhpht2zgnl7")
        if (res == 1) {
            const res2 = dm.bindWindow(this.hwnd, 'dx2', 'windows', 'windows', 0)
            if (res2 != 1) {
                this.changeProp('status', `dm后台绑定失败: ${res}`);
                return false
            }
        } else {
            this.changeProp('status', `dm注册失败: ${res}`);
            return false
        }
        return true;
    }

    // time延时, offsetTime上下浮动
    async sleep(time, offsetTime = 2) {
        // 生成±2秒范围内的随机浮动值
        const offset = (Math.random() * (offsetTime * 2) - offsetTime).toFixed(2); // -2到+2秒之间
        const actualSeconds = Math.max(0, parseFloat(time) + parseFloat(offset));

        // console.log(`基础等待: ${time}s | 实际等待: ${actualSeconds}s (浮动: ${offset}s)`);
        return new Promise(resolve => setTimeout(resolve, actualSeconds * 1000));
    }

    async isImageInScreen(aPath, bPath, threshold = 0.8) {

        try {
            // 使用Jimp加载图像
            const [aImage, bImage] = await Promise.all([
                Jimp.read(path.resolve(__dirname, '../resource', `aaa.bmp`)),
                Jimp.read(path.resolve(__dirname, '../resource', `123.bmp`))
            ]);

            const srcMat = cv.matFromImageData(aImage.bitmap);
            const templMat = cv.matFromImageData(bImage.bitmap);

            // 转换为灰度图
            const srcGray = new cv.Mat();
            const templGray = new cv.Mat();
            cv.cvtColor(srcMat, srcGray, cv.COLOR_RGBA2GRAY);
            cv.cvtColor(templMat, templGray, cv.COLOR_RGBA2GRAY);

            // 创建结果矩阵
            const result = new cv.Mat();
            const method = cv.TM_CCOEFF_NORMED; // 使用归一化互相关系数方法

            // 执行模板匹配
            cv.matchTemplate(srcGray, templGray, result, method);

            // 寻找最大匹配值
            const minMax = cv.minMaxLoc(result);
            const maxValue = minMax.maxVal;
            const maxLoc = minMax.maxLoc;

            // 释放内存
            srcMat.delete();
            templMat.delete();
            srcGray.delete();
            templGray.delete();
            result.delete();

            const isFound = maxValue >= threshold;

            return {
                found: isFound,
                confidence: maxValue,
                location: isFound ? {
                    x: maxLoc.x,
                    y: maxLoc.y,
                    width: bImage.bitmap.width,
                    height: bImage.bitmap.height
                } : null,
                threshold: threshold
            };
        } catch (error) {
            console.error('图像处理出错:', error);
            throw error;
        }
    }



    async isImageInScreen(aPath, bPath, threshold = 0.8) {
        // bPath为小图,也就是事先准备的模板图
        // aPath为大图,也就是实时截图
        try {
            // 使用Jimp加载图像
            const [aImage, bImage] = await Promise.all([
                Jimp.read(path.resolve(__dirname, '../resource', `aaa.bmp`)),
                Jimp.read(path.resolve(__dirname, '../resource', `123.bmp`))
            ]);
    
            const srcMat = cv.matFromImageData(aImage.bitmap);
            const templMat = cv.matFromImageData(bImage.bitmap);
    
            // 转换为灰度图
            const srcGray = new cv.Mat();
            const templGray = new cv.Mat();
            cv.cvtColor(srcMat, srcGray, cv.COLOR_RGBA2GRAY);
            cv.cvtColor(templMat, templGray, cv.COLOR_RGBA2GRAY);
    
            // 检查模板图的四个角颜色是否相同，如果相同则创建mask
            let mask = null;
            const corners = this.getImageCorners(bImage);
            const hasTransparentColor = this.hasSameCornerColors(corners);
            
            if (hasTransparentColor) {
                const transparentColor = corners[0]; // 任意一个角都可以，因为颜色相同
                mask = this.createTransparencyMask(templMat, transparentColor);
            }
    
            // 创建结果矩阵
            const result = new cv.Mat();
            const method = cv.TM_CCOEFF_NORMED; // 使用归一化互相关系数方法
    
            // 执行模板匹配（如果有mask则使用mask）
            if (mask) {
                cv.matchTemplate(srcGray, templGray, result, method, mask);
            } else {
                cv.matchTemplate(srcGray, templGray, result, method);
            }
    
            // 寻找最大匹配值
            const minMax = cv.minMaxLoc(result);
            const maxValue = minMax.maxVal;
            const maxLoc = minMax.maxLoc;
    
            // 释放内存
            srcMat.delete();
            templMat.delete();
            srcGray.delete();
            templGray.delete();
            result.delete();
            if (mask) {
                mask.delete();
            }
    
            const isFound = maxValue >= threshold;
    
            return {
                found: isFound,
                confidence: maxValue,
                location: isFound ? {
                    x: maxLoc.x,
                    y: maxLoc.y,
                    width: bImage.bitmap.width,
                    height: bImage.bitmap.height
                } : null,
                threshold: threshold,
                hasTransparentColor: hasTransparentColor
            };
        } catch (error) {
            console.error('图像处理出错:', error);
            throw error;
        }
    }


    // 获取图片四个角的颜色
    getImageCorners(image) {
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
    hasSameCornerColors(corners) {
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
    createTransparencyMask(templMat, transparentColor) {
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

}









module.exports = Mhxy

