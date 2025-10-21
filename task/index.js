const { 多点关联颜色匹配, 多点颜色匹配 } = require('../tools/colorMatching.js')
const { 图片匹配 } = require('../tools/imageMatching.js')
const { getScreen, 屏幕控制 } = require('../touping.js')
const 配置 = require('./config.js')

class Mhxy {
    width = 2400
    height = 1080
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

    async 左键点击(result) {
        if(result) {
            await 屏幕控制(this.hwnd, '0', String(result.x/this.width), String(result.y/this.height))
            await 屏幕控制(this.hwnd, '2', String(result.x/this.width), String(result.y/this.height))
        } else {
            console.log('左键点击坐标为空');
        }
    }

    async 滑动(result1, result2) {
        if(result1 && result2) {
            await 屏幕控制(this.hwnd, '0', String(result1.x/this.width), String(result1.y/this.height))
            await 屏幕控制(this.hwnd, '1', String(result2.x/this.width), String(result2.y/this.height))
            await 屏幕控制(this.hwnd, '2', String(result2.x/this.width), String(result2.y/this.height))
        } else {
            console.log('左键点击坐标为空');
        }
    }

    async 打开活动弹框() {
        const res1 = await this.多点关联颜色匹配(配置.活动按钮);
        if(res1) {
            console.log('打开活动弹框')
            await this.左键点击(res1);
            await this.延时(2, 0)  
        }

        const res2 = await this.多点颜色匹配(配置.活动界面);
        if(res2) {
            console.log('处于活动弹框')
            const res3 = await this.多点颜色匹配(配置.日常活动激活状态);
            if(res3) {
                console.log('活动已归位')
            } else {
                console.log('归位活动')  
                await this.左键点击({x: 574, y: 171});
            }
        } else {
            this.打开活动弹框();
        }
    }
}

module.exports = Mhxy


