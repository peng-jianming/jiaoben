const Mhxy = require('./index')
const StateMachine = require('../tools/stateMachine2.js')
const 配置 = require('../resource/index.js')
const lihuo = require('../tools/lihuo/index.js')
class Shimen extends Mhxy {
    async start() {
        const sta = new StateMachine(() => { })
            .on('回到主界面', async () => {
                this.回到主界面();
                return '打开活动界面'
            })
            .on('打开活动界面', async () => {

                await 配置.主界面活动按钮.查找并点击({
                    startMs: 1000,
                    endMs: 1500
                })

                const result = await 配置.活动界面.查找()
                if (result) {
                    return '打开师门界面'
                } else {
                    return '回到主界面'
                }
            })
            .on('打开活动界面', async () => {

                await 配置.主界面活动按钮.查找并点击({
                    startMs: 1000,
                    endMs: 1500
                })

                const result = await 配置.活动界面.查找()
                if (result) {
                    return '开启组队'
                } else {
                    return '回到主界面'
                }

                // 点击参加
                // 间隔2秒
                // 判断是否出现组队页面,进行组队,组队完成

                // 没有出现组队,就等待钟馗的便捷组队,组队完成,
                
                // 组队完成,点击参加任务,直接到钟馗处,点击抓鬼任务开始


            })
            .on('开启组队', async () => {

                await 配置.创建队伍.查找并点击({
                    startMs: 1000,
                    endMs: 1500
                })

                if (result) {
                    return '打开师门界面'
                } else {
                    return '回到主界面'
                }
            })

        await sta.start('回到主界面')


    }
}

module.exports = Shimen
