function MovementMobileConfirmation(_barcodeTemplate) {

    var self = this;
    var viewModel = {
        StockUnit: {
            Id: 0,
            ItemCode: "",
            Qty: 0,
            SerialNumber: "",
            LocationId: 0,
            LocationName: "",
            WarehouseId: 0,
            WarehouseName: "",
            QtyToMove: 0
        },
        DestinationLocation: {
            Id: 0,
            Name: "",
            WarehouseId: 0,
            WarehouseName: "WH Name",
            LocationName: "LC Name"
        },
        ShowBarcodeInput: true,
        QrCodeIcon: false,
        BoxIcon: false,
        BtnText: "POTWIERDŹ SKAN",
        BtnId: "scanButton",
        BtnColSize: "col-4",
        MarginBottomScanSection: "mb-auto"
    };
    var barcodeTemplate = _barcodeTemplate;

    this.Init = function () {
        viewModel.StockUnit = {};
        viewModel.DestinationLocation = {};
        ShowParseButton();
        Render();
        Actions();
        $("#barcode").focus();
    };

    function ParseBarcode() {
        let barcode = $("#barcode").val();
        
        let json = new JsonHelper().GetPostData("/CORE/Common/ParseBarcode", { barcode, template: barcodeTemplate });
        json.done(function (barcodeParsedViewModel) {
            console.log("/CORE/Common/ParseBarcode.Done");
            viewModel.StockUnit.ItemCode = barcodeParsedViewModel.ItemCode;
            viewModel.StockUnit.Qty = barcodeParsedViewModel.Qty;
            viewModel.StockUnit.SerialNumber = barcodeParsedViewModel.SerialNumber;
            viewModel.StockUnit.CurrenLocationName = barcodeParsedViewModel.Location;
            viewModel.StockUnit.CurrenWarehouseName = "MG-JSON";

            console.log("barcodeParsedViewModel");

            if (barcodeParsedViewModel.SerialNumber != null) {
                let jsonSU = new JsonHelper().GetPostData("/iLOGIS/StockUnit/GetByIdOrSerialNumber", {
                    Id: 0,
                    serialNumber: viewModel.StockUnit.SerialNumber
                });
                jsonSU.done(function (stockUnitViewModel) {
                    viewModel.StockUnit.Id = stockUnitViewModel.Id;
                    viewModel.StockUnit.ItemCode = stockUnitViewModel.ItemCode;
                    viewModel.StockUnit.Qty = stockUnitViewModel.CurrentQtyinPackage;
                    viewModel.StockUnit.QtyToMove = stockUnitViewModel.CurrentQtyinPackage;
                    viewModel.StockUnit.SerialNumber = stockUnitViewModel.SerialNumber;
                    viewModel.StockUnit.LocationId = stockUnitViewModel.WarehouseLocationId;
                    viewModel.StockUnit.LocationName = stockUnitViewModel.WarehouseLocationName;
                    viewModel.StockUnit.WarehouseId = stockUnitViewModel.WarehouseId;
                    viewModel.StockUnit.WarehouseName = stockUnitViewModel.WarehouseName != null ? stockUnitViewModel.WarehouseName : "?";
                    viewModel.DestinationLocation.WarehouseId = viewModel.StockUnit.WarehouseId;
                    viewModel.DestinationLocation.WarehouseName = viewModel.StockUnit.WarehouseName;
                    viewModel.DestinationLocation.LocationName = viewModel.StockUnit.LocationName;

                    if (stockUnitViewModel.Id > 0) {
                        viewModel.QrCodeIcon = true;
                        viewModel.ShowBarcodeInput = false;
                        ShowClearButton();
                        ShowConfirmationButton();
                        Render();
                        $("#destinationLocation").focus();
                    }
                    else {
                        new Alert().Show("warning", "Nie znaleziono opakowania dla podanego kodu");
                    }
                });
            }
            else {
                new Alert().Show("warning", "Dekodowanie: " + barcodeParsedViewModel.ErrorText);
            }
            $("#barcode").focus();
        });
    }

    function ConfirmMovement() {
        let destinationLocationName = $("#destinationLocation").val();
        if (isDestinationLocationCorrect(destinationLocationName)) {
            var JsonHelp = new JsonHelper();
            var ReturnJson = JsonHelp.GetPostData("/iLOGIS/StockUnit/ConfirmMovement",
                {
                    stockUnitId: viewModel.StockUnit.Id,
                    destinationlocationName: destinationLocationName,
                });
            ReturnJson.done(function (iLogisStatus) {
                if (iLogisStatus == 0) {
                    self.Init();
                    new Alert().Show("success", "Zakończono pomyślnie");
                }
                else {
                    new Alert().Show("danger", TranslateStatus(iLogisStatus));
                }
            });
        } else {
            new Alert().Show("danger", TranslateStatus(150));
            $("#destinationLocation").focus();
        }
    }

    function isDestinationLocationCorrect(destinationLocationName) {
        if (destinationLocationName == viewModel.DestinationLocation.LocationName.substring(0, 4) || destinationLocationName == viewModel.DestinationLocation.LocationName) {
            return true;
        } else {
            return false;
        }
    }

    function ShowParseButton() {
        viewModel.ShowBarcodeInput = true;
        viewModel.QrCodeIcon = false;
        viewModel.BoxIcon = false;
        viewModel.BtnText = "POTWIERDŹ SKAN";
        viewModel.BtnId = "scanButton";
        viewModel.BtnColSize = "col-4";
        viewModel.MarginBottomScanSection = "mb-auto";
    }

    function ShowConfirmationButton() {
        viewModel.DeliverIcon = true;
        viewModel.BtnText = "Zatwierdz Lokację";
        viewModel.BtnId = "confirmLocationButton";
        viewModel.BtnColSize = "col-4";
    }

    function ShowClearButton() {
        viewModel.BoxIcon = false;
        viewModel.BtnId = "clearBarcode";
        viewModel.BtnText = "";
        viewModel.BtnColSize = "col-6";
        viewModel.MarginBottomScanSection = "mb-2";
    }

    function Render() {
        RenderTemplate("#MovementConfirmationTemplate", "#iLogisWmsMovementMobileConfirmation", viewModel);
    }

    function RenderTemplate(templateID, contentDivSelector, object, isAppend = false) {
        if (isAppend == false) {
            $(contentDivSelector).html("");
        }
        let template = $(templateID).html();
        let rendered = Mustache.render(template, object);
        $(contentDivSelector).append(rendered);
    }

    function Actions() {
        $(document).off("click", "#clearBarcode");
        $(document).on("click", "#clearBarcode", function (event) {
            self.Init();
        });

        $(document).off("click", "#scanButton");
        $(document).on("click", "#scanButton", function (event) {
            ParseBarcode();
        });

        $(document).off("click", "#confirmLocationButton");
        $(document).on("click", "#confirmLocationButton", function (event) {
            ConfirmMovement();
        });
    }
}
