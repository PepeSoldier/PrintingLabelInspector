
var WorkstationGrid = function(gridDivSelector) {
    GridBulkUpdate.call(this, gridDivSelector);
    var self = this;
    this.divSelector = gridDivSelector;
    this.resources = new GridHelper("ResourceDropDown", "/MasterData/Resource").GetList(false, null).responseJSON;
    this.gridHelper = new GridHelper("Workstation", "/MasterData/Workstation");

}

WorkstationGrid.prototype = Object.create(GridBulkUpdate.prototype);
WorkstationGrid.prototype.constructor = WorkstationGrid;

WorkstationGrid.prototype.InitGrid = function () {
    var bulkUpdate1 = this.bulkUpdateItem;
    var ids1 = this.idsTable;
    var resources = this.resources;
    $(this.divSelector).jsGrid({
        width: "100%", height: function () { return $(this).height() > 400 ? ($(this).height()).toString() + "px" : "400px"; },
        bulkUpdate: bulkUpdate1, ids: ids1,
        inserting: true, editing: true, sorting: false, paging: false, filtering: true,
        confirmDeleting: false,
        fields: [
            { name: "Id", type: "text", title: "Id", width: 35, filtering: false, editing: false, inserting: false },
            { name: "Name", type: "text", title: "Nazwa", width: 190, filtering: true, css: "textPink" },
            {
                name: "LineId", type: "select", title: "Zasób", width: 150, filtering: true,
                items: resources, valueField: "Id", textField: "Name",
                itemTemplate: function (value, item) {
                    var group = resources.find(x => x.Id == value);
                    return group != null ? group.Name : " - ";
                }
            },
            { name: "SortOrder", type: "text", title: "Kolejność Montaż", width: 120, filtering: false },
            { name: "SortOrderTrain", type: "text", title: "Kolejność Pociąg", width: 120, filtering: false },
            { name: "FlowRackLOverride", type: "text", title: "L", width: 120, filtering: false },
            { name: "ProductsFromIn", type: "text", title: "Prod.Od.Wej", width: 60, filtering: false },
            { name: "ProductsFromOut", type: "text", title: "Prod.Od.Wyj.", width: 60, filtering: false },
            {
                width: 40, filtering: false, editing: false, inserting: false,
                itemTemplate: function (value, item) {
                        return $("<button>").html('<i class="fas fa-history"></i>')
                            .addClass("btn btn-sm btn-info btnShowChangeLog")
                            .on("click", function () { new PFEPChangeLog().ShowLogByObjectId("Workstation", item.Id); });
                }
            },
            { name: "Akcje2", type: "control", width: 100, modeSwitchButton: true, editButton: true }
        ],
        controller: this.gridHelper.DB,
        onDataLoaded: function () { },
            //rowClick: function (args) {}
    });
    this.grid = $(this.divSelector).data("JSGrid");
};
WorkstationGrid.prototype.CreateNewGridInstance = function (divSelector) {
    return new WorkstationGrid(divSelector);
}