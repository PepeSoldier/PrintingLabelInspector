
function JsonHelper(){
    
    this.GetPostData = function (link, filters) {
        //var dfd = $.Deferred();
        return $.ajax({
            async: true, type: "POST", data: filters,
            url: link,
            success: function (data) {
                //dfd.resolve(data);
            },
            error: function (xhr, ajaxOptions, thrownError) {
                ShowAlertConnectionProblem(thrownError);
            },
            fail: function (xhr, ajaxOptions, thrownError) {
                console.log("fail");
                ShowAlertConnectionProblem(thrownError);
            }
        });
        // dfd.promise();
    };
    this.GetPostDataAwait = function (link, filters) {
        //var dfd = $.Deferred();
        return $.ajax({
            async: false, type: "POST", data: filters,
            url: link,
            success: function (data) {
                //dfd.resolve(data);
            },
            error: function (xhr, ajaxOptions, thrownError) {
                ShowAlertConnectionProblem(thrownError);
            },
            fail: function (xhr, ajaxOptions, thrownError) {
                console.log("fail");
                ShowAlertConnectionProblem(thrownError);
            }
        });
        // dfd.promise();
    };
    this.GetData = function (link, filters) {
        return $.ajax({
            async: true, type: "GET", data: filters,
            url: link,
            success: function (data) {
            },
            error: function (xhr, ajaxOptions, thrownError) {
                ShowAlertConnectionProblem(thrownError);
            },
            fail: function (xhr, ajaxOptions, thrownError) {
                console.log("fail");
                ShowAlertConnectionProblem(thrownError);
            }
        });
    };
    this.GetDataAwait = function (link, filters) {
        return $.ajax({
            async: false, type: "GET", data: filters,
            url: link,
            success: function (data) {
            },
            error: function (xhr, ajaxOptions, thrownError) {
                ShowAlertConnectionProblem(thrownError);
            },
            fail: function (xhr, ajaxOptions, thrownError) {
                console.log("fail");
                ShowAlertConnectionProblem(thrownError);
            }
        });
    };
    this.Update = function (url, item) {
        console.log(item);
        return $.ajax({
            async: true, type: "POST", data: item,
            url: url
        });
    };

    function ShowAlertConnectionProblem(thrownError) {
        if (thrownError.length == 0)
            thrownError = "Problem z połączeniem";
        new Alert().Show("danger", thrownError);
        return thrownError;
    }
}
