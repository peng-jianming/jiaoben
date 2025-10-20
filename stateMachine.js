const runningTasks = new Set();
const runningHandlers = new Map();
let id = 0;

function delay(ms) {
    return new Promise((resolve) => setTimeout(resolve, ms));
}

async function run(id, handler, interval = 0) {
    while (runningTasks.has(id)) {
        const startTime = Date.now();
        runningHandlers.set(id, handler());
        try {
            await runningHandlers.get(id);
        } finally {
            runningHandlers.delete(id);
        }
        await delay(interval - (Date.now() - startTime));
    }
}

async function clearPromiseInterval(intervalId) {
    if (typeof intervalId === 'number' && runningTasks.has(intervalId)) {
        if (runningHandlers.has(intervalId)) {
            await runningHandlers.get(intervalId);
        }
        runningTasks.delete(intervalId);
    }
}

function setPromiseInterval(handler, interval) {
    id += 1;
    runningTasks.add(id);
    run(id, handler, interval);
    return id;
}

class PromiseInterval {
    constructor(ms) {
        this.ms = ms;
        this.timer = undefined;
    }

    start(promiseFun, onError) {
        if (this.timer === undefined) {
            this.timer = setPromiseInterval(async () => {
                try {
                    await promiseFun();
                } catch (e) {
                    this.stop();
                    if (onError) onError(e);
                    else throw e;
                }
            }, this.ms);
        }
    }

    stop() {
        clearPromiseInterval(this.timer);
        this.timer = undefined;
    }
}

module.exports = class StateMachine {
    constructor(publisher) {
        this.publisher = publisher;
        this.lastState = undefined;
        this.states = {};
        this.runners = [];
        this.timeoutChecker = [];
        this.mainLoop = undefined;
        this.onTickCallback = undefined;
        this.onErrorCallback = undefined;
        this.onTimeoutCallback = undefined;
    }

    _errorHandle(e) {
        this.stop();
        if (this.onErrorCallback) this.onErrorCallback(e);
        else throw e;
    }

    _timeoutHandle(state) {
        this.stop();
        if (this.onTimeoutCallback) this.onTimeoutCallback(state);
        else throw new Error(`state: ${state} timeout`);
    }

    _startTimeoutChecker(state, ms) {
        if (ms) {
            this.timeoutChecker.push(setTimeout(() => this._timeoutHandle(state), ms));
        }
    }

    _stopTimeoutChecker() {
        this.timeoutChecker.forEach(clearTimeout);
        this.timeoutChecker = [];
    }

    _startRunner(stateMachineOrSubscriber, tick) {
        if (stateMachineOrSubscriber instanceof StateMachine) {
            try {
                stateMachineOrSubscriber.onError((e) => this._errorHandle(e));
            } catch (e) { }
            try {
                stateMachineOrSubscriber.onTimeout((state) => this.onTimeout(state));
            } catch (e) { }
            stateMachineOrSubscriber.start(tick);
            this.runners.push(stateMachineOrSubscriber);
        } else {
            const runner = new PromiseInterval(tick);
            runner.start(stateMachineOrSubscriber, (e) => this._errorHandle(e));
            this.runners.push(runner);
        }
    }

    _stopRunner() {
        this.runners.forEach(runner => runner.stop());
        this.runners = [];
    }

    async _publish(isFirstTick) {
        const state = await this.publisher();
        if (this.onTickCallback) {
            await this.onTickCallback(state, this.lastState, isFirstTick);
        }

        if (this.lastState !== state) {
            this.lastState = state;
            this._stopRunner();
            this._stopTimeoutChecker();

            if (this.states[state]) {
                for (const [subscriber, timeout, tick] of this.states[state]) {
                    this._startTimeoutChecker(state, timeout);
                    if (tick === -Infinity) {
                        await subscriber();
                        this._stopTimeoutChecker();
                    } else {
                        this._startRunner(subscriber, tick);
                    }
                }
            }
        }
    }

    on(state, stateMachineOrSubscriber, timeout = 0, tick = 200) {
        if (!this.states[state]) this.states[state] = [];
        this.states[state].push([stateMachineOrSubscriber, timeout, tick]);
        return this;
    }

    onTick(callback) {
        if (this.onTickCallback) throw new Error('Only one TickHandler allowed');
        this.onTickCallback = callback;
        return this;
    }

    onError(callback) {
        if (this.onErrorCallback) throw new Error('Only one ErrorHandler allowed');
        this.onErrorCallback = callback;
        return this;
    }

    onTimeout(callback) {
        if (this.onTimeoutCallback) throw new Error('Only one TimeoutHandler allowed');
        this.onTimeoutCallback = callback;
        return this;
    }

    start(tick = 200) {
        let isFirstTick = true;
        this.lastState = undefined;
        this.mainLoop = new PromiseInterval(tick);
        this.mainLoop.start(
            () => {
                const firstTickFlag = isFirstTick;
                if (isFirstTick) isFirstTick = false;
                return this._publish(firstTickFlag);
            },
            (e) => this._errorHandle(e)
        );
    }

    stop() {
        if (this.mainLoop) this.mainLoop.stop();
        this._stopRunner();
        this._stopTimeoutChecker();
    }
}



