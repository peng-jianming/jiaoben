const Mhxy = require('./index')
const { getList } = require('../touping.js')

const StateMachine = require('../stateMachine.js')

const 配置 = require('../config.js')

class Demo extends Mhxy {
    constructor(hwnd, changeProp) {
        super(hwnd, changeProp)
    }
    async start() {
        // const result = await this.多点关联颜色匹配(配置.主线);
        // console.log(result, "=====");
        console.log(StateMachine, "===");

        const otherStateMachine = new StateMachine(() => {
            console.log('11111111111');

            return 'state1'
        }).on('state1', () => {
            console.log('state4 开始了')
            return new Promise((resolve) => {
                setTimeout(() => resolve(console.log('state4 结束了')), 3 * 1000)
            })
        },
            0,
            -Infinity)
            .onTimeout((state) => console.log(state, '超时了'))
        otherStateMachine.start(1000)
    }
}

setTimeout(() => {
    getList().then(item => {
        const demo = new Demo(item[0].deviceId, () => { })
        // setInterval(() => {
        //     demo.start()
        // }, 1000)
        demo.start()
    });
}, 1000);

// module.exports = Demo




