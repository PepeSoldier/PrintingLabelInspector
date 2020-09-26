
function WorkplaceGrid(gridDivSelector) {

    var self = this;
    var divSelector = gridDivSelector;
    var gridHelper = new GridHelper("Workplace", "/ONEPROD/ConfigurationMES");
    var machines = new GridHelper("Resource", "/ONEPROD/Configuration").GetList(false, {Type: 20}).responseJSON;
    var printerTypes = GetPrinterTypes();
    var SerialNumberTypes = GetSerialNumberTypes();
    var workplaceTypes = GetWorkplaceTypes();

    this.InitGrid = function (isTraceability, isReportOnline) {
        console.log(machines);
        $(divSelector).jsGrid({
            width: "100%",
            inserting: true, editing: true, sorting: false, paging: false, filtering: false,
            confirmDeleting: false,
            onItemDeleting: function (args) {
                gridHelper.onItemDeletingBehavior(args, divSelector);
            },
            fields: [
                { name: "Id", type: "text", title: "Id", width: 50, filtering: false, editing: false, inserting: false },
                { name: "Name", type: "text", title: "Nazwa", width: 160, filtering: false },
                {
                    name: "Type", type: "select", title: "Typ", width: 80, filtering: false,
                    items: workplaceTypes, valueField: "Id", textField: "Name",
                    itemTemplate: function (value, item) {
                        console.log(workplaceTypes);
                        var m = workplaceTypes.find(x => x.Id == value);
                        return m != null ? m.Name : " - ";
                    }
                },
                { name: "ComputerHostName", type: "text", title: "PC", width: 100, filtering: false },
                { name: "PrinterIPv4", type: "text", title: "IP drukarki", width: 100, filtering: false },
                {
                    name: "MachineId", type: "select", title: "Maszyna", width: 100, filtering: false,
                    items: machines, valueField: "Id", textField: "Name",
                    itemTemplate: function (value, item) {
                        console.log(machines);
                        var m = machines.find(x => x.Id == value);
                        return m != null ? m.Name : " - ";
                    }
                },
                { name: "LabelANC", type: "text", title: "Prod. kod", width: 100, filtering: false },
                { name: "LabelName", type: "text", title: "Nazwa Etyk.", width: 100, filtering: false },
                { name: "PrinterType", title: "Typ Drukarki", type: "select", items: printerTypes, valueField: "Id", textField: "Name", width: 100, filtering: false },
                { name: "SerialNumberType", title: "Typ Nr Ser.", type: "select", items: SerialNumberTypes, valueField: "Id", textField: "Name", width: 100, filtering: false },
                { name: "LabelLayoutNo", type: "text", title: "LayoutNo", width: 50, filtering: false },
                { name: "PrintLabel", type: "checkbox", title: "Druk?", width: 50, filtering: false },
                { name: "IsTraceability", type: "checkbox", title: "trace?", width: 50, filtering: false, css: (isTraceability? "" : "hidden") },
                { name: "IsReportOnline", type: "checkbox", title: "rep.o.?", width: 50, filtering: false, css: (isReportOnline ? "" : "hidden") },

                { name: "Akcje3", type: "control", width: 100, modeSwitchButton: true, editButton: true }
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


    function GetPrinterTypes() {
        return [
            { "Id": 0, "Name": "-" },
            { "Id": 1, "Name": "Laserowa" },
            { "Id": 2, "Name": "Zebra" },
            { "Id": 4, "Name": "CAB" }
        ];
    }
    function GetSerialNumberTypes() {
        return [
            { "Id": 0, "Name": "-" },
            { "Id": 1, "Name": "YWWD5" },
            { "Id": 2, "Name": "D9" },
        ];
    }
    function GetWorkplaceTypes() {
        return [
            { "Id": 0, "Name": "-" },
            { "Id": 10, "Name": "IN" },
            { "Id": 20, "Name": "STANDARD" },
            //{ "Id": 25, "Name": "STANDARD_WITH_QTY_REPORT" },
            { "Id": 30, "Name": "QUALITY_CHECK_POINT" },
            { "Id": 90, "Name": "OUT" },
        ];
    }

    //Undefined = 0,
    //IN = 10,
    //STANDARD = 20,
    //STANDARD_WITH_QTY_REPORT = 25,
    //QUALITY_CHECK_POINT = 30,
    //OUT = 90,

}