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
    reg(reg_code) {
        return lh.Reg(reg_code)
    },
    lhSetPath(path){
        return lh.LhSetPath(path)
    }
}