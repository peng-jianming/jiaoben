const Mhxy = require('./index')
const 配置 = require('./config.js')

class Demo extends Mhxy {
    async start() {
        // 可以通过 this.params 获取任务参数
        // 例如: this.params.target 或 this.params.interval
        
        const result = await this.多点关联颜色匹配(配置.aaaa)
        if (result) {
            await this.ADB左键点击(result)
        }
        // await this.ADB滑动({ x: 527, y: 1000 }, { x: 527, y: 500 })
    }
}

module.exports = Demo







