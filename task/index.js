const TaskBase = require('../core/TaskBase');
const 配置 = require('../resource/index.js')
const { 随机延时 } = require('../tools/tools.js')
class Mhxy extends TaskBase {

    async 回到主界面() {
        // 判断当前是否是主界面,否则不断关闭弹框,直到是主界面
        while (!(await 配置.主界面活动按钮.查找())) {

            await 配置.关闭1.查找并点击()

            await 随机延时(500, 1500)
            return
        }
    }
}

module.exports = Mhxy
