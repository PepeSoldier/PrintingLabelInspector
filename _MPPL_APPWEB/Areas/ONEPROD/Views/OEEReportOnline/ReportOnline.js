function ReportOnline(_workplaceId, _resourceId, _mesWorkplace) {
    var self_ = this;
    //var currentShiftStartTime;
    var dateFrom;
    var dateTo;
    var mesWorkplace = _mesWorkplace;
    var workplaceId = _workplaceId;
    var resourceId = _resourceId;
    var selectedStoppageId = 0;
    //var lastReadedRecordId = 0;
    var wnd;
    var interval = null;
    var lastRefresh = moment(new Date());
    var saveInProcess = false;

    var ReportData = {
        ReportId: 0,
        ProductionDate: 0,
        ItemId: 0,
        ProdQty: 0,
        CycleTime: 0,
        UsedTime: 0
    };

    this.Init = function (elementSelector) {
        Render(elementSelector).then(
            function () {
                Actions();
                self_.GetLabourBrigades();
                SignalR();
                dateFrom = CalcStartShiftTime(0);
                dateTo = CalcEndShiftTime(0);
                console.log(self_);
                self_.SetBtnShiftText();
                self_.Refresh(true);
                StartThread();
                console.log("Report Online init. " + dateFrom.format("YYYY-MM-DD HH:mm") + " - " + dateTo.format("YYYY-MM-DD HH:mm"));
            }
        );
    };
    function Render(elementSelector) {
        return new Promise((resolve, reject) => {
            var url = "/ONEPROD/OEEReportOnline/Index";
            let json = new JsonHelper().GetPostData(url, {
                resourceId,
                workplaceId
            });
            json.done(function (data) {
                $(elementSelector).html(data);
                resolve();
            });
        });
    }

    function StartThread() {
        self_.StopInterval();
        interval = setInterval(function () {
            //self_.Refresh();
        }, 10000);
    }
    this.StopInterval = function () {
        window.clearInterval(interval);
        interval = null;
    };

    this.SetShiftDate = function (dateStartShift, dateEndShift) {
        if (dateStartShift != null) { dateFrom = dateStartShift; }
        if (dateEndShift != null) { dateTo = dateEndShift; }
    };
    this.Refresh = function(force = false) { //function (dateStartShift, dateEndShift, force = false) {

        var currentTimestamp = moment(new Date());
        var duration = moment.duration(lastRefresh.diff(currentTimestamp));
        var elapsedTime = Math.abs(parseInt(duration.asSeconds()));
      
        if (elapsedTime >= 10 || force == true)
        {
            lastRefresh = new moment(new Date());

            self_.GetProductionLogs();
            self_.GetStoppages();
            self_.GetReport(); //(dateStartShift, dateEndShift);
        }
    };
    
    this.GetProductionLogs = function (pageIndex = 1) {
        $("#prodLogPageIndex").text(pageIndex);
        var url = "/ONEPROD/MES/GetProductionLogs";
        let json = new JsonHelper().GetPostData(url, {
            workplaceId,
            dateFrom: dateFrom.format("YYYY-MM-DD HH:mm"),
            dateTo: dateTo.format("YYYY-MM-DD HH:mm"),
            pageIndex,
            pageSize: 12
        });
        json.done(function (data) {
            let prodLogs = data.prodLogs;
            let prodLogSummary = data.prodLogSummary;

            $('.prodLogRow').filter(function (i) { return i > 0; }).remove();
            for (let i = 0; i < prodLogs.length; i++) {
                PutProductionLog(prodLogs[i]);
            }
            //if (prodLogs.length > 0) {
                PutProductionCounters(prodLogSummary);
            //}

        });
    };
    this.SetProductionReason = function (id, reasonId, reasonTypeId) {
        var url = "/ONEPROD/MES/SetProductionReason";
        //-1, 10, 11, 12
        let json = new JsonHelper().GetPostData(url,
            { id, reasonId, reasonTypeId });
        json.done(function (jsonData) {
            console.log("SetProductionReason.done");
            console.log(jsonData);
            mesWorkplace.MesWorkplaceWorkorder.RefreshConfirmedWorkorders(jsonData.data);
            //refresh displayed data
        });
    };
    function PutProductionLog(productionLog) {
        let template = $(".prodLogRow")[0].cloneNode(true);
        let iconClass = "";

        $(template).attr("data-Id", productionLog.Id);
        $(template).find(".TimeStamp").text(new moment(productionLog.TimeStamp).format('HH:mm:ss'));
        $(template).find(".ItemCode").text(productionLog.ItemCode);
        $(template).find(".Workorder").text(productionLog.ClientWorkOrderNumber);
        $(template).find(".DeclaredQty").text(productionLog.DeclaredQty);
        $(template).find(".SerialNo").text(pad(productionLog.SerialNo, 8));

        if (productionLog.ReasonName.length > 0) {
            $(template).find(".reasonTypeName").text(productionLog.ReasonTypeName);
            $(template).find(".reasonName").text(productionLog.ReasonName);
            $(template).find(".Workorder").addClass("ShiftTextUp");
            $(template).find(".ItemCode").addClass("ShiftTextUp");
        }
        
        //switch (productionLog.EntryType) {
        //    case 10: iconClass = "fas fa-check-circle"; break;
        //    case 11: iconClass = "fas fa-times-circle"; break;
        //    case 12: iconClass = "fas fa-times-circle"; break;
        //    default: iconClass = "fas fa-question-circle"; break;
        //}

        iconClass = GetEntryType(productionLog.ReasonTypeEntryType);

        $(template).find(".Status span").addClass(iconClass);
        $(template).removeClass("hidden");
        $("#prodLogRows").append($(template));
    }
    function PutProductionCounters(prodLogSummary) {
        console.log("prod counters refresh");
        var arr = [];
        arr.push({ Name: "Production", Value: 10, ProdQtyTotal: 0, UsedTime: 0 });
        arr.push({ Name: "ScrapMaterial", Value: 11, ProdQtyTotal: 0, UsedTime: 0 });
        arr.push({ Name: "ScrapProcess", Value: 12, ProdQtyTotal: 0, UsedTime: 0 });
        arr.push({ Name: "ScrapProcessScratch", Value: 13, ProdQtyTotal: 0, UsedTime: 0 });
        arr.push({ Name: "ScrapProcessDent", Value: 14, ProdQtyTotal: 0, UsedTime: 0 });
        arr.push({ Name: "ScrapProcessCrack", Value: 15, ProdQtyTotal: 0, UsedTime: 0 });
        arr.push({ Name: "ScrapProcessFold", Value: 16, ProdQtyTotal: 0, UsedTime: 0 });
        arr.push({ Name: "ScrapLabel", Value: 19, ProdQtyTotal: 0, UsedTime: 0 });

        //$("#prodEntryType-10").text(productionLog.ProdQtyTotalGood);
        $(".boxProduction .prodQtyVal div").text(0);
        $(".boxScrap .prodQtyVal div").text(0);
        //$(".boxPLC .prodQtyVal div").text(0);

        console.log("ReportOnline.PutProductionCounters");
        for (var i = 0; i < arr.length; i++) {
            $("#prodEntryType-" + arr[i].Value).addClass("prodQtyValZero");
            let val = arr[i].Value;
            let prodLogOfType = prodLogSummary.filter(x => x.EntryType == val); //)
            if (prodLogOfType !== null && prodLogOfType.length > 0) {
                let prodQty = prodLogOfType.map(v => v.Qty);
                arr[i].UsedTime = prodQty.reduce((a, b) => a + b);
                $("#prodEntryType-" + arr[i].Value).text(arr[i].UsedTime);
                if (arr[i].UsedTime > 0) {
                    $("#prodEntryType-" + arr[i].Value).removeClass("prodQtyValZero");
                }
            }
        }

        //$("#prodEntryType-11").text(productionLog.);
        //$("#prodEntryType-12").text(productionLog.ProdQtyTotalGood);
        //$("#prodEntryType-19").text(productionLog.ProdQtyTotalGood);
    }

    this.GetStoppages = function () {
        var url = "/ONEPROD/OEEReportOnline/GetStoppages";
        let json = new JsonHelper().GetPostData(url,
            {
                dateFrom: dateFrom.format("YYYY-MM-DD HH:mm"),
                dateTo: dateTo.format("YYYY-MM-DD HH:mm"),
                resourceId
            });
        json.done(function (stoppages) {
            $('.stoppageRow').filter(function (i) { return i > 0; }).remove();
            for (let i = 0; i < stoppages.length; i++) {
                PutStoppageLog(stoppages[i]);
            }
            HighlightSelectedStoppage();
            PutStoppageCounters(stoppages);
        });
    };
    this.GetStoppage = function (stoppageId, callback) {
        var url = "/ONEPROD/OEEReportOnline/GetStoppages";
        let json = new JsonHelper().GetPostData(url,
            {
                dateFrom: dateFrom.format("YYYY-MM-DD HH:mm"),
                dateTo: dateTo.format("YYYY-MM-DD HH:mm"),
                resourceId,
                stoppageId
            });
        json.done(function (stoppageLog) {
            callback(stoppageLog);
        });
    };
    this.SetStoppageReason = function () {
        var url = "/ONEPROD/OEEReportOnline/SetStoppageReason";
        let json = new JsonHelper().GetPostData(url,
            { id, reasonId });
        json.done(function (result) {
            console.log(result);
            if (result == -1) {
                new Alert().Show("danger", "Nie możesz zmienić powodu unieważnionej etykiety");
            }
            else if (result == 1) {
                new Alert().Show("info", "Unieważniono etykietę. Deklaracja została cofnięta");
                if (mesWorkplace != null) {
                    mesWorkplace.MesWorkplaceWorkorder.RefreshConfirmedWorkorders(data.updatedWorkordersIds);
                }
            }
            //refresh displayed data
        });
    };
    function PutStoppageLog(stoppageLog) {
        let template = $(".stoppageRow")[0].cloneNode(true);

        $(template).attr("data-Id", stoppageLog.Id);
        $(template).find(".TimeStamp").text(new moment(stoppageLog.ProductionDate).format('HH:mm:ss'));
        $(template).find(".LastUpdate").text(new moment(stoppageLog.TimeStamp).format('YYYY-MM-DD HH:mm:ss'));
        //$(template).find(".ReasonTypeId").attr("style","line-height: 42px;").addClass(GetEntryType(stoppageLog.ReasonTypeId));
        $(template).find(".ReasonTypeId").text(stoppageLog.ReasonTypeId);
        $(template).find(".ReasonTypeId").addClass("boxStop_" + stoppageLog.ReasonTypeId);
        $(template).find(".ResonName").text(stoppageLog.ResonName);
        $(template).find(".UsedTime").text(FormatTime(stoppageLog.UsedTime));
        $(template).removeClass("hidden");

        $("#stoppageRows").append($(template));
    }
    function PutStoppageCounters(stoppages) {
    
        console.log("ReportOnline.PutStoppageCounters");
        $(".boxStop .prodQtyVal div").addClass("prodQtyValZero");
        $(".boxStop .prodInfoHeader").addClass("prodQtyValZero");
        $(".boxStop .prodQtyVal div").text(FormatTime(0));
        
        $(".boxStop").each(function (i) {
            var reasonTypeId = parseInt($(this).attr("data-reasonTypeId"));
            let stoppageOfType = stoppages.filter(x => x.ReasonTypeId == reasonTypeId);

            if (stoppageOfType !== null && stoppageOfType.length > 0) {
                let stoppageTime = stoppageOfType.map(v => v.UsedTime);
                let totalUsedTime = stoppageTime.reduce((a, b) => a + b);
                $("#prodEntryType-" + reasonTypeId).text(FormatTime(totalUsedTime));
                if (totalUsedTime > 0) {
                    $("#prodEntryType-" + reasonTypeId).removeClass("prodQtyValZero");
                    $(".boxStop_" + reasonTypeId + " .prodInfoHeader").removeClass("prodQtyValZero");
                }
            }
        });
    }
    function PutPLCCounter(plcCounter)
    {
        $("#plcCounter").text(plcCounter);
    }

    this.GetReport = function () {
        console.log("ReportOnline.GetReport");
        
        var url = "/ONEPROD/OEEReportOnline/GetReportOnline";
        let json = new JsonHelper().GetPostData(url,
        {
            dateFrom: dateFrom.format("YYYY-MM-DD HH:mm"),
            dateTo: dateTo.format("YYYY-MM-DD HH:mm"),
            resourceId,
            workplaceId
        });
        json.done(function (data) {
            var prodUsedTime = 0;
            $('.reportRow').filter(function (i) { return i > 0; }).remove();
            for (let i = 0; i < data.productionLogs.length; i++) {
                PutReportProductionData(data.productionLogs[i]);
                prodUsedTime += data.productionLogs[i].UsedTime;
            }
            PutReportSummaryRow("Produkcja", prodUsedTime);

            var stoppagesUsedTime = 0;
            for (let i = 0; i < data.stoppageSummary.length; i++) {
                PutReportStoppageData(data.stoppageSummary[i]);
                stoppagesUsedTime += data.stoppageSummary[i].UsedTime;
            }
            PutReportSummaryRow("Zatrzymania", stoppagesUsedTime);
            PutReportSummaryRow("Łącznie", prodUsedTime + stoppagesUsedTime);
            PutPLCCounter(data.countedByPLC);
        });
    };
    function PutReportProductionData(productionLog) {
        console.log("ReportOnline.PutReportProductionData");
        let template = $(".reportRow")[0].cloneNode(true);

        $(template).find(".ItemId").text(productionLog.ItemId);
        $(template).find(".ReasonId").text(productionLog.ReasonId);
        $(template).find(".CycleTime").text(0);
        $(template).find(".ProductionDate").text(new moment().format('HH:mm:ss')); //new moment(productionLog.ProductionDate).format('HH:mm:ss'));
        $(template).find(".Description").text(productionLog.ItemCode);
        $(template).find(".ProdQty").text(productionLog.ProdQty);
        $(template).find(".ReportedProdQty").text(productionLog.ReportedProdQty);
        $(template).find(".UsedTime").text(FormatTime(productionLog.UsedTime));
        $(template).find(".ReasonTypeId").attr("style","line-height: 42px;").addClass(GetEntryType(productionLog.ReasonTypeId));
        $(template).find(".Status").text();
        $(template).removeClass("hidden");

        $("#reportRows").append($(template));
    }
    function PutReportStoppageData(stoppageSummary) {
        let template = $(".reportRow")[0].cloneNode(true);

        $(template).find(".ItemId").text();
        $(template).find(".ReasonId").text(stoppageSummary.ReasonId);
        $(template).find(".CycleTime").text(0);
        $(template).find(".ProductionDate").text(new moment().format('HH:mm:ss')); //new moment(productionLog.ProductionDate).format('HH:mm:ss'));
        $(template).find(".Description").text(stoppageSummary.ReasonName);
        $(template).find(".Description").addClass("ReasonName");
        
        //$($(template).find(".Description")).css("font-size", "14px;");
        $(template).find(".ProdQty").text();
        $(template).find(".UsedTime").text(FormatTime(stoppageSummary.UsedTime));
        $(template).find(".ReasonTypeId").text(stoppageSummary.ReasonTypeId);
        $(template).find(".ReasonTypeId").addClass("boxStop_" + stoppageSummary.ReasonTypeId);
        $(template).find(".Status").text();
        $(template).removeClass("hidden");

        $("#reportRows").append($(template));
    }   
    function PutReportSummaryRow(title, totalTime) {
        let template = $(".reportRow")[0].cloneNode(true);
        $(template).find(".Description").text(title);
        $(template).find(".ReportedProdQty").text("suma:");
        $(template).find(".UsedTime").text(FormatTime(totalTime));
        $(template).removeClass("hidden");
        $(template).addClass("reportSummaryRow");
        $("#reportRows").append($(template));
    }

    function FormatTime(totalSeconds) {

        let hours = 0;
        let minutes = 0;
        let seconds = 0;

        if (totalSeconds != null && totalSeconds > 0) {
            hours = parseInt(totalSeconds / 3600);
            minutes = parseInt(totalSeconds % 3600 / 60);
            seconds = parseInt(totalSeconds % 60);
        }
        
        return Format00(hours) + ":" + Format00(minutes) + ":" + Format00(seconds);
    }
    function Format00(digit) {
        if (digit < 10) {
            return "0" + digit.toString();
        }
        else {
            return digit.toString();
        }
    }
    function pad(num, size) {
        var s = num + "";
        while (s.length < size) s = "0" + s;
        return s;
    }

    this.Save = function (withLogout = false)
    {
        console.log("ReportOnline.Save");

        if (saveInProcess == true) return;

        saveInProcess = true;
        DisableElement("#btnSaveReportOnline");
        var url = "/ONEPROD/OEEReportOnline/SaveReport";
        var brigadeId = $("#brigadeList").val();
        let json = new JsonHelper().GetPostData(url,
            {
                resourceId,
                workplaceId,
                brigadeId,
                dateFrom: dateFrom.format("YYYY-MM-DD HH:mm"),
                dateTo: dateTo.format("YYYY-MM-DD HH:mm")
            });
        json.done(function (data) {
            saveInProcess = false;
            EnableElement("#btnSaveReportOnline");
            new Alert().Show("success", "Raport został zapisany");

            if (withLogout == true) {
                console.log("ReportOnline.Save.Logout");
                document.getElementById('logoutFormOneprodMes').submit();
            } else {
                self_.Refresh(true);
            }
        });
        json.fail(function () {
            saveInProcess = false;
            EnableElement("#btnSaveReportOnline");
            new Alert().Show("danger", "Zapis raportu nie powiódł się");
        });
    };

    function CalcStartShiftTime(shift_relative) {
        var today = new Date();
        var hour = today.getHours();
        today.setHours(hour + 2);
        hour = today.getHours();
        var shiftEndHour = hour < 8 ? 6 : hour < 16 ? 14 : 22;

        today.setSeconds(0); today.setMinutes(0); today.setMilliseconds(0);
        today.setHours(shiftEndHour - 8 * (shift_relative + 1));
        return new moment(today);
    }
    function CalcEndShiftTime(shift_relative) {
        var today = new Date();
        var hour = today.getHours();
        today.setHours(hour + 2);
        hour = today.getHours();
        var shiftEndHour = hour < 8 ? 6 : hour < 16 ? 14 : 22;

        today.setSeconds(0); today.setMinutes(0); today.setMilliseconds(0);
        today.setHours(shiftEndHour - 8 * shift_relative);
        return new moment(today);
    }
    this.SetBtnShiftText = function () {
        //$(".btnShift").each(function () {
        //    var minusHours = parseInt($(this).attr("data-minusHours"));
        //    var startShiftTime = self_.GetCurrentShiftStartTime(minusHours);
        //    var endShiftTime = self_.GetCurrentShiftStartTime(minusHours + 8);
        //    $(this).text(startShiftTime.format("DD/MM HH:mm") + "-" + endShiftTime.format("HH:mm"));
        //});
        let minusHours = parseInt($("#labelShift").attr("data-minusHours"));
        let startShiftTime = self_.GetCurrentShiftStartTime(minusHours);
        let endShiftTime = self_.GetCurrentShiftStartTime(minusHours + 8);

        dateFrom = startShiftTime;
        dateTo = endShiftTime;
        
        $("#labelShift").text(startShiftTime.format("DD/MM HH:mm") + "-" + endShiftTime.format("HH:mm"));
    };

    this.GetCurrentShiftStartTime = function (minusHours) {
        var stShiftTime = CalcStartShiftTime(0);
        if(minusHours > 0)
            return stShiftTime.add(minusHours, 'hours');
        else
            return stShiftTime.subtract(Math.abs(minusHours), 'hours');
    };
    this.ShowReasonSelectorWindow = function (isStoppage, entryId, splitAllowed) {

        selectedStoppageId = entryId;
        HighlightSelectedStoppage();

        try {
            if (wnd !== null)
                wnd.Close();
        }
        catch{
            console.log("catch exception");
        }

        wnd = new PopupWindow(1660, 800, 143, 8);
        wnd.Init("OeeReasonWindow", "Wybierz powód");
        wnd.AddClass("OeeReasonWindow");
        wnd.Show("loading...");

        $.get("/ONEPROD/OEEReportOnline/ReportOnlineReason", function (data) {
            wnd.Show(data);
            var reasonSelector = new OEEReasonSelector2("#Reasons1", resourceId, SelectReasonCallback, splitAllowed);
            reasonSelector.ShowReasons(isStoppage, entryId);
        });
    };
    function SelectReasonCallback(entryId, reasonTypeId, reasonId) {
        console.log("SelectReasonCallback(entryId, entryType, reasonId) " + entryId + ", " + reasonTypeId + ", " + reasonId);
        reasonTypeId = Math.abs(reasonTypeId);
        reasonTypeId = reasonTypeId == 0 || reasonTypeId == 1 ? Math.abs(reasonId) : reasonTypeId;
        reasonId = reasonId > 0 ? reasonId : null;

        // < 20 are only production reasons, 2 is exception for no planned close of machine
        if (reasonTypeId < 20 && reasonTypeId != 2) {
            $.post(
                "/ONEPROD/MES/SetProductionReason",
                { id: entryId, reasonId: reasonId, reasonTypeId: reasonTypeId },
                function (jsonData) {
                    wnd.Close();
                    self_.Refresh(true);
                    if (jsonData.result == -1)
                        new Alert().Show("warning", "Nie można zmienić powodu unieważnionej deklaracji");
                    else
                        mesWorkplace.MesWorkplaceWorkorder.RefreshConfirmedWorkorders(jsonData.data);
                }
            );
        } else {
            $.post(
                "/ONEPROD/OEEReportOnline/SetStoppageReason",
                { id: entryId, reasonId: reasonId, reasonTypeId: reasonTypeId},
                function (data) {
                    wnd.Close();
                    self_.Refresh(true);
                }
            );
        }
        
    }
    function GetEntryType(entryType) {
        console.log("Entry type:" + entryType);
        switch (entryType) {
            case 10: iconClass = "fas fa-check-circle"; break;
            case 11: iconClass = "fas fa-times-circle"; break;
            case 12: iconClass = "fas fa-times-circle"; break;
            case 19: iconClass = "far fa-file-alt"; break;
            case entryType >= 20: iconClass = "fas fa-stop-circle"; break;
            default: iconClass = "fas fa-question-circle"; break;
        }
        return iconClass;
    }

    function HighlightSelectedStoppage() {
        $(".stoppageRow").removeClass("selectedRow");
        $(".stoppageRow[data-id=" + selectedStoppageId + "]").addClass("selectedRow");
    }

    this.ShowStopSplitWindow = function (stoppageId) {

        try {
            if (wnd !== null)
                wnd.Close();
        }
        catch{
            console.log("catch exception");
        }

        wnd = new PopupWindow(935, 800, 143, 8);
        wnd.Init("OeeStopSplitWindow", "Podziel postój");
        //wnd.AddClass("");
        wnd.Show("loading...");

        $.get("/ONEPROD/OEEReportOnline/StopSplit?stoppageId=" + stoppageId, function (data) {
            wnd.Show(data);
            var stopSplit = new StopSplit(stoppageId, StopSplitCallback);
            stopSplit.Init();
        });
    };
    function StopSplitCallback() {
        console.log("StopSplitCallback");
        wnd.Close();
        self_.Refresh(true);
    }

    this.ShowChangeDeclarationDateWindow = function (prodLogId) {

        try {
            if (wnd !== null)
                wnd.Close();
        }
        catch{
            console.log("catch exception");
        }

        wnd = new PopupWindow(935, 800, 143, 8);
        wnd.Init("ChangeDeclarationDateWindow", "Zmień datę deklaracji");
        //wnd.AddClass("");
        wnd.Show("loading...");

        $.get("/ONEPROD/OEEReportOnline/ChangeDeclarationDate?productionLogId=" + prodLogId, function (data) {
            wnd.Show(data);
            var changeDeclarationDate = new ChangeDeclarationDate(prodLogId, ChangeDeclarationDateCallback);
            changeDeclarationDate.Init();
        });
    };
    function ChangeDeclarationDateCallback() {
        console.log("StopSplitCallback");
        wnd.Close();
        self_.Refresh(true);
    }
    
    this.GetLabourBrigades = function () {
        var url = "/ONEPROD/MES/GetLabourBrigades";
        let json = new JsonHelper().GetPostData(url, null);
        json.done(function (data) {
            $('#brigadeList').html(""); 
            $.each(data, function (key, value) {
                $('#brigadeList')
                    .append($("<option></option>")
                        .attr("value", value.Id)
                        .text(value.Name));
            });
        });
    };

    function SignalR() {
        console.log("preparing the hub");
        jlR = $.connection.workplaceHub;
        jlR.client.broadcastMessage = function (msg) {
            console.log("signalR");
            console.log(msg);

            if (msg == "RefreshProdLogs") {
                self_.Refresh(true);
            }
        };

        $.connection.hub.start().done(function () {
            console.log("Hub is started.");
            jlR.server.joinWorkplace("workplace_" + workplaceId);
        });
        $.connection.hub.disconnected(function () {
            setTimeout(function () {
                $.connection.hub.start();
            }, 5000); // Restart connection after 5 seconds.
        });
    }

    function Actions() {
        $(document).off("click", ".btnShift");
        $(document).on("click", ".btnShift", function () {
            let minusHours = parseInt($(this).attr("data-minusHours"));
            let minusHoursTotal = Math.min(parseInt($("#labelShift").attr("data-minusHours")) + minusHours, 0);
            $("#labelShift").attr("data-minusHours", minusHoursTotal);
            self_.SetBtnShiftText();
            self_.SetShiftDate(self_.GetCurrentShiftStartTime(minusHoursTotal), self_.GetCurrentShiftStartTime(minusHoursTotal + 8));
            self_.Refresh(true);
        });
        $(document).off("click", "#btnSaveReportOnline");
        $(document).on("click", "#btnSaveReportOnline", function () {
            self_.Save(false);
        });
        $(document).off("click", ".prodLogRow");
        $(document).on("click", ".prodLogRow", function () {
            console.log("prodLogRow click");
            var entryId = $(this).attr("data-Id");
            self_.ShowReasonSelectorWindow(0, entryId);
        });
        $(document).off("click", ".stoppageRow");
        $(document).on("click", ".stoppageRow", function () {
            console.log("stoppageRow click");
            var entryId = $(this).attr("data-Id");

            self_.GetStoppage(entryId, function (stoppageLog) {
                console.log("GetStopage callback");
                //var entryLastUpdate = new moment($(this).find(".LastUpdate").text());
                var entryLastUpdate = new moment(stoppageLog[0].TimeStamp).format('YYYY-MM-DD HH:mm:ss');
                var nowMoment = moment(new Date());
                var dur = moment.duration(nowMoment.diff(entryLastUpdate));
                var minutes = parseInt(dur.asMinutes());

                self_.ShowReasonSelectorWindow(1, entryId, (minutes >= 1 ? true : false));
            });
        });
        $(document).off("click", ".btnChangePage");
        $(document).on("click", ".btnChangePage", function () {
            let val = parseInt($(this).attr("data-Value"));
            let pageIndex = Math.max(parseInt($("#prodLogPageIndex").text()) + val, 1);
            self_.GetProductionLogs(pageIndex);
        });
        $(document).off("click", ".btnReportOnline");
        $(document).on("click", ".btnReportOnline", function () {
            $(".btnReportOnline").removeClass("selectedRow");
            $(this).addClass("selectedRow");
        });
        $(document).off("click", "#btnProductionLogs");
        $(document).on("click", "#btnProductionLogs", function () {
            $(".reportContent").addClass("hidden");
            $("#prodLogRows").removeClass("hidden");
            self_.Refresh();
        });
        $(document).off("click", "#btnStoppages");
        $(document).on("click", "#btnStoppages", function () {
            $(".reportContent").addClass("hidden");
            $("#stoppageRows").removeClass("hidden");
            self_.Refresh();
        });
        $(document).off("click", "#btnReport");
        $(document).on("click", "#btnReport", function () {
            self_.SetBtnShiftText();
            $(".reportContent").addClass("hidden");
            $("#reportSummaryRows").removeClass("hidden");
            self_.Refresh();
        });
        $(document).off("click", "#btnSplit");
        $(document).on("click", "#btnSplit", function () {
            let stoppageId = $(this).attr("data-Id");
            self_.ShowStopSplitWindow(stoppageId);
        });
        $(document).off("click", "#btnSplitDisabled");
        $(document).on("click", "#btnSplitDisabled", function () {
            new Alert().Show("danger", "Podział postoju jest możliwy dopiero po zakończeniu naliczania czasu");
        });
        $(document).off("click", "#btnChangeDeclarationDate");
        $(document).on("click", "#btnChangeDeclarationDate", function () {
            let prodLogId = $(this).attr("data-Id");
            self_.ShowChangeDeclarationDateWindow(prodLogId);
        });
    }
}