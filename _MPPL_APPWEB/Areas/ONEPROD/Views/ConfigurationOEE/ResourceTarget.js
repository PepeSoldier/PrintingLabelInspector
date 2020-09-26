function ResourceTarget(gridDivSelector, resourceId) {

    var self = this;
    var divSelector = gridDivSelector;
    var gridHelper = new GridHelper("ResourceTarget", "/ONEPROD/ConfigurationOEE");
    gridHelper.SetFilter({ "ResourceId": resourceId });

    var gridHelperReasonTypes = new GridHelper("ReasonTypes", "/ONEPROD/ConfigurationOEE");
    var types = [{ "Id": -1, "Name": "" }];
    types = types.concat(gridHelperReasonTypes.GetList(false, {}).responseJSON);
    
    this.InitGrid = function () {
        
        $(divSelector).jsGrid({
            width: "100%",
            height: "450px",
            inserting: true,
            editing: true,
            sorting: false,
            paging: false,
            filtering: false,
            fields: [
                { name: "Id", type: "text", title: "Id", width: 5, editing: false, inserting: false, css: "hidden", filtering: false },
                {
                    name: "ResourceId", type: "text", title: "Zasób Id", width: 5, editing: true, inserting: true, css: "hidden", filtercss: "hidden", editcss: "hidden", filtering: false,
                    insertValue: function () { return resourceId; },
                    editValue: function () { return resourceId; }
                },
                { name: "ReasonTypeId", title: "TYP WPISU", type: "select", items: types, valueField: "Id", textField: "Name", width: 100 },
                { name: "Target", type: "text", title: "Cel [s]", width: 60 },
                { name: "Akcje", type: "control", width: 50, modeSwitchButton: true, editButton: true }
            ],
            controller: gridHelper.DB,
            onItemEditing: function (args) {
                console.log("startEdycji");
            },
            onDataLoaded: function () {
                console.log("grid data loaded");
            },
            rowClick: function (args) { },
            onItemUpdated: function (args) {
                console.log(args);
            },
            onItemInserted: function (args) {
                $(divSelector).jsGrid("loadData");
            }
        });
    };
    this.RefreshGrid = function () {
        $(divSelector).jsGrid("search");
    };
}
