
var Package = function (gridDivSelector, showPriceColumn) {
    GridBulkUpdate.call(this, gridDivSelector);
    var self = this;
    this.divSelector = gridDivSelector;
    this.gridHelper = new GridHelper("Package", "/iLogis/Config");
    this.unitOfMeasures = GetUoM();
    this.packageTypes = GetEPT();
    this._showPriceColumn = showPriceColumn;

    function GetUoM() {
        return [
            { "Id": 0, "Name": "-" },
            { "Id": 1, "Name": "szt" },
            { "Id": 2, "Name": "kg" },
            { "Id": 3, "Name": "m" }
        ];
    }
    function GetEPT() {
        return [
            { "Id": 0, "Name": "-" },
            { "Id": 10, "Name": "CartonBox" },
            { "Id": 20, "Name": "MetalContainer" },
            { "Id": 30, "Name": "PlasticContainer" },
            { "Id": 40, "Name": "Coil" }
        ];
    }
};

Package.prototype = Object.create(GridBulkUpdate.prototype);
Package.prototype.constructor = Package;

Package.prototype.InitGrid = function () {
    console.log("Init Package Grid");
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
            { name: "Code", type: "text", title: "Kod opakowania", width: 150, css: "textWhiteBold" },
            { name: "Name", type: "text", title: "Nazwa", width: 150, css: "textGreen" },
            { name: "Type", title: "Typ", type: "select", items: this.packageTypes, valueField: "Id", textField: "Name", width: 130 },
            { name: "Depth", type: "text", title: "Dł.(L)", width: 40 },
            { name: "Width", type: "text", title: "Szer.(W)", width: 40 },
            { name: "Height", type: "text", title: "Wys.(H)", width: 40 },
            { name: "Weight", type: "text", title: "Waga", width: 40 },
            { name: "Returnable", type: "checkbox", title: "Zwrotne", width: 60 },
            { name: "PackagesPerPallet", type: "text", title: "opak / pal", width: 60 },
            { name: "FullPalletHeight", type: "text", title: "Wysokość palety", width: 60 },
            { name: "UnitOfMeasure", title: "J.M", type: "select", items: this.unitOfMeasures, valueField: "Id", textField: "Name", width: 50 },
            { name: "UnitPrice", type: "text", title: "Cena", width: 50, css: function () { return this._showPriceColumn == true ? "" : "hidden"; } },
            {
                width: 40, filtering: false, editing: false, inserting: false,
                itemTemplate: function (value, item) {
                    return $("<button>").html('<i class="fas fa-history"></i>')
                        .addClass("btn btn-sm btn-info btnShowChangeLog")
                        .on("click", function () { new PFEPChangeLog().ShowLogByObjectId("Package", item.Id); });
                }
            },
            this.ManageColumn(),
            { type: "control", width: 100, modeSwitchButton: true, editButton: true }
        ],
        controller: this.gridHelper.DB,
        onDataLoaded: function () {
            console.log("grid data loaded");
        }
    });
};
Package.prototype.CreateNewGridInstance = function (divSelector) {
    return new Package(divSelector);
};
