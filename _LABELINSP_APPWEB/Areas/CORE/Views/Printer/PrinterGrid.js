
var PrinterGrid = function (gridDivSelector) {
    console.log("PRinter Types");
    GridBulkUpdate.call(this, gridDivSelector);
    var self = this;
    this.divSelector = gridDivSelector;
    this.gridHelper = new GridHelper("Printer", "/CORE/Printer");
    this.printerTypes = GetPrinterTypes();
    
    function GetPrinterTypes() {
        return [
            { "Id": 0, "Name": "-" },
            { "Id": 1, "Name": "Laser" },
            { "Id": 2, "Name": "Zebra" },
            { "Id": 4, "Name": "CAB" },
            { "Id": 8, "Name": "CITIZEN" }
        ];
    }
}

PrinterGrid.prototype = Object.create(GridBulkUpdate.prototype);
PrinterGrid.prototype.constructor = PrinterGrid;

PrinterGrid.prototype.InitGrid = function () {
    console.log("Init PrinterGrid Grid");
    var grid = this;
    $(this.divSelector).jsGrid({
        width: "100%", height: function () { return $(this).height() > 400 ? ($(this).height()).toString() + "px" : "400px"; },
        inserting: true, editing: true, sorting: false, filtering: true,
        paging: true, pageLoading: true, pageSize: 20,
        confirmDeleting: false,
        onItemDeleting: function (args) {
            grid.gridHelper.onItemDeletingBehavior(args, grid.divSelector);
        },
        fields: [
            { name: "Id", type: "text", title: "Id", width: 40, filtering: false, editing: false, inserting: false },
            { name: "Name", type: "text", title: "Nazwa", width: 100 },
            { name: "Model", type: "text", title: "Model", width: 100 },
            { name: "SerialNumber", type: "text", title: "Numer Seryjny", width: 100 },
            { name: "User", type: "text", title: "Użytkownik", width: 100 },
            { name: "Password", type: "text", title: "Hasło", width: 100 },
            { name: "IpAdress", type: "text", title: "Adres IP", width: 100 },
            { name: "PrinterType", title: "Typ drukarki", type: "select", items: this.printerTypes, valueField: "Id", textField: "Name", width: 100, filtering: true },
            { type: "control", width: 50, modeSwitchButton: true, editButton: true }
        ],
        controller: this.gridHelper.DB,
        onDataLoaded: function () {
            console.log("grid data loaded");
        },
    });
};
PrinterGrid.prototype.CreateNewGridInstance = function (divSelector) {
    return new PrinterGrid(divSelector);
};