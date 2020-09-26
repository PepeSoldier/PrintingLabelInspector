
function Analyze() {

    var self = this;

    this.Refresh = function() {
        var values = {};
        $.each($("#FormAnalyzePSI").serializeArray(), 
                function (i, field) { 
                    values[field.name] = field.value; 
                }
        );
        
        var db = GetJsonData(values["SelectedDate"], values["SelectedLine"], values["SelectedShift"]);
        self.RefreshData(db);

        $("#ResultP1").text(db.DSA);
        $("#ResultP2").text(db.SeqCnt);
        $("#ResultP3").text(db.SeqSum);
        $("#ResultPSI").text(db.PSI);
    }

    this.RefreshData = function(db) {
        $("#jsGrid").jsGrid({
            width: "100%",
            height: "600px",

            inserting: false,
            editing: false,
            sorting: false,
            paging: false,
            filtering: false,

            data: db.OrdersArch,

            fields: [
                { name: "StartDate", type: "text", title: "Start", width: 30, cellRenderer: FormatDate },
                { name: "EndDate", type: "text", title: "Koniec", width: 30, cellRenderer: FormatDate },
                { name: "OrderNo", type: "text", width: 50 },
                { name: "PNC", type: "text", width: 45 },
                { name: "QtyPlanned", type: "text", title: "Szt", width: 30 },
                { name: "W", type: "text", width: 20, cellRenderer: AddCellStyle },
                { name: "Reason.Name", type: "text", title: "Powód", width: 100, cellRenderer: RenderReason  },
                { name: "Id", type: "text", width: 30, validate: "required", css: "hidden orderId" },
                { name: "S", type: "text", title: "s", width: 15, cellRenderer: RenderCheckBox, css: "cbCell" }
                //{ name: "Line", type: "text", width: 25 }
            ]
        });
        $("#jsGrid2").jsGrid({
            width: "100%",
            height: "600px",

            inserting: false,
            editing: false,
            sorting: false,
            paging: false,
            filtering: false,

            data: db.Orders,

            fields: [
                { name: "StartDate", type: "text", title: "Start", width: 30, cellRenderer: FormatDate },
                { name: "EndDate", type: "text", title: "Koniec", width: 30, cellRenderer: FormatDate },
                { name: "OrderNo", type: "text", width: 50 },
                { name: "PNC", type: "text", width: 45 },
                { name: "QtyPlanned", type: "text", title: "Szt", width: 30 },
                { name: "Seq", type: "text", width: 20, cellRenderer: AddCellStyle },
                { name: "W", type: "text", width: 20, cellRenderer: AddCellStyle },
                { name: "ProdSeq", type: "text", width: 20, cellRenderer: AddCellStyleSeq },
                { name: "Reason.Name", type: "text", title: "Powód", width: 100, cellRenderer: RenderReason },
                { name: "Id", type: "text", width: 30, validate: "required", css: "hidden orderId" },
                { name: "S", type: "checkbox", title: "s", width: 15, cellRenderer: RenderCheckBox, css: "cbCell"  }
                //{ name: "Line", type: "text", width: 25 }
            ]
        });
    }

    function GetJsonData(date, line, shift) {
        return (function () {
            var json = null;
            $.ajax({
                'async': false,
                'global': false,
                'url': "/PRD/PSI/RefreshPlan?orderDate=" + date + "&shift=" + shift + "&line=" + line,
                'dataType': "json",
                'success': function (data) {
                    json = data;
                }
            });
            return json;
        })();
    }

    function AddCellStyle(value, item) {
        if (value == 0)
            return $("<td>").addClass("orderOK");
        else if (value == 1)
            return $("<td>").addClass("orderDeleted");
        else if (value == 2)
            return $("<td>").addClass("orderAdded");
        else if (value == 3)
            return $("<td>").addClass("orderSeqChanged");
        else if (value == 4)
            return $("<td>").addClass("orderSeqProd");
        else
            return $("<td>").append(value);
    };
    function AddCellStyleSeq(value, item) {
        if (value == 0)
            return $("<td>").addClass("orderOK").append(item.ProdSeqTmp);
        else if (value == 3)
            return $("<td>").addClass("orderSeqChanged").append(item.ProdSeqTmp);
        else if (value == 4)
            return $("<td>").addClass("orderSeqProd").append(item.ProdSeqTmp);
        else
            return $("<td>").append(item.ProdSeqTmp);
    };
    function FormatDate(value, item) {
        return $("<td>").append(moment(moment(value).toDate()).format("HH:mm"));
    }
    function RenderCheckBox() {
        return $("<td>").append('<input type="checkbox" class="cbSelectOrder">');
    }
    function RenderReason(value, item) {
        var name = item.Reason != null ? item.Reason.Name : "";
        var comment = "";
        comment = item.CommentText != null ? item.CommentText : comment;
        comment = item.CommentSupplier != null ? comment + "-" + item.CommentSupplier : comment;
        comment = item.CommentAnc != null ? comment + "-" + item.CommentAnc : comment;

        return $("<td>").append(
            '<div class="reasonCell" data-toggle="tooltip" data-placement="bottom" title="' + name + '">' + name +
            '</div><div class="commentCell" data-toggle="tooltip" data-placement="bottom" title="' + comment + '">' + comment + '</div>'
        );
    }
}