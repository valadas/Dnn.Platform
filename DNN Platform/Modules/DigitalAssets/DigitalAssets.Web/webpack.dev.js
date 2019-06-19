const merge = require('webpack-merge');
const webpack = require('webpack');
const common = require('./webpack.common.js');
const path = require('path');

module.exports = merge(common, {
    mode: 'development',
    output: {
        publicPath: "http://localhost:8080/dist/"
    },
    devServer: {
        host: 'localhost',
        port: 8080,
        disableHostCheck: true,
        hot: true
    },
    plugins: [
        new webpack.HotModuleReplacementPlugin()
    ]
});