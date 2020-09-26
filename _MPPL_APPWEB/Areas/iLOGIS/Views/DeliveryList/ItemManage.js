function ItemManageDL(_deliveryList, _trainId, loopQty) {

    var deliveryList = _deliveryList;
    var self = this;
    var trainId = _trainId;
    //var workorders;
    var windowSelector = "";
    var wnd = {};

    //WINDOW POPUPS
    function OpenWindow() {
        if (wnd != null) {
            try {
                wnd.Close();
            }
            catch (ex) {
                console.log(ex);
            }
        }
        wnd = new PopupWindow(850, 200, 140, 280);
    }
    
    //ShowItemPopup
    this.ShowPopup_ItemManage = function (itemWMSId, workstationId) {

        var jsQ = new JsonHelper().GetData("/iLOGIS/DeliveryList/ItemManage", { "trainId": trainId, "itemWMSId": itemWMSId, "workstationId": workstationId });
        jsQ.done(function (data) {
            console.log("Confirm");
            OpenWindow();
            wnd.Init("Item", "Szczegóły Artykułu");
            windowSelector = wnd.DivSelector();
            wnd.Show(data);
        });
    };
    this.ShowPopup_ItemManageInventory = function (itemWMSId, workstationId, workstationQty) {

        var jsQ = new JsonHelper().GetData("/iLOGIS/DeliveryList/ItemManageInventory",
            { "trainId": trainId, "itemWMSId": itemWMSId, "workstationId": workstationId, "workstationQty": workstationQty });
        jsQ.done(function (data) {
            console.log("ShowPopup_ItemManageInventory");
            wnd = self.GetCurrentWindow();
            wnd.Init("Item", "Edytuj ilość");
            wnd.Show(data);
            let kp = new KeyPad("#inventoryQty", "#kaypad112");
            kp.Init();
        });
    };
    this.ShowPopup_ItemManageManualQty = function (itemWMSId, workstationId, workstationQty) {

        var jsQ = new JsonHelper().GetData("/iLOGIS/DeliveryList/ItemManageManualQty",
            { "trainId": trainId, "itemWMSId": itemWMSId, "workstationId": workstationId, "workstationQty": workstationQty });
        jsQ.done(function (data) {
            console.log("ShowPopup_ItemManageManualQty");
            wnd = self.GetCurrentWindow();
            wnd.Init("Item", "Dostawa niepełnego opakowania");
            wnd.Show(data);
            let kp = new KeyPad("#deliveredQty", "#kaypad113");
            kp.Init();
        });
    };


    //---------------------------------ItemManageWindow---------------------------
    var itemWMSId = 0;
    var workstationId = 0;
    var qtyPerPackage = 1;
    var deliveredCounter = 0;
    var selectedItemRow = 0;

    this.ChangeItemRow = function (d) {
        var itemRows = $("#itemRows").find(".itemRow");

        if (d > 0) {
            console.log("nastepny");
            selectedItemRow++;
            selectedItemRow = FindNextVisible(itemRows);
            selectedItemRow = selectedItemRow > itemRows.length - 1 ? itemRows.length - 1 : selectedItemRow;
        } else {
            console.log("poprzedni");
            selectedItemRow--;
            selectedItemRow = FindPrevVisible(itemRows);
            selectedItemRow = selectedItemRow < 0 ? 0 : selectedItemRow;
        }

        console.log("selectedItemRow: " + selectedItemRow);

        if (0 <= selectedItemRow && selectedItemRow < itemRows.length) {
            var $itemRow = $("#itemRows").children().eq(selectedItemRow); //$(itemRows[selectedItemRow]);
            itemWMSId = $itemRow.attr("data-itemid");
            workstationId = $itemRow.attr("data-workstationid");
            self.GetItemDetails();
        }
    };
    this.GetItemDetails = function () {
        deliveredCounter = 0;
        console.log("GetItemDetails " + itemWMSId + ", " + workstationId);
        var jsQ = new JsonHelper().GetPostData("/iLOGIS/DeliveryList/GetItemDetails", { trainId, itemWMSId, workstationId });
        jsQ.done(function (data) {
            console.log("GetItemDetails json done");
            RefreshItemData(data);
            deliveryList._CalculateItemCoverage(data);
        });
    };
    function RefreshItemData(data) {
        var itemRow = $(document).find('.itemRow[data-itemId="' + itemWMSId + '"][data-workstationId="' + workstationId + '"]');
        $(".selectedRow").removeClass("selectedRow");
        $(itemRow).addClass("selectedRow");
        $(itemRow).attr("data-itemdeliveredqty", data.TotalQtyDelivered);

        data.TotalQtyUsed = parseInt($(itemRow).find(".summary").attr("data-itemUsedQty"));
        var qtyToBeDelivered = parseInt($(itemRow).find(".summary1").text());

        qtyPerPackage = data.QtyPerPackage;
        $(windowSelector + " #wrkstId").text(data.WorkstationId);
        $(windowSelector + " #wrkstName").text(data.WorkstationName);
        $(windowSelector + " #itemId").text(data.ItemWMSId);
        $(windowSelector + " #itemCode").text(data.ItemCode);
        $(windowSelector + " #itemName").text(data.ItemName);
        $(windowSelector + " #qtyRequested").text(data.TotalQtyRequested);
        $(windowSelector + " #qtyDelivered").text(data.TotalQtyDelivered);
        $(windowSelector + " #qtyUsed").text(data.TotalQtyUsed);
        $(windowSelector + " #qtyPerPackage").text(data.QtyPerPackage);
        $(windowSelector + " #qtyWorkstation").text(data.TotalQtyDelivered - data.TotalQtyUsed);
        $(windowSelector + " #boxWorkstation").text(((data.TotalQtyDelivered - data.TotalQtyUsed) / data.QtyPerPackage).toFixed(2));
        $(windowSelector + " #delQty").text(0);
        $(windowSelector + " #delBoxes").text(0);
        $(windowSelector + " #qtyToBeDelivered").text(qtyToBeDelivered);
        $(windowSelector + " #packagesToBeDelivered").text(
            parseInt(qtyToBeDelivered / data.QtyPerPackage) + " [+" + qtyToBeDelivered % data.QtyPerPackage + "szt]"
        );

    }
    function FindNextVisible(itemRows) {
        //:visible
        let rowsCounter = itemRows.length;
        let row = selectedItemRow;
        let found = false;
        while (!found && row < rowsCounter) {
            if ($(itemRows[row]).hasClass("hidden")) {
                row++;
            }
            else {
                found = true;
            }
        }
        return row;
    }
    function FindPrevVisible(itemRows) {
        //:visible
        let rowsCounter = itemRows.length;
        let row = selectedItemRow;
        let found = false;
        while (!found && row >= 0) {
            if ($(itemRows[row]).hasClass("hidden")) {
                row--;
            }
            else {
                found = true;
            }
        }
        return row;
    }

    this.ChangeQty = function(d) {
        if (d > 0) {
            console.log("doadje qty");
            deliveredCounter++;
        } else {
            console.log("odejmuje qty");
            deliveredCounter--;
        }

        $("#delQty").text((deliveredCounter * qtyPerPackage).toString());
        $("#delBoxes").text(deliveredCounter);

        if (deliveredCounter * qtyPerPackage >= parseInt($("#qtyToBeDelivered").text())) {
            $("#qtyToBeDelivered").addClass("qtyOK");
        }
        else {
            $("#qtyToBeDelivered").removeClass("qtyOK");
        }

    };

    this.Save = function () {
        var itemWMS = parseInt($(windowSelector + " #itemId").text());
        var wrkstId = parseInt($(windowSelector + " #wrkstId").text());
        var qty = parseInt($(windowSelector + " #delQty").text());
        ChangeItemQty(itemWMS, wrkstId, qty);
    };
    this.ClearStock = function () {
        let wQty = parseInt($(windowSelector + " #qtyWorkstation").text());
        var itemWMS = parseInt($(windowSelector + " #itemId").text());
        var wrkstId = parseInt($(windowSelector + " #wrkstId").text());
        var qty = 0 - wQty;
        ChangeItemQty(itemWMS, wrkstId, qty);
    };
    this.ConfirmDelivery = function () {
        let wQty = 0;
        let dQty = parseInt($(windowSelector + " #deliveredQty").val());
        let qty = dQty + wQty;
        let itemWMSId = parseInt($(windowSelector + " #btnConfirmDelivery").attr("data-itemId"));
        let wrkstId = parseInt($(windowSelector + " #btnConfirmDelivery").attr("data-wrkstId"));
        ChangeItemQty(itemWMSId, wrkstId, qty);
    };
    this.ConfirmInventory = function () {
        let wQty = parseInt($(windowSelector + " #workstationQty").val());
        let iQty = parseInt($(windowSelector + " #inventoryQty").val());
        let qty = iQty - wQty;
        let itemWMSId = parseInt($(windowSelector + " #btnConfirmInventory").attr("data-itemId"));
        let wrkstId = parseInt($(windowSelector + " #btnConfirmInventory").attr("data-wrkstId"));
        ChangeItemQty(itemWMSId, wrkstId, qty);
    };

    function ChangeItemQty (itemWMSId, workstationId, qty) {
        var jsQ = new JsonHelper().GetPostData("/iLOGIS/DeliveryList/ChangeItemQty", {trainId, itemWMSId, workstationId, qty });
        jsQ.done(function (data) {
            //self.GetItemDetails();
            wnd.Close();
            AddTransporterLog(itemWMSId, workstationId, qty);
            deliveryList.Refresh();
        });
    }
    function AddTransporterLog (itemWMSId, workstationId, qty) {
        var jsQ = new JsonHelper().GetPostData("/iLOGIS/DeliveryList/AddTransporterLog", { trainId, itemWMSId, workstationId, qty });
        jsQ.done(function (data) { });
    }

    
    //-------------------------------GETTERS&SETTERS---------------------------
    this.SetItemWMSId = function (v) { itemWMSId = v; };
    this.SetWorkstationId = function (v) { workstationId = v; };
    this.SetQtyPerPackage = function (v) { qtyPerPackage = v; };
    this.SetDeliveredCounter = function (v) { deliveredCounter = v; };
    this.SetSelectedItemRow = function (v) { selectedItemRow = v; };
    this.GetCurrentWindow = function () { return wnd; };
}