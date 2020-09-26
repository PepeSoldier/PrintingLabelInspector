var EnergyCost = function(gridDivSelector) {
    GridBulkUpdate.call(this, gridDivSelector);
    var self = this;
    this.divSelector = gridDivSelector;
    this.gridHelper = new GridHelper("EnergyCost", "/ONEPROD/ConfigurationENERGY");
    this.EnumEnergyType = GetEnumEnergyType();
    this.UnitOfMeasure = GetUnitOfMeasure();

    function GetEnumEnergyType() {
        return [
            { "Id": 0, "Name": "-" },
            { "Id": 10, "Name": "Prąd" },
            { "Id": 20, "Name": "Gaz" },
            { "Id": 30, "Name": "Powietrze" },
            { "Id": 40, "Name": "Ciepło" },
        ];
    }
    function GetUnitOfMeasure() {
        return [
            { "Id": 0, "Name": "-" },
            { "Id": 4, "Name": "kWh" },
            { "Id": 5, "Name": "Metr sześcienny" },
            { "Id": 6, "Name": "Gigadżul" },
        ];
    }
}

EnergyCost.prototype = Object.create(GridBulkUpdate.prototype);
EnergyCost.prototype.constructor = EnergyCost;

EnergyCost.prototype.InitGrid = function () {
    console.log("Init EnergyCost Grid");
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
            //{ name: "Id", type: "text", title: "Id", width: 40, filtering: false, editing: false, inserting: false },
            { name: "StartDate", title: "Data początkowa", type: "date", width: 80 },
            { name: "EndDate", title: "Data końcowa", type: "date", width: 60 },
            { name: "EnergyType", title: "Typ", type: "select", items: this.EnumEnergyType, valueField: "Id", textField: "Name", width: 100 },
            { name: "UnitOfMeasure", title: "Jednostka Pomiaru", type: "select", items: this.UnitOfMeasure, valueField: "Id", textField:"Name"},
            { name: "PricePerUnit", title: "Cena za jednostkę [PLN]", type: "text", width: 100 },
            { name: "kWhConverter", title: "Przelicznik na kWh", type: "text", width: 100 },
            { name: "UseConverter", title: "Włączanie przelicznika do obliczeń", type: "checkbox", width: 100, css: "jsgrid-cell-withinput" },
            { type: "control", width: 50, modeSwitchButton: true, editButton: true, deleteButton: true }
        ],
        controller: this.gridHelper.DB,
        onDataLoaded: function () {
            console.log("grid data loaded");
        }, 
    });
};
EnergyCost.prototype.CreateNewGridInstance = function (divSelector) {
    return new EnergyCost(divSelector);
};
this.RefreshGrid = function () {
  };


