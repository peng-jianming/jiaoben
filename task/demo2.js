const Mhxy = require('./index')
const dm = require('../damo.js')

class Demo2 extends Mhxy {
    constructor(hwnd, changeProp) {
        super(hwnd, changeProp)
    }
    async start() {
        this.changeProp('status', 'demo2')
        while (this.flag) {
            dm.sendString(this.hwnd, '22222222')
            await new Promise(resolve => setTimeout(resolve, 1000))
        }
    }
}

module.exports = Demo2