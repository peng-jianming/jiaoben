const WebSocket = require('ws');
const path = require('path');

// 连接到上游 WebSocket 服务（ws://127.0.0.1:33332）
let upstreamWS = null;

let currentResolve = () => { }

function connectUpstream() {
    upstreamWS = new WebSocket('ws://127.0.0.1:33332');


    upstreamWS.on('message', (data) => {
        try {
            const res = JSON.parse(data)
            // console.log("极限投屏返回值", res);
            if (res.StatusCode == 200) {
                currentResolve(res)
            }
        } catch (error) {
            console.log('极限投屏返回错误', error.message);
        }

    });
    upstreamWS.on('error', (data) => {
        console.log('连接投屏出错了');
    });
}

connectUpstream()

const getList = async () => {
    return new Promise(resolve => {
        const data = {
            "action": "list"
        }
        upstreamWS.send(JSON.stringify(data))
        currentResolve = (res) => {
            resolve(JSON.parse(res.result))
        }
    })
}

const getScreen = async (deviceIds) => {
    return new Promise(resolve => {
        const data = {
            "action": "screen",
            "comm": {
                "deviceIds": deviceIds,
                "savePath": path.resolve(__dirname, 'resource/cache'),
                "onlyDeviceName": 1
            }
        }

        upstreamWS.send(JSON.stringify(data))
        currentResolve = () => {
            resolve(path.resolve(__dirname, 'resource/cache', `${deviceIds.replace(/[.:]/g, '_')}.png`))
        }
    })
}
const 屏幕控制 = async (deviceIds, mask, x, y) => {
    return new Promise(resolve => {
        const data = {
            "action": "PointerEvent",
            "comm": {
                "deviceIds": deviceIds,
                "mask": mask,
                "x": x,
                "y": y,
                "endx": "0",
                "endy": "0",
                "delta": "0"
            }
        }

        upstreamWS.send(JSON.stringify(data))
        currentResolve = resolve
    })
}


module.exports = {
    getList,
    getScreen,
    屏幕控制
};

