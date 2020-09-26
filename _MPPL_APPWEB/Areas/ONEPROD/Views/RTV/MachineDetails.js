function RTV_MachineDetails() {
    var rtv = new RTV();
    //var rtvOEE = new RTV_OEE();
    //var rtvProdData = new RTV_ProdData();
    //var rtvChart = new RTV_Chart();
    //var rtvConn = new RTV_Connections();

    var machineId = 0; 

    this.Init = function (_machineId) {
        ConsoleLog("RTV_MachineDetails INIT (" + _machineId + ")");
        machineId = _machineId;
        rtv.Init(machineId);
    };
}

function RTV(_machineId, _parentElementSelector, _fixedDigits, _dashboardMode) {

    var self = this;
    var rtvOEE = new RTV_OEE(this, _fixedDigits);
    var rtvProdData = new RTV_ProdData(this);
    var rtvChart = new RTV_Chart(this);
    var rtvConn = new RTV_Connections(this);
    var rtvPlan = new RTV_Plan(this);
    
    var interval = null;
    var machineId = 0;
    var dashboardMode = _dashboardMode;
    var shift = 1;
    var parentElementSelector = "";
    var shiftRelative = 0;
    var currentMinute = 0;
    var currentSec = 0;
    var lastRefreshedMinute = -1;
    var startShiftTime = moment(new Date()); //todays date
    var saveScreenStatus = 0; //0 default, 1 saving process, 2 saved
    var refreshStatus = 0; //0 available, 1 refreshing process

    this.GetShiftRelative = function () {
        return shiftRelative;
    };
    this.ModifyShiftRelative = function (value) {
        return shiftRelative += value;
    };
    this.SetLastRefreshedMinute = function (value) {
        lastRefreshedMinute = value;
    };
    this.GetParentrSelector = function () { return parentElementSelector; };

    this.Init = function () {
        ConsoleLog("--------------------------RTV INIT (" + _machineId + ")");
        machineId = _machineId;
        parentElementSelector = _parentElementSelector;
        shift = CalcShiftNumber();
        startShiftTime = moment(CalcStartShiftTime(shiftRelative));
        lastRefreshedMinute = -1;
        currentMinute = 0;

        $("#shiftDate").text(startShiftTime.format('YYYY-MM-DD HH:mm'));
        PrepareScreen();
        rtvProdData.PutRTVData(PrepareInitData());
        self.RefreshAll_FORCE();
        rtvProdData.LoadLastShiftData(1);

        rtvPlan.Refresh();
        StartThread();
    };
    function PrepareScreen() {
        //console.log("PrepareScreen");
        $(".chartHour").removeClass("rtvGreen");
        $(".chartHour").removeClass("rtvRed");
        $(".chartHour").text("");
        $(".chartMinute").removeClass("rtvRed");
        $(".chartMinute").removeClass("rtvGreen");
        $(".chartMinute").removeClass("rtvBlue");
        $(".chartMinute").removeClass("rtvYellow");
        $(".chartMinute").removeClass("rtvGray");
        $(".chartMinute").removeAttr("data-cycletime");
        $(".chartMinute").removeAttr("data-qty");
        $(".chartMinute").removeAttr("data-usedtime");
    }

    this.SaveScreenshot = function () {
        if (dashboardMode == false) {
            if (saveScreenStatus == 0) {
                saveScreenStatus = 1;
                html2canvas(document.body, {
                    onrendered: function (canvas) {
                        var base64image = canvas.toDataURL();
                        $.post("/ONEPROD/RTV/SaveScreenshot", { base64image, machineId, shift }, function () {
                            saveScreenStatus = 2;
                        });
                    }
                });
            }
        }
    };

    this.RefreshAll = function() {
        ConsoleLog("Refreshing ALL (" + machineId + ")");
        currentMinute = self.GetCurrentMinute();
        currentSec = self.GetCurrentSec();

        if (refreshStatus == 0) {
            refreshStatus = 1;
            if (currentMinute >= 479) {
                self.SaveScreenshot();
            }

            if (currentMinute >= 480) {
                console.log("current minute >= 480:" + currentMinute);
                if (saveScreenStatus >= 2) {
                    location.reload(true);
                }
            }

            ajax = self.GetAjaxData("/ONEPROD/RTV/GetRTVData", self.PrepareFilters(shiftRelative));
            ajax.done(function (data) {
                refreshStatus = 0;
                rtvConn.__CONNECTION_BLINKFAST(true);

                if (data.ProducedQty > 0) {
                    //console.log(self.GetParentrSelector());
                    $(self.GetParentrSelector()).removeClass("colorInactive");
                }

                if (data.ProducedQty > parseInt($(self.GetParentrSelector() + " #ProducedQty").text())) {
                    //ConsoleLog("Change of Qty Detected");
                    //rtvPlan.Refresh();
                    rtvProdData.PutRTVData(data);
                    rtvOEE.LoadOEEData();
                    rtvChart.RefreshMinutesChart(lastRefreshedMinute, currentMinute);
                    rtvProdData.RefreshTargetAndDelta(rtvChart.GetTargetForMinuteAndSec(currentMinute, currentSec));
                    rtvChart.RefreshChartHour();
                    rtvConn.__CONNECTION_BLINKFAST(false);
                }
                else if (currentMinute > lastRefreshedMinute) {
                    //ConsoleLog("Change of Minute Detected");
                    //rtvPlan.Refresh();
                    rtvProdData.PutRTVData(data);
                    rtvOEE.LoadOEEData();
                    rtvChart.RefreshRTVChart();
                    rtvChart.RefreshMinutesChart(lastRefreshedMinute, currentMinute);
                    rtvProdData.RefreshTargetAndDelta(rtvChart.GetTargetForMinuteAndSec(currentMinute, currentSec));
                    rtvConn.__CONNECTION_BLINKFAST(false);
                }
                else {
                    rtvProdData.PutRTVData(data);
                    rtvProdData.RefreshTargetAndDelta(rtvChart.GetTargetForMinuteAndSec(currentMinute, currentSec));
                    rtvChart.RefreshChartHour();
                    rtvConn.__CONNECTION_BLINKFAST(false);
                    //ConsoleLog(">>>>>>>>>NO-ANY-CHANGE-DETECTED<<<<<<<<<<");
                }
            });
            ajax.fail(function () {
                refreshStatus = 0;
                rtvConn.__CONNECTION_BLINKFAST(false);
            });
        }
        else {
            ConsoleLog("Refreshing refused due to not finished last request");
        }
        //ConsoleLog("---------------------------------------");
    }
    this.RefreshAll_FORCE = function() {
        ConsoleLog("Refreshing ALL FORCE (" + machineId + ")");
        currentMinute = self.GetCurrentMinute();

        rtvPlan.Refresh();

        rtvConn.__CONNECTION_BLINKFAST(true);
        ajax = self.GetAjaxData("/ONEPROD/RTV/GetRTVData", self.PrepareFilters(shiftRelative));
        ajax.done(function (data) {
            rtvProdData.PutRTVData(data);
            rtvOEE.LoadOEEData();
            rtvChart.RefreshRTVChart();
            rtvChart.RefreshMinutesChart(lastRefreshedMinute, currentMinute);
            rtvProdData.RefreshTargetAndDelta(rtvChart.GetTargetForMinute(currentMinute));
            rtvConn.__CONNECTION_BLINKFAST(false);
        });
        ajax.fail(function () {
            rtvConn.__CONNECTION_BLINKFAST(false);
        });

        //ConsoleLog("---------------------------------------");
    }
    this.PrepareFilters = function(shift_relative, save = false) {
        var filter = {};
        filter.endShiftTime = CalcEndShiftTime(shift_relative);
        filter.startShiftTime = CalcStartShiftTime(shift_relative);
        filter.dateFrom = filter.startShiftTime.format('YYYY-MM-DD HH:mm');
        filter.dateTo = filter.endShiftTime.format('YYYY-MM-DD HH:mm');
        filter.machineId = machineId;

        if (!save) {
            filter.endShiftTime = "";
            filter.startShiftTime = "";
        }

        return filter;
    }
    this.GetAjaxData = function (url, filters){
        rtvConn.__ConnectionAjax(2);
        filters.endShiftTime = null;
        filters.startShiftTime = null;

        //console.log("GET AJAX machine: " + filters.machineId);

        return $.ajax({
            async: true, type: "POST", data: filters,
            url: url,
            success: function (data) {
                rtvConn.__ConnectionAjax(1);
            },
            error: function (xhr, ajaxOptions, thrownError) {
                rtvConn.__ConnectionAjax(0);
            }
        });
    }
    this.GetCurrentSec = function () {
        var end1 = moment(new Date());
        let sec = parseInt(end1.format("ss"));
        return sec;
    };
    this.GetCurrentMinute = function () {
        var end1 = moment(new Date());
        var duration = moment.duration(end1.diff(startShiftTime));
        var minutes = parseInt(duration.asMinutes());
        return minutes > 480 ? 481 : minutes;
    };
    this.GetCurrentHour = function () {
        var end1 = moment(new Date());
        var duration = moment.duration(end1.diff(startShiftTime));
        var hours = parseInt(duration.asHours());
        return hours > 8 ? 8 : hours;
    }

    function StartThread() {
        self.StopInterval();
        interval = setInterval(function () {
            if (rtvConn.IsNotRefreshingState()) {
                $(self.GetParentrSelector() + " #Connection").animate({ "opacity": .2 }, 500, function () {
                    $(self.GetParentrSelector() + " #Connection").animate({ "opacity": 1 }, 250, function () {
                        console.log("Interval: " + interval);
                        self.RefreshAll();
                        rtvConn.CheckConnection();
                    });
                });
            }
        }, 5000);
    }
    this.StopInterval = function () {
        window.clearInterval(interval);
        interval = null;
    };
    function PrepareInitData() {
        var data = {};
        data.ProducedQty = 0;
        data.CycleTime = 0;
        data.PartCode = 0;
        data.ProgramName = 0;
        data.Pirometers = [];

        return data;
    }
    function CalcShiftNumber() {
        var today = new Date();
        var hour = today.getHours();
        today.setHours(hour + 2);
        hour = today.getHours();
        return hour < 8 ? 3 : hour < 16 ? 1 : 2;
    }
    function CalcEndShiftTime(shift_relative) {
        var today = new Date();
        var hour = today.getHours();
        today.setHours(hour + 2);
        hour = today.getHours();
        var shiftEndHour = hour < 8 ? 6 : hour < 16 ? 14 : 22;

        today.setSeconds(0); today.setMinutes(0); today.setMilliseconds(0);
        today.setHours(shiftEndHour - (8 * shift_relative));
        return new moment(today);
    }
    function CalcStartShiftTime(shift_relative) {
        var today = new Date();
        var hour = today.getHours();
        today.setHours(hour + 2);
        hour = today.getHours();
        var shiftEndHour = hour < 8 ? 6 : hour < 16 ? 14 : 22;

        today.setSeconds(0); today.setMinutes(0); today.setMilliseconds(0);
        today.setHours(shiftEndHour - (8 * (shift_relative + 1)));
        return new moment(today);
    }

    this.GetMachineId = function () { return machineId; };
}

