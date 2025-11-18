const Mhxy = require('./index')

const StateMachine = require('../tools/stateMachine2.js')

const 配置 = require('./config.js')

class Shimen extends Mhxy {
    constructor(hwnd, changeProp) {
        super(hwnd, changeProp)
    }
    async start() {
        console.log('222222277');
        this.changeProp('dddd', "22222212")
        await this.延时(2000)
        // new StateMachine(() => {

        // })
        //     .on('回到主界面', () => {})
        //     .on('打开活动界面', () => {
        //         // 点击活动
        //         // 判断是否出现活动界面,
        //         // 有则进入下一步,没有则,回到主界面
        //     })
        //     .on('打开师门界面', () => {
        //         // 点击师门
        //         // 判断是否出现师门界面
        //         // 有则进入下一步,没有则,回到主界面
        //     })
        //     .on('接取师门任务', () => {
        //         // 判断师门任务是否完成
        //         // 完成 -> 结束
        //         // 未完成 -> 接取师门任务, 进入下一状态
        //     })
        //     .on('接取师门任务', () => {
        //         new StateMachine(() => {
        //             if(this.findImage('TP\师门\跳过对话.bmp')) {
        //                 return '跳过对话'
        //             }
        //             if(this.findImage('TP\师门\购买物品.bmp')) {
        //                return '购买物品' 
        //             }
        //             if(this.findImage('TP\师门\师门界面.bmp')) {
        //                 return '师门界面'
        //             }
        //         })
        //         .on('跳过对话', () => {
        //             // 点击跳过对话
        //         })
        //         .on('购买物品', () => {
        //             // 点击购买物品
        //         })
        //         .on('师门界面', () => {
        //             // 判断师门任务是否完成
        //             // 完成 -> 结束
        //         })
        //         .start()

        //     })
        //     .start()
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







