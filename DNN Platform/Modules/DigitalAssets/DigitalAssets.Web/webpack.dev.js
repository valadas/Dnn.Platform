const merge = require('webpack-merge');
const common = require('./webpack.common.js');

module.exports = merge(common, {
    mode: 'development',
    output: {
        publicPath: "http://localhost:8080/dist/"
    },
    devServer: {
        host: 'localhost',
        port: 8080,
        disableHostCheck: true
    }
});