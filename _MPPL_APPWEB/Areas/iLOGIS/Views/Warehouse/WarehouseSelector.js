function WarehouseSelector(warehouseId, warehouseName, warehouseCode = "") {

    var self = this;
    var wnd = new PopupWindow("100%", "100%", 0, 0);
    var selectedWarehouseId = warehouseId;
    var selectedWarehouseName = warehouseName;
    var selectedWarehouseCode= warehouseCode;
    var selectWarehouseResolve = function () { };
    var viewModel = {
        WarehouseBoxList: [],
        DestinationLocationName: "",
        CurrentWarehouseName: warehouseName,
        IsCustomList: false
    };
    var fullScreenMode = true;
    Init();

    function Init() {
        Actions();
    }

    this.SetCustomList = function (WarehouseSelectorViewModelList, destLocationName) {
        viewModel.WarehouseBoxList = WarehouseSelectorViewModelList;
        viewModel.IsCustomList = true;
        viewModel.DestinationLocationName = destLocationName;
    };
    this.SelectWarehouse = function (_fullScreenMode = true) {
        console.log("WarehouseSelector.SelectWarehouse");
        
        fullScreenMode = _fullScreenMode;
        LoadWarehouseSelectorView();

        return new Promise((resolve, reject) => {
            selectWarehouseResolve = resolve;
            //setTimeout(() => {
            //    CloseWindow();
            //    new Alert().Show("info", "Masz 2 minuty na wybranie magazynu");
            //    reject("error. time out");
            //}, 5000);
        });
    };

    function LoadWarehouseSelectorView() {
        var JsonHelp = new JsonHelper();
        var ReturnJson = JsonHelp.GetData("/iLOGIS/Warehouse/WarehouseSelector", {});
        ReturnJson.done(function (view) {

            if (fullScreenMode)
                wnd = new PopupWindow("100%", "100%", 0, 0);
            else
                wnd = new PopupWindow(500, 700, 60, 80);

            wnd.Init("mainMenu", "Wybierz Magazyn");
            wnd.Show(view);

            if (viewModel.IsCustomList == false)
                GetWarehouses_AndDrawWarehouses(selectedWarehouseId, 0);
            else
                DrawWarehouses();
        });
    }

    function SelectdestinationLocation() {
        
        let destinationLocationName = $("#destinationLocationName").val();

        if ($(".selectedWarehouse").length == 0) {
            bootbox.alert("Zaznacz Magazyn");
        } else {
            //callback({ WarehouseId: selectedWarehouseId, WarehouseName: selectedWarehouseName });
            selectWarehouseResolve({
                WarehouseId: selectedWarehouseId,
                WarehouseName: selectedWarehouseName,
                WarehouseCode: selectedWarehouseCode,
                LocationName: destinationLocationName
            });
            CloseWindow();
        }
    }

    function GetWarehouses_AndDrawWarehouses(parentWarehouseId, warehouseId) {
        
        $("#iLogisWMSWarehouseSelector").html("");
        let JsonHelp = new JsonHelper();
        let ReturnJson = JsonHelp.GetPostData("/iLOGIS/Warehouse/GetWarehouses", {
            parentWarehouseId,
            warehouseId
        });
        ReturnJson.done(function (WarehouseSelectorViewModelList) {
            if (WarehouseSelectorViewModelList.length == 0) {
                viewModel.SelectedWarehouseId = selectedWarehouseId;
            } else {
                viewModel.WarehouseBoxList = WarehouseSelectorViewModelList;
                viewModel.SelectedWarehouseId = WarehouseSelectorViewModelList[0].ParentWarehouseId;
                viewModel.CurrentWarehouseName = WarehouseSelectorViewModelList[0].ParentWarehouseName;
            }
            DrawWarehouses();
        });
    }

    function DrawWarehouses() {
        RenderTemplate("#WarehouseBoxTemplate", "#iLogisWMSWarehouseSelector", viewModel);

        if (selectedWarehouseId != null && selectedWarehouseId.length > 0) {
            $(".warehouseBox#" + selectedWarehouseId).addClass("selectedWarehouse");
        }
        AddDoubleTapForWarehouseSelector();
    }

    function CloseWindow() {
        return wnd.Close();
    }

    function AddDoubleTapForWarehouseSelector() {
        $(".warehouseBox").each(function () {
            var mc = new Hammer.Manager(this);
            mc.add(new Hammer.Tap({ event: 'doubletap', taps: 2 }));
            mc.on("doubletap", function () {
                console.log("wtf");
                GetWarehouses_AndDrawWarehouses(selectedWarehouseId, 0);
            });
        });
    }

    function Actions() {
        $(document).off("click", "#selectWarehouseLocation");
        $(document).on("click", "#selectWarehouseLocation", function () {
            SelectdestinationLocation();
        });

        $(document).off("click", ".warehouseBox");
        $(document).on("click", ".warehouseBox", function () {
            console.log("warehouseBox click");
            $(".warehouseBox").removeClass("selectedWarehouse");
            selectedWarehouseId = $(this).attr("data-warehouseid");
            selectedWarehouseName = $(this).find(".whName").text().trim();
            selectedWarehouseCode = $(this).find(".whCode").text().trim();
            $(this).addClass("selectedWarehouse");
        });

        $(document).off("click", "#btnGoToMainWarehouse, .btnGoHome");
        $(document).on("click", "#btnGoToMainWarehouse, .btnGoHome", function () {
            selectedWarehouseId = $(this).attr("data-warehouseid");
            selectedWarehouseName = $(this).find(".whName").text().trim();
            GetWarehouses_AndDrawWarehouses(0, 0);
        });

        $(document).off("click", "#btnGoToParentWarehouse, .btnGoUp");
        $(document).on("click", "#btnGoToParentWarehouse, .btnGoUp", function () {
            whId = $(this).attr("data-warehouseid");
            GetWarehouses_AndDrawWarehouses(0, whId);
        });

        $(document).off("click", "#btnNoLocation");
        $(document).on("click", "#btnNoLocation", function () {
            $("#destinationLocationName").val("0");
        });
    }
}