
function Warehouse(gridDivSelector) {

    var self = this;
    var divSelector = gridDivSelector;
    var gridHelper = new GridHelper("Warehouse", "/iLogis/Config");

    var warehouses = [{ Id: -1, Name: "" }];
    warehouses = warehouses.concat(new GridHelper("Warehouse", "/iLogis/Config")
        .GetList(false, { pageIndex: 1, pageSize: 9999 })
        .responseJSON.data.map(x => ({ "Id": x.Id, "Name": x.Code + ". " + x.Name})));
   
 
    this.InitGrid = function () {

        $(divSelector).jsGrid({
            width: "100%", height: function () { return $(this).height() > 400 ? ($(this).height()).toString() + "px" : "400px"; },
            inserting: true, editing: true, sorting: false, filtering: true,
            paging: true, pageLoading: true, pageSize: 20,
            confirmDeleting: false,
            onItemDeleting: function (args) {
                gridHelper.onItemDeletingBehavior(args, divSelector);
            },
            fields: [
                { name: "Id", type: "text", title: "Id", width: 40, filtering: false, editing: false, inserting: false, css: "textDark" },
                { name: "Code", type: "text", title: "Kod", width: 90, css: "textPink" },
                { name: "Name", type: "text", title: "Name", width: 350 },
                {
                    name: "ParentWarehouseId", type: "select", title: "Magazyn Nadrzędny", width: 200, filtering: true, css: "text-truncate textBlue", 
                    items: warehouses, valueField: "Id", textField: "Name",
                    itemTemplate: function (value, item) {
                        var m = warehouses.find(x => x.Id == value);
                        return m != null ? m.Name : value;
                    }
                },
                {
                    name: "AccountingWarehouseId", type: "select", title: "Magazyn Księgowy", width: 200, filtering: true, css: "text-truncate textBlue",
                    items: warehouses, valueField: "Id", textField: "Name",
                    itemTemplate: function (value, item) {
                        var m = warehouses.find(x => x.Id == value);
                        return m != null ? m.Name : value;
                    }
                },
                //{ name: "QtyOfSubLocations", type: "text", title: "QtyOfSubLocations", width: 50, editing: false, inserting: false, filtering: false },
                { name: "isMRP", type: "checkbox", title: "MRP?", width: 40, },
                { name: "isOutOfScore", type: "checkbox", title: "O.ofS.", width: 40, },
                { name: "IndependentSerialNumber", type: "checkbox", title: "S.No?", width: 50, },
                { name: "LabelLayoutFileName", type: "text", title: "Nazwa Etykiety", width: 70, },
                
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