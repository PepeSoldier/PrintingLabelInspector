function DeliveryListLF(hasUserAdminRights) {
    var pickingListShared = new PickingListShared();
    var self = this;
    var _transporterId = 0;
    var viewModel = {
        TransporterId: 0,
        TransporterName: 0,
        HasUserAdminRights: hasUserAdminRights,
        List: [{}]
    };
    var viewData = {};

    this.Init = function () {
        console.log("viewModel.HasUserAdminRights");
        console.log(viewModel.HasUserAdminRights);
        _transporterId = $("#contentView").attr("data-transporterId");
        _lastDeliveryListId = $("#contentView").attr("data-lastdeliverylistid");
        GetList();
        Actions();
    };

    function GetList() {
        $("#contentView").html(ShowLoadingSnippet());

        var JsonHelp = new JsonHelper();
        var ReturnJson = JsonHelp.GetPostData("/iLOGIS/DeliveryListLineFeed/DeliveryListLFGetList", { transporterId: _transporterId });
        ReturnJson.done(function (deliveryListViewModelList) {
            //debugger;
            if (deliveryListViewModelList.length > 0) {
                viewModel.TransporterId = deliveryListViewModelList[0].TransporterId;
                viewModel.TransporterName = deliveryListViewModelList[0].TransporterName;
                //viewModel.DedicatedResources = deliveryListViewModelList.transporter.DedicatedResources;
                viewModel.List = deliveryListViewModelList;
                viewModel.List.forEach(function (entry) {
                    entry.StateIcon = pickingListShared.GetStateIconLineFeed(entry.Status);

                    while (entry.PickingListStatuses.length < 6) {
                        entry.PickingListStatuses.push(0);
                    }
                });
                $("#contentView").html("");
                RenderTemplate("#DeliveryListsBoxTemplate", "#contentView", viewModel);
                $($(".orderRow")[0]).addClass("selectedRow");
                AddSwipeAndDoubleTapDeliveryList();
            }
        });
    }
    function Create() {
        return new Promise((resolve, reject) => {
            $("#mainMenu").html(ShowLoadingSnippet());
            let json = new JsonHelper().GetPostData(pickingListShared.urlDeliveryListCreate, {
                workOrderId: viewData.workOrderId, transporterId: viewData.transporterId
            });
            json.done(function (_deliveryListId) {
                //return _deliveryListId;
                viewData.deliveryListId = _deliveryListId;

                console.log("GoToDeliveryListItem 4");
                resolve("DeliveryListCreated");
            });
        });
        
    }

    function GetViewData(selectedRow) {
        viewData.transporterId = $("#contentView").attr("data-transporterId");
        if (selectedRow == undefined) {
            viewData.workOrderId = $(".selectedRow").attr("data-workorderid");
            viewData.deliveryListId = $(".selectedRow").attr("data-id");
        }
        else {
            viewData.workOrderId = $(selectedRow).attr("data-workorderid");
            viewData.deliveryListId = $(selectedRow).attr("data-id");
        }
    }
    function isConnectionWithFSDS(_deliveryListId) {
        return !(isNaN(_deliveryListId));
    }
    function GoToDeliveryListItem() {
        
        window.location.hash = doLink("/iLOGIS/DeliveryListLineFeed/DeliveryListItemsLF", {
            deliveryListId: viewData.deliveryListId,
            workorderId: viewData.workOrderId,
            transporterId: viewData.transporterId
        });
    }

    function Actions() {
        $(document).off("click",".orderRow");
        $(document).on("click",".orderRow", function () {
            $.each($.find(".orderRow"), function () {
                $(this).removeClass("selectedRow");
            });
            $(this).addClass("selectedRow");
        });

        $(document).off("click", "#btnGoToDeliveryListItems");
        $(document).on("click", "#btnGoToDeliveryListItems", function () {
            console.log("click btnGoToDeliveryListItems");
            if ($(".selectedRow").length == 0) {
                bootbox.alert("Wybierz Jedno ze zleceń");
            } else {
                GetViewData($(".selectedRow")[0]);
                GoToDeliveryListItem();
            }
        });

        $(document).off("click", "#btnGoToSummary");
        $(document).on("click", "#btnGoToSummary", function () {
            var selectedRow = $(".selectedRow");
            let deliveryListId = selectedRow.attr("data-id");
            let workOrderId = selectedRow.attr("data-workorderid");
            window.location.hash = doLink(pickingListShared.urlDeliveryListSummary, {
                deliveryListId, workOrderId, transporterId: _transporterId
            });
        });

        $(document).off("click", "#btnCloseDeliveryList");
        $(document).on("click", "#btnCloseDeliveryList", function () {
            ShowLoadingSnippetWithOverlay();
            let deliveryListId = parseInt($($(".selectedRow")[0]).attr("data-id"));
            let workorderId = parseInt($(".selectedRow").attr("data-workorderid"));
            let transporterId = parseInt($("#contentView").attr("data-transporterId"));

            new JsonHelper()
                .GetPostData("/iLOGIS/DeliveryListLineFeed/DeliveryListLFConfirmAllAndClose", { deliveryListId, transporterId, workorderId })
                .done(function (status) {
                    RemoveLoadingSnippetWithOverlay();
                    new Alert().Show("success", "Gotowe");
                    let icon = pickingListShared.GetStateIconLineFeed(status);
                    $($(".selectedRow")[0]).find(".StatusIcon").removeClassPrefix("state");
                    $($(".selectedRow")[0]).find(".StatusIcon").removeClassPrefix("fa");
                    $($(".selectedRow")[0]).find(".StatusIcon").addClass(icon);
                    $($(".selectedRow")[0]).find(".StatusIcon").addClass("state" + status);
                })
                .fail(function () {
                    RemoveLoadingSnippetWithOverlay();
                    new Alert().Show("danger", "Coś poszło nie tak");
                });
        });
    }
    function AddSwipeAndDoubleTapDeliveryList() {
        $('.orderRow').each(function () {
            var elOrderRow = this;
            var mc = new Hammer(this);
            mc.on("swipeleft doubletap", function (event) {
                GetViewData(elOrderRow);
                GoToDeliveryListItem();
                return false;
            });
        });
    }
    

    
}