const Mhxy = require('./index')
const { getScreen, getList } = require('../touping.js')
class Demo extends Mhxy {
    constructor(hwnd, changeProp) {
        super(hwnd, changeProp)
    }
    async start() {
        // await getList()
        // await getScreen(this.hwnd)
        console.log(await this.isImageInScreen());
        
    }
}


const demo = new Demo('94295493', () => {})
setTimeout(() => {
    demo.start()
}, 1000);

// module.exports = Demo