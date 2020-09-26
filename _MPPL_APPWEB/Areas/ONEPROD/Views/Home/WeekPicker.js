function WeekPicker(_elementSelector, inputId) {
    var self = this;
    var elementSelector = _elementSelector;

    this.Init = function () {
        moment.locale('pl');
        DrawDiv();
        DrawDisplay();
        DrawInput();
        DrawButton("next");
        DrawButton("previous");

        GetInput().datetimepicker({
            i18n: { pl: { months: Months(), dayOfWeek: Days() } },
            weeks: true,
            timepicker: false,
            format: 'Y-m-d',
            dayOfWeekStart: 1,
            onSelectDate(ct, $i) {
                console.log("onselectedDate");
                setWeekYear(ct);
            }
        });
        //}).on("change", function (e) {
        //    var zm = moment($(this).val().trim());
        //    setWeekYear(zm);
        //})

        setWeekYear(moment()); // Set initial week & year
    };
    
    // Variables
    //var currentDate = moment();
    var selectedWeek = null;
    var selectedYear = null;
    var selectedMomentCurrentDate = moment();

    // Public functions
    this.getWeek = function () {
        return selectedWeek;
    };
    this.getYear = function () {
        return selectedYear;
    };
    this.getWeekStartDate = function () {
        var currentDate1 = new moment(selectedMomentCurrentDate);
        //console.log("currentDate1");
        //console.log(currentDate1);

        dayOfweek1 = currentDate1.weekday();

        //if first week day is sunday:
        //dayOfweek1 = dayOfweek1 == 0 ? 7 : dayOfweek1;
        //dayOfweek1 = dayOfweek1 - 1;

        //if first week day is monday:
        dayOfweek1 = dayOfweek1 - 1;


        mondayDate = currentDate1.add(-(dayOfweek1 + 1), 'day').startOf('day').hour(6); //.hour(6).minute(0).second(0);
        //console.log("mondayDate");
        //console.log(mondayDate);
        return mondayDate;
    };
    this.getWeekEndDate = function () {
        var currentDate2 = new moment(selectedMomentCurrentDate);

        dayOfweek2 = currentDate2.weekday();

        //if first week day is sunday:
        //dayOfweek2 = dayOfweek2 == 0 ? 7 : dayOfweek2;
        //dayOfweek2 = 7 - dayOfweek2 + 1;

        //if first week day is monday:
        dayOfweek2 = 7 - dayOfweek2 + 1;

        sundayDate = currentDate2.add(+(dayOfweek2 - 1), 'day').startOf('day').hour(6); //.hour(6).minute(0).second(0);
        //console.log("sundayDate");
        //console.log(sundayDate);
        return sundayDate;
    };

    function setWeekYear(currentDate) {
        console.log("Set Week");
        selectedMomentCurrentDate = new moment(currentDate);
        var calendarWeek = selectedMomentCurrentDate.week();
        var year = selectedMomentCurrentDate.year();
        var month = selectedMomentCurrentDate.month();

        selectedWeek = calendarWeek;
        if (month == 11 && calendarWeek == 1) {
            year += 1;
        }
        selectedYear = year;
        
        GetInput().val(selectedMomentCurrentDate.format("YYYY-MM-DD"));
        GetDisplay().val(calendarWeek + "/" + year);
    }
    function AddWeek(element) {
        return element.click(function () {
            var newDate = selectedMomentCurrentDate.add(7, 'days');//.day(7);
            setWeekYear(newDate);
        });
    }
    function SubstractWeek(element) {
        return element.click(function () {
            var newDate = selectedMomentCurrentDate.add(-7, 'days');//.day(-7);
            setWeekYear(newDate);
        });
    }

    function DrawDiv() {
        $(elementSelector).append("<div id='weekPickerWraper' style='width: 320px;'>");
        elementSelector = elementSelector + " #weekPickerWraper";
        $(elementSelector).css("width", "280px");
        $(elementSelector).css("height", "38px");
        $(elementSelector).css("background", "white");
        $(elementSelector).css("border", "1px solid #d0d0d0");
        $(elementSelector).css("border-radius", "5px");
        $(elementSelector).css("padding-top", "0");
        $(elementSelector).css("padding-left", "7px");
    }
    function DrawButton(direction) {
        var button = $("<div class='weekpickerArrow' style='cursor: pointer;'></div>");
        

        if (direction == "next") {
            $element = GetInput().parent();
            button.addClass("next-" + $(elementSelector).attr("id"));
            button.addClass("fas fa-angle-right");
            button.css("padding", "2px 0 0 8px");
            button.css("font-size", "28px");
            button.insertAfter($element);

            //ButtonClickListener("next", button);
            AddWeek(button);
        }
        else if (direction == "previous") {
            $element = GetDisplay().parent();
            button.addClass("previous-" + $(elementSelector).attr("id"));
            button.addClass("fas fa-angle-left");
            button.css("padding", "2px 8px 0 0");
            button.css("font-size", "28px");
            button.insertBefore($element);

            //ButtonClickListener("previous", button);
            SubstractWeek(button);

        }
    }
    function DrawDisplay() {
        $(elementSelector).append("<div style='float: left; height:100%;padding-bottom:1px;'><input id='weekPickerDisplay' type='text' class='form-control text-center' style='width: 105px; border: 0; height:100%;'></div>");

    }
    function DrawInput() {
        $(elementSelector).append("<div style='float: left; height:100%;padding-bottom:1px;'><input id='" + inputId + "' style='width: 105px; border: 0; height:100%;' type='text' class='form-control text-center' spellcheck='false'></div>");
    }
    function GetInput() {
        return $(elementSelector).find("#" + inputId);
    }
    function GetDisplay() {
        return $(elementSelector).find("#weekPickerDisplay");
    }
    
    function Months(){
        return [ 'Styczeń', 'Luty', 'Marzec', 'Kwiecień', 'Maj', 'Czerwiec', 'Lipiec', 'Sierpień', 'Wrzesień', 'Październik', 'Listopad', 'Grudzień'];
    }
    function Days(){
        return ["Nd", "Pn", "Wt", "Śr", "Cz","Pt", "Sb"];
    }
}
