
function WarehouseItem(gridDivSelector) {

    var self = this;
    var divSelector = gridDivSelector;
    var gridHelper = new GridHelper("WarehouseItem", "/iLogis/Config");
    var warehouses = new GridHelper("Warehouse", "/iLogis/Config").GetList(false, null).responseJSON;
    var itemWMSs = new GridHelper("ItemWMS", "/iLogis/Config").GetList(false, null).responseJSON;

    this.InitGrid = function () {
        $(divSelector).jsGrid({
            width: "100%", height: function () { return $(this).height() > 400 ? ($(this).height()).toString() + "px" : "400px"; },
            inserting: true, editing: true, sorting: false, filtering: false,
            paging: true, pageLoading: true, pageSize: 20,
            confirmDeleting: false,
            onItemDeleting: function (args) {
                gridHelper.onItemDeletingBehavior(args, divSelector);
            },
            fields: [
                { name: "Id", type: "text", title: "Id", width: 50, filtering: false, editing: false, inserting: false },
                {
                    name: "WarehouseId", type: "select", title: "Lokacja", width: 100, filtering: false,
                    items: warehouses, valueField: "Id", textField: "Name",
                    itemTemplate: function (value, item) {
                        var m = warehouses.find(x => x.Id == value);
                        return m != null ? m.Name : " - ";
                    }
                },
                {
                    name: "ItemGroupId", type: "select", title: "Item Group", width: 100, filtering: false,
                    items: itemWMSs, valueField: "Id", textField: "Name",
                    itemTemplate: function (value, item) {
                        var m = itemWMSs.find(x => x.Id == value);
                        return m != null ? m.Name : " - ";
                    }
                },
                { name: "QtyPerLocation", type: "text", title: "Max ilość w lokalizacji", width: 100, filtering: false },
                { name: "Qty", type: "text", title: "Ilość", width: 50, filtering: false },
                { name: "HoursCoverage", type: "text", title: "HoursCoverage", width: 100, filtering: false },
                { name: "UtylizationPerOnepiece", type: "text", title: "UtylizationPerOnepiece", width: 150, filtering: false },
                { type: "control", width: 100, modeSwitchButton: true, editButton: true }
            ],
            controller: gridHelper.DB,
            onDataLoaded: function () {
                console.log("grid data loaded");
            },
            rowClick: function (args) { }
        });
    };
    this.RefreshGrid = function () {
        $(divSelector).jsGrid("search");
    };
}