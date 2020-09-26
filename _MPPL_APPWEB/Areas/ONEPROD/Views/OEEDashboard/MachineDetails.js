
var oeeResultGauge = new OEEGauge("oeeResultGauge");
var oeeAGauge = new OEEGauge("jg2");
var oeePGauge = new OEEGauge("jg3");
var oeeQGauge = new OEEGauge("jg4");
var gridProd = new OeeBrowseGrid("#gridProd");

function InitGauges() {
    oeeResultGauge.Init();
    oeeAGauge.InitAPQ();
    oeePGauge.InitAPQ();
    oeeQGauge.InitAPQ();
}
function RefreshGauges() {
    console.log("RefreshGauges");
    var dp = new OEEDataProvider();
    dp.GetData(filters);
    oeeResultGauge.Refresh(dp.data.OeeResult, dp.data.Targets.OeeTarget);
    oeeAGauge.Refresh(dp.data.AvailabilityResult, dp.data.Targets.AvailabilityTarget);
    oeePGauge.Refresh(dp.data.PerformanceResult, dp.data.Targets.PerformanceTarget);
    oeeQGauge.Refresh(dp.data.QualityResult, dp.data.Targets.QualityTarget);

    var totalScrap = dp.data.ScrapProcessQty + dp.data.ScrapMaterialQty;
    var totalProducedQty = dp.data.ProducedGoodQty + totalScrap;
    var nrft = parseFloat(1000000 * totalScrap / totalProducedQty).toFixed(0);

    $("#ProductionTimeInMin").text(dp.data.ProducedGoodQty + " szt (" + dp.data.ProductionTimeInMin.toFixed(0) + " min)");
    $("#StopsPlannedInMin").text(dp.data.StopsPlannedInMin.toFixed(1));
    $("#StopsPerformanceInMin").text(dp.data.StopsPerformanceInMin.toFixed(1));
    $("#StopsUnplannedInMin").text(dp.data.StopsUnplannedInMin.toFixed(1));

    //dwa poniższe trzeba ogarnac
    //$("#StopsUnplannedChangeoverInMin").text(dp.data.StopsUnplannedChangeoverInMin.toFixed(1));
    //$("#StopsBreakdownInMin").text(dp.data.StopsBreakdownInMin.toFixed(1));

    $("#ScrapProcessQty").text(dp.data.ScrapProcessQty + " szt (" + (100 * dp.data.ScrapProcessQty / totalProducedQty).toFixed(2) + "%)");
    $("#ScrapMaterialQty").text(dp.data.ScrapMaterialQty + " szt (" + (100 * dp.data.ScrapMaterialQty / totalProducedQty).toFixed(2) + "%)");
    $("#nrft").text(nrft);
    //nrft

}
function RefreshHorizontalBar() {
    $.get("/ONEPROD/OEEDashboard/OeeResultsHorizontalBar", filters, function (data) {
        $("#OeeResultsHorizontalBar").html(data);
    });
}
function RefreshAllOEECharts() {
    chartOEEData = new JsonHelper().GetPostData("/ONEPROD/OEEDashboard/GetChartOEEDataResults", filters);
    chartOEEData.done(function (data) {
        $(".chartOEE").each(function (i) {
            RefreshOeeChart(data, $(this).attr("id"), parseInt($(this).attr("data-dataset")));
        });
        //RefreshOeeChart(data, 'chartOEE', 1);
        //RefreshOeeChart(data, 'chartA', 3);
        //RefreshOeeChart(data, 'chartP', 5);
        //RefreshOeeChart(data, 'chartQ', 7);
    });
}
function RefreshReportOnlineData() {
    chartOEEData = new JsonHelper().GetPostData("/ONEPROD/OEEDashboard/GetOeeOnlineData", filters);
    chartOEEData.done(function (data) {
        console.log("MachineDetails.RefreshReportOnlineData");
        $("#reportOnlinePlcProdData").text(data.plcProdData + " SZT");
        $("#reportOnlinePlcStopData").text(parseFloat(data.plcStopData.toFixed(2)) + " MIN");
        $("#reportOnlineOperatorProdData").text(data.operatorProdData + " SZT");
        $("#reportOnlineOperatorStopData").text(parseFloat(data.operatorStopData.toFixed(2)) + " MIN");

        //console.log("RTV screenshots");
        //console.log(data.RTVScreenShotFilePath);
        //console.log(data.RTVScreenShotNameList);

        let rtvScreenShotNameList = data.RTVScreenShotNameList;
        let rtvScreenShotFilePath = data.RTVScreenShotFilePath;

        if (rtvScreenShotNameList.length > 0) {
            for (i = 0; i < rtvScreenShotNameList.length - 1; i++) {
                var link1 = '<a href="/Uploads/RTVscreenshots/' + rtvScreenShotNameList[i] + '" data-fancybox="groupRTV"></a>';
                $("#rtvScreenShots").prepend(link1);
            }
            $("#rtvScreenShot").attr("href", '/Uploads/RTVscreenshots/' + rtvScreenShotNameList[rtvScreenShotNameList.length-1]);

            $("[data-fancybox]").fancybox({ animationEffect: false });
        }
    });
}
function RefreshReportEnergyData(isMediaEnabled = true) {
    if (isMediaEnabled) {
        console.log("RefreshReportEnergyData");
        var energyDraw = new EnergyDetails(filters);
        energyDraw.LoadEnergyDetails("#ChartsEnergy");
        energyDraw.DrawContent();
    }
}
function ExtendDataset(dataset, copy = false) {
    dataset.data[1] = dataset.data[0];
    dataset.data[0] = copy ? dataset.data[0] : null;
    dataset.data[2] = copy ? dataset.data[0] : null;

    dataset.backgroundColor[1] = dataset.backgroundColor[0];
    dataset.backgroundColor[0] = dataset.backgroundColor[0];
    dataset.backgroundColor[2] = dataset.backgroundColor[0];
}
function RefreshOeeChart(data, elementId, dataSetIndex) {

    $('#' + elementId).html("");
    $('#' + elementId).empty();
    $('#' + elementId).append('<canvas id="Canvas' + elementId + '"></canvas>');

    AssignLabelsToDataSet(data);
    var dataSet = [];

    var dataTarget = data.datasets[dataSetIndex - 1];
    var dataResults = data.datasets[dataSetIndex];

    if (dataTarget.data.length < 2) {
        ExtendDataset(dataTarget, true);
        ExtendDataset(dataResults);

        data.labels[1] = data.labels[0];
        data.labels[0] = "";
        data.labels[2] = "";
    }

    dataSet.push(dataTarget);
    dataSet.push(dataResults);

    var trendline = { style: "rgba(145,105,180, 0.8)", width: 2 }
    dataSet[1].trendlineLinear = trendline;

    var myChart = new Chart('Canvas' + elementId, {
        //plugins: [pluginTrendlineLinear],
        title: data.title,
        type: 'bar',
        responsive: true,
        data: {
            labels: data.labels,
            datasets: dataSet,
        },
        options: {
            scales: {
                yAxes: [{
                    type: 'linear',
                    display: true,
                    position: 'left',
                    ticks: {
                        beginAtZero: true,
                        steps: 12,
                        stepValue: 10,
                        max: 120
                    },
                }],
            },
            maintainAspectRatio: false,
            legend: {
                display: false
            },
            tooltips: {
                enabled: true,
                callbacks: {
                    title: function (tooltipItems, data) {
                        //console.table(tooltipItems);
                        //console.table(data);
                        return '';
                    },
                    label: function (tooltipItem, data) {
                        return (data.datasets[tooltipItem.datasetIndex].label).replace('\n', ' ') + ": " +
                            tooltipItem.yLabel.toFixed(2).replace('.00', '') +
                            data.datasets[tooltipItem.datasetIndex].displayUnit;
                    }
                }
            },
            //plugins: {
            //    datalabels: LabelsBar,
            //},
        },
        plugins: [{
            beforeInit: function (chart) {
                chart.data.labels.forEach(function (e, i, a) {
                    if (!Array.isArray(e) && a.length < 15) {
                        e = e.replace(' ', '\n');
                        e = e.replace(' ', '\n');
                        e = e.replace(' ', '\n');
                        e = e.replace(' ', '\n');
                        if (/\n/.test(e)) {
                            a[i] = e.split(/\n/)
                        }
                    }
                })
            }
        }, pluginTrendlineLinear],
    });
}

