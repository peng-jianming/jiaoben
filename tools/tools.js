const Jimp = require('jimp')
const { 调用ADB } = require('../touping.js')

const 获取图片宽高 = async (imagePath) => {
    try {
        const image = await Jimp.Jimp.read(imagePath);
        return {
            width: image.bitmap.width,
            height: image.bitmap.height
        };
    } catch (error) {
        console.error('读取图片失败:', error);
        throw error;
    }
}

const 随机区间位置 = (start, end) => {
    return Math.floor(Math.random() * (end - start)) + start;
}

const 随机区间时间 = (startMs, endMs) => {
    if (startMs > endMs) {
        [startMs, endMs] = [endMs, startMs];
    }
    // 生成包含两端点的随机整数
    return Math.floor(Math.random() * (endMs - startMs + 1)) + startMs;
}

const 随机坐标 = (x1, y1, width, height) => {
    return {
        x: 随机区间位置(x1, x1 + width),
        y: 随机区间位置(y1, y1 + height)
    }
}

const 延时 = (time) => {
    return new Promise((resolve) => setTimeout(resolve, time));
}

const 随机延时 = (startMs, endMs) => {
    return 延时(随机区间时间(startMs, endMs));
}

const ADB左键点击 = async (result) => {
    if (result) {
        // 按下
        await 调用ADB(global.hwnd, `input motionevent DOWN ${result.x} ${result.y}`)

        await 随机延时(100, 500)
        // 弹起
        await 调用ADB(global.hwnd, `input motionevent UP ${result.x} ${result.y}`)
    } else {
        console.log('左键点击坐标为空');
    }
}


const 百分之十随机用户操作 = async () => {
    if (Math.random() < 0.1) {
        const result = 随机坐标(200, 800, 600, 1000)
        console.log("🖱 触发随机事件 随机点击: (" + result.x + "," + result.y + ")");
        await ADB左键点击(result)
    }
}

module.exports = {
    获取图片宽高,
    随机区间位置,
    随机区间时间,
    随机坐标,
    延时,
    随机延时,
    ADB左键点击,
    百分之十随机用户操作
}