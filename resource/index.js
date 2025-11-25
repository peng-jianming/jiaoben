const fs = require('fs');
const path = require('path');

// 获取当前目录下的所有文件夹
const resourceDir = __dirname;
const folders = fs.readdirSync(resourceDir, { withFileTypes: true })
    .filter(dirent => dirent.isDirectory() && dirent.name !== 'cache')
    .map(dirent => dirent.name);

// 动态导入所有子文件夹的 index.js
const allExports = {}; 
folders.forEach(folderName => {
    const folderPath = path.join(resourceDir, folderName, 'index.js');
    if (fs.existsSync(folderPath)) {
        const folderExports = require(`./${folderName}`);
        Object.assign(allExports, folderExports);
    }
});

module.exports = allExports;




