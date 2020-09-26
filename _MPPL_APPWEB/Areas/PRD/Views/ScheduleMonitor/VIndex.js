var orderCount = 32;
var line = "";
var statusesInternal = [];
var statusesExternal = [];

function Thread(interval) {
    RefreshScheduleMonitor();
    var x = setInterval(function () {
        RefreshScheduleMonitor();
    }, interval);
}
function InitScheduleMonitor(_line, _statusesInternal, _statusesExternal) {
    line = _line;
    statusesInternal = _statusesInternal;
    statusesExternal = _statusesExternal;

    $("#orders").html("");
    for (i = 0; i < orderCount; i++) {

        $("#orders").append('<div class="col-12 colNP" id="orderBox_' + i + '">');
        if (i < 4) {
            LoadOrderLiteBoxes("#orderBox_" + i);
        }
        else {
            LoadOrderBoxes("#orderBox_" + i);
        }
    }
}
function LoadOrderBoxes(selector) {
    $.get("/PRD/ScheduleMonitor/VIndexOrder?index=" + i, function (html) {
        $(selector).append(html);
    });
}
function LoadOrderLiteBoxes(selector) {
    $.get("/PRD/ScheduleMonitor/VIndexOrderLite?index=" + i, function (html) {
        $(selector).append(html);
    });
}

function RefreshScheduleMonitor() {
    console.log("Refresh...");
    var filter = {};
    filter.StartDate = new moment(Date()).format('YYYY-MM-DD HH:mm:ss'); //"2019-01-18 0:00"; //
    filter.Line = line;

    new JsonHelper().GetPostData("/PRD/ScheduleMonitor/GetSchedule", filter)
        .done(function (scheduleMonitorOrderViewModel) {
            $("#lastUpdate").text(moment(Date()).format("HH:mm:ss"));

            let data = scheduleMonitorOrderViewModel;

            $('.redLine').remove();

            for (i = 0; i < orderCount; i++) {
                $("#orderBox_" + i + " .boxBlock").fadeOut(10);

                PutValues(i, data);
                SetupRedLine(i, data);
                RefreshStatusesInternal(i, data);
                RefreshStatusesExternal(i, data);

                $("#orderBox_" + i + " .boxBlock").fadeIn(10);
            }
        });
}

