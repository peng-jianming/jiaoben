const Mhxy = require('./index')
const { getList } = require('../touping.js')
const StateMachine = require('../tools/stateMachine.js')

const 配置 = require('./config.js')

class Demo extends Mhxy {
    constructor(hwnd, changeProp) {
        super(hwnd, changeProp)
    }
    async start() {
        // const result = await this.多点关联颜色匹配(配置.活动按钮);
        // console.log(result, "=====");
        // if(result) {
        //     this.左键点击(result)

        //     this.滑动({x: 1330, y: 257}, {x: 1335, y: 600})
        // }
        // this.打开活动弹框()
        let num = 0
        const otherStateMachine = new StateMachine()
            .on('state1', async () => {
                console.log('state1 开始了')
                otherStateMachine.currentState = 'state2'
                await this.延时(3)
            }, 0, -Infinity)
            .on('state2', () => {
                throw new Error('112233');
                otherStateMachine.currentState = 'state3' 
                console.log('state2 开始了')
            }, 4000, 1000)
            .on('state3', () => {
                console.log('state3 开始了')
            }, 9000, 1000)
            .onTimeout((state) => {
                // num += 1
                console.log(state, '超时了')
            })
            .onError((e) => {
                console.log('错误了',e.message)
            })
        
        otherStateMachine.start(1000)
        otherStateMachine.currentState = 'state1'
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




