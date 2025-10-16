const WebSocket = require('ws');
const path = require('path');
const { log } = require('console');
const fs = require('fs');
const { Jimp } = require('jimp');
// 连接到上游 WebSocket 服务（ws://127.0.0.1:33332）
let upstreamWS = null;

let currentResolve = () => { }

function connectUpstream() {
    upstreamWS = new WebSocket('ws://127.0.0.1:33332');


    upstreamWS.on('message', (data) => {
        try {
            const res = JSON.parse(data)
            // console.log("极限投屏返回值", res);
            if (res.StatusCode == 200) {
                currentResolve(res)
            }
        } catch (error) {
            console.log('极限投屏返回错误', error.message);
        }

    });
}

connectUpstream()

const getList = async () => {
    return new Promise(resolve => {
        const data = {
            "action": "list"
        }
        upstreamWS.send(JSON.stringify(data))
        currentResolve = (res) => {
            resolve(JSON.parse(res.result))
        }
    })
}

const getScreen = async (deviceIds) => {
    return new Promise(resolve => {
        const data = {
            "action": "screen",
            "comm": {
                "deviceIds": deviceIds,
                "savePath": path.resolve(__dirname, 'resource/cache'),
                "onlyDeviceName": 1
            }
        }
        
        upstreamWS.send(JSON.stringify(data))
        currentResolve =  () => {
            resolve(path.resolve(__dirname, 'resource/cache', `${deviceIds.replace(/[.:]/g, '_')}.png`))
        }
    })
}

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


module.exports = {
    getList,
    getScreen
};

