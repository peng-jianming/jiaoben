const { getList } = require('../touping');

/**
 * 消息处理器
 * 处理来自客户端的 WebSocket 消息
 */
class MessageHandler {
    constructor(workerManager, wss) {
        this.workerManager = workerManager;
        this.wss = wss;
        this.workerManager.on('update', () => {
            this.broadcast();
        });
    }

    handleClientMessage(message) {
        switch (message.type) {
            case 'init':
                this.handleInit();
                break;
            case 'start':
                this.handleStart(message);
                break;
            case 'stop':
                this.handleStop(message);
                break;
        }
    }

    async handleInit() {
        try {
            const deviceListRaw = await getList();
            
            const deviceList = deviceListRaw.map(item => ({
                hwnd: item.deviceId,
                name: item.name
            }));

            deviceList.forEach(device => {
                this.workerManager.initWorker(device.hwnd, device.name);
            });
            this.broadcast();
        } catch (error) {
            console.error('初始化设备列表失败:', error);
        }
    }

    handleStart(message) {
        const { deviceList, taskConfig, options } = message.data;
        if (!deviceList || !taskConfig) return;

        const hwnds = deviceList.map(device => device.hwnd);
        this.workerManager.sendToWorkers(hwnds, {
            type: 'start',
            taskConfig,
            options
        });
    }

    handleStop(message) {
        const { deviceList } = message.data;
        if (!deviceList) return;

        const hwnds = deviceList.map(device => device.hwnd);
        this.workerManager.sendToWorkers(hwnds, {
            type: 'stop'
        });
    }

    broadcast() {
        const message = JSON.stringify({
            type: 'update',
            data: this.workerManager.getStatusList()
        });
        this.wss.clients.forEach((client) => {
            if (client.readyState === require('ws').OPEN) {
                client.send(message);
            }
        });
    }
}

module.exports = MessageHandler;

