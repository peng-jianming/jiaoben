const Jimp = require('jimp');
const path = require('path');

async function convert() {
    const inputPath = path.join(__dirname, 'ddd.bmp');
    const outputPath = path.join(__dirname, 'ddd.png');

    try {
        console.log(`Reading ${inputPath}...`);
        let image;
        
        if (Jimp.Jimp && typeof Jimp.Jimp.read === 'function') {
             image = await Jimp.Jimp.read(inputPath);
        } else if (typeof Jimp.read === 'function') {
             image = await Jimp.read(inputPath);
        } else {
            // In case it's a new version where default export is different
            console.log('Jimp structure:', Object.keys(Jimp));
            throw new Error('Could not find Jimp.read function');
        }
        
        console.log(`Writing to ${outputPath}...`);
        await image.writeAsync(outputPath);
        console.log('Conversion complete.');
    } catch (error) {
        console.error('Error converting image:', error);
    }
}

convert();
