const { fork } = require('child_process');
const path = require('path');
const EventEmitter = require('events');
const { getList } = require('../touping');
/**
 * 工作进程管理器
 * 负责管理所有工作进程的生命周期、通信和 WebSocket 消息处理
 */
class WorkerManager extends EventEmitter {
    constructor(wss) {
        super();
        this.workers = new Map();
        this.wss = wss;
    }

    /**
     * 初始化工作进程
     */
    initWorker(hwnd, name) {
        if (this.workers.has(hwnd)) return this.workers.get(hwnd);

        const worker = fork(path.join(__dirname, '../worker.js'));
        const workerInfo = {
            process: worker,
            hwnd,
            name,
            status: '空闲中',
            lastUpdate: new Date()
        };

        this.workers.set(hwnd, workerInfo);

        worker.on('message', (message) => {
            console.log('收到来自子进程的信息:', message);
            Object.assign(workerInfo, message);
            workerInfo.lastUpdate = new Date();
            this.broadcast(workerInfo);
        });

        // 子进程错误或者退出,那么会在当前页面有记录,如果刷新了,那么记录就消失
        worker.on('exit', () => {
            this.workers.delete(hwnd);
            this.broadcast({ ...workerInfo, status: '子进程退出' });
        });

        worker.on('error', () => {
            this.workers.delete(hwnd);
            this.broadcast({ ...workerInfo, status: '子进程错误' });
        });

        return workerInfo;
    }

    /**
     * 发送消息到工作进程
     */
    sendToWorker(hwnd, message) {
        const workerInfo = this.workers.get(hwnd);
        if (!workerInfo) throw new Error(`Worker ${hwnd} not found`);
        workerInfo.process.send({ ...message, hwnd });
    }

    /**
     * 停止任务（带超时强制重启机制）
     * @param {string} hwnd - 设备句柄
     * @param {number} timeout - 超时时间（毫秒），默认3000ms
     * @returns {Promise<boolean>} - 是否成功停止（true=正常停止，false=强制重启）
     */
    async stopTask(hwnd, timeout = 3000) {
        const workerInfo = this.workers.get(hwnd);
        if (!workerInfo) {
            console.warn(`Worker ${hwnd} not found`);
            return false;
        }

        const stopStartTime = Date.now();

        // 发送停止消息
        try {
            this.sendToWorker(hwnd, { type: 'stop' });
        } catch (error) {
            console.error(`发送停止消息到 ${hwnd} 失败:`, error);
            return false;
        }

        // 等待任务停止，检查状态变化
        return new Promise((resolve) => {
            const checkInterval = setInterval(() => {
                const currentStatus = workerInfo.status;
                const elapsed = Date.now() - stopStartTime;

                // 如果状态变为空闲，说明正常停止了
                if (currentStatus === '空闲中') {
                    clearInterval(checkInterval);
                    clearTimeout(timeoutTimer);
                    console.log(`任务 ${hwnd} 正常停止`);
                    resolve(true);
                    return;
                }

                // 如果状态是"正在停止..."，说明正在停止中，继续等待
                // 如果状态不是"正在停止..."也不是"空闲中"，且已经过了超时时间，强制重启
                if (currentStatus !== '正在停止...' && currentStatus !== '空闲中' && elapsed >= timeout) {
                    clearInterval(checkInterval);
                    clearTimeout(timeoutTimer);
                    console.warn(`任务 ${hwnd} 在 ${timeout}ms 内未停止（状态：${currentStatus}），强制重启子进程`);
                    this.forceRestartWorker(hwnd);
                    resolve(false);
                    return;
                }

                // 如果状态是"正在停止..."但超时了，强制重启
                if (currentStatus === '正在停止...' && elapsed >= timeout) {
                    clearInterval(checkInterval);
                    clearTimeout(timeoutTimer);
                    console.warn(`任务 ${hwnd} 在 ${timeout}ms 内未停止（仍在停止中），强制重启子进程`);
                    this.forceRestartWorker(hwnd);
                    resolve(false);
                    return;
                }
            }, 100); // 每100ms检查一次

            // 超时定时器（备用）
            const timeoutTimer = setTimeout(() => {
                clearInterval(checkInterval);
                const finalStatus = workerInfo.status;
                console.warn(`任务 ${hwnd} 停止超时（最终状态：${finalStatus}），强制重启子进程`);
                this.forceRestartWorker(hwnd);
                resolve(false);
            }, timeout);
        });
    }

