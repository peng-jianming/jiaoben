const TaskBase = require('../core/TaskBase');
const 配置 = require('../resource/index.js')

class Mhxy extends TaskBase {

    async 回到主界面() {
        return true || false
    }

    async 打开活动界面() {
        // 点击活动
        const isFind = await 配置.主界面活动按钮.查找并点击({
            startMs: 1000,
            endMs: 3000
        })

        if (!isFind) {
            return false
        }

        // 出现活动界面,
        const isFind2 = await 配置.活动界面.查找()
        return !!isFind2
    }

    async 战斗界面处理() {

    }

}

module.exports = Mhxy
