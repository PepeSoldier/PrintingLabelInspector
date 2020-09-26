
function ItemGroupCycleTimePopup(gridDivSelector, areaId) {
    var self = this;
    var divSelector = gridDivSelector;
    var area = { "areaID": areaId };
    var gridHelper = new GridHelper("ItemGroupCycleTime", "/ONEPROD/Configuration");
    var machines = new GridHelper("Resource", "/ONEPROD/Configuration").GetList(false, area).responseJSON;

    this.InitGrid = function () {
        $(divSelector).jsGrid({
            width: "100%",
            inserting: true, editing: true, sorting: false, paging: false, filtering: false,
            confirmDeleting: false,
            onItemDeleting: function (args) {
                gridHelper.onItemDeletingBehavior(args, divSelector);
            },
            fields: [
                { name: "Id", width: 50, filtering: false, inserting: false },
                {
                    name: "MachineId", type: "select", title: "Maszyna", width: 100, filtering: false,
                    items: machines, valueField: "Id", textField: "Name",
                    itemTemplate: function (value, item) {
                        var m = machines.find(x => x.Id == value);
                        return m != null ? m.Name : " - ";
                    }
                },
                { name: "CycleTime", type: "text", title: "Czas cyklu", width: 100, filtering: false, align: "center"},
                {
                    name: "ItemGroupId", css: "hide",
                    insertValue: function () {
                        return $("#ItemGroupCT").attr("data-val");
                    }
                },
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