var chartGroups = { groups: [] };
var chartGroup = { name: "", charts: [] };

function AddChartToCollection(chart, chartGroupName) {

    group = chartGroups.groups.find(x => x.name == chartGroupName);

    if (group == null) {
        //console.log("dodaj grupę");
        chartGroups.groups.push({ name: chartGroupName, charts: [chart] });
    }
    else {
        group.charts.push(chart);
        if (group.charts.length >= 2) {
            ShowCustomLegend2("#" + chartGroupName + "-Legend", group.charts);
        }
    }


    //for (i = 0; i < chartGroups.length; i++) {
    //    if (chartGroups[i].name == chartGroupName) {
    //        chartGroups[i].charts.push(chart);

    //        if (chartGroups[i].charts.length >= 3) {
    //            ShowCustomLegend2("#" + chartGroupName + "-Legend", chartGroups[i].charts);
    //        }
    //    }
    //}
    //if (charts.length >= 3) {
    //    //ShowCustomLegend2("#" + el.id + "-Legend", charts);
    //}
}

function RefreshAllChartsByEntryType() {
    console.log("RefreshAllChartsByEntryType");
    //console.log(RefreshAllChartsByEntryType);
    $.each($(".chartGroup"), function (i, el) {
        var chartGroup = el.id;
        $(".GroupChartsEntryType, .GroupChartsCumulatedEntryType, .GroupChartsParetoEntryType", el).each(function () {
            filters.limit = 5;
            RefreshOneChartByEntryType(this, chartGroup);
        });
        //console.log("grupa " + i + " gotowa /" + el.id);
    });
}
async function RefreshOneChartByEntryType(el, chartGroup = "", autohideEmpty = true) {

    var reasonTypeId = $(el).attr("data-reasonTypeId");
    var Id = el.id;
    var jsonLink = "";
    var chartOptions = {};

    if ($(el).hasClass("GroupChartsEntryType")) {
        jsonLink = "/ONEPROD/OEEDashboard/GetChartDataByEntryType";
        chartOptions = groupOptions;
    }
    else if ($(el).hasClass("GroupChartsCumulatedEntryType")) {
        jsonLink = "/ONEPROD/OEEDashboard/GetChartDataCumulatedByEntryType";
        chartOptions = groupOptions;
    }
    else if ($(el).hasClass("GroupChartsParetoEntryType")) {
        jsonLink = "/ONEPROD/OEEDashboard/GetChartParetoDataResults";
        chartOptions = paretoOptions;
    } else if ($(el).hasClass("GroupChartsParetoOfReasonType")) {
        jsonLink = "/ONEPROD/OEEDashboard/GetChartParetoDataOfReasonTypes";
        chartOptions = paretoOptions;
    }
    else if ($(el).hasClass("GroupChartsOfReasonType")) {
        jsonLink = "/ONEPROD/OEEDashboard/GetChartDataOfReasonTypes";
        chartOptions = paretoOptions;
    }
    else if ($(el).hasClass("GroupChartsCumulatedOfReasonType")) {
        jsonLink = "/ONEPROD/OEEDashboard/GetChartDataCumulatedOfReasonTypes";
        chartOptions = paretoOptions;
    }
    else if ($(el).hasClass("GroupChartsParetoScrapReason")) {
        jsonLink = "/ONEPROD/OEEDashboard/GetChartParetoScrapReason";
        chartOptions = paretoOptions;
    }
    else if ($(el).hasClass("GroupChartsParetoScrapReasonType")) {
        jsonLink = "/ONEPROD/OEEDashboard/GetChartParetoScrapReasonType";
        chartOptions = paretoOptions;
    }
    else if ($(el).hasClass("GroupChartsParetoScrapAnc")) {
        jsonLink = "/ONEPROD/OEEDashboard/GetChartParetoScrapAnc";
        chartOptions = paretoOptions;
    }
        

    if (Id !== null) {
        filters.reasonTypeId = reasonTypeId; //filters.entryType = reasonTypeId;
        chartJson = new JsonHelper().GetPostData(jsonLink, filters);
        chartJson.done(function (chartData) {
            if (chartData.labels.length > 0) {
                var chart = RefreshChartByEntryType(chartData, "#" + Id, Id + "Canvas", chartOptions);
                //console.log("chartGroup");
                //console.log(chartGroup);
                if (chartOptions !== paretoOptions) {
                    AddChartToCollection(chart, chartGroup);
                }
            }
            else {
                $("#" + Id).html("");
                $("#" + Id).empty();
                $("#" + Id + "Legend").html("");
                var parentEl = $(el).closest(".SectionBox");
                $(parentEl).addClass(autohideEmpty == true ? "hidden" : "");
            }
        });
    }
}
async function RefreshOneChartOfReasonType(el, chartGroup = "", autohideEmpty = false) {
    var Id = $(el).attr("id");
    var jsonLink = "";
    var chartOptions = {};

    if ($(el).hasClass("GroupChartsOfReasonType")) {
        jsonLink = "/ONEPROD/OEEDashboard/GetChartDataOfReasonTypes";
        chartOptions = groupOptions;
    }
    else if ($(el).hasClass("GroupChartsCumulatedOfReasonType")) {
        jsonLink = "/ONEPROD/OEEDashboard/GetChartDataCumulatedOfReasonTypes";
        chartOptions = groupOptions;
    }
    else if ($(el).hasClass("GroupChartsParetoOfReasonType")) {
        jsonLink = "/ONEPROD/OEEDashboard/GetChartParetoDataOfReasonTypes";
        chartOptions = paretoOptions;
    }

    //console.log("new chart");
    if (Id !== null) {
        filters.limit = 1000;
        chartJson = new JsonHelper().GetPostData(jsonLink, filters);
        chartJson.done(function (chartData) {
            if (chartData.labels.length > 0) {
                var chart = RefreshChartByEntryType(chartData, "#" + Id, Id + "Canvas", chartOptions);
                //console.log("chartGroup");
                //console.log(chartGroup);
                if (chartOptions !== paretoOptions) {
                    AddChartToCollection(chart, chartGroup);
                }
            }
            else {
                $("#" + Id).html("");
                $("#" + Id).empty();
                $("#" + Id + "Legend").html("");
                var parentEl = $(el).closest(".SectionBox");
                $(parentEl).addClass(autohideEmpty == true ? "hidden" : "");
            }
        });
    }
}

