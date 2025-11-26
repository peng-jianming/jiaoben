const Jimp = require('jimp')
const { getScreen } = require('../touping.js')
const { opencv找图, 获取图片宽高, 随机坐标, 随机延时, ADB左键点击, 裁剪图片 } = require('./tools')
const lh = require('./lihuo')
class Field {
    constructor({ 方式, 图片路径, 标识, 相似度, 查找区域, 分类名 }) {
        this.方式 = 方式;
        this.图片路径 = 图片路径;
        this.标识 = 标识;
        this.分类名 = 分类名;
        this.相似度 = 相似度 || 0.9
        this.查找区域 = 查找区域 || { x: 0, y: 0, width: 0, height: 0 }
    }

    async 查找() {
        let url = await getScreen(global.hwnd)

        // 如果区域有效，则裁剪图片并覆盖原图
        if (this.查找区域 && this.查找区域.width && this.查找区域.height) {
            await 裁剪图片(url, this.查找区域)
        }

        if (this.方式 == 'opencv找图') {
            const point = await opencv找图(url, this.图片路径, 30, this.相似度)
            if (point) {
                global.changeProp('action', `找到${this.标识}, 坐标: ${point.x}, ${point.y}`)
                return {
                    x: this.查找区域.x + point.x,
                    y: this.查找区域.y + point.y
                }
            } else {
                // global.changeProp('action', `未找到${this.标识}`)
                return null
            }
        }

        if (this.方式 == 'yolo') {
            const result = await lh.yolo(url);
            if (result) {
                const res = result.find(item => item.label == this.分类名 && item.sim >= this.相似度)
                if (res) {
                    global.changeProp('action', `找到${this.标识}, 坐标: ${point.x}, ${point.y}`)
                    return {
                        x: this.查找区域.x + res.x,
                        y: this.查找区域.y + res.y,
                        w: res.w,
                        h: res.h
                    }
                }
            }
            return null
        }
    }

    async 查找并点击({ x, y, w, h, isOffset, startMs, endMs } = {}) {
        const point = await this.查找();
        if (point) {
            // 是否是偏移点击
            if (isOffset) {
                // 根据自身this.x和this.y来进行x,y,后,加上w,h进行随机点击
                // 只有x,y,就精确点击x,y坐标
                if (!w && !h) {
                    await ADB左键点击({ x: point.x + x, y: point.y + y })
                }
                // x,y,w,h都传入,则随机点击x,y,w,h范围内的坐标
                if (w && h) {
                    const result = 随机坐标(point.x + x, point.y + y, w, h)
                    await ADB左键点击(result)
                }
            } else {
                // 有传入x,y,但是没有传入w,h,则精确点击x,y坐标
                if (x && y && !w && !h) {
                    await ADB左键点击({ x, y })
                }

                // 有传入x,y,也有传入w,h,则随机点击x,y,w,h范围内的坐标
                if (x && y && w && h) {
                    const result = 随机坐标(x, y, w, h)
                    await ADB左键点击(result)
                }

                // 没有传入x,y,w,h,则根据查询到的结果进行范围点击
                if (!x && !y && !w && !h) {
                    let result
                    if (point.w && point.h) {
                        result = 随机坐标(point.x, point.y, point.w, point.h)
                    } else {
                        const { width, height } = await 获取图片宽高(this.图片路径)
                        result = 随机坐标(point.x, point.y, width, height)
                    }
                    await ADB左键点击(result)
                }
            }
            if (startMs && endMs && endMs >= startMs) {
                await 随机延时(startMs, endMs)
            }
            return true
        }
        return false
    }

    设置查找区域(查找区域) {
        this.查找区域 = 查找区域
        return this
    }
}


module.exports = Field




// 不断截图 保留相同像素,不同则设置为透明 制作一个新的透明图 (这个只适合有条件可以截取不同位置图片的情况)

// 点击像素点,然后根据当前点击的像素点颜色,圈选图中所有相同颜色的像素点,支持点击多个,然后再点击确定,把未圈选的像素点,都设置为透明, 这样制作透明图





// 查找一个, 根据查找的位置, 查找另外一个 (设为设为另外一个的查找区域)
// 查找一个, 根据查找的位置, 设置点击区域
// 查找一个, 然后随意点击一个位置
