
function WarehouseLocation(gridDivSelector) {

    var self = this;
    var divSelector = gridDivSelector;
    var gridHelper = new GridHelper("WarehouseLocation", "/iLogis/Config");

    var warehouses = [{ Id: -1, Name: "" }];
    warehouses = warehouses.concat(new GridHelper("Warehouse", "/iLogis/Config")
        .GetList(false, { pageIndex: 1, pageSize: 9999 })
        .responseJSON.data.map(x => ({ "Id": x.Id, "Name": x.Code + ". " + x.Name })));

    var warehouseLocations = [{ Id: -1, Name: "" }];
    warehouseLocations = warehouseLocations.concat(new GridHelper("WarehouseLocation", "/iLogis/Config")
        .GetList(false, { pageIndex: 1, pageSize: 50 })
        .responseJSON.data.map(x => ({ "Id": x.Id, "Name": x.Name })));

    var warehouseLocationTypes = [{ Id: -1, Name: "" }];
    warehouseLocationTypes = warehouseLocationTypes.concat(new GridHelper("WarehouseLocationType", "/iLogis/Config")
        .GetList(false, { pageIndex: 1, pageSize: 9999 })
        .responseJSON.data.map(x => ({ "Id": x.Id, "Name": x.Name })));


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
                { name: "Id", type: "text", title: "Id", width: 50, filtering: false, editing: false, inserting: false },
                { name: "Name", type: "text", title: "Name", width: 80, css: "textWhiteBold" },
                { name: "NameFormatted", type: "text", title: "Name formatted", width: 80, css: "textWhiteBold", filtering: false, editing: false, inserting: false },
                {
                    name: "ParentWarehouseLocationId", type: "select", title: "Lokacja Nadrzędna", width: 100,
                    items: warehouseLocations, valueField: "Id", textField: "Name",
                    itemTemplate: function (value, item) {
                        var m = warehouseLocations.find(x => x.Id == value);
                        return m != null ? m.Name : " - ";
                    }
                },
                {
                    name: "WarehouseId", type: "select", title: "Magazyn", width: 100,
                    items: warehouses, valueField: "Id", textField: "Name",
                    itemTemplate: function (value, item) {
                        var m = warehouses.find(x => x.Id == value);
                        return m != null ? m.Name : " - ";
                    }
                },
                {
                    name: "TypeId", type: "select", title: "Typ", width: 100,
                    items: warehouseLocationTypes, valueField: "Id", textField: "Name",
                    itemTemplate: function (value, item) {
                        var m = warehouseLocationTypes.find(x => x.Id == value);
                        return m != null ? m.Name : " - ";
                    }
                },
                //{ name: "Type", title: "Typ", type: "select", items: WarehouseLocationType, valueField: "Id", textField: "Name", width: 150, filtering: false },
                { name: "QtyOfSubLocations", type: "text", title: "QtyOfSubLc.", width: 40, filtering: false },
                {
                    name: "Utilization", type: "select", title: "Utiliz.", width: 40, filtering: true,
                    filterTemplate: function () {
                        var $select = jsGrid.fields.select.prototype.filterTemplate();
                        $select.prepend($("<option>").prop("value", "-1").text(" "));
                        $select.prepend($("<option>").prop("value", "6").text(">= 0.75"));
                        $select.prepend($("<option>").prop("value", "5").text("<= 0.75"));
                        $select.prepend($("<option>").prop("value", "4").text(">= 0.5"));
                        $select.prepend($("<option>").prop("value", "3").text("<= 0.5"));
                        $select.prepend($("<option>").prop("value", "2").text(">= 0.25"));
                        $select.prepend($("<option>").prop("value", "1").text("<= 0.25"));
                        $select.prepend($("<option>").prop("value", "0").text("Puste"));
                        return $select;
                    },
                    itemTemplate: function (value, item) {
                        return value.toFixed(2);
                    },
                },
                { name: "AvailableForPicker", type: "checkbox", title: "Dost.dla Pickera", width: 40, filtering: false },
                //{ name: "V", type: "text", title: "V", width: 30, filtering: false },
                //{ name: "W", type: "text", title: "W", width: 30, filtering: false },

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

    function GetWLType() {
        return [
            {
                "Id": 0,
                "Name": "Unknown"
            },
            {
                "Id": 10,
                "Name": "HighStorageRack"
            },
            {
                "Id": 20,
                "Name": "OnFloor"
            },
            {
                "Id": 30,
                "Name": "WarehouseTrolley"
            },
            {
                "Id": 90,
                "Name": "Feeder"
            }
        ];
    }
    
}