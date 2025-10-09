const Mhxy = require('./index')
const dm = require('../damo.js')
class Demo extends Mhxy {
    constructor(hwnd, changeProp) {
        super(hwnd, changeProp)
    }
    async start() {
        setTimeout(() => {
            this.flag = false
        }, 5000);
        this.changeProp('status', 'demo')
        while (this.flag) {
            dm.sendString(this.hwnd, '111111')
        }
    }
}


module.exports = Demo