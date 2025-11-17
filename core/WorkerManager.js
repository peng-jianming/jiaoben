const { fork } = require('child_process');
const path = require('path');
const EventEmitter = require('events');

/**
 * 子进程管理器
 * 负责管理所有工作进程的生命周期和通信
 */
class WorkerManager extends EventEmitter {
    constructor() {
        super();
        this.workers = new Map(); // { hwnd: { process, hwnd, name, status, ... } }
    }

    initWorker(hwnd, name) {
        if (this.workers.has(hwnd)) return this.workers.get(hwnd);

        const worker = fork(path.join(__dirname, '../worker.js'));
        const workerInfo = {
            process: worker,
            hwnd,
            name,
            status: '空闲中',
            lastUpdate: Date.now()
        };

        this.workers.set(hwnd, workerInfo);

        worker.on('message', (message) => {
            console.log('收到来自子进程的信息:', message);
            Object.assign(workerInfo, message.data);
            workerInfo.lastUpdate = Date.now();
            this.emit('update');
        });

        worker.on('exit', () => {
            this.workers.delete(hwnd);
            this.emit('update');
        });

        worker.on('error', () => {
            this.workers.delete(hwnd);
            this.emit('update');
        });

        return workerInfo;
    }

    sendToWorker(hwnd, message) {
        const workerInfo = this.workers.get(hwnd);
        if (!workerInfo) throw new Error(`Worker ${hwnd} not found`);
        workerInfo.process.send({ ...message, hwnd });
    }

    sendToWorkers(hwnds, message) {
        hwnds.forEach(hwnd => {
            try {
                this.sendToWorker(hwnd, message);
            } catch (err) {
                console.error(`Failed to send message to ${hwnd}:`, err);
            }
        });
    }

    getStatusList() {
        return Array.from(this.workers.values()).map(info => ({
            hwnd: info.hwnd,
            name: info.name,
            status: info.status || '空闲中'
        }));
    }

    cleanup() {
        this.workers.forEach((info, hwnd) => {
            if (info.process) info.process.kill();
            this.workers.delete(hwnd);
        });
    }
}

module.exports = WorkerManager;

