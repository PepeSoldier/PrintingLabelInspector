
function WarehouseLocationType(gridDivSelector) {

    var self = this;
    var divSelector = gridDivSelector;
    var gridHelper = new GridHelper("WarehouseLocationType", "/iLogis/Config");
    var WarehouseLocationTypeEnum = GetWLType();

    this.InitGrid = function () {
        $(divSelector).jsGrid({
            width: "100%", height: function () { return $(this).height() > 400 ? ($(this).height()).toString() + "px" : "400px"; },
            inserting: true, editing: true, sorting: false, filtering: true,
            paging: true, pageLoading: true, pageSize: 20,
            confirmDeleting: false,
            onItemDeleting: function (args) {
                gridHelper.onItemDeletingBehavior(args, divSelector);
            },
            fields: [
                { name: "Id", type: "text", title: "Id", width: 30, filtering: false, editing: false, inserting: false },
                { name: "Name", type: "text", title: "Name", width: 80, css: "textWhiteBold text-left pl-2", filtering: true, editing: true, inserting: true },
                { name: "Description", type: "text", title: "Description", width: 150, filtering: true, editing: true, inserting: true },
                { name: "TypeEnum", title: "Typ", type: "select", items: WarehouseLocationTypeEnum, valueField: "Id", textField: "Name", width: 80, css: "textGreen", filtering: true },
                { name: "Width", type: "text", title: "Width", width: 30, filtering: true },
                { name: "Height", type: "text", title: "Height", width: 30, filtering: true },
                { name: "Depth", type: "text", title: "Depth", width: 30, filtering: true },
                { name: "MaxWeight", type: "text", title: "Max Weighth [kg]", width: 60, css: "textPink", filtering: true },
                { name: "DisplayFormat", type: "text", title: "DisplayFormat", width: 80, css: "", filtering: true, editing: true, inserting: false },
                { type: "control", width: 100, modeSwitchButton: true, editButton: true }
            ],
            controller: gridHelper.DB,
            onDataLoaded: function () {
                console.log("grid data loaded");
            },
            rowClick: function (args) { }
        });
    };
    this.RefreshGrid = function () {
        $(divSelector).jsGrid("search");
    };

    function GetWLType() {
        return [
            {
                "Id": -1,
                "Name": "Undefined"
            },
            {
                "Id": 0,
                "Name": "Shelf"
            },
            {
                "Id": 20,
                "Name": "OnFloor"
            },
            {
                "Id": 30,
                "Name": "Trolley"
            },
            {
                "Id": 50,
                "Name": "Flow Rack"
            },
            {
                "Id": 90,
                "Name": "Feeder"
            }
        ];
    }
}