const WebSocket = require('ws');

// 连接到上游 WebSocket 服务（ws://127.0.0.1:33332）
let upstreamWS = null;

let currentResolve = () => { }

function connectUpstream() {
    upstreamWS = new WebSocket('ws://127.0.0.1:33332');

    upstreamWS.on('open', () => {
        console.log('连接上极限投屏了！');
    });

    upstreamWS.on('message', (data) => {
        const res = JSON.parse(data)
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

module.exports = {
    getList
};