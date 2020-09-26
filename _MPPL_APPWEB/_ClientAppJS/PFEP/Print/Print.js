function DefPrint() {
    var self = this;
    var selectedRoutineH = 4;
    var selectedRoutineId = 1;
    var timeRef = "";
    var spinnerVal = 1;
    
    function GetStartTime() {
        var result = "";
        $.ajax({
            url: "/Print/GetStartTime/?routine=" + selectedRoutineH,
            async: false,
            success: function (data) {
                result = data;
            },
            error: function (xhr, ajaxOptions, thrownError) {
                new Alert().Show("Danger", thrownError);
            }
        });
        console.log(result);
        return result;
    }
    function UpdateSelectedTime() {
        //console.log(timeRef);
        //console.log(selectedRoutineH);
        //console.log(spinnerVal);

        $.post('/PFEP/Print/GetCalculatedTimeRange',
            { refTime: timeRef, selectedRoutine: selectedRoutineH, spinnerVal: spinnerVal },
            function (date) {
                $('#RoutineSelectedTime1').val(date[0]);
                $('#RoutineSelectedTime2').val(date[1]);
                self.SelectRows(date[0], date[1]);
            }
        ).fail(function (response) {
            new Alert().Show("danger", "Nie można przeliczyć zakresu godzin. Prawdopodobnie wystąpił problem z połączeniem z serwerem. " + response.responseText);
        });
    }

    this.SelectRows = function(dateFrom, dateTo) {

        var currentDate = new Date(timeRef);
        var dateFrom1 = new Date(dateFrom);
        var dateTo1 = new Date(dateTo);
        
        $("#ProductionPlan20").find(".OrderDateTime").each(function (index) {
            currentDate = new Date($(this).text());
            if (dateFrom1 <= currentDate && currentDate < dateTo1) {
                $(this).parent().addClass('RowSelected');
            }
            else {
                $(this).parent().removeClass('RowSelected');
            }
        });

        SelectFullOrders();
        ScrollPlan20();
    }
    this.SetSelectedRoutine = function (routineTime, routineId) {
        selectedRoutineH = routineTime;
        selectedRoutineId = routineId;
        spinnerVal = 1;
        UpdateSelectedTime();
    }
    this.GetSelectedRoutine = function () {
        return selectedRoutineId;
    }
    this.IncreaseSpinnerVal = function () {
        spinnerVal++;
        UpdateSelectedTime();
    }
    this.DecreaseSpinnerVal = function () {
        spinnerVal--;
        UpdateSelectedTime();
    }
    this.UpdateReferenceTime = function(refTime) {
        timeRef = refTime;
        spinnerVal = 1;
    }

    this.Print = function (printer) {
        var tab = [];
        var counter = 0;
        $("#ProductionPlan20").find(".RowSelected").each(function (index) {
            tab[counter] = $(this)["0"].cells["0"].innerHTML
            counter++;
        });

        $.ajax({
            async: false,
            type: "POST",
            url: "/Print/SendToPrinter",
            dataType: "json",
            data: { routine: selectedRoutineId, printerName: printer, ProdOrders: tab },
        });
    }
    this.SavePDF = function (printer) {
        var tab = [];
        var counter = 0;
        $("#ProductionPlan20").find(".RowSelected").each(function (index) {
            tab[counter] = $(this)["0"].cells["0"].innerHTML
            counter++;
        });
        
        $.ajax({
            async: false,
            type: "POST",
            url: "/PFEP/Print/SavePDF",
            //dataType: "json",
            data: { routine: selectedRoutineId, printerName: printer, ProdOrders: tab },
            success: function (url) {
                var link = document.createElement("a");
                link.download = url[1];
                link.href = new BaseUrl().link + url[0] + "/" + url[1];
                link.click();
            }
        });
    }
    this.ShowPDF = function (printer) {
        var tab = [];
        var sTab = "";
        var counter = 0;
        $("#ProductionPlan20").find(".RowSelected").each(function (index) {
            tab[counter] = $(this)["0"].cells["0"].innerHTML;

            if (counter > 0)
                { sTab += ","; }
            sTab += $(this)["0"].cells["0"].innerHTML;

            counter++;
        });

        var win = window.open("/PFEP/Print/PrintView?routine=" + selectedRoutineId + "&sOrders=" + sTab, "Podglad" + new Date());
                
        //$.ajax({
        //    async: false,
        //    type: "POST",
        //    url: "/PFEP/Print/ViewPDF",
        //    //dataType: "json",
        //    data: { routine: selectedRoutineId, printerName: printer, ProdOrders: tab },
        //    success: function (data) {
        //        console.log("success ");
        //        var win = window.open("/PFEP/Print/PrintView", "Podglad" + new Date());
                
        //        $(win.document).ready(function () {
                    
        //            setTimeout(function () {
        //                console.log(new Date() + " wait...");
        //            }, 2000);
                    
        //            win.document.body.innerHTML = data;
        //            eval(win.document.getElementById("runscript").innerHTML);
        //        });
        //    },
        //    error: function() {
        //        console.log("error ");
        //    }
            
        //});
    }

    function ScrollPlan20(){
        var rowSelected = $('#ProductionPlan20 .RowSelected:first')[0];
        if (rowSelected != null) {
            var rowpos = rowSelected != null ? $('#ProductionPlan20 .RowSelected:first')[0].offsetTop : 0;
            $('#ProductionPlan20 .jsgrid-grid-body')[0].scrollTop = (rowpos >= 42) ? rowpos - 42 : rowpos;
        }
    }
    function ScrollPlanOrder() {
        var rowSelectedFirst = $('#ProductionPlanOrder .RowSelected:first')[0];

        if (rowSelectedFirst != null) {
            var firstRowOffset = $('#ProductionPlanOrder .RowSelected:first')[0].offsetTop;
            var lastRowOffset = $('#ProductionPlanOrder .RowSelected:last')[0].offsetTop + 35;
            var selectionHeight = lastRowOffset - firstRowOffset;
            var gridHeight = $(".jsgrid-grid-body").height()
            var currentScroll = $('#ProductionPlanOrder .jsgrid-grid-body')[0].scrollTop;

            if (lastRowOffset > gridHeight + currentScroll) {
                $('#ProductionPlanOrder .jsgrid-grid-body')[0].scrollTop = lastRowOffset - gridHeight + 35;
            }
            if (firstRowOffset < currentScroll) {
                $('#ProductionPlanOrder .jsgrid-grid-body')[0].scrollTop = firstRowOffset - 35;
            }
        }
    }

    //GridJS Finctions
    this.DrawJsGrid = function () {
        //console.log("draw JS columns");
        $("#ProductionPlan20").jsGrid({
            width: "100%", height: "900px",
            inserting: false, editing: false, sorting: false, paging: false, filtering: true,
            fields: [
                { name: "Id", type: "text", title: "Id", width: 30, filtering: false, css: "hidden" },
                { name: "StartDate", type: "text", title: "Data", width: 40, css: "OrderDateTime", filterTemplate: function () { return ""; }, filterValue: FilterDateVal, cellRenderer: FormatDate },
                //{ name: "LineName", type: "text", title: "Linia", width: 25 },
                //{ name: "LineName", type: "text", title: "Linia", width: 25, filterTemplate: FilterLine, filterValue: FilterLineVal, css: "hidden"},
                { name: "OrderNo", type: "text", title: "Nr zlecenia", width: 40, css: "OrderNo", filterTemplate: function () { return ""; }, filterValue: FilterOrderNoVal },
                //{ name: "PNCCode", type: "text", title: "PNC", width: 45 },
                { name: "Qty", type: "text", title: "Szt", width: 15, filtering: false },
                //{ name: "QtyOrder", type: "text", title: "Szt zl.", width: 15, filtering: false },
                { name: "Printed", type: "text", title: "Wydruk?", width: 30, filtering: false, cellRenderer: SetPrintStatus }
            ],
            controller: {
                loadData: function (filter) {
                    console.log("JsGrid: load data (20)");
                    filter.RoutineId = self.GetSelectedRoutine();
                    filter.PNCCode = FilterPNCVal();
                    filter.LineName = FilterLineVal();
                    return self.GetPrintHistory(filter);
                },
            },
            onDataLoaded: function () {
                UpdateSelectedTime();
            },
            rowClick: function (args) {
                var $row = this.rowByItem(args.item);
                SelectRow20OnClick($row);
            }
        });

        $("#ProductionPlanOrder").jsGrid({
            width: "100%", height: "900px",
            inserting: false, editing: false, sorting: false, paging: false, filtering: true,
            fields: [
                { name: "Id", type: "text", title: "Id", width: 30, filtering: false, css: "hidden" },
                { name: "StartDate", type: "text", title: "Data", width: 40, css: "OrderDateTime", filterTemplate: FilterDate, filterValue: FilterDateVal, cellRenderer: FormatDate },
                { name: "LineName", type: "text", title: "Linia", width: 25, filterTemplate: FilterLine, filterValue: FilterLineVal },
                { name: "OrderNo", type: "text", title: "Nr zlecenia", width: 50, css: "OrderNo", filterTemplate: FilterOrderNo, filterValue: FilterOrderNoVal },
                { name: "PNCCode", type: "text", title: "PNC", width: 45, filterTemplate: FilterPNC, filterValue: FilterPNCVal },
                { name: "QtyOrder", type: "text", title: "Szt P", width: 15, filtering: false },
                { name: "Qty", type: "text", title: "Szt R", width: 15, filtering: false },
                { name: "QtyPrinted", type: "text", title: "Szt D", width: 15, filtering: false },
            ],
            controller: {
                loadData: function (filter) {
                    console.log("JsGrid: load data orders");
                    filter.RoutineId = self.GetSelectedRoutine();
                    $("#ProductionPlan20").jsGrid("search");
                    return self.GetPrintHistoryOrders(filter);
                },
            },
            onDataLoaded: function () {
                UpdateSelectedTime();
                new Alert().GetAlerts(new User());
            },
            rowClick: function (args) {
                var $row = this.rowByItem(args.item);
                SelectRowOrderOnClick($row);
                //var $row = this.rowByItem(args.item);
                //$row.toggleClass("RowSelected");
            }
        });
    }
    this.RefreshJsGrid = function () {
        $("#ProductionPlanOrder").jsGrid("search");
        return new $.Deferred();
    }
    this.GetPrintHistory = function (filter) {
        return (function () {
            var json = null;
            $.ajax({
                async: false, global: false, dataType: "json", type: "POST",
                url: "/PFEP/Print/GetPrintHistoryOrders20",
                data: filter,
                success: function (data) {
                    json = data;
                    //new Alert().Show("success", "Załadowano dane");
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    new Alert().Show("danger", thrownError);
                }
            });
            return json;
        })();
    }
    this.GetPrintHistoryOrders = function (filter) {
        return (function () {
            var json = null;
            $.ajax({
                async: false, global: false, dataType: "json", type: "POST",
                url: "/PFEP/Print/GetPrintHistoryOrders",
                data: filter,
                success: function (data) {
                    json = data;
                    new Alert().Show("success", "Załadowano dane");
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    new Alert().Show("danger", thrownError);
                }
            });
            return json;
        })();
    }
    this.RecalculateProdPlan = function (days) {
        var result = confirm('Na pewno chcesz przeliczyć plan od nowa? Obliczenie może potrwać dłużej niż 1min.');
        if (result) {
            new Alert().Show("warning", "Trwa przeliczanie... Poczekaj");

            $.ajax({
                async: false, global: false, dataType: "json", type: "POST",
                url: "/PFEP/Print/RecalculatePlan20?days=" + days,
                success: function () {
                    new Alert().Show("success", "Przeliczono");
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    new Alert().Show("danger", thrownError);
                }
            });
        }
    }

    function SelectRow20OnClick($row) {
        $row.toggleClass("RowSelected");
        SelectFullOrders();
    }
    function SelectRowOrderOnClick($row) {
        $row.toggleClass("RowSelected");
        Select20Orders();
    }
    function SelectFullOrders() {
        var selecterOrderNumbers = [];

        $("#ProductionPlan20").find(".RowSelected").each(function (index) {
            newItem = $(this).find(".OrderNo").text();
            if (selecterOrderNumbers.indexOf(newItem) === -1)
                selecterOrderNumbers.push(newItem);
        });

        $("#ProductionPlanOrder").find(".OrderNo").each(function (index) {
            currentOrderNo = $(this).text();
            if (selecterOrderNumbers.includes(currentOrderNo)) {
                $(this).parent().addClass('RowSelected');
            }
            else {
                $(this).parent().removeClass('RowSelected');
            }
        });
        
        ScrollPlanOrder();
        
    }
    function Select20Orders() {
        var selecterOrderNumbers = [];

        $("#ProductionPlanOrder").find(".RowSelected").each(function (index) {
            newItem = $(this).find(".OrderNo").text();
            if (selecterOrderNumbers.indexOf(newItem) === -1)
                selecterOrderNumbers.push(newItem);
        });

        $("#ProductionPlan20").find(".OrderNo").each(function (index) {
            currentOrderNo = $(this).text();
            if (selecterOrderNumbers.includes(currentOrderNo)) {
                $(this).parent().addClass('RowSelected');
            }
            else {
                $(this).parent().removeClass('RowSelected');
            }
        });

        ScrollPlan20();

    }
    function FormatDate(value, item) {
        return $("<td>").append(moment(moment(value).toDate()).format("YYYY-MM-DD HH:mm"));
    }
    function FilterDate() {
        return $("<input type='text' class='datetimepicker jsGridFilter' id='refTime'>");
    }
    function FilterDateVal() {
        return $("#refTime").val();
    }
    function FilterLine() {
        return $("<input type='text' class='jsGridFilter' id='filterLine'>");
    }
    function FilterLineVal() {
        return $("#filterLine").val();
    }
    function FilterOrderNo() {
        return $("<input type='text' class='jsGridFilter' id='filterOrderNo'>");
    }
    function FilterOrderNoVal() {
        return $("#filterOrderNo").val();
    }
    function FilterPNC() {
        return $("<input type='text' id='filterPNC'>");
    }
    function FilterPNCVal() {
        return $("#filterPNC").val();
    }
    function SetPrintStatus(value, item) {
        if (value == true)
            return $("<td>").append("OK");
        else
            return $("<td>").append("");
    }
}
