
function GridProcess(gridDivSelector) {

    var self = this;
    var divSelector = gridDivSelector;
    var gridHelper = new GridHelper("Process", "/ONEPROD/Configuration");
    var parents = gridHelper.GetList(false, { Name: null, ParentId: -1, Deleted: 0}).responseJSON;

    this.InitGrid = function () {
        $(divSelector).jsGrid({
            width: "100%",
            inserting: true, editing: true, sorting: false, paging: false, filtering: true,
            confirmDeleting: false,
            onItemDeleting: function (args) {
                gridHelper.onItemDeletingBehavior(args, divSelector);
            },
            fields: [
                { name: "Id", type: "text", title: "Id", width: 50, filtering: false, editing: false, inserting: false },
                { name: "Name", type: "text", title: "Nazwa", width: 250 },
                {
                    name: "ParentId", type: "select", title: "Parent", width: 100, filtering: true,
                    items: parents, valueField: "Id", textField: "Name",
                    itemTemplate: function (value, item)
                    {
                        var p = parents.find(x => x.Id == value); return p != null ? p.Name : " - ";
                    },
                    filterTemplate: function () {
                        var $filterControl = jsGrid.fields.select.prototype.filterTemplate.call(this);
                        $filterControl[0].options.add(new Option("", "-1"), $filterControl[0].options[0]);
                        $filterControl[0].options.selectedIndex = 0;
                        return $filterControl;
                    }
                },
                { name: "Akcje2", type: "control", width: 100, modeSwitchButton: true, editButton: true }
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