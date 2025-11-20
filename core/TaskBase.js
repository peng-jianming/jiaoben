const { 多点关联颜色匹配, 多点颜色匹配 } = require('../tools/colorMatching.js')
const { getScreen, 屏幕控制, 调用ADB } = require('../touping.js')
const Jimp = require('jimp')

/**
 * 任务基类
 * 所有任务类都应该继承此类
 * 提供通用的屏幕操作、颜色匹配、图片匹配等功能
 */
class TaskBase {
    flag = true

    stop() {
        this.flag = false;
    }

    async start() {
        throw new Error('子类必须实现start()方法');
    }

    随机区间位置(start, end) {
        return Math.floor(Math.random() * (end - start)) + start;
    }

    随机坐标(x1, y1, x2, y2) {
        return {
            x: this.随机区间位置(x1, x2),
            y: this.随机区间位置(y1, y2)
        }
    }

    随机区间时间(startMs, endMs) {
        // 确保 startMs <= endMs
        if (startMs > endMs) {
            [startMs, endMs] = [endMs, startMs];
        }
        // 生成包含两端点的随机整数
        return Math.floor(Math.random() * (endMs - startMs + 1)) + startMs;
    }

    延时(time) {
        return new Promise((resolve) => {
            if (!this.flag) {
                resolve();
                return;
            }

            const startTime = Date.now();
            const interval = 50; // 每50ms检查一次flag

            const checkInterval = setInterval(() => {
                if (!this.flag) {
                    clearInterval(checkInterval);
                    clearTimeout(timeout);
                    resolve();
                    return;
                }

                // 如果已经过了指定时间，也结束
                if (Date.now() - startTime >= time) {
                    clearInterval(checkInterval);
                    resolve();
                }
            }, interval);

            const timeout = setTimeout(() => {
                clearInterval(checkInterval);
                resolve();
            }, time);
        });
    }

    随机延时(startMs, endMs) {
        return this.延时(this.随机区间时间(startMs, endMs));
    }

    async 获取图片宽高(imagePath) {
        try {
            const image = await Jimp.Jimp.read(imagePath);
            return {
                width: image.bitmap.width,
                height: image.bitmap.height
            };
        } catch (error) {
            console.error('读取图片失败:', error);
            throw error;
        }
    }

    async 左键点击(result) {
        if (result) {
            await 屏幕控制(global.hwnd, '0', String(result.x / this.width), String(result.y / this.height))
            await this.延时(1000)
            await 屏幕控制(global.hwnd, '2', String(result.x / this.width), String(result.y / this.height))
        } else {
            console.log('左键点击坐标为空');
        }
    }

