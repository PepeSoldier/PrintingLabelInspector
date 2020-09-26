
function BOM(gridDivSelector) {

    var self = this;
    var divSelector = gridDivSelector;
    var gridHelper = new GridHelper("BOM", "/ONEPROD/BOM");
    

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
                { name: "ParentCode", type: "text", title: "Parent", width: 80, filtering: true, editing: false, inserting: false },
                { name: "ParentName", type: "text", title: "Parent", width: 150, filtering: true, editing: false, inserting: false },
                { name: "ChildCode", type: "text", title: "Child", width: 80, filtering: true, editing: false, inserting: false },
                { name: "ChildName", type: "text", title: "Child", width: 150, filtering: true, editing: false, inserting: false },
                { name: "QtyUsed", type: "text", title: "QtyUsed", width: 50, filtering: true, editing: false, inserting: false },
                { name: "PREFIX", type: "text", title: "PREFIX", width: 50, filtering: true, editing: false, inserting: false },
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