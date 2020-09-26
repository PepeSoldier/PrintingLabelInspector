
function ItemGroupToolPopup(gridDivSelector) {

    var self = this;
    var divSelector = gridDivSelector;
    var gridHelper = new GridHelper("ItemGroupTool", "/ONEPROD/ConfigurationAPS");
    var itemGroups = new GridHelper("ItemGroup", "/ONEPROD/Configuration").GetList(false, null).responseJSON;
    var tools = new new GridHelper("Tool", "/ONEPROD/ConfigurationAPS").GetList(false, null).responseJSON;

    this.InitGrid = function () {
        $(divSelector).jsGrid({
            width: "100%",
            inserting: true, editing: true, sorting: false, paging: false, filtering: false,
            confirmDeleting: false,
            onItemDeleting: function (args) {
                gridHelper.onItemDeletingBehavior(args, divSelector);
            },
            fields: [
                { name:"Id", width:50, filtering:false,inserting:false},
                {
                    name: "ToolId", type: "select", title: "Kategoria", width: 100, filtering: false,
                    items: tools, valueField: "Id", textField: "Name",
                    itemTemplate: function (value, item) {
                        var m = tools.find(x => x.Id == value);
                        return m != null ? m.Name : " - ";
                    }
                },
                {
                    name: "ItemGroupId", css: "hide",
                    insertValue: function () {
                        return $("#ItemGroupT").attr("data-val");
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