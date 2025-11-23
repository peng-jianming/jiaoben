const TaskBase = require('../core/TaskBase');
const { getScreen, 屏幕控制, 调用ADB } = require('../touping.js')
const lihuo = require('../tools/lihuo')
const path = require('path');
const 配置 = require('../resource/index.js')

class Mhxy extends TaskBase {

    async 回到主界面() {
        return true || false
    }

    async 打开活动界面() {
        // 点击活动
        const point = await 配置.主界面活动按钮.查找并点击()
        
        if (!point) {
            return false
        }

        await this.随机延时(1000, 3000)

        // 出现活动界面,
        const point1 = await 配置.活动界面.查找()
        return !!point1
    }

    async 战斗界面处理() {
        
    }



    async 随机点击坐标(目标信息) {
        const point = this.随机坐标(目标信息.区域.x1, 目标信息.区域.y1, 目标信息.区域.x2, 目标信息.区域.y2)
        global.changeProp('action', `随机点击${目标信息.标识}, 坐标: ${point.x}, ${point.y}`)
        await this.ADB左键点击(point)
    }

    async 查找并点击图片(目标信息) {
        const point = await this.查找(目标信息)
        if (point) {
            global.changeProp('action', `点击${目标信息.标识}`)
            await this.ADB左键点击(point)
            return true
        } else {
            return false
        }

    }

    async 查找(目标信息, 是否随机坐标 = true) {
        // 是否传入大图,如果没有传入大图,则获取当前屏幕截图
        let url = 目标信息.大图路径 || await getScreen(global.hwnd)
        // global.changeProp('action', `查找${目标信息.标识}`)
        if (目标信息.方式 == '找图') {
            const point = lihuo.lhFindPicMateFile(url, 目标信息.图片路径)
            if (point) {
                global.changeProp('action', `找到${目标信息.标识}, 坐标: ${point.x}, ${point.y}`)
                if (是否随机坐标) {
                    const { width, height } = await this.获取图片宽高(目标信息.图片路径)
                    const result = this.随机坐标(point.x, point.y, point.x + width, point.y + height)
                    return result
                } else {
                    return point
                }
            } else {
                global.changeProp('action', `未找到${目标信息.标识}`)
                return false;
            }
        }
    }

}

module.exports = Mhxy
