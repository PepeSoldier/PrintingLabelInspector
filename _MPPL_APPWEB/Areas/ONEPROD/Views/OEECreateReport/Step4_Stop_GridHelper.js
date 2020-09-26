function StopGridHelper(divSelector, reportId, db, reasons, reasonTypeId, timeColTitle, defaultHour) {

    var self = this;
    var repId = reportId;

    //console.table(reasons);

    this.GenerateGrid = function () {
        $(divSelector).jsGrid({
            width: "100%", inserting: true, editing: true, sorting: false, paging: false, autoload: true, filtering: false,
            controller: db,
            fields: [
                { name: "Id", title: "Id", type: "text", width: 30, css: "hidden" },
                { name: "ReportId", type: "text", width: 40, css: "hidden", insertValue: ReprtId(), insertTemplate: Templt() },
                { name: "ReasonTypeId", type: "number", width: 40, css: "hidden", insertValue: ReasonTypeId(), insertTemplate: Templt() },
                {
                    name: "Hour", title: "Godz.", type: "text", width: 20,
                    insertTemplate: function (val) { return InsrtHourTemplt(this); },
                    insertValue: function () { return this._insertPicker.val(); }
                },
                { name: "ReasonId", title: "Powód", type: "select", items: reasons, valueField: "Id", textField: "Name", width: 80 },
                { name: "Comment", title: "Komentarz", type: "text", width: 50 },
                {
                    name: "UsedTime", title: timeColTitle, type: "number", width: 20,
                    itemTemplate: function (value, item) { return parseFloat((value / 60).toFixed(4)); },
                    insertTemplate: function (val) { return InsrtUsedTimeTemplt(this, 0); },
                    editTemplate: function (val) { return InsrtUsedTimeTemplt(this, val); },
                    insertValue: function () { return this._insertUsedTimePicker.val() * 60; },
                    editValue: function () { return this._insertUsedTimePicker.val() * 60; }
                },
                //{ name: "TimeUrInMinutes", title: "Ur(min)", type: "number", width: 20, filtering: false },
                //{ name: "TimeOptInMinutes", title: "Opt(min)", type: "number", width: 20, filtering: false },
                { type: "control", width: 40 }
            ]
        });
    };

    function ReasonTypeId() { return function () { return reasonTypeId; }; }
    function ReprtId() { return function () { return repId; }; }
    function Templt() { return $("<td>"); }    
    function InsrtHourTemplt(field) {
        field._insertPicker = $("<input>").val(defaultHour);
        return field._insertPicker;
    }    
    function InsrtHourVal() { console.log("value hour"); console.log(this._insertPicker.val()); return this._insertPicker.val(); }

    function InsrtUsedTimeTemplt(field, val) {
        field._insertUsedTimePicker = $("<input>").val(val/60);
        return field._insertUsedTimePicker;
    }
    function InsrtUsedTimeVal() { console.log("value used time"); console.log(this._insertUsedTimePicker.val()); return this._insertUsedTimePicker.val(); }

}