const WebSocket = require('ws');
const path = require('path');

// 连接到上游 WebSocket 服务（ws://127.0.0.1:33332）
let upstreamWS = null;

// 使用 null 来标识没有等待的请求，这样更容易判断
let getListcurrentResolve = null
let getScreencurrentResolve = null
let 屏幕控制currentResolve = null
let 调用ADBcurrentResolve = null

function connectUpstream() {
    upstreamWS = new WebSocket('ws://127.0.0.1:33332');


    upstreamWS.on('message', (data) => {
        try {
            const res = JSON.parse(data)
            // console.log("极限投屏返回值", res);
            if (res.StatusCode == 200) {
                // 根据响应的 action 类型来判断应该调用哪个 resolve
                // 尝试从响应中获取 action，如果没有则按顺序处理（FIFO）
                if (getListcurrentResolve) {
                    const resolve = getListcurrentResolve;
                    getListcurrentResolve = null;
                    resolve(res);
                } else if (getScreencurrentResolve) {
                    const resolve = getScreencurrentResolve;
                    getScreencurrentResolve = null;
                    resolve();
                } else if (屏幕控制currentResolve) {
                    const resolve = 屏幕控制currentResolve;
                    屏幕控制currentResolve = null;
                    resolve(res);
                } else if (调用ADBcurrentResolve) {
                    const resolve = 调用ADBcurrentResolve;
                    调用ADBcurrentResolve = null;
                    resolve();
                }
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
        getListcurrentResolve = (res) => {
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
        getScreencurrentResolve = () => {
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
        屏幕控制currentResolve = resolve
    })
}

const 调用ADB = async (deviceIds, command) => {
    return new Promise(resolve => {
        const data = {
            "action": "adb",
            "comm": {
                "deviceIds": deviceIds,
                "command": command
            }
        }

        upstreamWS.send(JSON.stringify(data))
        调用ADBcurrentResolve = () => {
            resolve()
        }
    })
}


module.exports = {
    getList,
    getScreen,
    屏幕控制,
    调用ADB
};

