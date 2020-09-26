var WarehouseLocationSort = function(gridDivSelector) {
    GridBulkUpdate.call(this, gridDivSelector);
    var self = this;
    this.divSelector = gridDivSelector;
    this.gridHelper = new GridHelper("WarehouseLocationSort", "/iLogis/Config");    
}

WarehouseLocationSort.prototype = Object.create(GridBulkUpdate.prototype);
WarehouseLocationSort.prototype.constructor = WarehouseLocationSort;

WarehouseLocationSort.prototype.InitGrid = function () {
    console.log("Init WarehouseLocationSort Grid");
    var grid = this;
    var bulkUpdate1 = this.bulkUpdateItem;
    var ids1 = this.idsTable;
    //var grid = this;
    $(this.divSelector).jsGrid({
        width: "100%", height: function () { return $(this).height() > 400 ? ($(this).height()).toString() + "px" : "400px"; },
        bulkUpdate: bulkUpdate1, ids: ids1,
        inserting: true, editing: true, sorting: false, filtering: true,
        paging: true, pageLoading: true, pageSize: 200,
        confirmDeleting: false,
        onItemDeleting: function (args) {
            grid.gridHelper.onItemDeletingBehavior(args, grid.divSelector);
        },
        fields: [
            { name: "Id", type: "text", title: "Id", width: 40, filtering: false, editing: false, inserting: false },
            { name: "RegalNumber", type: "text", title: "Numer Reagłu", width: 100 },
            { name: "SortOrder", type: "text", title: "Kolejnosc sortowania", width: 100 },
            { name: "SortColumnAscending", type: "text", title: "Sortuj Kolumne (0 malejaco, 1 rosnąco)", width: 100 },
            //this.ManageColumn(),
            { type: "control", width: 50, modeSwitchButton: true, editButton: true }
        ],
        controller: this.gridHelper.DB,
        onDataLoaded: function () {
            console.log("grid data loaded");
        },
    });
};
WarehouseLocationSort.prototype.CreateNewGridInstance = function (divSelector) {
    return new WarehouseLocationSort(divSelector);
};


