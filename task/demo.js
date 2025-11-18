const Mhxy = require('./index')
const 配置 = require('./config.js')
const lihuo = require('../tools/lihuo')
const path = require('path')

const { getScreen, 屏幕控制, 调用ADB } = require('../touping.js')
class Demo extends Mhxy {
    async start() {
        // 可以通过 this.params 获取任务参数
        // 例如: this.params.target 或 this.params.interval

        // const result = await this.多点关联颜色匹配(配置.aaaa)
        // if (result) {
        //     await this.ADB左键点击(result)
        // }
        let i = 0
        while (this.flag) {
            
            // await this.ADB左键点击({ x: 972, y: 747 })
            const url = await getScreen(this.hwnd)
            const point = lihuo.lhFindPicMateFile(url,  path.resolve(__dirname, '../resource', `1111.png`))
            if(point) {
                await this.ADB左键点击(point)
                this.changeProp('action', '点击学习报告')
            } else {
                this.changeProp('action', '未找到学习报告')
            }
    
            await this.延时(2000)
            // awaitnew Promise(resolve => setTimeout(resolve, 2000))
        }
        // await this.ADB滑动({ x: 527, y: 1000 }, { x: 527, y: 500 })
    }
}

module.exports = Demo







