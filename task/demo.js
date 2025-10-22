const Mhxy = require('./index')
const { getList } = require('../touping.js')
const StateMachine = require('../tools/stateMachine.js')

const 配置 = require('./config.js')

class Demo extends Mhxy {
    constructor(hwnd, changeProp) {
        super(hwnd, changeProp)
    }
    async start() {
        // 执行任务的开始，不管目前处于哪一步，都是从头开始
        // 出任何问题了，直接从头开始


        // const result = await this.多点关联颜色匹配(配置.活动按钮);
        // console.log(result, "=====");
        // if(result) {
        //     this.左键点击(result)

        //     this.滑动({x: 1330, y: 257}, {x: 1335, y: 600})
        // }
        // this.打开活动弹框()
        let num = 0
        const otherStateMachine = new StateMachine()
            .on('进入主页面', async () => {
                console.log('点击活动按钮')
                otherStateMachine.currentState = '进入活动界面'
            }, 0, -Infinity)
            .on('进入活动界面', () => {
                console.log('点击师门按钮')
                otherStateMachine.currentState = '进入师门界面'
            }, 5000, -Infinity)
            .on('进入师门界面', () => {
                console.log('点击开始任务')
                otherStateMachine.currentState = '做师门任务'
            }, 0, -Infinity)
            .on('做师门任务', async () => {
                console.log('正在做师门任务')
                const aaa = new StateMachine(() => {
                    // 记录标记()
                    // if (师门召唤兽购买上交) {
                    //     return '师门召唤兽购买上交'
                    // }
                    // if (找到右侧师门) {
                    //     return '找到右侧师门'
                    // }
                    // if (不动检测()) {
                    //     return '卡住了'
                    // }
                    return '卡住了'
                })
                    .on('找到右侧师门', async () => {
                        console.log('点击右侧师门');
                    }, 0, -Infinity)
                    .on('师门召唤兽购买上交', async () => {
                        console.log('购买召唤兽');
                    }, 0, -Infinity)
                    .on('卡住了', async () => {
                        aaa.stop()
                        otherStateMachine.currentState = '进入主页面'
                    }, 0, -Infinity)


                aaa.start(1000)

            }, 0, -Infinity)
            .onTimeout((state) => {
                console.log(state, '超时了')
                otherStateMachine.currentState = '进入主页面'
            })

        otherStateMachine.start(1000)
        otherStateMachine.currentState = '进入主页面'
    }
}

setTimeout(() => {
    // getList().then(item => {
    //     const demo = new Demo(item[0].deviceId, () => { })
    //     setInterval(() => {
    //         demo.start()
    //     }, 10000)
    //     demo.start()
    // });
    const demo = new Demo('1111', () => { })

    demo.start()
}, 1000);

// module.exports = Demo




// 延时
// 滑动
// 随机点击
// 偶尔失误点击
// 制作透明图，然后获取剩余的颜色和坐标


