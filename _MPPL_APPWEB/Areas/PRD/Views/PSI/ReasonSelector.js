function ReasonSelector2(containerId, callback) {

    var reasons = GetReasons();
    var self = this;
    var containerEl = containerId;

    $(containerEl).on("click", ".reason1, .reason2", function () {
        console.log("reason1 click");
        self.ShowReasons($(this).attr("id").split("_")[1]);
    });

    $(containerEl).on("click", ".selectReason", function () {
        var id = $(this).attr("id").split("_")[1];
        self.SelectReason(id);
    });

    function GetReasons() {
        var resosns1;
        $.ajax({
            async: false,
            global: false,
            url: "/PRD/PSI/GetReasons",
            dataType: "json",
            type: "GET",
            success: function (data) {
                resosns1 = data;
            }
        });
        return resosns1;
    }

    this.ShowReasons = function (parentId) {
        console.log("pokaz rizony");
        var content = "";
        var back = 0;
        var subReasons = 0;
        $(containerEl).html("");

        for (i = 0; i < reasons.length; i++) {
            content = "";
            if (reasons[i].ParentId == parentId) {
                content += '<div class="resonbox">';
                content += '<div class="reason1" id="reason_' + reasons[i].Id + '">' + reasons[i].Name + '</div>';
                content += '<div class="selectReason" id="selectReson_' + reasons[i].Id + '">wybierz</div>'
                content += '</div>';
                $(containerEl).append(content);
                subReasons++;
            }
            if (reasons[i].Id == parentId) {
                back = reasons[i].ParentId;
            }
        }

        if (!(subReasons > 0)) {
            self.SelectReason(parentId);
        }
        else {
            if (parentId > 0) {
                content = "";
                content += '<div class="row">';
                content += '<div class="col-12 reason2" id="reason_' + back + '">';
                content += " << powrót ";
                content += '</div></div>';
                $(containerEl).append(content);
            }
        }
    }
    this.SelectReason = function (reasonId) {
        //$('#overlay, #overlay-back').fadeOut();
        callback(reasonId);
    }

}

