
function ItemGroup(gridDivSelector) {

    var self = this;
    var divSelector = gridDivSelector;
    var gridHelper = new GridHelper("ItemGroup", "/ONEPROD/Configuration");
    var resourceGroups = new GridHelper("ResourceGroup", "/ONEPROD/Configuration").GetList(false, null).responseJSON;
    var processes = new GridHelper("Process", "/ONEPROD/Configuration").GetList(false, { Name: null, ParentId: -1, Deleted: 0 }).responseJSON;

    ShowWindowItemGroupTools = function (partCategoryId) {
        $.get("/ONEPROD/ConfigurationAPS/ItemGroupTool/" + partCategoryId, function (data) {
            wnd = new PopupWindow(850, 200);
            wnd.Init("windowItemGroupTL", "Narzędzia połączone z kategorią");
            wnd.Show(data);
        });
    }

    ShowWindowCycleTimes = function (partCategoryId) {
        $.get("/ONEPROD/Configuration/ItemGroupCycleTime/" + partCategoryId, function (data) {
            wnd = new PopupWindow(850, 200);
            wnd.Init("windowItemGroupCT", "Czasy cyklu dla kategorii");
            wnd.Show(data);
        });
    }


    this.InitGrid = function () {
       
        $(divSelector).jsGrid({
            width: "100%",
            inserting: true, editing: true, sorting: true, paging: true, filtering: true,
            confirmDeleting: false,
            onItemDeleting: function (args) {
                gridHelper.onItemDeletingBehavior(args, divSelector);
            },
            fields: [
                { name: "Id", type: "text", title: "Id", width: 75, editing: false, inserting: false, filtering:false },
                { name: "Name", type: "text", title: "Nazwa", width: 150 },
                {
                    name: "ProcessId", type: "select", title: "Proces", width: 220,
                    items: processes, valueField: "Id", textField: "Name",
                    itemTemplate: function (value, item) {
                        var m = processes.find(x => x.Id == value);
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
                    name: "ResourceGroupId", type: "select", title: "Grupa Maszyn", width: 130, 
                    items: resourceGroups, valueField: "Id", textField: "Name",
                    itemTemplate: function (value, item) {
                        var area = resourceGroups.find(x => x.Id == value);
                        return area != null ? area.Name : " - ";
                    },
                    filterTemplate: function () {
                        var $filterControl = jsGrid.fields.select.prototype.filterTemplate.call(this);
                        $filterControl[0].options.add(new Option("", "-1"), $filterControl[0].options[0]);
                        $filterControl[0].options.selectedIndex = 0;
                        return $filterControl;
                    }
                },
                gridHelper.ColumColor(),
                { name: "MinBatch", type: "text", title: "Min. Batch", width: 60, filtering: false },
                { name: "NM_BoxCount", type: "text", title: "BX", width: 60, editing: false, inserting: false, filtering: false },
                { name: "NM_CycleTimeCount", type: "text", title: "CT", width: 60, editing: false, inserting: false, filtering: false },
                { name: "NM_PartCount", type: "text", title: "ANC", width: 60, editing: false, inserting: false, filtering: false },
                { name: "NM_ToolCount", type: "text", title: "TL", width: 60, editing: false, inserting: false, filtering: false },
                {
                    width: 60, filtering: false, editing: false, inserting: false,
                    itemTemplate: function (value, item) {
                        return $("<button>").text("TL").addClass("btn btn-default btnPartCatTool")
                                    .on("click", function () {
                                        ShowWindowItemGroupTools(item.Id);
                                    });
                    },
                },
                {
                    width: 60, filtering: false, editing: false, inserting: false,
                    itemTemplate: function (value, item) {
                        return $("<button>").text("CT").addClass("btn btn-default btnPartCatTool")
                                    .on("click", function () {
                                        ShowWindowCycleTimes(item.Id);
                                    });
                    },
                },
                { name: "Akcje", type: "control", width: 100, modeSwitchButton: true, editButton: true, inserting: false },
            ],
            controller: gridHelper.DB,
            onDataLoaded: function () {
                console.log("grid data loaded-2");
            },
            rowClick: function (args) { }
        });
    };
    this.RefreshGrid = function (filter) {
        gridHelper.SetFilter(filter, "filterVal");
        $(divSelector).jsGrid("search");
    };

}