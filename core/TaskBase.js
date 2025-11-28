const { 调用ADB } = require('../touping.js')
class TaskBase {
    flag = true

    stop() {
        this.flag = false;
    }

    async start() {
        throw new Error('子类必须实现start()方法');
    }
}

module.exports = TaskBase;

