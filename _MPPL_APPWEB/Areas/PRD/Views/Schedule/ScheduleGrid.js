function ScheduleGrid(statusGrid) {

    var self = this;
    this.InitGrid = function () {
        $("#ScheduleGrid").jsGrid({
            width: "100%", height: "800px",
            inserting: false, editing: false, sorting: false, paging: false, filtering: true,
            fields: [
                { name: "OrderId", type: "text", title: "Id", width: 0, filtering: false, css: "hidden orderId" },
                { name: "Temp", type: "text", title: "K", width: 70, filtering: false, css: "drag-handler", cellRenderer: RenderDragCell },
                { name: "StartDate", type: "text", title: "Data", width: 130, css: "OrderDateTime", cellRenderer: FormatDate, filterTemplate: FilterDate, filterValue: FilterDateVal },
                { name: "Line", type: "text", title: "Linia", width: 60, filterTemplate: FilterLine, filterValue: FilterLineVal },
                { name: "OrderNo", type: "text", title: "Nr zlecenia", width: 110 },
                { name: "PNC", type: "text", title: "PNC", width: 110 },
                { name: "QtyPlanned", type: "text", title: "Szt P", width: 60, filtering: false },
                { name: "QtyRemain", type: "text", title: "Szt R", width: 60, filtering: false },
                { name: "Notice", type: "text", title: "Notka", width: 200, filtering: false },
                { name: "SeqTemp", type: "text", title: "Seq", width: 80, filtering: false, css: "sequence", cellRenderer: FormatSeqTemp },
                { name: "SeqOriginal", type: "text", title: "Seq", width: 80, filtering: false, css: "OrgSeq" },
                { name: "StateA", type: "text", title: "A", width: 25, filtering: false, css: "stateA" },
                { name: "StateB", type: "text", title: "B", width: 25, filtering: false, css: "stateB" },
                { name: "FirstProductIn", type: "text", title: "Start", width: 130, css: "FirstProductIn", filtering: false, cellRenderer: FormatDate },
            ],
            controller: {
                loadData: function (filter) {
                    var json = null;
                    //console.log(filter);
                    $.ajax({
                        async: false, global: false, dataType: "json", type: "POST",
                        url: "/PRD/Schedule/GetSchedule",
                        data: filter,
                        success: function (data) {
                            json = data;
                            DoDragAndDrop();
                            new Alert().Show("success", "Załadowano dane");
                        },
                        error: function (xhr, ajaxOptions, thrownError) {
                            new Alert().Show("danger", thrownError);
                        }
                    });
                    return json;
                },
            },
            onDataLoaded: function () {
                var fe = FindFirstEmpty();

                if (fe > 0) {
                    var prevPlannedDate = $($("#ScheduleGrid tbody").find(".OrderDateTime")[fe - 1]).text();
                    var prevRealDate = $($("#ScheduleGrid tbody").find(".FirstProductIn")[fe - 1]).text();
                    var thisPlannedDate = $($("#ScheduleGrid tbody").find(".OrderDateTime")[fe]).text();

                    var tdiff = new Date(prevRealDate) - new Date(prevPlannedDate);
                    var t = new Date(thisPlannedDate);
                    var newT = new Date(t.getTime() + tdiff);
                    var minutes = tdiff / (1000 * 60);
                    //console.log(prevPlannedDate + ", " + prevRealDate + " => " + tdiff);
                    //console.log(thisPlannedDate + ", " + newT);

                    $($("#ScheduleGrid tbody").find(".FirstProductIn")[fe]).html(
                        minutes + " [min] => " + moment(newT).format('HH:mm') + 
                        "<span id=\"btnScheduleGridCalculateClock\" class=\"fas fa-clock\" data-newtime=" + moment(newT).format('Y-MM-D_HH:mm') + "></span>"
                    );
                    if (Math.abs(minutes) >= 15)
                        $($("#ScheduleGrid tbody").find(".FirstProductIn")[fe]).css("color", "red");
                    else
                        $($("#ScheduleGrid tbody").find(".FirstProductIn")[fe]).css("color", "green");
                }
                else {
                    $($("#ScheduleGrid tbody").find(".FirstProductIn")[fe]).html("b/d");
                }
                
            },
            rowClass: function (item, itemIndex) {
                var rowclass = "";
                rowclass += (item.IsSeqChenged == true) ? "rowSeqChanged" : "rowNotChanged";
                rowclass += " stateA" + item.StateA;
                rowclass += " stateB" + item.StateB;
                return rowclass;
            },
            rowClick: function (args) {
                statusGrid.RefreshGrid(args.item.OrderId);
                $("#selectedOrderId").val(args.item.OrderId);
                $("#selectedOrderNo").val(args.item.OrderNo);
                $("#newLine").val(args.item.Line);
                $("#newQty").val(args.item.QtyRemain);
                $("#qtyProducedInPast").val(args.item.QtyProducedInPast);
                $("#newStartTime").val(moment(moment(args.item.StartDate).toDate()).format("YYYY-MM-DD HH:mm"));

                $(".highlight").toggleClass("highlight");
                this.rowByItem(args.item).toggleClass("highlight");
            }
        });
    }
    this.RefreshGrid = function () {
        $("#ScheduleGrid").jsGrid("search");
    }
    this.FilterLineVal = function () {
        return FilterLineVal();
    }
    this.ShowSnapshots = function () {
        $("#snapshot #snapshotsUl").remove();
        $("#snapshot").append('<ul id="snapshotsUl" style="padding: 0;">');
        var snapshots = GetSnapshots();
        for (i = 0; i < snapshots.length; i++) {
            $("#snapshotsUl").append(
                '<li style="list-style: none;">' +
                '<div class="snapshotLi">' +
                '<span class="snapshotNo">' + snapshots[i].SnapshotNo + '</span><span>. </span>' +
                '<span>' + moment(moment(snapshots[i].CreationDate).toDate()).format("MMM-DD HH:mm:ss") + ' </span>' +
                '<span>' + snapshots[i].LineName + ' </span>' +
                '<span>' + snapshots[i].CreatorUserName + ' </span>' +
                '<span>[' + snapshots[i].TotalChanges + ']</span>' +
                '<i class="fas fa-eye snapshotShowChanges" data-toggle="tooltip" data-placement="bottom" title="pokaż zmiany"></i>' +
                '<i class="fas fa-trash-alt snapshotDelete" data-toggle="tooltip" data-placement="bottom" title="skasuj"></i>' +
                '</div>' +
                '</li>'
            );
        }
    }
    this.ShowChanges = function (snapshotNo) {
        var orderIds;
        $.ajax({
            url: "/PRD/Schedule/GetSnapshotOrders?snapshotNo=" + snapshotNo,
            type: "GET",
            dataType: "json",
            async: false,
            success: function (orderIds1) {
                orderIds = orderIds1;
            }
        });
        $(".sequence").removeClass("seqChanged");

        for (i = 0; i < orderIds.length; i++) {
            $(".orderId:contains('" + orderIds[i] + "')").parent().find(".sequence").addClass("seqChanged");
        }
    }
    this.DeleteSnapShot = function (snapshotNo) {
        $.ajax({
            url: "/PRD/Schedule/DeleteSnapshot?snapshotNo=" + snapshotNo,
            type: "GET",
            dataType: "json",
            async: false,
            success: function (alertMsg) {
                console.log(alertMsg);
                self.ShowSnapshots();
                self.RefreshGrid();
            }
        });
    }
    this.ChangeQty = function (orderId, newQty, callbackObj) {
        $.ajax({
            url: "/PRD/Schedule/ChangeQty?orderId=" + orderId + "&newQty=" + newQty,
            type: "GET",
            dataType: "json",
            success: function (actions) {
                callbackObj.RefreshGrid();
            }
        });
    }
    this.SetQtyProducedInPast = function (orderId, qtyProducedInPast, callbackObj) {
        $.ajax({
            url: "/PRD/Schedule/SetQtyProducedInPast?orderId=" + orderId + "&qtyProducedInPast=" + qtyProducedInPast,
            type: "GET",
            dataType: "json",
            success: function (actions) {
                callbackObj.RefreshGrid();
            }
        });
    }
    this.ChangeLine = function (orderId, lineName) {
        $.ajax({
            url: "/PRD/Schedule/ChangeLine?orderId=" + orderId + "&lineName=" + lineName,
            type: "GET",
            dataType: "json",
            success: function (actions) {
            }
        });
    }
    this.CalculateWholeLine = function (orderId, lineName, newStartTime) {
        $.ajax({
            url: "/PRD/Schedule/CalculateAllLine?orderId=" + orderId + "&lineName=" + lineName + "&newStartTime=" + newStartTime,
            type: "POST",
            dataType: "json",
            success: function (actions) {
                self.RefreshGrid();
            }
        });
    }

    function FindFirstEmpty() {
        var found = false;
        var returnIndex = 0
        $("#ScheduleGrid tbody").find(".FirstProductIn").each(function (index) {
            if ($(this).text() == "" && !found) {
                //console.log($(this).text())
                found = true;
                returnIndex = index;
            }
        })

        return returnIndex;
    }
    function FormatDate(value, item) {
        var dateTxt = value != null ? moment(moment(value).toDate()).format("YYYY-MM-DD HH:mm") : "";
        return $('<td>').append(dateTxt);
    }
    function FormatSeqTemp(value, item) {
        if (item.SeqOriginal != item.SeqTemp) {
            return $('<td class="seqChanged">').append(value);
        }
        else {
            return $("<td>").append(value);
        }
    }
    function FilterDate() {
        Date.prototype.addHours = function (h) {
            this.setHours(this.getHours() + h);
            return this;
        }

        var currentdate = new Date().addHours(-2);
        //currentdate = currentdate

        var datetime = currentdate.getFullYear() + "-"
            + (100 + (currentdate.getMonth() + 1)).toString().substring(1, 3) + "-"
            + (100 + currentdate.getDate()).toString().substring(1, 3) + " "
            + currentdate.getHours() + ":00"
            //+ currentdate.getMinutes();

        return $("<input type='text' class='datetimepicker jsGridFilter' id='refTime'>").attr("value", datetime);
    }
    function FilterDateVal() {
        return $("#refTime").val();
    }
    function FilterLine() {
        return $("<input type='text' id='filterLine' class='jsGridFilter'>").attr("value", 101);
    }
    function FilterLineVal() {
        return $("#filterLine").val();
    }
    function RenderDragCell(value, item) {
        return $('<td>').append('<span class="fas fa-bars dragCell"></span>');
    }
    function DoDragAndDrop() {
        Sortable.create(
            $('#ScheduleGrid .jsgrid-grid-body .jsgrid-table tbody')[0], {
                animation: 150,
                scroll: true,
                handle: '.drag-handler',
                onStart: function (evt) {
                    $(evt.item.cells[1]).css("background-color", "yellow");
                },
                onEnd: function (evt) {
                    UpdateSequenceOfRow(evt.newIndex, evt.item);
                }
            }
        );
    }
    function ReadSequenceOfRow(rowIndex) {
        var row1 = $('#ScheduleGrid .jsgrid-grid-body .jsgrid-table tbody').children()[rowIndex];
        var sequence = $(row1).find(".sequence").text();
        return sequence;
    }
    function UpdateSequenceOfRow(rowIndex, row) {
        var newValue = 0;
        var OrgSeq = Number($(row).find(".OrgSeq").text());
        var prevSeq = Number(ReadSequenceOfRow(rowIndex - 1));
        var nextSeq = Number(ReadSequenceOfRow(rowIndex + 1));
        var orderId = Number($(row.cells[0]).text());

        if (prevSeq <= OrgSeq && OrgSeq <= nextSeq) {
            newValue = parseInt(prevSeq + (nextSeq - prevSeq) / 2, 10);
            $(row).find(".sequence").text(newValue);
            $(row.cells[1]).css("background-color", "lightblue");
            $(row.cells[9]).removeClass("seqChanged");
        }
        else {
            newValue = parseInt(prevSeq + (nextSeq - prevSeq) / 2, 10);
            $(row).find(".sequence").text(newValue);
            $(row.cells[1]).css("background-color", "red");
            $(row.cells[9]).addClass("seqChanged");
        }
        ChangeSequence(orderId, newValue);
    }
    function ChangeSequence(orderId, seq) {
        $.ajax({
            url: "/PRD/Schedule/ChangeSequence?orderId=" + orderId + "&seq=" + seq,
            type: "GET",
            dataType: "json",
            success: function (actions) {
            }
        });
    }
    function GetSnapshots() {
        var s;
        $.ajax({
            url: "/PRD/Schedule/GetSnapshots",
            type: "GET",
            dataType: "json",
            async: false,
            success: function (snapshots) {
                s = snapshots;
            }
        });
        return s;
    }

}