const path = require('path');
const Field = require('../tools/Field.js')


const 跳过 =  new Field({
    标识: '跳过',
    方式: 'opencv找图',
    图片路径: path.resolve(__dirname, 'common', `tiaoguo.png`)
})

const 使用 =  new Field({
    标识: '使用',
    方式: 'opencv找图',
    图片路径: path.resolve(__dirname, 'common', `shiyong.png`)
})

const 上交 =  new Field({
    标识: '上交',
    方式: 'opencv找图',
    图片路径: path.resolve(__dirname, 'common', `shangjiao.png`)
})

const 主界面活动按钮 =  new Field({
    标识: '主界面活动按钮',
    方式: 'opencv找图',
    图片路径: path.resolve(__dirname, 'common', `huodong.png`)
})

const 活动界面 = new Field({
    标识: '活动界面',
    方式: 'opencv找图',
    图片路径: path.resolve(__dirname, 'common', `huodongjiemian.png`)
})

const 活动界面师门任务 = new Field({
    标识: '活动界面师门任务',
    方式: 'opencv找图',
    图片路径: path.resolve(__dirname, 'shimen', `shimenrenwu.png`),
})

const 活动界面参加按钮 = new Field({
    标识: '活动界面参加按钮',
    方式: 'opencv找图',
    图片路径: path.resolve(__dirname, 'shimen', `canjia.png`),
})

const 师门界面 = new Field({
    标识: '师门界面',
    方式: 'opencv找图',
    图片路径: path.resolve(__dirname, 'shimen', `shimenjiemian.png`)
})

const 师门界面继续任务按钮 = new Field({
    标识: '师门界面继续任务按钮',
    方式: 'opencv找图',
    图片路径: path.resolve(__dirname, 'shimen', `shimenjixurenwu.png`)
})

const 师门界面选择按钮 = new Field({
    标识: '师门界面选择按钮',
    方式: 'opencv找图',
    图片路径: path.resolve(__dirname, 'shimen', `xuanzhe.png`)
})

const 师门界面去完成按钮 = new Field({
    标识: '师门界面去完成按钮',
    方式: 'opencv找图',
    图片路径: path.resolve(__dirname, 'shimen', `shimenquwancheng.png`)
})

const 主界面_师门文字 = new Field({
    标识: '主界面_师门文字',
    方式: 'opencv找图',
    图片路径: path.resolve(__dirname, 'shimen', `shimenwenzi.png`)
})

const 主界面_师门集物 = new Field({
    标识: '主界面_师门集物',
    方式: 'opencv找图',
    图片路径: path.resolve(__dirname, 'shimen', `shimenjiwu.png`)
})

const 主界面_查看门派关系按钮 = new Field({
    标识: '主界面_查看门派关系按钮',
    方式: 'opencv找图',
    图片路径: path.resolve(__dirname, 'shimen', `chakanmenpaiguanxi.png`)
})
const 对话_师门任务按钮 = new Field({
    标识: '对话_师门任务按钮',
    方式: 'opencv找图',
    图片路径: path.resolve(__dirname, 'shimen', `duihua_shimenrenwu.png`)
})
const 弹框_购买 = new Field({
    标识: '弹框_购买',
    方式: 'opencv找图',
    图片路径: path.resolve(__dirname, 'shimen', `goumai.png`)
})
const 摆摊弹框_购买 = new Field({
    标识: '摆摊弹框_购买',
    方式: 'opencv找图',
    图片路径: path.resolve(__dirname, 'shimen', `baitangoumai.png`)
})
const 对话_师门寻趣按钮 = new Field({
    标识: '对话_师门寻趣按钮',
    方式: 'opencv找图',
    图片路径: path.resolve(__dirname, 'shimen', `shimenxunqu.png`)
})

module.exports = {
    主界面活动按钮,
    活动界面,
    师门界面继续任务按钮,
    师门界面去完成按钮,
    师门界面,
    活动界面师门任务,
    活动界面参加按钮,
    主界面_师门文字,
    主界面_师门集物,
    跳过,
    使用,
    主界面_查看门派关系按钮,
    对话_师门任务按钮,
    弹框_购买,
    上交,
    摆摊弹框_购买,
    师门界面选择按钮,
    对话_师门寻趣按钮
}




