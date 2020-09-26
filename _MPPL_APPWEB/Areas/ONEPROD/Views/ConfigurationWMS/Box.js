
function Box(gridDivSelector) {
    var self = this;
    var divSelector = gridDivSelector;
    var gridHelper = new GridHelper("Box", "/ONEPROD/ConfigurationWMS");
    
    

    this.InitGrid = function () {
        $(divSelector).jsGrid({
            width: "100%",
            inserting: true, editing: true, sorting: false, paging: false,filtering:true,
            confirmDeleting: false,
            onItemDeleting: function (args) {
                gridHelper.onItemDeletingBehavior(args, divSelector);
            },
            fields: [
                { name: "Id", type: "text", title: "Id", width: 75, filtering: false, inserting:false,editing:false },
                { name: "Name", type: "text", title: "Nazwa", width: 250 },
                { name: "QtyOfSubLocations", type: "text", title: "Ilość", width: 150, filtering: false },
                {
                    width: 60, filtering: false, editing: false, inserting: false,
                    itemTemplate: function (value, item) {
                        return $("<button>").text("Otwórz").addClass("btn btn-default btnPartCatTool")
                                    .on("click", function () {
                                        ShowWindowBoxItemGroup(item.Id);
                                    });
                    },
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

    var BaseUrl = "/ONEPROD/";

    ShowWindowBoxItemGroup = function (boxId) {
        $.get(BaseUrl + "ConfigurationWMS/BoxItemGroup/" + boxId, function (data) {
            wnd = new PopupWindow(850, 200);
            wnd.Init("windowItemGroupTL", "Narzędzia połączone z kategorią");
            wnd.Show(data);
        });
    }
}