
var WorkstationItem = function(gridDivSelector) {
    GridBulkUpdate.call(this, gridDivSelector);
    var self = this;
    this.divSelector = gridDivSelector;
    this.gridHelper = new GridHelper("WorkstationItem", "/iLogis/Config");
    //var workstations = new GridHelper("Workstation", "/MasterData/Workstation").GetList(false, null).responseJSON;
    //var items = new GridHelper("Item2", "/iLogis/Config").GetList(false, null).responseJSON;
}

WorkstationItem.prototype = Object.create(GridBulkUpdate.prototype);
WorkstationItem.prototype.constructor = WorkstationItem;

WorkstationItem.prototype.InitGrid = function () {
    var grid = this;
    var el = $(this.divSelector);
    var bulkUpdate1 = this.bulkUpdateItem;
    var ids1 = this.idsTable;
    
    $(this.divSelector).jsGrid({
        width: "100%", height: function () { return $(this).height() > 400 ? ($(this).height()).toString() + "px" : "400px"; },
        bulkUpdate: bulkUpdate1, ids: ids1,
        inserting: true, editing: true, sorting: false, filtering: true,
        paging: true, pageLoading: true, pageSize: 20,
        confirmDeleting: false, autoload: false,
        onItemDeleting: function (args) {
            grid.gridHelper.onItemDeletingBehavior(args, grid.divSelector);
        },
        fields: [
            { name: "Id", type: "text", title: "Id", width: 50, filtering: false, editing: false, inserting: false },
            { name: "ItemWMSId", type: "text", title: "Id ANC", width: 50, filtering: false, css: "textDark", insertcss: "tbItemId inputDisabled", editcss: "tbItemId inputDisabled" },
            { name: "ItemCode", title: "Kod ANC", type: "text", width: 110, filtering: true, editing: false, insertcss: "tbItemCode", editcss: "textWhiteBold", pageSize: 100, css: "itemCodeColumn textWhiteBold" },
            {
                name: "ItemName", title: "Nazwa ANC", type: "text", width: 250, css: "textBlue", insertcss: "tbItemName", editcss: "tbItemName",
                //editTemplate: function (value, item) { return value; }, editValue: function () { return ""; },
                insertTemplate: function (value, item) { this._editPickerName = $('<input type="text" disabled>').val(value); return this._editPickerName; },
                insertValue: function () { return this._editPickerName.val(); },
                editTemplate: function (value, item) { this._editPickerName = $('<input type="text" disabled>').val(value); return this._editPickerName; },
                editValue: function () { return this._editPickerName.val(); }
            },
            { name: "WorkstationId", type: "text", title: "St.Id", width: 50, filtering: false, css: "textDark", insertcss: "tbWorkstationId inputDisabled", editcss: "tbWorkstationId inputDisabled" },
            { name: "WorkstationName", type: "text", title: "Stanowisko", width: 150, filtering: true, css: "tbWorkstationNameColumn textPink", insertcss: "tbWorkstationName", editcss: "tbWorkstationNameEdit" },
            { name: "LineName", type: "text", title: "Linia", width: 90, filtering: true, css: "", insertcss: "tbLineName", editcss: "tbLineNameEdit" },
            { name: "PutTo", type: "text", title: "Odł.do", width: 60, filtering: true, css: "text"},
            
            { name: "MaxPackages", type: "text", title: "Max liczba opakowań", width: 50 },
            { name: "SafetyStock", type: "text", title: "Zapas bezpiecz.", width: 50 },
            { name: "MaxBomQty", type: "text", title: "Sztuk w BOM", width: 50 },
            { name: "CheckOnly", type: "checkbox", title: "Kontrola", width: 30 },
            {
                width: 40, filtering: false, editing: false, inserting: false,
                itemTemplate: function (value, item) {
                    return $("<button>").html('<i class="fas fa-history"></i>')
                        .addClass("btn btn-sm btn-info btnShowChangeLog")
                        .on("click", function () { new PFEPChangeLog().ShowLogByObjectId("WorkstationItem", item.Id); });
                }
            },
            this.ManageColumn(),
            { type: "control", width: 100, modeSwitchButton: true, editButton: true }
        ],
        controller: this.gridHelper.DB,
        onDataLoaded: function () {
            console.log("grid data loaded");
            grid.InitAutocompletes();
        },
        editItem: function (item) {
            this.__proto__.editItem.call(this, item);
            WorkstationAutcomplete(".tbWorkstationId input", ".tbWorkstationNameEdit input", ".tbLineNameEdit input");
        }
    });
    grid.InitAutocompletes();
};
WorkstationItem.prototype.CreateNewGridInstance = function (divSelector) {
    return new WorkstationItem(divSelector);
};
WorkstationItem.prototype.InitAutocompletes = function() {
    ItemWMSAutcomplete(".tbItemId input", ".tbItemCode input", ".tbItemName input");
    ItemWMSAutcomplete("", ".itemCodeColumn input", "");
    WorkstationAutcomplete(".tbWorkstationId input", ".tbWorkstationName input", ".tbLineName input");
    //WorkstationAutcomplete("", ".tbWorkstationNameColumn input", "");
}