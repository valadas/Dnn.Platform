const path = require('path');

module.exports = {
    entry: './src/index.js',
    output: {
        filename: 'View.js',
        path: path.resolve(__dirname, '../')
    }
};