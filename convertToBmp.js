const Jimp = require('jimp');
const path = require('path');

async function convert() {
    const inputPath = path.join(__dirname, 'duihuakuang.png');
    const outputPath = path.join(__dirname, 'duihuakuang.bmp');

    try {
        console.log(`Reading ${inputPath}...`);
        let image;
        if (Jimp.Jimp && typeof Jimp.Jimp.read === 'function') {
             image = await Jimp.Jimp.read(inputPath);
        } else {
             image = await Jimp.read(inputPath);
        }
        
        console.log(`Writing to ${outputPath}...`);
        await new Promise((resolve, reject) => {
            image.write(outputPath, (err) => {
                if (err) reject(err);
                else resolve();
            });
        });
        console.log('Conversion complete.');
    } catch (error) {
        console.error('Error converting image:', error);
    }
}

convert();
