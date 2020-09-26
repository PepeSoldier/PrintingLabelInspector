var Routing = function (appRoot, contentSelector) {

    BaseUrl = appRoot; //$('#BaseUrl').attr('href');
    if (BaseUrl.length > 0 && BaseUrl[BaseUrl.length - 1] == "/") {
        BaseUrl = BaseUrl.substring(0, BaseUrl.length - 1);
    }
    var contentDiv = contentSelector;

    function getUrlFromHash(hash) {
        var urltmp = hash.split('#'); //hash.replace('#/', '');
        var url = urltmp[1];
        return url;
    }

    return {
        init: function () {
            Sammy(contentSelector, function () {
                this.get(/\#\/(.*)/, function (context) {;
                    var url = getUrlFromHash(context.path);
                    if (url != "/") {
                        console.log("Summy JS ShowLoadingSnippet");
                        UninitOnScan();
                        $(contentDiv).html("");
                        $(contentDiv).html(ShowLoadingSnippet());
                        context.load(url, { cache: false, data: { t: (new Date().getTime()) }}).swap();
                    }
                });
            }).run(BaseUrl + '#/');

        }
    };
}


