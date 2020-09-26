

var Item = function(gridDivSelector) {

    var BaseUrl = "/ONEPROD/"

    function saveItemColor(itemId, colorType, color) {
        $.ajax({
            url: BaseUrl + "Configuration/ItemUpdateColor",
            data: '{ Id: ' + itemId + ', ' + colorType + ': "' + color + '"}',
            type: 'POST',
            contentType: 'application/json; charset=utf-8',
            success: function (data) {

            }
        });
    }
    
    GridBulkUpdate.call(this, gridDivSelector);
    var self = this;
    this.divSelector = gridDivSelector;
    this.gridHelper = new GridHelper("Item", "/ONEPROD/Configuration");
    this.itemCategories = new GridHelper("ItemGroup", "/ONEPROD/Configuration").GetList(false, null).responseJSON;
    this.areas = GetResourceGroups();
    this.types = GetTypes();

    this.RefreshGrid = function (filter) {
        this.gridHelper.SetFilter(filter);
        var grid = $(this.divSelector).data("JSGrid");

        if (grid.bulkUpdate == false) {
            $(this.divSelector).jsGrid("loadData");
        } else {
            grid.data.push({ Id: "" });
            grid.refresh();
        };

    };
    function GetResourceGroups() {
        var arr = [
            { "Id": -1,"Name": ""},
            { "Id": null,"Name": "-"}
        ];

        var arr2 = new GridHelper("ResourceGroup", "/ONEPROD/Configuration").GetList(false, null).responseJSON;

        return arr.concat(arr2);
    }
    function GetTypes() {
        return [
            {"Id": 0,  "Name": "-"},
            {"Id": 10, "Name": "Surowiec"},
            {"Id": 20, "Name": "Pozycja Zakupowa"},
            {"Id": 30, "Name": "Pozycja Pośrednia"},
            {"Id": 40, "Name": "Półfabrykat"},
            {"Id": 50, "Name": "Wyrób Gotowy"},
            {"Id": 90, "Name": "Grupa"}
        ];
    }
}

Item.prototype = Object.create(GridBulkUpdate.prototype);
Item.prototype.constructor = Item;

Item.prototype.InitGrid = function () {
    var bulkUpdate1 = this.bulkUpdateItem;
    var ids1 = this.idsTable;
    var itemCategories = this.itemCategories;
    var areas = this.areas;
    var types = this.types;
    var gridHelper = this.gridHelper;
    var divSelector = this.divSelector;
    $(this.divSelector).jsGrid({
        width: "100%",
        bulkUpdate: bulkUpdate1, ids: ids1,
        inserting: true, editing: true, sorting: false, filtering: true,
        paging: true, pageLoading: true, pageSize: 20,
        confirmDeleting: false,
        onItemDeleting: function (args) {
            gridHelper.onItemDeletingBehavior(args, divSelector);
        },
        fields: [
            { name: "Id", type: "text", title: "Id", width: 40, filtering: false, editing: false, inserting: false },
            { name: "Code", type: "text", title: "Nr Artykułu", width: 75 },
            { name: "Name", type: "text", title: "Nazwa Artykułu", width: 150, editing: false, inserting: false },
            {
                name: "ItemGroupId", type: "select", title: "Grupa Artykułu", width: 100,
                items: this.itemCategories, valueField: "Id", textField: "Name",
                itemTemplate: function (value, item) {
                    var m = itemCategories.find(x => x.Id == value);
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
                name: "ResourceGroupId", type: "select", title: "Grupa Zasobów", width: 150, filtering: true, editing: false, inserting: false,
                items: areas, valueField: "Id", textField: "Name",
                itemTemplate: function (value, item) {
                    var resGrpId = item.ItemGroup != null ? item.ItemGroup.ResourceGroupId : null;
                    var area = areas.find(x => x.Id == resGrpId);
                    return area != null ? area.Name : " - ";
                }
            },
            { name: "Type", title: "Typ", type: "select", items: types, valueField: "Id", textField: "Name", width: 100, filtering: true },
            { name: "WorkOrderGenerator", type: "checkbox", title: "Gener.?", width: 40, filtering: false, css: "jsgrid-cell-withinput" },
            this.gridHelper.ColumColorEmpty(),
            //this.ManageColumn(),
            { name: "Akcje3", type: "control", width: 100, modeSwitchButton: true, editButton: true },
        ],
        controller: this.gridHelper.DB,
        onDataLoaded: function () {
            console.log("grid data loaded");
        },
        rowClick: function (args) { }
    });
};
Item.prototype.CreateNewGridInstance = function (divSelector) {
    return new Item(divSelector);
}