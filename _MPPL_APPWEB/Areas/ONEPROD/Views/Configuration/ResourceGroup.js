
function ResourceGroup(gridDivSelector) {

    var self = this;
    var divSelector = gridDivSelector;
    var gridHelper = new GridHelper("ResourceGroup", "/ONEPROD/Configuration");
    var clients = gridHelper.GetList(false, null).responseJSON;

    this.InitGrid = function () {
        console.log(clients);
        $(divSelector).jsGrid({
            width: "100%",
            inserting: true, editing: true, sorting: false, paging: false, filtering: true,
            confirmDeleting: false,
            onItemDeleting: function (args) {
                gridHelper.onItemDeletingBehavior(args, divSelector);
            },
            fields: [
                { name: "Id", type: "text", title: "Id", width: 50, filtering: false, editing: false, inserting: false },
                { name: "Name", type: "text", title: "Nazwa", width: 200, filtering: true },
                { name: "SafetyTime", type: "text", title: "Czas Bezp.", width: 100, filtering: false },
                { name: "StageNo", type: "text", title: "Numer Etapu", width: 100, filtering: false },
                {
                    name: "ClientId", type: "select", title: "Client", width: 100, filtering: false,
                    items: clients, valueField: "Id", textField: "Name",
                    itemTemplate: function (value, item) {
                        var m = clients.find(x => x.Id == value);
                        return m != null ? m.Name : " - ";
                    }
                },
                {
                    name: "ShowBatches", type: "checkbox", title: "Batche", width: 75, filtering: false,
                    
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