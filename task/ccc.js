const Jimp = require('jimp');
const cv = require('../opencv.js');
const path = require('path')
const { getScreen } = require('../touping.js')

/**
 * 将十六进制颜色转换为BGR对象（OpenCV顺序）
 * @param {string} hex - 十六进制颜色值
 * @returns {Object} BGR颜色对象
 */
function 将十六进制颜色转换为BGR对象(hex) {
    const result = /^#?([a-f\d]{2})([a-f\d]{2})([a-f\d]{2})$/i.exec(hex);
    if (result) {
        // RGB 转 BGR：交换红色和蓝色通道
        return {
            b: parseInt(result[3], 16), // 蓝色 (原RGB中的蓝色)
            g: parseInt(result[2], 16), // 绿色
            r: parseInt(result[1], 16)  // 红色 (原RGB中的红色)
        };
    }
    return { b: 0, g: 0, r: 0 };
}

/**
 * 获取指定像素点的颜色（BGR顺序）
 * @param {cv.Mat} mat - OpenCV Mat对象
 * @param {number} x - x坐标
 * @param {number} y - y坐标
 * @returns {Object} BGR颜色对象
 */
function 获取指定像素点的颜色(mat, x, y) {
    const pixel = mat.ucharPtr(y, x);
    return {
        b: pixel[0], // 蓝色
        g: pixel[1], // 绿色  
        r: pixel[2]  // 红色
    };
}

/**
 * 检查颜色是否匹配（考虑容差）
 * @param {Object} color1 - 颜色1 (BGR)
 * @param {Object} color2 - 颜色2 (BGR)
 * @param {number} tolerance - 容差值(相似度)
 * @returns {boolean} 是否匹配
 */
function 颜色匹配(color1, color2, tolerance) {
    if (tolerance > 1) tolerance = 1
    const num = 255 - (tolerance * 255)
    return Math.abs(color1.r - color2.r) <= num &&
        Math.abs(color1.g - color2.g) <= num &&
        Math.abs(color1.b - color2.b) <= num;
}

module.exports = async function 多点关联颜色匹配(colorPoints, imagePath, tolerance = 10, searchRegion = null) {
    try {
        // 1. 使用 Jimp 加载图片
        const image = await Jimp.Jimp.read(imagePath);
        const width = image.bitmap.width;
        const height = image.bitmap.height;

        // 2. 将 Jimp 图片转换为 OpenCV Mat
        const imageData = image.bitmap.data;
        const mat = new cv.Mat(height, width, cv.CV_8UC4);
        mat.data.set(imageData);

        // 3. 将 RGBA 转换为 BGR（OpenCV 使用 BGR 顺序）
        const bgrMat = new cv.Mat();
        cv.cvtColor(mat, bgrMat, cv.COLOR_RGBA2BGR);

        // 4. 计算颜色点之间的偏移量
        const basePoint = colorPoints[0];
        const offsets = colorPoints.map(point => ({
            color: 将十六进制颜色转换为BGR对象(point.颜色), // 注意：这里要转换为 BGR
            offsetX: point.x - basePoint.x,
            offsetY: point.y - basePoint.y
        }));

        // console.log('目标颜色 (BGR):', offsets[0].color);

        // 5. 确定搜索区域
        let startX = 0;
        let startY = 0;
        let endX = bgrMat.cols;
        let endY = bgrMat.rows;

        if (searchRegion) {
            // 验证搜索区域是否在图片范围内
            startX = Math.max(0, searchRegion.x);
            startY = Math.max(0, searchRegion.y);
            endX = Math.max(0, searchRegion.x2);
            endY = Math.max(0, searchRegion.y2);

            // console.log(`搜索区域: (${startX}, ${startY}) 到 (${endX}, ${endY})`);
        } else {
            // console.log(`搜索整个图片: (0, 0) 到 (${endX}, ${endY})`);
        }

        // 6. 遍历图片进行匹配
        for (let y = startY; y < endY; y++) {
            for (let x = startX; x < endX; x++) {
                // 检查基准点颜色是否匹配
                const baseColor = 获取指定像素点的颜色(bgrMat, x, y);

                if (颜色匹配(baseColor, offsets[0].color, tolerance)) {
                    // 检查其他点是否匹配
                    let allMatch = true;

                    for (let i = 1; i < offsets.length; i++) {
                        const checkX = x + offsets[i].offsetX;
                        const checkY = y + offsets[i].offsetY;

                        // 检查边界（考虑搜索区域）
                        if (checkX < startX || checkX >= endX ||
                            checkY < startY || checkY >= endY) {
                            allMatch = false;
                            break;
                        }

                        const checkColor = 获取指定像素点的颜色(bgrMat, checkX, checkY);
                        if (!颜色匹配(checkColor, offsets[i].color, tolerance)) {
                            allMatch = false;
                            break;
                        }
                    }

                    if (allMatch) {
                        // 调试信息
                        // const debugColor = 获取指定像素点的颜色(bgrMat, x, y);
                        // console.log(`找到匹配点: (${x}, ${y})`);
                        // console.log('实际颜色 (BGR):', debugColor);
                        // console.log('目标颜色 (BGR):', offsets[0].color);

                        // 清理内存并返回结果
                        mat.delete();
                        bgrMat.delete();
                        return { x, y };
                    }
                }
            }
        }

        // 7. 清理内存
        mat.delete();
        bgrMat.delete();

        return null;

    } catch (error) {
        console.error('颜色匹配错误:', error);
        return null;
    }
}


