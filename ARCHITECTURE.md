# 项目架构优化说明

## 优化内容

本次优化主要改进了子进程和 UI 界面的数据交互方式，使代码结构更清晰、更易理解。

## 主要改进

### 1. 模块化设计

将原来的单一文件重构为清晰的类结构：

- **index.js** - 主进程入口（包含 WorkerManager 和 MessageHandler 类）
- **worker.js** - 子进程入口（已优化）

### 2. 简化的消息流

**优化前：**
```
前端 → WebSocket → index.js (多个 if 判断) → 子进程 → index.js (Map 操作) → WebSocket → 前端
```

**优化后：**
```
前端 → WebSocket → MessageHandler.handleClientMessage() → WorkerManager → 子进程
                                                              ↓
前端 ← WebSocket ← MessageHandler.broadcast() ← WorkerManager (状态更新)
```

所有逻辑集中在 `index.js` 中，结构清晰，易于理解。

### 3. 清晰的状态管理

- 所有状态统一存储在 `WorkerManager` 中
- 使用事件系统自动同步状态到前端
- 状态更新逻辑集中，易于维护

### 4. 改进的错误处理

- 统一的错误处理机制
- 更详细的错误日志
- 优雅的进程关闭

## 使用方式

### 前端消息格式

#### 初始化设备列表
```javascript
socket.send(JSON.stringify({
    type: 'init'
}));
```

#### 启动任务
```javascript
socket.send(JSON.stringify({
    type: 'start',
    data: {
        deviceList: [
            { hwnd: 'device1', name: '设备1' }
        ],
        taskConfig: ['demo']  // 任务名称数组
    }
}));
```

#### 停止任务
```javascript
socket.send(JSON.stringify({
    type: 'stop',
    data: {
        deviceList: [
            { hwnd: 'device1', name: '设备1' }
        ]
    }
}));
```

### 前端接收消息

```javascript
socket.onmessage = (event) => {
    const message = JSON.parse(event.data);
    
    if (message.type === 'update') {
        // message.data 是设备状态列表
        // [{ hwnd, name, status, lastUpdate }]
        console.log('设备状态更新:', message.data);
    }
};
```

### 任务类编写

任务类需要继承 `task/index.js` 中的 `Mhxy` 类：

```javascript
const Mhxy = require('./index');

class MyTask extends Mhxy {
    constructor(hwnd, statusUpdater) {
        super(hwnd, statusUpdater);
    }
    
    async start() {
        // 更新状态
        this.changeProp('status', 'running');
        this.changeProp('message', '任务开始执行...');
        
        // 执行任务逻辑
        // ...
        
        // 更新状态
        this.changeProp('status', 'idle');
        this.changeProp('message', '任务完成');
    }
    
    stop() {
        this.flag = false;
        this.changeProp('status', 'stopped');
    }
}

module.exports = MyTask;
```

### 注册新任务

在 `worker.js` 中注册：

```javascript
const taskConfig = {
    demo: require('./task/demo'),
    myTask: require('./task/myTask'),  // 添加新任务
}
```

## 文件结构

```
项目根目录/
├── task/                    # 任务目录
│   ├── index.js            # 任务基类
│   ├── demo.js             # 示例任务
│   └── config.js           # 任务配置
├── ui/                      # 前端界面
│   └── index.js            # 前端入口
├── worker.js                # 子进程入口
├── index.js                 # 主进程入口（包含 WorkerManager 和 MessageHandler）
└── ARCHITECTURE.md          # 本文档
```

## 优势总结

1. **代码更清晰**：职责分离，每个模块只做一件事
2. **易于维护**：修改某个功能只需修改对应模块
3. **易于扩展**：添加新功能只需添加对应的方法
4. **更好的错误处理**：统一的错误处理机制
5. **更好的可读性**：代码结构清晰，注释完善

## 迁移指南

如果你有旧代码需要迁移：

1. **任务类**：无需修改，`changeProp` 函数接口保持不变
2. **前端代码**：消息格式保持不变，无需修改
3. **配置**：无需修改

所有优化都在后端完成，对前端和任务代码完全兼容。

