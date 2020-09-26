//iLOGIS ItemGrid

var AutomaticRulePackageGrid = function (gridDivSelector) {
    GridBulkUpdate.call(this, gridDivSelector);
    var self = this;
    this.divSelector = gridDivSelector;
    this.gridHelper = new GridHelper("AutomaticRulePackage", "/iLOGIS/Config");
};

AutomaticRulePackageGrid.prototype = Object.create(GridBulkUpdate.prototype);
AutomaticRulePackageGrid.prototype.constructor = AutomaticRulePackageGrid;

AutomaticRulePackageGrid.prototype.InitGrid = function () {
    var bulkUpdate1 = this.bulkUpdateItem;
    var ids1 = this.idsTable;
    var divC = this.divSelector;
    var ele = $(divC);
    var self = this;
    console.log($(divC).height());
    $(this.divSelector).jsGrid({
        width: "100%", height: function () { return $(this).height() > 400 ? ($(this).height()).toString() + "px" : "400px"; },
        bulkUpdate: bulkUpdate1, ids: ids1,
        inserting: true, editing: true, sorting: true,
        paging: true, pageLoading: true, pageSize: 100,
        confirmDeleting: false, filtering: true,
        fields: [
            { name: "Id", type: "text", title: "Id", width: 40, filtering: false, editing: false, inserting: false },
            { name: "PREFIX", type: "text", title: "PREFIX", width: 50, css: "textWhiteBold" },
            { name: "Name", type: "text", title: "Nazwa", width: 130, css: "textBlue" },
            //{ name: "PackageId", type: "text", title: "Opakowanie", width: 50, css: "textPink", sorting: true },
            { name: "PackageId", type: "text", title: "Op.Id", width: 50, filtering: false, css: "textDark", insertcss: "tbPackageId inputDisabled", editcss: "tbPackageIdEdit inputDisabled" },
            { name: "PackageName", type: "text", title: "Opakowanie", width: 190, filtering: true, css: "tbPackageNameColumn textGreen", insertcss: "tbPackageName", editcss: "tbPackageNameEdit" },
            
            { name: "QtyPerPackage", type: "text", title: "Ilość w opak.", width: 50 },
            { name: "PackagesPerPallet", type: "text", title: "Opak./pal.", width: 50 },
            { name: "PalletW", type: "text", title: "PalletW", width: 50 },
            { name: "PalletD", type: "text", title: "PalletD", width: 50 },
            { name: "PalletH", type: "text", title: "PalletH", width: 50 },
            { name: "Active", type: "checkbox", title: "Aktywny?", width: 60 },
            { name: "LastChange", type: "date", title: "Ost.zmian", width: 90, filtering: false, editing: false, inserting: false },
            { name: "UserName", type: "text", title: "Użytk.", width: 60, filtering: false, editing: false, inserting: false },
            {
                width: 40, filtering: false, editing: false, inserting: false,
                itemTemplate: function (value, item) {
                    return $("<button>").html('<i class="fas fa-history"></i>')
                        .addClass("btn btn-sm btn-info btnShowChangeLog")
                        .on("click", function () { new PFEPChangeLog().ShowLogByObjectId("AutomaticRule", item.Id); });
                }
            },
            //this.ManageColumn(),
            { name: "Akcje2", type: "control", width: 100, modeSwitchButton: true, editButton: true }
        ],
        controller: this.gridHelper.DB,
        onDataLoaded: function () {
            self.InitAutocompletes();
        },
        editItem: function (item) {
            this.__proto__.editItem.call(this, item);
            PackageAutcomplete(".tbPackageIdEdit input", ".tbPackageNameEdit input", null, null);

            $(document).off('keyup', ".tbPackageNameEdit input");
            $(document).on('keyup', ".tbPackageNameEdit input, .tbPackageName input", function () {
                if ($(this).val().length <= 0) {
                    $(this).val("p:");
                }
            });
        }
    });
};

AutomaticRulePackageGrid.prototype.CreateNewGridInstance = function (divSelector) {
    return new AutomaticRulePackageGrid(divSelector);
};

AutomaticRulePackageGrid.prototype.InitAutocompletes = function () {
    PackageAutcomplete(".tbPackageId input", ".tbPackageName input", ".palletH input", ".packagesPerPallet input");
    PackageAutcomplete("", ".tbPackageNameColumn input");
    $(".tbPackageName input").val("p:");
    $(".tbPackageNameColumn input").val("p:");
};