//---------------------------------------------------------------------:)

function Dashboard() {
    var baseUrl = new BaseUrl().link + 'AP/';
    var chartTitles = []; 
    var chartDivs = []; 
    var chartPersonalFilters = [];
    var timeVal = 0;
    var personalVal = 0;
    var activityDiv = "";   
    var activityLogPages = 5;

    this.loadControls = function (div) {
        $(div).load(baseUrl + "Dashboard/Controlls");
    }
    this.AddChart = function (chartDiv,chartTitle, chartPersonalFilter) {
        chartTitles.push(chartTitle);
        chartDivs.push(chartDiv);
        chartPersonalFilters.push(chartPersonalFilter);
    }
    this.AddActivityLog = function (activityLogDivId, pages) {
        activityDiv = activityLogDivId;
        activityLogPages = pages;
    }
    this.Refresh = function (timeVal1, personalVal1) {
        timeVal = timeVal1;
        personalVal = personalVal1;

        $(activityDiv).html('');
        RefreshChart(0);
    }
    this.LoadActivityLog = function (startRow, rowsCount) {
        LoadActivityLog(startRow, rowsCount);
    }

    function RefreshChart(i) {
        
        if (i < chartDivs.length)
        {   
            $(chartDivs[i]).html('');
            $.post(baseUrl + "Dashboard/PieChart", { TimeFilter: timeVal, PersonalFilter: chartPersonalFilters[i], ChartTitle: chartTitles[i] })
                .done(function (data) {
                    $(chartDivs[i]).html(data);
                    RefreshChart(i + 1);
                });
        }
        else {
            $("#chartLegend").fadeIn(600);
            setTimeout(function () {
                LoadActivityLog(0, activityLogPages);
            }, 600);
        }
    }
    function LoadActivityLogLoop(startRow, rowsCount){
        var currentRow = startRow;
        while (currentRow <= rowsCount) {
            LoadActivityLog(currentRow, 1)
            currentRow++;
        }
    }
    function LoadActivityLog(startRow, rowsCount) {
        if (activityDiv != "") {
            $.post(baseUrl + "Dashboard/ActivitiesLog", { TimeFilter: timeVal, PersonalFilter: personalVal, LogCurrentRow: startRow, LogLoadRows: rowsCount })
                .done(function (data) {
                    $(activityDiv).append(data);
                    //$(data).hide().appendTo(activityDiv).fadeIn(2000);
                });
        }
    }
}    

function DashboardChart()
{
    var baseUrl = new BaseUrl().link + 'AP/';
    this.DrawPieChart = function (canvasId, ChartData) {

        var chartFontSize = 16; //$(".mainChart").width() / 25;

        var pieOptions = {
            segmentShowStroke: false,
            onClick: function () {
                var state = this.active[0]._model.label;
                var departmentId = this.active[0]._chart.config.data.datasets[0].departmentId;
                var userId = this.active[0]._chart.config.data.datasets[0].userId;
                var chartTitle = this.active[0]._chart.config.data.datasets[0].name;
                var dateFrom = this.active[0]._chart.config.data.datasets[0].dateFrom;
                var dateFromStr = "&dateFrom=" + dateFrom;

                //alert("Nazwa : " + this.active[0]._model.label + " \n " +
                //    "ID: " + this.active[0]._index + " \n" +
                //    "Wartosc: " + this.active["0"]._chart.config.data.datasets[0].data[this.active[0]._index] + " \n" +
                //    "ChartTitle: " + chartTitle
                //);
                
                if (chartTitle == "Przypisane do mojego działu"){
                    window.open(baseUrl + "Action/Browsenew?states=" + state + dateFromStr + "&departmentId=" + departmentId);
                }
                else if (chartTitle == "Przypisane do mnie") {
                    window.open(baseUrl + "Action/Browsenew?states=" + state + dateFromStr + "&assignedId=" + userId);
                }
                else if (chartTitle == "Moje") {
                    window.open(baseUrl + "Action/Browsenew?states=" + state + dateFromStr + "&creatorId=" + userId);
                }
                else {
                    window.open(baseUrl + "Action/Browsenew?states=" + state + dateFromStr);
                }
            },
            animateScale: true,
            legend: {
                display: false,
                position: 'bottom'
            },
            pieceLabel: {
                mode: 'percentage', // mode 'label', 'value' or 'percentage', default is 'percentage'
                precision: 0,       // precision for percentage, default is 0, higher precision is 10
                fontSize: chartFontSize, // font size, default is defaultFontSize
                fontColor: '#fff',  // font color, default is '#fff'
                fontStyle: 'normal',// font style, default is defaultFontStyle
                fontFamily: "'Helvetica Neue', 'Helvetica', 'Arial', sans-serif",   // font family, default is defaultFontFamily
                arc: false,         // draw label in arc, default is false
                position: 'border', // position to draw label, available value is 'default', 'border' and 'outside' // default is 'default'
            }
        }
        console.log(ChartData);
        var dataToChartAll = {
            labels: ChartData.data.map(function (a) { return a.name }),
            datasets: [
                {
                    data: ChartData.data.map(function (a) { return a.value }),
                    backgroundColor: ChartData.data.map(function (a) { return a.color }),
                    name: ChartData.chartType,
                    departmentId: ChartData.user.DepartmentId,
                    userId: ChartData.user.Id,
                    dateFrom: ChartData.dateFrom
                }]
        };

        var myPieChartAll = new Chart($(canvasId), {
            type: 'pie',
            data: dataToChartAll,
            options: pieOptions
        });
    };

    function postURL(url, multipart) {
        var form = document.createElement("FORM");
        form.method = "POST";
        if (multipart) {
            form.enctype = "multipart/form-data";
        }
        form.style.display = "none";
        document.body.appendChild(form);
        form.action = url.replace(/\?(.*)/, function (_, urlArgs) {
            urlArgs.replace(/\+/g, " ").replace(/([^&=]+)=([^&=]*)/g, function (input, key, value) {
                input = document.createElement("INPUT");
                input.type = "hidden";
                input.name = decodeURIComponent(key);
                input.value = decodeURIComponent(value);
                form.appendChild(input);
            });
            return "";
        });
        form.submit();
    }
}