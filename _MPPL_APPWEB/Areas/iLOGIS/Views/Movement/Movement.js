function Movement(_barcodeTemplate) {

    var self = this;
    var barcodeTemplate = _barcodeTemplate;
    var defaultValues = {
        ItemCode: "",
        QtyToMove: 0,
        SourceLocation: {
            Id: 0,
            Name: "",
            WarehouseId: 0,
            WarehouseName: "",
            WarehouseCode: ""
        },
        DestinationLocation: {
            Id: 0,
            Name: "",
            WarehouseId: 0,
            WarehouseName: "",
            WarehouseCode: ""
        },
        LocationMode: "A",
        ManualMode: false
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
            WarehouseCode: "",
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
        ModeChangeActive: true,
        ShowBarcodeInput: true,
        PickIcon: false,
        DeliverIcon: false,
        BtnText: "POTWIERDŹ SKAN",
        BtnId: "scanButton",
        BtnColSize: "col-4",
        MarginBottomScanSection: "mb-auto"
    };

    var versionController = new VersionController("iLogisMobileWMSMovement", true);
    versionController.CheckForUpdates("/iLOGIS/Home/GetCurrentVersion");

    this.Init = function () {
        viewModel.StockUnit = {};
        viewModel.DestinationLocation = {};
        ShowParseButton();
        Render();
        Actions();
        //AddSwipeAutoManual();
        $("#barcode").focus();

        if (defaultValues.ManualMode) {
            $("#nav-manual-tab").click();
        }
        //new Alert().Show("info", "wymiary 12:");
        //new Alert().Show("info", "szer: " + $(document).width());
        //new Alert().Show("info", "wys: " + $(document).height());
    };
    this.LoadDataByStockUnitId = function (id) {
        viewModel.StockUnit.Id = id;
        viewModel.StockUnit.SerialNumber = null;
        GetStockUnitByIdOrSerialNumber();
    };

    function SetDefaultValues() {
        defaultValues.QtyToMove = $("#QtyToMove").val();
        defaultValues.ItemCode = viewModel.StockUnit.ItemCode;
        defaultValues.LocationMode = viewModel.LocationMode;
        defaultValues.SourceLocation.WarehouseCode = $("#currentWarehouseCode").val();
        defaultValues.DestinationLocation = viewModel.DestinationLocation;
        defaultValues.DestinationLocation.WarehouseCode = $("#destWarehouseCode").val();
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

            if (barcodeParsedViewModel.SerialNumber != null) {
                GetStockUnitByIdOrSerialNumber();
            } else {
                new Alert().Show("warning", "Dekodowanie: " + barcodeParsedViewModel.ErrorText);
            }

            $("#barcode").focus();
        });
    }
    function GetStockUnitByIdOrSerialNumber() {
        let jsonSU = new JsonHelper().GetPostData("/iLOGIS/StockUnit/GetByIdOrSerialNumber", {
            Id: viewModel.StockUnit.Id,
            serialNumber: viewModel.StockUnit.SerialNumber
        });
        jsonSU.done(function(stockUnitViewModel) {
            viewModel.StockUnit = stockUnitViewModel;
            viewModel.StockUnit.WarehouseName = stockUnitViewModel.WarehouseName != null ? stockUnitViewModel.WarehouseName : "?";
            viewModel.StockUnit.WarehouseCode = stockUnitViewModel.WarehouseCode != null ? stockUnitViewModel.WarehouseCode : "?";
            viewModel.StockUnit.QtyToMove = Math.max(0, stockUnitViewModel.CurrentQtyinPackage);

            viewModel.DestinationLocation.Id = 0;
            viewModel.DestinationLocation.Name = "";    
            viewModel.DestinationLocation.WarehouseId = viewModel.StockUnit.WarehouseId;
            viewModel.DestinationLocation.WarehouseName = viewModel.StockUnit.WarehouseName;
            viewModel.DestinationLocation.WarehouseCode = viewModel.StockUnit.WarehouseCode;

            if (defaultValues.DestinationLocation.WarehouseId > 0) {
                viewModel.DestinationLocation.Id = defaultValues.DestinationLocation.Id;
                viewModel.DestinationLocation.Name = defaultValues.DestinationLocation.Name;
                viewModel.DestinationLocation.WarehouseId = defaultValues.DestinationLocation.WarehouseId;
                viewModel.DestinationLocation.WarehouseName = defaultValues.DestinationLocation.WarehouseName;
                viewModel.DestinationLocation.WarehouseCode = defaultValues.DestinationLocation.WarehouseCode;
            }

            $("#destWarehouseCode").text(viewModel.DestinationLocation.WarehouseCode);
            $("#destinationWarehouse").text(viewModel.DestinationLocation.WarehouseName);
            $("#destinationWarehouse").attr("data-warehouseid", viewModel.DestinationLocation.WarehouseId);
            $("#destinationLocation").attr("data-locationid", viewModel.DestinationLocation.Id);
            $("#destinationLocation").val(viewModel.DestinationLocation.Name);

            if (stockUnitViewModel.Id > 0) {
                viewModel.ModeChangeActive = false;
                viewModel.PickIcon = true;
                viewModel.ShowBarcodeInput = false;
                ShowClearButton();
                RenderStockUnitInfo();
            }
            else {
                new Alert().Show("warning", "Nie znaleziono opakowania dla podanego numeru seryjnego");
            }
        });
    }

    function ShowWarehouseSelector() {
        console.log("Movement.ShowWarehouseSelector");
        CheckBoxForceVisible(false);
        ws = new WarehouseSelector(viewModel.DestinationLocation.WarehouseId, viewModel.DestinationLocation.WarehouseName);
        ws.SelectWarehouse(false)
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
        viewModel.DestinationLocation.WarehouseCode = selectedWarehouse.WarehouseCode;
        viewModel.DestinationLocation.WarehouseId = selectedWarehouse.WarehouseId;
        $("#destWarehouseCode").val(viewModel.DestinationLocation.WarehouseCode);
        //$("#destinationWarehouseCode").text(viewModel.DestinationLocation.WarehouseCode);
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
        //Render();
        RenderStockUnitInfo();
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
            if (warehouseLocationViewModel.Warehouse != null && warehouseLocationViewModel.WarehouseId != null) {
                viewModel.WarehouseId = warehouseLocationViewModel.WarehouseId;
                viewModel.WarehouseCode = warehouseLocationViewModel.Warehouse.Code;
                viewModel.WarehouseName = warehouseLocationViewModel.Warehouse.Name;
            }
            
            if (viewModel.DestinationLocation.Id > 0) {
                new Alert().Show("success", "Znaleziono automatycznie lokację " + viewModel.DestinationLocation.Name);
                ShowExecuteButton();
                //Render();
                //RenderStockUnitInfo();
            }
            else {
                new Alert().Show("info", "Nie udało się znaleźć automatycznie lokacji");
            }
            
            $("#destinationLocation").val(viewModel.DestinationLocation.Name);
            $("#destinationLocation").attr("data-locationid", viewModel.DestinationLocation.Id);

            $("#destinationWarehouse").text(viewModel.WarehouseName);
            $("#destinationWarehouse").attr("data-warehouseid", viewModel.WarehouseId);

            $("#destWarehouseCode").val(viewModel.WarehouseCode);
            //$("#destinationWarehouseCode").text(viewModel.WarehouseCode);
            $("#destinationWarehouseCode").attr("data-warehouseid", viewModel.WarehouseId);

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
                    ws.SelectWarehouse(false)
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
                        ////Render();
                    }
                    viewModel.DestinationLocation.Id = whLocVM.Id;
                    viewModel.DestinationLocation.Name = whLocVM.Name;
                    viewModel.DestinationLocation.WarehouseCode = whLocVM.WarehouseCode;
                    viewModel.DestinationLocation.WarehouseName = whLocVM.WarehouseName;
                    viewModel.DestinationLocation.WarehouseId = whLocVM.WarehouseId;
                    $("#destWarehouseCode").val(viewModel.DestinationLocation.WarehouseCode);
                    $("#destinationWarehouse").text(viewModel.DestinationLocation.WarehouseName);
                    $("#destinationWarehouse").attr("data-warehouseid", viewModel.DestinationLocation.WarehouseId);
                    $("#destinationLocation").attr("data-locationid", viewModel.DestinationLocation.Id);
                    $("#destinationLocation").val(viewModel.DestinationLocation.Name);

                    ShowExecuteButton();
                    //Render();
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
        qtyToMove = Math.min(qtyToMove, viewModel.StockUnit.CurrentQtyinPackage);
        qtyToMove = Math.max(qtyToMove, 0);
        viewModel.StockUnit.QtyToMove = qtyToMove;
        $("#QtyToMove").val(viewModel.StockUnit.QtyToMove);
    }
    function MoveToLocation() {
        DisableElement("#btnExecute");
        viewModel.StockUnit.QtyToMove = $("#QtyToMove").val();
        let force = $("#cbForce").prop("checked");
        let discartSerialNumber = $("#cbDiscartSerialNumber").prop("checked");
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
        mtl_json.done(function (jsonModel) {
            EnableElement("#btnExecute");
        });
        mtl_json.done(function (jsonModel) {
            EnableElement("#btnExecute");
            if (jsonModel.Status == 0) {
                self.Init();
                new Alert().Show("success", "Zakończono pomyślnie" + " ("  + jsonModel.Data + ")");
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
    function Move() {
        $("#btnExecute").addClass("disabled");
        viewModel.StockUnit.QtyToMove = $("#QtyToMove").val();
        viewModel.Type = $("#ddlMovementType").val();

        if (defaultValues.ManualMode == true)
            _Move_Manual();
        else
            _Move_Automatic();
    }
    function _Move_Manual() {
        let sourceWarehouseCode = $("#currentWarehouseCode").val();
        let destinationWarehouseCode = $("#destWarehouseCode").val().trim();
        let itemCode = $("#ItemCode").val();
        let unitOfMeasure = $("#UnitOfMeasure").attr("data-UoM");

        let jsonQuery = new JsonHelper().GetPostData("/iLOGIS/StockUnit/MoveManual", {
            itemCode: itemCode,
            qtyToMove: viewModel.StockUnit.QtyToMove,
            type: viewModel.Type,
            sourceWarehouseCode: sourceWarehouseCode,
            destinationWarehouseCode: destinationWarehouseCode,
            unitOfMeasure: unitOfMeasure
        });
        jsonQuery.done(function (jsonModel) {
            $("#btnExecute").removeClass("disabled");
            if (jsonModel.Status == 0) {
                self.Init();
                new Alert().Show("success", "Zakończono pomyślnie" + " (" + jsonModel.Data + ")");
            }
            else {
                new Alert().Show("danger", TranslateStatus(jsonModel.Status) + " (" + jsonModel.Data + ")");
            }
            stockLocationDetailsGrid.InitGridExtended(540);
            stockLocationDetailsGrid.RefreshGrid();
        });
        jsonQuery.fail(function (iLogisStatus) {
            $("#btnExecute").removeClass("disabled");
        });
    }
    function _Move_Automatic() {
        let force = $("#cbForce").prop("checked");
        let discartSerialNumber = $("#cbDiscartSerialNumber").prop("checked");
        var destLocationId = $("#destinationLocation").attr("data-locationid");
        let unitOfMeasure = $("#UnitOfMeasure").attr("data-UoM");

        if (destLocationId == null || destLocationId == "") {
            BootBoxAlert("destinationLocation", "Najpierw wyszukaj lokację");
        }

        let jsonQuery = new JsonHelper().GetPostData("/iLOGIS/StockUnit/Move", {
            stockUnitId: viewModel.StockUnit.Id,
            serialNumber: viewModel.StockUnit.SerialNumber,
            qtyToMove: viewModel.StockUnit.QtyToMove,
            type: viewModel.Type,
            destinationlocationId: destLocationId,
            destinationWarehouseId: viewModel.DestinationLocation.WarehouseId,
            force: force,
            discartSerialNumber: discartSerialNumber,
            unitOfMeasure: unitOfMeasure
        });
        jsonQuery.done(function (jsonModel) {
            $("#btnExecute").removeClass("disabled");
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
            stockLocationDetailsGrid.InitGridExtended(540);
            stockLocationDetailsGrid.RefreshGrid();
        });
        jsonQuery.fail(function (iLogisStatus) {
            $("#btnExecute").removeClass("disabled");
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
    function VerifyUnitOfMeasure() {
        let uom = $("#UnitOfMeasure").val();
        if (uom == "?") {
            new Alert().Show("danger", "Wybierz kod z listy aby wczytać jednostkę miary");
            return false;
        }
        else {
            return true;
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
        RenderTemplate("#MovementTemplate", "#iLogisWmsMovement", viewModel);
        RenderStockUnitInfo();
        RenderMovementSearchStockUnit();
        RenderMovementScanStockUnit();
    }
    function RenderTemplate(templateID, contentDivSelector, object, isAppend = false) {
        if (isAppend == false) {
            $(contentDivSelector).html("");
        }
        let template = $(templateID).html();
        let rendered = Mustache.render(template, object);
        $(contentDivSelector).append(rendered);
    }

    function RenderStockUnitInfo() {
        RenderTemplate("#MovementStockUnitInfoTemplate", "#MovementStockUnitInfo", viewModel);
    }
    function RenderMovementSearchStockUnit() {
        RenderTemplate("#MovementSearchStockUnitTemplate", "#MovementSearchStockUnit", viewModel);
    }
    function RenderMovementScanStockUnit() {
        RenderTemplate("#MovementScanStockUnitTemplate", "#MovementScanStockUnit", viewModel);
    }

    function AddSwipeAutoManual() {
        //var IconContent = document.getElementById('SwapAutoManual');
        //var mc = new Hammer(IconContent);

        //mc.on("swipeleft", function (event) {
        //    if (viewModel.ModeChangeActive) {
        //        viewModel.LocationMode = "A";
        //        $("#AutoManualLocationMode").html(viewModel.LocationMode);
        //    }
        //});
        //mc.on("swiperight", function (event) {
        //    if (viewModel.ModeChangeActive) {
        //        viewModel.LocationMode = "M";
        //        $("#AutoManualLocationMode").html(viewModel.LocationMode);
        //    }
        //});
    }

    function Actions() {
        ItemWMSAutcomplete("", "#itemCode", "", "#UnitOfMeasure");

        $(document).off("click", "#clearBarcode");
        $(document).on("click", "#clearBarcode", function (event) {
            self.Init();
        });

        $(document).off("click", "#scanButton");
        $(document).on("click", "#scanButton", function (event) {
            ParseBarcode();
        });

        $(document).off("click", "#destinationWarehouse, #destinationWarehouseCode");
        $(document).on("click", "#destinationWarehouse, #destinationWarehouseCode", function (event) {
            ShowWarehouseSelector();
        });

        $(document).off("click", "#confirmLocationButton");
        $(document).on("click", "#confirmLocationButton", function (event) {
            MoveToLocation();
        });

        $(document).off("click", "#btnGetLocation");
        $(document).on("click", "#btnGetLocation", function (event) {
            GetLocattion();
        });

        $(document).off("click", "#btnExecute");
        $(document).on("click", "#btnExecute", function (event) {
            if (VerifyUnitOfMeasure()) {
                if ($(this).hasClass("disabled") == false) {
                    SetDefaultValues();
                    Move();
                }
            }
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
                GetAlternativeUnitOfMeasures(itemCode); //"020007050"
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

        $(document).off("click", ".nav-item");
        $(document).on("click", ".nav-item", function (event) {

            defaultValues.ManualMode = $(".active.nav-item").attr("id") == "nav-manual-tab";

            viewModel.StockUnit = {};
            viewModel.DestinationLocation = {};
            ShowParseButton();
            RenderStockUnitInfo();

            if (defaultValues.ManualMode == true) {
                $("#ItemCode").removeAttr("readonly");
                $("#ItemCode").removeAttr("disabled");
                $("#currentWarehouseCode").removeAttr("readonly");
                $("#currentWarehouseCode").removeAttr("disabled");
                $("#destinationLocation").attr("disabled", true);
                $("#destinationWarehouse").attr("disabled", true);
                ItemWMSAutcomplete("#ItemCode", "#ItemCode", "#ItemName", "#UnitOfMeasure");

                viewModel.DestinationLocation.WarehouseId = defaultValues.DestinationLocation.WarehouseId;
                viewModel.DestinationLocation.WarehouseName = defaultValues.DestinationLocation.WarehouseName;
                viewModel.DestinationLocation.WarehouseCode = defaultValues.DestinationLocation.WarehouseCode;
                $("#destWarehouseCode").val(viewModel.DestinationLocation.WarehouseCode);
                $("#destinationWarehouse").text(viewModel.DestinationLocation.WarehouseName);
                $("#destinationWarehouse").attr("data-warehouseid", viewModel.DestinationLocation.WarehouseId);
                $("#currentWarehouseCode").val(defaultValues.SourceLocation.WarehouseCode);
            }
            else {
                $("#ItemCode").attr('disabled', true);
                $("#destinationLocation").remove("disabled");
                $("#destinationWarehouse").remove("disabled");
                $("#currentWarehouseCode").attr('disabled', true);
            }
        });

        $(document).off("keyup", "#destinationLocation");
        $(document).on("keyup", "#destinationLocation", function (event) {
            console.log("change");
            $("#btnExecute").addClass("disabled");
        });

        $(document).off("focusout", "#ItemCode");
        $(document).on("focusout", "#ItemCode", function (event) {
            VerifyUnitOfMeasure();
        });
    }
}
