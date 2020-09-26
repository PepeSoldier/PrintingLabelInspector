function TransporterList(type = "pickingList") {
    var pickingListShared = new PickingListShared();
    var viewModel = {
        PickerList: []
    };

    this.Init = function () {
        GetPickers();
        Actions();
    };
    this.InitLF = function () {
        GetLineFeeders();
        Actions();
    };

    function GetPickers() {
        $("#contentView").html(ShowLoadingSnippet());
        var JsonHelp = new JsonHelper();
        var ReturnJson = JsonHelp.GetPostData(pickingListShared.urlGetPickers);
        ReturnJson.done(function (data) {
            $("#contentView").html("");
            viewModel.PickerList = data;
            RenderTemplate("#PickerBoxTemplate", "#contentView", viewModel);
            DoubleTapForPickerSelection();
        });
    }
    function GetLineFeeders() {
        $("#contentView").html(ShowLoadingSnippet());
        var JsonHelp = new JsonHelper();
        var ReturnJson = JsonHelp.GetPostData("/iLOGIS/DeliveryListLineFeed/GetLineFeeders");
        ReturnJson.done(function (data) {
            $("#contentView").html("");
            viewModel.PickerList = data;
            RenderTemplate("#PickerBoxTemplate", "#contentView", viewModel);
            DoubleTapForPickerSelection();
        });
    }
    function DoubleTapForPickerSelection() {
        $(".pickerBox").each(function () {
            var pickerId = $(this).attr("data-id");
            var mc = new Hammer.Manager(this);
            mc.add(new Hammer.Tap({ event: 'doubletap', taps: 2 }));
            mc.on("doubletap", function () {
                if(type == "pickingList")
                    window.location.hash = doLink(pickingListShared.urlPickingList, { pickerId });
                else
                    window.location.hash = doLink("/iLOGIS/DeliveryListLineFeed/DeliveryListLF", { transporterId: pickerId });
            });
        });
    }
    function Actions() {
        $(document).off("click", ".orderRow")
        $(document).on("click", ".orderRow", function () {
            $.each($.find(".orderRow"), function () {
                $(this).removeClass("selectedRow");
            });
            $(this).addClass("selectedRow");
        });
        $(document).off("click", "#btnGoToPickingList");
        $(document).on("click", "#btnGoToPickingList", function () {
            let pickerId = $(".selectedRow").attr("data-id");
            if ($(".selectedRow").length == 0) {
                bootbox.alert("Wybierz Pickera");
            } else {
                window.location.hash = doLink(pickingListShared.urlPickingList, { pickerId });
            }
        });
        $(document).off("click", "#btnGoToDeliveryListLF");
        $(document).on("click", "#btnGoToDeliveryListLF", function () {
            let transporterId = $(".selectedRow").attr("data-id");
            if ($(".selectedRow").length == 0) {
                bootbox.alert("Wybierz dowożącego");
            } else {
                window.location.hash = doLink("/iLOGIS/DeliveryListLineFeed/DeliveryListLF", { transporterId });
            }
        });
    }
}
