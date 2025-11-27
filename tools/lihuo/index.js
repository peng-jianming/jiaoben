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

let isSetYoloPath = false

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

        const ret = new winax.Variant(-1, 'byref')
        lh.OcrDetectFile(
            ImgFile,
            Confidence,
            ret,
            50,                // Padding: 默认50
            1200,              // MaxSideLen: 默认1024
            0.5,               // BoxScoreThresh: 默认0.5
            0.3,               // BoxThresh: 固定0.3
            1.6,               // UnClipRatio: 默认1.6
            1,                 // DoAngle: 默认1启用
            1                  // MostAngle: 默认1启用
        );

        return parseOcrResult(ret.toString())

    },
    yolo(PicFilePath) {
        if (!isSetYoloPath) {
            const aaa = lh.Yolov8SetModelPath(path.join(__dirname, '../../resource/yolo/model'))
            if (aaa != 1) {
                return null
            }
            const bbb = lh.Yolov8SetParam(1, 'model.lh', '', 1, -1);
            if (bbb != 1) {
                return null
            }
            const ccc = lh.Yolov8InitModel('1')
            if (ccc != 1) {
                return null
            }

            isSetYoloPath = true
        }


        const ret1 = new winax.Variant(-1, 'byref')
        const ret2 = new winax.Variant(-1, 'byref')
        const ret = lh.Yolov8DetectFile('1', PicFilePath, 640, 640, 0.5, 1, ret1, ret2)
        if (ret > -1) {
            return ret1.toString().split('|').filter(item => !!item).map(item => {
                const arr = item.split(',');
                return {
                    id: arr[0],
                    x: arr[1],
                    y: arr[2],
                    w: arr[3],
                    h: arr[4],
                    sim: arr[5],
                    label: arr[6]

                }
            })
        } else {
            return null
        }
    }
}

function parseOcrResult(resultStr) {
    if (!resultStr || resultStr.trim() === '') {
        return [];
    }

    const results = [];
    // 按行分割（|huanhang|）
    const lines = resultStr.split('|huanhang|');

    for (const line of lines) {
        if (line.trim() === '') continue;

        // 按字段分割（|fenge|）
        const fields = line.split('|fenge|');

        if (fields.length >= 12) {
            const result = {
                text: fields[0], // 识别文本
                points: [fields[1], fields[2], fields[5], fields[6]],
                width: parseInt(fields[9]),     // 宽度
                height: parseInt(fields[10]),   // 高度
                confidence: parseFloat(fields[11]) // 置信度
            };
            results.push(result);
        }
    }

    return results;
}