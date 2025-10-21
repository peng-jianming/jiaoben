const { 多点关联颜色匹配, 多点颜色匹配 } = require('../tools/colorMatching.js')
const { 图片匹配 } = require('../tools/imageMatching.js')
const { getScreen } = require('../touping.js')


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

    async 延时(time, offsetTime = 2) {
        // 生成±2秒范围内的随机浮动值
        const offset = (Math.random() * (offsetTime * 2) - offsetTime).toFixed(2); // -2到+2秒之间
        const actualSeconds = Math.max(0, parseFloat(time) + parseFloat(offset));

        return new Promise(resolve => setTimeout(resolve, actualSeconds * 1000));
    }

    // 返回找到的坐标{ x: 0, y: 0 }, 没找到返回null
    async 多点关联颜色匹配(信息) {
        const url = await getScreen(this.hwnd)
        return 多点关联颜色匹配(信息.特征, url, 信息.相似度, 信息.区域)
    }

    // 返回true or false
    async 多点颜色匹配(信息) {
        const url = await getScreen(this.hwnd)
        return 多点颜色匹配(信息.特征, url, 信息.相似度, 信息.区域)
    }
}

module.exports = Mhxy


