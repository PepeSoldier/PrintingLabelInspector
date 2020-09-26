//iLOGIS ItemGrid

var AutomaticRuleGrid = function (gridDivSelector) {
    GridBulkUpdate.call(this, gridDivSelector);
    var self = this;
    this.divSelector = gridDivSelector;
    this.gridHelper = new GridHelper("AutomaticRule", "/iLOGIS/Config");
};

AutomaticRuleGrid.prototype = Object.create(GridBulkUpdate.prototype);
AutomaticRuleGrid.prototype.constructor = AutomaticRuleGrid;

AutomaticRuleGrid.prototype.InitGrid = function () {
    var bulkUpdate1 = this.bulkUpdateItem;
    var ids1 = this.idsTable;
    var divC = this.divSelector;
    var ele = $(divC);
    console.log($(divC).height());
    $(this.divSelector).jsGrid({
        width: "100%", height: function () { return $(this).height() > 400 ? ($(this).height()).toString() + "px" : "400px"; },
        bulkUpdate: bulkUpdate1, ids: ids1,
        inserting: true, editing: true, sorting: true,
        paging: true, pageLoading: true, pageSize: 100,
        confirmDeleting: false, filtering: true,
        fields: [
            { name: "Id", type: "text", title: "Id", width: 40, filtering: false, editing: false, inserting: false },
            { name: "PREFIX", type: "text", title: "PREFIX", width: 50, css: "textWhiteBold" },
            { name: "Name", type: "text", title: "Nazwa", width: 160, css: "textBlue" },
            { name: "WorkstationName", type: "text", title: "Stanowisko", width: 50, css: "textPink", sorting: true },
            { name: "LineNames", type: "text", title: "Linie", width: 100, filtering: false },
            { name: "MaxPackages", type: "text", title: "Max liczba opak.", width: 50 },
            { name: "SafetyStock", type: "text", title: "Zapas bezpiecz.", width: 50 },
            { name: "MaxBomQty", type: "text", title: "Sztuk w BOM", width: 50 },
            { name: "CheckOnly", type: "checkbox", title: "Kontrola", width: 40 },
            { name: "Active", type: "checkbox", title: "Aktywny?", width: 60 },
            { name: "LastChange", type: "date", title: "Ost.zmian", width: 90, filtering: false, editing: false, inserting: false },
            { name: "UserName", type: "text", title: "Użytk.", width: 60, filtering: false, editing: false, inserting: false },
            {
                width: 40, filtering: false, editing: false, inserting: false,
                itemTemplate: function (value, item) {
                    return $("<button>").html('<i class="fas fa-history"></i>')
                        .addClass("btn btn-sm btn-info btnShowChangeLog")
                        .on("click", function () { new PFEPChangeLog().ShowLogByObjectId("AutomaticRule", item.Id); });
                }
            },
            //this.ManageColumn(),
            { name: "Akcje2", type: "control", width: 100, modeSwitchButton: true, editButton: true }
        ],
        controller: this.gridHelper.DB,
        onDataLoaded: function () { },
        rowClick: function (args) {
            //console.log("row click");
            //console.log(args);
            
            //if (args.event.target.cellIndex < 2)
            //{
            //    var wnd = new PopupWindow("70%", "70%", 140, 110);
            //    wnd.Init("ChangeLog", "Historia Zmian");
            //    wnd.Show("loading...");
            //    var jsQ;

            //    if (args.event.target.cellIndex == 0) {
            //        jsQ = new JsonHelper().GetData("/iLOGIS/PFEP/ChangeLog", { "mode": "AutomaticRule", "objectId": args.item.Id });
            //        jsQ.done(function (data) {
            //            wnd.Show(data);
            //        });
            //    } else if (args.event.target.cellIndex == 1) {
            //        jsQ = new JsonHelper().GetData("/iLOGIS/PFEP/ChangeLog", { "mode": "AutomaticRule", "objectDescription": args.item.PREFIX });
            //        jsQ.done(function (data) {
            //            wnd.Show(data);
            //        });
            //    }
            //}
        }
    });
};

AutomaticRuleGrid.prototype.CreateNewGridInstance = function (divSelector) {
    return new AutomaticRuleGrid(divSelector);
};