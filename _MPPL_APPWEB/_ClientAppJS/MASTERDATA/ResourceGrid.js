
function ResourceGrid(gridDivSelector) {

    var self = this;
    var divSelector = gridDivSelector;
    var groups = new GridHelper("ResourceGroup", "/MASTERDATA/Resource").GetList(false, null).responseJSON;
    console.log(groups);
    var areas = new GridHelper("Area", "/MASTERDATA/Area").GetList(false, null).responseJSON;
    console.log(areas);
    var gridHelper = new GridHelper("Resource", "/MasterData/Resource");


    var types = GetTypes();

    this.InitGrid = function () {
        
        $(divSelector).jsGrid({
            width: "100%", height: function () { return $(this).height() > 400 ? ($(this).height()).toString() + "px" : "400px"; },
            inserting: true, editing: true, sorting: false, paging: false, filtering: true,
            confirmDeleting: false,
            fields: [
                { name: "Id", type: "text", title: "Id", width: 35, filtering: false, editing: false, inserting: false },
                { name: "Name", type: "text", title: "Nazwa", width: 190, filtering: true },
                { name: "Type", title: "TYP", type: "select", items: types, valueField: "Id", textField: "Name", width: 100, filtering: false },
                {
                    name: "ResourceGroupId", type: "select", title: "Grupa", width: 150, filtering: true,
                    items: groups, valueField: "Id", textField: "Name",
                    itemTemplate: function (value, item) {
                        var group = groups.find(x => x.Id == value);
                        return group != null ? group.Name : " - ";
                    }
                },
                //{
                //    name: "AreaId", type: "select", title: "Obszar", width: 150, filtering: true,
                //    items: areas, valueField: "Id", textField: "Name",
                //    itemTemplate: function (value, item) {
                //        var area = areas.find(x => x.Id == value);
                //        return area != null ? area.Name : " - ";
                //    }
                //},
                gridHelper.ColumColor(),
                { name: "Akcje2", type: "control", width: 100, modeSwitchButton: true, editButton: true }
            ],
            controller: gridHelper.DB,
            onDataLoaded: function () { },
            rowClick: function (args) {}
        });
    };

    this.RefreshGrid = function () {
        $(divSelector).jsGrid("search");
    };

    function GetTypes() {
        return [
            {
                "Id": -1,
                "Name": "-",
            },
            {
                "Id": 0,
                "Name": "Grupa",
            },
            {
                "Id": 20,
                "Name": "Zasób",
            },
            {
                "Id": 60,
                "Name": "Podzasób",
            },
            {
                "Id": 80,
                "Name": "Stanowisko",
            },
        ];
    }
}