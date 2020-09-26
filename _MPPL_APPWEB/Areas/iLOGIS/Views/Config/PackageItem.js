
var PackageItem = function (gridDivSelector) {
    GridBulkUpdate.call(this, gridDivSelector);
    var self = this;
    this.divSelector = gridDivSelector;
    this.gridHelper = new GridHelper("PackageItem", "/iLogis/Config");
    //this.packages = new GridHelper("Package", "/iLogis/Config").GetList(false, null).responseJSON;
    //this.itemWMSs = new GridHelper("Item2", "/iLogis/Config").GetList(false, null).responseJSON;
    this.warehouseLocationTypes = [{ Id: -1, Name: "" }];
    this.warehouseLocationTypes = this.warehouseLocationTypes.concat(new GridHelper("WarehouseLocationType", "/iLogis/Config")
        .GetList(false, { pageIndex: 1, pageSize: 9999 })
        .responseJSON.data.map(x => ({ "Id": x.Id, "Name": x.Name })));

    this.warehouses = [{ Id: -1, Name: "" }];
    this.warehouses = this.warehouses.concat(new GridHelper("Warehouse", "/iLogis/Config")
        .GetList(false, { pageIndex: 1, pageSize: 9999 })
        .responseJSON.data.map(x => ({ "Id": x.Id, "Name": x.Name })));

    this.pickingStrategy = GetPickingStrategy();

    function GetPickingStrategy() {
        return [
            { "Id": 0, "Name": "-" },
            { "Id": 1, "Name": "Order Qty" },
            { "Id": 2, "Name": "Full Package" }
        ];
    }
};

PackageItem.prototype = Object.create(GridBulkUpdate.prototype);
PackageItem.prototype.constructor = PackageItem;

PackageItem.prototype.InitGrid = function () {
    console.log("Init PackageItem Grid");
    var grid = this;
    var bulkUpdate1 = this.bulkUpdateItem;
    var ids1 = this.idsTable;
    var self = this;
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
            { name: "Id", type: "text", title: "Id", width: 40, filtering: false, editing: false, inserting: false },
            { name: "ItemWMSId", type: "text", title: "Id ANC", width: 40, filtering: false, css: "textDark", insertcss: "tbItemId inputDisabled", editcss: "tbItemId inputDisabled" },
            {
                name: "ItemCode", title: "Kod ANC", type: "text", width: 70, filtering: true, insertcss: "tbItemCode", pageSize: 100, css: "itemCodeColumn textWhiteBold",
                //editTemplate: function (value, item) {
                //    this._editPicker = $('<input type="text" id="tbItemCode">').val(value);
                //    ItemAutcomplete(".tbItemId input", this._editPicker, ".tbItemName input");
                //    return this._editPicker;
                //},
                //editValue: function () { return this._editPicker.val(); }
                editing: false, editcss: "text-center textWhiteBold"
            },
            {
                name: "ItemName", title: "Nazwa ANC", type: "text", width: 140, filtering: true, css: "textBlue text-truncate", insertcss: "tbItemName", editcss: "tbItemName",
                //editTemplate: function (value, item) { return value; }, editValue: function () { return ""; },
                insertTemplate: function (value, item) { this._editPickerName = $('<input type="text" disabled>').val(value); return this._editPickerName; },
                insertValue: function () { return this._editPickerName.val(); },
                editTemplate: function (value, item) { this._editPickerName = $('<input type="text" disabled>').val(value); return this._editPickerName; },
                editValue: function () { return this._editPickerName.val(); }
            },
            { name: "PackageId", type: "text", title: "Op.Id", width: 40, filtering: false, css: "textDark", insertcss: "tbPackageId inputDisabled", editcss: "tbPackageIdEdit inputDisabled" },
            { name: "PackageName", type: "text", title: "Opakowanie", width: 170, filtering: true, css: "tbPackageNameColumn textGreen", insertcss: "tbPackageName", editcss: "tbPackageNameEdit" },
            { name: "QtyPerPackage", type: "text", title: "Ilość w opak.", width: 50, filtering: false },
            { name: "PackagesPerPallet", type: "text", title: "Opak. / Pal.", width: 50, filtering: false, css: "packagesPerPallet" },
            {
                name: "WarehouseId", type: "select", title: "Magazyn", width: 100, css: "text-truncate",
                items: this.warehouses, valueField: "Id", textField: "Name",
                itemTemplate: function (value, item) {
                    return item.WarehouseName;
                }
            },
            {
                name: "WarehouseLocationTypeId", type: "select", title: "Typ", width: 100,
                items: this.warehouseLocationTypes, valueField: "Id", textField: "Name",
                itemTemplate: function (value, item) {
                    return item.WarehouseLocationTypeName;
                }
            },
            {
                name: "PickingStrategy", type: "select", title: "Pick.typ", width: 100,
                items: this.pickingStrategy, valueField: "Id", textField: "Name",
                itemTemplate: function (value, item) {
                    var m = grid.pickingStrategy.find(x => x.Id == value);
                    return m != null ? m.Name : " - ";
                }
            },
            { name: "QtyPerPallet", type: "text", title: "Ilość / Pal.", width: 40, filtering: false, editing: false, inserting: false },
            {
                name: "UnitOfMeasure", type: "text", title: "JM", width: 40, editing: false, filtering: false,
                itemTemplate: function (value, item) { return ConvertUoM(value); }
            },
            { name: "PalletD", type: "text", title: "dł. pal.", width: 40, filtering: false },
            { name: "PalletW", type: "text", title: "szer. pal.", width: 40, filtering: false },
            { name: "PalletH", type: "text", title: "wys. pal.", width: 40, filtering: false, css: "palletH" },
            { name: "WeightGross", type: "text", title: "Waga brutto opak.", width: 40, filtering: false },
            {
                width: 40, filtering: false, editing: false, inserting: false,
                itemTemplate: function (value, item) {
                    return $("<button>").html('<i class="fas fa-history"></i>')
                        .addClass("btn btn-sm btn-info btnShowChangeLog")
                        .on("click", function () { new PFEPChangeLog().ShowLogByObjectId("PackageItem", item.Id); });
                }
            },
            //this.ManageColumn(),
            { type: "control", width: 100, modeSwitchButton: true, editButton: true }
        ],
        controller: this.gridHelper.DB,
        onDataLoaded: function () {
            console.log("grid data loaded");
            self.InitAutocompletes();
        },
        editItem: function (item) {
            this.__proto__.editItem.call(this, item);
            PackageAutcomplete(".tbPackageIdEdit input", ".tbPackageNameEdit input", null,null);

            $(document).off('keyup', ".tbPackageNameEdit input");
            $(document).on('keyup', ".tbPackageNameEdit input, .tbPackageName input", function () {
                if ($(this).val().length <= 0) {
                    $(this).val("p:");
                }
            });
        }
    });
    //this.InitAutocompletes();
};
PackageItem.prototype.CreateNewGridInstance = function (divSelector) {
    return new PackageItem(divSelector);
};
PackageItem.prototype.InitAutocompletes = function () {
    ItemWMSAutcomplete(".tbItemId input", ".tbItemCode input", ".tbItemName input");
    ItemWMSAutcomplete("", ".itemCodeColumn input", "");
    PackageAutcomplete(".tbPackageId input", ".tbPackageName input", ".palletH input", ".packagesPerPallet input");
    PackageAutcomplete("", ".tbPackageNameColumn input");

    $(".tbPackageName input").val("p:");
    $(".tbPackageNameColumn input").val("p:");
};



