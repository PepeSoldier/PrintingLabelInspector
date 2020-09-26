
var ItemMeasurement = function (gridDivSelector) {
    GridBulkUpdate.call(this, gridDivSelector);
    var self = this;
    this.divSelector = gridDivSelector;
    this.gridHelper = new GridHelper("ItemMeasurement", "/ONEPROD/Quality");
};

ItemMeasurement.prototype = Object.create(GridBulkUpdate.prototype);
ItemMeasurement.prototype.constructor = ItemMeasurement;

ItemMeasurement.prototype.InitGrid = function () {
    console.log("Init ItemMeasurement Grid");
    var grid = this;
    var bulkUpdate1 = this.bulkUpdateItem;
    var ids1 = this.idsTable;

    $(this.divSelector).jsGrid({
        width: "100%", height: function () { return $(this).height() > 400 ? ($(this).height()).toString() + "px" : "400px"; },
        bulkUpdate: bulkUpdate1, ids: ids1,
        inserting: false, editing: false, sorting: false, filtering: true,
        paging: true, pageLoading: true, pageSize: 20, exporting: true,
        confirmDeleting: false,
        onItemDeleting: function (args) {
            grid.gridHelper.onItemDeletingBehavior(args, grid.divSelector);
        },
        fields: [
            { name: "Id", type: "text", title: "Id", width: 30, filtering: false, editing: false, inserting: false },
            //{ name: "ItemOPId", type: "text", title: "Id ANC", width: 40, filtering: false, css: "textDark", insertcss: "tbItemId inputDisabled", editcss: "tbItemId inputDisabled" },
            {
                name: "ItemCode", title: "Kod ANC", type: "text", width: 70, filtering: true, insertcss: "tbItemCode", pageSize: 100, css: "itemCodeColumn textWhiteBold",
                editing: false
            },
            { name: "ItemName", title: "Nazwa ANC", type: "text", width: 140, filtering: true, css: "textBlue text-truncate" },
            { name: "SerialNumber", type: "text", title: "Numer Seryjny", width: 40 },
            { name: "Counter", type: "text", title: "Numer Pomiaru", width: 40 },

            { name: "Result0", type: "text", title: "Wynik 1", width: 40 },
            { name: "Result1", type: "text", title: "Wynik 2", width: 40 },
            { name: "Result2", type: "text", title: "Wynik 3", width: 40 },
            { name: "Result3", type: "text", title: "Wynik 4", width: 40 },
            { name: "Result4", type: "text", title: "Wynik 5", width: 40 },
            { name: "Result5", type: "text", title: "Wynik 6", width: 40 },
            { name: "Result6", type: "text", title: "Wynik 7", width: 40 },
            { name: "Result7", type: "text", title: "Wynik 8", width: 40 },
            { name: "Result8", type: "text", title: "Wynik 9", width: 40 },
            { name: "Result9", type: "text", title: "Wynik 10", width: 40 },
            { type: "control", width: 40, modeSwitchButton: true, editButton: false }
        ],
        controller: this.gridHelper.DB,
        onDataLoaded: function () {
            console.log("grid data loaded");
        }
    });
};
ItemMeasurement.prototype.CreateNewGridInstance = function (divSelector) {
    return new ItemMeasurement(divSelector);
};

ItemMeasurement.prototype.InitAutocompletes = function () {
    ItemAutcomplete(".tbItemId input", ".tbItemCode input", ".tbItemName input");
    ItemAutcomplete("", ".itemCodeColumn input", "");
};