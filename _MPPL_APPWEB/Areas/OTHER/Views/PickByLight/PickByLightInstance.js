var PickByLightInstance = function () { };

var PickByLightInstanceGrid = function (gridDivSelector) {
    GridBulkUpdate.call(this, gridDivSelector);
    var self = this;
    this.divSelector = gridDivSelector;
    this.gridHelper = new GridHelper("PickByLightInstance", "/OTHER/PickByLight");
};

PickByLightInstanceGrid.prototype = Object.create(GridBulkUpdate.prototype);
PickByLightInstanceGrid.prototype.constructor = PickByLightInstanceGrid;

PickByLightInstanceGrid.prototype.InitGrid = function () {
    
    $(this.divSelector).jsGrid({
        width: "100%", height: function () { return $(this).height() > 400 ? ($(this).height()).toString() + "px" : "400px"; },
        inserting: true, editing: true, sorting: true,
        paging: true, pageLoading: true, pageSize: 100, autoload: true,
        confirmDeleting: false, filtering: true,
        fields: [
            { name: "Id", type: "text", title: "Id", width: 40, filtering: false, editing: false, inserting: false },
            { name: "Name", type: "text", title: "Nazwa", width: 200, css: "textBlue" },
            { name: "PLCDriverIPAdress", type: "text", title: "PLC IP Adress", width: 50, sorting: true },
            { name: "TCPPort", type: "text", title: "TCP Port", width: 100, filtering: false },
            { name: "LastChange", type: "date", title: "Ost.zmian", width: 60, filtering: false, editing: false, inserting: false  },
            { name: "UserName", type: "text", title: "Użytk.", width: 60, filtering: false, editing: false, inserting: false },
            {
                width: 40, filtering: false, editing: false, inserting: false,
                itemTemplate: function (value, item) {
                    return $("<button>").html('<i class="fab fa-elementor"></i>')
                        .addClass("btn btn-sm btn-info btnGoToElements")
                        .on("click", function () { new PickByLightInstanceElement().ShowList(item.Id); });
                }
            },
            { name: "Akcje2", type: "control", width: 100, modeSwitchButton: true, editButton: true }
        ],
        controller: this.gridHelper.DB
    });
};

PickByLightInstanceGrid.prototype.CreateNewGridInstance = function (divSelector) {
    return new PickByLightInstanceGrid(divSelector);
};