function SetupRedLine(i, data) {
    let totalMinutes = 0;
    let orderMinutes = 0;
    let shifts = 0;
    let overflow = false;
    let nowDate = new moment(new Date());
    let diff = {};

    if (data[i] != null) {
        //console.log("data"); console.log(data[i]);
        if (moment(data[i].StartDate) > nowDate) {
            let oSt = moment(data[i].StartDate) > nowDate ? moment(data[i].StartDate) : nowDate;
            let oEnd = moment(data[i].EndDate);
            diff = null;
            diff = moment.duration(oSt.diff(oEnd));
            //var mmn = oSt.diff(oEnd, 'minutes');
            orderMinutes = 0;
            orderMinutes = diff._milliseconds / (1000 * 60);
            shifts = parseInt(orderMinutes / 480);
            totalMinutes += orderMinutes - shifts * 480;
        }
        //console.log(totalMinutes);

        if (Math.abs(totalMinutes) > 480 && overflow == false) {
            console.log("Przekroczono X minut");
            overflow = true;
            $("#orderBox_" + i).append('<div class="redLine"></div>');
            $("#gradient2").css("height", ((i - 3) * 52) + "px");
            $("#gradient1").css("height", (1460 - (i - 3) * 52) + "px");
        }
    }
}
function PutValues(i, data) {
    if (data[i] != null) {
        $("#orderBox_" + i).find(".boxOrderNo").html(SplitText(data[i].OrderNo, 3));
        $("#orderBox_" + i).find(".boxPNC").html(SplitText(data[i].PNC, 3));
        $("#orderBox_" + i).find("#date").html(moment(data[i].StartDate).format("ddd DD/MM"));
        $("#orderBox_" + i).find(".boxHour").html(moment(data[i].StartDate).format("HH:mm"));
        $("#orderBox_" + i).find("#QtyPlanned").html(data[i].QtyPlanned);
        $("#orderBox_" + i).find("#QtyIn").html(data[i].QtyIN);
        $("#orderBox_" + i).find("#QtyOut").html(data[i].QtyOUT);

        if (data[i].QtyIN < 1) {
            //console.log("qty in < 1");
            $("#orderBox_" + i).find("#QtyIn").css("color", "#2f3f41");
        }
        else {
            $("#orderBox_" + i).find("#QtyIn").css("color", "");
        }

        if (data[i].QtyOUT < 1) {
            $("#orderBox_" + i).find("#QtyOut").css("color", "#2f3f41");
        }
        else {
            $("#orderBox_" + i).find("#QtyOut").css("color", "");
        }
    }
    else {
        console.log("data null");
        console.log(data[i]);
        $("#orderBox_" + i).find(".boxOrderNo").html("");
        $("#orderBox_" + i).find(".boxPNC").html("");
        $("#orderBox_" + i).find("#date").html("");
        $("#orderBox_" + i).find(".boxHour").html("--:--");
        $("#orderBox_" + i).find("#QtyPlanned").html("");
        $("#orderBox_" + i).find("#QtyIn").html("");
        $("#orderBox_" + i).find("#QtyOut").html("");
    }
}
function RefreshStatusesInternal(i, data) {

    //$("#orderBox_" + i).find("#stateMAGA").html("");
    //$("#orderBox_" + i).find("#stateMAGB").html("");

    for (let i = 0; i < statusesInternal.length; i++) {
        $("#orderBox_" + i).find("#state" + statusesInternal[i]).html("");;
    }

    $("#orderBox_" + i).find(".missItemCode").text('');
    $("#orderBox_" + i).find(".missItemName").text('');
    $("#orderBox_" + i).find(".missQty").text('');
    $("#orderBox_" + i + " .StatusInfoExtra").text("");


    if (i < data.length && data[i].Statuses != null) {
        console.log("woId: " + data[i].OrderId)
        var statusName = "";
        var statusInfo = "";
        var statusCount = 0;
        var elStatus;

        for (k = 0; k < data[i].Statuses.length; k++) {
            statusName = data[i].Statuses[k].StatusName;
            statusInfo = data[i].Statuses[k].StatusInfoExtra2;

            //PICKING STATUS
            if (statusesInternal.filter(x => x == statusName).length > 0)
            {
                elStatus = $("#orderBox_" + i).find("#state" + statusName);
                exists = $(elStatus).find("div:contains('" + data[i].Statuses[k].StatusInfo + "')");

                if (exists.length == 0) {

                    text = '';
                    if (data[i].Statuses[k].StatusState == 0) {
                        text = '<div style="font-size: 38px;padding-top:10px;" class="fas fa-print"></i>';
                    }
                    else if (data[i].Statuses[k].StatusState == 10 && data[i].Statuses[k].StatusInfo == "") {
                        text = '<div style="font-size: 38px;padding-top:10px;" class="fas fa-dolly"></i>';
                    }
                    else {
                        text = data[i].Statuses[k].StatusInfo;
                    }

                    $(elStatus).append(
                        '<div class="boxState state' + data[i].Statuses[k].StatusState + '">' +
                        PutStatus(text) +
                        '</div>'
                    );
                }
                else {
                    $(exists).addClass('state' + data[i].Statuses[k].StatusState);
                }

                //dodaj info o kodzie, którego brakuje
                if (data[i].Statuses[k].StatusState == 90) {
                    $("#orderBox_" + i).find(".missItemCode").text(data[i].Statuses[k].StatusInfoExtra);
                    $("#orderBox_" + i).find(".missItemName").text(data[i].Statuses[k].StatusInfoExtra2);
                    $("#orderBox_" + i).find(".missQty").text(data[i].Statuses[k].StausInfoExtraNumber);
                }

                //dopasuj klasę do ilości statusów
                var count = $(elStatus).find(".boxState").length;
                $(elStatus).find(".boxState").removeClass("boxStatus*");
                $(elStatus).find(".boxState").addClass("boxStatus" + count);
            }
        }
    }
}
function RefreshStatusesExternal(i, data) {

    //$("#orderBox_" + i).find("#stateTECHINNERDOO").removeClassPrefix("state");
    //$("#orderBox_" + i).find("#stateTECHOUTERDOO").removeClassPrefix("state");
    //$("#orderBox_" + i).find("#stateTECHTUB").removeClassPrefix("state");
    //$("#orderBox_" + i).find("#stateTECHSIDEPANE").removeClassPrefix("state");
    //$("#orderBox_" + i).find("#statePKD").removeClassPrefix("state");
    //$("#orderBox_" + i).find("#statePKG").removeClassPrefix("state");
    //$("#orderBox_" + i).find("#statePP").removeClassPrefix("state");

    for (let i = 0; i < statusesExternal.length; i++) {
        $("#orderBox_" + i).find("#state" + statusesExternal[i]).removeClassPrefix("state");
    }

    if (i < data.length && data[i].Statuses != null) {
        var statusName = "";
        var statusInfoExtra2 = "";
        var statusCount = 0;
        var el;

        for (k = 0; k < data[i].Statuses.length; k++) {
            statusName = data[i].Statuses[k].StatusName;
            statusInfoExtra2 = data[i].Statuses[k].StatusInfoExtra2;

            if (statusesExternal.filter(x=>x == statusName).length > 0) {
               
                console.log(statusInfoExtra2);
                $("#orderBox_" + i).find("#state" + statusName + statusInfoExtra2).addClass('state' + data[i].Statuses[k].StatusState);
                $("#orderBox_" + i).find("#state" + statusName + statusInfoExtra2 + " .StatusInfoExtra").text("[" + data[i].Statuses[k].StausInfoExtraNumber + "]");
            }
        }
    }
}
function PutStatus(text = '') {
    return "<div>" + text + "</div>";
}
function SplitText(text, chars) {
    rText = "";
    for (j = 0; j < text.length; j += chars) {
        rText += text.substring(j, j + chars);
        rText += " ";
    }
    return rText;
}