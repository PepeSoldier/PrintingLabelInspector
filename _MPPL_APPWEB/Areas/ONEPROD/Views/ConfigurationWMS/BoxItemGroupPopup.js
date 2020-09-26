
function BoxItemGroupPopup(gridDivSelector) {
    var self = this;
    var divSelector = gridDivSelector;
    var gridHelper = new GridHelper("BoxItemGroup", "/ONEPROD/ConfigurationWMS");
    var itemGroups = new GridHelper("ItemGroup", "/ONEPROD/Configuration").GetList(false, null).responseJSON;

    this.InitGrid = function () {
        $(divSelector).jsGrid({
            width: "100%",
            inserting: true, editing: true, sorting: false, paging: false, filtering: false,
            confirmDeleting: false,
            onItemDeleting: function (args) {
                gridHelper.onItemDeletingBehavior(args, divSelector);
            },
            fields: [
                { name: "Id", css: "hide" },
                {
                    name: "WarehouseId", css: "hide",
                    insertValue: function () {
                        return $("#BoxCategoryT").attr("data-val");
                    }
                },
                {
                    name: "HoursCoverage", css: "hide",
                    insertValue: function () {
                        return 0;
                    }
                },
                {
                    name: "ItemGroupId", type: "select", title: "Kategoria", width: 100, filtering: false,
                    items: itemGroups, valueField: "Id", textField: "Name",
                    itemTemplate: function (value, item) {
                        console.log("let me in");
                        var m = itemGroups.find(x => x.Id == item.ItemGroupId);
                        return m != null ? m.Name : " - ";
                    }
                },
                { name: "QtyPerLocation", type: "text", title: "Ilość", width: 100, filtering: false },
                { name: "Akcje3", type: "control", width: 100, modeSwitchButton: true, editButton: true },
            ],
            controller: gridHelper.DB,
            onDataLoaded: function () {
                console.log("grid data loaded");
            },
            rowClick: function (args) { }
        });
    };
    this.RefreshGrid = function (filter) {
        gridHelper.SetFilter(filter, "filterVal");
        $(divSelector).jsGrid("search");
    };

}