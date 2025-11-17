# 项目重构总结

## 📋 重构目标

将项目整理成一个可复用的模板，让开发者专注于 **UI界面** 和 **任务逻辑** 之间的交互，核心框架代码保持不变。

## 🔄 重构内容

### 1. 创建核心框架目录 (`core/`)

将核心框架代码从主文件中提取出来，放入 `core/` 目录：

- **`core/WorkerManager.js`**: 子进程管理器
  - 管理所有工作进程的生命周期
  - 处理进程间通信
  - 提供状态查询接口

- **`core/MessageHandler.js`**: 消息处理器
  - 处理来自客户端的 WebSocket 消息
  - 处理设备初始化、任务启动、任务停止等操作
  - 广播状态更新到所有客户端

- **`core/TaskBase.js`**: 任务基类
  - 提供所有任务类的基础功能
  - 包含颜色匹配、屏幕操作、工具方法等
  - 所有任务类都应该继承此类

### 2. 重构任务基类

- **之前**: `task/index.js` 包含所有任务基类代码（390行）
- **现在**: 
  - `core/TaskBase.js`: 核心任务基类（所有通用方法）
  - `task/index.js`: 兼容性包装（保留 Mhxy 类名，仅10行）

### 3. 更新主入口文件

- **`index.js`**: 使用模块化的核心框架
  - 引入 `core/WorkerManager` 和 `core/MessageHandler`
  - 代码更简洁，职责更清晰

### 4. 保持向后兼容

- 保留 `task/index.js` 中的 `Mhxy` 类名
- 现有任务代码无需修改即可使用
- 所有方法调用保持不变

## 📁 新的目录结构

```
项目根目录/
├── core/                      # 核心框架代码（通常不需要修改）
│   ├── WorkerManager.js      # 子进程管理器
│   ├── MessageHandler.js     # 消息处理器
│   └── TaskBase.js           # 任务基类
├── task/                      # 任务逻辑（需要扩展）
│   ├── TaskRegistry.js       # 任务注册表（自动发现任务）
│   ├── index.js              # 任务基类包装（兼容性）
│   ├── config.js             # 任务配置
│   ├── demo.js               # 示例任务
│   └── shimen.js             # 示例任务
├── ui/                        # UI界面（需要扩展）
│   ├── index.js              # UI入口
│   └── demoComponent.js      # UI组件示例
├── tools/                     # 工具函数
├── hotReload.js              # 热重载管理器
├── worker.js                 # 工作线程入口
├── index.js                  # 主服务器入口
└── PROJECT_TEMPLATE.md       # 项目模板使用指南
```

## ✅ 重构优势

### 1. 关注点分离

- **核心框架** (`core/`): 负责基础设施，通常不需要修改
- **任务逻辑** (`task/`): 专注于业务逻辑实现
- **UI界面** (`ui/`): 专注于用户交互

### 2. 易于维护

- 核心代码集中管理，修改影响范围可控
- 任务代码独立，互不干扰
- UI代码独立，易于替换

### 3. 易于扩展

- 添加新任务：只需在 `task/` 目录下创建文件
- 添加新UI：只需在 `ui/` 目录下创建组件
- 无需修改核心框架代码

### 4. 代码复用

- 核心框架可以在其他项目中复用
- 任务基类提供统一接口
- 工具函数可以跨项目使用

## 📝 使用指南

详细的使用指南请参考 [PROJECT_TEMPLATE.md](./PROJECT_TEMPLATE.md)

### 快速开始

1. **添加新任务**
   ```javascript
   // task/myTask.js
   const Mhxy = require('./index');
   class MyTask extends Mhxy {
       async start() {
           // 你的任务逻辑
       }
   }
   module.exports = MyTask;
   ```

2. **添加新UI组件**
   ```javascript
   // ui/myComponent.js
   export default {
       props: { deviceList: Array, socket: WebSocket },
       methods: {
           handleStart(deviceList) {
               this.socket.send(JSON.stringify({
                   type: 'start',
                   data: { deviceList, taskConfig: ['myTask'] }
               }));
           }
       },
       template: `...`
   };
   ```

## 🔍 代码变更对比

### 之前的结构

```
index.js (340行)
├── WorkerManager 类
├── MessageHandler 类
└── 服务器启动代码

task/index.js (390行)
└── Mhxy 类（包含所有方法）
```

### 现在的结构

```
index.js (约200行)
└── 服务器启动代码 + 引入核心模块

core/
├── WorkerManager.js (约90行)
├── MessageHandler.js (约85行)
└── TaskBase.js (约400行)

task/index.js (约30行)
└── Mhxy 类（兼容性包装）
```

## ⚠️ 注意事项

1. **不要修改核心框架代码** (`core/` 目录)
   - 这些代码是框架的基础
   - 修改可能导致系统不稳定
   - 如果需要扩展功能，考虑在任务基类中添加

2. **保持向后兼容**
   - 现有任务代码无需修改
   - `Mhxy` 类名保持不变
   - 所有方法调用保持不变

3. **热重载支持**
   - 修改任务文件会自动重载
   - 修改核心框架需要重启服务器

## 🎉 总结

通过这次重构，项目结构更加清晰，职责分离更加明确。开发者可以专注于编写任务逻辑和UI界面，而不用担心底层框架的实现细节。

核心框架代码已经封装好，可以长期复用，即使之后编写其他任务，也可以直接复用代码，只需要修改任务和UI部分。

