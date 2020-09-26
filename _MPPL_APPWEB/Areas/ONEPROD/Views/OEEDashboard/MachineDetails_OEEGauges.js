function OEEDataProvider() {

    this.data = {
        OeeResult: 0,
        AvailabilityResult: 0,
        PerformanceResult: 0,
        QualityResult: 0
    };

    this.GetData = function (filter) {
        var data1 = ($.ajax({
            async: false, type: "POST", data: filter,
            url: '/ONEPROD/OEE/OEEGetData',
            success: function (data) {
                //console.log(data);
            },
            error: function (xhr, ajaxOptions, thrownError) {
                console.log(thrownError);
            }
        })).responseJSON;

        this.data = data1;

        //this.data.OeeResult = data1.OeeResult * 100;
        //this.data.Availability = data1.Availability * 100;
        //this.data.Performance = data1.Performance * 100;
        //this.data.Quality = data1.Quality * 100;
    };
}

function OEEGauge(divId) {

    var gaugeDefs = {};
    var gaugeObject = {};

    this.Init = function () {
        gaugeDefs = {
            value: 0,
            min: 0,
            max: 100,
            symbol: '%',
            decimals: 2,
            gaugeWidthScale: 1.2,
            levelColors: ["#CE1B21"],
            relativeGaugeSize: true,
            counter: true,
            hideMinMax: true
        };

        gaugeObject = new JustGage({
            id: divId,
            defaults: gaugeDefs
        });
    };
    this.InitCompare = function () {
        gaugeDefs = {
            value: 0,
            min: 0,
            max: 100,
            symbol: '%',
            decimals: 2,
            gaugeWidthScale: 1.2,
            levelColors: ["#CE1B21"],
            relativeGaugeSize: true,
            counter: true,
            hideMinMax: true,
            hideValue: true
        };

        gaugeObject = new JustGage({
            id: divId,
            defaults: gaugeDefs
        });
    };
    this.InitAPQ = function () {
        gaugeDefs = {
            value: 0,
            min: 0,
            max: 100,
            symbol: '%',
            donut: true,
            decimals: 1,
            gaugeWidthScale: 0.8,
            levelColors: ["#CE1B21", "#D0532A", "#FFC414", "#00FF00"],
            //textRenderer: function (val) { return ''; },
            relativeGaugeSize: true,
            counter: true

        };

        gaugeObject = new JustGage({
            id: divId,
            defaults: gaugeDefs
        });
    };
    this.Refresh = function (value, target) {
        //console.log("gauge value");
        value *= 100;
        target *= 100;
        //console.log(gaugeObject);
        if (value < target) {
            gaugeObject.config.levelColors = ["#CE1B21"]; //ORANGE["#FFC414"];
        }
        else {
            gaugeObject.config.levelColors = ["#36f204"];
        }
        gaugeObject.putTarget(target, "#cccccc");
        gaugeObject.refresh(value);
    }
}