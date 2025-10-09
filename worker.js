// const Demo = require('./task/demo')
// const Demo2 = require('./task/demo2')

const taskConfig = {
    demo: require('./task/demo'),
    demo2: require('./task/demo2')
}

let currentTask = null

process.on('message', async (message) => {

    const changeProp = (prop, value) => {
        process.send({
            hwnd: message.hwnd,
            prop,
            value
        });
    }

    switch (message.type) {
        case 'start':
            message.taskConfig.forEach(async task => {
                currentTask = new taskConfig[task](message.hwnd, changeProp)
                await currentTask.start()
            });
            break;
        case 'stop':
            currentTask.stop();
            break;
        default:
            console.log('未知消息类型:', message.type);
    }
});
