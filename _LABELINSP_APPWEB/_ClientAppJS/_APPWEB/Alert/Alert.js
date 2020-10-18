function Alert() {

    var self = this;

    this.GetAlerts = function (userName) {
        $.ajax({
            async: true,
            type: "POST",
            url: new BaseUrl().link + "Base/GetAlerts",
            data: { "userName": userName },
            success: function (Alerts) {
                for (i = 0; i < Alerts.length; i++) {
                    console.log(Alerts[i]);
                    self.Show(Alerts[i].MessageTypeName, Alerts[i].Message);
                }
            },
            error: function () { console.log("get alerts error"); }
        });
    };
    this.GetAlertsService = function (userName, timeout) {
        GetAlertAjax(userName, timeout, GetAlertAjax);
    };
    this.Show = function (messageType, messageText, deletePrevious = false) {
        var link1 = "";
        var $alertDiv = $(AlertTemplate(messageType, messageText));

        //if (messageType == "danger")
        //    link1 = new BaseUrl().link + 'content/alert_err1.wav';
        //else
        //    link1 = new BaseUrl().link + 'content/alert4.wav';
        //$.playSound(link1);

        if (deletePrevious == true) {
            $("#AlertDivFixes").html("");
        }

        $("#AlertDivFixes").append($alertDiv);
        MoveOnTop();
        $alertDiv.hide();
        $alertDiv.show();
        $alertDiv.fadeTo(3500, 1000).slideUp(1000, function () {
            $alertDiv.slideUp(1000);
            $alertDiv.remove();
        });
    };
    this.ShowJson = function (jsonModel, deletePrevious = false)
    {
        if (jsonModel != null &&
            jsonModel.MessageType != null && jsonModel.MessageType > 0 &&
            jsonModel.Message != null && jsonModel.Message.length > 0)
        {
            var link1 = "";
            var $alertDiv = $(AlertTemplate(jsonModel.MessageTypeString, jsonModel.Message));

            if (deletePrevious == true) {
                $("#AlertDivFixes").html("");
            }

            $("#AlertDivFixes").append($alertDiv);
            MoveOnTop();
            $alertDiv.hide();
            $alertDiv.show();
            $alertDiv.fadeTo(3500, 1000).slideUp(1000, function () {
                $alertDiv.slideUp(1000);
                $alertDiv.remove();
            });
        }
    };

    function GetAlertAjax(userName, timeout, callback) {
        //console.log("Get Alerts...");
        $.ajax({
            async: true,
            type: "POST",
            url: new BaseUrl().link + "Base/GetAlerts",
            data: { "userName": userName },
            success: function (Alerts) {
                for (i = 0; i < Alerts.length; i++) {
                    console.log(Alerts[i]);
                    self.Show(Alerts[i].MessageTypeName, Alerts[i].Message);
                }
                setTimeout(function () { callback(userName, timeout, GetAlertAjax); }, timeout);
            },
            error: function () {
                console.log("get alerts error");
                setTimeout(function () { callback(userName, timeout, GetAlertAjax); }, 5000);
            }
        });
    }
    function AlertTemplate(messageTypeName, message) {
        var alert =
            '<div class="alert alert-' + messageTypeName + ' alert-dismissable" style="display: none;">' +
                '<button type="button" class="close" data-dismiss="alert">x</button>' +
                 '<span>[' + new moment().format("HH:mm:ss") + '] </span>' +
                 '<span>' + message + '</span>' +
            '</div >'

        return alert;
    }
    (function ($) {
        $.extend({
            playSound: function () {
                return $(
                    '<audio class="sound-player" autoplay="autoplay" style="display:none;">'
                    + '<source src="' + arguments[0] + '" />'
                    + '<embed src="' + arguments[0] + '" hidden="true" autostart="true" loop="false"/>'
                    + '</audio>'
                ).appendTo('body');
            },
            stopSound: function () {
                $(".sound-player").remove();
            }
        });
    })(jQuery);

    function MoveOnTop() {
        $(".alert-fixed").css("z-index", HighestZIndex());
    };
    function HighestZIndex() {
        var maxZ = Math.max.apply(null, $.map($('body > *'), function (e, n) {
            //if ($(e).css('position') == 'absolute')
            return parseInt($(e).css('z-index')) || 1;
        }));

        //console.log("HighestZIndex " + maxZ);
        return maxZ + 1;
    };
}