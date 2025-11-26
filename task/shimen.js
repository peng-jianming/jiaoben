const Mhxy = require('./index')
const StateMachine = require('../tools/stateMachine2.js')
const 配置 = require('../resource/index.js')
const lihuo = require('../tools/lihuo/index.js')
const path = require('path')
const winax = require('../tools/lihuo/winax')
class Shimen extends Mhxy {
    async start() {
        const ret = lihuo.reg('pengjianming07da20d304e552776cf6a1c9f7eebb5a')
        console.log('注册结果:', ret);

        // setInterval(async () => {
        //     const screen = await getScreen(global.hwnd)
        //     console.log(screen);
        // }, 1000);
        const aaa = lihuo.dll.Yolov8SetModelPath(path.join(__dirname, 'model'))
        const bbb = lihuo.dll.Yolov8SetParam(1, 'model.lh', '', 1, -1);
        const ccc = lihuo.dll.Yolov8InitModel('1')
        const ret1 = new winax.Variant(-1, 'byref')
        const ret2 = new winax.Variant(-1, 'byref')
        const ddd = lihuo.dll.Yolov8DetectFile('1', path.join(__dirname, '../resource/cache/ggg.png'), 640, 640, 0.5, 1, ret1, ret2)
        console.log(ddd, ret1.toString(), ret2.toString(), "===");






















        // const sta = new StateMachine(() => { })
        //     .on('回到主界面', async () => {
        //         this.回到主界面();
        //         return '打开活动界面'
        //     })
        //     .on('打开活动界面', async () => {

        //         await 配置.主界面活动按钮.查找并点击({
        //             startMs: 1000,
        //             endMs: 1500
        //         })

        //         const result = await 配置.活动界面.查找()
        //         if (result) {
        //             return '打开师门界面'
        //         } else {
        //             return '回到主界面'
        //         }
        //     })
        //     .on('打开师门界面', async () => {
        //         // 点击活动界面师门参加按钮
        //         const ponit = await 配置.活动界面师门任务.查找()

        //         await 配置.活动界面参加按钮.设置查找区域({ x: ponit.x, y: ponit.y, width: 442, height: 123 }).查找并点击({
        //             startMs: 1000,
        //             endMs: 1500
        //         })

        //         const isFind = await 配置.师门界面.查找()

        //         if (isFind) {
        //             return '接取师门任务'
        //         } else {
        //             return '回到主界面'
        //         }
        //     })
        //     .on('接取师门任务', async () => {
        //         // 判断师门任务是否完成
        //         if (await 配置.师门界面继续任务按钮.查找并点击()) {
        //             return '做师门任务'
        //         }

        //         if (await 配置.师门界面去完成按钮.查找并点击()) {
        //             return '做师门任务'
        //         }

        //         if (await 配置.师门界面选择按钮.查找并点击()) {
        //             return '做师门任务'
        //         }

        //         global.changeProp('action', '师门任务已经完成')
        //         sta.stop()

        //     })
        //     .on('做师门任务', async () => {
        //         let i = 0
        //         while (true) {
        //             if (i <= 0) {
        //                 await 配置.主界面_师门文字.查找并点击({
        //                     isOffset: true,
        //                     x: 0,
        //                     y: 0,
        //                     w: 336,
        //                     h: 121
        //                 })
        //                 i = 3
        //             } else {
        //                 await 配置.跳过.查找并点击({
        //                     x: 300,
        //                     y: 300,
        //                     w: 2000,
        //                     h: 980
        //                 })
        //                 await 配置.使用.查找并点击()
        //                 await 配置.上交.查找并点击()
        //                 await 配置.主界面_师门集物.查找并点击()
        //                 await 配置.弹框_购买.查找并点击()
        //                 await 配置.摆摊弹框_购买.查找并点击()

        //                 await 配置.对话选项框.查找并点击({
        //                     isOffset: true,
        //                     x: 40,
        //                     y: 120,
        //                     w: 400,
        //                     h: 60
        //                 })
        //                 await 配置.对话说话框.查找并点击({
        //                     x: 300,
        //                     y: 300,
        //                     w: 1800,
        //                     h: 600
        //                 })
        //             }
        //             i--
        //         }
        //     })

        // await sta.start('回到主界面')


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
