function HeaderStepIndicator() {

    this.Refresh = function (stage) {

        var st = 1;
        while (st < 5) {
            if (st <= stage) {
                window.location.hash = "#ReportStep" + st;

                if (!$("#StepHeader" + st).hasClass("headerBOXdone")) {
                    $("#StepHeader" + st).addClass("headerBOXdone");
                }
            }
            else {
                $("#StepHeader" + st).removeClass("headerBOXdone");
            }
            st++;
        }

        //if (stage == 1) {
        //    window.location.hash = "#ReportStep1";
        //    $("#StepHeader1").toggleClass("headerBOXdone");
        //}
        //else if (stage == 2) {
        //    window.location.hash = "#ReportStep2";
        //    $("#StepHeader2").toggleClass("headerBOXdone");
        //}
        //else if (stage == 3) {
        //    window.location.hash = "#ReportStep3";
        //    $("#StepHeader3").toggleClass("headerBOXdone");
        //}
        //else if (stage == 4) {
        //    window.location.hash = "#ReportStep4";
        //    $("#StepHeader4").toggleClass("headerBOXdone");
        //}
    }
}
