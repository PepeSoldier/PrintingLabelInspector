

function CalculationAlert(guid1)
{
    var guid = guid1;
    var alertBoxes;

    this.GetAlerts = function() {
        console.log("getAlerts...");
        $.ajax({
            url: BaseUrl + "ONEPROD/Base/GetAlerts",
            type: 'POST',
            data: "{Guid: '" + guid + "'}",
            contentType: 'application/json; charset=utf-8',
            success: function (result) {
                for (var r = 0; r < result.length; r++) {
                    if (result[r].Id >= 0) {
                        findAlertBox(result[r].Id).ShowAlert(result[r].Message, result[r].Status, result[r].Id, result[r].Type, result[r].DateTime);
                    }
                }
            }
        });
    }
    function findAlertBox(id) {

        if (typeof alertBoxes !== 'undefined') {
            for (i = 0; i < alertBoxes.length; i++) {
                if (alertBoxes[i].Id == id) {
                    return alertBoxes[i];
                }
            }
        }
        else {
            alertBoxes = new Array();
        }

        var ab = new AlertBox(id);
        alertBoxes.push(ab);
        return ab;
    };
}

function AlertBox(id)
{
    var self = this;
    //var cpb = new Progressbar("#asb_" + id, "pbar" + id);

    this.Id = id;

    this.ShowAlert = function (Message, Status, Id, Type, DateTime1) {
        if (Type == 1)
        {
            var el = $('#status1').find("#asbTxt_" + Id);

            if (el.length > 0) {
                $(el[0]).text(Message + " " + Status);
                //cpb.Update(Status.replace("%", "").replace(",", "."));
                if (Status == "100%")
                    //$(el[0]).css('background-color', '#e4f7e7');
                    $(el[0]).removeClass('bgGray1');
                    $(el[0]).addClass('bgGreen');
            }
            else {
                $('#status1').append(
                    '<div class="algStatusBox" id="asb_' + Id + '">' +
                        '<div class="asbTxt bgGray1" id="asbTxt_' + Id + '"></div>' +
                    '</div>'
                );
                //cpb.Show();
                self.ShowAlert(Message, Status, Id);
            }
        }
        else {
            let datetimeFormatted = ""; 

            if (DateTime1 != null) {
                datetimeFormatted = new moment(DateTime1).format('YYYY-MM-DD HH:mm:ss');
            }
            else {
                datetimeFormatted = new moment(new Date()).format('YYYY-MM-DD HH:mm:ss');
            }

            $('#status2').append("<span class='LogDate'>" + datetimeFormatted + "</span>: " + Message + " " + Status);
            $('#status2').append("<br>");
        }
    }
}