function RTV_OEE(rtv, fixedDigits = 2) {
    //var rtv = rtv;
    //var fixedDigits = fixedDigits;
    //oee data current
    //oee data last shift
    this.LoadOEEData = function() {
        //ConsoleLog("Refreshing OEE Data");

        ajax = rtv.GetAjaxData("/ONEPROD/RTV/GetOeeResult", rtv.PrepareFilters(rtv.GetShiftRelative()));
        ajax.done(function (data) {
            _putOEEData(data);
        });
    }
    function _putOEEData(data) {
        $(rtv.GetParentrSelector() + " .result").each(function () {
            var id = $(this).attr("id");
            var target = id.replace("Result", "Target");
            if (data[id] - data.Targets[target] > -0.05) {
                $(this).addClass("colorInTarget");
                $(this).removeClass("colorOutOfTarget");
            } else {
                $(this).removeClass("colorInTarget");
                $(this).addClass("colorOutOfTarget");
            }
            $(rtv.GetParentrSelector() + " #" + id).text((data[id] * 100).toFixed(fixedDigits) + "%");
        });
    };

}

function RTV_ProdData(rtv) {
    //var rtv = rtv;
    var self = this;

    this.PutRTVData = function (data) {
        //ConsoleLog("Refreshing RTV Data");
        $(rtv.GetParentrSelector() + " #ProducedQty").text(data.ProducedQty);
        $(rtv.GetParentrSelector() + " #CycleTime").text(data.CycleTime + "s");
        //$(rtv.GetParentrSelector() + " #PartCode").text(data.PartCode);
        $(rtv.GetParentrSelector() + " #ProgramName").text(data.ProgramName);
        $(rtv.GetParentrSelector() + " #ProgramNo").text(data.ProgramNo);

        GetMesData(data);
        GetParameters();
        _PutDataOven(data);
    };
    function _PutDataOven(data) {
        //ConsoleLog("Refreshing RTV Data (Oven)");

        $(rtv.GetParentrSelector() + ' *[id*="PirometerDetail"]').removeClass();
        $(rtv.GetParentrSelector() + ' *[id*="PirometerDetail"]').addClass("colorInactive");
        $(rtv.GetParentrSelector() + ' *[id*="PirometerTemp"]').text(0);

        var classTermometer = "";
        var classTarget = "";

        if (data.Pirometers !== null) {
            for (i = 0; i < data.Pirometers.length; i++) {
                if (!(data.Pirometers[i].PirometerTempMax > 0)) {
                    classTermometer = "fa-thermometer-empty";
                    classTarget = "colorInactive";
                }
                else if (data.Pirometers[i].PirometerTempMin > data.Pirometers[i].PirometerTemp) {
                    classTermometer = "fa-thermometer-empty";
                    classTarget = "colorCloseToTarget";
                }
                else if (data.Pirometers[i].PirometerTempMax < data.Pirometers[i].PirometerTemp) {
                    classTermometer = "fa-thermometer-full";
                    classTarget = "colorOutOfTarget";
                }
                else {
                    classTermometer = "fa-thermometer-half";
                    //class3 = "colorInTarget";
                }

                $(rtv.GetParentrSelector() + " #PirometerTemp" + (i + 1)).text(data.Pirometers[i].PirometerTemp);
                $(rtv.GetParentrSelector() + " #PirometerTempMin" + (i + 1)).text(data.Pirometers[i].PirometerTempMin);
                $(rtv.GetParentrSelector() + " #PirometerTempMax" + (i + 1)).text(data.Pirometers[i].PirometerTempMax);

                $(rtv.GetParentrSelector() + " #PiroTermometer" + (i + 1)).removeClass();
                $(rtv.GetParentrSelector() + " #PiroTermometer" + (i + 1)).addClass("fas");
                $(rtv.GetParentrSelector() + " #PiroTermometer" + (i + 1)).addClass(classTermometer);
                $(rtv.GetParentrSelector() + " #PirometerDetail" + (i + 1)).removeClass();
                $(rtv.GetParentrSelector() + " #PirometerDetail" + (i + 1)).addClass(classTarget);
            }
        }
    }

    this.RefreshTargetAndDelta = function (qtyTarget) {
        var qtyDone = parseInt($(rtv.GetParentrSelector() + " #ProducedQty").text());
        var delta = qtyDone - qtyTarget;

        $(rtv.GetParentrSelector() + " #TargetQty").text(qtyTarget);
        $(rtv.GetParentrSelector() + " #DeltaQty").text(delta);
    };
    this.LoadLastShiftData = function (i) {
        var filters = rtv.PrepareFilters(rtv.GetShiftRelative() + i);
        $("#prevShiftDate").text(filters.dateFrom);

        ajax = rtv.GetAjaxData("/ONEPROD/RTV/GetOeeResult", filters);
        ajax.done(function (data) {
            if (data.NumberOfRecords < 2 && i <= 9) {
                self.LoadLastShiftData(i + 1);
            }
            else {
                _putLastShiftData(data);
            }
        });
    };
    function _putLastShiftData(data) {
        $(rtv.GetParentrSelector() + " .resultLastShift").each(function () {
            var id = $(this).attr("id");
            var id2 = id.replace("LastShift", "");
            var target = id.replace("ResultLastShift", "Target");
            if (data[id2] - data.Targets[target] > -0.05) {
                $(this).addClass("colorInTarget");
                $(this).removeClass("colorOutOfTarget");
            } else {
                $(this).removeClass("colorInTarget");
                $(this).addClass("colorOutOfTarget");
            }
            $(rtv.GetParentrSelector() + " #" + id).text((data[id2] * 100).toFixed(2) + "%");
        });
    }

    function GetMesData(data1) {
        ajax = rtv.GetAjaxData("/ONEPROD/MES/GetCurrentProducedItem", { resourceId: rtv.GetMachineId() });
        ajax.done(function (data) {
            if (data != null) {
                $(rtv.GetParentrSelector() + " #PartCode").text(data.Code);
                $(rtv.GetParentrSelector() + " #PartName").text(data.Name);
            }
            else {
                $(rtv.GetParentrSelector() + " #PartCode").text(data1.PartCode);
            }
        });
    }
    function GetParameters() {
        ajaxParams = rtv.GetAjaxData("/ONEPROD/RTV/GetProductionDataParameters", { machineId: rtv.GetMachineId() });
        ajaxParams.done(function (data) {
            if (data != null) {
                //console.log(data); 
                for (i = 0; i < data.length; i++) {
                    console.log("#" + data[i].Name + "_val");

                    let min = parseFloat(data[i].Min);
                    let max = parseFloat(data[i].Max);
                    let span = parseFloat((max + 50) - Math.max(min - 50,0));
                    let value = parseFloat(data[i].Value);
                    let percent = value * 100 / span;

                    let classParam = "bgColorInactive";

                    if (value >= min && value <= max) {
                        classParam = "bgColorCloseToTarget";

                        if (value >= min + 7 && value <= max - 6) {
                            classParam = "bgColorInTarget";
                        }
                    }
                    else {
                        classParam = "bgColorOutOfTarget";
                    }

                    $("#" + data[i].Name + "_val").text(data[i].Value);
                    $("#" + data[i].Name + "_indicator").css('height', percent + '%');
                    $("#" + data[i].Name + "_indicator").addClass(classParam);

                }
            }
            else {
                console.log("no data"); 
            }
        });
    }
}

