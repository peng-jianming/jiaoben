const TaskBase = require('../core/TaskBase');
const 配置 = require('./config.js')


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
}

module.exports = Mhxy
