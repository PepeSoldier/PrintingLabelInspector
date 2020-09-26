function PickingListSummary() {
    var pickingListShared = new PickingListShared();
    var viewModel = {};

    this.Init = function () {
        GetList();
        Actions();
    };

    function GetList() {
        viewModel.PickingListId = $("#contentView").attr("data-pickinglistid");
        viewModel.PickerId = $("#contentView").attr("data-pickerid");
        viewModel.WorkOrderId = $("#contentView").attr("data-workorderid");
        viewModel.PickingListGuid = $("#contentView").attr("data-pickinglistguid");
        $("#contentView").html(ShowLoadingSnippet());
        var JsonHelp = new JsonHelper();
        var ReturnJson = JsonHelp.GetPostData("/iLOGIS/PickingListItem/SummaryGetList", {
            workOrderId: viewModel.WorkOrderId,
            pickerId: viewModel.PickerId,
            pickingListGuid: viewModel.PickingListGuid,
        });
        ReturnJson.done(function (summaryList) {
            viewModel.SummaryList = summaryList;
            viewModel.ConnectedTransporters = summaryList[0] != null ? summaryList[0].ConnectedTransporters : "";
            viewModel.SummaryList.forEach(function (entry) {
                entry.StateIcon = GetStateIcon(entry.Status);
            });
            RenderTemplate("#PickingListSummaViewTemplate", "#contentView", viewModel);
        });
    }
    function GetStateIcon(state) {
        if (state == 10) {
            stateIcon = "fas fa-play-circle";
        }
        else if (state == 20) {
            stateIcon = "far fa-play-circle";
        }
        else if (state == 30) {
            stateIcon = "fas fa-dolly";
        }
        else if (state == 40) {
            stateIcon = "fas fa-flag-checkered";
        }
        else if (state == 50) {
            stateIcon = "fas fa-exclamation-circle";
        }
        else if (state == 60) {
            stateIcon = "fas fa-exclamation-circle";
        }
        return stateIcon;
    };
    function Actions() {
        $(document).off("click", "#btnGoToPickingList");
        $(document).on("click", "#btnGoToPickingList", function () {
            window.location.hash = doLink(pickingListShared.urlPickingListItem, {
                pickingListId: viewModel.PickingListId,
                workorderId: viewModel.WorkOrderId,
                pickerId: viewModel.PickerId
            });
        });
    }
}