function RefreshChartByEntryType(data, elementSelector, canvasId, options) {

    $(elementSelector).html("");
    $(elementSelector).empty();
    $(elementSelector).append('<canvas id="' + canvasId + '" ></canvas>');

    var testArray = CreateCommentSummary(data);
    if (testArray.length > 0) {
        $(elementSelector + "Comment").tooltip({ title: testArray.toString() });
    }
    else {
        $(elementSelector + "Comment").hide();
    }

    AssignLabelsToDataSet(data);
    Change0toNull(data);

    if (data.datasets[0].data.length < 2) {

        for (i = 0; i < data.datasets.length; i++) {
            if (data.datasets[i].label == "Cel")
                ExtendDataset(data.datasets[i], true);
            else
                ExtendDataset(data.datasets[i], false);
        }

        data.labels[1] = data.labels[0];
        data.labels[0] = "";
        data.labels[2] = "";
    }

    var myChart = new Chart($("#" + canvasId), {
        title: data.title,
        type: 'bar',
        responsive: true,
        maintainAspectRatio: true,
        data: {
            labels: data.labels,
            datasets: data.datasets
        },
        options: options,
        plugins: [{
            beforeInit: function (chart) {
                chart.data.labels.forEach(function (e, i, a) {
                    if (!Array.isArray(e) && a.length < 15) {
                        e = e.replace(' ', '\n');
                        e = e.replace(' ', '\n');
                        e = e.replace(' ', '\n');
                        e = e.replace(' ', '\n');
                        if (/\n/.test(e)) {
                            a[i] = e.split(/\n/);
                        }
                    }
                });
            }
        }]
    });

    return myChart;
    //ShowCustomLegend(elementSelector + "Legend", myChart);
}
function ShowCustomLegend(legendDivSelector, myChart) {

    $(legendDivSelector).html('');

    myChart.legend.legendItems.forEach(function (item) {
        var legendItem = $('<div>').attr('id', legendDivSelector + 'Itm_' + item.datasetIndex)
            .attr('class', 'LegendItm');
        var legendItemLC = $('<div>')
            .attr('class', 'LegendItmLC')
            .css('background-color', item.fillStyle).css('border', item.lineWidth + "px solid " + item.strokeStyle[0]);
        var legendItemLT = $('<div>')
            .attr('class', 'LegendItmLT')
            .text(item.text);

        legendItem.append(legendItemLC);
        legendItem.append(legendItemLT);
        $(legendDivSelector).append(legendItem);

        legendItem.click(function (e) {
            legendItem.toggleClass("inactive");
            myChart.legend.options.onClick.call(myChart, e, item);
        });
    })
}
function ShowCustomLegend2(legendDivSelector, charts = []) {

    $(legendDivSelector).html('');
    console.log("MachineDetails.ShowCustomLegend2");

    var legendItem = null;
    var legendItemLC = null;
    var legendItemLT = null;

    if (0 < charts.length) {
        //console.log("Sa charty");
        charts[0].legend.legendItems.forEach(function (item) {

            legendItem = $('<div>').attr('id', legendDivSelector + 'Itm_' + item.datasetIndex)
                .attr('class', 'LegendItm');
            legendItemLC = $('<div>')
                .attr('class', 'LegendItmLC')
                .css('background-color', item.fillStyle).css('border', item.lineWidth + "px solid " + item.strokeStyle[0]);
            legendItemLT = $('<div>')
                .attr('class', 'LegendItmLT')
                .text(item.text);

            legendItem.append(legendItemLC);
            legendItem.append(legendItemLT);
            $(legendDivSelector).append(legendItem);

            legendItem.click(function (e) {
                $(this).toggleClass("inactive");
                for (i2 = 0; i2 < charts.length; i2++) {
                    var chart = charts[i2];
                    chart.legend.options.onClick.call(chart, e, item);
                }
            });
        })
    }
}

