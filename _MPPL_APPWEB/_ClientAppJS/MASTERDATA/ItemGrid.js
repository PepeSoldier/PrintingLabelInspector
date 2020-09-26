
function ItemGrid(gridDivSelector) {

    var self = this;
    var divSelector = gridDivSelector;
    var gridHelper = new GridHelper("Item", "/MASTERDATA/Item");
    

    this.InitGrid = function () {

        $(divSelector).jsGrid({
            width: "100%", height: function () { return $(this).height() > 400 ? ($(this).height()).toString() + "px" : "400px"; },
            inserting: true, editing: true, sorting: false,
            paging: true, pageLoading: true, pageSize: 20,
            confirmDeleting: false, filtering: true,
            fields: [
                { name: "Id", type: "text", title: "Id", width: 40, filtering: false },
                { name: "Code", type: "text", title: "Kod", width: 75 },
                { name: "Name", type: "text", title: "Nazwa", width: 150, },
                { name: "Akcje2", type: "control", width: 100, modeSwitchButton: true, editButton: true }
            ],
            controller: gridHelper.DB,
            onDataLoaded: function () { },
            rowClick: function (args) { }
        });
    };

    this.RefreshGrid = function () {
        $(divSelector).jsGrid("search");
    };
}