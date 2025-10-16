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



/**
 * 将 PNG 图片转换为 BMP 格式
 * @param {string} pngPath - PNG 图片的路径
 * @param {string} bmpPath - 输出 BMP 图片的路径（可选，默认替换原文件扩展名）
 * @returns {Promise<string>} 返回转换后的 BMP 文件路径
 */
const convertPngToBmp = async (pngPath, bmpPath = null) => {
    try {
        // 检查输入文件是否存在
        if (!fs.existsSync(pngPath)) {
            throw new Error(`PNG 文件不存在: ${pngPath}`);
        }

        // 如果没有指定输出路径，则使用原文件名替换扩展名
        if (!bmpPath) {
            const ext = path.extname(pngPath);
            const nameWithoutExt = path.basename(pngPath, ext);
            const dir = path.dirname(pngPath);
            bmpPath = path.join(dir, `${nameWithoutExt}.bmp`);
        }

        console.log(`开始转换: ${pngPath} -> ${bmpPath}`);

        // 使用 Jimp 读取 PNG 图片
        const image = await Jimp.read(pngPath);

        // 转换为 BMP 格式并保存
        await image.write(bmpPath);

        console.log(`转换完成: ${bmpPath}`);
        return bmpPath;

    } catch (error) {
        console.error('PNG 转 BMP 转换失败:', error.message);
        throw error;
    }
}


