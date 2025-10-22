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
        
        // 新增：主动状态转移相关属性
        this._pendingTransition = null;
        this._isTransitioning = false;
        this._transitionPromise = null;
        this._transitionResolve = null;
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
        // 如果有待处理的状态转移，优先处理
        if (this._pendingTransition) {
            const targetState = this._pendingTransition;
            this._pendingTransition = null;
            
            console.log(`主动状态转移: ${this.lastState} -> ${targetState}`);
            
            if (this.onTickCallback) {
                await this.onTickCallback(targetState, this.lastState, isFirstTick);
            }
            
            this.lastState = targetState;
            this._stopRunner();
            this._stopTimeoutChecker();

            if (this.states[targetState]) {
                for (const [subscriber, timeout, tick] of this.states[targetState]) {
                    this._startTimeoutChecker(targetState, timeout);
                    if (tick === -Infinity) { 
                        await subscriber();
                        this._stopTimeoutChecker();
                    } else {
                        this._startRunner(subscriber, tick);
                    }
                }
            }
            
            // 解析转移承诺
            if (this._transitionResolve) {
                this._transitionResolve();
                this._transitionResolve = null;
                this._transitionPromise = null;
                this._isTransitioning = false;
            }
            
            return;
        }

        // 原有的状态检测逻辑
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

    // 新增：主动状态转移方法
    async transitionTo(targetState, timeout = 5000) {
        if (this._isTransitioning) {
            throw new Error('状态转移正在进行中，请等待完成');
        }

        this._isTransitioning = true;
        this._pendingTransition = targetState;
        
        // 创建转移承诺
        this._transitionPromise = new Promise((resolve, reject) => {
            this._transitionResolve = resolve;
            
            // 设置转移超时
            if (timeout > 0) {
                setTimeout(() => {
                    if (this._isTransitioning) {
                        this._isTransitioning = false;
                        this._pendingTransition = null;
                        this._transitionPromise = null;
                        this._transitionResolve = null;
                        reject(new Error(`状态转移超时: ${targetState}`));
                    }
                }, timeout);
            }
        });

        // 立即触发状态发布以处理转移
        await this._publish(false);
        
        return this._transitionPromise;
    }

    // 新增：强制状态转移（不等待处理完成）
    forceTransitionTo(targetState) {
        this._pendingTransition = targetState;
        this._isTransitioning = false;
        this._transitionPromise = null;
        this._transitionResolve = null;
        
        // 立即触发状态发布
        this._publish(false).catch(error => {
            console.error('强制状态转移失败:', error);
        });
    }

    // 新增：获取当前状态
    getCurrentState() {
        return this.lastState;
    }

    // 新增：检查是否在特定状态
    isInState(state) {
        return this.lastState === state;
    }

    // 新增：等待特定状态
    async waitForState(targetState, timeout = 30000) {
        if (this.lastState === targetState) {
            return true;
        }

        return new Promise((resolve, reject) => {
            const timeoutId = setTimeout(() => {
                reject(new Error(`等待状态超时: ${targetState}`));
            }, timeout);

            const originalTickCallback = this.onTickCallback;
            this.onTickCallback = async (state, lastState, isFirstTick) => {
                if (originalTickCallback) {
                    await originalTickCallback(state, lastState, isFirstTick);
                }
                
                if (state === targetState) {
                    clearTimeout(timeoutId);
                    this.onTickCallback = originalTickCallback;
                    resolve(true);
                }
            };
        });
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
        
        // 清理转移相关状态
        this._isTransitioning = false;
        this._pendingTransition = null;
        if (this._transitionResolve) {
            this._transitionResolve();
            this._transitionResolve = null;
        }
        this._transitionPromise = null;
    }
}