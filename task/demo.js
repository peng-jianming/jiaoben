const Mhxy = require('./index')
// const { getScreen, getList } = require('../touping.js')
const Jimp = require('jimp');
const path = require('path')
class Demo extends Mhxy {
    constructor(hwnd, changeProp) {
        super(hwnd, changeProp)
    }
    async start() {
        // await getList()
        // await getScreen(this.hwnd)
        // console.log(Jimp.Jimp.read(path.resolve(__dirname, '../resource', `aaa.bmp`)), "===");

        console.log(await this.isImageInScreen(
            path.resolve(__dirname, '../resource', `ccc.png`),
            [
                path.resolve(__dirname, '../resource', `123.bmp`),
                // path.resolve(__dirname, '../resource', `ccc.png`),
                // path.resolve(__dirname, '../resource', `5555.bmp`),
            ]
        )
        );

    }
}


const demo = new Demo('94295493', () => { })
setTimeout(() => {
    demo.start()
}, 1000);

// module.exports = Demo