function EnergyDetails(filters) {
    var self = this;
    var filt = filters;

    this.LoadEnergyDetails = function (divSelector) {
        console.log("Load EnergyDetails.js");
        var JsonHelp = new JsonHelper();
        var ReturnJson = JsonHelp.GetData("/ONEPROD/Energy/EnergyDetails");
        ReturnJson.done(function (data) {
            $(divSelector).html(data);
        });
    };
    this.DrawContent = function () {
        console.log("DrawContent - EnergyDetails");
        energyData = new JsonHelper().GetPostData("/ONEPROD/Energy/GetEnergySummaryData", filt);
        energyData.done(function (data) {
            //console.log(data);
            if (data != null) {
                $("#energyTotalCost").text(data.TotalCost.toFixed(2));
                $("#energyCostPerProductionUnit").text(data.PricePerProductionUnit.toFixed(5));
                $("#energyUsePerProductionUnit").text(data.UsePerProductionUnit.toFixed(4));
                RefreshEnergyChartData(data.ChartPricePerUnit, "chartEnergyPricePerUnit");
                RefreshEnergyConsumptionChartData(data.ChartEnergyConsumption, "chartEnergyConsumption");
                RefreshPieEnergyChartData(data.ChartTotalCostByType, "chartEnergyTotalCost");
                RefreshPieEnergyChartData(data.ChartUseEnergyPerUnit, "chartEnergyUsePerUnit");
            }
        });
    };
    this.DrawBig = function (divSelector, chartNumber) {
        energyData = new JsonHelper().GetPostData("/ONEPROD/Energy/GetEnergySummaryData", filt);
        energyData.done(function (data) {
            if (chartNumber == 1) {
                RefreshPieEnergyChartData(data.ChartTotalCostByType, divSelector);
            } else if (chartNumber == 2) {
                RefreshPieEnergyChartData(data.ChartUseEnergyPerUnit, divSelector);
            } else if (chartNumber == 3) {
                RefreshEnergyChartData(data.ChartPricePerUnit, divSelector);
            } else if (chartNumber == 4) {
                RefreshEnergyConsumptionChartData(data.ChartEnergyConsumption, divSelector);
            }
        });
    };

    function RefreshEnergyChartData(data, elementId) {
        $('#' + elementId).html("MachieDetails_RefreshEnergyData_RefreshChartData");
        $('#' + elementId).empty();
        $('#' + elementId).append('<canvas id="Canvas' + elementId + '"></canvas>');

        var dataSet = [];

        var dataResults = data.datasets[0];
        AssignLabelsEnergyToDataSet(data);
        dataSet.push(dataResults);


        var myChart = new Chart('Canvas' + elementId, {
            title: data.title,
            type: 'bar',
            responsive: true,
            data: {
                labels: data.labels,
                datasets: dataSet
            },
            options: {
                scales: {
                    yAxes: [{
                        type: 'linear',
                        display: true,
                        position: 'left',
                        ticks: {
                            beginAtZero: true
                        }
                    }]
                },
                maintainAspectRatio: false,
                legend: LabelsNone,
                tooltips: {
                    enabled: true,
                    callbacks: {
                        title: function (tooltipItems, data) {
                            return '';
                        },
                        label: function (tooltipItem, data) {
                            return (data.datasets[tooltipItem.datasetIndex].label).replace('\n', ' ') + ": " +
                                tooltipItem.yLabel.toFixed(2).replace('.00', '') +
                                data.datasets[tooltipItem.datasetIndex].displayUnit;
                        }
                    }
                }
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
                                a[i] = e.split(/\n/);
                            }
                        }
                    });
                }
            }, pluginTrendlineLinear]
        });
    }
    function RefreshEnergyConsumptionChartData(data, elementId) {

        $('#' + elementId).html("MachieDetails_RefreshEnergyData_RefreshChartData");
        $('#' + elementId).empty();
        $('#' + elementId).append('<canvas id="Canvas' + elementId + '"></canvas>');

        var dataSet = [];

        var productionResults = data.datasets[0];
        var gasResults = data.datasets[1];
        var electricityResults = data.datasets[2];
        var waterResults = data.datasets[3];

        dataSet.push(productionResults);
        dataSet.push(gasResults);
        dataSet.push(electricityResults);
        dataSet.push(waterResults);
        AssignLabelsToDataSet(dataSet);

        var myChart = new Chart('Canvas' + elementId, {
            title: data.title,
            type: 'bar',
            responsive: true,
            
            data: {
                labels: data.labels,
                datasets: dataSet,
            },
            options: {
                maintainAspectRatio: false,
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
                            labelString: 'pln'
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
                            labelString: 'szt.'
                        }
                    }],
                    xAxes: [{ ticks: { autoSkip: false } }],
                },
                legend: { display: true },
                tooltips: {
                    mode: 'index',
                    intersect: false,
                    callbacks: {
                        title: function (tooltipItems, data) {
                            return '';
                        },
                        label: function (tooltipItem, data) {
                            return data.datasets[tooltipItem.datasetIndex].label + ": " +
                                tooltipItem.yLabel.toFixed(2).replace('.00', '') +
                                data.datasets[tooltipItem.datasetIndex].displayUnit;
                        }
                    }
                }
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
                                a[i] = e.split(/\n/);
                            }
                        }
                    });
                }
            }]
        });
        return myChart;
    }
    function RefreshPieEnergyChartData(data, elementId) {
        $('#' + elementId).html("MachieDetails_RefreshEnergyData_RefreshChartData");
        $('#' + elementId).empty();
        $('#' + elementId).append('<canvas id="Canvas' + elementId + '"></canvas>');

        var dataSet = [];
        AssignLabelsEnergyToDataSet(data);
        var dataResults = data.datasets[0];

        dataSet.push(dataResults);


        var myChart = new Chart('Canvas' + elementId, {
            title: data.title,
            type: 'pie',
            responsive: true,
            data: {
                labels: data.labels,
                datasets: dataSet
            }
        });
    }
    function AssignLabelsEnergyToDataSet(data) {
        for (i = 0; i < data.datasets.length; i++) {

            if (data.datasets[i].datalabels == "LabelsEnergyLine") {
                data.datasets[i].datalabels = LabelsEnergyLine;
            }
            if (data.datasets[i].datalabels == "LabelsPie") {
                data.datasets[i].datalabels = LabelsPie;
            }
        }
    }
    function AssignLabelsToDataSet(data) {
        for (i = 0; i < data.length; i++) {
            if (data[i].datalabels == "LabelsLineRound") {
                data[i].datalabels = LabelsLineRound;
            }
        }
    }
    var LabelsEnergyLine = {
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
            };
        },
        formatter: function (value, context) {
            return value.toFixed(4);
        }
    };
}