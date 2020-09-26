var TrainRoute = function (trainId) {
    var self = this;
    this.trainId = trainId;
    
    this.TrainGetCurrentData = function () {
        var json = new JsonHelper();
        var jsonQuery = json.GetPostData("/iLogis/PFEP/TrainGetCurrentData", { trainId });
        jsonQuery.done(function (currentWorkstationName) {
            //console.log("train: " + trainId + ", " + currentWorkstationName);

            $(".trainRow[data-transporterid=" + trainId + "]").find(".workstation").removeClass("selected");

            if (currentWorkstationName.length > 0) {
                $(".trainRow[data-transporterid=" + trainId + "]").find(".workstation[data-workstationname=" + currentWorkstationName + "]").addClass("selected");
            }
        });
    };

    //function StartThread() {
    //    self.StopInterval();
    //    interval = setInterval(function () {
    //        self.TrainGetCurrentData();
    //    }, 5000);
    //}
    //this.StopInterval = function () {
    //    window.clearInterval(interval);
    //    interval = null;
    //};

};