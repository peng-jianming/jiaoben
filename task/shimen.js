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
            .on('打开师门界面', async () => {
                // 点击活动界面师门参加按钮
                const ponit = await 配置.活动界面师门任务.查找()

                if (!ponit) {
                    return '回到主界面'
                }

                await 配置.活动界面参加按钮.设置查找区域({ x: ponit.x, y: ponit.y, width: 450, height: 130 }).查找并点击({
                    startMs: 1000,
                    endMs: 1500
                })

                const isFind = await 配置.师门界面.查找()

                if (isFind) {
                    return '接取师门任务'
                } else {
                    return '回到主界面'
                }
            })
            .on('接取师门任务', async () => {
                // 判断师门任务是否完成
                if (await 配置.师门界面继续任务按钮.查找并点击()) {
                    return '做师门任务'
                }

                if (await 配置.师门界面去完成按钮.查找并点击()) {
                    return '做师门任务'
                }

                if (await 配置.师门界面选择按钮.查找并点击()) {
                    return '做师门任务'
                }

                await 配置.关闭师门弹框.查找并点击();

                global.changeProp('action', '师门任务已经完成')
                sta.stop()

            })
            .on('做师门任务', async () => {

                const sta1 = new StateMachine(() => { })
                    .on('1', async () => await 配置.跳过.查找并点击({
                        x: 300,
                        y: 300,
                        w: 2000,
                        h: 980
                    }) ? '1' : '2')
                    .on('2', async () => await 配置.使用.查找并点击() ? '1' : '3')
                    .on('3', async () => await 配置.上交.查找并点击() ? '1' : '4')
                    .on('4', async () => await 配置.弹框_购买.查找并点击() ? '1' : '5')
                    .on('5', async () => await 配置.摆摊弹框_购买.查找并点击() ? '1' : '6')
                    .on('6', async () => await 配置.对话选项框.查找并点击({
                        isOffset: true,
                        x: 40,
                        y: 120,
                        w: 400,
                        h: 60
                    }) ? '1' : '7')
                    .on('7', async () => await 配置.对话说话框.查找并点击({
                        x: 300,
                        y: 300,
                        w: 1800,
                        h: 600
                    }) ? '1' : '8')
                    .on('8', async () => await 配置.主界面_师门文字.查找并点击({
                        isOffset: true,
                        x: 0,
                        y: 0,
                        w: 336,
                        h: 121
                    }) ? '1' : '9')
                    .on('9', async () => {
                        if (await 配置.师门_任务完成弹框.查找并点击({
                            x: 1038,
                            y: 802,
                            w: 323,
                            h: 68,
                            startMs: 1000,
                            endMs: 1500
                        })) {
                            await 配置.关闭师门弹框.查找并点击();
                            sta1.stop();
                            sta.stop();
                        }
                        return '1'
                    })

                await sta1.start('1')
            })

        await sta.start('回到主界面')


    }
}

// 回到主界面
// 主界面 -> 点击活动
// 活动界面 -> 点击师门
// 师门界面 -> 判断师门任务是否完成, 完成则结束, 未完成则点击师门任务,开始任务

// 主界面(界面不确定) -> 随机不断循环, 直到出现师门界面, 判断师门任务是否完成, 完成则结束
// 如果任务未完成, 则继续循环


//这其中需要判断是否卡住, 如果卡住则直接返回主界面,重新开始


module.exports = Shimen