    async 滑动(result1, result2) {
        if (result1 && result2) {
            function 获取贝塞尔曲线(qx, qy, zx, zy) {
                function 三次贝塞尔曲线计算(cp, t) {
                    // X轴计算
                    let cx = 3.0 * (cp[1].x - cp[0].x);
                    let bx = 3.0 * (cp[2].x - cp[1].x) - cx;
                    let ax = cp[3].x - cp[0].x - cx - bx;

                    // Y轴计算
                    let cy = 3.0 * (cp[1].y - cp[0].y);
                    let by = 3.0 * (cp[2].y - cp[1].y) - cy;
                    let ay = cp[3].y - cp[0].y - cy - by;

                    // 三次方计算
                    let tSquared = t * t;
                    let tCubed = tSquared * t;

                    return {
                        "x": (ax * tCubed) + (bx * tSquared) + (cx * t) + cp[0].x,
                        "y": (ay * tCubed) + (by * tSquared) + (cy * t) + cp[0].y
                    };
                }

                var arr = []
                // 生成4个控制点（起点、两个随机控制点、终点）
                var controlPoints = [
                    { "x": qx, "y": qy },  // 起点
                    {
                        "x": qx + (Math.random() * 240 - 120),
                        "y": qy + Math.random() * 100
                    },
                    {
                        "x": zx + (Math.random() * 240 - 120),
                        "y": zy + (Math.random() * 200 - 100)
                    },
                    { "x": zx, "y": zy }  // 终点
                ];

                // 生成贝塞尔曲线路径点 - 增大步长使滑动更连贯
                for (let t = 0; t <= 1; t += 0.15) {
                    let point = 三次贝塞尔曲线计算(controlPoints, t);
                    arr.push([parseInt(point.x), parseInt(point.y)]);
                }

                return arr;
            }

            // 生成人类化的延时模式
            function 生成人类延时模式(点数) {
                const 模式 = [];
                const 总时间 = 300 + Math.random() * 200; // 300-500ms总时间（更快的滑动）
                const 基础间隔 = 总时间 / 点数;

                // 人类滑动特点：开始慢，中间快，结束前慢
                for (let i = 0; i < 点数; i++) {
                    let 进度 = i / 点数;
                    let 延时倍数;

                    if (进度 < 0.2) {
                        // 开始阶段：稍慢
                        延时倍数 = 0.8 + Math.random() * 0.2;
                    } else if (进度 > 0.8) {
                        // 结束阶段：变慢
                        延时倍数 = 1.0 + Math.random() * 0.3;
                    } else {
                        // 中间阶段：快速且变化
                        延时倍数 = 0.4 + Math.random() * 0.3;
                    }

                    // 添加随机波动
                    const 随机波动 = (Math.random() - 0.5) * 0.2;
                    延时倍数 += 随机波动;

                    // 确保最小值
                    延时倍数 = Math.max(0.25, 延时倍数);

                    模式.push(基础间隔 * 延时倍数);
                }

                return 模式;
            }

            const arr = 获取贝塞尔曲线(result1.x, result1.y, result2.x, result2.y);

            try {
                // 生成延时模式
                const 延时模式 = 生成人类延时模式(arr.length - 1);

                // 使用ADB motionevent命令模拟完整滑动轨迹
                // 按下起点
                调用ADB(global.hwnd, `input motionevent DOWN ${arr[0][0]} ${arr[0][1]}`);

                // 初始按下后的小延迟
                await new Promise(resolve => setTimeout(resolve, 10 + Math.random() * 10));

                // 移动到各个中间点 - 不等待ADB响应，连续发送命令使滑动更连贯
                for (let i = 1; i < arr.length - 1; i++) {
                    const point = arr[i];
                    调用ADB(global.hwnd, `input motionevent MOVE ${point[0]} ${point[1]}`);
                    await new Promise(resolve => setTimeout(resolve, Math.max(5, 延时模式[i])));
                }

                // 移动到终点
                const lastPoint = arr[arr.length - 1];
                调用ADB(global.hwnd, `input motionevent MOVE ${lastPoint[0]} ${lastPoint[1]}`);

                // 抬起前的微小停顿
                await new Promise(resolve => setTimeout(resolve, 20 + Math.random() * 20));

                // 在终点抬起
                await 调用ADB(global.hwnd, `input motionevent UP ${lastPoint[0]} ${lastPoint[1]}`);

            } catch (error) {
                console.log('滑动过程中出错:', error);
            }

        } else {
            console.log('左键点击坐标为空');
        }
    }

    async ADB左键点击(result) {
        if (result) {
            // 按下
            await 调用ADB(global.hwnd, `input motionevent DOWN ${result.x} ${result.y}`)
            // 保持指定时长
            await this.延时(this.随机区间时间(300, 800))
            // 弹起
            await 调用ADB(global.hwnd, `input motionevent UP ${result.x} ${result.y}`)
        } else {
            console.log('左键点击坐标为空');
        }
    }

