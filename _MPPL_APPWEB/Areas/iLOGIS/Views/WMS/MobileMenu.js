function MobileMenuLayout() {

    var self = this;

    this.LoadView = function (backButton) {
        
        var JsonHelp = new JsonHelper();
        var ReturnJson = JsonHelp.GetData("/iLOGIS/WMS/MobileMenuLayout", {});
        ReturnJson.done(function (data) {
            $("#mainMenu").html(data);
        });
    };
}