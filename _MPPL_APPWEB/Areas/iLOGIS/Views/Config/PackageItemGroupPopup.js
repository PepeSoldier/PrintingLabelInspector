
function PackageItemGroupPopup(gridDivSelector) {

    var self = this;
    var divSelector = gridDivSelector;
    var gridHelper = new GridHelper("PackageItemGroup", "/iLogis/Config");
    var packages = new GridHelper("Package", "/iLogis/Config").GetList(false, null).responseJSON;
    var itemWMSs = new GridHelper("ItemWMS", "/iLogis/Config").GetList(false, null).responseJSON;
    var ids = [];

    this.InitGrid = function () {

        $(divSelector).jsGrid({
            width: "100%", height: function () { return $(this).height() > 400 ? ($(this).height()).toString() + "px" : "400px"; },
            inserting: false, editing: true, sorting: false,
            paging: false, filtering: false, deleting: false,
            fields: [
                {
                    name: "PackageId", type: "select", title: "Opakowanie", width: 100, filtering: false,
                    items: packages, valueField: "Id", textField: "Name",
                    itemTemplate: function (value, item) {
                        var m = packages.find(x => x.Id == value);
                        return m != null ? m.Name : " - ";
                    }
                },
                {
                    name: "ItemWMSId", type: "select", title: "ItemWMS", width: 100, filtering: false,
                    items: itemWMSs, valueField: "Id", textField: "Name",
                    itemTemplate: function (value, item) {
                        var m = itemWMSs.find(x => x.Id == value);
                        return m != null ? m.Name : " - ";
                    }
                },
                { name: "QtyPerPackage", type: "text", title: "Ilość w opakowaniu", width: 200, filtering: false },
                { type: "control", width: 100, modeSwitchButton: true, editButton: true, deleteButton: false }
            ],
            controller:db,
            onDataLoaded: function () {
                console.log("grid data loaded");
            },
            rowClick: function (args) { }
        });
    };
    this.RefreshGrid = function () {
        $(divSelector).jsGrid("search");
    };
    this.SetTable = function (arrayIds) {
        ids = arrayIds;
    }
    
    var db = {
        loadData: function (filter) {
            return $.ajax({
                async: false, type: "POST", data: filter,
                url: "/iLogis/Config/PackageItemGroup" + "GetList",
                success: function (data) {
                    console.log(data);
                },
                error: function (xhr, ajaxOptions, thrownError) {
                }
            });
        },
        updateItem: function (part) {
            return $.ajax({
                async: true, type: "POST", data: { part: part, ids: ids },
                url: "/iLogis/Config/PackageItemGroup" + "Update"
            });
        }
    }

}