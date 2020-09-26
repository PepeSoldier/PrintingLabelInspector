function RefreshChart(data, elementId) {

    $('#' + elementId).html("");
    $('#' + elementId).empty();
    $('#' + elementId).append('<canvas id="Canvas' + elementId + '"></canvas>');

   // AssignLabelsToDataSet(data);
    var dataSet = [];

    var dataTarget = data.datasets[dataSetIndex - 1];
    var dataResults = data.datasets[dataSetIndex];

    //if (dataTarget.data.length < 2) {
    //    ExtendDataset(dataTarget, true);
    //    ExtendDataset(dataResults);

    //    data.labels[1] = data.labels[0];
    //    data.labels[0] = "";
    //    data.labels[2] = "";
    //}

    dataSet.push(dataTarget);
    dataSet.push(dataResults);

    var trendline = { style: "rgba(145,105,180, 0.8)", width: 2 }
    dataSet[1].trendlineLinear = trendline;

    var myChart = new Chart('Canvas' + elementId, {
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
var chartGroup = { name: "", charts: [] }


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
        return value.toFixed(1).replace('.0', '');
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
        console.log(value);
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