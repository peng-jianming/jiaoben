const express = require('express');
const bodyParser = require('body-parser');
const WebSocket = require('ws');
const { fork } = require('child_process');
const path = require('path');
const EventEmitter = require('events');
const { getList } = require('./touping');

const server = express();
const wss = new WebSocket.Server({ port: 8081 });

// ==================== 子进程管理器 ====================
class WorkerManager extends EventEmitter {
    constructor() {
        super();
        this.workers = new Map(); // { hwnd: { process, hwnd, name, status, ... } }
    }

    initWorker(hwnd, name) {
        if (this.workers.has(hwnd)) return this.workers.get(hwnd);

        const worker = fork(path.join(__dirname, 'worker.js'));
        const workerInfo = {
            process: worker,
            hwnd,
            name,
            status: 'idle',
            lastUpdate: Date.now()
        };

        this.workers.set(hwnd, workerInfo);

        worker.on('message', (message) => {
            if (message.type === 'status') {
                workerInfo.status = message.status;
            } else if (message.type === 'update') {
                Object.assign(workerInfo, message.data);
            }
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
            status: info.status || 'idle'
        }));
    }

    cleanup() {
        this.workers.forEach((info, hwnd) => {
            if (info.process) info.process.kill();
            this.workers.delete(hwnd);
        });
    }
}

// ==================== 消息处理器 ====================
class MessageHandler {
    constructor(workerManager) {
        this.workerManager = workerManager;
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
        const { deviceList, taskConfig } = message.data;
        if (!deviceList || !taskConfig) return;

        const hwnds = deviceList.map(device => device.hwnd);
        this.workerManager.sendToWorkers(hwnds, {
            type: 'start',
            taskConfig
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
        wss.clients.forEach((client) => {
            if (client.readyState === WebSocket.OPEN) {
                client.send(message);
            }
        });
    }
}

// 初始化管理器
const workerManager = new WorkerManager();
const messageHandler = new MessageHandler(workerManager);

server.use(bodyParser.json());
server.use(express.static('.')); // 提供静态文件服务

// 获取UI页面
server.get('/', (req, res) => {
  res.end(`
        <!DOCTYPE html>
        <html lang="en">
          <head><title>Hello</title></head>
          <link rel="stylesheet" href="./ui/elementUI.css">
          <body>
            <div id="app"></div>
          </body>
          <!-- 引入客户端脚本 -->
          <script src="./ui/vue.js"></script>
          <script type="module" src="./ui/elementUI.js"></script>
          <script type="module" src="./ui/index.js"></script>
        </html>
      `);
});


// WebSocket连接处理
wss.on('connection', (ws) => {
  console.log('新的 WebSocket 连接');

  ws.on('message', (message) => {
    try {
      const data = JSON.parse(message);
      messageHandler.handleClientMessage(data);
    } catch (error) {
      console.error('处理 WebSocket 消息失败:', error);
      ws.send(JSON.stringify({
        type: 'error',
        message: '消息格式错误'
      }));
    }
  });

  ws.on('close', () => {
    console.log('WebSocket 连接关闭');
  });

  ws.on('error', (error) => {
    console.error('WebSocket 错误:', error);
  });
});

// 优雅关闭
process.on('SIGINT', () => {
  console.log('\n正在关闭服务器...');
  workerManager.cleanup();
  wss.close(() => {
    process.exit(0);
  });
});

server.listen(8080, () => {
  console.log('服务器运行在端口 8080');
  console.log('WebSocket 服务器运行在端口 8081');
});
