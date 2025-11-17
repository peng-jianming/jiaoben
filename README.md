# 自动化任务框架

一个专注于 UI 界面和任务逻辑交互的自动化任务框架模板。

## ✨ 特性

- 🎯 **关注点分离**：核心框架、任务逻辑、UI 界面完全分离
- 🔄 **自动发现**：任务自动注册，无需手动配置
- 🔥 **热重载**：修改任务文件自动重载，无需重启
- 📡 **实时通信**：WebSocket 实时状态同步
- 🎨 **灵活扩展**：轻松添加新任务和 UI 组件

## 📁 项目结构

```
├── core/              # 核心框架（通常不需要修改）
├── task/              # 任务逻辑（需要扩展）
├── ui/                # UI界面（需要扩展）
├── tools/             # 工具函数
├── index.js           # 主服务器入口
└── worker.js          # 工作线程入口
```

## 🚀 快速开始

### 1. 安装依赖

```bash
npm install
```

### 2. 启动服务器

```bash
node index.js
```

### 3. 访问 UI

打开浏览器访问 `http://localhost:8080`

## 📝 添加新任务

在 `task/` 目录下创建任务文件：

```javascript
const Mhxy = require('./index');

class MyTask extends Mhxy {
    async start() {
        this.changeProp('status', 'running');
        // 你的任务逻辑
        this.changeProp('status', 'idle');
    }
}

module.exports = MyTask;
```

## 🎨 添加新 UI 组件

在 `ui/` 目录下创建组件文件，并在 `ui/index.js` 中注册。

## 📚 详细文档

查看 [PROJECT_TEMPLATE.md](./PROJECT_TEMPLATE.md) 了解详细的使用指南。

## 🔧 核心框架说明

核心框架代码位于 `core/` 目录，包括：

- **WorkerManager**: 管理所有工作进程
- **MessageHandler**: 处理 WebSocket 消息
- **TaskBase**: 任务基类，提供通用方法

这些代码通常不需要修改，专注于编写任务逻辑和 UI 界面即可。

## 📄 许可证

MIT

