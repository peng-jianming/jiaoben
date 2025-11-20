const path = require('path');
const Field = require('../tools/Field.js')

const 主界面活动按钮 =  new Field({
    标识: '主界面活动按钮',
    方式: '找图',
    图片路径: path.resolve(__dirname, 'common', `huodong.png`)
})

const 活动界面 = new Field({
    标识: '活动界面',
    方式: '找图',
    图片路径: path.resolve(__dirname, 'common', `huodongjiemian.png`)
})

const 活动界面师门任务 = new Field({
    标识: '活动界面师门任务',
    方式: '找图',
    图片路径: path.resolve(__dirname, 'shimen', `shimenrenwu.png`),
})

const 活动界面参加按钮 = new Field({
    标识: '活动界面参加按钮',
    方式: '找图',
    图片路径: path.resolve(__dirname, 'shimen', `canjia.png`),
})

const 师门界面 = new Field({
    标识: '师门界面',
    方式: '找图',
    图片路径: path.resolve(__dirname, 'shimen', `shimenjiemian.png`)
})

const 师门界面继续任务按钮 = new Field({
    标识: '师门界面继续任务按钮',
    方式: '找图',
    图片路径: path.resolve(__dirname, 'shimen', `shimenjixurenwu.png`)
})

const 师门界面去完成按钮 = new Field({
    标识: '师门界面去完成按钮',
    方式: '找图',
    图片路径: path.resolve(__dirname, 'shimen', `shimenquwancheng.png`)
})



module.exports = {
    主界面活动按钮,
    活动界面,
    师门界面继续任务按钮,
    师门界面去完成按钮,
    师门界面,
    活动界面师门任务,
    活动界面参加按钮,
}




