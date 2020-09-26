
function Resource(gridDivSelector) {

    var self = this;
    var divSelector = gridDivSelector;
    var areas = new GridHelper("ResourceGroup", "/ONEPROD/Configuration").GetList(false, null).responseJSON;
    var gridHelper = new GridHelper("Resource", "/ONEPROD/Configuration");
    var types = GetTypes();

    this.InitGrid = function () {
        console.log(areas);
        $(divSelector).jsGrid({
            width: "100%",
            inserting: true, editing: true, sorting: false, paging: false, filtering: true,
            confirmDeleting: false,
            onItemDeleting: function (args) {
                gridHelper.onItemDeletingBehavior(args, divSelector);
            },
            fields: [
                { name: "Name", type: "text", title: "Nazwa", width: 150, filtering: true },
                {
                    name: "ResourceGroupId", type: "select", title: "Grupa", width: 150, filtering: false,
                    items: areas, valueField: "Id", textField: "Name",
                    itemTemplate: function (value, item) {
                        var area = areas.find(x => x.Id == value);
                        return area != null ? area.Name : " - ";
                    }
                },
                { name: "Type", title: "TYP", type: "select", items: types, valueField: "Id", textField: "Name", width: 100, filtering: false },
                { name: "Id", type: "text", title: "Id", width: 35, filtering: false, editing: false, inserting: false },
                { name: "FlowTime", type: "text", title: "Czas Przep.[s]", width: 60, filtering: false, cellRenderer: cellR() },
                { name: "SafetyTime", type: "text", title: "Czas Bezp.[min]", width: 60, filtering: false, cellRenderer: cellR() },
                { name: "StageNo", type: "text", title: "Nr Etapu", width: 60, filtering: false, cellRenderer: cellR() },
                { name: "ToolRequired", type: "checkbox", title: "Nrzd.", width: 50, filtering: false, css: "jsgrid-cell-withinput" },
                { name: "ShowBatches", type: "checkbox", title: "Batche", width: 75, filtering: false, css: "jsgrid-cell-withinput" }, 
                { name: "IsOEE", type: "checkbox", title: "OEE?", width: 50, filtering: false, css: "jsgrid-cell-withinput" },
                { name: "TargetAvailability", type: "text", title: "Cel A", width: 50, filtering: false, headercss: "tooltip1", cellRenderer: cellR() },
                { name: "TargetPerformance", type: "text", title: "Cel P", width: 50, filtering: false, headercss: "tooltip1", cellRenderer: cellR() },
                { name: "TargetQuality", type: "text", title: "Cel Q", width: 50, filtering: false, headercss: "tooltip1", cellRenderer: cellR() },
                { name: "TargetOee", type: "text", title: "Cel OEE", width: 50, filtering: false, headercss: "tooltip1", cellRenderer: cellR() },
                {
                    title: "Cele", width: 60, filtering: false, editing: false, inserting: false,
                    itemTemplate: function (value, item) {
                        return $("<button>").text("->").addClass("btn btn-default btnResourceTargets")
                            .on("click", function () {
                                ShowWindowResourceTargets(item.Id, item.Name);
                            });
                    }
                },
                //{ name: "TargetInSec_StopPlanned", type: "text", title: "Cel S.PL.", width: 50, filtering: false, headercss: "tooltip1", cellRenderer: cellR() },
                //{ name: "TargetInSec_StopUnplanned", type: "text", title: "Cel S.NPL", width: 50, filtering: false, headercss: "tooltip1", cellRenderer: cellR() },
                //{ name: "TargetInSec_StopUnplannedBreakdown", type: "text", title: "Cel AWAR", width: 50, filtering: false, headercss: "tooltip1", cellRenderer: cellR() },
                //{ name: "TargetInSec_StopUnplannedPreformance", type: "text", title: "Cel WYD", width: 50, filtering: false, headercss: "tooltip1", cellRenderer: cellR() },
                //{ name: "TargetInSec_StopUnplannedChangeOver", type: "text", title: "Cel PRZ", width: 50, filtering: false, headercss: "tooltip1", cellRenderer: cellR() },
                gridHelper.ColumColor(),
                { name: "Akcje2", type: "control", width: 100, modeSwitchButton: true, editButton: true }
            ],
            controller: gridHelper.DB,
            onDataLoaded: function () { },
            rowClass: function (item, itemIndex) { return "rowType_" + item.Type + " rowParent_" + item.ResourceGroupId + " rowId_" + item.Id; },
            rowClick: function (args) {}
        });
    };

    function cellR() {
        return function (value, item) {
            var class1 = value > 0 ? "" : "zero";
            return '<td><span class="' + class1 + '">' + value + '</span></td>';
        };
    }

    this.RefreshGrid = function () {
        $(divSelector).jsGrid("search");
    };

    function GetTypes() {
        return [
            { "Id": -1, "Name": "-" },
            { "Id": 0, "Name": "Grupa" },
            { "Id": 20, "Name": "Zasób" },
            { "Id": 60, "Name": "Podzasób" },
            { "Id": 80, "Name": "Stanowisko" }
        ];
    }

    var wnd = null;
    function ShowWindowResourceTargets(resourceId, resourceName) {
        var jsQ = new JsonHelper().GetData("/ONEPROD/ConfigurationOEE/ResourceTarget", { "machineId": resourceId });
        jsQ.done(function (data) {
            if (wnd !== null) {
                try { wnd.Close(); }
                catch (ex) { console.log(ex); }
            }
            wnd = new PopupWindow(900, 300, 140, 380);
            wnd.Init("ResourceTargets", "Cele Zasobu: " + resourceName);
            wnd.Show(data);
        });
    }
}