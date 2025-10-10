const Mhxy = require('./index')
const { getScreen } = require('../touping.js')
class Demo extends Mhxy {
    constructor(hwnd, changeProp) {
        super(hwnd, changeProp)
    }
    async start() {
        // if (this.bind()) {
        //     await getScreen(this.hwnd)
        // }
        await getScreen(this.hwnd)
        this.二值化()
    }
}

module.exports = Demo