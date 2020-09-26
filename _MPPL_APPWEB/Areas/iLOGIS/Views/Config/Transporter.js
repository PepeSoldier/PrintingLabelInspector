

var Transporter = function(gridDivSelector) {
    GridBulkUpdate.call(this, gridDivSelector);
    var self = this;
    this.divSelector = gridDivSelector;
    this.gridHelper = new GridHelper("Transporter", "/iLogis/Config");
    this.types = GetTypes();


    function GetTypes() {
        return [
            { "Id": -1, "Name": "-" },
            { "Id": 1, "Name": "Picker" },
            { "Id": 2, "Name": "Pociąg" },
            { "Id": 3, "Name": "Rozwożący" }
        ];
    }
}

Transporter.prototype = Object.create(GridBulkUpdate.prototype);
Transporter.prototype.constructor = Transporter;

Transporter.prototype.InitGrid = function () {
    console.log("Init Transporter Grid");
    var grid = this;
    var bulkUpdate1 = this.bulkUpdateItem;
    var ids1 = this.idsTable;
    //var grid = this;
    $(this.divSelector).jsGrid({
        width: "100%", height: function () { return $(this).height() > 400 ? ($(this).height()).toString() + "px" : "400px"; },
        bulkUpdate: bulkUpdate1, ids: ids1,
        inserting: true, editing: true, sorting: false, filtering: true,
        paging: true, pageLoading: true, pageSize: 20,
        confirmDeleting: false,
        onItemDeleting: function (args) {
            grid.gridHelper.onItemDeletingBehavior(args, grid.divSelector);
        },
        fields: [
            { name: "Id", type: "text", title: "Id", width: 40, filtering: false, editing: false, inserting: false },
            { name: "Name", type: "text", title: "Nazwa", width: 250 },
            { name: "Code", type: "text", title: "Numer", width: 100 },
            { name: "Type", title: "Typ", type: "select", items: this.types, valueField: "Id", textField: "Name", width: 100, filtering: true },
            { name: "DedicatedResources", type: "text", title: "Zasoby", width: 100 },
            { name: "ConnectedTransporters", type: "text", title: "Numery Połączone", width: 100 },
            { name: "LoopQty", type: "text", title: "Ilość / Pętla", width: 50 },
            {
                width: 60, filtering: false, editing: false, inserting: false,
                itemTemplate: function (value, item) {
                    if (item.Type == 1) {
                        return $("<button>").text("Weryfikuj")
                            .addClass("btn btn-sm btn-default btnVerifyPickingList")
                            .attr("data-transporterId", item.Id)
                            .on("click", function () { VerifyPickingList(item.Id, this);});
                    }
                    if (item.Type == 2) {
                        return $("<button>").text("Przelicz")
                            .addClass("btn btn-sm btn-default btnVerifyDeliveryList")
                            .attr("data-transporterId", item.Id)
                            .on("click", function () { VerifyDeliveryList(item.Id, this); });
                    }
                    else {
                        return "";
                    }
                }
            },
            {
                width: 40, filtering: false, editing: false, inserting: false,
                itemTemplate: function (value, item) {
                    if (item.Type == 2) {
                        return $("<button>").html('<i class="fas fa-history"></i>')
                            .addClass("btn btn-sm btn-info btnShowChangeLog")
                            .attr("data-transporterId", item.Id)
                            .on("click", function () { new PFEPChangeLog().ShowLogByObjectId("Transporter", item.Id); });
                    }
                    else {
                        return "";
                    }
                }
            },
            this.ManageColumn(),
            { type: "control", width: 50, modeSwitchButton: true, editButton: true }
        ],
        controller: this.gridHelper.DB,
        onDataLoaded: function () {
            console.log("grid data loaded");
        },
    });
};
Transporter.prototype.CreateNewGridInstance = function (divSelector) {
    return new Transporter(divSelector);
};

function VerifyDeliveryList(trainId, btn) {
    //$(".btnVerifyDeliveryList").addClass("disabled");
    $(btn).addClass("hidden");
    let json = new JsonHelper();
    let vJson = json.GetPostData("/iLogis/DeliveryList/VerifyDeliveryList", { trainId });
    vJson.done(function () {
        new Alert().Show("success", "Przeliczenie zakończone (pociąg: " + trainId + ")");
        //$(".btnVerifyDeliveryList").removeClass("disabled");
        $(btn).removeClass("hidden");
    });
    vJson.fail(function () {
        $("#btn1Piece").removeClass("brdActive");
        new Alert().Show("danger", "Przeliczenie nie  powiodło się (pociąg: " + trainId + ")");
        //$(".btnVerifyDeliveryList").removeClass("disabled");
        $(btn).removeClass("hidden");
    });
}
function VerifyPickingList(pickerId, btn) {
    //$(".btnVerifyDeliveryList").addClass("disabled");
    $(btn).addClass("hidden");
    let json = new JsonHelper();
    let vJson = json.GetPostData("/iLogis/PickingList/VerifyPickingList", { pickerId });
    vJson.done(function () {
        new Alert().Show("success", "Przeliczenie zakończone (picker: " + pickerId + ")");
        //$(".btnVerifyDeliveryList").removeClass("disabled");
        $(btn).removeClass("hidden");
    });
    vJson.fail(function () {
        $("#btn1Piece").removeClass("brdActive");
        new Alert().Show("danger", "Przeliczenie nie  powiodło się (picker: " + pickerId + ")");
        //$(".btnVerifyDeliveryList").removeClass("disabled");
        $(btn).removeClass("hidden");
    });
}

