//iLOGIS ItemGrid

var ItemGrid = function (gridDivSelector) {
    console.log("itemGridInit");
    GridBulkUpdate.call(this, gridDivSelector);
    var self = this;
    this.divSelector = gridDivSelector;
    this.gridHelper = new GridHelper("Item2", "/iLOGIS/Config");
    this.types = GetTypes();
    this.UnitOfMeasures = UnitOfMeasuresList();

    function GetTypes() {
        return [
            { "Id": 0, "Name": "-" },
            { "Id": 10, "Name": "Surowiec" },
            { "Id": 20, "Name": "Pozycja Zakupowa" },
            { "Id": 30, "Name": "Pozycja Pośrednia" },
            { "Id": 40, "Name": "Półfabrykat" },
            { "Id": 50, "Name": "Wyrób Gotowy" },
            { "Id": 90, "Name": "Grupa" }
        ];
    }
};

ItemGrid.prototype = Object.create(GridBulkUpdate.prototype);
ItemGrid.prototype.constructor = ItemGrid;

ItemGrid.prototype.InitGrid = function () {
    var bulkUpdate1 = this.bulkUpdateItem;
    var ids1 = this.idsTable;
    $(this.divSelector).jsGrid({
        width: "100%", height: function () { return $(this).height() > 400 ? ($(this).height()).toString() + "px" : "400px"; },
        bulkUpdate: bulkUpdate1, ids: ids1,
        inserting: false, editing: true, sorting: false,
        paging: true, pageLoading: true, pageSize: 20,
        confirmDeleting: false, filtering: true,
        fields: [
            { name: "Id", type: "text", title: "Id", width: 40, filtering: false, editing: false },
            { name: "Code", type: "text", title: "Kod", width: 75, editing: false, css: "textWhiteBold" },
            { name: "Name", type: "text", title: "Nazwa", width: 200, editing: false, css: "textBlue"  },
            { name: "ItemGroupId", type: "text", title: "Gr.Id", width: 40 },
            { name: "ItemGroupName", type: "text", title: "Grupa", width: 150 },
            {
                name: "UnitOfMeasure", type: "text", title: "JM", width: 50, editing: false, filtering: false,
                itemTemplate: function (value, item) { return ConvertUoM(value); }
            },
            { name: "UnitOfMeasure", type: "select", title: "JM2", width: 50, editing: false, filtering: true, items: this.UnitOfMeasures, valueField: "Id", textField: "Name" },
            { name: "DEF", type: "text", title: "DEF", width: 50, editing: false },
            { name: "BC", type: "text", title: "BC", width: 50, editing: false },
            { name: "H", type: "text", title: "H", width: 50, editing: false },
            { name: "PREFIX", type: "text", title: "PREFIX", width: 100, editing: false },
            { name: "PickerNo", type: "text", title: "Nr Pickera", width: 50 },
            { name: "TrainNo", type: "text", title: "Nr Pociągu", width: 50 },
            { name: "Weight", type: "text", title: "Waga", width: 50 },
            { name: "Type", title: "Typ", type: "select", items: this.types, valueField: "Id", textField: "Name", width: 100, filtering: true },
            { name: "StartDate", type: "text", title: "Data St.", width: 100, editing: false },
            {
                width: 40, filtering: false, editing: false, inserting: false,
                itemTemplate: function (value, item) {
                    return $("<button>").html('<i class="fas fa-history"></i>')
                        .addClass("btn btn-sm btn-info btnShowChangeLog")
                        .on("click", function () { new PFEPChangeLog().ShowLogByObjectId("ItemWMS", item.Id); });
                }
            },
            this.ManageColumn(),
            { name: "Akcje2", type: "control", width: 100, modeSwitchButton: true, editButton: true }
        ],
        controller: this.gridHelper.DB,
        onDataLoaded: function () { },
        //rowClick: function (args) { }
    });
    this.grid = $(this.divSelector).data("JSGrid");
};
ItemGrid.prototype.CreateNewGridInstance = function (divSelector) {
    return new ItemGrid(divSelector);
}