function Change0toNull(data) {
    for (i = 0; i < data.datasets.length; i++) {

        if (data.datasets[i].label == "Komentarze") {
            data.datasets[i].data.forEach(function (item, j, arr) {
                if (arr[j] < 0) {
                    arr[j] = null;
                }
            });
        }
        else if (data.datasets[i].label != "Cel") {
            data.datasets[i].data.forEach(function (item, j, arr) {
                if (arr[j] == 0) {
                    arr[j] = null;
                }
            });
        }
    }
}
function AssignLabelsToDataSet(data) {
    for (i = 0; i < data.datasets.length; i++) {

        if (data.datasets[i].datalabels == "LabelsLine") {
            data.datasets[i].datalabels = LabelsLine;
        }
        if (data.datasets[i].datalabels == "LabelsLineRound") {
            data.datasets[i].datalabels = LabelsLineRound;
        }
        if (data.datasets[i].datalabels == "LabelsBar") {
            data.datasets[i].datalabels = LabelsBar;
        }
        if (data.datasets[i].datalabels == "LabelsNone") {
            data.datasets[i].datalabels = LabelsNone;
        }
        if (data.datasets[i].datalabels == "LabelsComments") {
            data.datasets[i].datalabels = LabelsComments;
        }
    }
}
function CreateCommentSummary(data) {
    var arrayComment = [];
    for (i = 0; i < data.datasets.length; i++) {
        for (j = 0; j < data.datasets[i].comments.length; j++) {
            if (data.datasets[i].comments[j] != null && (data.datasets[i].comments[j]).length > 1) {

                var comment = data.datasets[i].comments[j];

                arrayComment.push(
                    data.labels[j] + "-" +
                    data.datasets[i].label + " (" +
                    data.datasets[i].data[j] + " min): " +
                    comment
                );
            }
        }
    }
    return arrayComment;
}

