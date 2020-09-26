function WorkingHoursPreview(dateElementSelector, shiftElementSelector) {

    var self = this;
    var dateFrom = "";
    var dateTo = "";

    this.RefreshHours = function (resultElementSelector) {
        self.CalculateStartAndEndDate();
        $(resultElementSelector).val(dateFrom + " - " + dateTo);
    }
    this.CalculateStartAndEndDate = function () {
        var date = GetDate();
        console.log("date: " + date);
        var dateNext = new Date(date);
        var shift = GetShift();
        var hourFrom = "";
        var hourTo = "";

        if (shift == 1) {
            hourFrom = "06:00";
            hourTo = "14:00";    
        }
        else if (shift == 2) {
            hourFrom = "14:00";
            hourTo = "22:00";
        }
        else {
            hourFrom = "22:00";
            hourTo = "06:00";
            dateNext.setDate(dateNext.getDate() + 1);
        }

        dateFrom = date.getFullYear() + '-' + (zero(date.getMonth() + 1)) + '-' + zero(date.getDate()); //date.toISOString().slice(0, 10);
        dateTo = dateNext.getFullYear() + '-' + (zero(dateNext.getMonth() + 1)) + '-' + zero(dateNext.getDate()); //dateNext.toISOString().slice(0, 10);

        console.log("dateFrom:" + dateFrom);
        console.log("dateTo:" + dateTo);

        dateFrom += " " + hourFrom;
        dateTo += " " + hourTo;

        return { dateFrom, dateTo };
    }
    this.GetShiftStartHour = function () {
        var shift = GetShift();

        if (shift == 1) {
            return "06:00";
        }
        else if (shift == 2) {
            return "14:00";
        }
        else {
            return "22:00";
        }
    }

    function GetDate() {
        console.log("sel date: " + $(dateElementSelector).val());
       return new Date($(dateElementSelector).val())
    }
    function GetShift() {
        return $(shiftElementSelector).val()
    }
    function zero(number) {
        return number = (number < 10) ? "0" + number : number;
    }
}