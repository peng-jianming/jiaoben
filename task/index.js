const TaskBase = require('../core/TaskBase');
const { getScreen, 屏幕控制, 调用ADB } = require('../touping.js')
const lihuo = require('../tools/lihuo')
const path = require('path');
const 配置 = require('../resource/index.js')
/**
 * 任务基类（兼容性包装）
 * 为了保持向后兼容，保留 Mhxy 类名
 * 实际继承自 core/TaskBase，所有方法都在基类中实现
 * 
 * 使用方式：
 * const Mhxy = require('./index');
 * class MyTask extends Mhxy {
 *     async start() {
 *         // 你的任务逻辑
 *     }
 * }
 */
class Mhxy extends TaskBase {
    // 可以在这里添加项目特定的方法
    // 例如：打开活动弹框、特定游戏操作等

    // 示例：项目特定的方法
    // async 打开活动弹框() {
    //     const res1 = await this.多点关联颜色匹配(配置.活动按钮);
    //     if (res1) {
    //         console.log('打开活动弹框')
    //         await this.左键点击(res1);
    //         await this.延时(2000)
    //     }
    // }

    async 回到主界面() {
        return true || false
    }

    async 打开活动界面() {
        // 点击活动
        const point = await this.查找并点击图片(配置.主界面活动按钮)
        if (!point) {
            return false
        }

        await this.随机延时(1000, 3000)

        // 出现活动界面,
        const point1 = await this.查找(配置.活动界面)
        return !!point1
    }

    async 随机点击坐标(目标信息) {
        const point = this.随机坐标(目标信息.区域.x1, 目标信息.区域.y1, 目标信息.区域.x2, 目标信息.区域.y2)
        this.changeProp('action', `随机点击${目标信息.标识}, 坐标: ${point.x}, ${point.y}`)
        await this.ADB左键点击(point)
    }



    async 查找并点击图片(目标信息) {
        const point = await this.查找(目标信息)
        if (point) {
            this.changeProp('action', `点击${目标信息.标识}`)
            await this.ADB左键点击(point)
            return true
        } else {
            return false
        }

    }

    async 查找(目标信息, 是否随机坐标 = true) {
        // 是否传入大图,如果没有传入大图,则获取当前屏幕截图
        let url = 目标信息.大图路径 || await getScreen(this.hwnd)
        // this.changeProp('action', `查找${目标信息.标识}`)
        if (目标信息.方式 == '找图') {
            const point = lihuo.lhFindPicMateFile(url, 目标信息.图片路径)
            if (point) {
                this.changeProp('action', `找到${目标信息.标识}, 坐标: ${point.x}, ${point.y}`)
                if (是否随机坐标) {
                    const { width, height } = await this.获取图片宽高(目标信息.图片路径)
                    const result = this.随机坐标(point.x, point.y, point.x + width, point.y + height)
                    return result
                } else {
                    return point
                }
            } else {
                this.changeProp('action', `未找到${目标信息.标识}`)
                return false;
            }
        }
    }

}

module.exports = Mhxy




// .查找().点击()

class Action {
    constructor({ 方式, 图片路径, 大图路径, 标识 }, 操作类) {
        this.方式 = 方式;
        this.图片路径 = 图片路径;
        this.大图路径 = 大图路径;
        this.标识 = 标识;
        this.找到的坐标 = { x: 0, y: 0 }
        this.操作类 = 操作类
    }

    async 查找() {
        let url = this.大图路径 || await getScreen(this.hwnd)

        if (this.方式 == '找图') {
            const point = lihuo.lhFindPicMateFile(url, this.图片路径)
            if (point) {
                this.操作类.changeProp('action', `找到${this.标识}, 坐标: ${point.x}, ${point.y}`)
                this.找到的坐标 = point
            } else {
                this.操作类.changeProp('action', `未找到${this.标识}`)
            }
        }

        return this
    }


    async 范围点击() {
        const { width, height } = await this.操作类.获取图片宽高(this.图片路径)
        const result = this.操作类.随机坐标(this.找到的坐标.x, this.找到的坐标.y, width, height)
        if (result) {

        } else {

        }
        return this
    }

    点击(坐标) {

    }
}