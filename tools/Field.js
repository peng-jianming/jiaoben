const Jimp = require('jimp')
const { getScreen, 屏幕控制, 调用ADB } = require('../touping.js')
const lihuo = require('./lihuo')

class Field {
    constructor({ 方式, 图片路径, 大图路径, 标识, 相似度, 区域, 字库序号, 字库路径, 文字, 偏色 }) {
        this.方式 = 方式;
        this.图片路径 = 图片路径;
        this.大图路径 = 大图路径;
        this.标识 = 标识;
        this.相似度 = 相似度 || 0.8
        this.区域 = 区域 || { x1: 0, y1: 0, x2: 0, y2: 0 }
        this.字库序号 = 字库序号
        this.字库路径 = 字库路径
        this.文字 = 文字
        this.偏色 = 偏色
    }

    async _查找() {
        let url = this.大图路径 || await getScreen(global.hwnd)

        if (this.方式 == '找图') {
            const point = lihuo.lhFindPicMateFile(url, this.图片路径, this.相似度, this.区域.x1, this.区域.y1, this.区域.x2, this.区域.y2)
            if (point) {
                global.changeProp('action', `找到${this.标识}, 坐标: ${point.x}, ${point.y}`)
                return point
            } else {
                global.changeProp('action', `未找到${this.标识}`)
                return false;
            }
        }

        if (this.方式 == '找字') {
            lihuo.setDict(this.字库序号, this.字库路径)
            const point = lihuo.FindStrFile(url, this.文字, this.字库序号, this.偏色, this.相似度, this.区域.x1, this.区域.y1, this.区域.x2, this.区域.y2)
            if (point) {
                global.changeProp('action', `找到${this.标识}, 坐标: ${point.x}, ${point.y}`)
                return point
            } else {
                global.changeProp('action', `未找到${this.标识}`)
                return false;
            }
        }
        if (this.方式 == 'AI找字') {
            const point = lihuo.ocrDetectFile(url, this.文字, this.相似度)
            console.log(point, "tttttttttttttttttttt");
            
            // if (point) {
            //     global.changeProp('action', `找到${this.标识}, 坐标: ${point.x}, ${point.y}`)
            //     return point
            // } else {
            //     global.changeProp('action', `未找到${this.标识}`)
            //     return false;
            // }
        }
    }

    async 查找并点击() {
        const point = await this._查找(this)
        if (point) {
            global.changeProp('action', `点击${this.标识}`)
            const { width, height } = await this.获取图片宽高(this.图片路径)
            const result = this.随机坐标(point.x, point.y, width, height)
            await this.ADB左键点击(result)
            return result
        } else {
            return false
        }
    }

    async 查找() {
        const point = await this._查找(this)
        if (point) {
            return point
        } else {
            return false
        }
    }

    设置查找区域(区域) {
        this.区域 = 区域
        return this
    }
    设置大图路径(路径) {
        this.大图路径 = 路径
        return this
    }

    async 获取图片宽高(imagePath) {
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

    随机区间位置(start, end) {
        return Math.floor(Math.random() * (end - start)) + start;
    }

    随机坐标(x1, y1, width, height) {
        return {
            x: this.随机区间位置(x1, x1 + width),
            y: this.随机区间位置(y1, y1 + height)
        }
    }

    随机区间时间(startMs, endMs) {
        if (startMs > endMs) {
            [startMs, endMs] = [endMs, startMs];
        }
        // 生成包含两端点的随机整数
        return Math.floor(Math.random() * (endMs - startMs + 1)) + startMs;
    }

    延时(time) {
        return new Promise((resolve) => {
            setTimeout(resolve, time);
        });
    }

    随机延时(startMs, endMs) {
        return this.延时(this.随机区间时间(startMs, endMs));
    }

    async ADB左键点击(result) {
        if (result) {
            // 按下
            await 调用ADB(global.hwnd, `input motionevent DOWN ${result.x} ${result.y}`)

            await this.随机延时(300, 800)
            // 弹起
            await 调用ADB(global.hwnd, `input motionevent UP ${result.x} ${result.y}`)
        } else {
            console.log('左键点击坐标为空');
        }
    }
}


module.exports = Field