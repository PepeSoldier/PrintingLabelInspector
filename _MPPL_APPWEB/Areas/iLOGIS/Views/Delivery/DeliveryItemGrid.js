//iLOGIS DeliveryItemGrid

var DeliveryItemGrid = function (gridDivSelector) {
    GridBulkUpdate.call(this, gridDivSelector);
    this.divSelector = gridDivSelector;
    this.gridHelper = new GridHelper("DeliveryItem", "/iLogis/Delivery");
};

DeliveryItemGrid.prototype = Object.create(GridBulkUpdate.prototype);
DeliveryItemGrid.prototype.constructor = DeliveryItemGrid;

DeliveryItemGrid.prototype.InitGrid = function () {

    var grid = this;
    $(this.divSelector).jsGrid({
        width: "100%", 
        bulkUpdate: false,
        inserting: false, editing: true, sorting: false,
        paging: true, pageLoading: true, pageSize: 20,
        confirmDeleting: false, filtering: false,
        fields: [
            { name: "Id", type: "text", title: "Id", width: 35,  visible:false },
            { name: "ItemCode", type: "text", title: "Kod Porduktu", width: 65 },
            { name: "NumberOfPackages", type: "text", title: "Il. opakowań", width: 50 },
            { name: "QtyInPackage", type: "text", title: "Ilosc", width: 50 },
            //{ name: "Akcje2", type: "control", width: 100, modeSwitchButton: true, editButton: true }
        ],
        controller: grid.gridHelper.DB,
        onDataLoaded: function () {
            console.log("grid data loaded");
        },
        //rowClick: function (args) { }
    });
    this.grid = $(this.divSelector).data("JSGrid");
};
DeliveryItemGrid.prototype.CreateNewGridInstance = function (divSelector) {
    return new DeliveryItemGrid(divSelector);
};
DeliveryItemGrid.prototype.RefreshGrid = function (filterData) {
    if (filterData != null) {
        this.gridHelper.SetFilter(filterData);
    }
    $(this.divSelector).jsGrid("search");
};
























































//var DeliveryItemGrid = function (gridDivSelector, supplierId, deliveryId) {
//    console.log("DeliveryItemGridInit");
//    GridBulkUpdate.call(this, gridDivSelector);
//    var self = this;
//    this.divSelector = gridDivSelector;
//    this.supplierId = supplierId;
//    this.deliveryId = deliveryId;

//    this.gridHelper = new GridHelper("DeliveryItem", "/iLogis/Delivery");
//    this.gridHelper.SetFilter({ SupplierId: supplierId, DeliveryId: deliveryId });

//};

//DeliveryItemGrid.prototype = Object.create(GridBulkUpdate.prototype);
//DeliveryItemGrid.prototype.constructor = DeliveryItemGrid;

//DeliveryItemGrid.prototype.InitGrid = function () {

//    var grid = this;
//    var _supplierId = this.supplierId;
//    var _deliveryId = this.deliveryId;

//    $(this.divSelector).jsGrid({
//        width: "100%", height: "740px",
//        bulkUpdate: false,
//        inserting: true, editing: true, sorting: false,
//        paging: false, pageLoading: false, pageSize: 20,
//        confirmDeleting: false, filtering: false,
//        fields: [
//            { name: "Id", type: "text", title: "Id", width: 35, editing: false, filtering: false, inserting: false },
//            { name: "SupplierId", type: "text", width: 40, css: "hidden", insertValue: function () { return _supplierId; }, insertTemplate: function () { return $("<td>"); } },
//            { name: "DeliveryId", type: "text", width: 40, css: "hidden", insertValue: function () { return _deliveryId; }, insertTemplate: function () { return $("<td>"); } },
//            {
//                name: "ItemId", type: "text", width: 40, title: "Id Produktu", insertcss: "tbItemId inputDisabled",
//                editcss: "tbItemId inputDisabled", editing: false, fitlering: false, inserting: true
//            },
//            { name: "ItemCode", type: "text", title: "Kod Porduktu", insertcss: "tbItemCode", width: 65, editing: true, filtering: false },
//            { name: "ItemName", type: "text", title: "Nazwa Produktu", insertcss: "tbItemName", width: 160, editing: true, filtering: false },
//            { name: "Quantity", type: "text", title: "Quantity", width: 50, editing: true, filtering: false },
            

//            { name: "Akcje2", type: "control", width: 100, modeSwitchButton: true, editButton: true }
//        ],
//        controller: grid.gridHelper.DB,
//        onDataLoaded: function () {
//            console.log("grid data loaded");
//            grid.InitAutocompletes();
//        },
//        //rowClick: function (args) { }
//    });
//    this.grid = $(this.divSelector).data("JSGrid");
//};
//DeliveryItemGrid.prototype.CreateNewGridInstance = function (divSelector) {
//    return new DeliveryItemGrid(divSelector);
//};
//DeliveryItemGrid.prototype.RefreshGrid = function (filterData) {
//    if (filterData != null) {
//        this.gridHelper.SetFilter(filterData);
//    }
//    ItemAutcomplete(".tbItemId input", ".tbItemCode input", ".tbItemName input");
//    $(this.divSelector).jsGrid("loadData");
//};

//DeliveryItemGrid.prototype.InitAutocompletes = function () {
//    console.log("TUTAJ");
//    //ItemAutcomplete(".tbItemId input", ".tbItemCode input", ".tbItemName input");
//};

