const dm = require('../damo.js')

class Mhxy {
    flag = true
    hwnd = 0
    constructor(hwnd, changeProp) {
        this.hwnd = hwnd
        this.changeProp = changeProp;
        const res = dm.reg("xf30557fc317f617eead33dfc8de3bdd4ab9043", "x4lpdhpht2zgnl7")
        if (res == 1) {
            const res2 = dm.bindWindow(hwnd, 'dx2', 'windows', 'windows', 0)
            if (res2 != 1) {
                console.log('dm后台绑定失败', res);
            }
        } else {
            console.log('dm注册失败', res);
        }
    }
    stop() {
        this.flag = false;
    }
    start() { }

    // time延时, offsetTime上下浮动
    async sleep(time, offsetTime = 2) {
        // 生成±2秒范围内的随机浮动值
        const offset = (Math.random() * (offsetTime * 2) - offsetTime).toFixed(2); // -2到+2秒之间
        const actualSeconds = Math.max(0, parseFloat(time) + parseFloat(offset));

        // console.log(`基础等待: ${time}s | 实际等待: ${actualSeconds}s (浮动: ${offset}s)`);
        return new Promise(resolve => setTimeout(resolve, actualSeconds * 1000));
    }
}


module.exports = Mhxy

