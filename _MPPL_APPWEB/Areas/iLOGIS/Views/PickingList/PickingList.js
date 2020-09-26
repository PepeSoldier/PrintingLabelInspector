function PickingList() {
    var pickingListShared = new PickingListShared();
    var self = this;
    var _pickerId = 0;
    var viewModel = {};
    var viewData = {};

    this.Init = function () {
        _pickerId = $("#contentView").attr("data-pickerId");
        _lastPickingListId = $("#contentView").attr("data-lastPickingListId");
        _adminMode = $("#contentView").attr("data-adminMode") == "True" ? true : false;
        GetList();
        Actions();
    };

    function GetList() {
        $("#contentView").html(ShowLoadingSnippet());

        var JsonHelp = new JsonHelper();
        var ReturnJson = JsonHelp.GetPostData(pickingListShared.urlPickingListGetList, { pickerId: _pickerId });
        ReturnJson.done(function (PLWOviewModel) {
            viewModel.IsAdminMode = _adminMode;
            viewModel.TransporterId = PLWOviewModel.transporter.Id;
            viewModel.TransporterName = PLWOviewModel.transporter.Name;
            viewModel.DedicatedResources = PLWOviewModel.transporter.DedicatedResources;
            viewModel.List = PLWOviewModel.list;
            viewModel.List.forEach(function (entry) {
                entry.StateIcon = pickingListShared.GetStateIcon(entry.Status);
                entry.Guid = entry.Guid;
                entry.HasGuid = (entry.Guid == null || entry.Guid == "")? false : true;
            });
            $("#contentView").html("");
            filterPickingLists();
            RenderTemplate("#PickingListsBoxTemplate", "#contentView", viewModel);
            AddSwipeAndDoubleTapPickingList();
        });
    }

    function filterPickingLists() {
        if (!viewModel.IsAdminMode)
        {
            viewModel.List.forEach(function (pickingList) {
                if (pickingList.Status < 20) {
                    pickingList.hidden = "hidden";
                }
            })
        }
    }

    function Create() {
        return new Promise((resolve, reject) => {
            $("#mainMenu").html(ShowLoadingSnippetCreatingPickingList());
            let json = new JsonHelper().GetPostData(pickingListShared.urlPickingListCreate, {
                workOrderId: viewData.workOrderId, pickerId: viewData.pickerId
            });
            json.done(function (_pickingListId) {
                //return _pickingListId;
                viewData.pickingListId = _pickingListId;                
                //if (isConnectionWithFSDS(viewData.pickingListId)) {
                //    console.log("PROBLEM THERE IS CONNECTION WITH FSDS");
                //    window.location.hash = doLink(pickingListShared.urlPickingListItem, {
                //        pickingListId: viewData.pickingListId,
                //        workorderId: viewData.workOrderId,
                //        pickerId: viewData.pickerId
                //    });

                //    //GoToPickingListItem();
                //}
                //else {
                //    new Alert().Show("danger", viewData.pickingListId);
                //}
                console.log("GoToPickingListItem 4");
                resolve("PickingListCreated");
            });
        })
    }

    function CreateMany(_workOrderIds) {
        return new Promise((resolve, reject) => {
            $("#contentView").html(ShowLoadingSnippetCreatingPickingList());
            var json = new JsonHelper().GetPostData(pickingListShared.urlPickingListCreateMany, {
                workOrderIds: _workOrderIds, pickerId: viewModel.TransporterId
            });
            json.done(function (jsonModel) {
                new Alert().ShowJson(jsonModel);
                let pickingLists = jsonModel.Data
                pickingLists.forEach(function (pickingList) {
                    var pList = viewModel.List.find(x => x.WorkOrderId == pickingList.WorkOrderId);
                    pList.PickingListId = pickingList.Id;
                    pList.Status = pickingList.Status;
                    pList.StateIcon = pickingListShared.GetStateIcon(pickingList.Status);
                });
                $("#contentView").html("");
                RenderTemplate("#PickingListsBoxTemplate", "#contentView", viewModel);
                AddSwipeAndDoubleTapPickingList();
                resolve(pickingLists);
            });
            json.fail(function (item) {
                reject("PickingListNotCreated");
                new Alert().Show("danger", "Problem z połączeniem");
            });
        });
    }

    function GetPickingListsForAllPickers(_pickingListsIds) {
        return new Promise((resolve, reject) => {
            //$("#contentView").html(ShowLoadingSnippet());
            var json = new JsonHelper().GetPostData(pickingListShared.urlGetPickingListsForAllPickers, {
                pickingListsIds: _pickingListsIds,
            });
            json.done(function (pickingLists) {
                //$("#contentView").html("");
                resolve(pickingLists);
            });
            json.fail(function (item) {
                reject("PickingListNotCreated");
                new Alert().Show("danger", "Problem z połączeniem");
            });
        });
    }

    
    function CreateGroup(_pickingListIds) {
        var JsonHelp = new JsonHelper();
        let ReturnJson = JsonHelp.GetPostData("/iLogis/PickingList/PickingListCreateGroup", {
            pickingListIds: _pickingListIds,
        });
        ReturnJson.done(function (pickingListIds) {
            pickingListIds.forEach(function (item) {
                let pickingList = viewModel.List.find(x => x.PickingListId == item.Id);
                if (pickingList != null) {
                    pickingList.Guid = item.Guid;
                    pickingList.HasGuid = true;
                }
            })
            RenderTemplate("#PickingListsBoxTemplate", "#contentView", viewModel);
            AddSwipeAndDoubleTapPickingList();
            new Alert().Show("success", "Udało się utworzyć grupę");
        });

    }

    function GetViewData(selectedRow) {
        viewData.pickerId = $("#contentView").attr("data-pickerId");
        if (selectedRow == undefined) {
            viewData.workOrderId = $(".selectedRow").attr("data-workorderid");
            viewData.pickingListId = $(".selectedRow").attr("data-id");
            viewData.guid = $(".selectedRow").attr("data-guid");
        }
        else {
            viewData.workOrderId = $(selectedRow).attr("data-workorderid");
            viewData.pickingListId = $(selectedRow).attr("data-id");
            viewData.guid = $(selectedRow).attr("data-guid");
        }
    }
    function isConnectionWithFSDS(_pickingListId) {
        return !(isNaN(_pickingListId));
    }
    function GoToPickingListItem() {
        
        window.location.hash = doLink(pickingListShared.urlPickingListItem, {
            pickingListId: viewData.pickingListId,
            pickingListGuid: viewData.guid,
            workOrderId: viewData.workOrderId,
            pickerId: _pickerId
        });
    }

    function CreateOrGoToPickingListItems() {
        if (viewData.pickingListId == 0) {
            Create().then((data) => {
                console.log(data);
                GoToPickingListItem();
            });
        } else {
            GoToPickingListItem();
        }
    }

    function SelectingPickingLists(item) {
        let guid = $(item).attr("data-guid");
        if ($(item).hasClass("selectedRow")) {
            if (guid == "") {
                $(item).removeClass("selectedRow");
            } else {
                RemoveBySelector(".selectedRow");
            }
        } else {
            if (guid != "") {
                RemoveBySelector(".selectedRow");
                let guidArr = $.find("[data-guid='" + guid + "']");
                guidArr.forEach(function (itm) {
                    $(itm).addClass("selectedRow");
                });
            } else {
                RemoveBySelector("[data-guid != '']");
                $(item).addClass("selectedRow");
            }
        }
    }

    function RemoveBySelector(selector) {
        $.each($.find(selector), function (item) {
            $(this).removeClass("selectedRow");
        });
    }

    

    function DeleteGroup() {
        let _groupGuid = $(".orderRow").attr("data-guid");
        var JsonHelp = new JsonHelper();
        let ReturnJson = JsonHelp.GetPostData("/iLogis/PickingList/PickingListDeleteGroup", {
            pickingListGroupGuid: _groupGuid,
        });
        ReturnJson.done(function (pickingListByGroup) {
            pickingListByGroup.forEach(function (item) {
                let pickingList = viewModel.List.find(x => x.PickingListId == item.Id);
                if (pickingList != null) {
                    pickingList.Guid = item.Guid;
                    pickingList.HasGuid = false;
                }
            })
            RenderTemplate("#PickingListsBoxTemplate", "#contentView", viewModel);
            AddSwipeAndDoubleTapPickingList();
            new Alert().Show("success", "Udało się usunąć grupy");
        });

    }
    function AddSwipeAndDoubleTapPickingList() {
        $('.orderRow').each(function () {
            var elOrderRow = this;
            var mc = new Hammer(this);
            mc.on("swipeleft doubletap", function (event) {
                console.log('swipe the picking list -->');
                GetViewData(elOrderRow);
                CreateOrGoToPickingListItems();
                return false;
            });
            mc.on("swiperight", function (event) {
                GetViewData(elOrderRow);
                if (viewData.pickingListId == 0) {
                    new Alert().Show("info", "przesun w lewo by utworzyć picking listę");
                }
                else {
                    window.location.hash = doLink(pickingListShared.urlPickingListLineFeed, {
                        pickingListId: viewData.pickingListId,
                        workorderId: viewData.workOrderId,
                        pickerId: viewData.pickerId
                    });
                }
                return false;
            });
        });
    }

    function CreatePickingListsAndSetToPending() {
        let _workOrdersIds = [];
        let selectedRows = [];
        selectedRows = $(".selectedRow").toArray();
        selectedRows.forEach(function (item) {
            _workOrdersIds.push($(item).attr("data-workorderid"));
        })
        CreateMany(_workOrdersIds);
    }

    function isMultiSelectActive() {
        return $("#multiSelect").hasClass("active");
    }

    function SelectAllWithTheSameGuid(guid) {
        let guidArr = $.find("[data-guid='" + guid + "']");
        guidArr.forEach(function (itm) {
            $(itm).addClass("selectedRow");
        });
    }

    function Actions() {
            $(document).off("click", ".orderRow");
            $(document).on("click", ".orderRow", function () {
                let guid = $(this).attr("data-guid");
                if (isMultiSelectActive()) {
                    SelectingPickingLists(this);
                } else {
                    if (guid != "") {
                        RemoveBySelector(".selectedRow");
                        SelectAllWithTheSameGuid(guid);
                        $("#crtPickingListGroup").addClass("hidden");
                        $("#rmvPickingListGroup").removeClass("hidden");
                    } else {
                        RemoveBySelector(".orderRow");
                        $("#rmvPickingListGroup").addClass("hidden");
                        $(this).addClass("selectedRow");
                    }
                }
            });

       
            $(document).off("click", "#multiSelect");
            $(document).on("click", "#multiSelect", function () {
                if (isMultiSelectActive()) { // Wyłączanie multiselektu.
                    $(this).removeClass("active");
                    $("#crtPickingListGroup").addClass("hidden");
                    $("#rmvPickingListGroup").addClass("hidden");
                    RemoveBySelector(".selectedRow");

                    $.each($.find(".orderRow"), function () {
                        $(this).removeClass("hidden");
                    });
                } else { // Włączanie multiselektu.
                    $(this).addClass("active");
                    $("#crtPickingListGroup").removeClass("hidden");
                    $("#rmvPickingListGroup").addClass("hidden");
                }
            });

        $(document).off("click", "#createPickingListGroup");
        $(document).on("click", "#createPickingListGroup", function () {
            let numberofSelectedPickingLists = $.find(".selectedRow");
            if (numberofSelectedPickingLists.length < 2) {
                bootbox.alert("Zaznacz conajmniej 2 pozycje.");
            }
            else {
                let _workOrdersIds = [];
                let _pickingListsIds = [];
                let selectedRows = [];
                selectedRows = $(".selectedRow").toArray();
                selectedRows.forEach(function (selectedRow) {
                    if ($(selectedRow).attr("data-id") == 0) {
                        _workOrdersIds.push($(selectedRow).attr("data-workorderid"));
                    } else {
                        _pickingListsIds.push($(selectedRow).attr("data-id"));
                    }
                });

                GetPickingListsForAllPickers(_pickingListsIds).then((pickingLists) => {
                    pickingLists.forEach(function (pickingList) {
                        _pickingListsIds.push(pickingList.Id);
                    });
                });

                if (_workOrdersIds.length > 0) {
                    CreateMany(_workOrdersIds).then((pickingLists) => {
                        pickingLists.forEach(function (pickingList) {
                            _pickingListsIds.push(pickingList.Id);
                        });
                        CreateGroup(_pickingListsIds);
                    });
                } else {
                    CreateGroup(_pickingListsIds);
                }
            };
        });

        $(document).off("click", "#removePickingListGroup");
        $(document).on("click", "#removePickingListGroup", function () {
            DeleteGroup();
        });

        $(document).off("click", "#btnGoToPickingListItems");
        $(document).on("click", "#btnGoToPickingListItems", function () {
            if ($(".selectedRow").length == 0) {
                bootbox.alert("Wybierz Jedno z grupy zleceń");
            } else {
                GetViewData();
                CreateOrGoToPickingListItems();
            }
        });

        $(document).off("click", "#btnCreatePickingListItems");
        $(document).on("click", "#btnCreatePickingListItems", function () {
            CreatePickingListsAndSetToPending();
        });
        


        $(document).off("click", "#btnGoToSummary");
        $(document).on("click", "#btnGoToSummary", function () {
            var selectedRow = $(".selectedRow");
            let pickingListId = selectedRow.attr("data-id");
            let workOrderId = selectedRow.attr("data-workorderid");
            window.location.hash = doLink(pickingListShared.urlPickingListSummary, {
                pickingListId, workOrderId, pickerId: _pickerId
            });
        });
    }
   
    

    
}