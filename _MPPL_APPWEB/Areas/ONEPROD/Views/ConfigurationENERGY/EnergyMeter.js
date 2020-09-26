var EnergyMeter = function(gridDivSelector) {
    GridBulkUpdate.call(this, gridDivSelector);
    var self = this;
    this.divSelector = gridDivSelector;
    this.gridHelper = new GridHelper("EnergyMeter", "/ONEPROD/ConfigurationENERGY");
    this.EnumEnergyType = GetEnumEnergyType();
    this.UnitOfMeasure = GetUnitOfMeasure();
    this.machines = new GridHelper("Resource", "/ONEPROD/Configuration").GetList(false, null).responseJSON;
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

EnergyMeter.prototype = Object.create(GridBulkUpdate.prototype);
EnergyMeter.prototype.constructor = EnergyMeter;

EnergyMeter.prototype.InitGrid = function () {
    console.log("Init EnergyMeter Grid");
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
            { name: "Name", type: "text", title: "Nazwa", width: 60},
            { name: "MarkedName", type: "text", title: "Oznaczenie", width: 80},
            { name: "Description", type: "text", title: "Opis", width: 80, filtering:false},
            {
                name: "ResourceId", type: "select", title: "Maszyna", width: 100, filtering:false,
                items: this.machines, valueField: "Id", textField: "Name",
                itemTemplate: function (value, item) {
                    var m = this.items.find(x => x.Id == value);
                    return m != null ? m.Name : " - ";
                }
            },
            { name: "EnergyType", title: "Typ", type: "select", items: this.EnumEnergyType, valueField: "Id", textField: "Name", width: 100},
            { name: "UnitOfMeasure", title: "Jednostka Pomiaru", type: "select", items: this.UnitOfMeasure, valueField: "Id", textField: "Name", width: 100 },
            { type: "control", width: 50, modeSwitchButton: true, editButton: true, deleteButton: true }
        ],
        controller: this.gridHelper.DB,
        onDataLoaded: function () {
            console.log("grid data loaded");
        },
    });
};
EnergyMeter.prototype.CreateNewGridInstance = function (divSelector) {
    return new EnergyMeter(divSelector);
};
this.RefreshGrid = function () {
    $(divSelector).jsGrid("search");
};


