function MovementMobile(_barcodeTemplate) {
    console.log("MovementMobile");
    var self = this;
    var barcodeTemplate = _barcodeTemplate;
    var defaultValues = {
        ItemCode: "",
        QtyToMove: 0,
        DestinationLocation: {
            Id: 0,
            Name: "",
            WarehouseId: 0,
            WarehouseName: "",
            WarehouseCode: ""
        },
        LocationMode: "A",
        ManualMode: false,
    };
    var viewModel = {
        StockUnit: {
            Id: 0,
            ItemCode: "",
            CurrentQtyinPackage: 0,
            SerialNumber: "",
            MaxQtyPerPackage: 0,
            WarehouseLocationId: 0,
            WarehouseLocationName: "",
            WarehouseId: 0,
            WarehouseName: "",
            QtyToMove: 0
        },
        DestinationLocation: {
            Id: 0,
            Name: "",
            WarehouseId: 0,
            WarehouseName: "WH Name",
            WarehouseCode: "WH Code"
        },
        LocationMode: "A",
        ManualMode: false,
        ModeChangeActive: true,
        ShowBarcodeInput: true,
        PickIcon: false,
        DeliverIcon: false,
        BtnText: "POTWIERDŹ SKAN",
        BtnId: "scanButton",
        BtnColSize: "col-4",
        MarginBottomScanSection: "mb-auto"
    };

    this.Init = function () {
        viewModel.StockUnit = {};
        viewModel.StockUnit.ItemCode = defaultValues.ItemCode;
        viewModel.DestinationLocation = defaultValues.DestinationLocation;
        viewModel.ManualMode = defaultValues.ManualMode;
        viewModel.LocationMode = defaultValues.LocationMode;

        ShowParseButton();
        Render();
        Actions();
        AddSwipeAutoManual();
        SetSwipeAutoManual();
        $("#barcode").focus();
    };

    function SetDefaultValues() {
        defaultValues.QtyToMove = $("#QtyToMove").val();
        defaultValues.ItemCode = viewModel.StockUnit.ItemCode;
        defaultValues.LocationMode = viewModel.LocationMode;
        defaultValues.DestinationLocation = viewModel.DestinationLocation;
        defaultValues.ManualMode = viewModel.ManualMode;
    }
    function SetSwipeAutoManual() {
        if (viewModel.LocationMode == "M" && viewModel.ManualMode == true) {
            viewModel.LocationMode = "M";
            viewModel.ManualMode = true;
            $("#manualModeInput").removeClass("hidden");
            $("#autoModeInput").addClass("hidden");
            $("#AutoManualLocationMode").html(viewModel.LocationMode);
        }
    }

    function ParseBarcode() {
        let barcode = $("#barcode").val();
        
        let json = new JsonHelper().GetPostData("/CORE/Common/ParseBarcode", { barcode, template: barcodeTemplate });
        json.done(function (barcodeParsedViewModel) {
            console.log("/CORE/Common/ParseBarcode.Done");
            viewModel.StockUnit.ItemCode = barcodeParsedViewModel.ItemCode;
            viewModel.StockUnit.CurrentQtyinPackage = barcodeParsedViewModel.Qty;
            viewModel.StockUnit.SerialNumber = barcodeParsedViewModel.SerialNumber;
            viewModel.StockUnit.CurrenLocationName = barcodeParsedViewModel.Location;
            viewModel.StockUnit.CurrenWarehouseName = "MG-JSON";

            console.log("barcodeParsedViewModel");

            if (barcodeParsedViewModel.SerialNumber != null) {
                GetStockUnitByIdOrSerialNumber(0, viewModel.StockUnit.SerialNumber);
            }
            else {
                new Alert().Show("warning", "Dekodowanie: " + barcodeParsedViewModel.ErrorText);
            }

            $("#barcode").focus();
        });
    }
    function GetStockUnitByIdOrSerialNumber(id, serialNumber) {
        let jsonSU = new JsonHelper().GetPostData("/iLOGIS/StockUnit/GetByIdOrSerialNumber", {
            Id: id,
            serialNumber: serialNumber
        });
        jsonSU.done(function(stockUnitViewModel) {
            viewModel.StockUnit = stockUnitViewModel;
            viewModel.StockUnit.WarehouseName = stockUnitViewModel.WarehouseName != null ? stockUnitViewModel.WarehouseName : "?";
            viewModel.StockUnit.QtyToMove = stockUnitViewModel.CurrentQtyinPackage;
            viewModel.DestinationLocation.WarehouseId = viewModel.StockUnit.WarehouseId;
            viewModel.DestinationLocation.WarehouseName = viewModel.StockUnit.WarehouseName;
            if (stockUnitViewModel.Id > 0) {
                viewModel.PickIcon = true;
                viewModel.ModeChangeActive = false;
                viewModel.ShowBarcodeInput = false;
                ShowClearButton();
                Render();
            }
            else {
                new Alert().Show("warning", "Nie znaleziono opakowania dla podanego kodu");
            }
        });
    }
    function GetStockUnitByCodeAndLocation() {
        let itemCode = $("#itemCode1").val();
        let locationName = $("#locationName1").val();

        let jsonSU = new JsonHelper().GetPostData("/iLOGIS/StockUnit/GetByCodeAndLocation", {
            itemCode, locationName
        });
        jsonSU.done(function (stockUnitViewModel) {
            viewModel.StockUnit = stockUnitViewModel;
            viewModel.StockUnit.WarehouseName = stockUnitViewModel.WarehouseName != null ? stockUnitViewModel.WarehouseName : "?";
            viewModel.StockUnit.QtyToMove = stockUnitViewModel.CurrentQtyinPackage;
            viewModel.DestinationLocation.WarehouseId = viewModel.StockUnit.WarehouseId;
            viewModel.DestinationLocation.WarehouseName = viewModel.StockUnit.WarehouseName;
            if (stockUnitViewModel.Id > 0) {
                viewModel.PickIcon = true;
                viewModel.ModeChangeActive = false;
                viewModel.ShowBarcodeInput = false;
                ShowClearButton();
                Render();
            }
            else {
                new Alert().Show("warning", "Nie znaleziono opakowania dla podanego kodu");
            }
        });
    }

    function ShowWarehouseSelector() {
        console.log("Movement.ShowWarehouseSelector");
        CheckBoxForceVisible(false);
        //ws = new WarehouseSelector(viewModel.DestinationLocation.WarehouseId, viewModel.DestinationLocation.WarehouseName);
        ws = new WarehouseSelector(0, "");
        ws.SelectWarehouse()
            .then(
                function (result) {
                    /* handle a successful result */
                    console.log("Wybrałem Warehouse");
                    console.log(result);
                    WarehouseSelectedCallback(result);
                },
                function (error) {
                    /* handle an error */
                    console.log("Nie wybrałem warehouse:" + error);
                }
            );
    }
    function WarehouseSelectedCallback(selectedWarehouse) {
        console.log("WarehouseSelectedCallback");
        viewModel.DestinationLocation.WarehouseName = selectedWarehouse.WarehouseName;
        viewModel.DestinationLocation.WarehouseId= selectedWarehouse.WarehouseId;
        $("#destinationWarehouse").text(viewModel.DestinationLocation.WarehouseName);
        $("#destinationWarehouse").attr("data-warehouseid", viewModel.DestinationLocation.WarehouseId);

        if (selectedWarehouse.LocationName == "0" || selectedWarehouse.LocationName == "-") {
            _setNoLocation();
        }
        else if (selectedWarehouse.LocationName != null && selectedWarehouse.LocationName.length > 0) {
            //_getLocationByName(selectedWarehouse);
            _GetLocation(selectedWarehouse.LocationName, selectedWarehouse.WarehouseId);
        }
        else {
            _findLocationOnSelectedWareohouse(selectedWarehouse);
        }
    }
    function _setNoLocation() {
        viewModel.DestinationLocation.Name = "-";
        viewModel.DestinationLocation.Id = -1;
        $("#destinationLocation").val(viewModel.DestinationLocation.Name);
        $("#destinationLocation").attr("data-locationid", viewModel.DestinationLocation.Id);
        ShowExecuteButton();
        Render();
        new Alert().Show("info", "Zostanie wykonane przeniesienie bez lokalizowania");
    }
    function _findLocationOnSelectedWareohouse(selectedWarehouse) {
        var JsonHelp = new JsonHelper();
        var ReturnJson = JsonHelp.GetPostData("/iLOGIS/WarehouseLocation/FindEmptyLocation", {
            itemCode: viewModel.StockUnit.ItemCode,
            qtyPerPackage: viewModel.StockUnit.MaxQtyPerPackage,
            warehouseId: selectedWarehouse.WarehouseId
        });
        ReturnJson.done(function (warehouseLocationViewModel) {
            viewModel.DestinationLocation.Id = warehouseLocationViewModel.Id;
            viewModel.DestinationLocation.Name = warehouseLocationViewModel.Name;

            if (viewModel.DestinationLocation.Id > 0) {
                new Alert().Show("success", "Znaleziono automatycznie lokację " + viewModel.DestinationLocation.Name);
                ShowExecuteButton();
                Render();
            }
            else {
                new Alert().Show("info", "Nie udało się znaleźć automatycznie lokacji");
            }

            $("#destinationLocation").val(viewModel.DestinationLocation.Name);
            $("#destinationLocation").attr("data-locationid", viewModel.DestinationLocation.Id);
        });
        ReturnJson.fail(function (warehouseLocationViewModel) {
                new Alert().Show("info", "Nie udało się znaleźć automatycznie lokacji");
        });
    }

    function GetLocattion() {
        CheckBoxForceVisible(false);
        let locationName = $("#destinationLocation").val();

        if (locationName == "0" || locationName == "-") {
            _setNoLocation();
        }
        else {
            _GetLocation(locationName, viewModel.DestinationLocation.WarehouseId);
        }
    }
    function _GetLocation(locationName, warehouseId) {
        let jsonGetLocation = new JsonHelper().GetPostData("/iLOGIS/WarehouseLocation/GetLocation", {
            nameFormatted: locationName,
            warehouseId: warehouseId
        });
        jsonGetLocation.done(function (warehouseLocationViewModelList) {
            //console.log(warehouseLocationViewModelList);
            if (warehouseLocationViewModelList.length <= 0) {
                console.log("Nie udało się znaleźć lokacji");
                new Alert().Show("info", "Nie udało się znaleźć lokacji");
                viewModel.DestinationLocation.Id = 0;
                viewModel.DestinationLocation.Name = "";
                $("#destinationLocation").attr("data-locationid", 0);
                $("#destinationLocation").val("");
                $("#btnExecute").addClass("disabled");
            }
            else {
                if (warehouseLocationViewModelList.length > 1) {
                    new Alert().Show("warning", "Znaleziono wiele pasujących lokacji. Wybierz Magazyn docelowy!");

                    let wlvmList = []; //warehouseSelectorViewModelList
                    for (let w = 0; w < warehouseLocationViewModelList.length; w++) {
                        wlvmList.push({
                            Id: warehouseLocationViewModelList[w].WarehouseId,
                            Name: warehouseLocationViewModelList[w].WarehouseName,
                            Code: warehouseLocationViewModelList[w].WarehouseCode
                        });
                    }

                    ws = new WarehouseSelector(0, "");
                    ws.SetCustomList(wlvmList, warehouseLocationViewModelList[0].Name);
                    ws.SelectWarehouse()
                        .then(
                            function (result) {
                                console.log("Wybrałem Warehouse");
                                console.log(result);
                                WarehouseSelectedCallback(result);
                            },
                            function (error) {
                                console.log("Nie wybrałem warehouse:" + error);
                            }
                        );
                }
                else {
                    $("#btnExecute").removeClass("disabled");
                    let whLocVM = warehouseLocationViewModelList[0];

                    if (whLocVM.WarehouseId != viewModel.DestinationLocation.WarehouseId) {
                        new Alert().Show("warning", "Znaleziono lokacje na innym magazynie");
                    }
                    else {
                        new Alert().Show("success", "Znaleziono lokację");
                        //viewModel.DestinationLocation.Id = whLocVM.Id;
                        //viewModel.DestinationLocation.Name = whLocVM.Name;
                        //$("#destinationLocation").attr("data-locationid", viewModel.DestinationLocation.Id);
                        //$("#destinationLocation").val(viewModel.DestinationLocation.Name);

                        //ShowExecuteButton();
                        //Render();
                    }
                    viewModel.DestinationLocation.Id = whLocVM.Id;
                    viewModel.DestinationLocation.Name = whLocVM.Name;
                    viewModel.DestinationLocation.WarehouseName = whLocVM.WarehouseName;
                    viewModel.DestinationLocation.WarehouseCode = whLocVM.WarehouseCode;
                    viewModel.DestinationLocation.WarehouseId = whLocVM.WarehouseId;
                    $("#destinationWarehouseCode").text(viewModel.DestinationLocation.WarehouseCode);
                    $("#destinationWarehouse").text(viewModel.DestinationLocation.WarehouseName);
                    $("#destinationWarehouse").attr("data-warehouseid", viewModel.DestinationLocation.WarehouseId);
                    $("#destinationLocation").attr("data-locationid", viewModel.DestinationLocation.Id);
                    $("#destinationLocation").val(viewModel.DestinationLocation.Name);

                    ShowExecuteButton();
                    Render();
                }
            }
        });
    }
    function GetAlternativeUnitOfMeasures(itemCode) {
        new JsonHelper().GetPostData("/MASTERDATA/Item/ItemAlternativeUnitOfMeasures", { itemCode })
            .done(function (unitOfMeasures) {
                unitOfMeasures.forEach(function (item) {
                    console.log(ConvertUoM(item));
                });

                RenderTemplate("#UnitOfMeasureListTemplate", "#btnChangeUoM", { UnitOfMeasures: unitOfMeasures }, true);
            });
    }
    function ChangeQtyToMove(val) {
        let qtyToMove = parseFloat($("#QtyToMove").val());
        qtyToMove = qtyToMove + val;
        qtyToMove = viewModel.StockUnit.SerialNumber != "0"? Math.min(qtyToMove, viewModel.StockUnit.CurrentQtyinPackage) : qtyToMove;
        qtyToMove = Math.max(qtyToMove, 0);
        viewModel.StockUnit.QtyToMove = qtyToMove;
        $("#QtyToMove").val(viewModel.StockUnit.QtyToMove);
    }
    function MoveToLocation() {
        DisableElement("#btnExecute");
        ShowLoadingSnippetWithOverlay();
        viewModel.StockUnit.QtyToMove = $("#QtyToMove").val();
        let force = $("#cbForce").prop("checked");
        let discartSerialNumber = false; //$("#cbDiscartSerialNumber").prop("checked");
        let unitOfMeasure = $("#UnitOfMeasure").attr("data-UoM");
        CheckBoxForceVisible(false);

        var mtl_json = new JsonHelper().GetPostData("/iLOGIS/StockUnit/MoveToLocation",
            {
                stockUnitId: viewModel.StockUnit.Id,
                serialNumber: viewModel.StockUnit.SerialNumber,
                qtyToMove: viewModel.StockUnit.QtyToMove,
                destinationlocationId: viewModel.DestinationLocation.Id,
                destinationWarehouseId: viewModel.DestinationLocation.WarehouseId,
                force: force,
                discartSerialNumber: discartSerialNumber,
                unitOfMeasure: unitOfMeasure
            });
        mtl_json.fail(function (jsonModel) {
            RemoveLoadingSnippetWithOverlay();
            EnableElement("#btnExecute");
        });
        mtl_json.done(function (jsonModel) {
            RemoveLoadingSnippetWithOverlay();
            EnableElement("#btnExecute");
            if (jsonModel.Status == 0) {
                self.Init();
                new Alert().Show("success", "Zakończono pomyślnie" + " (" + jsonModel.Data + ")");
            }
            else if (jsonModel.Status == 125) {
                CheckBoxForceVisible(true);
                new Alert().Show("danger", TranslateStatus(jsonModel.Status) + " (" + jsonModel.Data + ")");
            }
            else if (jsonModel.Status == 400) {
                CheckBoxForceVisible(true);
                new Alert().Show("danger", TranslateStatus(jsonModel.Status) + " (" + jsonModel.Data + ")");
            }
            else if (jsonModel.Status == 901 || jsonModel.Status == 900) {
                self.Init();
                new Alert().Show("warning", "Zakończone bez wydruku etykiet" + " (" + jsonModel.Data + ")");
            }
            else {
                new Alert().Show("danger", TranslateStatus(jsonModel.Status) + " (" + jsonModel.Data + ")");
            }
        });
    }
    function CheckBoxForceVisible(isVisible) {
        if (isVisible == true) {
            $("#cbForceGroup").removeClass("hidden");
        }
        else {
            $("#cbForceGroup").addClass("hidden");
            $("#cbForce").prop('checked', false);
        }
    }

    function ShowParseButton() {
        viewModel.ModeChangeActive = true;
        viewModel.ShowBarcodeInput = true;
        viewModel.PickIcon = false;
        viewModel.DeliverIcon = false;
        viewModel.BtnText = "POTWIERDŹ SKAN";
        viewModel.BtnId = "scanButton";
        viewModel.BtnColSize = "col-4";
        viewModel.MarginBottomScanSection = "mb-auto";
    }
    function ShowExecuteButton() {
        viewModel.DeliverIcon = true;
        viewModel.BtnText = "Zatwierdz Lokację";
        viewModel.BtnId = "btnExecute";
        viewModel.BtnColSize = "col-4";
    }
    function ShowClearButton() {
        viewModel.DeliverIcon = false;
        viewModel.BtnId = "clearBarcode";
        viewModel.BtnText = "";
        viewModel.BtnColSize = "col-6";
        viewModel.MarginBottomScanSection = "mb-2";
    }

    function Render() {
        RenderTemplate("#MovementMobileTemplate", "#iLogisWmsMovementMobile", viewModel);
    }
    function RenderTemplate(templateID, contentDivSelector, object, isAppend = false) {
        if (isAppend == false) {
            $(contentDivSelector).html("");
        }
        let template = $(templateID).html();
        let rendered = Mustache.render(template, object);
        $(contentDivSelector).append(rendered);
    }
    function AddSwipeAutoManual() {
        var IconContent = document.getElementById('SwapAutoManual');
        var mc = new Hammer(IconContent);

        mc.on("swipeleft", function (event) {
            if (viewModel.ModeChangeActive) {
                viewModel.LocationMode = "A";
                viewModel.ManualMode = false;
                $("#manualModeInput").addClass("hidden");
                $("#autoModeInput").removeClass("hidden");
                $("#AutoManualLocationMode").html(viewModel.LocationMode);
            }
        });
        mc.on("swiperight", function (event) {
            if (viewModel.ModeChangeActive) {
                viewModel.LocationMode = "M";
                viewModel.ManualMode = true;
                $("#manualModeInput").removeClass("hidden");
                $("#autoModeInput").addClass("hidden");
                $("#AutoManualLocationMode").html(viewModel.LocationMode);
            }
        });
    }
    
    function Actions() {
        $(document).off("click", "#clearBarcode");
        $(document).on("click", "#clearBarcode", function (event) {
            self.Init();
        });

        $(document).off("click", "#scanButton");
        $(document).on("click", "#scanButton", function (event) {
            if (viewModel.ManualMode == false)
                ParseBarcode();
            else
                GetStockUnitByCodeAndLocation();
        });

        $(document).off("click", "#destinationWarehouse");
        $(document).on("click", "#destinationWarehouse", function (event) {
            ChangeQtyToMove(0);
            ShowWarehouseSelector();
        });

        $(document).off("click", "#btnExecute");
        $(document).on("click", "#btnExecute", function (event) {
            if ($(this).hasClass("disabled") == false) {
                SetDefaultValues();
                MoveToLocation();
            }
            else {
                new Alert().Show("warning", "Wprowadż lokację docelową");
            }
        });

        $(document).off("click", "#btnGetLocation");
        $(document).on("click", "#btnGetLocation", function (event) {
            ChangeQtyToMove(0);
            GetLocattion();
        });

        $(document).off("focusout", "#QtyToMove");
        $(document).on("focusout", "#QtyToMove", function (event) {
            ChangeQtyToMove(0);
        });

        $(document).off("click", ".btnQtyChange");
        $(document).on("click", ".btnQtyChange", function (event) {
            let val = parseInt($(this).attr("data-val"));
            ChangeQtyToMove(val);
        });

        $(document).off("click", "#btnChangeUoM");
        $(document).on("click", "#btnChangeUoM", function (event) {
            let itemCode = $("#ItemCode").val();
            let isDisplayed = $(this).find("#UnitOfMeasureSelectorWrapper").length > 0;

            console.log(itemCode);

            if (isDisplayed) {
                $("#UnitOfMeasureSelectorWrapper").remove();
            }
            else {
                GetAlternativeUnitOfMeasures(itemCode);
            }
        });

        $(document).off("click", ".UnitOfMeasureOption");
        $(document).on("click", ".UnitOfMeasureOption", function (event) {
            let selectedUoMText = $(this).text().trim();
            let selectedUoMValue = $(this).attr("data-UoM").trim();
            console.log("UnitOfMeasureOption click:" + selectedUoMText + ". " + selectedUoMValue);
            $("#UnitOfMeasure").val(selectedUoMText);
            $("#UnitOfMeasure").attr("data-UoM", selectedUoMValue);
        });

        $(document).off("keyup", "#destinationLocation");
        $(document).on("keyup", "#destinationLocation", function (event) {
            console.log("change");
            $("#btnExecute").addClass("disabled");
        });
    }
}
