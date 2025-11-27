const path = require('path');
const Field = require('../../tools/Field.js');

const 跳过 = new Field({
    标识: '跳过',
    方式: 'opencv找图',
    图片路径: path.resolve(__dirname, `tiaoguo.png`)
});

const 使用 = new Field({
    标识: '使用',
    方式: 'opencv找图',
    图片路径: path.resolve(__dirname, `shiyong.png`)
});

const 上交 = new Field({
    标识: '上交',
    方式: 'opencv找图',
    图片路径: path.resolve(__dirname, `shangjiao.png`)
});

const 主界面活动按钮 = new Field({
    标识: '主界面活动按钮',
    方式: 'opencv找图',
    图片路径: path.resolve(__dirname, `huodong.png`)
});

const 活动界面 = new Field({
    标识: '活动界面',
    方式: 'opencv找图',
    图片路径: path.resolve(__dirname, `huodongjiemian.png`)
});

module.exports = {
    跳过,
    使用,
    上交,
    主界面活动按钮,
    活动界面
};

