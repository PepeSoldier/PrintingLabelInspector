function DashboardMissingPicking() {   
    var viewModel = {
        data: null,
    };

    this.Init = function () {
        LoadMissingPicking();
        Actions();
    };

    function LoadMissingPicking() {
        var JsonHelp = new JsonHelper();
        let ReturnJson = JsonHelp.GetPostData("/iLogis/WMS/DashboardMissingPicking");
        ReturnJson.done(function (missingPicking) {
            missingPicking = InitializeData();
            viewModel.data = missingPicking.slice(0,3);
            RenderTemplate("#missingPickingTemplate", "#missingPickingView", viewModel);
        });
        ReturnJson.fail(function () {
            new Alert().Show("success", "Problem z połączeniem z bazą danych");
        })   ;
    }

    function InitializeData() {
        let ldCount = 29;
        return GenerateLDFakeData(ldCount);
    }

    function GenerateLDFakeData(ldCount) {
        let ldList = [];
        let randNum = parseInt( Math.random() * 3);
        for (let i = 0; i < ldCount; i++) {
            let zm = {
                Time:"20min",
                ItemCode:"A423 129 345",
                WarehouseLocation: "101-14-10-21",
                Warehouse: "Mag_1",
                Qty: randNum + i,
            };
            ldList.push(zm);
        }
        return ldList;
    }

    function Actions() {
    };


};