// async function main() {
//     const colorPoints = [
//         { 颜色: "#cb5ae2", x: 1995, y: 303 },
//         { 颜色: "#cf62eb", x: 1993, y: 301 },
//         { 颜色: "#db66f2", x: 1991, y: 298 },
//         { 颜色: "#da61f7", x: 1984, y: 297 },
//         { 颜色: "#e366f2", x: 1987, y: 292 },
//         { 颜色: "#c56fda", x: 1982, y: 289 },
//         { 颜色: "#bf6ad9", x: 1993, y: 289 },
//         { 颜色: "#d86bf4", x: 1987, y: 289 },
//         { 颜色: "#c06bda", x: 1987, y: 286 },
//         { 颜色: "#ca63df", x: 1982, y: 279 },
//         { 颜色: "#db66f4", x: 1991, y: 278 },
//         { 颜色: "#df65fe", x: 1972, y: 298 },
//         { 颜色: "#d85af8", x: 1968, y: 294 },
//         { 颜色: "#d165ee", x: 1968, y: 288 },
//         { 颜色: "#da64f9", x: 1969, y: 282 },
//         { 颜色: "#d06af3", x: 1970, y: 277 }
//     ];
//     const result = await multiPointColorMatch(
//         colorPoints,
//         path.resolve(__dirname, '../resource', `bbb.bmp`),
//         0.8,  // 容差
//         {
//             x: 1900,
//             y: 270,
//             x2: 2000,
//             y2: 310
//         }
//     );
//     console.log(result);

//     if (result) {
//         console.log(`找到匹配点: (${result.x}, ${result.y})`);
//     } else {
//         console.log('未找到匹配点');
//     }
// }
// setTimeout(() => {
//     main();
// }, 1000)




// // 测试函数
// async function debugColorAtPoint(imagePath, x, y) {
//     try {
//         const image = await Jimp.Jimp.read(imagePath);
//         const imageData = image.bitmap.data;
//         const mat = new cv.Mat(image.bitmap.height, image.bitmap.width, cv.CV_8UC4);
//         mat.data.set(imageData);

//         const bgrMat = new cv.Mat();
//         cv.cvtColor(mat, bgrMat, cv.COLOR_RGBA2BGR);

//         const color = 获取指定像素点的颜色(bgrMat, x, y);
//         console.log(`位置 (${x}, ${y}) 的颜色 (BGR):`, color);

//         mat.delete();
//         bgrMat.delete();
//     } catch (error) {
//         console.error('调试错误:', error);
//     }
// }
