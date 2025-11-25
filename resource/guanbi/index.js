const path = require('path');
const Field = require('../../tools/Field.js');

const 关闭 = new Field({
    标识: '关闭',
    方式: 'opencv找图',
    图片路径: path.resolve(__dirname, `guanbi-1.png`)
});

module.exports = {
    关闭
};

