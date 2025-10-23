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
                    if (this.timer !== undefined) {
                        this.stop();
                        if (onError) onError(e);
                        else throw e;
                    }
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
        this.publisher = publisher || (() => { });
        this.lastState = undefined;
        this.currentState = undefined;
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
        // this.stop();
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

    async _publish() {
        const state = await this.publisher();
        if (state) this.currentState = state

        if (this.states[this.currentState]) {
            this.lastState = this.currentState;
            this._stopTimeoutChecker();
            for (const [subscriber, timeout] of this.states[this.currentState]) {
                this._startTimeoutChecker(this.currentState, timeout);
                this.currentState = ''
                await subscriber();
            }
        }
    }
    // 默认timeout:0,没有超时限制,
    on(state, stateMachineOrSubscriber, timeout = 0) {
        if (!this.states[state]) this.states[state] = [];
        this.states[state].push([stateMachineOrSubscriber, timeout]);
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



     // 执行任务的开始，不管目前处于哪一步，都是从头开始
        // 出任何问题了，直接从头开始


        // 以轮询为主,轮询有返回的状态,不管注册函数里面是否有设置状态,都根据轮询返回的状态为主
        // 轮询无返回状态,注册函数里面设置了状态,那么就根据注册函数里面设置的状态跑
        // 轮询无返回状态,注册函数里面也没有设置状态,那么就会一直轮询,导致脚本卡住,函数里面有没有注册状态的情况,需要有超时时间
        // const otherStateMachine = new StateMachine(() => {
        //     console.log('111111111', otherStateMachine.currentState);
        //     // return otherStateMachine.currentState;
        //     // return '进入主页面'
        // })
        //     .on('进入主页面', async () => {
        //         console.log('点击活动按钮')
        //         // otherStateMachine.currentState = '进入活动界面'
        //         // await this.延时(5,0)
        //     })
        //     .on('进入活动界面', () => {
        //         console.log('点击师门按钮')
        //         otherStateMachine.currentState = '进入师门界面'
        //     })
        //     .on('进入师门界面', () => {
        //         console.log('点击开始任务')
        //         // otherStateMachine.currentState = '进入师门界面'
        //     })
        //     .on('做师门任务', async () => {
        //         console.log('正在做师门任务')
        //         const aaa = new StateMachine(() => {
        //             // 记录标记()
        //             // if (师门召唤兽购买上交) {
        //             //     return '师门召唤兽购买上交'
        //             // }
        //             // if (找到右侧师门) {
        //             //     return '找到右侧师门'
        //             // }
        //             // if (不动检测()) {
        //             //     return '卡住了'
        //             // }
        //             return '卡住了'
        //         })
        //             .on('找到右侧师门', async () => {
        //                 console.log('点击右侧师门');
        //             })
        //             .on('师门召唤兽购买上交', async () => {
        //                 console.log('购买召唤兽');
        //             })
        //             .on('卡住了', async () => {
        //                 aaa.stop()
        //                 console.log('卡住了');
        //                 otherStateMachine.currentState = '进入主页面'
        //             })


        //         aaa.start(1000)

        //     })
        //     .onTimeout((state) => {
        //         console.log(state, '超时了')
        //         otherStateMachine.currentState = '进入主页面'
        //     })

        // otherStateMachine.start(1000)
        // otherStateMachine.currentState = '进入主页面'