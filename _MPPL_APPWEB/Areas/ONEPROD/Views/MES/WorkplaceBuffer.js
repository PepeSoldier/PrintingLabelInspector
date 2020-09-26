function MesWorkplaceBuffer(_mesWorkplace)
{
    var self = this;
    var wnd = null;
    let mesWorkplace = _mesWorkplace;
    let listMode = 0; //0 - for ordes, 1 - whole  buffer items
    //let barcodeTemplate = "CCCCCCCCC-QQQQQQ-LLLLLL-SSSSSS";
    let barcodeTemplate = "9SSSSSSSSSS1QQQQQDD";

    this.Init = function () {
        console.log("MesWorkplaceBuffer Init");
        Actions();
        $("#btnShowWorkplaceBuffer").removeClass("hidden");
    };
    this.LoadView = function () {
        if (wnd != null) {
            wnd.Close();
            wnd = null;
        }
        else {

            if (mesWorkplace.MesWorkplaceWorkorder.SelectedWorkorder.RemainQty > 0) {
                wnd = new PopupWindow(1160, 800, 143, 8);
                wnd.Init("ScanItemsWindow", "Zeskanuj poszczególne produkty", () => { wnd = null; });
                wnd.AddClass("ScanItemsWindow");
                wnd.Show("loading...");

                let jsShowScanItems = new JsonHelper().GetData("/ONEPROD/MES/WorkplaceBufferView", {});
                jsShowScanItems.done(function (view) {
                    wnd.Show(view);
                    LoadItems();
                });
            }
            else {
                new Alert().Show("danger", "Wybierz zamówienie");
            }
        }
    };

    function LoadItems() {
        selectedWorkorderId = listMode == 1 ? 0 : mesWorkplace.MesWorkplaceWorkorder.SelectedWorkorder.Id;

        let jsPLT = new JsonHelper().GetPostData("/ONEPROD/MES/WorkplaceBufferGetItems",
            {
                workplaceId: mesWorkplace.Id,
                selectedWorkorderId
            });
        jsPLT.done(function (scanItemsViewModel) {
            if (scanItemsViewModel != null) {
                _DrawBufferItems(scanItemsViewModel);
                SetViewUpToMode();
            }
        });
    }
    function SetViewUpToMode() {

        $(".btnModeSwitcher").removeClass("selected");
        
        if (listMode == 0) {
            $("#infoMode1").addClass("hidden");
            $("#infoMode0").removeClass("hidden");
            $("#btnOrderMode").addClass("selected");
            $("#bufferItemsHeader").find(".QtyRequested").text("ILOŚĆ WYMAGANA");
            $("#bufferItemsHeader").find(".qtySlash").removeClass("hidden");
            $("#bufferItemsHeader").find(".qtyOrWo").removeClass("text-center");
            $("#bufferItemsHeader").find(".TimeLoaded").text("DATA ZAŁADOWANIA");
        } else {
            $("#infoMode0").addClass("hidden");
            $("#infoMode1").removeClass("hidden");
            $("#btnWholeBufferMode").addClass("selected");
            $("#bufferItemsHeader").find(".QtyRequested").text("NR ZLECENIA");
            $("#bufferItemsHeader").find(".qtySlash").addClass("hidden");
            $("#bufferItemsHeader").find(".qtyOrWo").addClass("text-center");
            $("#bufferItemsHeader").find(".TimeLoaded").text("DATA / NR SERYJNY");
        }
    }
    function ParseBarcode(barcode) {
        let jsPLT = new JsonHelper().GetPostData("/iLOGIS/StockUnit/GetByBarcode",
            {
                barcode: barcode,
                template: barcodeTemplate
            });
        jsPLT.done(function (StockUnitViewModel) {
            $("#ItemCode").val(StockUnitViewModel.ItemCode);
            $("#Qty").val(StockUnitViewModel.CurrentQtyinPackage);
            $("#SerialNumber").val(StockUnitViewModel.SerialNumber);
        });
        jsPLT.fail(function () {
            new Alert().Show("danger", "Wystąpił problem...");
        });
    }
    function AddItem() {
        let barcode = $("#inputBarcodeField").val();
        let itemCode = $("#ItemCode").val();
        let qty = $("#Qty").val();
        let serialNumber = $("#SerialNumber").val();

        let jsPLT = new JsonHelper().GetPostData("/ONEPROD/MES/WorkplaceBufferAddItem",
           {
               workplaceId : mesWorkplace.Id,
               selectedWorkorderId: mesWorkplace.MesWorkplaceWorkorder.SelectedWorkorder.Id,
               itemCode,
               qty,
               serialNumber,
               barcode
           });
        jsPLT.done(function (jsonModel) {
            new Alert().ShowJson(jsonModel);
            LoadItems(mesWorkplace.Id);
            
        });
        jsPLT.fail(function () {
            new Alert().Show("danger", "Wystąpił problem. Brak możliwości pobrania elementów do skanowania");
        });
    }
    function RemoveItem(scannedItemId) {
        let jsPLT = new JsonHelper().GetPostData("/ONEPROD/MES/WorkplaceBufferRemoveItem", {
            workplaceBufferId: scannedItemId
        });
        jsPLT.done(function () {
            LoadItems();
        });
    }
    function _DrawBufferItems(scanItemsViewModel) {
        _ClearBufferItems();
        for (var k = 0; k < scanItemsViewModel.length; k++) {
            _DrawBufferItem(scanItemsViewModel[k], k);
        }
    }
    function _DrawBufferItem(item, k) {
        var itm = $(".scanItemGrid").find(".bufferItem")[0];
        var cln = itm.cloneNode(true);
        $(cln).removeClass("hidden");

        $(cln).attr("data-id", item.Id);
        $(cln).find(".ChildCode").text(item.ChildCode);
        $(cln).find(".ChildName").text(item.ChildName);
        $(cln).find(".QtyAvailable").text(item.QtyAvailable);
        $(cln).find(".QtyRequested").text(item.QtyRequested);
        $(cln).find(".TimeLoaded").text(item.TimeLoaded);
        $(cln).find(".QtyInBom").text(item.QtyInBom);
        $(cln).find(".SerialNumber").text(item.SerialNumber);

        if (listMode == 1) {
            
            $(cln).find(".QtyRequested").text(item.WorkorderNumber);
            $(cln).find(".QtyRequested").addClass("WorkorderNumber");
            $(cln).find(".QtyRequested").parent().addClass("text-center");
            $(cln).find(".qtySlash").addClass("hidden");
        }

        $("#bufferItems").append(cln);
    }
    function _ClearBufferItems() {
        var scanItemsClear = $(".scanItemGrid").find(".bufferItem");
        scanItemsClear.each(function () {
            if (!$(this).hasClass("hidden")) {
                $(this).remove();
            }
        });
    }
    
    //UNUSED??
    function CheckProperItemCode(barcode, workorderId) {
        let jsCheckProperItemCode = new JsonHelper().GetPostData("/ONEPROD/MES/CheckProperItemCode",
            {
                barcode: barcode,
                workorderId: workorderId
            });
        jsCheckProperItemCode.done(function (data) {
            if (data == "success") {
                new Alert().Show("success", "Zeskanowano poprawny produkt.");
            } else {
                new Alert().Show("danger", "Zeskanowano niepoprawny produkt. Zapis loga nie powiódł się");
            }
        });
        jsCheckProperItemCode.fail(function () {
            new Alert().Show("danger", "Wystąpił problem. Nie udało się porównać Kodów produktu");
        });
    }
    function ConfirmCheckScan() {
        let properItemCodes = 0;
        let itemCounter = 0;
        $(".fieldsetProperty").each(function () {
            itemCounter++
            var goodItemCode = $(this).find(".properItemCode")[0].hidden;
            var badItemCode = $(this).find(".wrongItemCode")[0].hidden;

            if (goodItemCode && !badItemCode) {
                properItemCodes++;
            }
        })
        if (itemCounter == properItemCodes) {
            wnd.Close();
            return true;
        } else {
            new Alert().Show("danger", "Wystąpił problem. Niepoprawne ItemCody");
        }
    }

    function Actions() {

        $(document).off("click", "#btnShowWorkplaceBuffer");
        $(document).on("click", "#btnShowWorkplaceBuffer", function () {
            console.log("ShowWorkplaceBuffer");
            self.LoadView();
        });

        $(document).off("click", ".btnClickFocus");
        $(document).on("click", ".btnClickFocus", function () {
            $("#inputBarcodeField").focus();
        });

        $(document).off("click", "#btnAddItem");
        $(document).on("click", "#btnAddItem", function () {
            AddItem();
        });

        $(document).off("click", "#btnParseBarcode");
        $(document).on("click", "#btnParseBarcode", function () {
            let barcode = $("#inputBarcodeField").val();
            ParseBarcode(barcode);
        });

        $(document).off("click", ".bufferItem");
        $(document).on("click", ".bufferItem", function () {
            $(".bufferItem").removeClass("selectedRow");
            $(this).addClass("selectedRow");
        });

        $(document).off("click", ".btnDelete");
        $(document).on("click", ".btnDelete", function () {
            console.log("btnDelete click");
            let scannedItemId = $(this).parent().attr("data-id");
            RemoveItem(scannedItemId);
        });

        $(document).off("click", "#btnOrderMode");
        $(document).on("click", "#btnOrderMode", function () {
            listMode = 0;
            SetViewUpToMode();
            LoadItems();
        });

        $(document).off("click", "#btnWholeBufferMode");
        $(document).on("click", "#btnWholeBufferMode", function () {
            listMode = 1;
            SetViewUpToMode();
            LoadItems();
        });
    }

    //$(document).on("focusout", ".inputBarcodeField", function () {
    //    let barcodeLength = $(this).val().trim().length;

    //    if (barcodeLength == 9) {
    //        let isItemCodeCorrect = false;
    //        let barcode = $(this).val();
    //        let itemCodeFromBom = $(this).parent().parent().find("#itemCode").text().trim();
    //        isItemCodeCorrect = CheckProperItemCode(barcode, itemCodeFromBom);

    //        if (isItemCodeCorrect) {
    //            $(this).parent().parent().find(".properItemCode")["0"].hidden = true;
    //            $(this).parent().parent().find(".wrongItemCode")["0"].hidden = false;
    //        }else{
    //            $(this).parent().parent().find(".properItemCode")["0"].hidden = false;
    //            $(this).parent().parent().find(".wrongItemCode")["0"].hidden = true;
    //        }
    //    }
    //});

    //function ShowScanItemsWindow(bomChildList, rawBomChildList) {
    //    try {
    //        if (wnd !== null)
    //            wnd.Close();
    //    }
    //    catch (e) {
    //        console.log("catch exception");
    //    }

    //    wnd = new PopupWindow(960, 800, 143, 8);
    //    wnd.Init("ScanItemsWindow", "Zeskanuj poszczególne produkty");
    //    wnd.AddClass("ScanItemsWindow");
    //    wnd.Show("loading...");

    //    let jsShowScanItems = new JsonHelper().GetPostDataAwait("/ONEPROD/MES/ScanItemView",
    //      {
    //          data: bomChildList
    //      });
    //    jsShowScanItems.done(function (viewWithScanItemModelList) {
    //        if (viewWithScanItemModelList != null) {
    //            wnd.Show(viewWithScanItemModelList);
    //            $("#ConfirmCheckScanItemsSubmit").on("click", function () {
    //                var isScanOk = ConfirmCheckScan();
    //                if (isScanOk) {
    //                    console.log(rawBomChildList); // Tutaj powracam do funkcji, gdzie mam potwierdzone wszystkie barcody, że jest ok i mam informacje o ProductionLog'ach z tych itemow.
    //                }
    //            });
    //        }
    //    });
    //}
        //this.ScanItems = function () {
    //    let jsPLT = new JsonHelper().GetPostData("/ONEPROD/MES/ScanItems",
    //       {
    //           selectedWorkorderId: mesWorkplace.GetSelectedWorkorderId()
    //       });
    //    jsPLT.done(function (bomChildList) {
    //        if (bomChildList != null) {
    //            ShowScanItemsWindow(bomChildList, bomChildList);
    //        }
    //    });
    //    jsPLT.fail(function () {
    //        new Alert().Show("danger", "Wystąpił problem. Brak możliwości pobrania elementów do skanowania");
    //    });
    //};
}