const dm = require('../damo.js')
const { Jimp } = require('jimp');
const cv = require('../opencv.js');
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
        var jimpSrc = await Jimp.read('./94295493.png');
        var src = cv.matFromImageData(jimpSrc.bitmap);

        let gray = new cv.Mat();
        cv.cvtColor(src, gray, cv.COLOR_RGBA2GRAY);

        // let rgba = new cv.Mat();
        // cv.cvtColor(gray, rgba, cv.COLOR_GRAY2RGBA);

        new Jimp({
            width: gray.cols,
            height: gray.rows,
            data: Buffer.from(gray.data)
        })
            .write('output.png');
    
    
        src.delete();
        gray.delete();
        // rgba.delete();
    }
    
}


module.exports = Mhxy

