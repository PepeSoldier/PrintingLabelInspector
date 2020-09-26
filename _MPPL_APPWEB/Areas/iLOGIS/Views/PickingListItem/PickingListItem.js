
function PickingListItem() {
    var pickingListShared = new PickingListShared();
    var keypadLocation = new KeyPadDigitDoubleRowsWithoutClose("#PlatformPosition_", "#kaypad115", "L");
    var self = this;
    var _pickerId = 0;
    var _pickingListId = 0;
    var _workOrderId = 0;
    var _parameterH = -1;
    var _pickingListGuid = "";
    var _defaultTrolley = "";
    //var vc = new VersionController(0);
   
    //var ParameterH = -1;//parametersH_Array[Math.round((parametersH_Array.length - 1) / 2)];
    var viewModel = {
        PickingListItems: [],
        SelectedPlatformId: undefined,
    };

    this.Init = function () {
        _pickerId = $("#contentView").attr("data-pickerId");
        _workOrderId = $("#contentView").attr("data-workorderId");
        _pickingListId = $("#contentView").attr("data-pickingListId");
        _pickingListGuid = $("#contentView").attr("data-pickingListGuid");
        GetData();
        self.AddOnScan();
        Actions();
    };

    function GetData() {
        $("#contentView").html(ShowLoadingSnippet());
        var JsonHelp = new JsonHelper();
        var ReturnJson;
        ReturnJson = JsonHelp.GetPostData(pickingListShared.urlPickingListItemGetList, {
            workOrderId: _workOrderId,
            pickerId: _pickerId,
            parameterH: _parameterH,
            pickingListGuid: _pickingListGuid,
        });
        ReturnJson.done(function (PickingListItemsViewModel) {
            viewModel = PickingListItemsViewModel;
            viewModel.ParameterHView = _parameterH == -1 ? '*' : _parameterH;
            viewModel.PickinListStatusView = viewModel.PickingListStatus < 50 ? false : true;
            viewModel.SelectedPlatformId = undefined;
            viewModel.HasGuid = viewModel.PickingListGuid == null ? false : true;
            viewModel.IsManyProductionOrders = viewModel.ProductionOrderList.length > 1 ? true : false;
            _filterPickingListItems();
            Render();
            AddSwipeHParameterListItems();
            AddSwipeAndDoubletapPickingListItems();

            $($(".orderRow")[0]).addClass("selectedRow");
            //GetVersionOfPickingList();
        });
    }

    function GetVersionOfPickingList() {
        var ReturnJson;
        ReturnJson = new JsonHelper().GetPostData("/iLOGIS/PickingList/GetVersion", {});
        ReturnJson.done(function (version) {
            //vc.CheckVersion(version); 
        });
    }

    function PlatformLocationUpdate() {
        var platformLocationName = $("#PlatformPosition_").val();

        if (pickingListShared.ValidatePlatform(platformLocationName) == true)
        {
            $("#contentView").html(ShowLoadingSnippet());
            let jsQ = new JsonHelper().Update(pickingListShared.urlPickingListPlatformLocationUpdate,
                {
                    platformId: viewModel.SelectedPlatformId,
                    platformLocationName
                });
            jsQ.done(function (platformLocationName) {
                //viewModel.PlatformList.find(x => x.PlatformId == viewModel.SelectedPlatformId).SelectedPlatform = "";
                //viewModel.PlatformList.find(x => x.PlatformId == viewModel.SelectedPlatformId).PlatformLocationName = platformLocationName;
                //viewModel.SelectedPlatformId = undefined;
                //FilterRenderAddSwipe();
                GetData();
                new Alert().Show("success", "Gotowe");
            });
        }
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

        viewModel.PlatformLocationName = _setLocationName(viewModel.SelectedPlatformId);
        _filterPickingListItems();
        Render();
        AddSwipeHParameterListItems();
        keypadLocation.Init();
    }
    function _setLocationName(platformId) {
        return platformId != undefined ? viewModel.PlatformList.find(x => x.PlatformId == platformId).PlatformLocationName : "";
    }
    function _filterPickingListItems() {
        let isPlatformSelected = viewModel.SelectedPlatformId != null ? true : false;

        viewModel.PickingListItems.forEach(function (entry) {
            if (isPlatformSelected == true) {
                entry.Visible = entry.PlatformId == viewModel.SelectedPlatformId ? true : false;
            }
            else {
                entry.Visible = entry.Status > 20 ? true : false;
            }
            entry.StateIcon = pickingListShared.GetStateIcon(entry.Status);
            entry.WarehouseLocationName = pickingListShared.FormatWarehouseLocationName(entry.WarehouseLocationName);
        });
        // Sprawdzić, czy są jakieś ze statusem powyżej 20
        // Jeśli tak to odkryć wszystkie te same z tymi ItemCode'ami
        // Jeśli nie ma żadnej o statusie 20 to pobrać pierwszą z góry i ją odsłonić
        // rezem ze wszystkimi ItemCode'ami
        
        if (viewModel.PickingListItems.length > 0 && isPlatformSelected == false) {
            viewModel.PickingListItems.forEach(function (entry) {
                if (entry.Status > 20) {
                    viewModel.PickingListItems.forEach(function (item) {
                        if (entry.ItemCode == item.ItemCode && entry.WarehouseLocationId == item.WarehouseLocationId) {
                            item.Visible = true;
                        }
                    });
                }
            });
            
            let isAnyVisible = viewModel.PickingListItems.find(x => x.Visible == true && x.Status == 20);

            if (isAnyVisible == null || isAnyVisible.length == 0) {
                viewModel.PickingListItems[0].Visible = true;
                let pickingListItemVisible = viewModel.PickingListItems[0];
                viewModel.PickingListItems.forEach(function (entry) {
                    if (entry.ItemCode == pickingListItemVisible.ItemCode) {
                        entry.Visible = true;
                    }
                });
            }
        }
    }

    function SaveStateAndGoBackToPickingList() {
        var json = new JsonHelper().GetPostData(pickingListShared.urlPickingListSetStatus,
            { pickingListId: _pickingListId, pickerId: _pickerId, workOrderId: _workOrderId });
        json.done(function () {
            window.location.hash = doLink(pickingListShared.urlPickingList, { pickerId: _pickerId, lastPickingListId: _pickingListId });
        });
    }

    function GoToPickingListItemManage() {
        var selectedRow = $(".selectedRow");
        let PickingListItemId = 0;

        if (selectedRow.length == 0) {
            bootbox.alert("Wybierz wiersz");
        }
        else {
            PickingListItemId = selectedRow.attr("data-id");
        }
        new PickingListItemManage(PickingListItemId, viewModel.ScannedBarcode, _defaultTrolley, ItemManageCallback);
    }
    function GoToPickingListItemManage_OnScan(sCode) {
        //funkcja się nazywała AssignScannedBarcode
        console.log("GoToPickingListItemManage_OnScan");
        console.log("sCode: " + sCode);
        let ssCode = sCode.replace("\u0010", "");
        console.log("ssCode: " + ssCode);

        if (viewModel.SelectedPlatformId != undefined)
        {
            $("#PlatformPosition_").val(ssCode);
        }
        else
        {
            let selectedRow = $(".selectedRow");
            let PickingListItemId = selectedRow.attr("data-id");

            if ($(".selectedRow").length == 1) {
                viewModel.ScannedBarcode = ssCode;
                new PickingListItemManage(PickingListItemId, viewModel.ScannedBarcode, _defaultTrolley, ItemManageCallback);
            }
        }
    }
    function ItemManageCallback(enumPickingListItemStatus, defTrolley) {
        console.log("ItemManageCallback");

        InitOnScan(function (sCode, iQty) {
            GoToPickingListItemManage_OnScan(sCode);
        });

        switch (enumPickingListItemStatus) {
            case undefined || null:
                console.log("ItemManageClose");
                break;
            case 10:
                new Alert().Show("info", "ZNALEZIONO NOWE LOKACJE");
                break;
            case 40:
                new Alert().Show("danger", "NIE ZNALEZIONO NOWYCH LOKACJI");
                break;
            case 50:
                new Alert().Show("success", "GOTOWE!");
                break;
            case 60:
                new Alert().Show("warning", "ZAPISANO I ZNALEZIONO NOWE LOKACJE");
                break;
            default:
                new Alert().Show("danger", "NIEZNANY BŁĄD - BRAK ZAPISU");
        }
        _defaultTrolley = defTrolley;
    
        GetData();
    }

    function Render() {
        RenderTemplate("#PickingListItemsTemplate", "#contentView", viewModel);
    }

    function AddSwipeHParameterListItems() {
        var GridHeaderContent = document.getElementById('gridHeaderId');
        var mcs = new Hammer(GridHeaderContent);
        mcs.on("swipeleft", function (event) {
            let lastValParameterH = _parameterH;
            _parameterH = pickingListShared.SetHValue(lastValParameterH, -1);
            if (lastValParameterH != _parameterH) {
                GetData();
            }
        });
        mcs.on("swiperight", function (event) {
            let lastValParameterH = _parameterH;
            _parameterH = pickingListShared.SetHValue(lastValParameterH, 1);
            if (lastValParameterH != _parameterH) {
                GetData();
            }
        });
    }
    function AddSwipeAndDoubletapPickingListItems() {
        $('.orderRow').each(function () {
            var elOrderRow = this;
            var mc = new Hammer(this);
            mc.on("swipeleft doubletap", function (event) {
                console.log("swipe double tap");
                let PickingListItemId = $(elOrderRow).attr("data-id");
                new PickingListItemManage(PickingListItemId, viewModel.ScannedBarcode, _defaultTrolley,ItemManageCallback);
            });
        });
    }
    this.AddOnScan = function () {
        InitOnScan(function (sCode, iQty) {
            GoToPickingListItemManage_OnScan(sCode);
        });
    };
    function Actions() {
        $(document).off("click", ".orderRow");
        $(document).on("click", ".orderRow", function () {
            $.each($.find(".orderRow"), function () {
                $(this).removeClass("selectedRow");
            });
            $(this).addClass("selectedRow");
        });

        $(document).off("click", ".PlatformName");
        $(document).on("click", ".PlatformName", function (event) {
            var platformId = $(this).attr("data-platformid");
            SelectPlatform(platformId);
        });

        $(document).off("click", "#SavePlatform_");
        $(document).on("click", "#SavePlatform_", function (event) {
            PlatformLocationUpdate();
        });

        $(document).off("click", "#btnBackToPickingList");
        $(document).on("click", "#btnBackToPickingList", function () {
            window.location.hash = doLink(pickingListShared.urlPickingList, { pickerId: _pickerId, lastPickingListId: _pickingListId });
        });

        $(document).off("click", "#btnBackToPickingListDataNull");
        $(document).on("click", "#btnBackToPickingListDataNull", function () {
            viewModel.PickingListStatus = 25;
            SaveStateAndGoBackToPickingList();
        });

        $(document).off("click", "#PickingListItemHeader");
        $(document).on("click", "#PickingListItemHeader", function () {
            if ($(".productionOrderDetails").hasClass("hidden")) {
                $(".productionOrderDetails").removeClass("hidden");
            } else {
                $(".productionOrderDetails").addClass("hidden");
            }
        });


        $(document).off("click", "#btnComplete");
        $(document).on("click", "#btnComplete", function () {
            SaveStateAndGoBackToPickingList();
        });

        $(document).off("click", "#btnGoToItemManage");
        $(document).on("click", "#btnGoToItemManage", function () {
            GoToPickingListItemManage();
        });

        $(document).off("click", "#btnGoToSummary");
        $(document).on("click", "#btnGoToSummary", function () {
            window.location.hash = doLink(pickingListShared.urlPickingListSummary, {
                pickingListId : _pickingListId,
                workOrderId: _workOrderId,
                pickerId: _pickerId,
                pickingListGuid: _pickingListGuid
            });
        });
    }

}