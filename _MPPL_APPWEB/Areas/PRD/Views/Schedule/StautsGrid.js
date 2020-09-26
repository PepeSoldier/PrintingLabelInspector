function StatusGrid(divSelector) {

    var self = this;
    var statuses = GetStatuses();
    var types = GetTypes();
    var thisOrderId = 0;
    var gridHelper = new GridHelper("Status", "/PRD/ScheduleMonitor");

    this.InitGrid = function () {
        $(divSelector).jsGrid({
            width: "100%", inserting: true, editing: true, sorting: false, paging: false, autoload: true, filtering: false,
            controller: gridHelper.DB,
            fields: [
                { name: "Id", title: "Id", type: "text", width: 10, css: "hidden" },
                { name: "OrderId", type: "text", width: 10, css: "hidden", insertValue: OrderId(), insertTemplate: Templt() },
                { name: "StatusName", title: "Typ", type: "select", width: 30, items: types, valueField: "Id", textField: "Name" },
                { name: "StatusState", title: "Status", type: "select", width: 30, items: statuses, valueField: "Id", textField: "Name" },
                { name: "StatusInfo", title: "LOK", type: "text", width: 22 },
                { name: "StatusInfoExtra", title: "ANC", type: "text", width: 35 },
                { name: "StatusInfoExtra2", title: "Nazwa", type: "text", width: 30 },
                { name: "StausInfoExtraNumber", title: "Ilość", type: "text", width: 23 },
                { type: "control", width: 40 }
            ]
        });
    }
    this.RefreshGrid = function (orderId) {
        thisOrderId = orderId;
        gridHelper.SetFilter({ OrderId: orderId });
        $(divSelector).jsGrid("search");
    }

    function EntryType() { return function () { return entryType; }; }
    function OrderId() { return function () { return thisOrderId; }; }
    function Templt() { return $("<td>"); }
    function InsrtHourTemplt(field) { field._insertPicker = $("<input>").val(defaultHour); return field._insertPicker; }
    function InsrtHourVal() { console.log("value hour"); console.log(this._insertPicker.val()); return this._insertPicker.val(); }
    function GetStatuses() {
        return [
            {
                "Id": 0,
                "Name": "Wydr.",
            },
            {
                "Id": 10,
                "Name": "Picking",
            },
            {
                "Id": 50,
                "Name": "Gotowe",
            },
            {
                "Id": 90,
                "Name": "Niekompl.",
            }
        ];
    }
    function GetTypes() {
        return [
            {
                "Id": "MAGA",
                "Name": "MAG A",
            },
            {
                "Id": "MAGB",
                "Name": "MAG B",
            },
            {
                "Id": "TECH",
                "Name": "TECH",
            },
            {
                "Id": "PP",
                "Name": "PODM.P",
            },
            {
                "Id": "PKG",
                "Name": "PODM.KG",
            },
            {
                "Id": "PKD",
                "Name": "PODM.KD",
            }
        ];
    }
}