const path = require('path');
const Field = require('../../tools/Field.js');

const 活动界面师门任务 = new Field({
    标识: '活动界面师门任务',
    方式: 'opencv找图',
    图片路径: path.resolve(__dirname, `shimenrenwu-weiwancheng.png`),
});

const 活动界面参加按钮 = new Field({
    标识: '活动界面参加按钮',
    方式: 'opencv找图',
    图片路径: path.resolve(__dirname, `canjia.png`),
});

const 师门界面 = new Field({
    标识: '师门界面',
    方式: 'opencv找图',
    图片路径: path.resolve(__dirname, `shimenjiemian.png`)
});

const 师门界面继续任务按钮 = new Field({
    标识: '师门界面继续任务按钮',
    方式: 'opencv找图',
    图片路径: path.resolve(__dirname, `shimenjixurenwu.png`)
});

const 师门界面选择按钮 = new Field({
    标识: '师门界面选择按钮',
    方式: 'opencv找图',
    图片路径: path.resolve(__dirname, `xuanzhe.png`)
});

const 师门界面去完成按钮 = new Field({
    标识: '师门界面去完成按钮',
    方式: 'opencv找图',
    图片路径: path.resolve(__dirname, `shimenquwancheng.png`)
});

const 主界面_师门文字 = new Field({
    标识: '主界面_师门文字',
    方式: 'opencv找图',
    图片路径: path.resolve(__dirname, `shimenwenzi.png`)
});

const 弹框_购买 = new Field({
    标识: '弹框_购买',
    方式: 'opencv找图',
    图片路径: path.resolve(__dirname, `goumai.png`)
});

const 摆摊弹框_购买 = new Field({
    标识: '摆摊弹框_购买',
    方式: 'opencv找图',
    图片路径: path.resolve(__dirname, `baitangoumai.png`)
});


const 师门_任务完成弹框 = new Field({
    标识: '师门_任务完成弹框',
    方式: 'opencv找图',
    图片路径: path.resolve(__dirname, `shimenwancheng.png`)
});

module.exports = {
    活动界面师门任务,
    活动界面参加按钮,
    师门界面,
    师门界面继续任务按钮,
    师门界面选择按钮,
    师门界面去完成按钮,
    主界面_师门文字,
    弹框_购买,
    摆摊弹框_购买,
    师门_任务完成弹框
};

