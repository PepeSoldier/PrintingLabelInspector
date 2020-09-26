function DeliveryInspectionSummaryItem(itemCode, deliveryId, deliveryItemListGroupGuid) {
    DeliveryInspectionAbstract.call(this, 0, deliveryId, deliveryItemListGroupGuid);

    var self = this;
    //var supplierId = "";
    //var deliveryId = "";
    var viewModel = {
        ItemCode: itemCode,
        SupplierId: 0,
        DeliveryId: deliveryId,
        DeliveryItemListGroupGuid: deliveryItemListGroupGuid,
        DeliveryItems: []
    };

    this.Init = function () {
        //console.log(deliveryInspectionSummaryViewModel);
        //viewModel.DeliveryItems = deliveryInspectionSummaryViewModel.DeliveryItems;
        //supplierId = deliveryInspectionSummaryViewModel.SupplierId;
        //deliveryId = deliveryInspectionSummaryViewModel.DeliveryId;
        GetData().then(function () {
            Render();
        });
        
        Actions();
    };

    function GetData() {
        return new Promise((resolve, reject) => {
            let deliveryJson = new JsonHelper().GetPostData("/iLogis/Delivery/DeliveryInspectionSummaryDataItem",
                {
                    itemCode: viewModel.ItemCode, deliveryId: viewModel.DeliveryId, deliveryItemListGroupGuid: viewModel.DeliveryItemListGroupGuid
                });
            deliveryJson.done(function (deliveryInspectionSummaryViewModel) {
                console.log("deliveryInspectionSummaryViewModel");
                console.log(deliveryInspectionSummaryViewModel);
                viewModel.DeliveryItems = deliveryInspectionSummaryViewModel.DeliveryItems;
                viewModel.SupplierId = deliveryInspectionSummaryViewModel.SupplierId;
                viewModel.SelectedItemCode = deliveryInspectionSummaryViewModel.SelectedItemCode;
                viewModel.SelectedItemName = deliveryInspectionSummaryViewModel.SelectedItemName;
                viewModel.SelectedItemQtyDocument = deliveryInspectionSummaryViewModel.SelectedItemDocumentQty;

                if (deliveryInspectionSummaryViewModel.DeliveryItems.length > 0) {
                    viewModel.SelectedItemQtyFound = deliveryInspectionSummaryViewModel.DeliveryItems.map(item => item.TotalQtyFound).reduce((prev, next) => prev + next);
                } else {
                    viewModel.SelectedItemQtyFound = 0;
                }
                resolve();
            });
            deliveryJson.fail(function () {
                new Alert().Show("danger", "Dokument nie został odnaleziony");
                reject();
            });
        });
    }
    function DeleteItem(deliveryItemId) {
        let deliveryJson = new JsonHelper().GetPostData(
            "/iLogis/Delivery/DeliveryInspectionBlindCheckDeleteItem",
            { deliveryItemId }
        );
        deliveryJson.done(function (deliveryInspectionSummaryViewModel) {
            self.Init();
        });
        deliveryJson.fail(function () {
            new Alert().Show("danger", "usuwanie nie powiodło się");
        });
    }
   
    function Render(){
        RenderTemplate("#deliverySummaryItemTemplate", "#deliverySummaryItem", viewModel);
        self.RenderInfoHeader();
        RenderInfoHeaderForItem();
    }

    function RenderInfoHeaderForItem() {
        RenderTemplate("#deliveryInspectionItemInfoHeaderTemplate", "#deliveryInspectionItemInfoHeader", viewModel);
    }

    function Actions() {
        $(document).off("click", "#backButton");
        $(document).on("click", "#backButton", function () {
            console.log("brak skaszowanych danych");
            window.location.hash = "#/iLOGIS/Delivery/DeliveryInspectionSummary/?supplierId=" + viewModel.SupplierId + "&deliveryId=" + viewModel.DeliveryId + "&deliveryItemListGroupGuid=" + viewModel.DeliveryItemListGroupGuid;
        });

        $(document).off("click", ".summaryItemDeleteIcon");
        $(document).on("click", ".summaryItemDeleteIcon", function (event) {
            event.stopPropagation();
            let deliveryItemId = $(this).attr("data-id");

            bootbox.confirm({
                message: "Jesteś pewny, że chcesz usunąć wpis?",
                size: 'small',
                buttons: {
                    cancel: { label: '<i class="fa fa-times"></i> NIE' },
                    confirm: { label: '<i class="fa fa-check"></i> TAK'}
                },
                callback: function (result) {
                    if (result == true) {
                        console.log("delete item: " + deliveryItemId);
                        DeleteItem(deliveryItemId);
                    }
                }
            });
        });
        $(document).off("click", ".deliverySummaryItemRow");
        $(document).on("click", ".deliverySummaryItemRow", function () {
            window.location.hash = "#/iLOGIS/Delivery/DeliveryInspectionSummaryItemLocate/?itemCode=" + viewModel.ItemCode + "&deliveryId=" + viewModel.DeliveryId + "&deliveryItemListGroupGuid=" + viewModel.DeliveryItemListGroupGuid;
        });
    }
}

function IsCorrect(data) {
    console.log(data);
    return "1";
}