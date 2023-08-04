const path = require('path')
const HtmlWebpackPlugin = require('html-webpack-plugin')
const { CleanWebpackPlugin } = require('clean-webpack-plugin')

module.exports = {
    entry: [
        './src/index.js'
    ],
    output: {
        path: path.join(__dirname, '/dist'),
        filename: 'index.bundle.js'
    },
    devServer: {
        static: path.resolve(__dirname, 'src'),
        port: 4800,
        open: true,
        hot: true,
        historyApiFallback:{
            index:'/'
        },
    },
    module: {
        rules: [
            {
                test: /\.(js|jsx)$/,
                exclude: /nodeModules/,
                use: {
                    loader: 'babel-loader'
                }
            },
            {
                test: /\.tsx?$/,
                use: 'ts-loader',
                exclude: /node_modules/,
              },
            {
                test: /\.(css|less)$/,
                use: ['style-loader', 'css-loader', 'less-loader']
            },
            {
                test: /\.(png|svg|jpg|jpeg|gif)$/i,
                type: 'asset/resource',
                use: ['file-loader'],
            },
        ]
    },
    resolve: {
        extensions: ['.js', '.jsx', '.tsx', '.ts']
    },
    plugins: [
        new HtmlWebpackPlugin({ template: './src/index.html' }),
        new CleanWebpackPlugin()
    ],
}