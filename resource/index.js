const path = require('path');

const 主界面活动按钮 = {
    标识: '主界面活动按钮',
    方式: '找图',
    图片路径: path.resolve(__dirname, 'common', `huodong.png`)
}

const 活动界面 = {
    标识: '活动界面',
    方式: '找图',
    图片路径: path.resolve(__dirname, 'common', `shimenjiemian.png`)
}

const 活动界面师门参加按钮 = {
    标识: '活动界面师门参加按钮',
    区域: { x: 1858, y: 222, x2: 111, y2: 2222 },
}

const 师门界面继续任务按钮 = {
    标识: '师门界面继续任务按钮',
    方式: '找图',
    图片路径: path.resolve(__dirname, 'shimen', `shimenjixurenwu.png`)
}

const 师门界面去完成按钮 = {
    标识: '师门界面去完成按钮',
    方式: '找图',
    图片路径: path.resolve(__dirname, 'shimen', `shimenquwancheng.png`)
}

const 师门界面 = {
    标识: '师门界面',
    方式: '找图',
    图片路径: path.resolve(__dirname, 'shimen', `shimenjiemian.png`)
}

module.exports = {
    主界面活动按钮,
    活动界面,
    师门界面继续任务按钮,
    师门界面去完成按钮,
    师门界面,
    活动界面师门参加按钮
}