function RTV_Chart(rtv) {
    //var rtv = rtv;
    var data = {};
    var chartDonut;
    var thisRtvChart = this;
    //chart target/actual
    //chart minutes
    //chart hours
    this.RefreshRTVChart = function () {
        //ConsoleLog("Refreshing RTV chart");
        ajax = rtv.GetAjaxData("/ONEPROD/RTV/GetChartRTVDataResults", rtv.PrepareFilters(rtv.GetShiftRelative()));
        ajax.done(function (d) {
            data = d;
            _refreshRTVChart(data, "chartRTV");
            thisRtvChart.RefreshChartHour();
        });
    };
    function _refreshRTVChart(data, elementId) {

        var el = $(rtv.GetParentrSelector()).find('#' + elementId);
        if (el.length > 0) {
            $(el[0]).html("");
            $(el[0]).empty();
            $(el[0]).append('<canvas id="Canvas' + elementId + '"></canvas>');

            var dataSet = [];
            dataSet.push(data.datasets[0]);
            dataSet.push(data.datasets[1]);
            data.datasets[1].borderDash = [10, 5];

            var myChart2 = new Chart($("#Canvas" + elementId), {
                title: data.title,
                type: 'line',
                data: {
                    labels: data.labels,
                    datasets: dataSet
                },
                options: {
                    legend: false,
                    responsive: true,
                    maintainAspectRatio: false,
                    animation: false,
                    scales: {
                        yAxes: [{
                            ticks: {
                                beginAtZero: true,
                                fontColor: "#569693"
                            },
                            gridLines: {
                                display: true,
                                color: "#21464e88"
                            }
                        }],
                        xAxes: [{
                            ticks: {
                                autoSkip: true,
                                autoSkipPadding: 158,
                                fontColor: "#569693"
                            },
                            gridLines: {
                                display: true,
                                color: "#21464e88"
                            }
                        }]
                    }
                }
            });
        }
    }
    this.RefreshChartHour = function () {
        //ConsoleLog("Refreshing Chart Hour");

        var currentSec = rtv.GetCurrentSec();
        var currentMinute = rtv.GetCurrentMinute();
        var currentHour = rtv.GetCurrentHour();
        var qtyDone;
        var qtyTarget;

        $(rtv.GetParentrSelector() + " .chartHour").each(function (i, el) {

            if (0 <= i && i < currentHour + 1) {
                //ConsoleLog("Refreshing hour: " + i);
                from = 60 * i;
                to = 60 * (i + 1) - 1;

                if (data != null && data.datasets != null && data.datasets.length > 1) {

                    if (to < currentMinute) {
                        qtyDone = parseInt(data.datasets[0].data[to]) - (from - 1 > 0 ? parseInt(data.datasets[0].data[from - 1]) : 0);//parseInt(data.datasets[0].data[from]);
                        qtyTarget = parseInt(data.datasets[1].data[to]) - (from - 1 > 0 ? parseInt(data.datasets[1].data[from - 1]) : 0);//- parseInt(data.datasets[1].data[from]);
                        //$(el).text(qtyDone - qtyTarget);
                    }
                    else {
                        qtyDone = parseInt($(rtv.GetParentrSelector() + " #ProducedQty").text()) - (from - 1 > 0 ? parseInt(data.datasets[0].data[from-1]) : 0);
                        //qtyTarget = parseInt(data.datasets[1].data[currentMinute]) - parseInt(data.datasets[1].data[from]);
                        qtyTarget = thisRtvChart.GetTargetForMinuteAndSec(currentMinute, currentSec) - (from - 1 > 0 ? parseInt(data.datasets[1].data[from-1]) : 0);

                        //$(el).text(qtyDone - qtyTarget);
                    }

                    thisRtvChart.RefreshChartHourByIndex(i, qtyDone, qtyTarget);

                    //if (qtyDone < qtyTarget) {
                    //    $(el).removeClass("rtvGreen");
                    //    $(el).addClass("rtvRed");
                    //}
                    //else {
                    //    $(el).removeClass("rtvRed");
                    //    $(el).addClass("rtvGreen");
                    //}
                }
            }
        });
    };
    this.RefreshChartHourByIndex = function (index, qtyDone, qtyTarget) {
        let el1 = $(rtv.GetParentrSelector() + " .chartHour")[index];

        $(el1).text(qtyDone - qtyTarget);

        if (qtyDone < qtyTarget) {
            $(el1).removeClass("rtvGreen");
            $(el1).addClass("rtvRed");
        }
        else {
            $(el1).removeClass("rtvRed");
            $(el1).addClass("rtvGreen");
        }
    }

    this.GetTargetForMinute = function (currentMinute) {
        var qtyTarget = (data != null && data.datasets != null) ? parseInt(data.datasets[1].data[currentMinute]) : 0;
        return qtyTarget > 0 ? qtyTarget : 0;
    };
    this.GetTargetForMinuteAndSec = function (currentMinute, currentSec) {

        if (data != null && data.datasets != null) {
            let dataLen = data.datasets[1].data.length-1;

            var qtyTargetD = currentMinute > 0? parseInt(data.datasets[1].data[currentMinute-1]) : 0;
            var qtyTargetU = parseInt(data.datasets[1].data[Math.min(currentMinute, dataLen)]);
            return parseInt(qtyTargetD + (qtyTargetU - qtyTargetD)*currentSec / 60);
        }
        else {
            return 0;
        }
    };

    this.RefreshMinutesChart = function (lastRefreshedMinute, currentMinute)
    {
        lastRefreshedMinute = lastRefreshedMinute - 480;
        lastRefreshedMinute = lastRefreshedMinute < 0 ? 0 : lastRefreshedMinute;

        var fltr = rtv.PrepareFilters(rtv.GetShiftRelative(), true);
        fltr.dateFrom = new moment(fltr.startShiftTime).add(lastRefreshedMinute, 'minutes').format('YYYY-MM-DD HH:mm');
        fltr.dateTo = new moment(fltr.startShiftTime).add(currentMinute, 'minutes').format('YYYY-MM-DD HH:mm');

        ajax = rtv.GetAjaxData("/ONEPROD/RTV/GetMinutesDetails", fltr);
        ajax.done(function (chartMinutesDataList) {
            //ConsoleLog("Refreshing minutes: " + lastRefreshedMinute + " - " + currentMinute);
            for (i = lastRefreshedMinute; i <= currentMinute; i++) {

                var chartMinutesData = chartMinutesDataList[i - lastRefreshedMinute];

                if (chartMinutesData !== null) {
                    var el = $(rtv.GetParentrSelector() + " .chartMinute")[i];
                    var actualCycleTime = chartMinutesData.ProdQty > 0 ? chartMinutesData.UsedTime / chartMinutesData.ProdQty : 0;
                    actualCycleTime = (parseFloat(actualCycleTime) > 0 ? actualCycleTime : 0).toFixed(2);

                    $(el).attr("data-cycletime", actualCycleTime);
                    $(el).attr("data-qty", chartMinutesData.ProdQty);
                    $(el).attr("data-usedtime", chartMinutesData.UsedTime);

                    _addRTVClass(el, chartMinutesData, actualCycleTime);
                }
                else {
                    ConsoleLog("No data for minute: " + i);
                }
            }
            rtv.SetLastRefreshedMinute(currentMinute);
            //rtv.lastRefreshedMinute = currentMinute;
            _refreshChartDonut();
            _refreshCycleTimeAvg(currentMinute);
            _refreshCycleTimeAnalize(currentMinute);
        });
    };
    function _refreshCycleTimeAvg(currentMinute, callback) {
        //ConsoleLog("Refreshing Cycle Time AVG");

        var countQty = 0;
        var usedTime = 0;
        var countQty_temp = 0;
        var usedTime_temp = 0;

        var i = 0;
        while (countQty < 10 && i < 15) {
            $el = $($(rtv.GetParentrSelector() + " .chartMinute")[currentMinute - i]);
            countQty_temp = parseInt($el.attr("data-qty"));
            usedTime_temp = parseFloat($el.attr("data-usedtime"));
            countQty += countQty_temp > 0 ? countQty_temp : 0;
            usedTime += usedTime_temp > 0 ? usedTime_temp : 0;
            i++;
        }

        $(rtv.GetParentrSelector() + " #ctQty").text(countQty);
        $(rtv.GetParentrSelector() + " #CycleTimeAvg").text((countQty > 0 ? usedTime / countQty : 0).toFixed(2) + "s");
        //rtv.rtvConn.__CONNECTION_BLINKFAST(false);
    }
    function _refreshCycleTimeAnalize(currentMinute) {
        var labels = [];
        var dataCT = [];
        var bgColors = [];
        var brdColors = [];
        //prepare labels,backgrounds,borders
        let divL = 30;
        let divR = 10;
        let div = 0.1;

        var cycleTime = parseFloat($("#CycleTime").text());
        for (i = -divL; i < 0; i++) {
            labels[i + divL] = (cycleTime + (div * i)).toFixed(1);
            dataCT[i + divL] = 0;
            bgColors[i + divL] = 'rgba(0, 216, 184, 0.2)';
            brdColors[i + divL] = 'rgba(0, 216, 184, 1)';
        }
        for (i = 0; i < divR; i++) {
            labels[i + divL] = (cycleTime + (div * i)).toFixed(1);
            dataCT[i + divL] = 0;
            bgColors[i + divL] = 'rgba(207, 169, 0, 0.2)'; //'rgba(255, 99, 132, 0.2)'
            brdColors[i + divL] = 'rgba(207, 169, 0, 1)';  //'rgba(255, 99, 132, 1)'
        }
        //prepare data
        cycleTime = 0.1;
        for (i = 0; i < currentMinute; i++) {
            $elMinute = $($(".chartMinute")[currentMinute - i]);
            cycleTime = parseFloat($elMinute.attr("data-cycletime"));

            if (cycleTime > 0) {
                var j = 0;
                while (parseFloat(labels[j]) <= cycleTime && j < labels.length) {
                    j++;
                }
                dataCT[j - 1] += parseInt($elMinute.attr("data-qty"));
            }
        }
        //chartCTAnalysis

        var el = $(rtv.GetParentrSelector() + ' #chartCTAnalysis');
        if (el.length > 0) {

            $(el).html("");
            $(el).empty();
            $(el).append('<canvas id="CanvaschartCTAnalysis"></canvas>');

            myChart3 = new Chart($("#CanvaschartCTAnalysis"), {
                title: "CT ANALYSIS",
                type: 'bar',
                data: {
                    labels: labels,
                    datasets: [
                        {
                            label: "Ilość",
                            backgroundColor: bgColors,
                            borderColor: brdColors,
                            borderWidth: 1,
                            data: dataCT
                        }
                    ],
                },
                options: {
                    legend: false,
                    responsive: true,
                    maintainAspectRatio: false,
                    animation: false,
                    scales: {
                        yAxes: [{
                            ticks: {
                                beginAtZero: true,
                                maxTicksLimit: 4,
                                fontColor: "#569693"
                            },
                            gridLines: {
                                display: true,
                                color: "#21464e88"
                            }
                        }],
                        xAxes: [{
                            ticks: {
                                autoSkip: false,
                                fontColor: "#569693"
                            },
                            gridLines: {
                                display: true,
                                color: "#21464e88"
                            }
                        }]
                    }
                }
            });
        }
    }
    function _addRTVClass(el, chartMinutesData, actualCycleTime) {
        //$(el).removeClass("rtvRed");
        //$(el).removeClass("rtvBlue");
        //$(el).removeClass("rtvGray");
        $(el).removeClass("rtvYellow");
        $(el).removeClass("rtvGreen");
        //$(el).removeClass("rtvViolet");
        $(el).removeClassPrefix("rtvReasonTypeId_");

        var arr1 = [30, 31, 33, 20, 21, 0];

        if (arr1.includes(chartMinutesData.ReasonTypeId)) {
            $(el).addClass("rtvReasonTypeId_" + chartMinutesData.ReasonTypeId);
        }
        else if (actualCycleTime > chartMinutesData.CycleTime) {
            $(el).addClass("rtvYellow");
        }
        else {
            $(el).addClass("rtvGreen");
        }

        //if (chartMinutesData.EntryType == 30 || chartMinutesData.EntryType == 31) {
        //    $(el).addClass("rtvRed");
        //}
        //else if (chartMinutesData.EntryType == 33) {
        //    $(el).addClass("rtvViolet");
        //}
        //else if (chartMinutesData.EntryType == 20 || chartMinutesData.EntryType == 21) {
        //    $(el).addClass("rtvBlue");
        //}
        //else if (chartMinutesData.ProdQty == 0 && chartMinutesData.EntryType < 0) {
        //    $(el).addClass("rtvGray");
        //}
        //else if (actualCycleTime > chartMinutesData.CycleTime) {
        //    $(el).addClass("rtvYellow");
        //}
        //else {
        //    $(el).addClass("rtvGreen");
        //}
    }
    function _refreshChartDonut() {

        if (chartDonut != null) {
            chartDonut.destroy();
        }
        
        chartDonut = null;
        var el = $(rtv.GetParentrSelector() + ' #chartDonut');

        if (el.length > 0) {
            var ids = ($("#chartDonut").attr("data-classes")).split(',');
            //var colors = ($("#chartDonut").attr("data-colors")).split(',');
            var colors = [];

            $(rtv.GetParentrSelector() + ' #chartDonut').html("");
            $(rtv.GetParentrSelector() + ' #chartDonut').empty();
            $(rtv.GetParentrSelector() + ' #chartDonut').append('<canvas id="CanvasDonut"></canvas>');

            var dataMinutes = [];

            //for (let i = 0; i < ids.length; i++) {
            //    dataMinutes[i] = $(rtv.GetParentrSelector() + " .chartMinute.rtvReasonTypeId_" + ids[i]).length;
            //}

            console.log("colors");

            colors[0] = $($("#donutLegend .rtvReasonTypeId_31")[0]).css("background-color");
            colors[1] = $($("#donutLegend .rtvReasonTypeId_20")[0]).css("background-color");
            colors[2] = $($("#donutLegend .rtvGray")[0]).css("background-color");
            colors[3] = $($("#donutLegend .rtvYellow")[0]).css("background-color");
            colors[4] = $($("#donutLegend .rtvReasonTypeId_10")[0]).css("background-color");
            colors[5] = $($("#donutLegend .rtvReasonTypeId_33")[0]).css("background-color");

            dataMinutes[0] = $(rtv.GetParentrSelector() + " .chartMinute.rtvReasonTypeId_30").length;
            dataMinutes[0]+= $(rtv.GetParentrSelector() + " .chartMinute.rtvReasonTypeId_31").length;
            dataMinutes[1] = $(rtv.GetParentrSelector() + " .chartMinute.rtvReasonTypeId_20").length;
            dataMinutes[2] = $(rtv.GetParentrSelector() + " .chartMinute.rtvGray").length;
            dataMinutes[2]+= $(rtv.GetParentrSelector() + " .chartMinute.rtvReasonTypeId_0").length;
            dataMinutes[3] = $(rtv.GetParentrSelector() + " .chartMinute.rtvYellow").length;
            dataMinutes[4] = $(rtv.GetParentrSelector() + " .chartMinute.rtvReasonTypeId_10").length;
            dataMinutes[4]+= $(rtv.GetParentrSelector() + " .chartMinute.rtvGreen").length;
            dataMinutes[5] = $(rtv.GetParentrSelector() + " .chartMinute.rtvReasonTypeId_33").length;
            dataMinutes[5]+= $(rtv.GetParentrSelector() + " .chartMinute.rtvReasonTypeId_21").length;

            $(rtv.GetParentrSelector() + " #minuteProduction").text(dataMinutes[4]);
            $(rtv.GetParentrSelector() + " #minuteMicroStops").text(dataMinutes[3]);
            $(rtv.GetParentrSelector() + " #minuteBreakdowns").text(dataMinutes[0]);
            $(rtv.GetParentrSelector() + " #minuteBreaks").text(dataMinutes[1]);
            $(rtv.GetParentrSelector() + " #minuteSetups").text(dataMinutes[5]);
            $(rtv.GetParentrSelector() + " #minuteNoData").text(dataMinutes[2]);

            
            chartDonut = new Chart($(rtv.GetParentrSelector() + " #CanvasDonut"), {
                type: 'doughnut',
                data: {
                    datasets: [{
                        data: dataMinutes,
                        backgroundColor: colors,
                        borderWidth: 0,
                        label: 'Dataset 1',
                        datalabels: {
                            align: 'end',
                            anchor: 'start',
                            display: true
                        }
                    }],
                    labels: ids
                },
                options: {
                    responsive: false,
                    maintainAspectRatio: true,
                    legend: false,
                    segmentShowStroke: false,
                    title: { display: false, text: 'A' },
                    animation: { animateScale: false, animateRotate: false },
                    plugins: {
                        datalabels: {
                            color: 'white',
                            display: true, 
                            formatter: Math.round
                        }
                    }
                }
            });
        }
    }
}

