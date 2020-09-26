
function ItemGroupCycleTime(gridDivSelector) {
    // PO MODYFIKACJACH

    var self = this;
    var divSelector = gridDivSelector;
    var gridHelper = new GridHelper("ItemGroupCycleTime", "/ONEPROD/Configuration");
    var itemGroups = new GridHelper("ItemGroup", "/ONEPROD/Configuration").GetList(false, null).responseJSON;
    var machines = new GridHelper("Resource", "/ONEPROD/Configuration").GetList(false, null).responseJSON;

    this.InitGrid = function () {
        $(divSelector).jsGrid({
            width: "100%",
            inserting: true, editing: true, sorting: false, paging: false, filtering: true,
            confirmDeleting: false,
            onItemDeleting: function (args) {
                gridHelper.onItemDeletingBehavior(args, divSelector);
            },
            fields: [
                { name: "Id", css: "hidden" },
                {
                    name: "MachineId", type: "select", title: "Maszyna", width: 250,
                    items: machines, valueField: "Id", textField: "Name",
                    itemTemplate: function (value, item) {
                        var m = machines.find(x => x.Id == value);
                        return m != null ? m.Name : " - ";
                    },
                    filterTemplate: function () {
                        var $filterControl = jsGrid.fields.select.prototype.filterTemplate.call(this);
                        $filterControl[0].options.add(new Option("", "-1"), $filterControl[0].options[0]);
                        $filterControl[0].options.selectedIndex = 0;
                        return $filterControl;
                    }
                },
                {
                    name: "ItemGroupId", type: "select", title: "Kategoria", width: 250,
                    items: itemGroups, valueField: "Id", textField: "Name",
                    itemTemplate: function (value, item) {
                        var m = itemGroups.find(x => x.Id == value);
                        return m != null ? m.Name : " - ";
                    },
                    filterTemplate: function () {
                        var $filterControl = jsGrid.fields.select.prototype.filterTemplate.call(this);
                        $filterControl[0].options.add(new Option("", "-1"), $filterControl[0].options[0]);
                        $filterControl[0].options.selectedIndex = 0;
                        return $filterControl;
                    }
                },
                { name: "CycleTime", type: "text", title: "Czas Cyklu", width: 50, filtering: false },
                { name: "ProgramNumber", type: "text", title: "Nr Prog.", width: 50, filtering: false },
                { name: "ProgramName", type: "text", title: "Nazwa Prog.", width: 80, filtering: false },
                { name: "PiecesPerPallet", type: "text", title: "Szt/Pal", width: 50, filtering: false },
                { name: "Preferred", type: "checkbox", title: "Preferred", width: 50, filtering: false },
                { name: "Active", type: "checkbox", title: "Active", width: 50, filtering: false },
                { name: "Akcje3", type: "control", width: 50, modeSwitchButton: true, editButton: true }
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

