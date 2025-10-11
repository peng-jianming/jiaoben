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

    async 二值化() {
        var jimpSrc = await Jimp.read(path.resolve(__dirname, '../resource/cache', `${this.hwnd}.png`));

        var src = cv.matFromImageData(jimpSrc.bitmap);

        let gray = new cv.Mat();
        cv.cvtColor(src, gray, cv.COLOR_RGBA2GRAY);

        let rgba = new cv.Mat();
        cv.cvtColor(gray, rgba, cv.COLOR_GRAY2RGBA);

        new Jimp({
            width: rgba.cols,
            height: rgba.rows,
            data: Buffer.from(rgba.data)
        })
            .write('output.png');


        src.delete();
        gray.delete();
        rgba.delete();
    }

    async isImageInScreen(aPath, bPath, threshold = 0.8) {

        try {
            // 使用Jimp加载图像
            const [aImage, bImage] = await Promise.all([
                Jimp.read(path.resolve(__dirname, '../resource', `ddd.png`)),
                Jimp.read(path.resolve(__dirname, '../resource', `ggg.png`))
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
}




module.exports = Mhxy

