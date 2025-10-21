// const addon = require("./build/Release/cal.node");
const winax = require('./index');
const dll = new winax.Object("dm.dmsoft");
const ret = dll.Reg("xf30557fc317f617eead33dfc8de3bdd4ab9043", "x4lpdhpht2zgnl7");
console.log(ret, "==========");
dll.moveTo(10, 10);
// const arr1 = addon.Export_GetProcessList();
// console.log(arr1);
// console.log(addon.Export_InjectDll(23420, "E:\\Dll1.dll"), "----");


// (async () => {
//     await new Promise((resolve, reject) => {
//         setTimeout(() => {
//             console.log(arr1);
//             resolve()
//         }, 2000);
//     })
// })()
// const dllFile = 'myDllDemo.dll';
// const dllPath = path.join(Ps.getExtraResourcesDir(), "dll", dllFile);
// console.log(dllPath, "-------");
// // 映射到C语言 int数组类型
// // var IntArray = ArrayType(ref.types.int);

// // 加载 DLL文件,无需写扩展名,将DLL中的函数映射成JS方法
// const MyDellDemo = new ffi.Library(dllPath, {
//   // 方法名必须与C函数名一致
//   add: [
//     'int', // 对应 C函数返回类型
//     ['int', 'int'] // C函数参数列表
//   ],
//   // 使用 ffi中内置类型的简写类型
//   addPtr: ['void', ['int', 'int', 'int*']],
//   // IntArray 是上面通过 ArrayType 构建出来的类型
//   // initArray: ['void', [IntArray, 'int']]
// });