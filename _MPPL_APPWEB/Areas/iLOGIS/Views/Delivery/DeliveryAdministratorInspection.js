function TrueFalseTranslation(x) {
    if (x == "sum") {
        return "";
    } else {
        return x == true ? "TAK" : "NIE";
    }
}
function DontDrawZero(x) {
    return x == 0 ? "" : x;
}

function DeliveryAdministratorInspection() {

    var self = this;
    var viewModel = {
        DeliveryItemList: {
            Id: 0,
            ItemWMSId: 0,
            ItemCode: "",
            ItemName: "",
            DeliveryId: 0,
            SupplierId: 0,
            NumberOfPackages: 0,
            QtyInPackage: 0,
            TotalQty: 0,
            TotalQtyInspection: 0,
            WasPrinted: false,
            RemainingQty: 0
        }
    };
    this.Init = function (deliveryId) {
        viewModel.DeliveryId = deliveryId;
        LoadDeliveryItems();
        Actions();
    };

    function LoadDeliveryItems() {
        console.log("LoadDeliveryItems");
        let JsonHelp = new JsonHelper();
        let ReturnJson = JsonHelp.GetPostData("/iLogis/Delivery/DeliveryAdministratorInspectionData",
            { deliveryId: viewModel.DeliveryId });
        ReturnJson.done(function (deliveryItemList)
        {
            viewModel.DeliveryItemList = GenerateGridData(deliveryItemList),
            console.log(viewModel);
            Render();
        });
        ReturnJson.fail(function () {
            new Alert().Show("success", "Problem z połączeniem z bazą danych");
        });
    }
    function GenerateGridData(deliveryItemList) {
        let deliveryItemViewList = [];
        let lastItemCode = "";
        let totalQtyDocument = 0;
        let totalQtyInspection = 0;

        deliveryItemList.forEach(function (entry)
        {
            entry.ItemCodeVisible = entry.ItemCode != lastItemCode;
            entry.TotalQtyInspection = entry.OperatorEntry? entry.NumberOfPackages * entry.QtyInPackage : 0;

            if (entry.ItemCode != lastItemCode && lastItemCode != "") {
                let summaryRow = GenerateSummaryRow(totalQtyDocument, totalQtyInspection);
                deliveryItemViewList.push(summaryRow);
                SetItemRowsStatus(deliveryItemViewList);
                totalQtyDocument = 0;
                totalQtyInspection = 0;
            }

            if (entry.OperatorEntry == true) {
                totalQtyInspection += entry.NumberOfPackages * entry.QtyInPackage;
            }
            else {
                totalQtyDocument += entry.TotalQty;
            }

            deliveryItemViewList.push(entry);
            lastItemCode = entry.ItemCode;
        });

        deliveryItemViewList.push(GenerateSummaryRow(totalQtyDocument, totalQtyInspection));
        return deliveryItemViewList;
    }
    function GenerateSummaryRow(totalQtyDocument, totalQtyInspection) {
        return {
            Id: 0,
            ItemWMSId: 0,
            ItemCode : "SUMA",
            TotalQty : totalQtyDocument,
            TotalQtyInspection : totalQtyInspection,
            NumberOfPackages : 0,
            IsQtyEqual: totalQtyDocument == totalQtyInspection? true: false,
            IsSummaryRow: true,
            ItemCodeVisible: true,
            OperatorEntry: false,
            AdminEntry: false,
            WasPrinted: "sum",
            RemainingQty: 0
        };
    }
    function CreateStockUnit(deliveryItem) {
        console.log(deliveryItem);
        let jsh = new JsonHelper().GetPostData("/iLOGIS/StockUnit/CreateNewFromDelivery",
            {
                itemWMSId: deliveryItem.ItemWMSId,
                //itemCode: deliveryItem.ItemCode,
                qty: deliveryItem.QtyInPackage,
                maxQtyPerPackage: deliveryItem.QtyInPackage,
                numberOfPackages: deliveryItem.NumberOfPackages,
                warehouseId: 0,
                deliveryId: deliveryItem.DeliveryId,
            });
        jsh.done(function (response) {
            if (response.status > 0) {
                new Alert().Show("danger", "Błąd: iLOGIS:" + response.status);
            }

            if (response.status == 400) {
                new Alert().Show("info", "Wybierz miejsce lokalizowania");
            }
            //else {
            //    //console.log("");
            //}
            console.log("StockUnitCreated");
        });
        jsh.fail(function () {
            new Alert().Show("danger", "Wystąpił problem podczas tworzenia opakowania");
        });
    }
    function SetItemRowsStatus(deliveryItemViewList) {
        let lastRow = Math.max(0, deliveryItemViewList.length - 1);
        let status = deliveryItemViewList[lastRow].IsQtyEqual;
        let found = false;

        lastRow--;
        while (lastRow >= 1 && !found) {
            deliveryItemViewList[lastRow].IsQtyEqual = status;
            lastRow--;
            found = deliveryItemViewList[lastRow].IsSummaryRow == true;
        }
    }

    function Render(){
        RenderTemplate("#deliveryAdministrationInspectionTemplate", "#contentView", viewModel);
    }

    function Actions() {
        $(document).off("input", ".numberOfPackages");
        $(document).on("input", ".numberOfPackages", function (e) {
            let id = $(this).attr("data-id");
            let value = $(this).val();

            let obj = viewModel.DeliveryItemList.find(x => x.Id == id);
            obj.NumberOfPackages = value;
        });

        $(document).off("input", ".qtyInPackage");
        $(document).on("input", ".qtyInPackage", function (e) {
            let id = $(this).attr("data-id");
            let value = $(this).val();

            let obj = viewModel.DeliveryItemList.find(x => x.Id == id);
            obj.QtyInPackage = value;
        });

        $(document).off("click", "#btnSelectAll");
        $(document).on("click", "#btnSelectAll", function (e) {
            console.log("#btnSelectAll");
            $(".toBePrinted").attr('checked', true);
        });

        $(document).off("click", "#btnUnselectAll");
        $(document).on("click", "#btnUnselectAll", function (e) {
            console.log("#btnUnselectAll");
            $(".toBePrinted").attr('checked', false);
        });

        $(document).off("click", "#btnPrintSelected");
        $(document).on("click", "#btnPrintSelected", function (e) {
            console.log("#btnPrintSelected");
            var selected = [];
            var items = [];
            $('.toBePrinted:checked').each(function () {
                selected.push($(this));
                items.push(viewModel.DeliveryItemList.find(x => x.Id == $(this).attr("data-id")));
            });

            console.log(selected);
            console.log(items);

            items.forEach(function (itm) {
                CreateStockUnit(itm);
            });
            
        });

    }


}