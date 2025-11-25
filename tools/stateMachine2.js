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
        this.mainLoop = undefined;
        this.onErrorCallback = undefined;
        this._resolvePromise = undefined;
        this._startPromise = undefined;
        this.params = {}
    }

    _errorHandle(e) {
        this.stop();
        if (this.onErrorCallback) this.onErrorCallback(e);
        else throw e;
    }

    _stopRunner() {
        this.runners.forEach(runner => runner.stop());
        this.runners = [];
    }

    setParams(params) {
        this.params = params;
    }

    getParams() {
        return this.params;
    }

    async _publish() {
        const state = await this.publisher(this.setParams.bind(this));
        if (state) this.currentState = state

        if (this.states[this.currentState]) {
            this.lastState = this.currentState;
            const result = await this.states[this.currentState](this.lastState, this.currentState, this.getParams.bind(this));
            this.currentState = result || ''
        }
    }

    on(state, stateMachineOrSubscriber) {
        this.states[state] = stateMachineOrSubscriber;
        return this;
    }

    onError(callback) {
        if (this.onErrorCallback) throw new Error('Only one ErrorHandler allowed');
        this.onErrorCallback = callback;
        return this;
    }

    start(initState, tick = (Math.floor(Math.random() * 2001) + 1000)) {
        // 如果已经有 Promise 在等待，直接返回它
        if (this._startPromise) {
            return this._startPromise;
        }

        // 创建一个新的 Promise，只有在 stop 时才会 resolve
        this._startPromise = new Promise((resolve) => {
            this._resolvePromise = resolve;
        });
        if (initState) this.currentState = initState
        this.lastState = undefined;
        this.mainLoop = new PromiseInterval(tick);
        this.mainLoop.start(
            () => {
                return this._publish();
            },
            (e) => this._errorHandle(e)
        );

        return this._startPromise;
    }

    stop() {
        if (this.mainLoop) this.mainLoop.stop();
        this._stopRunner();

        // 如果有等待的 Promise，resolve 它
        if (this._resolvePromise) {
            this._resolvePromise();
            this._resolvePromise = undefined;
            this._startPromise = undefined;
        }
    }
}


