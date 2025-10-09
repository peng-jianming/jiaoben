const express = require('express');
const { fork } = require('child_process');
const path = require('path');
const bodyParser = require('body-parser')
const WebSocket = require('ws');
const dm = require('./damo.js')
const server = express();
const wss = new WebSocket.Server({ port: 8081 });

server.use(bodyParser.json());
server.use(express.static('.')); // 提供静态文件服务

// 获取UI页面
server.get('/', (req, res) => {
  res.end(`
        <!DOCTYPE html>
        <html lang="en">
          <head><title>Hello</title></head>
          <link rel="stylesheet" href="/elementUI.css">
          <body>
            <div id="app"></div>
          </body>
          <!-- 引入客户端脚本 -->
          <script src="/vue.js"></script>
          <script type="module" src="/elementUI.js"></script>
          <script type="module" src="./ui/index.js"></script>
        </html>
      `);
});

// 全局变量存储子进程列表
const dataList = new Map();

// WebSocket连接处理
wss.on('connection', (ws) => {

  ws.on('message', (message) => {

    // {
    //   type: 'init', // 类型
    //   data: { // 数据载体
    //     list: [], // 操作设备列表
    //     taskConfig: [] // 任务配置
    //   }
    // }
    const result = JSON.parse(message)

    if (result.type == 'init') {
      const deviceList = [
        { hwnd: 59247500, name: '记事本1' },
        { hwnd: 9769290, name: '记事本2' },
      ];

      // 记录还未记录的设备信息
      deviceList.forEach(device => {

        if (!dataList.has(device.hwnd)) {

          const worker = fork(path.join(__dirname, 'worker.js'));

          dataList.set(device.hwnd, {
            process: worker,
            hwnd: device.hwnd,
            name: device.name
          });

          worker.on('message', (message) => {
            const data = dataList.get(message.hwnd);
            data[message.prop] = message.value
            broadcastUpdate();
          });

          worker.on('exit', (code, signal) => {
            dataList.delete(device.hwnd);
            broadcastUpdate();
          });

          worker.on('error', (err) => {
            dataList.delete(device.hwnd);
            broadcastUpdate();
          });
        }

      });

      broadcastUpdate()
    }

    if (result.type == 'start') {
      result.data.list.forEach(device => {
        const data = dataList.get(device.hwnd);
        data.process.send({ type: 'start', data: result.data });
      });
    }

    if (result.type == 'stop') {
      result.data.list.forEach(device => {
        const data = dataList.get(device.hwnd);
        data.process.send({ type: 'stop', data: result.data });
      })
    }
  })
});

function broadcastUpdate() {

  const statusList = Array.from(dataList.values()).map(info => ({
    hwnd: info.hwnd,
    name: info.name,
    status: info.status
  }));

  const message = JSON.stringify({ type: 'update', data: statusList });

  wss.clients.forEach((client) => {
    if (client.readyState === WebSocket.OPEN) {
      client.send(message);
    }
  });

}

server.listen(8080, () => {
  console.log('Server running on port 8080');
});