// let num = 0
// setInterval(() => {
//     // 模拟事件发生器 —— 随机生成数字 1-10
//     num = Math.ceil(Math.random() * 10)
// }, 1000)

// // 这是一个用于测试的始终返回 state1 的状态机
// const otherStateMachine = new StateMachine(() => 'state1').on('state1', () =>
//     console.log('otherStateMachine', 'state1'),
// )

// const stateMachine = new StateMachine(() => {
//     // 模拟十分之一概率的错误
//     if (num === 1) throw Error('发生错误了')
//     // 模拟十分之二的概率返回 state1
//     else if (num < 4) return 'state1'
//     // 模拟十分之二的概率返回 state2
//     else if (num < 6) return 'state2'
//     // 模拟十分之二的概率返回 state3
//     else if (num < 8) return 'state3'
//     // 模拟十分之一的概率返回 state4
//     else if (num === 8) return 'state4'
//     // 模拟剩下十分之二的概率返回 unknown
//     else return 'unknown'
// })
//     // 任意状态发生的时候都会触发 onTick
//     .onTick((state, lastState, isFirstTick) =>
//         console.log(state, lastState, isFirstTick),
//     )
//     // 当 state1 发生的时候，启动一个每 100 毫秒输出一次 'state1' 的定时任务（如果发生了其他事情，该定时任务会停止
//     .on('state1', () => console.log('state1'), 0, 100)
//     // 当 state2 发生的时候，启动一个每 200 毫秒（tick 默认 200）输出一次 'state2' 的定时任务。如果 state2 持续了超过 10 秒钟，则触发超时
//     .on('state2', () => console.log('state2'), 10 * 1000)
//     // 当 state3 发生的时候启动 otherStateMachine 状态机（如果没发生，该状态机会被自动停止
//     .on('state3', otherStateMachine, 0, 500)
//     // 当 state4 发生的时候，"阻塞"整个状态机（tick 被设置成了 -Infinity），直到 state4 的任务执行完毕后，状态机才会继续工作
//     .on(
//         'state4',
//         () => {
//             console.log('state4 开始了')
//             return new Promise((resolve) => {
//                 setTimeout(() => resolve(console.log('state4 结束了')), 3 * 1000)
//             })
//         },
//         0,
//         -Infinity,
//     )
//     // 如果超过了 2 秒钟啥事情都没有发生（unknown 状态），则触发超时
//     .on('unknown', () => console.log('没有事情发生...'), 2 * 1000)
//     // 捕获状态机执行期间的所有超时，当超时发生时状态机会终止
//     .onTimeout((state) => console.log(state, '超时了'))
//     // 捕获状态机执行期间的所有异常，当异常发生时状态机会终止
//     .onError((e) => console.log(e))


// // 启动状态机，让其每 500 毫秒检测一次状态（tick 默认 200）
// stateMachine.start(500)
// // stateMachine.stop() // 终止状态机