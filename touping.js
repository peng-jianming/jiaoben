const WebSocket = require('ws');

// 连接到上游 WebSocket 服务（ws://127.0.0.1:33332）
let upstreamWS = null;

let currentResolve = () => { }

function connectUpstream() {
    upstreamWS = new WebSocket('ws://127.0.0.1:33332');

    upstreamWS.on('message', (data) => {
        const res = JSON.parse(data)
        console.log("极限投屏返回值", res);
        
        currentResolve(res.result)
    });
}

connectUpstream()

const getList = () => {
    return new Promise(resolve => {
        upstreamWS.send('{ "action":"list" }')
        currentResolve = resolve
    })
}

const getScreen = (deviceIds) => {
    return new Promise(resolve => {
        const data = {
            "action": "screen",
            "comm": {
                "deviceIds": deviceIds,
                "savePath": __dirname + '/task',
                "onlyDeviceName": 1
            }
        }
        upstreamWS.send(JSON.stringify(data))
        currentResolve = resolve
    })
}

module.exports = {
    getList,
    getScreen
};