    /**
     * 强制重启工作进程
     * @param {string} hwnd - 设备句柄
     */
    forceRestartWorker(hwnd) {
        const workerInfo = this.workers.get(hwnd);
        if (!workerInfo) {
            console.warn(`Worker ${hwnd} not found, cannot restart`);
            return;
        }

        const name = workerInfo.name;
        const process = workerInfo.process;

        // 强制结束旧进程
        try {
            if (process && !process.killed) {
                process.kill('SIGKILL'); // 强制终止
            }
        } catch (error) {
            console.error(`强制终止进程 ${hwnd} 失败:`, error);
        }

        // 从 workers Map 中移除
        this.workers.delete(hwnd);

        // 等待一小段时间确保进程完全退出
        setTimeout(() => {
            // 重新创建新的工作进程
            this.initWorker(hwnd, name);
            console.log(`工作进程 ${hwnd} 已强制重启`);
        }, 100);
    }

    /**
     * 处理来自客户端的 WebSocket 消息
     */
    handleClientMessage(message) {
        const { deviceList } = message;
        if (deviceList) {
            const hwnds = deviceList.map(device => device.hwnd);

            // 如果是停止消息，使用带超时的停止机制
            if (message.type === 'stop') {
                hwnds.forEach(hwnd => {
                    // 异步执行，不阻塞
                    this.stopTask(hwnd).catch(err => {
                        console.error(`停止任务 ${hwnd} 失败:`, err);
                    });
                });
            } else {
                // 其他消息正常发送
                hwnds.forEach(hwnd => {
                    try {
                        this.sendToWorker(hwnd, message);
                    } catch (err) {
                        console.error(`Failed to send message to ${hwnd}:`, err);
                    }
                });
            }
        }
    }

    /**
     * 初始化设备列表并发送给客户端
     */
    async handleInit() {
        try {
            const deviceListRaw = await getList();

            const deviceList = deviceListRaw.map(item => ({
                hwnd: item.deviceId,
                name: item.name
            }));

            deviceList.forEach(device => {
                this.initWorker(device.hwnd, device.name);
            });

            const message = JSON.stringify({
                type: 'init',
                data: {
                    list: Array.from(this.workers.values()).map(info => this.getDataFormat(info))
                }
            });
            this.wss.clients.forEach((client) => {
                if (client.readyState === require('ws').OPEN) {
                    client.send(message);
                }
            });
        } catch (error) {
            console.error('初始化设备列表失败:', error);
        }
    }

    /**
     * 广播单个 worker 状态到所有 WebSocket 客户端
     */
    broadcast(workerInfo) {
        const message = JSON.stringify({
            type: 'updateItem',
            data: this.getDataFormat(workerInfo)
        });
        this.wss.clients.forEach((client) => {
            if (client.readyState === require('ws').OPEN) {
                client.send(message);
            }
        });
        // const message = JSON.stringify({
        //     type: 'update',
        //     data: this.getStatusList()
        // });
        // this.wss.clients.forEach((client) => {
        //     if (client.readyState === require('ws').OPEN) {
        //         client.send(message);
        //     }
        // });
    }

    cleanup() {
        this.workers.forEach((info, hwnd) => {
            if (info.process) info.process.kill();
            this.workers.delete(hwnd);
        });
    }

    /**
     * 格式化日期为 yyyy-MM-DD HH:mm:ss
     */
    formatDate(date) {
        if (!date) return '';
        const d = new Date(date);
        const year = d.getFullYear();
        const month = String(d.getMonth() + 1).padStart(2, '0');
        const day = String(d.getDate()).padStart(2, '0');
        const hours = String(d.getHours()).padStart(2, '0');
        const minutes = String(d.getMinutes()).padStart(2, '0');
        const seconds = String(d.getSeconds()).padStart(2, '0');
        return `${year}-${month}-${day} ${hours}:${minutes}:${seconds}`;
    }

    /**
     * 返回到客户端的数据格式
     */
    getDataFormat(data) {
        return {
            // 前四个字段固定
            hwnd: data.hwnd,
            name: data.name,
            status: data.status,
            lastUpdate: this.formatDate(data.lastUpdate),
            // 其他字段根据实际情况添加,但是不能影响前四个字段
            action: data.action
        }
    }
}

module.exports = WorkerManager;

