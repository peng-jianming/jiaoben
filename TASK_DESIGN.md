# 任务设计模式说明

## 当前设计模式

### 架构
```
UI 前端
  ↓ 发送任务配置
主进程 (index.js)
  ↓ 转发消息
子进程 (worker.js)
  ↓ 根据配置创建任务实例
任务类 (task/*.js)
  ↓ 继承基类
基类 (task/index.js) - 提供公共方法
```

### 设计模式：**策略模式 + 工厂模式**

- **策略模式**：每个任务类封装不同的执行策略
- **工厂模式**：通过任务注册表自动创建任务实例

## 优化后的设计

### 1. 自动任务发现

**之前**：需要在 `worker.js` 手动注册每个任务
```javascript
const taskConfig = {
    demo: require('./task/demo'),
    shimen: require('./task/shimen'),
    // 每添加一个任务都要手动添加
}
```

**现在**：自动发现并注册所有任务
```javascript
const taskRegistry = require('./task/TaskRegistry');
// 自动加载 task 目录下的所有任务
```

### 2. 支持任务参数

**之前**：任务无法接收参数
```javascript
currentTask = new taskConfig[taskName](hwnd, statusUpdater);
```

**现在**：支持传递参数
```javascript
// UI 端可以这样发送：
{
    type: 'start',
    data: {
        deviceList: [...],
        taskConfig: [
            'demo',  // 简单任务名
            { name: 'shimen', params: { count: 10 } }  // 带参数的任务
        ]
    }
}
```

### 3. 支持任务循环执行

**之前**：任务只能执行一次

**现在**：支持循环执行
```javascript
{
    type: 'start',
    data: {
        deviceList: [...],
        taskConfig: ['demo'],
        options: {
            loop: true,      // 是否循环
            interval: 3000   // 循环间隔（毫秒）
        }
    }
}
```

### 4. 任务类规范

每个任务类需要：
- 继承 `Mhxy` 基类
- 实现 `async start()` 方法
- 可选实现 `stop()` 方法

```javascript
const Mhxy = require('./index')

class MyTask extends Mhxy {
    async start() {
        // 任务逻辑
        // 可以通过 this.params 获取参数
        // 可以通过 this.changeProp(key, value) 更新状态
    }
    
    stop() {
        // 可选：停止任务逻辑
        this.flag = false;
    }
}

module.exports = MyTask
```

## 使用示例

### UI 端发送任务

```javascript
// 单次执行
socket.send(JSON.stringify({
    type: 'start',
    data: {
        deviceList: [{ hwnd: 'device1', name: '设备1' }],
        taskConfig: ['demo']
    }
}));

// 循环执行
socket.send(JSON.stringify({
    type: 'start',
    data: {
        deviceList: [{ hwnd: 'device1', name: '设备1' }],
        taskConfig: ['demo'],
        options: {
            loop: true,
            interval: 5000  // 每5秒执行一次
        }
    }
}));

// 带参数的任务
socket.send(JSON.stringify({
    type: 'start',
    data: {
        deviceList: [{ hwnd: 'device1', name: '设备1' }],
        taskConfig: [
            { name: 'shimen', params: { count: 10, autoSkip: true } }
        ]
    }
}));

// 多个任务顺序执行
socket.send(JSON.stringify({
    type: 'start',
    data: {
        deviceList: [{ hwnd: 'device1', name: '设备1' }],
        taskConfig: ['demo', 'shimen']
    }
}));
```

### 任务类中使用参数

```javascript
class Shimen extends Mhxy {
    async start() {
        const count = this.params.count || 1;  // 默认值
        const autoSkip = this.params.autoSkip || false;
        
        for (let i = 0; i < count; i++) {
            // 执行任务逻辑
            this.changeProp('progress', `${i + 1}/${count}`);
        }
    }
}
```

## 优势

1. **自动发现**：添加新任务无需修改 worker.js
2. **参数支持**：任务可以接收配置参数
3. **循环执行**：支持定时循环执行任务
4. **易于扩展**：只需创建新文件并继承基类
5. **代码清晰**：任务类职责单一，易于维护

## 对比其他设计模式

### 为什么不使用状态机模式？
- 状态机适合复杂的状态流转
- 当前任务相对独立，策略模式更合适
- 如果任务内部需要状态机（如 shimen.js），可以在任务内部使用

### 为什么不使用命令模式？
- 命令模式适合需要撤销、重做的场景
- 当前任务主要是执行，不需要撤销功能
- 策略模式更简洁直接

### 当前设计的适用场景
✅ 适合：任务相对独立，执行逻辑清晰
✅ 适合：需要灵活配置和参数传递
✅ 适合：需要循环执行或定时执行
✅ 适合：任务数量较多，需要自动管理

## 进一步优化建议

如果未来任务变得更复杂，可以考虑：

1. **任务链模式**：支持任务之间的依赖关系
2. **任务队列**：支持任务排队执行
3. **任务优先级**：支持任务优先级调度
4. **任务组合**：支持任务组合成复合任务

但对于当前需求，现有设计已经足够简洁和灵活。

