//var Routing = function (appRoot, contentSelector, defaultRoute) {

//    BaseUrl = appRoot; //$('#BaseUrl').attr('href');
//    BaseUrl = BaseUrl.substring(0, BaseUrl.length - 1);

//    function getUrlFromHash(hash) {
//        var urltmp = hash.split('#'); //hash.replace('#/', '');
//        var url = urltmp[1];
//        //if (url === appRoot)
//        //    url = defaultRoute;
//        return url;
//    }

//    return {
//        init: function () {
//            Sammy(contentSelector, function () {
//                this.get(/\#\/(.*)/, function (context) {
//                    var url = getUrlFromHash(context.path);
//                    if (url != "/") {
//                        $(contentSelector).fadeOut(50, function () {
//                            context.load(BaseUrl + url, { cache: false, data: { t: (new Date().getTime()) } }).swap();
//                        });
//                        $(contentSelector).fadeIn(500);
//                    }
//                });
//            }).run('#/');

//        }
//    };
//}