async function RefreshChartParetoScrapReasonType(Id) {
    chartJson = new JsonHelper().GetPostData("/ONEPROD/OEEDashboard/GetChartParetoScrapReasonType", filters);
    chartJson.done(function (chartData) {
        if (chartData.labels.length > 0) {
            var chart = RefreshChartByEntryType(chartData, "#" + Id, Id + "Canvas", paretoOptions);
            chart.options.scales.yAxes[0].scaleLabel.labelString = "szt";
            //console.log("chartGroup");
            //console.log(chartGroup);
        }
        else {
            $("#" + Id).html("");
            $("#" + Id).empty();
        }
    });
}
async function RefreshChartParetoScrapReason(Id) {
    console.log("RefreshChartParetoScrapReason");
    chartJson = new JsonHelper().GetPostData("/ONEPROD/OEEDashboard/GetChartParetoScrapReason", filters);
    chartJson.done(function (chartData) {
        if (chartData.labels.length > 0) {
            var chart = RefreshChartByEntryType(chartData, "#" + Id, Id + "Canvas", paretoOptions);
            chart.options.scales.yAxes[0].scaleLabel.labelString = "szt";
            //console.log("chartGroup");
            //console.log(chartGroup);
        }
        else {
            $("#" + Id).html("");
            $("#" + Id).empty();
        }
    });
}
async function RefreshChartParetoScrapAnc(Id) {
    console.log("RefreshChartParetoScrapAnc");
    chartJson = new JsonHelper().GetPostData("/ONEPROD/OEEDashboard/GetChartParetoScrapAnc", filters);
    chartJson.done(function (chartData) {
        if (chartData.labels.length > 0) {
            var chart = RefreshChartByEntryType(chartData, "#" + Id, Id + "Canvas", paretoOptions);
            chart.options.scales.yAxes[0].scaleLabel.labelString = "szt";
            //console.log("MachineDetails.RefreshChartParetoAnc");
            //console.log(chartGroup);
        }
        else {
            $("#" + Id).html("");
            $("#" + Id).empty();
        }
    });
}

