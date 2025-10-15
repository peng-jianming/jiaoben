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

        console.log(
            await this.isImageInScreen(
                path.resolve(__dirname, '../resource', `ccc.png`),
                [
                    path.resolve(__dirname, '../resource', `123.bmp`),
                    path.resolve(__dirname, '../resource', `5555.bmp`),
                    path.resolve(__dirname, '../resource', `ccc.png`),
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


const arr = [
    {
        x:1,
        y:1,
        color: '#ffffff'
    },
    {
        x:200,
        y:200,
        color: '#ffffff'
    },
    {
        x:900,
        y:900,
        color: '#ffffff'
    },
]



/**
 * 使用OpenCV.js进行高效颜色匹配
 * @param {Array} coordArray - 坐标像素数组
 * @param {string|HTMLImageElement} image - 图片路径或图像元素
 * @returns {Promise<boolean>}
 */
async function matchCoordinatesColorOpenCV(coordArray, image) {
    try {
        let mat;
        
        // 处理不同的输入类型
        if (typeof image === 'string') {
            // 从URL加载图片
            mat = await new Promise((resolve, reject) => {
                const img = new Image();
                img.onload = () => {
                    const src = cv.imread(img);
                    resolve(src);
                };
                img.onerror = reject;
                img.src = image;
            });
        } else if (image instanceof HTMLImageElement) {
            mat = cv.imread(image);
        } else {
            throw new Error('不支持的图片格式');
        }

        // 检查图像尺寸
        const rows = mat.rows;
        const cols = mat.cols;

        // 检查所有坐标是否在范围内
        for (const coord of coordArray) {
            if (coord.x >= cols || coord.y >= rows || coord.x < 0 || coord.y < 0) {
                console.warn(`坐标点 (${coord.x}, ${coord.y}) 超出图片范围`);
                mat.delete();
                return false;
            }
        }

        // 转换为RGBA格式以便处理颜色
        const rgbaMat = new cv.Mat();
        cv.cvtColor(mat, rgbaMat, cv.COLOR_BGR2RGBA);

        // 检查每个坐标点的颜色
        for (const coord of coordArray) {
            const pixel = rgbaMat.ptr(coord.y, coord.x);
            const r = pixel[0];
            const g = pixel[1];
            const b = pixel[2];
            
            // 将RGB转换为十六进制
            const actualColor = '#' + 
                r.toString(16).padStart(2, '0') +
                g.toString(16).padStart(2, '0') + 
                b.toString(16).padStart(2, '0');

            // 比较颜色（忽略大小写）
            if (actualColor.toLowerCase() !== coord.color.toLowerCase()) {
                console.log(`坐标 (${coord.x}, ${coord.y}) 颜色不匹配: 期望 ${coord.color}, 实际 ${actualColor}`);
                mat.delete();
                rgbaMat.delete();
                return false;
            }
        }

        // 释放内存
        mat.delete();
        rgbaMat.delete();
        
        return true;

    } catch (error) {
        console.error('OpenCV处理错误:', error);
        return false;
    }
}