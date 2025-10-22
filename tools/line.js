class ActionChain {
    constructor() {
        this.actions = [];
        this.context = {}; // 用于在动作之间传递数据
    }

    click(selector, options = {}) {
        this.actions.push(async () => {
            // 实现点击逻辑，selector可以是图像模板、坐标等
            const position = await findImage(selector);
            if (!position) {
                throw new Error(`未找到图像: ${selector}`);
            }
            await click(position.x, position.y);
            if (options.delayAfter) {
                await delay(options.delayAfter);
            }
        });
        return this;
    }

    sleep(ms) {
        this.actions.push(async () => {
            await delay(ms);
        });
        return this;
    }

    // 条件分支：如果条件满足，执行thenChain，否则执行elseChain
    condition(conditionFn, thenChain, elseChain) {
        this.actions.push(async () => {
            if (await conditionFn(this.context)) {
                await thenChain.execute(this.context);
            } else if (elseChain) {
                await elseChain.execute(this.context);
            }
        });
        return this;
    }

    // 循环执行某个子链，直到条件满足
    loop(conditionFn, chain) {
        this.actions.push(async () => {
            while (!(await conditionFn(this.context))) {
                await chain.execute(this.context);
            }
        });
        return this;
    }

    // 执行链
    async execute(context = {}) {
        this.context = context;
        for (const action of this.actions) {
            await action();
        }
    }
}

