function DeliveryInspectionLocalization() {

    var self = this;

    this.LoadView = function (backButton) {
        $("#Loading").html(ShowLoadingSnippet());

        var JsonHelp = new JsonHelper();
        var ReturnJson = JsonHelp.GetData("/iLOGIS/Delivery/DeliveryInspectionLocalization", {});
        ReturnJson.done(function (data) {
            $("#mainMenu").html(data);
        });
    }
}