const path = require('path');
const Field = require('../../tools/Field.js');

const 关闭1 = new Field({
    标识: '关闭1',
    方式: 'opencv找图',
    图片路径: path.resolve(__dirname, `guanbi-1.png`)
});
const 关闭师门弹框 = new Field({
    标识: '关闭师门弹框',
    方式: 'opencv找图',
    图片路径: path.resolve(__dirname, `guanbi-shimen.png`)
});

module.exports = {
    关闭1,
    关闭师门弹框
};

