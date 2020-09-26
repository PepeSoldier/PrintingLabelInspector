function PickingListItemManage(PickingListItemId, ScannedBarcode, defaultTrolley, callback)
{
    var pickingListShared = new PickingListShared();
    var keypadPlatform = new KeyPadDigitDoubleRows("#Platform_", "#kaypad114", "P");
    var keypadQtyPicked = new KeyPadDigitDoubleRows("#QtyPicked_", "#kaypad113", ".");
    var wnd = new PopupWindow("100%", "100%", 0, 0);
    var viewModel = {};
    var _defaultTrolley = defaultTrolley;
    Init();

    function Init() {
        LoadView();
        //InitOnScan(function (sCode, iQty) {
        //    AssignScannedBarcode(sCode);
        //});
        UninitOnScan();
        Actions();
    }

    function LoadView() {
        var JsonHelp = new JsonHelper();
        var ReturnJson = JsonHelp.GetData(pickingListShared.urlPickingListItemManage, {});
        ReturnJson.done(function (view) {
            wnd.Init("mainMenu", "Wprowadź ilość");
            wnd.Show(view);
            GetData();
        });
    }
    function GetData() {
        //funkcja nazywała się LoadContent
        ShowLoadingSnippetOnElement("#PickingListItemManageView");
        let JsonHelp = new JsonHelper();
        let ReturnJson = JsonHelp.GetPostData(pickingListShared.urlPickingListItemManageGetData, {
            pickingListItemId: PickingListItemId
        });
        ReturnJson.done(function (PickingListItemManageViewModel) {
            viewModel = PickingListItemManageViewModel;
            viewModel.QtyRemainToPick = viewModel.QtyRequested - viewModel.QtyPicked;
            viewModel.IsMaxItemsSet = viewModel.MaximumItemNumbersToPackage == 0 ? false : true;
            viewModel.PickingListItemLocationNameFormat = FormatWarehouseLocationName(viewModel.PickingListItemLocationName);
            viewModel.isLocationSet = viewModel.PickingListItemLocationNameFormat == "BRAK" ? false : true;
            viewModel.Barcode = ScannedBarcode;
            viewModel.PickingListItemPlatformName = viewModel.PickingListItemPlatformName == "" ? _defaultTrolley : viewModel.PickingListItemPlatformName;
            viewModel.ItemCodeScanned = "";
            viewModel.SerialNumberScanned = "";
            viewModel.LocationScanned = "";
            viewModel.QtyScanned = 0;
            viewModel.BarcodeFocusInput = "";
            Render();
            AssignScannedBarcode(); //BarcodeSetManage();
            SetCommentListSelectedOption();
        });
    }
    function Save() {
        CopyFormValuesToViewModel();
        if (IsDataValid()) {
            ShowLoadingSnippetOnElement("#PickingListItemManageView");
            var jsQ = new JsonHelper().Update(pickingListShared.urlPickingListItemManageSave, { pickingListItemManage: viewModel });
            jsQ.done(function (jsonModel) {
                RemoveLoadingSnippetFromElement("#PickingListItemManageView");
                let enumPickingListItemStatus = null;
                if (jsonModel.Status > 0) {
                    new Alert().Show("danger", TranslateStatus(jsonModel.Status) + ". " + jsonModel.Data);
                }
                else {
                    enumPickingListItemStatus = jsonModel.Data;
                }
                wnd.Close();
                callback(enumPickingListItemStatus, viewModel.PickingListItemPlatformName);
            });
            jsQ.fail(function () {
                RemoveLoadingSnippetFromElement("#PickingListItemManageView");
                wnd.Close();
                callback(8, ""); //8 nie istnieje w Enumie
            });
        }
    }

    function TryToFindNewLocations() {
        ShowLoadingSnippetOnElement("#PickingListItemManageView");
        var jsQ = new JsonHelper().Update(pickingListShared.urlPickingListItemManageSave, { pickingListItemManage: viewModel });
        jsQ.done(function (jsonModel) {
            RemoveLoadingSnippetFromElement("#PickingListItemManageView");
            let enumPickingListItemStatus = null;
            if (jsonModel.Status > 0) {
                new Alert().Show("danger", TranslateStatus(jsonModel.Status) + ". " + jsonModel.Data);
            }
            else {
                enumPickingListItemStatus = jsonModel.Data;
            }
            wnd.Close();
            callback(enumPickingListItemStatus, viewModel.PickingListItemPlatformName);
        });
        jsQ.fail(function () {
            RemoveLoadingSnippetFromElement("#PickingListItemManageView");
            wnd.Close();
            callback(8, ""); //8 nie istnieje w Enumie
        });
    }

    function IsDataValid() {
        if (isPlatformSyntaxAndNumberCorrect())
        {
            $("#Platform_").removeClass("required");

            if (viewModel.SerialNumberScanned != null 
                && viewModel.SerialNumberScanned.length > 0
                && viewModel.PickingListItemCode != viewModel.ItemCodeScanned)
            {
                BootBoxAlert("itemCode", "Zeskanowano różne kody ANC");
                return false;
            }

            if (isQtyPickedGreaterThanZero() || isCommentDifferentThanZero()) {
                $("#QtyPicked_").removeClass("required");
                $("#Comment_").removeClass("required");
                //Save();
                return true;
            }
            else {
                BootBoxAlert("QtyPicked_", "Wprowadź ilość większą od 0, lub wybierz komentarz");
                $("#Comment_").addClass("required");
                return false;
            }            

        }
        else {
            $("#QtyPicked_").removeClass("required");
            $("#Comment_").removeClass("required");
            BootBoxAlert("Platform_", "Uzupełnij poprawnie Platformę");
            return false;
        }
    }
    function CopyFormValuesToViewModel() {
        //funkcja nazywała się GetInputFields
        viewModel.PickingListItemPlatformName = $("#Platform_").val();
        viewModel.QtyPicked = $("#QtyPicked_").val();
        viewModel.CommentListId = $("#Comment_").val();
        viewModel.Barcode = $("#Barcode_").val();
        viewModel.CommentItemString = viewModel.CommentList[viewModel.CommentListId].Text;
    }
    function SetCommentListSelectedOption() {
        if (viewModel.isLocationSet) {
            let selectedOption = $("#Comment_")[0].options[viewModel.CommentListId];
            $(selectedOption).attr("selected", "selected");
        }
    }
    function DeleteScannedBarcode() {
        $("#Barcode_").val("");
        AssignScannedBarcode();
        //SimulateScan("#0250292900003800000###300110###030007652")
        //SimulateScan("#0230015500000200000#####9103###034026653")
        //SimulateScan("#0614352700003600000#####9103###034023700")
    }
    function AssignScannedBarcode() {
        console.log("PickingListItemManage.AssignScannedBarcode");
        let scannedBarcode = $("#Barcode_").val();

        if (scannedBarcode != null && scannedBarcode.length > 0)
        {
            viewModel.isBarcodeSet = true;
            viewModel.Barcode = scannedBarcode.replace("\u0010", "");
            //viewModel.QtyPickedFocusInput = "focusClass";
            //$("#QtyPicked_").addClass("focusClass");
            //$("#Barcode_").removeClass("focusClass");
            $("#btnAssignBarcode").addClass("hidden");
            $("#btnClearBarcode").removeClass("hidden");

            ParseBarcodeAndValidate().then(function () {
                viewModel.isDifferentSerialNumber = viewModel.StockUnitSerialNumber != viewModel.SerialNumberScanned ? true : false;
                viewModel.BarcodeItemCodeColor = viewModel.PickingListItemCode == viewModel.ItemCodeScanned ? "green" : "red";
                viewModel.BarcodeLocationNameColor = viewModel.PickingListItemLocationName == viewModel.LocationScanned ? "green" : "red";

                $("#itemCodeVerification").css("background-color", viewModel.BarcodeItemCodeColor);
                $("#locationVerification").css("background-color", viewModel.BarcodeLocationNameColor);
                $("#btnQtyFromBarcode").text(viewModel.QtyScanned);

                if (viewModel.isDifferentSerialNumber) {
                    $("#stockUnitSerialNumber").addClass("line-through");
                    $("#SerialNumberScanned").text(viewModel.SerialNumberScanned);
                    new Alert().Show("warning", "Pobierasz ilości z opakowania o innym numerze seryjnym");
                }
                else {
                    $("#stockUnitSerialNumber").removeClass("line-through");
                    $("#SerialNumberScanned").text("");
                }
            });
        }
        else {
            viewModel.isBarcodeSet = false;
            viewModel.Barcode = "";
            //viewModel.BarcodeFocusInput = "focusClass";
            $("#btnAssignBarcode").removeClass("hidden");
            $("#btnClearBarcode").addClass("hidden");

            viewModel.ItemCodeScanned = "";
            viewModel.SerialNumberScanned = "";
            viewModel.LocationScanned = "";
            viewModel.QtyScanned = 0;

            viewModel.isDifferentSerialNumber = false;
            viewModel.BarcodeItemCodeColor = "transparent";
            viewModel.BarcodeLocationNameColor = "transparent";

            $("#itemCodeVerification").css("background-color", viewModel.BarcodeItemCodeColor);
            $("#locationVerification").css("background-color", viewModel.BarcodeLocationNameColor);
            $("#btnQtyFromBarcode").text(0);
            $("#stockUnitSerialNumber").removeClass("line-through");
            $("#SerialNumberScanned").text("");
        }
    }
    //function BarcodeSetManage() {
    //    if (viewModel.isBarcodeSet) {
    //        viewModel.Barcode = ScannedBarcode.replace("\u0010", "");
    //        ParseBarcodeAndValidate();
    //        viewModel.QtyPickedFocusInput = "focusClass";
    //    } else {
    //        viewModel.Barcode = "";
    //        viewModel.BarcodeFocusInput = "focusClass";
    //        viewModel.BarcodeQty = 0;
    //    }
    //}
    function ParseBarcodeAndValidate() {

        return new Promise((resolve, reject) => {
            viewModel.Barcode = viewModel.Barcode.replace(".", "");
            viewModel.Barcode = viewModel.Barcode.replace(",", "");
            viewModel.Barcode = viewModel.Barcode.replace(" ", "");
            viewModel.Barcode = viewModel.Barcode.replace("\u0010", "");
            $("#Barcode_").val(viewModel.Barcode);

            new JsonHelper().GetPostData("/CORE/Common/ParseBarcode", { barcode: viewModel.Barcode, template: viewModel.BarcodeTemplate })
                .done(function (barcodeParsedViewModel) {
                    if (barcodeParsedViewModel.ErrorText != null) {

                        viewModel.ItemCodeScanned = barcodeParsedViewModel.ItemCode;
                        viewModel.SerialNumberScanned = barcodeParsedViewModel.SerialNumber;
                        viewModel.LocationScanned = barcodeParsedViewModel.Location;
                        viewModel.QtyScanned = barcodeParsedViewModel.Qty;
                        resolve();
                    }
                    else {
                        new Alert().Show("warning", barcodeParsedViewModel.ErrorText);
                        reject();
                    }
                })
                .fail(function () {
                    reject();
                });
        });
    }

    function ChangeQty(d) {
        dFloat = parseFloat(d);
        let qtyPicked = parseFloat($("#QtyPicked_").val()).toFixed(2);
        qtyPicked = parseFloat(qtyPicked) + parseFloat(dFloat);
        $("#QtyPicked_").val(qtyPicked.toFixed(2));
        //if (qtyPicked > 0) {
        //    $('#Platform_').removeAttr('disabled');
        //} else {
        //    $("#Platform_").attr('disabled', 'disabled');
        //    $("#Platform_").removeClass("required");
        //}
    }
    function FormatWarehouseLocationName(item) {
        if (item == "BR-AK-") {
            return "BRAK";
        } else {
            return item;
        }
    }
    function BootBoxAlert(element, message) {
        bootbox.alert(message, function () {
            $("#" + element).addClass("required");
        });
        $('.bootbox').on('hidden.bs.modal', function () {
            $("#" + element).focus();
        });
    }

    function isQtyPickedGreaterThanZero() {
        return parseFloat(viewModel.QtyPicked) > 0 ? true : false;
    }
    function isCommentDifferentThanZero() {
        return viewModel.CommentListId != 0 ? true : false;
    }
    function isCommentSetToNoItems(commentVal) {
        return commentVal == 1 ? true : false;
    }
    function isPlatformSyntaxAndNumberCorrect() {
        let reg = /^\b[A-Z]{1}[0-9]{3}\b|^\b[A-Z]{2}[0-9]{2}\b/;
        return reg.test(viewModel.PickingListItemPlatformName); 
    }
        
    function Render() {
        RenderTemplate("#PickingListItemManageTemplate", "#PickingListItemManageView", viewModel);
    }

    function Actions() {
        $(document).off("click", "#btnClearBarcode");
        $(document).on("click", "#btnClearBarcode", function (event) {
            DeleteScannedBarcode();
        });

        $(document).off("click", "#btnAssignBarcode");
        $(document).on("click", "#btnAssignBarcode", function (event) {
            AssignScannedBarcode();
        });

        $(document).off("focus", "#QtyPicked_");
        $(document).on("focus", "#QtyPicked_", function (event) {
            keypadPlatform.Close();
            keypadQtyPicked.Init();
        });

        //$(document).off("focus", "#Platform_");
        //$(document).on("focus", "#Platform_", function (event) {
        //    keypadQtyPicked.Close();
        //    keypadPlatform.Init();
        //});

        $(document).off("click", "#btnMinus_");
        $(document).on("click", "#btnMinus_", function (event) {
            let QtySave = $("#QtyPicked_").val();
            if (QtySave > 0) {
                ChangeQty(-1);
            } else {
                bootbox.alert("Nie możesz podać ujemnych wartości", function () { });
            }
        });
        $(document).off("click", "#btnPlus_");
        $(document).on("click", "#btnPlus_", function (event) {
            ChangeQty(1);
        });

        $(document).off("click", "#btnQtyFromBarcode");
        $(document).on("click", "#btnQtyFromBarcode", function () {
            $("#QtyPicked_").val($(this).text());
        });

        $(document).off("click", "#btnBack_");
        $(document).on("click", "#btnBack_", function (event) {
            wnd.Close();
            pickingListItem.AddOnScan();
        });

        $(document).off("click", "#btnFindNewLocation");
        $(document).on("click", "#btnFindNewLocation", function (event) {
            viewModel.QtyPicked = 0;
            TryToFindNewLocations();
        });

        $(document).off("click", "#btnSave_");
        $(document).on("click", "#btnSave_", function (event) {
            Save();
        });

        $(document).off('change', "#Comment_");
        $(document).on('change', "#Comment_", function () {
            if (isCommentSetToNoItems($(this).val())) {
                $("#QtyPicked_").val(0);
                viewModel.QtyPicked = 0;
            };
        });
    }
}



