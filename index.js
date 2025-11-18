const express = require('express');
const bodyParser = require('body-parser');
const WebSocket = require('ws');
const path = require('path');
const hotReload = require('./hotReload');

// 引入核心框架模块
const WorkerManager = require('./core/WorkerManager');

const app = express();
const httpServer = require('http').createServer(app);
const wss = new WebSocket.Server({ port: 8081 });

// 初始化管理器（合并了 MessageHandler 的功能）
const workerManager = new WorkerManager(wss);

// ==================== 热重载功能 ====================
// 监听根目录（用于 worker.js 和 index.js）
hotReload.watchDirectory(__dirname);
// 监听任务文件变化
hotReload.watchDirectory(path.join(__dirname, 'task'));
// 监听工具文件变化
hotReload.watchDirectory(path.join(__dirname, 'tools'));
// 监听 UI 文件变化
hotReload.watchDirectory(path.join(__dirname, 'ui'));

// 处理任务文件变化
hotReload.on('taskChanged', ({ taskName, filePath }) => {
    console.log(`\n🔄 任务文件已修改: ${taskName}`);
    console.log(`   文件路径: ${filePath}`);
    
    // 通知所有子进程重新加载该任务
    workerManager.workers.forEach((workerInfo, hwnd) => {
        try {
            workerManager.sendToWorker(hwnd, {
                type: 'reload',
                taskName
            });
        } catch (error) {
            console.error(`通知 Worker ${hwnd} 重载失败:`, error.message);
        }
    });
});

// 处理工具文件变化
hotReload.on('toolChanged', ({ filePath, filename }) => {
    console.log(`\n🔄 工具文件已修改: ${filename}`);
    console.log(`   文件路径: ${filePath}`);
    
    // 清除工具文件的 require 缓存（使用标准化路径）
    const normalizedPath = path.resolve(filePath);
    if (require.cache[normalizedPath]) {
        delete require.cache[normalizedPath];
        console.log(`✓ 已清除工具文件缓存: ${filename}`);
    }
    
    // 通知所有子进程重新加载所有任务（因为工具文件可能被多个任务使用）
    workerManager.workers.forEach((workerInfo, hwnd) => {
        try {
            workerManager.sendToWorker(hwnd, {
                type: 'reload',
                taskName: 'all' // 'all' 表示重新加载所有任务
            });
        } catch (error) {
            console.error(`通知 Worker ${hwnd} 重载失败:`, error.message);
        }
    });
});

// 处理 worker.js 变化（需要重启子进程）
hotReload.on('workerChanged', ({ filePath }) => {
    console.log(`\n⚠ worker.js 已修改，重启所有子进程，需要手动刷新页面`);
    console.log(`   文件路径: ${filePath}`);
    
    // 停止所有子进程
    workerManager.cleanup();
    
    // 重新初始化所有设备
    workerManager.handleInit();
});

// 处理 UI 文件变化
hotReload.on('uiChanged', ({ filePath, filename }) => {
    console.log(`\n🔄 UI 文件已修改: ${filename}`);
    console.log(`   文件路径: ${filePath}`);
});

app.use(bodyParser.json());
app.use(express.static('.')); // 提供静态文件服务

// 获取UI页面
app.get('/', (req, res) => {
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

  // 初始化或者获取设备列表给客户端
  workerManager.handleInit();

  ws.on('message', (message) => {
    try {
      const data = JSON.parse(message);
      console.log("收到来自客户端的信息:", data, );
      
      workerManager.handleClientMessage(data);
      
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
let isShuttingDown = false;

function gracefulShutdown() {
  if (isShuttingDown) return;
  isShuttingDown = true;
  
  console.log('\n正在关闭服务器...');
  
  // 1. 停止文件监听
  hotReload.stopAll();
  
  // 2. 停止所有子进程
  workerManager.cleanup();
  console.log('✓ 子进程已清理');
  
  // 3. 关闭所有 WebSocket 连接
  const clients = Array.from(wss.clients);
  if (clients.length > 0) {
    clients.forEach((client) => {
      if (client.readyState === WebSocket.OPEN || client.readyState === WebSocket.CONNECTING) {
        client.terminate(); // 强制关闭连接
      }
    });
  }
  
  // 4. 关闭 WebSocket 服务器
  wss.close(() => {
    console.log('✓ WebSocket 服务器已关闭');
  });
  
  // 5. 关闭 HTTP 服务器
  httpServer.close(() => {
    console.log('✓ HTTP 服务器已关闭');
    process.exit(0);
  });
  
  // 设置超时，强制退出（避免卡住）
  setTimeout(() => {
    console.log('⚠ 超时，强制退出...');
    process.exit(1);
  }, 3000);
}

process.on('SIGINT', gracefulShutdown);
process.on('SIGTERM', gracefulShutdown);

httpServer.listen(8080, () => {
  console.log('服务器运行在端口 8080');
  console.log('WebSocket 服务器运行在端口 8081');
});
