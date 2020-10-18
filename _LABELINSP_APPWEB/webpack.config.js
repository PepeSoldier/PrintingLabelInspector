const path = require('path');
const webpack = require('webpack');
const MinifyPlugin = require("babel-minify-webpack-plugin");
const HtmlWebpackPlugin = require('html-webpack-plugin');
const { CleanWebpackPlugin } = require('clean-webpack-plugin');

function GetConfig() {
    return {
        mode: "development",
        devtool: 'inline-source-map',
        entry: [],
        output: {},
        //plugins: [
        //    // new CleanWebpackPlugin(['dist/*']) for < v2 versions of CleanWebpackPlugin
        //    new CleanWebpackPlugin(),
        //    new HtmlWebpackPlugin({
        //        title: 'Caching',
        //        chunks: ['iLogis'],
        //        filename: 'ilogis.html'
        //    }),
        //    new HtmlWebpackPlugin({
        //        title: 'Caching',
        //        chunks: ['Oneprod'],
        //        filename: 'oneprod.html'
        //    }),
        //],
        //plugins: [
        //    //new MinifyPlugin(minifyOpts, pluginOpts)
        //    new MinifyPlugin({}, {})
        //],
        module: {
            rules: [
                { test: /\.js$/, exclude: /node_modules/, use: { loader: "babel-loader", options: { presets: ['@babel/preset-env'] } } },
                //{ test: /\.css?$/, use: ['style-loader', 'css-loader'] },
                //{ test: /\.(png|jpg|jpeg|gif|svg)$/, use: 'url-loader?limit=25000' },
                //{ test: /\.(png|woff|woff2|eot|ttf|svg)(\?|$)/, use: 'url-loader?limit=100000' }
            ]
        }
    };
}

module.exports = function (env) {
    env = env || {};
    var isProd = env.NODE_ENV === 'production';

    if (isProd) {
        GetConfig.devtool = 'source-map';
        GetConfig.plugins = GetConfig.plugins.concat([
            new webpack.optimize.UglifyJsPlugin({
                sourceMap: true
            })
        ]);
    }
    
    //config_ilogis = GetConfig();
    //config_ilogis.entry = {
    //    iLogis: './Areas/iLOGIS/Views/iLOGIS_Script.js',
    //    Oneprod: './Areas/iLOGIS/Views/StockUnit/Stock.js'
    //};
    //config_ilogis.output = {
    //    path: path.resolve(__dirname, './Scripts/dist'),
    //    filename: '[name].[contenthash].bundle.js',
    //    library: '[name]'
    //};

    config_ilogis = GetConfig();
    config_ilogis.entry = ['./Areas/iLOGIS/Views/iLOGIS_Script.js'];
    config_ilogis.output = {
        path: path.resolve(__dirname, './Areas/iLOGIS/Views/'),
        filename: 'iLOGIS_Script.bundle.js',
        library: 'iLogis'
    };

    //config_oneprod = GetConfig();
    //config_oneprod.entry = ['./Areas/iLOGIS/Views/StockUnit/Stock.js'];
    //config_oneprod.output = {
    //    path: path.resolve(__dirname, './Areas/iLOGIS/Views/'),
    //    filename: 'iLOGIS_Script.bundle2.js',
    //    //library: 'Oneprod'
    //};

    configs = [];
    configs.push(config_ilogis);
    //configs.push(config_oneprod);

    return configs;
};

//module.exports = [
//    {
//        mode: "development",
//        devtool: 'inline-source-map',
//        entry: [
//            './Areas/iLOGIS/Views/iLOGIS_Script.js'
//        ],
//        output: {
//            path: path.resolve(__dirname, './Areas/iLOGIS/Views/'),
//            filename: 'iLOGIS_Script.bundle.js',
//            library: 'iLogis'
//        },
//        plugins: [
//            //new MinifyPlugin(minifyOpts, pluginOpts)
//            new MinifyPlugin({}, {})
//        ],
//        module: {
//            rules: [
//                { test: /\.js$/, exclude: /node_modules/, use: { loader: "babel-loader", options: { presets: ['@babel/preset-env'] },
//                //{ test: /\.css?$/, use: ['style-loader', 'css-loader'] },
//                //{ test: /\.(png|jpg|jpeg|gif|svg)$/, use: 'url-loader?limit=25000' },
//                //{ test: /\.(png|woff|woff2|eot|ttf|svg)(\?|$)/, use: 'url-loader?limit=100000' }
//                }
//            }]
//        }
//    },
//    {
//        mode: "development",
//        entry: [
//            './Areas/iLOGIS/Views/StockUnit/Stock.js'
//        ],
//        output: {
//            path: path.resolve(__dirname, './Areas/iLOGIS/Views/'),
//            filename: 'iLOGIS_Script.bundle2.js',
//            library: 'Oneprod'
//        },
//        module: {
//            rules: [{
//                test: /\.js$/,
//                exclude: /node_modules/,
//                use: {
//                    loader: "babel-loader",
//                    options: {
//                        presets: ['@babel/preset-env']
//                    }
//                }
//            }]
//        }
//    }
//];



//{
    //    mode: "development",
    //    entry: [
    //        './Areas/iLOGIS/Views/iLOGIS_Script.js'
    //    ],
    //    output: {
    //        path: path.resolve(__dirname, './_ClientAppJS/AppBundle'),
    //        filename: 'iLOGIS_Script.bundle.js',
    //        library: 'App'
    //    },
    //    plugins: [
    //        new webpack.ProvidePlugin({
    //            $: 'jquery',
    //            jQuery: 'jquery',
    //            'window.jQuery': 'jquery'
    //        })
    //    ],
    //    module: {
    //        rules: [{
    //            test: /\.js$/,
    //            exclude: /node_modules/,
    //            use: {
    //                loader: "babel-loader",
    //                options: {
    //                    presets: ['@babel/preset-env'],
    //                    minimize: true
    //                }
    //            }
    //        }]
    //    }
    //},