const Mhxy = require('./index')
const StateMachine = require('../tools/stateMachine2.js')

const 配置 = require('../resource/index.js')

class Shimen extends Mhxy {
    async start() {
        const sta = new StateMachine(() => {
            // 操作 + 结果 + 下一状态
        })
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

                const ponit1 = await 配置.活动界面参加按钮.设置查找区域({ x1: ponit.x, y1: ponit.y, x2: ponit.x + 442, y2: ponit.y + 123 }).查找并点击()

                await this.随机延时(1000, 3000)

                // 出现师门界面
                const point2 = await 配置.师门界面.查找()
                if (point2) {
                    return '接取师门任务'
                } else {
                    return '回到主界面'
                }
            })
            .on('接取师门任务', async () => {

                // 判断师门任务是否完成
                const point = await 配置.师门界面继续任务按钮.查找并点击();
                if (point) {
                    return '做师门任务'
                }

                const point1 = await 配置.师门界面去完成按钮.查找并点击();
                if (point1) {
                    return '做师门任务'
                }

                global.changeProp('action', '师门任务已经完成')
                sta.stop()

            })
            .on('做师门任务', () => {
                console.log('正在做师门任务');
                
                // new StateMachine(() => {
                //     if(this.findImage('TP\师门\跳过对话.bmp')) {
                //         return '跳过对话'
                //     }
                //     if(this.findImage('TP\师门\购买物品.bmp')) {
                //        return '购买物品' 
                //     }
                //     if(this.findImage('TP\师门\师门界面.bmp')) {
                //         return '师门界面'
                //     }
                // })
                // .on('跳过对话', () => {
                //     // 点击跳过对话
                // })
                // .on('购买物品', () => {
                //     // 点击购买物品
                // })
                // .on('师门界面', () => {
                //     // 判断师门任务是否完成
                //     // 完成 -> 结束
                // })
                // .start()

            })

        sta.currentState = '回到主界面'
        await sta.start(this.随机区间时间(1000, 3000))

        console.log('222222222222222222222222');

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







