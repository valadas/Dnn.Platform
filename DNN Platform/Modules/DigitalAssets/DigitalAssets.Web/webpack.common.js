const path = require('path');

module.exports = {
    entry: './src/index.tsx',
    output: {
        filename: 'View.js',
        path: path.resolve(__dirname, '../js/')
    },    
    module: {
        rules: [
            {
                test: /\.css$/,
                use: [
                    'style-loader',
                    'css-loader'
                ]
            },
            {
                test: /\.tsx?$/,
                use: ['ts-loader', 'eslint-loader'],
                exclude: /node_modules/
            }
        ]
    },
    resolve: {
        extensions: ['.tsx', '.ts', '.js']
    }
};