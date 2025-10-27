const Mhxy = require('./index')
const { getList } = require('../touping.js')
const StateMachine = require('../tools/stateMachine2.js')

const 配置 = require('./config.js')

class Demo extends Mhxy {
    constructor(hwnd, changeProp) {
        super(hwnd, changeProp)
    }
    async start() {
        // this.滑动({ x: 327, y: 1366 }, { x: 1069, y: 1364 })
        await this.滑动({ x: 527, y: 1000 }, { x: 527, y: 500 })
    }
}

setTimeout(() => {
    getList().then(item => {
        const demo = new Demo(item[0].deviceId, () => { })
        setInterval(() => {
            demo.start()
        }, 3000)
        demo.start()
    });
    // const demo = new Demo('1111', () => { })

    // demo.start()
}, 1000);

// module.exports = Demo







