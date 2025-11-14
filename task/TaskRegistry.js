const fs = require('fs');
const path = require('path');

/**
 * 任务注册表
 * 自动发现并注册 task 目录下的所有任务类
 */
class TaskRegistry {
    constructor() {
        this.tasks = new Map();
        this.loadTasks();
    }

    /**
     * 自动加载所有任务
     */
    loadTasks() {
        const taskDir = __dirname;
        const files = fs.readdirSync(taskDir);

        files.forEach(file => {
            // 跳过非任务文件
            if (file === 'index.js' || file === 'config.js' || file === 'TaskRegistry.js') {
                return;
            }

            // 只加载 .js 文件
            if (!file.endsWith('.js')) {
                return;
            }

            try {
                const taskPath = path.join(taskDir, file);
                const TaskClass = require(taskPath);
                
                // 获取任务名称（文件名去掉扩展名）
                const taskName = path.basename(file, '.js');
                
                // 验证是否为有效的任务类（有 start 方法）
                if (TaskClass && typeof TaskClass.prototype.start === 'function') {
                    this.tasks.set(taskName, TaskClass);
                    console.log(`✓ 已注册任务: ${taskName}`);
                } else {
                    console.warn(`⚠ 跳过 ${file}: 不是有效的任务类`);
                }
            } catch (error) {
                console.error(`✗ 加载任务 ${file} 失败:`, error.message);
            }
        });
    }

    /**
     * 获取任务类
     */
    getTask(taskName) {
        return this.tasks.get(taskName);
    }

    /**
     * 获取所有任务名称
     */
    getTaskNames() {
        return Array.from(this.tasks.keys());
    }

    /**
     * 检查任务是否存在
     */
    hasTask(taskName) {
        return this.tasks.has(taskName);
    }
}

module.exports = new TaskRegistry();

