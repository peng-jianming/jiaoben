const winax = require('./winax')
const koffi = require('koffi');
const path = require('path');

function getLH() {
    const dllPath = path.join(__dirname, 'lh.dll');

    // 加载 DLL 并声明 SetDllPathW 接口
    const lhLib = koffi.load(dllPath);
    const SetDllPathW = lhLib.func('int SetDllPathW(const wchar_t* dllPath, int regFlag)');

    const setPathRet = SetDllPathW(dllPath, 1);
    console.log('免注册调用COM组件', setPathRet);
    // 需要导入winax，才有global.ActiveXObject
    return global.ActiveXObject('lh.lhsoft');
}

const lh = getLH()

module.exports = {
    dll: lh,
    ver() {
        return lh.Ver()
    },
    reg(reg_code) {
        return lh.Reg(reg_code)
    },
    lhSetPath(path) {
        return lh.LhSetPath(path)
    },
    lhFindPicMateFile(BigPicFile, SmallPicFile, sim = 0.8, MatchMode = 5, X1 = 0, Y1 = 0, X2 = 0, Y2 = 0) {
        const X = new winax.Variant(-1, 'byref')
        const Y = new winax.Variant(-1, 'byref')
        const W = new winax.Variant(-1, 'byref')
        const H = new winax.Variant(-1, 'byref')
        const ret = lh.LhFindPicMateFile(BigPicFile, SmallPicFile, X1, Y1, X2, Y2, sim, MatchMode, X, Y, W, H)        
        if (ret == 1) {
            return {
                x: Number(X),
                y: Number(Y)
            }
        }
        return null
    },
}