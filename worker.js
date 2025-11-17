const taskRegistry = require('./task/TaskRegistry');

let currentTask = null
let currentHwnd = null
let taskLoop = null // 任务循环定时器

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
    console.log('收到来自主进程的信息:', message);

    currentHwnd = message.hwnd;

    try {
        switch (message.type) {
            case 'start':
                await handleStart(message);
                break;
            case 'stop':
                handleStop();
                break;
            // 和任务逻辑无关,由热重载控制
            case 'reload':
                handleReload(message);
                break;
            default:
                console.warn('未知消息类型:', message.type);
        }
    } catch (error) {
        console.error('处理消息时出错:', error);
    }
});

/**
 * 处理启动任务
 */
async function handleStart(message) {
    const { taskList, options = {} } = message;
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

        for (const taskName of taskList) {
            // 再次检查是否被停止
            if (currentTask && currentTask.flag === false) {
                break;
            }

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
                currentTask = new TaskClass(message.hwnd, statusUpdater);
                await currentTask.start();
            } catch (error) {
                console.error(`任务 ${taskName} 执行失败:`, error);
                updateData({ 'status': `任务 ${taskName} 执行失败` });
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
                updateData({ 'status': '空闲中' });
                return;
            }
            try {
                await executeTasks();
            } catch (error) {
                clearInterval(taskLoop);
                taskLoop = null;
                updateData({ 'status': `循环任务错误` });
            }
        }, options.interval);
    } else {
        // 单次执行
        await executeTasks();
        updateData({ 'status': '空闲中' });
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
    updateData({ 'status': '空闲中' });
}

/**
 * 处理重载任务
 */
function handleReload(message) {
    const { taskName } = message;

    if (taskName && taskName !== 'all') {
        // 重新加载指定任务
        const success = taskRegistry.reloadTask(taskName);
        if (success) {
            console.log(`[Worker ${currentHwnd}] 任务 ${taskName} 已重新加载`);
        }
    } else {
        // 重新加载所有任务
        const count = taskRegistry.reloadAllTasks();
        console.log(`[Worker ${currentHwnd}] 已重新加载 ${count} 个任务`);
    }
}
