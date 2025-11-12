const Mhxy = require('./index')
const { getList } = require('../touping.js')
const StateMachine = require('../tools/stateMachine2.js')

const 配置 = require('./config.js')

class Demo extends Mhxy {
    constructor(hwnd, changeProp) {
        super(hwnd, changeProp)
    }
    async start() {
        const result = await this.多点关联颜色匹配(配置.aaaa)
        await this.ADB左键点击(result)
        // await this.ADB滑动({ x: 527, y: 1000 }, { x: 527, y: 500 })
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







