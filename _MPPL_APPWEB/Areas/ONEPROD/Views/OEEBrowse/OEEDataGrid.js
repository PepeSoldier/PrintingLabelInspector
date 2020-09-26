
function OEEDataGrid(gridDivSelector) {

    var self = this;
    var divSelector = gridDivSelector;
    var gridHelper = new GridHelper("OEEData", "/ONEPROD/OEEBrowse");
    var type = 0; //0 - prod, 1 - postoj
    var selectedReasonTypeId = -1;
    var selectedMachineIds = [];

    
    this.InitGrid = function () {
        $(divSelector).jsGrid({
            width: "100%", height: "100%", align: "center", pageSize: 50,
            inserting: false, editing: false, sorting: false, paging: true, pageLoading: true, autoload: true, filtering: true, exporting: true,
            controller: gridHelper.DB,
            fields: [
                { name: "Id", css: "hidden", type: "text", width: 100, },
                { name: "ResourceName", title: "Zasob", type: "text", width: 100, align: "center", filtering: false },
                //{ name: "entryType", css: EntryTypeCss(), title: "", type: "text", width: 10, itemTemplate: EntryTypeFieldTemplate() },
                { name: "ItemCode", title: "Kod Artykulu", type: "text", width: 60, align: "center" },
                { name: "ItemName", title: "Nazwa Artykulu", type: "text", width: 150, align: "center", itemTemplate: PartNameTemplate() },
                //{ name: "ReasonName", title: "Powód", type: "text", width: 100, align: "center" },
                { name: "ProdQty", title: "Ilosc", type: "text", width: 40, align: "center", css: "countField", filtering: false },
                { name: "CycleTime", title: "Czas Cyklu", type: "number", width: 40, align: "center", filtering: false },
                //{ name: "ProductionCycleTime", title: "Cz.Cykl. PRD", type: "text", width: 40, align: "center" },
                { name: "UsedTime", title: "Czas [min]", type: "text", width: 40, align: "center", filtering: false, itemTemplate: function (value) { return value.toFixed(2); } },
                //{ name: "EntryTimeStamp", title: "Data", type: "text", width: 90, align: "center" },
                { name: "ProductionDateFormatted", title: "Data", type: "text", width: 90, align: "center", filtering: false },
                { name: "YearWeek", title: "Nr Tyg", type: "text", width: 50, align: "center", filtering: false },
                { name: "Shift", title: "Zmiana", type: "text", width: 50, align: "center", filtering: false },
                { name: "LabourBrigadeName", title: "Brygada", type: "text", width: 40, align: "center", filtering: false },
                { name: "ReasonTypeId", title: "Typ", type: "select", items: GetTypes(), valueField: "Id", textField: "Name", width: 120, align: "center" },
                { name: "ReasonId", title: "Powód", type: "select", items: GetReasons(), valueField: "Id", textField: "Name", width: 120, align: "center" },
                { name: "UserName", title: "Uzytkownik", type: "text", width: 60, align: "center" },
                { name: "ReportId", title: "Raport", css: "hidden", type: "text", width: 30 },
                //{ name: "ReportLink", title: "Raport", css: "", type: "text", width: 30, itemTemplate: function (value, item) { return '<a target="_blank" href="/ONEPROD/OEECreateReport/Index/' + item.ReportId + '">' + item.ReportId + '</a>'; }},
                { type: "control", width: 100, modeSwitchButton: false, editButton: false, deleteButton: false }
            ],
            rowClass: function (item, itemIndex) { return GetClassByEntryType(item.EntryType) + " entryTypeRow"; },
            onDataLoading: function (args) {
                console.log("OEEDataGrid.OnDataLoading");
                let _filters = ReadManyFilters();
                gridHelper.SetFilter(_filters);
            },
            onDataLoaded: function (args) {
                var sum = 0;

                //let _filter = gridHelper.GetFilter();
                //if (selectedReasonTypeId != _filter.ReasonTypeId) {
                //    selectedReasonTypeId = _filter.ReasonTypeId;
                //    fields[12].items = GetReasons();
                //}
                
                $(".countField ").each(function (value, index) {
                    if (!isNaN(parseInt(index.innerHTML))){
                        sum += parseInt(index.innerHTML);
                    }
                }); 
                $("#summaryField").text("Ilość suma: " + sum);
            }
        });
    };
    this.RefreshGrid = function (filterData) {
        gridHelper.SetFilter(filterData);
        if (type != filterData.Type || selectedMachineIds != filterData.machineIds) {
            type = filterData.Type;
            selectedMachineIds = filterData.machineIds;
            self.InitGrid();
        }
        
        $(divSelector).jsGrid("search");
    };

    function PartNameTemplate() {
        return function (value, item) {
            if (item.ItemName != "") {
                return item.ItemName;
            }
            else {
                return "";
                //if (item.ReasonName != "")
                //    return item.ReasonName;
                //else
                //    return "Czas Dostępny";
            }
        };
    }

    function EntryTypeCss() {
        return function (value, item) {

            var divClass = "";

            if (value == 10) { divClass = "Production"; }
            else if (value == 11 || value == 12) { divClass = "Scrap"; }
            else if (value == 30) { divClass = "StopUnplanned"; }
            else if (value == 20) { divClass = "StopPlanned"; }

            return "entrytypecol " + divClass;
        };
    }

    function EntryTypeFieldTemplate() {
        return function (value, item) {
            var divClass = GetClassByEntryType(value);
            return $("<div>").addClass(divClass).addClass("EntryTypeSize").html(".");
        }
    }
    function GetClassByEntryType(value) {
        var divClass = "";

        if (value == 0) { divClass = "EtTimeAvailable"; }
        else if (value == 10) { divClass = "EtProduction"; }
        else if (value >= 11 && value <= 19) { divClass = "EtScrap"; }
        else if (value == 20) { divClass = "EtStopPlanned"; }
        else if (value == 21 || value == 33) { divClass = "EtStopChangeOver"; }
        else if (value == 30) { divClass = "EtStopUnplanned"; }
        else if (value == 31) { divClass = "EtStopBreakdown"; }
        else if (value == 32) { divClass = "EtStopPerformance"; }

        return divClass;
    }

    function GetTypes() {
        if (type == 0) {
            return GetTypesProd();
        } else {
            return GetTypesStop();
        }
    }
    function GetTypesProd() {
        //return [
        //    { "Id": -1, "Name": "" },
        //    { "Id": 10, "Name": "Produkcja" },
        //    { "Id": 11, "Name": "Scrap Materiałowy" },
        //    { "Id": 12, "Name": "Scrap Procesowy" },
        //    { "Id": 19, "Name": "Scrap Etykiety" }
        //];
        return GetTypesStop();
    }
    function GetTypesStop() {
        //return [
        //    { "Id": -1, "Name": "" },
        //    { "Id": 20, "Name": "Post. Planowany" },
        //    { "Id": 30, "Name": "Post. Nieplanowany" },
        //    { "Id": 32, "Name": "Post. Mikro" },
        //];
        var gridHelperReasonTypes = new GridHelper("ReasonTypes", "/ONEPROD/ConfigurationOEE");
        var reasonTypes = [{ "Id": -1, "Name": "" }];
        reasonTypes = reasonTypes.concat(gridHelperReasonTypes.GetList(false, {}).responseJSON);

        return reasonTypes;
    }
    function GetReasons() {
        //return [
        //    { "Id": 1, "Name": "Test 1" },
        //    { "Id": 2, "Name": "Test 2" }
        //];
        var reasons = [{ "Id": -1, "Name": "" }];


            var gridHelperReasons = new GridHelper("ReasonsByEntryTypeAndMachineId", "/ONEPROD/OEECreateReport");
            //reasons = reasons.concat(gridHelperReasons.GetList(false, { ReasonTypeId: selectedReasonTypeId }).responseJSON);
            reasons = reasons.concat(gridHelperReasons.GetList(false, { reasonTypeId: null, machineIds: selectedMachineIds }).responseJSON);

            //for (i = 0; i < selectedMachineIds.length; i++) {
            //    reasons = reasons.concat(gridHelperReasons.GetList(false, { reasonTypeId: null, machineId: selectedMachineIds[i] }).responseJSON);
            //}

            return reasons;
        
    }
    

}