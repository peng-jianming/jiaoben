# 项目模板使用指南

本项目已经整理成一个可复用的模板，专注于 **UI界面** 和 **任务逻辑** 之间的交互。核心框架代码已经封装在 `core/` 目录中，通常不需要修改。

## 📁 项目结构

```
项目根目录/
├── core/                      # 核心框架代码（通常不需要修改）
│   ├── WorkerManager.js      # 子进程管理器
│   ├── MessageHandler.js     # 消息处理器
│   └── TaskBase.js           # 任务基类
├── task/                      # 任务逻辑（需要扩展）
│   ├── TaskRegistry.js       # 任务注册表（自动发现任务）
│   ├── index.js              # 任务基类包装（兼容性）
│   ├── config.js             # 任务配置（颜色匹配等）
│   ├── demo.js               # 示例任务
│   └── shimen.js             # 示例任务
├── ui/                        # UI界面（需要扩展）
│   ├── index.js              # UI入口（Vue根组件）
│   ├── demoComponent.js      # UI组件示例
│   ├── vue.js                # Vue框架
│   ├── elementUI.js          # Element UI框架
│   └── elementUI.css         # Element UI样式
├── tools/                     # 工具函数（可选扩展）
│   ├── colorMatching.js      # 颜色匹配工具
│   ├── imageMatching.js      # 图片匹配工具
│   └── stateMachine.js       # 状态机工具
├── hotReload.js              # 热重载管理器（核心框架）
├── worker.js                 # 工作线程入口（核心框架）
├── index.js                  # 主服务器入口（核心框架）
├── touping.js                # 设备管理（核心框架）
└── package.json              # 依赖配置
```

## 🎯 核心设计理念

### 1. 关注点分离

- **核心框架** (`core/`): 负责进程管理、消息通信、任务调度等基础设施
- **任务逻辑** (`task/`): 专注于具体的业务逻辑实现
- **UI界面** (`ui/`): 专注于用户交互和界面展示

### 2. 自动发现机制

- 任务自动发现：在 `task/` 目录下创建新的任务文件，系统会自动注册
- 无需手动配置：不需要修改核心代码即可添加新任务

### 3. 热重载支持

- 修改任务文件后自动重载，无需重启服务器
- 修改 UI 文件后自动刷新（需要手动刷新浏览器）

## 📝 如何添加新任务

### 步骤 1: 创建任务文件

在 `task/` 目录下创建新的任务文件，例如 `myTask.js`:

```javascript
const Mhxy = require('./index');  // 继承任务基类
const 配置 = require('./config.js');  // 可选：使用配置

class MyTask extends Mhxy {
    constructor(hwnd, changeProp, params = {}) {
        super(hwnd, changeProp, params);
    }
    
    async start() {
        // 更新状态
        this.changeProp('status', 'running');
        this.changeProp('message', '任务开始执行...');
        
        // 获取任务参数（如果UI传递了参数）
        const count = this.params.count || 1;
        const target = this.params.target || 'default';
        
        // 执行任务逻辑
        for (let i = 0; i < count; i++) {
            // 检查是否被停止
            if (!this.flag) {
                break;
            }
            
            // 使用基类提供的方法
            const result = await this.多点关联颜色匹配(配置.某个配置);
            if (result) {
                await this.ADB左键点击(result);
            }
            
            // 更新进度
            this.changeProp('progress', `${i + 1}/${count}`);
            
            // 延时
            await this.延时(1000);
        }
        
        // 任务完成
        this.changeProp('status', 'idle');
        this.changeProp('message', '任务完成');
    }
    
    stop() {
        // 可选：实现停止逻辑
        this.flag = false;
        this.changeProp('status', 'stopped');
    }
}

module.exports = MyTask;
```

### 步骤 2: 任务自动注册

任务文件创建后，系统会自动发现并注册，无需手动配置。

### 步骤 3: 在 UI 中调用任务

在 UI 组件中发送任务配置：

```javascript
// 简单任务（无参数）
socket.send(JSON.stringify({
    type: 'start',
    data: {
        deviceList: [{ hwnd: 'device1', name: '设备1' }],
        taskConfig: ['myTask']
    }
}));

// 带参数的任务
socket.send(JSON.stringify({
    type: 'start',
    data: {
        deviceList: [{ hwnd: 'device1', name: '设备1' }],
        taskConfig: [
            { name: 'myTask', params: { count: 10, target: 'boss' } }
        ]
    }
}));

// 循环执行任务
socket.send(JSON.stringify({
    type: 'start',
    data: {
        deviceList: [{ hwnd: 'device1', name: '设备1' }],
        taskConfig: ['myTask'],
        options: {
            loop: true,
            interval: 5000  // 每5秒执行一次
        }
    }
}));
```

## 🎨 如何添加新 UI 组件

### 步骤 1: 创建 UI 组件

在 `ui/` 目录下创建新的组件文件，例如 `myComponent.js`:

```javascript
export default {
    props: {
        deviceList: {
            type: Array,
            default() {
                return []
            }
        },
        socket: {
            type: WebSocket,
            default: null
        }
    },
    data() {
        return {
            taskConfig: ['myTask'],
            taskParams: {
                count: 1,
                target: 'default'
            }
        }
    },
    methods: {
        async handleStart(deviceList) {
            this.socket.send(JSON.stringify({
                type: 'start',
                data: {
                    deviceList: deviceList,
                    taskConfig: [
                        { name: 'myTask', params: this.taskParams }
                    ]
                }
            }));
        },
        async handleStop(deviceList) {
            this.socket.send(JSON.stringify({
                type: 'stop',
                data: {
                    deviceList: deviceList
                }
            }));
        }
    },
    template: `
    <div>
        <el-form :model="taskParams" label-width="100px">
            <el-form-item label="执行次数">
                <el-input-number v-model="taskParams.count" :min="1" :max="100"></el-input-number>
            </el-form-item>
            <el-form-item label="目标">
                <el-input v-model="taskParams.target"></el-input>
            </el-form-item>
        </el-form>
        <el-button @click="handleStart(deviceList)">开始任务</el-button>
        <el-button @click="handleStop(deviceList)">停止任务</el-button>
    </div>
    `
};
```

