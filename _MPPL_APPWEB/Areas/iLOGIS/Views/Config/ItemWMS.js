
function ItemWMS(gridDivSelector) {

    var self = this;
    var divSelector = gridDivSelector;
    var gridHelper = new GridHelper("ItemWMS", "/iLogis/Config");
    var workstations = new GridHelper("Resource", "/iLogis/Config").GetList(false, null).responseJSON;
    var items = new GridHelper("Item", "/iLogis/Config").GetList(false, null).responseJSON;

    this.InitGrid = function () {
        console.log(items);
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
                    name: "WorkstationId", type: "select", title: "Stanowisko", width: 100, filtering: false,
                    items: workstations, valueField: "Id", textField: "Name",
                    itemTemplate: function (value, item) {
                        var m = workstations.find(x => x.Id == value);
                        return m != null ? m.Name : " - ";
                    }
                },
                {
                    name: "ItemId", type: "select", title: "Item", width: 100, filtering: false,
                    items: items, valueField: "Id", textField: "Name",
                    itemTemplate: function (value, item) {
                        var m = items.find(x => x.Id == value);
                        return m != null ? m.Name : " - ";
                    }
                },
                { name: "MaxPackages", type: "text", title: "Max liczba opakowań", width: 150, filtering: false },
                { name: "SafetyStock", type: "text", title: "Zapas bezpieczeństwa", width: 150, filtering: false },
                { name: "MaxBomQty", type: "text", title: "Sztuk w BOM", width: 150, filtering: false },
                { name: "Weight", type: "text", title: "Waga", width: 150, filtering: false },
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