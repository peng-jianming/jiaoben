const Mhxy = require('./index')
// const { getList } = require('../touping.js')
const StateMachine = require('../tools/stateMachine.js')

const 配置 = require('./config.js')

class Demo extends Mhxy {
    constructor(hwnd, changeProp) {
        super(hwnd, changeProp)
    }
    async start() {
        // const result = await this.多点关联颜色匹配(配置.主线);
        // console.log(result, "=====");

        const otherStateMachine = new StateMachine(() => {
            console.log('1111111');
            
            return 'state1'
        })
            .on('state1', () => { console.log('state1 开始了') }, 3000, -Infinity)
            .onTimeout((state) => { 

                console.log(state, '超时了')
                // otherStateMachine.start(1000)
             })

        otherStateMachine.start(1000)
    }
}

setTimeout(() => {
    // getList().then(item => {
    //     const demo = new Demo(item[0].deviceId, () => { })
        // setInterval(() => {
        //     demo.start()
        // }, 1000)
    //     demo.start()
    // });
    const demo = new Demo('1111', () => { })

    demo.start()
}, 1000);

// module.exports = Demo




