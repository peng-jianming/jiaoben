const WebSocket = require('ws');
const path = require('path');

// 通用请求函数：建立连接 -> 发送请求 -> 接收响应 -> 关闭连接
// 这种方式比长连接更稳定，避免了并发请求时的回调错乱问题
const sendRequest = (payload) => {
    return new Promise((resolve, reject) => {
        const ws = new WebSocket('ws://127.0.0.1:33332');

        ws.on('open', () => {
            try {
                ws.send(JSON.stringify(payload));
            } catch (e) {
                ws.close();
                reject(e);
            }
        });

        ws.on('message', (data) => {
            try {
                const res = JSON.parse(data);
                // console.log("极限投屏返回值", res);
                resolve(res);
            } catch (error) {
                console.log('极限投屏返回错误', error.message);
                resolve(null);
            } finally {
                ws.close();
            }
        });

        ws.on('error', (err) => {
            console.log('连接投屏出错了');
            reject(err);
            ws.close();
        });
    });
};

const getList = async () => {
    const data = {
        "action": "list"
    };
    const res = await sendRequest(data);
    if (res && res.result) {
        return JSON.parse(res.result);
    }
    return [];
}

const getScreen = async (deviceIds) => {
    const savePath = path.resolve(__dirname, 'resource/cache');
    const data = {
        "action": "screen",
        "comm": {
            "deviceIds": deviceIds,
            "savePath": savePath,
            "onlyDeviceName": 1
        }
    };

    const res = await sendRequest(data);
    // 无论返回值如何，只要请求结束，就认为截图已保存（或失败），返回预期路径
    return path.resolve(savePath, `${deviceIds.replace(/[.:]/g, '_')}.png`);
}

const 屏幕控制 = async (deviceIds, mask, x, y) => {
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
    };
    return await sendRequest(data);
}

const 调用ADB = async (deviceIds, command) => {
    const data = {
        "action": "adb",
        "comm": {
            "deviceIds": deviceIds,
            "command": command
        }
    };
    return await sendRequest(data);
}


module.exports = {
    getList,
    getScreen,
    屏幕控制,
    调用ADB
};