function RTV_Plan(rtv) {
    //var rtv = rtv;

    this.Refresh = function () {

        var fltr = rtv.PrepareFilters(rtv.GetShiftRelative(), true);
        //fltr.dateFrom = new moment(fltr.startShiftTime).add(lastRefreshedMinute, 'minutes').format('YYYY-MM-DD HH:mm');
        //fltr.dateTo = new moment(fltr.startShiftTime).add(currentMinute, 'minutes').format('YYYY-MM-DD HH:mm');

        var shiftStartDate = new moment(fltr.startShiftTime);
        var orderStartDate = new moment(fltr.startShiftTime);
        var diff1 = {};

        ajax = rtv.GetAjaxData("/ONEPROD/RTV/GetWorkorders", fltr);
        ajax.done(function (dataList) {

            $(rtv.GetParentrSelector() + " #plan").html("");

            var text = "";
            var width = 0;
            var totalWidth = 0;
            var left = 0;
            var code = "";
            var woQty = 0;
            var el;

            for (i = 0; i < dataList.length; i++) {
                width = dataList[i].ProcessingTime * 3 / 60;
                //left = new moment(dataList[i].StartTime) - startDate

                if (code != dataList[i].Code && totalWidth < 480 * 3 + 200) {

                    orderStartDate = new moment(dataList[i].StartTime);
                    diff1 = moment.duration(orderStartDate.diff(shiftStartDate));
                    left = 0 + diff1._milliseconds / (1000 * 20);

                    el = $("<div>");
                    $(el).addClass("rtvWorkorder");
                    $(el).css("width", width);
                    $(el).css("left", left);
                    woQty = dataList[i].Qty_Total;
                    $(el).html(new moment(dataList[i].StartTime).format('HH:mm') + "<div>" + dataList[i].Code + "</div><div class='woQty'>" + woQty + "</div>");
                    $(rtv.GetParentrSelector() + " #plan").append(el);
                }
                else if (code == dataList[i].Code) {
                    woQty += dataList[i].Qty_Total;
                    $(el).find(".woQty").text(woQty);
                }

                code = dataList[i].Code;
                totalWidth += width;
            }
        });
    };
}

