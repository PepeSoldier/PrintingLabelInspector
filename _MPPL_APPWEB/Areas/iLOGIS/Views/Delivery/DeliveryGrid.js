//iLOGIS BrowseGrid

var DeliveryGrid = function (gridDivSelector) {
    console.log("BrowseGridInit");
    GridBulkUpdate.call(this, gridDivSelector);
    var self = this;
    this.divSelector = gridDivSelector;
    this.gridHelper = new GridHelper("Delivery", "/iLogis/Delivery");
};

DeliveryGrid.prototype = Object.create(GridBulkUpdate.prototype);
DeliveryGrid.prototype.constructor = DeliveryGrid;

DeliveryGrid.prototype.InitGrid = function () {

    var grid = this;
    $(this.divSelector).jsGrid({
        width: "100%",
        bulkUpdate: false,
        inserting: false, editing: false, sorting: false,
        paging: true, pageLoading: true, pageSize: 20,
        confirmDeleting: false, filtering: false,
        fields: [
            { name: "Id", type: "text", title: "Id", width: 25,},
            { name: "SupplierName", type: "text", title: "Dostawca", width: 100, },
            { name: "DocumentNumber", type: "text", title: "Numer dokumentu", width: 100 },
            { name: "DocumentDate", type: "date", title: "Data dokumentu", width: 60, },
            { name: "StampTime", type: "date", title: "Data wprowadzenia", width: 60,},
            { name: "UserName", type: "text", title: "Uzytkownik", width: 60,},
            { name: "ItemsCount", type: "text", title: "Liczba Artykułów", width: 50, },
            {
                width: 20, filtering: false, editing: false, inserting: false,
                itemTemplate: function (value, item) {
                    //console.log(item);
                    return $("<button>").html('<i class="fas fa-pencil-alt"></i>')
                        .addClass("btn btn-sm btn-info btnOpen")
                        .attr("data-userId", item.Id)
                        .on("click", function () {
                            window.open("/iLogis/Delivery/DeliveryAdministratorInspection/" + item.Id, '_blank');
                        });
                }
            }
        ],
        controller: grid.gridHelper.DB,
        onDataLoaded: function () { },
        rowClick: function (args) {
            console.log(args.item);
            //RefreshDeliveryItemGrid(args.item.Id);
        }
    });
    this.grid = $(this.divSelector).data("JSGrid");
};
DeliveryGrid.prototype.CreateNewGridInstance = function (divSelector) {
    return new DeliveryGrid(divSelector);
};

DeliveryGrid.prototype.RefreshGrid = function (filterData) {
    if (filterData != null) {
        this.gridHelper.SetFilter(filterData);
    }
    $(this.divSelector).jsGrid("search");
};