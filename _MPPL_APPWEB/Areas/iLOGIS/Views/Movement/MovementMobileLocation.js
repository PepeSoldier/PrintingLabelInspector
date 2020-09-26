
function MovementMobileLocation() {

    var self = this;
    var viewModel = {
        SelectedWarehouseLocation: {
            Id: 0,
            Name: "LocationName",
            NameFormatted: "LocationName",
            QtyOfSubLocations: 0,
            WarehouseId: 0,
            WarehouseName: "WH Name",
            ParentLocationId: 0,
            ParentLocationName: "ParentLocationName",
            TypeId: 0,
            TypeName: "Loc. Type",
            Utilization: 0
        },
        DestinationWarehouseLocation: {
            Id: 0,
            Name: "",
            WarehouseId: 0,
            WarehouseName: "WH Name"
        },
        ModeChangeActive: true,
        ShowBarcodeInput: true,
        PickIcon: false,
        DeliverIcon: false,
        BtnText: "POTWIERDŹ SKAN",
        BtnId: "btnParse",
        BtnColSize: "col-4",
        MarginBottomScanSection: "mb-auto"
    };

    this.Init = function () {
        viewModel.SelectedWarehouseLocation = {};
        viewModel.DestinationWarehouseLocation = {};
        ShowParseButton();
        Render();
        Actions();
    };

    function ParseBarcode() {
        viewModel.ModeChangeActive = false;
        viewModel.PickIcon = true;
        viewModel.ShowBarcodeInput = false;
        viewModel.BtnId = "clearBarcode";
        viewModel.BtnText = "";
        viewModel.BtnColSize = "col-6";
        viewModel.MarginBottomScanSection = "mb-2";

        GetLocattion();
    }
    function GetLocattion() {
        let locationName = $("#barcode").val();
        viewModel.WarehouseId = 0;

        let jsonGetLocation = new JsonHelper().GetPostData("/iLOGIS/WarehouseLocation/GetLocation",
            { nameFormatted: locationName, warehouseId: viewModel.WarehouseId });
        jsonGetLocation.done(function (warehouseLocationViewModelList) {
            //console.log(warehouseLocationViewModelList);
            if (warehouseLocationViewModelList.length <= 0) {
                console.log("Nie udało się znaleźć lokacji");
                viewModel.SelectedWarehouseLocation = null;
                $("#destinationLocation").attr("data-locationid", 0);
                $("#destinationLocation").val("");
                new Alert().Show("info", "Nie udało się znaleźć lokacji");
            }
            else if (warehouseLocationViewModelList.length > 1) {
                console.log("znaleziono wiele pasujących lokacji");
                $("#destinationLocation").attr("data-locationid", 0);
                $("#destinationLocation").val("wbierz...");
                new Alert().Show("info", "znaleziono wiele pasujących lokacji. Wybierz jedną...");
            }
            else {

                console.log("znaleziono lokację");
                viewModel.SelectedWarehouseLocation = warehouseLocationViewModelList[0];

                if (viewModel.SelectedWarehouseLocation.WarehouseName == null ||
                    viewModel.SelectedWarehouseLocation.WarehouseName.length <= 0)
                {
                    viewModel.SelectedWarehouseLocation.WarehouseName = "?";
                }

                $("#destinationLocation").attr("data-locationid", viewModel.SelectedWarehouseLocation.Id);
                $("#destinationLocation").val(viewModel.SelectedWarehouseLocation.Name);

                viewModel.DeliverIcon = true;
                viewModel.BtnText = "Zatwierdz Lokację";
                viewModel.BtnId = "btnConfirm";
                viewModel.BtnColSize = "col-4";

                Render();
            }
        });
    }
    function MoveToLocation() {
        if (viewModel.DestinationWarehouseLocation.Name == "") {
            new Alert().Show("warning", "Wprowadz i pobierz lokację");
        }
        else {
            var JsonHelp = new JsonHelper();
            var ReturnJson = JsonHelp.GetPostData("/iLOGIS/WarehouseLocation/SetParentLocation",
                {
                    warehouseLocationName: viewModel.SelectedWarehouseLocation.Name,
                    parentWarehouseLocationName: viewModel.DestinationWarehouseLocation.Name,
                });
            ReturnJson.done(function (data) {
                self.Init();
                new Alert().Show("success", "Zakończono pomyślnie");
            });
        }
    }
    function ShowWarehouseSelector() {
        console.log("Movement.ShowWarehouseSelector");
        ws = new WarehouseSelector(viewModel.WarehouseId, viewModel.WarehouseName);
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
        viewModel.DeliverIcon = true;
        viewModel.BtnText = "Zatwierdz Lokację";
        viewModel.BtnId = "btnConfirm";
        viewModel.BtnColSize = "col-4";
        viewModel.DestinationWarehouseLocation.WarehouseName = selectedWarehouse.WarehouseName;
        viewModel.DestinationWarehouseLocation.WarehouseId = selectedWarehouse.WarehouseId;
        viewModel.DestinationWarehouseLocation.Name = selectedWarehouse.LocationName;

        $("#destinationWarehouse").text(viewModel.DestinationWarehouseLocation.WarehouseName);
        $("#destinationWarehouse").attr("data-warehouseid", viewModel.DestinationWarehouseLocation.WarehouseId);
        $("#destinationLocation").val(viewModel.DestinationWarehouseLocation.Name);

        if (selectedWarehouse.LocationName != null && selectedWarehouse.LocationName.length > 0) {
            _getLocation(selectedWarehouse);
        }
        else {
            _findLocation(selectedWarehouse);
        }
        //viewModel.DestinationLocationName = selectedWarehouse.Name,
        //viewModel.DestinationLocationId = selectedWarehouse.Id,
    }
    function _getLocation(selectedWarehouse) {
        let jsonGetLocation = new JsonHelper().GetPostData("/iLOGIS/WarehouseLocation/GetLocation", {
            nameFormatted: selectedWarehouse.LocationName,
            warehouseId: selectedWarehouse.WarehouseId
        });
        jsonGetLocation.done(function (warehouseLocationViewModelList) {
            if (warehouseLocationViewModelList.length <= 0) {
                new Alert().Show("info", "Nie udało się znaleźć lokacji");
                console.log("Nie udało się znaleźć lokacji");
                viewModel.DestinationWarehouseLocation.Id = 0;
                viewModel.DestinationWarehouseLocation.Name = "";
            }
            else {
                if (warehouseLocationViewModelList.length > 1) {
                    console.log("znaleziono wiele pasujących lokacji");
                }
                else {
                    console.log("znaleziono lokację");
                }
                viewModel.DestinationWarehouseLocation.Id = warehouseLocationViewModelList[0].Id;
                viewModel.DestinationWarehouseLocation.Name = warehouseLocationViewModelList[0].Name;
            }
        });
    }
    function _findLocation(selectedWarehouse) {
        var JsonHelp = new JsonHelper();
        var ReturnJson = JsonHelp.GetPostData("/iLOGIS/WarehouseLocation/FindEmptyLocation", {
            warehouseId: selectedWarehouse.WarehouseId
        });
        ReturnJson.done(function (locationViewModel) {
            viewModel.DestinationWarehouseLocation.Id = locationViewModel.Id;
            viewModel.DestinationWarehouseLocation.Name = locationViewModel.Name;
            $("#destinationLocation").val(viewModel.DestinationWarehouseLocation.Name);
            $("#destinationLocation").attr("data-locationid", viewModel.DestinationWarehouseLocation.Id);
        });
    }

    function ShowParseButton() {
        viewModel.ModeChangeActive = true;
        viewModel.ShowBarcodeInput = true;
        viewModel.PickIcon = false;
        viewModel.DeliverIcon = false;
        viewModel.BtnText = "POTWIERDŹ SKAN";
        viewModel.BtnId = "btnParse";
        viewModel.BtnColSize = "col-4";
        viewModel.MarginBottomScanSection = "mb-auto";
    }

    function Render() {
        RenderTemplate("#MovementMobileLocationTemplate", "#iLogisWMSMovementMobileLocation", viewModel);
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

        $(document).off("click", "#btnParse");
        $(document).on("click", "#btnParse", function (event) {
            ParseBarcode();
        });

        $(document).off("click", "#destinationWarehouse");
        $(document).on("click", "#destinationWarehouse", function (event) {
            ShowWarehouseSelector();
        });

        $(document).off("click", "#btnConfirm");
        $(document).on("click", "#btnConfirm", function (event) {
            MoveToLocation();
        });

        $(document).off("click", "#btnGetLocation");
        $(document).on("click", "#btnGetLocation", function (event) {
            //_findLocation({ WarehouseId: viewModel.DestinationWarehouseLocation.WarehouseId });
            _getLocation({
                LocationName: $("#destinationLocation").val(),
                WarehouseId: $("#destinationWarehouse").attr("data-warehouseid")
            });
        });
    }
}
