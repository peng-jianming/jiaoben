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
     * 处理来自客户端的 WebSocket 消息
     */
    handleClientMessage(message) {
        const { deviceList } = message;
        if (deviceList) {
            const hwnds = deviceList.map(device => device.hwnd);
            hwnds.forEach(hwnd => {
                try {
                    this.sendToWorker(hwnd, message);
                } catch (err) {
                    console.error(`Failed to send message to ${hwnd}:`, err);
                }
            });
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

