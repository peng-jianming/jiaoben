const Mhxy = require('./index')
const { getScreen, getList } = require('../touping.js')
const { Jimp } = require('jimp');
const path = require('path')
const fs = require('fs');
class Demo extends Mhxy {
    constructor(hwnd, changeProp) {
        super(hwnd, changeProp)
    }
    async start() {

        const url = await getScreen(this.hwnd)
        console.log(await this.findPic(url, path.resolve(__dirname, '../resource', `888.bmp`)), "====");
        
    }
}

setTimeout(() => {
    // convertPngToBmp(path.resolve(__dirname, '../resource', `192_168_31_112_5555.png`))
    getList().then(item => {
        const demo = new Demo(item[0].deviceId, () => { })
        

        setInterval(() => {
            demo.start()
        },1000)
    });

}, 1000);

// module.exports = Demo




