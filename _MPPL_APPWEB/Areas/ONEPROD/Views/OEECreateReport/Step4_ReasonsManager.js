function ReasonsManager() {
    this.dataRef;

    //w widoku index wywołujesz: reasonsData.dataRef.StopChangeOver
    //skad mogę wiedziec, że mogę się tak odwołać? Wiesz tylko Ty. 
    //Lepiej żeby obiekt jawnie udostępniał mi to co poniżej zmiast dataRef:
    //this.StopChangeOverReasons;
    //this.StopBreakdownReasons
    //this.StopPlannedReasons
    //this.StopUnplannedReasons
    //this.StopPerformanceReasons
    //Na początek było by oko, ale jeszcze jest fakt, że dla każdej maszyny będą inne powody :D

    this.GetData = function () {
        this.dataRef = RefreshData();
    };

    RefreshData = function () {
        var dataFromModel = null;
        $.ajax({
            async: false,
            url: "/ONEPROD/OEE/GetReasons",
            type: "GET",
            dataType: "json",
            success: function (data) {
                dataFromModel = data;
            },
            error: function (xhr, ajaxOptions, thrownError) {
                console.log("error: " + thrownError);
            }
        });
        return dataFromModel;
    };
};