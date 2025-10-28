const Jimp = require('jimp');
const cv = require('../opencv.js');
const path = require('path')
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
    if (tolerance >= 1) {
        // 当tolerance=1时，允许完全匹配
        return color1.r === color2.r && 
               color1.g === color2.g && 
               color1.b === color2.b;
    }
    
    const num = 255 - (tolerance * 255);
    return Math.abs(color1.r - color2.r) <= num &&
           Math.abs(color1.g - color2.g) <= num &&
           Math.abs(color1.b - color2.b) <= num;
}



async function 多点关联颜色匹配(colorPoints, imagePath, tolerance = 0.9, searchRegion = null) {
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
                        // 清理内存并返回结果
                        mat.delete();
                        bgrMat.delete();
                        // return { x, y };
                        return 颜色点范围随机点(colorPoints);
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
// 17.12

/**
 * 多点颜色匹配 - 直接检查指定坐标点的颜色
 * @param {Array} colorPoints - 颜色点数组，格式: [{颜色: "#d468f3", x: 1981, y: 568}, ...]
 * @param {string} imagePath - 图片路径
 * @param {number} tolerance - 相似度，默认0.9
 * @param {Object} searchRegion - 搜索区域，格式: {x, y, x2, y2}
 * @returns {boolean} 所有颜色点是否匹配
 */
async function 多点颜色匹配(colorPoints, imagePath, tolerance = 0.9, searchRegion = null) {
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

        // 4. 确定搜索区域边界
        let startX = 0;
        let startY = 0;
        let endX = bgrMat.cols;
        let endY = bgrMat.rows;

        if (searchRegion) {
            startX = Math.max(0, searchRegion.x);
            startY = Math.max(0, searchRegion.y);
            endX = Math.min(bgrMat.cols, searchRegion.x2);
            endY = Math.min(bgrMat.rows, searchRegion.y2);
        }

        // 5. 检查每个颜色点
        let allPointsMatch = true;

        for (const point of colorPoints) {
            const x = point.x;
            const y = point.y;

            // 检查坐标是否在有效范围内
            if (x < startX || x >= endX || y < startY || y >= endY) {
                console.log(`坐标 (${x}, ${y}) 超出有效范围`);
                allPointsMatch = false;
                break;
            }

            // 检查坐标是否在图片范围内
            if (x < 0 || x >= bgrMat.cols || y < 0 || y >= bgrMat.rows) {
                console.log(`坐标 (${x}, ${y}) 超出图片范围`);
                allPointsMatch = false;
                break;
            }

            // 获取实际颜色和目标颜色
            const actualColor = 获取指定像素点的颜色(bgrMat, x, y);
            const targetColor = 将十六进制颜色转换为BGR对象(point.颜色);

            // 调试信息（可选）
            // console.log(`检查点 (${x}, ${y}):`);
            // console.log('  目标颜色:', targetColor);
            // console.log('  实际颜色:', actualColor);
            // console.log('  匹配结果:', 颜色匹配(actualColor, targetColor, tolerance));

            // 检查颜色是否匹配
            if (!颜色匹配(actualColor, targetColor, tolerance)) {
                // console.log(`坐标 (${x}, ${y}) 颜色不匹配`);
                allPointsMatch = false;
                break;
            }
        }

        // 6. 清理内存
        mat.delete();
        bgrMat.delete();

        return allPointsMatch;

    } catch (error) {
        console.error('多点颜色匹配错误:', error);
        return false;
    }
}

/**
 * 从颜色点坐标的外接矩形中返回一个随机点
 * @param {Array} colorPoints - 颜色点数组，格式: [{颜色: "#795c2b", x: 1229, y: 83}, ...]
 * @returns {{x:number,y:number}|null} 随机点坐标；若入参无效返回null
 */
function 颜色点范围随机点(colorPoints) {
    if (!Array.isArray(colorPoints) || colorPoints.length === 0) {
        return null;
    }

    let minX = Infinity;
    let minY = Infinity;
    let maxX = -Infinity;
    let maxY = -Infinity;

    for (const p of colorPoints) {
        if (p && typeof p.x === 'number' && typeof p.y === 'number') {
            if (p.x < minX) minX = p.x;
            if (p.y < minY) minY = p.y;
            if (p.x > maxX) maxX = p.x;
            if (p.y > maxY) maxY = p.y;
        }
    }

    if (!isFinite(minX) || !isFinite(minY) || !isFinite(maxX) || !isFinite(maxY)) {
        return null;
    }

    const randX = Math.floor(Math.random() * (maxX - minX + 1)) + minX;
    const randY = Math.floor(Math.random() * (maxY - minY + 1)) + minY;
    return { x: randX, y: randY };
}

// async function main() {
//     const colorPoints = [
//         { "颜色": "#d45af0", "x": 1988, "y": 290 },
//         { "颜色": "#ca55e3", "x": 1984, "y": 279 },
//         { "颜色": "#d95deb", "x": 2012, "y": 278 },
//         { "颜色": "#bb45d8", "x": 2010, "y": 289 },
//         { "颜色": "#c147dd", "x": 2019, "y": 289 },
//         { "颜色": "#da64f5", "x": 2024, "y": 293 },
//         { "颜色": "#d768e2", "x": 2029, "y": 303 },
//         { "颜色": "#c96dd7", "x": 2041, "y": 303 },
//         { "颜色": "#d36bde", "x": 2039, "y": 300 },
//         { "颜色": "#d269df", "x": 2039, "y": 293 },
//         { "颜色": "#c865d9", "x": 2047, "y": 298 },
//         { "颜色": "#d15ee0", "x": 2047, "y": 289 },
//         { "颜色": "#c964e6", "x": 2048, "y": 279 },
//         { "颜色": "#dc62fd", "x": 2054, "y": 280 },
//         { "颜色": "#d45aee", "x": 2060, "y": 288 },
//         { "颜色": "#dc62f6", "x": 2061, "y": 293 },
//         { "颜色": "#c967c4", "x": 2067, "y": 300 },
//         { "颜色": "#d965ec", "x": 2071, "y": 297 },
//         { "颜色": "#d963f4", "x": 2078, "y": 302 },
//         { "颜色": "#d25cef", "x": 2090, "y": 304 },
//         { "颜色": "#e15af5", "x": 2094, "y": 297 },
//         { "颜色": "#c564d9", "x": 2087, "y": 291 },
//         { "颜色": "#d460f9", "x": 1968, "y": 302 },
//         { "颜色": "#df65fe", "x": 1972, "y": 298 },
//         { "颜色": "#d65bf4", "x": 1970, "y": 278 }
//       ]
//     const result = await 多点颜色匹配(
//         colorPoints,
//         path.resolve(__dirname, '../resource', `bbb.bmp`),
//         0.8
//     );
//     console.log(result);
// }
// setTimeout(() => {
//     main();
// }, 1000)


module.exports = {
    多点关联颜色匹配,
    多点颜色匹配,
    颜色点范围随机点
}

