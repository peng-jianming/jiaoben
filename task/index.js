const { 多点关联颜色匹配, 多点颜色匹配 } = require('../tools/colorMatching.js')
const { 图片匹配 } = require('../tools/imageMatching.js')
const { getScreen, 屏幕控制 } = require('../touping.js')
const 配置 = require('./config.js')

class Mhxy {
    width = 2400
    height = 1080
    flag = true
    hwnd = 0
    constructor(hwnd, changeProp) {
        this.hwnd = hwnd
        this.changeProp = changeProp;
    }
    stop() {
        this.flag = false;
    }
    start() { }

    // bind() {
    //     const res = dm.reg("xf30557fc317f617eead33dfc8de3bdd4ab9043", "x4lpdhpht2zgnl7")
    //     if (res == 1) {
    //         const res2 = dm.bindWindow(this.hwnd, 'dx2', 'windows', 'windows', 0)
    //         if (res2 != 1) {
    //             this.changeProp('status', `dm后台绑定失败: ${res}`);
    //             return false
    //         }
    //     } else {
    //         this.changeProp('status', `dm注册失败: ${res}`);
    //         return false
    //     }
    //     return true;
    // }

    // time延时, offsetTime上下浮动
    随机区间位置(start, end) {
        return Math.floor(Math.random() * (end - start)) + start;
    }
    随机区间时间(startSec, endSec) {
        return Math.floor(Math.random() * (endSec - startSec) * 1000) + startSec * 1000;
    }
    async 延时(time) {
        return new Promise(resolve => setTimeout(resolve, time));
    }

    // 返回找到的坐标{ x: 0, y: 0 }, 没找到返回null
    async 多点关联颜色匹配(信息) {
        const url = await getScreen(this.hwnd)
        return 多点关联颜色匹配(信息.特征, url, 信息.相似度, 信息.区域)
    }

    // 返回true or false
    async 多点颜色匹配(信息) {
        const url = await getScreen(this.hwnd)
        return 多点颜色匹配(信息.特征, url, 信息.相似度, 信息.区域)
    }

    async 左键点击(result) {
        if (result) {
            await 屏幕控制(this.hwnd, '0', String(result.x / this.width), String(result.y / this.height))
            await this.延时(this.随机区间时间(200, 1000))
            await 屏幕控制(this.hwnd, '2', String(result.x / this.width), String(result.y / this.height))
        } else {
            console.log('左键点击坐标为空');
        }
    }

    // async 滑动(result1, result2) {
    //     if (result1 && result2) {
    //         function 获取贝塞尔曲线(qx, qy, zx, zy) {
    //             function 三次贝塞尔曲线计算(cp, t) {
    //                 // X轴计算
    //                 let cx = 3.0 * (cp[1].x - cp[0].x);
    //                 let bx = 3.0 * (cp[2].x - cp[1].x) - cx;
    //                 let ax = cp[3].x - cp[0].x - cx - bx;

    //                 // Y轴计算
    //                 let cy = 3.0 * (cp[1].y - cp[0].y);
    //                 let by = 3.0 * (cp[2].y - cp[1].y) - cy;
    //                 let ay = cp[3].y - cp[0].y - cy - by;

    //                 // 三次方计算
    //                 let tSquared = t * t;
    //                 let tCubed = tSquared * t;

    //                 return {
    //                     "x": (ax * tCubed) + (bx * tSquared) + (cx * t) + cp[0].x,
    //                     "y": (ay * tCubed) + (by * tSquared) + (cy * t) + cp[0].y
    //                 };
    //             }
    //             var arr = []
    //             // 生成4个控制点（起点、两个随机控制点、终点）
    //             var controlPoints = [
    //                 { "x": qx, "y": qy },  // 起点
    //                 { "x": Math.random(qx - 120, qx + 120), "y": Math.random(qy, qy + 100) },  // 控制点1
    //                 { "x": Math.random(zx - 120, zx + 120), "y": Math.random(zy - 100, zy + 100) },  // 控制点2
    //                 { "x": zx, "y": zy }  // 终点
    //             ];

    //             // 生成贝塞尔曲线路径点
    //             for (let t = 0; t <= 1; t += 0.07) {  // 步长影响曲线平滑度
    //                 let point = 三次贝塞尔曲线计算(controlPoints, t);
    //                 arr.push([parseInt(point.x), parseInt(point.y)]);
    //             }

    //             return arr;
    //         }
    //         const arr = 获取贝塞尔曲线(result1.x, result1.y, result2.x, result2.y);
    //         // '0' 为按下, '1'为移动, '2'为弹起
    //         await 屏幕控制(this.hwnd, '0', String(result1.x / this.width), String(result1.y / this.height))
    //         await 屏幕控制(this.hwnd, '1', String(result2.x / this.width), String(result2.y / this.height))
    //         await 屏幕控制(this.hwnd, '2', String(result2.x / this.width), String(result2.y / this.height))
    //     } else {
    //         console.log('左键点击坐标为空');
    //     }
    // }

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

