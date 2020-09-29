
var ItemParameter = function (gridDivSelector) {
    GridBulkUpdate.call(this, gridDivSelector);
    var self = this;
    this.divSelector = gridDivSelector;
    this.gridHelper = new GridHelper("ItemParameter", "/ONEPROD/Quality");
   
};

ItemParameter.prototype = Object.create(GridBulkUpdate.prototype);
ItemParameter.prototype.constructor = ItemParameter;

ItemParameter.prototype.InitGrid = function () {
    console.log("Init ItemParameter Grid");
    var grid = this;
    var bulkUpdate1 = this.bulkUpdateItem;
    var ids1 = this.idsTable;

    $(this.divSelector).jsGrid({
        width: "100%", height: function () { return $(this).height() > 400 ? ($(this).height()).toString() + "px" : "400px"; },
        bulkUpdate: bulkUpdate1, ids: ids1,
        inserting: true, editing: true, sorting: false, filtering: true,
        paging: true, pageLoading: true, pageSize: 20,
        confirmDeleting: false,
        onItemDeleting: function (args) {
            grid.gridHelper.onItemDeletingBehavior(args, grid.divSelector);
        },
        fields: [
            { name: "Id", type: "text", title: "Id", width: 50, filtering: false, editing: false, inserting: false },
            { name: "ItemOPId", type: "text", title: "Id ANC", width: 40, filtering: false, css: "textDark", insertcss: "tbItemId inputDisabled", editcss: "tbItemId inputDisabled" },
            {
                name: "ItemCode", title: "Kod ANC", type: "text", width: 70, filtering: true, insertcss: "tbItemCode", pageSize: 100, css: "itemCodeColumn textWhiteBold",
                editing: false, editcss: "text-center textWhiteBold"
            },
            {
                name: "ItemName", title: "Nazwa ANC", type: "text", width: 140, filtering: true, css: "textBlue text-truncate", insertcss: "tbItemName", editcss: "tbItemName",
                insertTemplate: function (value, item) { this._editPickerName = $('<input type="text" disabled>').val(value); return this._editPickerName; },
                insertValue: function () { return this._editPickerName.val(); },
                editTemplate: function (value, item) { this._editPickerName = $('<input type="text" disabled>').val(value); return this._editPickerName; },
                editValue: function () { return this._editPickerName.val(); }
            },
            { name: "Length", type: "text", title: "Dł.(L)", width: 40 },
            { name: "Width", type: "text", title: "Szer.(W)", width: 40 },
            { name: "Depth", type: "text", title: "Wys.(D)", width: 40 },

            { name: "Length_Tolerance", type: "text", title: "Toler.L", width: 40 },
            { name: "Width_Tolerance", type: "text", title: "Toler.W", width: 40 },
            { name: "Depth_Tolerance", type: "text", title: "Toler.D", width: 40 },
            
            { name: "Weight", type: "text", title: "Waga", width: 40 },
            { name: "Color", type: "text", title: "Kolor", width: 40 },

            { name: "ProgramNumber", type: "text", title: "Nr prog.", width: 40 },
            { name: "ProgramName", type: "text", title: "Nazwa prog.", width: 40 },

            { type: "control", width: 100, modeSwitchButton: true, editButton: true }
        ],
        controller: this.gridHelper.DB,
        onDataLoaded: function () {
            console.log("grid data loaded");
            grid.InitAutocompletes();
        }
    });
};
ItemParameter.prototype.CreateNewGridInstance = function (divSelector) {
    return new ItemParameter(divSelector);
};

ItemParameter.prototype.InitAutocompletes = function () {
    ItemAutcomplete(".tbItemId input", ".tbItemCode input", ".tbItemName input");
    ItemAutcomplete("", ".itemCodeColumn input", "");
};