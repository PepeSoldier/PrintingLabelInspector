function DeliveryListItemsLF() {
    var pickingListShared = new PickingListShared();
    var _transporterId = 0;
    var _deliveryListId = 0;
    var _workOrderId = 0;
    var _parameterH = -1;
    var keypadLocation = new KeyPadDigitDoubleRowsWithoutClose("#PlatformPosition_", "#kaypad115", "L");

    var viewModel = {};

    this.Init = function () {
        _deliveryListId = $("#contentView").attr("data-deliverylistid");
        _transporterId = $("#contentView").attr("data-transporterid");
        _workOrderId = $("#contentView").attr("data-workorderid");
        GetList();
        Actions();
    };

    function GetList() {
        $("#contentView").html(ShowLoadingSnippet());
        var JsonHelp = new JsonHelper();
        var ReturnJson = JsonHelp.GetPostData("/iLOGIS/DeliveryListLineFeed/DeliveryListItemsLFGetList",{
            workOrderId: _workOrderId,
            transporterId: _transporterId,
            parameterH: _parameterH
        });
        ReturnJson.done(function (deliveryListItemViewModelList) {
            if (deliveryListItemViewModelList.length > 0) {
                viewModel.WorkOrderNo = deliveryListItemViewModelList[0].WorkorderNumber;
                viewModel.NumberToPick = deliveryListItemViewModelList.filter(x => x.Status == 20).length;
            }
            
            let platforms = Array.from(new Set(deliveryListItemViewModelList.map(x => ({ "PlatformId": x.WarehouseLocationId, "PlatformName": x.WarehouseLocationName }))));
            console.log(platforms);
            let uniquePlatforms = Array.from(new Map(platforms.map(x => [x["PlatformId"], x])).values());
            console.log(uniquePlatforms);

            viewModel.PlatformList = uniquePlatforms;
            
            console.log(viewModel.PlatformList);
            viewModel.DeliveryListItems = deliveryListItemViewModelList;
            viewModel.ParameterHView = _parameterH == -1 ? '*' : _parameterH;
            viewModel.SelectedPlatformId = undefined;
            FilterRenderAddSwipe();
            $($(".orderRow")[0]).addClass("selectedRow");
        });
    }

    function FilterRenderAddSwipe() {
        FilterDeliveryListItems();
        Render();
        RenderAddWorkstationItem();
        AddSwipeHParameterListItems();
    }

    function Render() {
        RenderTemplate("#DeliveryListLineFeedTemplate", "#contentView", viewModel);
    }
    function RenderAddWorkstationItem() {
        RenderTemplate("#LineFeedAddWorkstationItemTemplate", "#addWorkstationItemContent", null);
    }

    function FilterDeliveryListItems() {
        let isPlatformSelected = viewModel.SelectedPlatformId != null ? true : false;

        viewModel.DeliveryListItems.forEach(function (entry) {
            if (isPlatformSelected == true) {
                entry.Visible = entry.WarehouseLocationId == viewModel.SelectedPlatformId ? true : false;
            }
            else {
                entry.Visible = entry.Status != 1 ? true : false;
            }
            if (entry.Status == -50) {
                entry.StateIcon = "fas fa-check-circle";
            } else {
                entry.StateText = entry.WorkstationName;
                entry.WorkstationPutTo = entry.WorkstationPutTo != null && entry.WorkstationPutTo.length > 0 ?
                    "(" + entry.WorkstationPutTo + ")" : null;
            }
            entry.Balance = entry.QtyDelivered - entry.QtyRequested;
            entry.WarehouseLocationName = pickingListShared.FormatWarehouseLocationName(entry.WarehouseLocationName);
        });
        if (viewModel.DeliveryListItems.length > 0 && isPlatformSelected == false) {
            viewModel.DeliveryListItems[0].Visible = true;
        }
    }

    function AddSwipeHParameterListItems() {
        var GridHeaderContent = document.getElementById('gridHeaderId');
        var mcs = new Hammer(GridHeaderContent);
        mcs.on("swipeleft", function (event) {
            let lastValParameterH = _parameterH;
            _parameterH = pickingListShared.SetHValue(lastValParameterH, -1);
            if (lastValParameterH != _parameterH) {
                GetList();
            }
        });
        mcs.on("swiperight", function (event) {
            let lastValParameterH = _parameterH;
            _parameterH = pickingListShared.SetHValue(lastValParameterH, 1);
            if (lastValParameterH != _parameterH) {
                GetList();
            }
        });
    }

    function ConfirmDelivery() {

        let workstationId = parseInt($(".selectedRow").attr("data-WorkstationId"));
        

        if (!(workstationId > 0)) {
            console.log("zdefiniuj stanowisko");
            $("#addWorkstationItemContent").removeClass("hidden");
        }
        else {
            ConfirmDeliveryJSON();
        }
    }
    function ConfirmDeliveryJSON(callback) {
        $("#btnConfirmDelivery").addClass("inputDisabled");

        var selectedRowId = $(".selectedRow").attr("data-id");
        var workstationName = $(".selectedRow").find(".workstationName").text();

        new JsonHelper().GetPostData("/iLOGIS/DeliveryListLineFeed/DeliveryListLFConfirmDelivery", {
            deliveryListItemId: selectedRowId,
            workstationName: workstationName
        }).done(function (status) {
            $("#btnConfirmDelivery").removeClass("inputDisabled");
            $(".selectedRow").find(".StatusIcon").removeClassPrefix("state");
            $(".selectedRow").find(".StatusIcon").addClass("state" + status);
            if (callback != null) callback();
        }).fail(function () {
            new Alert().Show("danger", "Wystąpił bląd przy rozładunku");
            if (callback != null) callback();
        });
    }
    function AddWorkstationItemJSON() {

        let inputLineWorkstation = $("#workstationName").val().split('.');

        let itemWMSId = parseInt($(".selectedRow").attr("data-itemWMSId"));
        let lineName = inputLineWorkstation.length > 1 ? inputLineWorkstation[0] : "";
        let workstationName = inputLineWorkstation.length > 1 ? inputLineWorkstation[1] : inputLineWorkstation[0];
        let putTo = inputLineWorkstation.length > 2 ? inputLineWorkstation[2] : "";


        var json = new JsonHelper().GetPostData("/iLOGIS/Config/WorkstationItemAddOrUpdate", {
            itemWMSId, lineName, workstationName, putTo
        });
        json.done(function () {
            new Alert().Show("success", "Zapisano stanowisko dla artykułu");
            $("#addWorkstationItemContent").addClass("hidden");
            $(".selectedRow").find(".workstationName").text(workstationName);
            //ConfirmDeliveryJSON();    
            ConfirmDeliveryJSON(GetList);

        });
    }

    function SelectPlatform(platformId) {
        if (viewModel.SelectedPlatformId == undefined) {
            viewModel.PlatformList.find(x => x.PlatformId == platformId).SelectedPlatform = "selectedPlatform";
            viewModel.SelectedPlatformId = platformId;
        }
        else {
            if (viewModel.SelectedPlatformId == platformId) {
                viewModel.PlatformList.find(x => x.PlatformId == platformId).SelectedPlatform = "";
                viewModel.SelectedPlatformId = undefined;
            }
            else {
                viewModel.PlatformList.find(x => x.PlatformId == viewModel.SelectedPlatformId).SelectedPlatform = "";
                viewModel.PlatformList.find(x => x.PlatformId == platformId).SelectedPlatform = "selectedPlatform";
                viewModel.SelectedPlatformId = platformId;
            }
        }

        //viewModel.PlatformLocationName = SetLocationName(viewModel.SelectedPlatformId);
        FilterRenderAddSwipe();
        //keypadLocation.Init();
    }
    function SetLocationName(platformId) {
        return platformId != undefined ? viewModel.PlatformList.find(x => x.PlatformId == platformId).PlatformLocationName : "";
    }
    function UpdatePlatformLocation() {
        var platformLocationName = $("#PlatformPosition_").val();
        if (pickingListShared.ValidatePlatform(platformLocationName) == true) {
            $("#contentView").html(ShowLoadingSnippet());
            let jsQ = new JsonHelper().Update(pickingListShared.urlPickingListPlatformUpdateLocation, { platformId: viewModel.SelectedPlatformId, platformLocationName });
            jsQ.done(function (platformLocationName) {
                viewModel.PlatformList.find(x => x.PlatformId == viewModel.SelectedPlatformId).SelectedPlatform = "";
                viewModel.PlatformList.find(x => x.PlatformId == viewModel.SelectedPlatformId).PlatformLocationName = platformLocationName;
                viewModel.SelectedPlatformId = undefined;
                FilterRenderAddSwipe();
                new Alert().Show("success", "Gotowe");
            });
        }
    }

    function Actions() {
        $(document).off("click", ".orderRow");
        $(document).on("click", ".orderRow", function () {
            $.each($.find(".orderRow"), function () {
                $(this).removeClass("selectedRow");
            });
            $(this).addClass("selectedRow");
        });

        $(document).off("click", "#btnConfirmDelivery");
        $(document).on("click", "#btnConfirmDelivery", function () {
            ConfirmDelivery();
        });

        $(document).off("click", "#btnBackToDeliveryListItems");
        $(document).on("click", "#btnBackToDeliveryListItems", function () {
            window.location.hash = doLink("/iLOGIS/DeliveryListLineFeed/DeliveryListLF",
                { transporterId: _transporterId });
        });

        $(document).off("click", ".PlatformName");
        $(document).on("click", ".PlatformName", function (event) {
            var platformId = $(this).attr("data-platformid");
            SelectPlatform(platformId);
        });

        $(document).off("click", "#SavePlatform_");
        $(document).on("click", "#SavePlatform_", function (event) {
            UpdatePlatformLocation();
        });
        $(document).off("click", "#btnOverrideWorkstationItem");
        $(document).on("click", "#btnOverrideWorkstationItem", function (event) {
            $("#addWorkstationItemContent").removeClass("hidden");
        });
        $(document).off("click", "#btnAddWorkstationItem");
        $(document).on("click", "#btnAddWorkstationItem", function (event) {
            AddWorkstationItemJSON();
        });
    }

}