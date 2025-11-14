const taskRegistry = require('./task/TaskRegistry');

let currentTask = null
let currentHwnd = null
let taskLoop = null // 任务循环定时器

/**
 * 更新状态并发送到主进程
 */
function updateStatus(status) {
    process.send({
        type: 'status',
        status,
        hwnd: currentHwnd
    });
}

/**
 * 更新任意属性并发送到主进程
 */
function updateData(data) {
    process.send({
        type: 'update',
        data,
        hwnd: currentHwnd
    });
}

/**
 * 创建状态更新函数，供任务使用
 */
function createStatusUpdater() {
    return (key, value) => {
        updateData({ [key]: value });
    };
}

// 处理主进程消息
process.on('message', async (message) => {
    currentHwnd = message.hwnd;

    try {
        switch (message.type) {
            case 'start':
                await handleStart(message);
                break;
            case 'stop':
                handleStop();
                break;
            default:
                console.warn('未知消息类型:', message.type);
        }
    } catch (error) {
        console.error('处理消息时出错:', error);
        updateStatus('error');
    }
});

/**
 * 处理启动任务
 */
async function handleStart(message) {
    updateStatus('running');
    
    const { taskConfig: tasks, options = {} } = message;
    const statusUpdater = createStatusUpdater();

    // 停止之前的任务循环
    if (taskLoop) {
        clearInterval(taskLoop);
        taskLoop = null;
    }

    // 执行任务
    const executeTasks = async () => {
        // 检查是否被停止
        if (currentTask && currentTask.flag === false) {
            return;
        }

        for (const taskConfig of tasks) {
            // 再次检查是否被停止
            if (currentTask && currentTask.flag === false) {
                break;
            }

            // 支持字符串（任务名）或对象（任务名+参数）
            const taskName = typeof taskConfig === 'string' ? taskConfig : taskConfig.name;
            const taskParams = typeof taskConfig === 'object' ? taskConfig.params : {};

            if (!taskRegistry.hasTask(taskName)) {
                console.error(`任务 ${taskName} 不存在`);
                continue;
            }

            try {
                const TaskClass = taskRegistry.getTask(taskName);
                // 如果任务已存在且正在运行，先停止
                if (currentTask && typeof currentTask.stop === 'function') {
                    currentTask.stop();
                }
                currentTask = new TaskClass(message.hwnd, statusUpdater, taskParams);
                await currentTask.start();
            } catch (error) {
                console.error(`任务 ${taskName} 执行失败:`, error);
                updateStatus('error');
                throw error;
            }
        }
    };

    // 如果配置了循环执行
    if (options.loop && options.interval) {
        // 立即执行一次
        await executeTasks();
        
        // 设置循环
        taskLoop = setInterval(async () => {
            if (currentTask && currentTask.flag === false) {
                clearInterval(taskLoop);
                taskLoop = null;
                updateStatus('stopped');
                return;
            }
            try {
                await executeTasks();
            } catch (error) {
                clearInterval(taskLoop);
                taskLoop = null;
                updateStatus('error');
            }
        }, options.interval);
    } else {
        // 单次执行
        await executeTasks();
        updateStatus('idle');
    }
}

/**
 * 处理停止任务
 */
function handleStop() {
    // 停止任务循环
    if (taskLoop) {
        clearInterval(taskLoop);
        taskLoop = null;
    }

    // 停止当前任务
    if (currentTask) {
        if (typeof currentTask.stop === 'function') {
            currentTask.stop();
        }
        // 设置标志位停止循环
        if (currentTask.flag !== undefined) {
            currentTask.flag = false;
        }
        currentTask = null;
    }
    
    updateStatus('stopped');
}
