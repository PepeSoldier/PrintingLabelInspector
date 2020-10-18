
function UsersGrid(gridDivSelector) {

    var self = this;
    var divSelector = gridDivSelector;
    var gridHelper = new GridHelper("User", "/IDENTITY/Account");
    //var departments = new GridHelper("Department", "/DEF/MasterData").GetList(false, null).responseJSON;

    this.InitGrid = function () {
        $(divSelector).jsGrid({
            width: "100%",
            height: "100%",
            inserting: true,
            editing: true,
            sorting: true,
            paging: false,
            autoload: false,
            filtering: true,
            align: "center",
            controller: gridHelper.DB,
            fields: [
                { name: "Id", css: "hidden", type: "text", width: 100, },
                { name: "UserName", title: "UserName", type: "text", width: 50, align: "center" },
                { name: "FirstName", title: "FirstName", type: "text", width: 50, align: "center" },
                { name: "LastName", title: "LastName", type: "text", width: 60, align: "center" },
                { name: "Email", title: "Email", type: "text", width: 130, align: "center" },
                { name: "Title", title: "Title", type: "text", width: 30, align: "center" },
                { name: "Factory", title: "Factory", type: "text", width: 20, align: "center" },
                { name: "Akcje2", type: "control", width: 50, modeSwitchButton: true, editButton: true }
            ],
        });
    };
    this.RefreshGrid = function (filterData) {
        gridHelper.SetFilter(filterData);
        $(divSelector).jsGrid("search");
    };
}