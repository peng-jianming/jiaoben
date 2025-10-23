const Mhxy = require('./index')
const { getList } = require('../touping.js')
const StateMachine = require('../tools/stateMachine2.js')

const 配置 = require('./config.js')

class Demo extends Mhxy {
    constructor(hwnd, changeProp) {
        super(hwnd, changeProp)
    }
    async start() {
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





