function MesWorkplaceWorkorder(_workplace)
{
    var self = this;
    var wnd = null;
    var mesWorkplace = _workplace;
    var pageIndex = 1;
    var qtyPerPackageMethod = 0;

    //GETTERS, SETTERS
    this.SelectedWorkorder = {
        Id: 0,
        ItemId: 0,
        RemainQty: 0
    };

    this.Init = function () {
        Actions();
        self.RefreshWorkorders();
    };
    
    this.RefreshWorkorders = function () {
        console.log("REFRESH");
        $("#workorderGridData").html("");
        $("#workorderGridData").html(ShowLoadingSnippetNoText());

        var dateF = new moment().format("YYYY-MM-DD");
        var dateT = new moment().days(7).format("YYYY-MM-DD");

        $.post("/ONEPROD/MES/WorkplaceWorkorder",
            { dateFrom: dateF, dateTo: dateT, workplaceId: mesWorkplace.Id, pageIndex: pageIndex, pageSize: 13 },
            function (data) {
                //console.log(data);
                $("#workorderGridData").html(data);
                SelectWorkorder(self.SelectedWorkorder.Id, false);
            });
    };
    this.RefreshConfirmedWorkorders = function (confirmedWorkordersIds) {
        var dateF = new moment().format("YYYY-MM-DD");
        var dateT = new moment().days(7).format("YYYY-MM-DD");

        for (i = 0; i < confirmedWorkordersIds.length; i++) {
            var getWoTemplate = $.post("/ONEPROD/MES/WorkplaceWorkorder", {
                dateFrom: dateF,
                dateTo: dateT,
                workplaceId: mesWorkplace.Id,
                workorderId: confirmedWorkordersIds[i]
            });
            (function (id) {
                getWoTemplate.done(function (woTemplate) {
                    $("#planMonitorRow_" + id).html($(woTemplate + " .planMonitorRow").html());
                    if ($("#planMonitorRow_" + id).children(".gradient_ok").length > 0) {
                        $("#actionButtons").html("");
                    }
                    self.SetStatusActive();
                });
            })(confirmedWorkordersIds[i]);
        }
    };
    this.SetStatusActive = function () {
        $(".planMonitorRow[workorderId=" + self.SelectedWorkorder.Id + "]").find(".StatusWOInactive").addClass("hidden");
        $(".planMonitorRow[workorderId=" + self.SelectedWorkorder.Id + "]").find(".StatusWOActive").removeClass("hidden");
    };
    this.SetStatusInactive = function () {
        $(".planMonitorRow[workorderId*=" + self.SelectedWorkorder.Id + "]").find(".StatusWOInactive").removeClass("hidden");
        $(".planMonitorRow[workorderId*=" + self.SelectedWorkorder.Id + "]").find(".StatusWOActive").addClass("hidden");
    };
    this.DeleteWorkorder = function () {
        //var pmrow = $(document).find(".Popupwindow");
        $.ajax({
            url: "/ONEPROD/MES/DeleteWorkorder",
            type: "POST",
            data: '{workorderId: ' + self.SelectedWorkorder.Id + '}',
            contentType: 'application/json; charset=utf-8',
            success: function (result) {
                new Alert().Show("success", "Zlecenie zostało usunięte");
                self.RefreshWorkorders();
                wnd.Close();
            }
        });
    };

    function SelectWorkorder(id, boolResetQty) {
        let isItemCodeChanged = false;
        $(".planMonitorRow").find(".StatusWOActive").addClass("hidden");
        $(".planMonitorRow").find(".StatusWOInactive").removeClass("hidden");
        //if (self.SelectedWorkorder.Id > 0)
        if(id != null && id > 0)
        {
            let tempOldItemId = self.SelectedWorkorder.ItemId;
            $("#selectedWorkorderId").text(id);
            self.SelectedWorkorder.Id = id;
            self.SelectedWorkorder.ItemId = parseInt($("#planMonitorRow_" + id).attr("data-itemId"));
            self.SelectedWorkorder.RemainQty = parseInt($("#planMonitorRow_" + id).find(".qtyRemain").text());
            //self.SelectedWorkorder.Id = self.SelectedWorkorder.Id;
            //self.SelectedWorkorder.ItemCode = parseInt($("#planMonitorRow_" + id).find(".").text());

            isItemCodeChanged = tempOldItemId != self.SelectedWorkorder.ItemId;

            $(".planMonitorRow").removeClass("selectedRow");
            $(".planMonitorRow[workorderId=" + self.SelectedWorkorder.Id + "]").addClass("selectedRow");

            if (boolResetQty != true && mesWorkplace.SelectedTrolleyQty > 0) {
                self.SetStatusActive();
            }
        }

        if (boolResetQty) {
            mesWorkplace.WorkorderChanged(isItemCodeChanged);
        }
        //if (boolResetQty == true) {
        //    $("#btn1Piece .trolleyQty").text(0);
        //}
    }

    function ShowActionButtons() {
        _GetQtyPerPackage();
    }
    function _GetQtyPerPackage() {

        if (qtyPerPackageMethod == 0) {
            $.ajax({
                url: "/ONEPROD/WMS/GetQtyPerBox",
                type: "POST",
                data: '{workorderId: ' + self.SelectedWorkorder.Id + '}',
                contentType: 'application/json; charset=utf-8',
                success: function (result) {
                    if (result != null) {
                        if (result.length <= 0) {
                            qtyPerPackageMethod = 1;
                            _GetQtyPerPackage();
                        }
                        else {
                            _DrawActionButtons(result);
                        }
                    }
                }
            });
        }
        else if (qtyPerPackageMethod == 1 || mesWorkplace.IsTraceability) {
            try {
                $.ajax({
                    url: "/iLOGIS/WMS/GetQtyPerPackage",
                    type: "POST",
                    data: '{itemId: ' + self.SelectedWorkorder.ItemId + '}',
                    contentType: 'application/json; charset=utf-8',
                    success: function (result) {
                        if (result != null) {
                            if (result.length <= 0) {
                                qtyPerPackageMethod = -1;
                            }
                            else {
                                _DrawActionButtons(result);
                            }
                        }
                    },
                    error: function () {
                        _DrawActionButtons([]);
                    }
                });
            }
            catch {
                qtyPerPackageMethod = -1;
                _DrawActionButtons([]);
            }
        }
        else {
            _DrawActionButtons([]);
        }
    }
    function _DrawActionButtons(result) {

        $("#actionButtons").html("");
        //Draw buttons
        result.push(1);
        var countRounded = (result.length + result.length % 2);
        var height = 160 / countRounded;
        var width = 100 / (countRounded + 1);
        result.sort();

        for (i = 0; i < countRounded; i++) {
            if (i < result.length) {
                $("#actionButtons").append(
                    '<div class="brdL selectTrolleyQty clickable" style="width:' + width + '%;">' +
                    '<span class="trolleyQty"> ' + result[i] + '</span>' +
                    '</div>'
                );
            }
        }

        $("#actionButtons").append(
            '<div class="brdL selectTrolleyQty customTrolleyQty clickable" style="width:' + width + '%;">' +
            '<span class="trolleyQty">?</span>' +
            '</div>'
        );  
    }

    function ChangePage(dir) {

        let pageIndexTemp = pageIndex;

        if (dir >= 1) {
            pageIndex++;
        }
        else if (dir <= -1) {
            pageIndex--;
        }
        else {
            pageIndex = 1;
        }

        pageIndex = Math.max(pageIndex, 1);
        $("#workorderPageIndex").text(pageIndex);

        if (pageIndexTemp != pageIndex) {
            self.RefreshWorkorders();
        }
    }
    function OpenAddWorkOrderWindow() {
        console.log("Workplace.OpenAddWorkOrderWindow");
        wnd = new PopupWindow(850, 200);
        wnd.Init("windowAddWorkorder", 'Dodaj zlecenie do maszyny');
        wnd.Show("loading...");

        $.get("/ONEPROD/MES/AddWorkorder/?workplaceId=" + mesWorkplace.Id, function (data) {
            wnd.Show(data);
        });
    }
    function AddWorkorder(_qty, _itemCode) {
        console.log("Workplace.AddWorkorder - " + mesWorkplace.Id);

        $.ajax({
            url: "/ONEPROD/MES/AddWorkorder",
            type: "POST",
            data: { workplaceId: mesWorkplace.Id, "qty": _qty, "itemCode": _itemCode },
            success: function (result) {
                self.RefreshWorkorders();
                wnd.Close();
            },
            error: function () {
                new Alert().Show("danger", "Wystąpił problem. Zlecenie nie zostało dodane");
            }
        });
    }

    function Actions() {
        $(document).off("click", ".planMonitorRow");
        $(document).on("click", "#workplaceRefresh", function () {
            self.RefreshWorkorders();
        });

        $(document).off("click", ".planMonitorRow");
        $(document).on("click", ".planMonitorRow", function () {
            //self.SelectedWorkorder.Id = $(this).attr("workorderId");
            console.log("planMonitorRow click");
            id = parseInt($(this).attr("workorderId"));
            SelectWorkorder(id, true);
            ShowActionButtons();
        });

        $(document).off("click", "#btnAddWorkOrder");
        $(document).on("click", "#btnAddWorkOrder", function () {
            console.log("Workplace.btnAddWorkOrder");
            OpenAddWorkOrderWindow();
        });

        $(document).off("click", "#DeleteWorkorder");
        $(document).on("click", "#DeleteWorkorder", function () {
            self.DeleteWorkorder();
        });

        $(document).off("click", "#ConfirmAddWorkorderSubmit");
        $(document).on("click", "#ConfirmAddWorkorderSubmit", function () {
            console.log("Workplace.#ConfirmAddWorkorderSubmit");
            var _qty = parseInt($("#qty").val());
            var _itemCode = $("#ItemCode").val();
            AddWorkorder(_qty, _itemCode);
        });

        $(document).off("click", ".btnChangeWorkorderPage");
        $(document).on("click", ".btnChangeWorkorderPage", function () {
            let val = parseInt($(this).attr("data-Value"));
            ChangePage(val);
        });
    }    
}