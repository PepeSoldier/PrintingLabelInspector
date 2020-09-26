
function Init() {
    $(".tm-status").each(function () {

        var $el = $(this);
        var content = $el.text().toLowerCase().replace(/\s+/g, '');;
        tmSetBckgrnd(content, $el);
    });
}

function InitOnClick() {
    $(".tm-status").click(function () {

        var $el = $(this);
        var machineId = $el.attr("id").split("-")[3];
        var toolId = $el.attr("id").split("-")[2];

        var content = $el.text().toLowerCase().replace(/\s+/g, '');
        var status1 = (content * 1 + 1) % 2;
        $el.text(status1);
        tmSetBckgrnd(status1, $el);

        var assigned = $('#mt-Assigned-' + toolId + '-' + machineId + '').text().toLowerCase().replace(/\s+/g, '');
        var preffered = $('#mt-Preffered-' + toolId + '-' + machineId + '').text().toLowerCase().replace(/\s+/g, '');
        var placed = $('#mt-Placed-' + toolId + '-' + machineId + '').text().toLowerCase().replace(/\s+/g, '');

        if (assigned == 0) {
            $('#mt-Preffered-' + toolId + '-' + machineId + '').text("0").css("background-color", "#bdbdbd");
            $('#mt-Placed-' + toolId + '-' + machineId + '').text("0").css("background-color", "#bdbdbd");
        }

        $.ajax({
            url: BaseUrl + "ConfigurationAPS/ToolMachineUpdate",
            data: '{ MachineId: ' + machineId + ', ToolId:' + toolId + ', Assigned: ' + assigned + ', Preffered: ' + preffered + ', Placed: ' + placed + '}',
            type: 'POST',
            contentType: 'application/json; charset=utf-8',
            success: function (data) {
            }
        });
    });
}

function tmSetBckgrnd(content, el) {
    var color = "#bdbdbd";

    //prepare colors
    if (content == 1 && $(el).attr("id").split("-")[1] == "Assigned") {
        color = "#499e41";
    }
    if (content == 1 && $(el).attr("id").split("-")[1] == "Preffered") {
        color = "lightblue";
    }
    if (content == 1 && $(el).attr("id").split("-")[1] == "Placed") {
        color = "orange";
    }

    //put color
    if (color) {
        $(el).css("background-color", color);
    }

    //show hide divs: preffered and placed
    if ($(el).attr("id").split("-")[1] == "Assigned") {
        var machineId = $(el).attr("id").split("-")[3];
        var toolId = $(el).attr("id").split("-")[2];
        content = $(el).text().toLowerCase().replace(/\s+/g, '');

        if (content == "0") {
            $('#mt-Preffered-' + toolId + '-' + machineId + '').hide();
            $('#mt-Placed-' + toolId + '-' + machineId + '').hide();
        }
        else {
            $('#mt-Preffered-' + toolId + '-' + machineId + '').show();
            $('#mt-Placed-' + toolId + '-' + machineId + '').show();
        }
    }

}