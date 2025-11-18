const Mhxy = require('./index')

const StateMachine = require('../tools/stateMachine2.js')
const { getScreen, 屏幕控制, 调用ADB } = require('../touping.js')
const lihuo = require('../tools/lihuo')
const 配置 = require('./config.js')
const path = require('path');

class Shimen extends Mhxy {
    constructor(hwnd, changeProp) {
        super(hwnd, changeProp)
    }
    async start() {

        const sta = new StateMachine(() => {
            console.log('1111111111111');

        })
            .on('回到主界面', async () => {
                console.log('在主界面');
                await this.延时(2000)
                sta.currentState = '打开活动界面'
            })
            .on('打开活动界面', async () => {
                // 点击活动
                let url = await getScreen(this.hwnd)
                const point = lihuo.lhFindPicMateFile(url, path.resolve(__dirname, '../resource', `huodong.png`))
                if (point) {
                    await this.ADB左键点击(point)
                    this.changeProp('action', '点击活动按钮')
                } else {
                    this.changeProp('action', '未找到活动按钮')
                    sta.currentState = '回到主界面'
                    return
                }

                await this.延时(this.随机区间时间(1000, 3000))

                // 出现活动界面,
                let url1 = await getScreen(this.hwnd)
                const point1 = lihuo.lhFindPicMateFile(url1, path.resolve(__dirname, '../resource', `huodongjiemian.png`))
                if (point1) {
                    this.changeProp('action', '处于活动界面')
                    sta.currentState = '打开师门界面'
                    return
                } else {
                    this.changeProp('action', '未处于活动界面')
                    sta.currentState = '回到主界面'
                    return
                }
            })
            .on('打开师门界面', async () => {
                // 点击活动界面师门参加按钮
                this.changeProp('action', '点击活动界面师门参加按钮')
                await this.ADB左键点击({ x: 1858, y: 222 })

                await this.延时(this.随机区间时间(1000, 3000))
       
                // 出现师门界面
                let url1 = await getScreen(this.hwnd)
                const point1 = lihuo.lhFindPicMateFile(url1, path.resolve(__dirname, '../resource', `shimenjiemian.png`))
                if (point1) {
                    this.changeProp('action', '处于师门界面')
                    sta.currentState = '接取师门任务'
                    return
                } else {
                    this.changeProp('action', '未处于师门界面')
                    sta.currentState = '回到主界面'
                    return
                }
            })
            .on('接取师门任务', async () => {
                
                // 判断师门任务是否完成
                let url = await getScreen(this.hwnd)
                const point = lihuo.lhFindPicMateFile(url, path.resolve(__dirname, '../resource', `shimenjixurenwu.png`))
                if (point) {
                    this.changeProp('action', '点击师门界面继续完成按钮')
                    await this.ADB左键点击(point)
                    sta.currentState = '做师门任务'
                    return
                }

                const point1 = lihuo.lhFindPicMateFile(url, path.resolve(__dirname, '../resource', `shimenquwancheng.png`))
                if (point1) {
                    this.changeProp('action', '点击师门界面去完成按钮')
                    await this.ADB左键点击(point1)
                    sta.currentState = '做师门任务'
                    return
                } else {
                    this.changeProp('action', '师门任务已经完成')
                    sta.stop()
                    return
                }
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
        sta.start(1000)
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







