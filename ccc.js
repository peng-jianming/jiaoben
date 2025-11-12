const { dll: lh } = require('./tools/lihuo')



function main() {
    try {
        // 版本
        const ver = lh.Ver();
        console.log('插件版本号为:', ver);
        const ccc = lh.GetColor(100, 100);
        const aaa = lh.CmpColor(100, 100, 'cac4cc', 0.9);
        // const ddd = lh.MoveTo(100, 200);
        // console.log(ccc, aaa, ddd);
        const reg_ret = lh.Reg('pengjianming07da20d304e552776cf6a1c9f7eebb5a')
        console.log(ccc, aaa, "=========");




    } catch (err) {
        console.error('执行失败：', err);
    }
}

main();