function RTV_Connections(rtv) {
    //var rtv = rtv;
    var self = this;
    //controll of connectios:
    //plc, connector, analyzer, ajax
    this.CheckConnection = function() {
        //ConsoleLog("CheckConnection");
        self.__CONNECTION_BLINKFAST(true);
        self.__ConnectionAjax(2);
        //__PLCStatus(-1);
        //__PLCConnectorStatus(-1);
        //__PLCDataAnalyzerStatus(-1);

        $.post("/ONEPROD/RTV/CheckConnections", rtv.PrepareFilters(rtv.GetShiftRelative()), function (data) {
            self.__ConnectionAjax(1);
            __Connection(data.Connection);
            __PLCStatus(data.PLCStatus);
            __PLCConnectorStatus(data.PLCConnectorStatus);
            __PLCDataAnalyzerStatus(data.PLCDataAnalyzerStatus);
            self.__CONNECTION_BLINKFAST(false);
        })
            .fail(function () {
                self.__CONNECTION_BLINKFAST(false);
                self.__ConnectionAjax(0);
                __Connection(0);
            });
    }
    this.__ConnectionAjax = function(status) {
        //ConsoleLog("ConnectionAjax. STATUS: " + status);

        if (status == 2) {
            $(rtv.GetParentrSelector() + " #ConnectionAjax").removeClass("colorInTarget");
            $(rtv.GetParentrSelector() + " #ConnectionAjax").removeClass("colorOutOfTarget");
            $(rtv.GetParentrSelector() + " #ConnectionAjax").addClass("colorCloseToTarget");
            $(rtv.GetParentrSelector() + " #ConnectionAjax").removeClass("colorInactive");
        }
        else if (status == 1) {
            $(rtv.GetParentrSelector() + " #ConnectionAjax").addClass("colorInTarget");
            $(rtv.GetParentrSelector() + " #ConnectionAjax").removeClass("colorOutOfTarget");
            $(rtv.GetParentrSelector() + " #ConnectionAjax").removeClass("colorCloseToTarget");
            $(rtv.GetParentrSelector() + " #ConnectionAjax").removeClass("colorInactive");
        }
        else if (status == -1) {
            $(rtv.GetParentrSelector() + " #ConnectionAjax").removeClass("colorInTarget");
            $(rtv.GetParentrSelector() + " #ConnectionAjax").removeClass("colorOutOfTarget");
            $(rtv.GetParentrSelector() + " #ConnectionAjax").removeClass("colorCloseToTarget");
            $(rtv.GetParentrSelector() + " #ConnectionAjax").addClass("colorInactive");
        }
        else {
            $(rtv.GetParentrSelector() + " #ConnectionAjax").removeClass("colorInTarget");
            $(rtv.GetParentrSelector() + " #ConnectionAjax").addClass("colorOutOfTarget");
            $(rtv.GetParentrSelector() + " #ConnectionAjax").removeClass("colorCloseToTarget");
            $(rtv.GetParentrSelector() + " #ConnectionAjax").removeClass("colorInactive");
        }
    }
    function __Connection(status) {
        if (status == 1) {
            $(rtv.GetParentrSelector() + " #Connection").removeClass("colorInTarget");
            $(rtv.GetParentrSelector() + " #Connection").removeClass("colorOutOfTarget");
            $(rtv.GetParentrSelector() + " #Connection").addClass("colorInTarget");
        }
        else {
            $(rtv.GetParentrSelector() + " #Connection").removeClass("colorInTarget");
            $(rtv.GetParentrSelector() + " #Connection").removeClass("colorOutOfTarget");
            $(rtv.GetParentrSelector() + " #Connection").addClass("colorOutOfTarget");
        }
    }
    function __PLCStatus(staus) {
        if (staus == 1) {
            $(rtv.GetParentrSelector() + " #PLCStatus").addClass("colorInTarget");
            $(rtv.GetParentrSelector() + " #PLCStatus").removeClass("colorOutOfTarget");
            $(rtv.GetParentrSelector() + " #PLCStatus").removeClass("colorInactive");
        }
        else if (staus == -1) {
            $(rtv.GetParentrSelector() + " #PLCStatus").removeClass("colorInTarget");
            $(rtv.GetParentrSelector() + " #PLCStatus").removeClass("colorOutOfTarget");
            $(rtv.GetParentrSelector() + " #PLCStatus").addClass("colorInactive");
        }
        else {
            $(rtv.GetParentrSelector() + " #PLCStatus").removeClass("colorInTarget");
            $(rtv.GetParentrSelector() + " #PLCStatus").addClass("colorOutOfTarget");
            $(rtv.GetParentrSelector() + " #PLCStatus").removeClass("colorInactive");
        }
    }
    function __PLCConnectorStatus(status) {
        if (status == 1) {
            $(rtv.GetParentrSelector() + " #PLCConnectorStatus").addClass("colorInTarget");
            $(rtv.GetParentrSelector() + " #PLCConnectorStatus").removeClass("colorOutOfTarget");
            $(rtv.GetParentrSelector() + " #PLCConnectorStatus").removeClass("colorInactive");
        }
        else if (status == -1) {
            $(rtv.GetParentrSelector() + " #PLCConnectorStatus").removeClass("colorInTarget");
            $(rtv.GetParentrSelector() + " #PLCConnectorStatus").removeClass("colorOutOfTarget");
            $(rtv.GetParentrSelector() + " #PLCConnectorStatus").addClass("colorInactive");
        }
        else {
            $(rtv.GetParentrSelector() + " #PLCConnectorStatus").removeClass("colorInTarget");
            $(rtv.GetParentrSelector() + " #PLCConnectorStatus").addClass("colorOutOfTarget");
            $(rtv.GetParentrSelector() + " #PLCConnectorStatus").removeClass("colorInactive");
        }
    }
    function __PLCDataAnalyzerStatus(staus) {
        if (staus == 1) {
            $(rtv.GetParentrSelector() + " #PLCDataAnalyzerStatus").addClass("colorInTarget");
            $(rtv.GetParentrSelector() + " #PLCDataAnalyzerStatus").removeClass("colorOutOfTarget");
            $(rtv.GetParentrSelector() + " #PLCDataAnalyzerStatus").removeClass("colorInactive");
        }
        else if (staus == -1) {
            $(rtv.GetParentrSelector() + " #PLCDataAnalyzerStatus").removeClass("colorInTarget");
            $(rtv.GetParentrSelector() + " #PLCDataAnalyzerStatus").removeClass("colorOutOfTarget");
            $(rtv.GetParentrSelector() + " #PLCDataAnalyzerStatus").addClass("colorInactive");
        }
        else {
            $(rtv.GetParentrSelector() + " #PLCDataAnalyzerStatus").removeClass("colorInTarget");
            $(rtv.GetParentrSelector() + " #PLCDataAnalyzerStatus").addClass("colorOutOfTarget");
            $(rtv.GetParentrSelector() + " #PLCDataAnalyzerStatus").removeClass("colorInactive");
        }
    }

    var thredFastBlink;
    var thredFastBlinkCount = 0;
    this.__CONNECTION_BLINKFAST = function(active) {
        if (active == true) {
            if (thredFastBlinkCount < 1) {
                thredFastBlink = setInterval(function () {
                    if ($(rtv.GetParentrSelector() + " #Connection").hasClass("colorCloseToTarget")) {
                        $(rtv.GetParentrSelector() + " #Connection").removeClass("colorCloseToTarget");
                        $(rtv.GetParentrSelector() + " #Connection").removeClass("colorInTarget");
                        $(rtv.GetParentrSelector() + " #Connection").removeClass("colorOutOfTarget");
                        //$(rtv.GetParentrSelector() + " #Connection").addClass("colorInactive");
                    }
                    else {
                        $(rtv.GetParentrSelector() + " #Connection").removeClass("colorInTarget");
                        $(rtv.GetParentrSelector() + " #Connection").removeClass("colorOutOfTarget");
                        $(rtv.GetParentrSelector() + " #Connection").addClass("colorCloseToTarget");
                    }
                }, 125);
                //ConsoleLog("__CONNECTION_BLINKFAST is ACTIVATED");
            }
            thredFastBlinkCount++;
        }
        else {
            thredFastBlinkCount--;
            if (thredFastBlinkCount < 1) {
                clearInterval(thredFastBlink);
                thredFastBlinkCount = 0;
                thredFastBlink = null;
                $(rtv.GetParentrSelector() + " #Connection").removeClass("colorCloseToTarget");
                //ConsoleLog("__CONNECTION_BLINKFAST is DEACTIVATED");
            }
        }
    }

    this.IsNotRefreshingState = function () {
        if (thredFastBlink == null || (thredFastBlink < 1)) {
            return true;
        }
        else {
            return false;
        }
    }
}

function ConsoleLog(txt) {
    //console.log(txt);
}