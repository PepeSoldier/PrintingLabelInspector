function DashboardLastDeliveries() {   
    var viewModel = {
        data: null,
    };

    this.Init = function () {
        LoadLastDeliveries();
        Actions();
    };

    function LoadLastDeliveries() {
        var JsonHelp = new JsonHelper();
        let ReturnJson = JsonHelp.GetPostData("/iLogis/WMS/DashboardLastDaliveries");
        ReturnJson.done(function (lastDeliveries) {
            lastDeliveries = InitializeData();
            viewModel.data = lastDeliveries.slice(0,5);
            RenderTemplate("#lastDeliveriesTemplate", "#lastDeliveriesView", viewModel);
        });
        ReturnJson.fail(function () {
            new Alert().Show("success", "Problem z połączeniem z bazą danych");
        });
    }

    function InitializeData() {
        let ldCount = 37;
        return GenerateLDFakeData(ldCount);
    }

    function GenerateLDFakeData(ldCount) {
        let ldList = [];
        for (let i = 0; i < ldCount; i++) {
            let zm = {
                DocumentNumber: "Document_" + i,
                ItemCode: "A432 129 123",
                SupplierName: "Implea Sp. zo.o.",
                TotalItems: i * 23,
                ItemsCount: i*3,
            };
            ldList.push(zm);
        }
        return ldList;
    }

    function Actions() {
    };


};