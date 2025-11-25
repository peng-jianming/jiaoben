const taskRegistry = require('./task/TaskRegistry');

let currentTask = null
global.hwnd = null
let taskLoop = null // 任务循环定时器

/**
 * 更新任意属性并发送到主进程
 */
function updateData(data) {
    process.send({
        ...data,
        hwnd: global.hwnd
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

    global.hwnd = message.hwnd;

    try {
        switch (message.type) {
            case 'start':
                global.changeProp = createStatusUpdater();
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

    // 停止之前的任务循环
    if (taskLoop) {
        clearInterval(taskLoop);
        taskLoop = null;
    }

    // 执行任务
    const executeTasks = async () => {
        // 检查是否被停止
        if (currentTask && currentTask.flag === false) {
            currentTask = null;
            updateData({ 'status': '空闲中' });
            return;
        }

        for (const taskName of taskList) {
            // 再次检查是否被停止
            if (currentTask && currentTask.flag === false) {
                currentTask = null;
                updateData({ 'status': '空闲中' });
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
                currentTask = new TaskClass();
                updateData({ 'status': `${taskName}` });
                await currentTask.start();
                
                // 任务执行完成后，检查是否被停止
                if (currentTask && currentTask.flag === false) {
                    currentTask = null;
                    updateData({ 'status': '空闲中' });
                    return;
                }
            } catch (error) {
                console.error(`任务 ${taskName} 执行失败:`, error);
                updateData({ 'status': `任务 ${taskName} 执行失败` });
                // 如果是因为停止导致的错误，更新状态为空闲
                if (currentTask && currentTask.flag === false) {
                    currentTask = null;
                    updateData({ 'status': '空闲中' });
                    return;
                }
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
        // 如果任务没有被停止（currentTask 还存在且 flag 不为 false），更新状态为空闲中
        // 如果任务被停止了，executeTasks 中已经将 currentTask 设为 null 并更新了状态
        if (currentTask && currentTask.flag !== false) {
            updateData({ 'status': '空闲中' });
        } else if (!currentTask) {
            // 如果 currentTask 为 null，说明任务被停止了，状态已经在 executeTasks 中更新
            // 这里不需要再次更新
        }
    }
}

/**
 * 处理停止任务, 先尝试任务中停止, 如果任务中没有停止, 则强制停止
 */
function handleStop() {
    console.log('收到停止任务请求');
    
    // 立即更新状态，让主进程知道正在停止（但不立即设为空闲中）
    updateData({ 'status': '正在停止...' });
    
    // 停止任务循环
    if (taskLoop) {
        clearInterval(taskLoop);
        taskLoop = null;
    }

    // 停止当前任务
    if (currentTask) {
        // 先设置标志位，让任务能够快速响应
        if (currentTask.flag !== undefined) {
            currentTask.flag = false;
        }
        // 然后调用 stop 方法
        if (typeof currentTask.stop === 'function') {
            try {
                currentTask.stop();
            } catch (error) {
                console.error('调用任务 stop 方法时出错:', error);
            }
        }
        // 注意：不立即清空 currentTask，让 executeTasks 能够检测到停止
    }
    
    // 不立即更新状态为空闲，等待任务真正停止后再更新
    // 状态更新将在 executeTasks 完成后进行
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
            console.log(`[Worker ${global.hwnd}] 任务 ${taskName} 已重新加载`);
        }
    } else {
        // 重新加载所有任务
        const count = taskRegistry.reloadAllTasks();
        console.log(`[Worker ${global.hwnd}] 已重新加载 ${count} 个任务`);
    }
}
