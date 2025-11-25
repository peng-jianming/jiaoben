const Mhxy = require('./index')
const StateMachine = require('../tools/stateMachine2.js')
const 配置 = require('../resource/index.js')

class Shimen extends Mhxy {
    async start() {
        // const ret = lihuo.reg('pengjianming07da20d304e552776cf6a1c9f7eebb5a')
        // console.log('注册结果:', ret);

        // setInterval(async () => {
        //     const screen = await getScreen(global.hwnd)
        //     console.log(screen);
        // }, 1000);



        const sta = new StateMachine(() => { })
            .on('回到主界面', async () => {
                // TODO 返回主页面操作
                console.log('在主界面');
                await this.随机延时(1000, 3000)
                return '打开活动界面'
            })
            .on('打开活动界面', async () => {
                const result = await this.打开活动界面()
                if (result) {
                    return '打开师门界面'
                } else {
                    return '回到主界面'
                }
            })
            .on('打开师门界面', async () => {
                // 点击活动界面师门参加按钮
                const ponit = await 配置.活动界面师门任务.查找()

                await 配置.活动界面参加按钮.设置查找区域({ x1: ponit.x, y1: ponit.y, x2: ponit.x + 442, y2: ponit.y + 123 }).查找并点击({
                    startMs: 1000,
                    endMs: 3000
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

                global.changeProp('action', '师门任务已经完成')
                sta.stop()

            })
            .on('做师门任务', async () => {
                let i = 0
                while (true) {
                    if (i <= 0) {
                        await 配置.主界面_师门文字.查找并点击({
                            isOffset: true,
                            x: 0,
                            y: 0,
                            w: 336,
                            h: 121
                        })
                        i = 3
                    } else {
                        await 配置.跳过.查找并点击({
                            x: 300,
                            y: 300,
                            w: 2000,
                            h: 980
                        })
                        await 配置.使用.查找并点击()
                        await 配置.上交.查找并点击()
                        await 配置.对话_师门任务按钮.查找并点击()
                        await 配置.主界面_师门集物.查找并点击()
                        await 配置.弹框_购买.查找并点击()
                        await 配置.摆摊弹框_购买.查找并点击()
                        await 配置.对话_师门寻趣按钮.查找并点击({
                            isOffset: true,
                            x: 0,
                            y: 0,
                            w: 415,
                            h: 68
                        })
                        await 配置.主界面_查看门派关系按钮.查找并点击({
                            isOffset: true,
                            x: 0,
                            y: -102,
                            w: 358,
                            h: - 102 + 65
                        })
                    }
                    i--
                }
            })

        sta.currentState = '回到主界面'
        await sta.start(this.随机区间时间(1000, 3000))

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







// 点击文字

// 遍历其他,然后进行任务操作,

// 有进行任务操作,才继续点击文字,避免多次点击文字

// 需要加入随机错误点击, 不能全是有效点击