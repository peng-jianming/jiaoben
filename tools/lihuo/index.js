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

    lhFindPicMateFile(BigPicFile, SmallPicFile, sim = 0.8, X1 = 0, Y1 = 0, X2 = 0, Y2 = 0, MatchMode = 5) {
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

    setDict(index, TxtPath) {
        return lh.SetDict(index, TxtPath)
    },

    FindStrFile(PicFile, str, index, color_format, sim = 0.8, X1 = 0, Y1 = 0, X2 = 0, Y2 = 0) {
        const res = lh.UseDict(index)

        if (!res) {
            return null
        }

        const intX = new winax.Variant(-1, 'byref')
        const intY = new winax.Variant(-1, 'byref')
        console.log(PicFile, X1, Y1, X2, Y2, str, color_format, sim, "+++++++++++++");

        const ret = lh.FindStrFile(PicFile, X1, Y1, X2, Y2, str, color_format, sim, intX, intY)
        console.log({
            x: Number(intX),
            y: Number(intY)
        }, "--------------");

        if (ret == str) {
            return {
                x: Number(intX),
                y: Number(intY)
            }
        }
        return null
    },

    ocrInitModelFile() {
        lh.OcrInitModelFile(1, -1)
    },
    ocrDetectFile(ImgFile, Confidence = 0.8) {
        if (lh.OcrInitModelFile(1, -1, '', '', '', '') != 1) {
            return null
        }

        // 创建一个 Buffer 作为文本输出变量，koffi 会自动将其作为指针传递
        const Ret = Buffer.alloc(1024);
        const result = lh.OcrDetectFile(ImgFile, Confidence, Ret,
            50,                // Padding: 默认50
            1024,              // MaxSideLen: 默认1024
            0.5,               // BoxScoreThresh: 默认0.5
            0.3,               // BoxThresh: 固定0.3
            1.6,               // UnClipRatio: 默认1.6
            1,                 // DoAngle: 默认1启用
            1                  // MostAngle: 默认1启用
        );

        console.log(result, "================");

        // 将 Buffer 转换为字符串，找到第一个 null 字节的位置
        const nullIndex = Ret.indexOf(0);
        const text = nullIndex >= 0 ? Ret.slice(0, nullIndex).toString('utf8') : Ret.toString('utf8');
        return text;
    }
}