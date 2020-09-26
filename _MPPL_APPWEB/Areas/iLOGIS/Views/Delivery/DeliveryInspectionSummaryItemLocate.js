function DeliveryInspectionSummaryItemLocate(itemCode, deliveryId, deliveryItemListGroupGuid) {
    DeliveryInspectionAbstract.call(this, 0, deliveryId, deliveryItemListGroupGuid);

    let locatingInProcess = false;
    var self = this;
    
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
            Actions();
        });
    };

    function GetData() {
        return new Promise((resolve, reject) => {
            let deliveryJson = new JsonHelper().GetPostData("/iLogis/Delivery/DeliveryInspectionSummaryItemLocateGetData",
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

                //for (let i = 0; i < viewModel.DeliveryItems.length; i++) {
                //    viewModel.DeliveryItems[i].WasPrinted = viewModel.DeliveryItems[i].TotalQuantityLocated == viewModel.DeliveryItems[i].QtyInPackage;
                //}
                
                for (let i = 0; i < viewModel.DeliveryItems.length; i++) {
                    viewModel.DeliveryItems[i].IsLocationAssigned = viewModel.DeliveryItems[i].TotalLocatedQty == viewModel.DeliveryItems[i].QtyInPackage;
                }

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
    function LocateItems() {
        ShowLoadingSnippetWithOverlay();
        DisableElement("#btnConfirmLocation");
        let destinationLocationName = $("#destinationLocationName").val();
        let selectedRows = $(".deliverySummaryItemRow.selectedRow");
        let deliveryItems = [];
        let oneLabel = document.getElementById("cbOneLabel").checked == true;

        if (selectedRows.length == 0) {
            RemoveLoadingSnippetWithOverlay();
            bootbox.alert("Nie wybrano żadnej pozycji");
            EnableElement("#btnConfirmLocation");
        }
        else {
            for (let i = 0; i < selectedRows.length; i++) {
                let index = $(selectedRows[i]).index();
                viewModel.DeliveryItems[index].DestinationLocationName = destinationLocationName;
                console.log(viewModel.DeliveryItems[index]);
                deliveryItems.push(viewModel.DeliveryItems[index]);
            }

            new JsonHelper().GetPostData("/iLogis/StockUnit/CreateNewFromDeliveryManual",
                { deliveryItems, oneLabel }
            ).done(function (response) {
                locatingInProcess = false;
                RemoveLoadingSnippetWithOverlay();
                EnableElement("#btnConfirmLocation");
                if (response.status > 0) {
                    new Alert().Show("danger", TranslateStatus(response.status));
                }
                else {
                    new Alert().Show("success", "Zalokalizowano");
                }
                self.Init();
            }).fail(function () {
                locatingInProcess = false;
                RemoveLoadingSnippetWithOverlay();
                EnableElement("#btnConfirmLocation");
                new Alert().Show("danger", "lokalizowanie nie powiodło się dla niektórych pozycji");
            });
        }        
    }
    function CalculateSelectedItemsSummary() {

        let packages = 0;
        let totalPackages = 0;
        let totalQty = 0;

        $selectedRows = $(".deliverySummaryItemRow.selectedRow");
        $selectedRows.each(function (idx) {
            packages = parseInt($($selectedRows[idx]).find(".rowNumberOfPackages").text());
            totalPackages += packages;
            totalQty += packages * parseFloat($($selectedRows[idx]).find(".rowQtyInPackage").text());
        });

        $("#selectedItemsSummary").text("lokalizuj: " + totalQty + " (" + totalPackages + " opk.)");
    }

    function Render(){
        RenderTemplate("#deliverySummaryItemLocateTemplate", "#deliverySummaryItem", viewModel);
        self.RenderInfoHeader();
        RenderInfoHeaderForItem();
    }
    function RenderLocationForm() {
        RenderTemplate("#LocationFormTemplate", "#locationFormContainer", viewModel);
    }
    function RenderInfoHeaderForItem() {
        RenderTemplate("#deliveryInspectionItemInfoHeaderTemplate", "#deliveryInspectionItemInfoHeader", viewModel);
    }

    function Actions() {
        $(document).off("click", "#backButton");
        $(document).on("click", "#backButton", function () {
            window.location.hash = "#/iLOGIS/Delivery/DeliveryInspectionSummaryItem/?itemCode=" + viewModel.ItemCode + "&deliveryId=" + deliveryId + "&deliveryItemListGroupGuid=" + viewModel.DeliveryItemListGroupGuid;
        });
        $(document).off("click", "#btnLocate");
        $(document).on("click", "#btnLocate", function () {
            RenderLocationForm();
        });
        $(document).off("click", "#btnConfirmLocation");
        $(document).on("click", "#btnConfirmLocation", function () {
            if (locatingInProcess == false) {
                locatingInProcess = true;
                DisableElement("#btnConfirmLocation");
                LocateItems();
            }
        });

        $(document).off("click", ".deliverySummaryItemRow");
        $(document).on("click", ".deliverySummaryItemRow", function () {
            $(this).toggleClass("selectedRow");
            CalculateSelectedItemsSummary();
        });
    }
}

function IsCorrect(data) {
    console.log(data);
    return "1";
}