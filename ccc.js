const path = require('path');
const fs = require('fs');

// 1) 免注册设置 DLL 路径（等效于 Python: ctypes 调用 SetDllPathW）
const koffi = require('koffi');

// 优先使用当前项目根目录下的 lh.dll
const dllPath = path.join(__dirname, 'lh.dll');
if (!fs.existsSync(dllPath)) {
    throw new Error(`未找到 lh.dll，期望路径: ${dllPath}`);
}

// 加载 DLL 并声明 SetDllPathW 接口
const lhLib = koffi.load(dllPath);
const SetDllPathW = lhLib.func('int SetDllPathW(const wchar_t* dllPath, int regFlag)');

const setPathRet = SetDllPathW(dllPath, 1);
console.log('免注册调用COM组件', setPathRet);

// 2) 创建 COM 对象（等效于 Python: win32com.client.Dispatch("lh.lhsoft")）
// 使用项目自带的 winax 封装（tools/winax）
const winax = require('./tools/winax');

function main() {
    try {
        const lh = global.ActiveXObject('lh.lhsoft');

        // 版本
        const ver = lh.Ver();
        console.log('插件版本号为:', ver);
        const ccc = lh.GetColor(100, 100);
        const aaa = lh.CmpColor(100, 100, 'cac4cc', 0.9);
        // const ddd = lh.MoveTo(100, 200);
        // console.log(ccc, aaa, ddd);
        const reg_ret = lh.Reg('pengjianming07da20d304e552776cf6a1c9f7eebb5a')
        console.log(reg_ret, "=========");
        



    } catch (err) {
        console.error('执行失败：', err);
    }
}

main();