                // 生成贝塞尔曲线路径点
                for (let t = 0; t <= 1; t += 0.07) {
                    let point = 三次贝塞尔曲线计算(controlPoints, t);
                    arr.push([parseInt(point.x), parseInt(point.y)]);
                }

                return arr;
            }

            // 生成人类化的延时模式
            function 生成人类延时模式(点数) {
                const 模式 = [];
                const 总时间 = 800 + Math.random() * 400; // 800-1200ms总时间
                const 基础间隔 = 总时间 / 点数;

                // 人类滑动特点：开始慢，中间快，结束前慢
                for (let i = 0; i < 点数; i++) {
                    let 进度 = i / 点数;
                    let 延时倍数;

                    if (进度 < 0.2) {
                        // 开始阶段：稍慢
                        延时倍数 = 1.2 + Math.random() * 0.3;
                    } else if (进度 > 0.8) {
                        // 结束阶段：变慢
                        延时倍数 = 1.5 + Math.random() * 0.5;
                    } else {
                        // 中间阶段：快速且变化
                        延时倍数 = 0.8 + Math.random() * 0.4;
                    }

                    // 添加随机波动
                    const 随机波动 = (Math.random() - 0.5) * 0.3;
                    延时倍数 += 随机波动;

                    // 确保最小值
                    延时倍数 = Math.max(0.5, 延时倍数);

                    模式.push(基础间隔 * 延时倍数);
                }

                return 模式;
            }

            function 添加轨迹抖动(原始轨迹, 抖动强度 = 2) {
                return 原始轨迹.map((point, index) => {
                    // 起点和终点不抖动
                    if (index === 0 || index === 原始轨迹.length - 1) {
                        return point;
                    }

                    // 随机抖动
                    const 抖动X = (Math.random() - 0.5) * 抖动强度;
                    const 抖动Y = (Math.random() - 0.5) * 抖动强度;

                    return [
                        Math.round(point[0] + 抖动X),
                        Math.round(point[1] + 抖动Y)
                    ];
                });
            }

            const arr = 获取贝塞尔曲线(result1.x, result1.y, result2.x, result2.y);

            // const arr = 添加轨迹抖动(原始轨迹, 1.5); // 1.5像素的抖动强度
            try {
                // 生成延时模式
                // const 延时模式 = 生成人类延时模式(arr.length - 1);

                // 1. 按下起点
                await 屏幕控制(this.hwnd, '0',
                    String(result1.x / this.width),
                    String(result1.y / this.height));

                // 初始按下后的小延迟
                await new Promise(resolve => setTimeout(resolve, 800 + Math.random() * 50));
                    console.log(arr);
                    
                // 2. 按贝塞尔曲线路径移动，使用人类化延时
                for (let i = 1; i < arr.length - 1; i++) {
                    const point = arr[i];
                    await 屏幕控制(this.hwnd, '1',
                        String(point[0] / this.width),
                        String(point[1] / this.height));

                    // 使用预生成的人类化延时
                    // await new Promise(resolve => setTimeout(resolve, 延时模式[i]));
                }

                // 3. 移动到终点
                const lastPoint = arr[arr.length - 1];
                await 屏幕控制(this.hwnd, '1',
                    String(lastPoint[0] / this.width),
                    String(lastPoint[1] / this.height));

                // 抬起前的微小停顿（人类会稍微停顿再松开）
                await new Promise(resolve => setTimeout(resolve, 800 + Math.random() * 70));

                // 4. 在终点抬起
                await 屏幕控制(this.hwnd, '2',
                    String(lastPoint[0] / this.width),
                    String(lastPoint[1] / this.height));

            } catch (error) {
                console.log('滑动过程中出错:', error);
            }

        } else {
            console.log('左键点击坐标为空');
        }
    }


    百分之十随机用户操作() {
        // 百分10的概率触发随机事件
        if (Math.random() < 0.1) {
            let x = this.随机区间位置(200, 800);
            let y = this.随机区间位置(800, 1800);
            log("🖱 触发随机事件 随机点击: (" + x + "," + y + ")");
            this.左键点击({ x, y })
            this.延时(1000)
        }
    }


    // async 打开活动弹框() {
    //     const res1 = await this.多点关联颜色匹配(配置.活动按钮);
    //     if (res1) {
    //         console.log('打开活动弹框')
    //         await this.左键点击(res1);
    //         await this.延时(2, 0)
    //     }

    //     const res2 = await this.多点颜色匹配(配置.活动界面);
    //     if (res2) {
    //         console.log('处于活动弹框')
    //         const res3 = await this.多点颜色匹配(配置.日常活动激活状态);
    //         if (res3) {
    //             console.log('活动已归位')
    //         } else {
    //             console.log('归位活动')
    //             await this.左键点击({ x: 574, y: 171 });
    //         }
    //     } else {
    //         this.打开活动弹框();
    //     }
    // }

}

module.exports = Mhxy







