
function StopSplit(_stoppageId, callback) {

    var self = this;
    var stoppageId = _stoppageId;
    let minVal = 5;

    this.Init = function () {
        let val = parseInt($("#myRange").attr("value"));
        let maxVal = parseInt($("#myRange").attr("max"));

        if (maxVal <= minVal * 2) {
            new Alert().Show("warning", "Postoj zbyt krótki, nie można go podzielić");
            callback();
        }

        $("#myRange").attr("value", val);
        self.UpdateValues(maxVal, val);
        self.updateBoxes(maxVal, val);
        slider.value = val;
    };

    this.Execute = function () {
        console.log("StopSplit.Execute");
        let val = parseInt($("#myRange").attr("value"));
        let maxVal = parseInt($("#myRange").attr("max"));
        let secondsLeft = val;
        let secondsRight = maxVal - val;
        
        let json = new JsonHelper().GetPostData("/ONEPROD/OEEReportOnline/StopSplit", { stoppageId, secondsLeft, secondsRight });
        json.done(function (jsonModel) {
            new Alert().Show(jsonModel.MessageTypeString, jsonModel.Message);
            callback();
        });
        json.fail(function () {
            new Alert().Show("danger", "Operacja nie powiodła się");
            callback();
        });
    };
    this.MoveSliderByButtons = function (_btn) {
        console.log("StopSplit.MoveSliderByButtons");
        let sec = parseInt($(_btn).attr("data-sec"));
        let val = parseInt($("#myRange").attr("value"));
        let maxVal = parseInt($("#myRange").attr("max"));
        let newVal = self.limitValue(maxVal, val + sec); //Math.max(5, Math.min(val + sec, maxVal-5));

        $("#myRange").attr("value", newVal);
        self.UpdateValues(maxVal, newVal);
        self.updateBoxes(maxVal, newVal);
        slider.value = newVal;
    };
    this.limitValue = function (maxVal, val) {
        return Math.max(minVal, Math.min(val, maxVal - minVal));
    };
    this.UpdateValues = function (maxVal, val) {
        let totalSec = val;
        let minutes = parseInt(totalSec / 60);
        let seconds = totalSec - (minutes * 60);
        $("#leftMinutes").text(minutes);
        $("#leftSeconds").text(seconds);

        let totalSec2 = maxVal - val;
        let minutes2 = parseInt(totalSec2 / 60);
        let seconds2 = totalSec2 - (minutes2 * 60);
        $("#rightMinutes").text(minutes2);
        $("#rightSeconds").text(seconds2);
    };
    this.updateBoxes = function (maxVal, val) {

        let percentA = Math.min(98, Math.max(2, val * 100 / maxVal));
        let percentB = Math.min(98, Math.max(2, 100 - percentA));

        $("#boxA").css("width", percentA + "%");
        $("#boxB").css("width", percentB + "%");
    };


    var slider = document.getElementById('myRange');

    $(document).off("click", ".btnChangeSplit");
    $(document).on("click", ".btnChangeSplit", function () {
        self.MoveSliderByButtons(this);
    });

    $(document).off("click", "#btnSplitExecute");
    $(document).on("click", "#btnSplitExecute", function () {
        console.log("btnSplitExecute");
        self.Execute();
    });

    slider.oninput = function () {
        let maxVal = parseInt($(this).attr("max"));
        let val = self.limitValue(maxVal, this.value);

        $("#myRange").attr("value", val);
        self.UpdateValues(maxVal, val);
        self.updateBoxes(maxVal, val);
    };

}

