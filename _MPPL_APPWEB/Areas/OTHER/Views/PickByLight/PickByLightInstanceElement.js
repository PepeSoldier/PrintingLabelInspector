var PickByLightInstanceElement = function () {
    this.ShowList = function (instanceId) {
        console.log("PickByLightInstanceElement.ShowList " + instanceId);

        var jsQ = new JsonHelper().GetData("/OTHER/PickByLight/PickByLightInstanceElement", { "pickByLightInstanceId": instanceId });
        jsQ.done(function (data) {
            if (wnd !== null) {
                try { wnd.Close(); }
                catch (ex) { console.log(ex); }
            }
            wnd = new PopupWindow(1200, 500, 140, 380);
            wnd.Init("Lampki", "Lampki: " + instanceId);
            wnd.Show(data);
        });
    };
};

var PickByLightInstanceElementGrid = function (gridDivSelector, pickByLightInstanceId ) {
    GridBulkUpdate.call(this, gridDivSelector);
    var self = this;
    this.divSelector = gridDivSelector;
    this.pickByLightInstanceId = pickByLightInstanceId;
    this.gridHelper = new GridHelper("PickByLightInstanceElement", "/OTHER/PickByLight");
    this.gridHelper.SetFilter({ PickByLightInstanceId: pickByLightInstanceId });
};

PickByLightInstanceElementGrid.prototype = Object.create(GridBulkUpdate.prototype);
PickByLightInstanceElementGrid.prototype.constructor = PickByLightInstanceElementGrid;

PickByLightInstanceElementGrid.prototype.InitGrid = function () {
    let parentId = this.pickByLightInstanceId;
    $(this.divSelector).jsGrid({
        width: "100%", height: function () { return $(this).height() > 400 ? ($(this).height()).toString() + "px" : "400px"; },
        inserting: true, editing: true, sorting: true,
        paging: true, pageLoading: true, pageSize: 100, autoload: true,
        confirmDeleting: false, filtering: true,
        fields: [
            { name: "Id", type: "text", title: "Id", width: 40, filtering: false, editing: false, inserting: false },
            { name: "Name", type: "text", title: "Nazwa", width: 50 },
            { name: "ItemCode", type: "text", title: "Nr. Artykułu", width: 80, sorting: true },
            { name: "PLCMemoryAdress", type: "text", title: "Komórka Pamięci w PLC", width: 100, filtering: false },
            { name: "LastChange", type: "date", title: "Ost.zmian", width: 90, filtering: false, editing: false, inserting: false },
            { name: "UserName", type: "text", title: "Użytk.", width: 60, filtering: false, editing: false, inserting: false },
            { name: "PickByLightInstanceId", type: "text", title: "Parent", width: 60, css: "", insertValue: function () { return parentId; }, editValue: function () { return parentId; } },
            { name: "Akcje2", type: "control", width: 100, modeSwitchButton: true, editButton: true }
        ],
        controller: this.gridHelper.DB
    });
};

PickByLightInstanceElementGrid.prototype.CreateNewGridInstance = function (divSelector) {
    return new PickByLightInstanceElementGrid(divSelector);
};