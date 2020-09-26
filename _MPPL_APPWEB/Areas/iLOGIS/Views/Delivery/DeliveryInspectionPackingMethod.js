﻿function DeliveryInspectionPackingMethod(supplierId, deliveryId) {
    
    var self = this;
    //var supplierId = "";
    //var deliveryId = "";
    var viewModel = {
        SupplierId: supplierId,
        DeliveryId: deliveryId,
        DeliveryItems: []
    };

    this.Init = function () {
        //console.log(deliveryInspectionSummaryViewModel);
        //viewModel.DeliveryItems = deliveryInspectionSummaryViewModel.DeliveryItems;
        //supplierId = deliveryInspectionSummaryViewModel.SupplierId;
        //deliveryId = deliveryInspectionSummaryViewModel.DeliveryId;
        GetData().then(function () {
            AssignStatusIcons();
            Render();
        });
        
        Actions();
    };

    function GetData() {
        return new Promise((resolve, reject) => {
            let deliveryJson = new JsonHelper().GetPostData("/iLogis/Delivery/DeliveryInspectionSummaryData",
                {
                    supplierId: viewModel.SupplierId, deliveryId: viewModel.DeliveryId
                });
            deliveryJson.done(function (deliveryInspectionSummaryViewModel) {
                viewModel.DeliveryItems = deliveryInspectionSummaryViewModel.DeliveryItems;
                resolve();
            });
            deliveryJson.fail(function () {
                new Alert().Show("danger", "Dokument nie został odnaleziony");
                reject();
            });
        });
    }
    function AssignStatusIcons() {
        viewModel.DeliveryItems.forEach(function (dli) {
            if(dli.TotalQtyDocument == dli.TotalQtyFound)
                dli.StatusIcon = 'fas fa-check-circle colorGreen';
            else
                dli.StatusIcon = 'fas fa-times-circle colorRed';
        });
    }
    function Render(){
        RenderTemplate("#deliverySummaryTemplate", "#deliveryListItemsSummary", viewModel);
        self.RenderInfoHeader();
    }
    
    function Actions() {
        $(document).off("click", "#backButton");
        $(document).on("click", "#backButton", function () {
            window.location.hash = "#/iLOGIS/Delivery/DeliveryInspectionBlindCheck/?supplierId=" + supplierId + "&deliveryId=" + deliveryId;
        });

        $(document).off("click", ".deliverySummaryRow");
        $(document).on("click", ".deliverySummaryRow", function () {
            let itemCode = $(this).find(".itemCode").text().trim();
            console.log(itemCode);
            window.location.hash = "#/iLOGIS/Delivery/DeliveryInspectionSummaryItem/?itemCode=" + itemCode + "&deliveryId=" + deliveryId;
        });
    }
}

function IsCorrect(data) {
    console.log(data);
    return "1";
}