### 步骤 2: 在 UI 入口中注册组件

修改 `ui/index.js`:

```javascript
import demo from './demoComponent.js'
import myComponent from './myComponent.js'  // 导入新组件

new Vue({
    el: '#app',
    components: {
        demo,
        myComponent  // 注册新组件
    },
    data() {
        return {
            list: [],
            socket: null,
            currentComponent: 'demo'  // 当前显示的组件
        }
    },
    mounted() {
        this.socket = new WebSocket('ws://localhost:8081');
        // ... WebSocket 连接代码 ...
    },
    template: `
    <div>
        <el-tabs v-model="currentComponent">
            <el-tab-pane label="示例组件" name="demo">
                <demo :deviceList="list" :socket="socket"/>
            </el-tab-pane>
            <el-tab-pane label="我的组件" name="myComponent">
                <my-component :deviceList="list" :socket="socket"/>
            </el-tab-pane>
        </el-tabs>
    </div>
    `
});
```

## 🔧 任务基类提供的方法

所有任务类继承自 `TaskBase`，可以使用以下方法：

### 状态更新
- `this.changeProp(key, value)` - 更新状态并发送到 UI

### 工具方法
- `this.延时(time)` - 延时（毫秒）
- `this.随机区间位置(start, end)` - 生成随机位置
- `this.随机区间时间(startSec, endSec)` - 生成随机时间（毫秒）

### 颜色匹配
- `await this.多点关联颜色匹配(信息)` - 返回坐标 `{x, y}` 或 `null`
- `await this.多点颜色匹配(信息)` - 返回 `true` 或 `false`

### 屏幕操作
- `await this.左键点击(result)` - 点击坐标
- `await this.ADB左键点击(result)` - ADB 点击坐标
- `await this.滑动(result1, result2)` - 滑动操作
- `await this.ADB滑动(result1, result2)` - ADB 滑动操作
- `await this.百分之十随机用户操作()` - 随机操作（10% 概率）

### 任务控制
- `this.flag` - 任务运行标志（设置为 `false` 可停止任务）
- `this.params` - 任务参数（从 UI 传递）
- `this.hwnd` - 设备句柄
- `this.width` / `this.height` - 屏幕尺寸

## 📡 WebSocket 消息格式

### 客户端 → 服务器

#### 初始化设备列表
```javascript
{
    type: 'init'
}
```

#### 启动任务
```javascript
{
    type: 'start',
    data: {
        deviceList: [
            { hwnd: 'device1', name: '设备1' }
        ],
        taskConfig: ['task1', 'task2'],  // 或 [{ name: 'task1', params: {...} }]
        options: {  // 可选
            loop: true,
            interval: 5000
        }
    }
}
```

#### 停止任务
```javascript
{
    type: 'stop',
    data: {
        deviceList: [
            { hwnd: 'device1', name: '设备1' }
        ]
    }
}
```

### 服务器 → 客户端

#### 状态更新
```javascript
{
    type: 'update',
    data: [
        {
            hwnd: 'device1',
            name: '设备1',
            status: 'running'  // idle, running, stopped, error
        }
    ]
}
```

## 🚀 快速开始

1. **安装依赖**
   ```bash
   npm install
   ```

2. **启动服务器**
   ```bash
   node index.js
   ```

3. **访问 UI**
   打开浏览器访问 `http://localhost:8080`

4. **添加新任务**
   - 在 `task/` 目录下创建任务文件
   - 继承 `Mhxy` 基类
   - 实现 `start()` 方法

5. **添加新 UI**
   - 在 `ui/` 目录下创建组件文件
   - 在 `ui/index.js` 中注册组件

## ⚠️ 注意事项

1. **不要修改核心框架代码** (`core/`, `index.js`, `worker.js`, `hotReload.js`)
   - 这些代码是框架的基础，修改可能导致系统不稳定
   - 如果需要扩展功能，考虑在任务基类或工具函数中添加

2. **任务文件命名规范**
   - 文件名应该使用驼峰命名（如 `myTask.js`）
   - 类名应该与文件名一致（首字母大写）

3. **任务类必须实现的方法**
   - `async start()` - 必须实现
   - `stop()` - 可选实现

4. **热重载限制**
   - 修改 `index.js` 或 `worker.js` 需要手动重启服务器
   - 修改任务文件会自动重载
   - 修改 UI 文件需要手动刷新浏览器

## 📚 更多示例

查看 `task/demo.js` 和 `task/shimen.js` 了解任务实现的示例。

查看 `ui/demoComponent.js` 了解 UI 组件实现的示例。

## 🎉 总结

通过这个模板，你可以：

✅ **专注于业务逻辑**：只需编写任务类和 UI 组件  
✅ **快速开发**：核心框架已经搭建好，开箱即用  
✅ **易于维护**：代码结构清晰，职责分离  
✅ **易于扩展**：添加新任务和 UI 组件非常简单  
✅ **热重载支持**：开发时无需频繁重启服务器  

开始编写你的第一个任务吧！🚀

