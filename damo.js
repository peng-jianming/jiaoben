const winax = require('./winax')
const { execSync } = require('child_process')

// function getDM() {
//     try {
//         return new winax.Object('dm.dmsoft')
//     } catch (e) {
//         execSync(`regsvr32 ${path.resolve(__dirname, 'dm.dll')} /s`)
//         return new winax.Object('dm.dmsoft')
//     }
// }

function getDM() {
    try {
        return new winax.Object('dm.dmsoft');
    } catch (e) {
        try {
            execSync(`regsvr32 dm.dll`);
            return new winax.Object('dm.dmsoft');
        } catch (err) {
            console.error('注册dm.dll失败或创建对象失败:', err);
            return null;
        }
    }
}

const dm = getDM()

let mouseRange

function setMouseRange() {
    if (arguments.length === 4) mouseRange = Array.from(arguments)
    else mouseRange = undefined
}

module.exports = {
    dll: dm,
    reg(reg_code, ver_info) {
        return dm.Reg(reg_code, ver_info)
    },
    // 获取全局路径
    getPath() {
        return dm.GetPath()
    },
    // 设置全局路径,设置了此路径后,所有接口调用中,相关的文件都相对于此路径. 比如图片,字库等
    setPath(path) {
        return dm.SetPath(path)
    },
    // 是否弹出错误
    setErrorDisplay(flag) {
        return dm.SetShowErrorMsg(flag)
    },
    // 获取鼠标位置
    getCursorPos() {
        let x = new winax.Variant(-1, 'byref')
        let y = new winax.Variant(-1, 'byref')
        dm.GetCursorPos(x, y)
        return { x: Number(x), y: Number(y) }
    },
    // 获取指定按键的状态(前台,不是后台)
    getKeyState(keyCode) {
        return dm.GetKeyState(keyCode)
    },
    // 设置鼠标移动范围
    setMouseRange,
    moveTo(x, y) {
        if (mouseRange) {
            if (x < mouseRange[0]) x = mouseRange[0]
            else if (x > mouseRange[2]) x = mouseRange[2]
            if (y < mouseRange[1]) y = mouseRange[1]
            else if (y > mouseRange[3]) y = mouseRange[3]
        }
        return dm.MoveTo(x, y)
    },
    leftClick() {
        return dm.LeftClick()
    },
    leftDoubleClick() {
        return dm.LeftDoubleClick()
    },
    leftDown() {
        return dm.LeftDown()
    },
    leftUp() {
        return dm.LeftUp()
    },
    rightClick() {
        return dm.RightClick()
    },
    rightDown() {
        return dm.RightDown()
    },
    rightUp() {
        return dm.RightUp()
    },
    wheelDown() {
        return dm.WheelDown()
    },
    wheelUp() {
        return dm.WheelUp()
    },
    // 按下指定的虚拟键码
    keyPress(keyCode) {
        return dm.KeyPress(keyCode)
    },
    // 按住指定的虚拟键码
    keyDown(keyCode) {
        return dm.KeyDown(keyCode)
    },
    // 弹起来虚拟键
    keyUp(keyCode) {
        return dm.KeyUp(keyCode)
    },
    // 查找符合类名或者标题名的顶层可见窗口
    // class 字符串: 窗口类名，如果为空，则匹配所有. 这里的匹配是模糊匹配.
    // title 字符串: 窗口标题,如果为空，则匹配所有.这里的匹配是模糊匹配.
    // return 整形数表示的窗口句柄，没找到返回0
    findWindow(className, title, parentHWnd) {
        const hWnd = parentHWnd ? this.enumWindow(className, title, 3, parentHWnd)[0] : dm.FindWindow(className, title)
        if (hWnd) return hWnd
    },
    enumWindow(className, title, filter, parentHWnd = 0) {
        const wins = dm.EnumWindow(parentHWnd, title, className, filter)
        return wins.length > 0 ? wins.split(',').map(hWnd => Number(hWnd)) : []
    },
    // 获取给定窗口相关的窗口句柄
    getWindow(hWnd, flag) {
        return dm.GetWindow(hWnd, flag)
    },
    // 获取给定窗口位置
    getWindowRect(hWnd) {
        let x1 = new winax.Variant(-1, 'byref')
        let y1 = new winax.Variant(-1, 'byref')
        let x2 = new winax.Variant(-1, 'byref')
        let y2 = new winax.Variant(-1, 'byref')
        const ret = dm.GetWindowRect(hWnd, x1, y1, x2, y2)
        if (ret) {
            return {
                left: Number(x1),
                top: Number(y1),
                right: Number(x2),
                bottom: Number(y2),
            }
        }
    },
    // 获取给定窗口位置
    // getClientRect(hWnd) {
    //     let x1 = new winax.Variant(-1, 'byref')
    //     let y1 = new winax.Variant(-1, 'byref')
    //     let x2 = new winax.Variant(-1, 'byref')
    //     let y2 = new winax.Variant(-1, 'byref')
    //     const ret = dm.GetClientRect(hWnd, x1, y1, x2, y2)
    //     if (ret) {
    //         return {
    //             left: Number(x1),
    //             top: Number(y1),
    //             right: Number(x2),
    //             bottom: Number(y2),
    //         }
    //     }
    // },
    getPointWindow(x, y) {
        return dm.GetPointWindow(x, y)
    },
    getClientSize(hWnd) {
        let width = new winax.Variant(-1, 'byref')
        let height = new winax.Variant(-1, 'byref')
        const ret = dm.GetClientSize(hWnd, width, height)
        if (ret) {
            return {
                width: Number(width),
                height: Number(height)
            }
        }
    },
    moveWindow(hWnd, x, y) {
        return dm.MoveWindow(hWnd, x, y)
    },
    setWindowSize(hWnd, width, height) {
        return dm.SetWindowSize(hWnd, width, height)
    },
    setWindowState(hWnd, state) {
        return dm.SetWindowState(hWnd, state)
    },
    sendPaste(hWnd) {
        return dm.sendPaste(hWnd)
    },
    sendString(hWnd, content) {
        return dm.SendString(hWnd, content)
    },
    bindWindow(hWnd, display, mouse, keypad, mode) {
        return dm.BindWindow(hWnd, display, mouse, keypad, mode)
    },
    unBindWindow() {
        return dm.UnBindWindow()
    },
    capture(x1, y1, x2, y2, fileName) {
        return dm.Capture(x1, y1, x2, y2, fileName)
    },
    findPic(x1, y1, x2, y2, picName, deltaColor, sim, dir) {
        let x = new winax.Variant(-1, 'byref')
        let y = new winax.Variant(-1, 'byref')
        const index = dm.FindPic(x1, y1, x2, y2, picName, deltaColor, sim, dir, x, y)
        if (index !== -1) {
            return {
                x: Number(x),
                y: Number(y),
                index
            }
        }
    },
    findPicEx(x1, y1, x2, y2, picName, deltaColor, sim, dir) {
        const ret = dm.FindPicEx(x1, y1, x2, y2, picName, deltaColor, sim, dir)
        if (ret.length > 0) {
            return ret
                .split('|')
                .map((pic) => {
                    const [index, x, y] = pic.split(',')
                    return { index: Number(index), x: Number(x), y: Number(y) }
                })
        } else return []
    },
    getColor(x, y) {
        return dm.GetColor(x, y)
    },
    getColorNum(x1, y1, x2, y2, color, sim) {
        return dm.GetColorNum(x1, y1, x2, y2, color, sim)
    },
    getAveRGB(x1, y1, x2, y2) {
        return dm.GetAveRGB(x1, y1, x2, y2)
    },
    findColor(x1, y1, x2, y2, color, sim, dir) {
        let x = new winax.Variant(-1, 'byref')
        let y = new winax.Variant(-1, 'byref')
        const ret = dm.FindColor(x1, y1, x2, y2, color, sim, dir, x, y)
        if (ret) {
            return {
                x: Number(x),
                y: Number(y)
            }
        }
    },
    getNowDict() {
        return dm.GetNowDict()
    },
    setDict(index, file) {
        return dm.SetDict(index, file)
    },
    findStr(x1, y1, x2, y2, str, colorFormat, sim) {
        let x = new winax.Variant(-1, 'byref')
        let y = new winax.Variant(-1, 'byref')
        const index = dm.FindStr(x1, y1, x2, y2, str, colorFormat, sim, x, y)
        if (index !== -1) {
            return {
                index,
                x: Number(x),
                y: Number(y)
            }
        }
    },
    ocr(x1, y1, x2, y2, colorFormat, sim) {
        return dm.Ocr(x1, y1, x2, y2, colorFormat, sim)
    },
    getScreenSize() {
        return {
            width: dm.GetScreenWidth(),
            height: dm.GetScreenHeight()
        }
    },
}