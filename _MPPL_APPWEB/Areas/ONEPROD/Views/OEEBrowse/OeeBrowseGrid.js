
function OeeBrowseGrid(gridDivSelector) {

    var self = this;
    var divSelector = gridDivSelector;
    var gridHelper = new GridHelper("OeeReport", "/ONEPROD/OEEBrowse");
    
    this.InitGrid = function () {
        $(divSelector).jsGrid({
            width: "100%",
            height: "100%",
            inserting: false,
            editing: false,
            sorting: false,
            paging: false,
            autoload: false,
            filtering: false,
            align:"center",
            controller: gridHelper.DB,
            fields: [
                { name: "Id", css: "hidden", type: "text", width: 100, },
                //{ name: "entryType", css: EntryTypeCss(), title: "", type: "text", width: 10, itemTemplate: EntryTypeFieldTemplate() },
                { name: "ItemCode", title: "Kod Art.", type: "text", width: 70, align: "center" },
                { name: "ItemName", title: "Nazwa", type: "text", width: 150, align: "center", itemTemplate: PartNameTemplate() },
                //{ name: "ReasonName", title: "Powód", type: "text", width: 100, align: "center" },
                { name: "ProdQty", title: "Ilość", type: "text", width: 40, align: "center" },
                { name: "CycleTime", title: "Czas Cyklu", type: "text", width: 40, align: "center" },
                { name: "ProductionCycleTime", title: "Cz.Cykl. PRD", type: "text", width: 40, align: "center" },
                { name: "UsedTime", title: "Czas [min]", type: "text", width: 40, align: "center", itemTemplate: function (value) { return value.toFixed(2); } },
                //{ name: "EntryTimeStamp", title: "Data", type: "text", width: 90, align: "center" },
                { name: "Shift", title: "Zmiana", type: "text", width: 80, align: "center" },
                { name: "ProdDateFormatted", title: "Data", type: "text", width: 80, align: "center" },
                { name: "UserName", title: "Użytkownik", type: "text", width: 60, align: "center" },
                { name: "ReportId", title: "Raport", css: "hidden", type: "text", width: 30 },
                { name: "ReportLink", title: "Raport", css: "", type: "text", width: 40, itemTemplate: function (value, item) { return '<a target="_blank" href="/ONEPROD/OEECreateReport/Index/' + item.ReportId + '">' + item.ReportId + '</a>'; }},
            ],
            //rowClass: function (item, itemIndex) { return GetClassByEntryType(item.ReasonTypeEntryType) + " entryTypeRow"; }
            rowClass: function (item, itemIndex) { return GetClassByEntryType2(item.ReasonTypeId) + " entryTypeRow"; }
        });
    };
    this.RefreshGrid = function (filterData) {
        gridHelper.SetFilter(filterData);
        $(divSelector).jsGrid("search");
    };

    function PartNameTemplate() {
        return function (value, item) {
            if (item.ItemName !== "") {
                return item.ItemName;
            }
            else {
                if (item.ReasonName !== "")
                    return item.ReasonName;
                else
                    return item.ReasonTypeName;
            }
        };
    }

    function EntryTypeCss() {
        return function (value, item) {

            var divClass = "";

            if (value === 10) { divClass = "Production"; }
            else if (value === 11 || value === 12) { divClass = "Scrap"; }
            else if (value === 30) { divClass = "StopUnplanned"; }
            else if (value === 20) { divClass = "StopPlanned"; }

            return "entrytypecol " + divClass;
        };
    }

    function EntryTypeFieldTemplate() {
     
        return function (value, item) {
            var divClass = GetClassByEntryType2(value);
            return $("<div>").addClass(divClass).addClass("EntryTypeSize").html(".");
        };
    }
    function GetClassByEntryType2(value) {
        var divClass = "EtReasonTypeId_" + value;
        //if (value == 0) { divClass = "EtTimeAvailable"; }
        //else if (value == 10) { divClass = "EtProduction"; }
        //else if (value >= 11 && value <= 19) { divClass = "EtScrap"; }
        //else if (value == 20) { divClass = "EtStopPlanned"; }
        //else if (value == 21 || value == 33) { divClass = "EtStopChangeOver"; }
        //else if (value == 30) { divClass = "EtStopUnplanned"; }
        //else if (value == 31) { divClass = "EtStopBreakdown"; }
        //else if (value == 32) { divClass = "EtStopPerformance"; }
        return divClass;
    }
    //function GetClassByEntryType(value) {
    //    var divClass = "";

    //    if (value == 0) { divClass = "EtTimeAvailable"; }
    //    else if (value == 10) { divClass = "EtProduction"; }
    //    else if (value >= 11 && value <= 19) { divClass = "EtScrap"; }
    //    else if (value == 20) { divClass = "EtStopPlanned"; }
    //    else if (value == 21 || value == 33) { divClass = "EtStopChangeOver"; }
    //    else if (value == 30) { divClass = "EtStopUnplanned"; }
    //    else if (value == 31) { divClass = "EtStopBreakdown"; }
    //    else if (value == 32) { divClass = "EtStopPerformance"; }

    //    return divClass;
    //}
}