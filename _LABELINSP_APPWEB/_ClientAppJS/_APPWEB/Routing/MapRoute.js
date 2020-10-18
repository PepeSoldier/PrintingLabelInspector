//function MapRoute(parentDivId, clickDivId, urlRoute, parameters) {
//    $(parentDivId).on("click", clickDivId, function (e) {

//        $(parentDivId).html(ShowLoadingSnippet());
//        $.ajax({
//            type: "GET",
//            url: urlRoute,
//            data: parameters.serialize(),
//            success: function (data) {
//                //$(parentDivId).load("/Report/ReportCost/" + ReportId);
//                $(parentDivId).html(data);
//            }
//        });
//    });
//}

//function MapSimpleRout(parentDivId, urlRoute) {
//    console.log("konsol log");

//    $(parentDivId).html(ShowLoadingSnippet());
//    this.location.href = this.location.href + "#/iLOGIS/WMS/MovementScanItem";
    
//    var JsonHelp = new JsonHelper();
//    var ReturnJson = JsonHelp.GetData(urlRoute, {});
//    ReturnJson.done(function (data) {
//        $(parentDivId).html(data);
//    });

//}