    async ADB滑动(result1, result2) {
        if (result1 && result2) {
            function 获取贝塞尔曲线(qx, qy, zx, zy) {
                function 三次贝塞尔曲线计算(cp, t) {
                    // X轴计算
                    let cx = 3.0 * (cp[1].x - cp[0].x);
                    let bx = 3.0 * (cp[2].x - cp[1].x) - cx;
                    let ax = cp[3].x - cp[0].x - cx - bx;

                    // Y轴计算
                    let cy = 3.0 * (cp[1].y - cp[0].y);
                    let by = 3.0 * (cp[2].y - cp[1].y) - cy;
                    let ay = cp[3].y - cp[0].y - cy - by;

                    // 三次方计算
                    let tSquared = t * t;
                    let tCubed = tSquared * t;

                    return {
                        "x": (ax * tCubed) + (bx * tSquared) + (cx * t) + cp[0].x,
                        "y": (ay * tCubed) + (by * tSquared) + (cy * t) + cp[0].y
                    };
                }

                var arr = []
                // 生成4个控制点（起点、两个随机控制点、终点）
                var controlPoints = [
                    { "x": qx, "y": qy },  // 起点
                    {
                        "x": qx + (Math.random() * 240 - 120),
                        "y": qy + Math.random() * 100
                    },
                    {
                        "x": zx + (Math.random() * 240 - 120),
                        "y": zy + (Math.random() * 200 - 100)
                    },
                    { "x": zx, "y": zy }  // 终点
                ];

                // 生成贝塞尔曲线路径点 - 增大步长使滑动更连贯
                for (let t = 0; t <= 1; t += 0.15) {
                    let point = 三次贝塞尔曲线计算(controlPoints, t);
                    arr.push([parseInt(point.x), parseInt(point.y)]);
                }

                return arr;
            }

            // 生成人类化的延时模式
            function 生成人类延时模式(点数) {
                const 模式 = [];
                const 总时间 = 300 + Math.random() * 200; // 300-500ms总时间（更快的滑动）
                const 基础间隔 = 总时间 / 点数;

                // 人类滑动特点：开始慢，中间快，结束前慢
                for (let i = 0; i < 点数; i++) {
                    let 进度 = i / 点数;
                    let 延时倍数;

                    if (进度 < 0.2) {
                        // 开始阶段：稍慢
                        延时倍数 = 0.8 + Math.random() * 0.2;
                    } else if (进度 > 0.8) {
                        // 结束阶段：变慢
                        延时倍数 = 1.0 + Math.random() * 0.3;
                    } else {
                        // 中间阶段：快速且变化
                        延时倍数 = 0.4 + Math.random() * 0.3;
                    }

                    // 添加随机波动
                    const 随机波动 = (Math.random() - 0.5) * 0.2;
                    延时倍数 += 随机波动;

                    // 确保最小值
                    延时倍数 = Math.max(0.25, 延时倍数);

                    模式.push(基础间隔 * 延时倍数);
                }

                return 模式;
            }

            const arr = 获取贝塞尔曲线(result1.x, result1.y, result2.x, result2.y);

            try {
                // 生成延时模式
                const 延时模式 = 生成人类延时模式(arr.length - 1);

                // 使用ADB motionevent命令模拟完整滑动轨迹
                // 按下起点
                await 调用ADB(global.hwnd, `input motionevent DOWN ${arr[0][0]} ${arr[0][1]}`);

                // 初始按下后的小延迟
                await new Promise(resolve => setTimeout(resolve, 10 + Math.random() * 10));

                // 移动到各个中间点 - 不等待ADB响应，连续发送命令使滑动更连贯
                for (let i = 1; i < arr.length - 1; i++) {
                    const point = arr[i];
                    await 调用ADB(global.hwnd, `input motionevent MOVE ${point[0]} ${point[1]}`);
                    await new Promise(resolve => setTimeout(resolve, Math.max(5, 延时模式[i])));
                }

                // 移动到终点
                const lastPoint = arr[arr.length - 1];
                await 调用ADB(global.hwnd, `input motionevent MOVE ${lastPoint[0]} ${lastPoint[1]}`);

                // 抬起前的微小停顿
                await new Promise(resolve => setTimeout(resolve, 20 + Math.random() * 20));

                // 在终点抬起
                await 调用ADB(global.hwnd, `input motionevent UP ${lastPoint[0]} ${lastPoint[1]}`);

            } catch (error) {
                console.log('ADB滑动过程中出错:', error);
            }

        } else {
            console.log('左键点击坐标为空');
        }
    }

    async 百分之十随机用户操作() {
        // 百分10的概率触发随机事件
        if (Math.random() < 0.1) {
            let x = this.随机区间位置(200, 800);
            let y = this.随机区间位置(800, 1800);
            console.log("🖱 触发随机事件 随机点击: (" + x + "," + y + ")");
            await this.ADB左键点击({ x, y })
            await this.延时(1000)
        }
    }
}

module.exports = TaskBase;

