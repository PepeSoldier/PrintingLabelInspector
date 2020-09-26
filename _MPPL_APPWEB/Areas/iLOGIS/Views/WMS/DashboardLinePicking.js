function DashboardLinePicking() {   
    var viewModel = {
        data: null,
    };

    this.Init = function () {
        LoadLinePicking();
        Actions();
    };

    function LoadLinePicking() {
        var JsonHelp = new JsonHelper();
        let ReturnJson = JsonHelp.GetPostData("/iLogis/WMS/DashboardLinePicking");
        ReturnJson.done(function (linePicking) {
            linePicking = InitializeData();
            viewModel.data = linePicking.slice(0,9);
            RenderTemplate("#linePickingTemplate", "#linePickingView", viewModel);
        });
        ReturnJson.fail(function () {
            new Alert().Show("success", "Problem z połączeniem z bazą danych");
        });
    }

    function InitializeData() {
        let ldCount = 29;
        return GenerateLDFakeData(ldCount);
    }

    function GenerateLDFakeData(ldCount) {
        let ldList = [];
        let randNum = parseInt( Math.random() * 10);
        for (let i = 0; i < ldCount; i++) {
            let zm = {
                LineNumber: i,
                ItemCode: "911 32" + randNum + "1 4" + i +"00",
                Qty: i *17,
                DeliveryDate: "2020-04-23 1" + randNum + ":3"+ i,
            };
            ldList.push(zm);
        }
        return ldList;
    }

    function Actions() {
    };


};