var paretoOptions = {
    stacked: true,
    scales: {
        yAxes: [{
            type: 'linear',
            display: true,
            position: 'left',
            id: 'y-axis-1',
            ticks: {
                beginAtZero: true,
            },
            scaleLabel: {
                display: true,
                labelString: 'min'
            }
        }, {
            type: 'linear',
            display: true,
            position: 'right',
            id: 'y-axis-2',

            ticks: {
                beginAtZero: true
            },
            gridLines: {
                drawOnChartArea: false, // only want the grid lines for one axis to show up
            },
            scaleLabel: {
                display: true,
                labelString: '%'
            }
        }],
        xAxes: [{ ticks: { autoSkip: false } }],
    },
    legend: { display: false },
    tooltips: {
        mode: 'index',
        intersect: false,
        callbacks: {
            title: function (tooltipItems, data) {
                //console.table(tooltipItems);
                //console.table(data);
                return '';
            },
            label: function (tooltipItem, data) {
                return data.datasets[tooltipItem.datasetIndex].label + ": " +
                    tooltipItem.yLabel.toFixed(2).replace('.00', '') +
                    data.datasets[tooltipItem.datasetIndex].displayUnit;
            }
        }
    }
};
var groupOptions = {
    scales: {
        xAxes: [{ stacked: true }],
        yAxes: [
            { ticks: { beginAtZero: true }, position: 'left' }
        ]
    },
    legend: { display: false },
    tooltips: {
        mode: 'nearest',
        intersect: false,
        callbacks: {
            title: function (tooltipItems, data) {
                if (data.datasets[tooltipItems[0].datasetIndex].label == "Komentarze") {
                    return "Komentarze:"
                }
                else {
                    var d = data.labels[tooltipItems[0].index];
                    var title = Array.isArray(d) ? d.join(" ").replace('\n', ' ') : d.replace('\n', ' ');
                    return title;
                }
            },
            label: function (tooltipItem, data) {
                console.log("label");
                const value = data.datasets[tooltipItem.datasetIndex].data[tooltipItem.index];

                if (value == 0) {
                    return null;
                }

                if (data.datasets[tooltipItem.datasetIndex].label == "Komentarze") {
                    var arr = data.datasets[tooltipItem.datasetIndex].comments[tooltipItem.index].split('\n');
                    arr = arr.filter(x => x != "");
                    console.log("komentarze");
                    console.log(arr);
                    return arr;
                }
                else {
                    return (data.datasets[tooltipItem.datasetIndex].label).replace('\n', ' ') + ": " +
                        tooltipItem.yLabel.toFixed(2).replace('.00', '') +
                        data.datasets[tooltipItem.datasetIndex].displayUnit;
                }
            },
            labelColor: function (tooltipItem, chart) {

                console.log("labelColor");
                var color = "";

                if (chart.config.data.datasets[tooltipItem.datasetIndex].label == "Komentarze") {
                    var arr = chart.config.data.datasets[tooltipItem.datasetIndex].backgroundColor[tooltipItem.index].split('\n');
                    arr = arr.filter(x => x != "");
                    color = arr;
                    color = "red";
                }
                else {
                    color = chart.config.data.datasets[tooltipItem.datasetIndex].backgroundColor[tooltipItem.index];
                }
                console.log("colors");
                console.log(color);

                return {
                    borderColor: color,
                    backgroundColor: color
                }
            },
            footer: function (tooltipItems, data) {
                //console.log("this._chart.tooltip._options.callbacks.label");
                //console.log(this._chart.tooltip._options.callbacks.label);

                console.log("footer");
                this._chart.tooltip._options.callbacks.labelColor(tooltipItems[0], this._chart);
                return "";
            }
        }
    }
};
var LabelsLine = {
    anchor: 'center',
    align: 'center',
    padding: 3,
    borderRadius: 8,
    color: '#FFFFFF',
    backgroundColor: function (context) {
        return context.dataset.borderColor[0];
    },
    display: function (context) {
        var value = context.dataset.data[context.dataIndex];
        return value > 0;
    },
    font: {
        weight: 'bold', size: 9
    },
    formatter: function (value, context) {
        return value.toFixed(4).replace('.0', '');
    }
};
var LabelsLineRound = {
    anchor: 'center',
    align: 'center',
    padding: 3,
    borderRadius: 8,
    color: function (context) { return context.dataset.fontColor; },
    backgroundColor: function (context) {
        return context.dataset.pointBackgroundColor;
    },
    display: function (context) {
        var value = context.dataset.data[context.dataIndex];
        return value > 0;
    },
    font: function (context) {
        return {
            weight: 'bold',
            size: context.dataset.fontSize
        }
    },
    formatter: Math.round
};
var LabelsBar = {
    anchor: 'center',
    align: 'center',
    color: function (context) {
        var colorLb = context.dataset.borderColor[Math.min(context.dataIndex, context.dataset.borderColor.length - 1)];
        colorLb = shadeColor2(colorLb, -0.30);
        return colorLb;
    },
    display: function (context) {
        var value = context.dataset.data[context.dataIndex];
        var max = 1; //context.chart.config.data.datasets
        var sum = 0;
        var imax = context.chart.config.data.datasets.length;
        var jmax = context.chart.config.data.datasets[0].data.length;
        var height = context.chart.height;

        var j = 0;
        while (j < jmax) {
            sum = 0;
            for (i = 1; i < imax; i++) {
                localValue = parseInt(context.chart.config.data.datasets[i].data[j]);
                if (localValue > 0) {
                    sum += localValue;
                }
            }
            max = sum > max ? sum : max;
            sum = Math.max.apply(Math, context.chart.config.data.datasets[0].data); //sprawdz.czy cel wyższy
            max = sum > max ? sum : max;
            j++;
        }

        var percent = (max > 0 ? value / max : 0);
        var h_px = percent * height;
        if (h_px >= 20) {
            return true;
        }
        else {
            return false;
        }
    },
    font: {
        weight: 'bold', size: 12
    },
    formatter: Math.round,
    padding: {
        top: 10,
        bottom: 10
    }
};
var LabelsComments = {
    anchor: 'center',
    align: 'center',
    padding: 3,
    borderRadius: 8,
    color: function (context) { return context.dataset.fontColor; },
    backgroundColor: function (context) {
        return "red";
    },
    display: function (context) {
        var value = context.dataset.data[context.dataIndex];
        //console.log(value);
        return value >= 0;
    },
    font: {
        weight: 'bold', size: 12
    },
    formatter: function (value, context) {
        return "k";
    }
};
var LabelsNone = {
    display: false
};

function InitGridProd() {
    gridProd.InitGrid();
}
function RefreshGridProd() {
    filters.reasonTypeId = 10;
    gridProd.RefreshGrid(filters);
}

function shadeColor2(color, percent) {
    var f = parseInt(color.slice(1), 16), t = percent < 0 ? 0 : 255, p = percent < 0 ? percent * -1 : percent, R = f >> 16, G = f >> 8 & 0x00FF, B = f & 0x0000FF;
    return "#" + (0x1000000 + (Math.round((t - R) * p) + R) * 0x10000 + (Math.round((t - G) * p) + G) * 0x100 + (Math.round((t - B) * p) + B)).toString(16).slice(1);
}
Array.prototype.sum = function () { return [].reduce.call(this, (a, i) => a + i, 0); };
