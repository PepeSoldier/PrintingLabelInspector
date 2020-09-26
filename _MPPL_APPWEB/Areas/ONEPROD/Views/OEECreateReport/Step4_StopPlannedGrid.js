function StopPlannedGrid(gridDivSelector, reportId, machineId, defaultHour) {

    var self = this;
    var divSelector = gridDivSelector;

    //nowe reasony, trzeba podać do modelu machineId i zamienic selectList z selectList2
    var gridHelperReasons = new GridHelper("ReasonsByEntryTypeAndMachineId", "/ONEPROD/OEECreateReport");
    var selectList = gridHelperReasons.GetList(false, { machineId: machineId, entryType: 20 }).responseJSON;

    var gridHelper = new GridHelper("Stoppage", "/ONEPROD/OEECreateReport");
    gridHelper.SetFilter({ ReportId: reportId, EntryType: 20 });
    
    var sgh = new StopGridHelper(divSelector, reportId, gridHelper.DB, selectList, 20, "OPT(min)", defaultHour);

    this.InitGrid = function () {
        sgh.GenerateGrid();
    };
    this.RefreshGrid = function () {
        $(divSelector).jsGrid("loadData");
    };

    function GetData() {
        return [
            {
                "Godz": "10:45",
                "Powod": 3,
                "Ur": 10,
                "Opt": 10,
            },
            {
                "Godz": "8:54",
                "Powod": 2,
                "Ur": 34,
                "Opt": 20,
            },
        ];
    }
    function GetSelectList() {
        return [
            {
                "Id": 1,
                "Name": "Przerwy"
            },
            {
                "Id": 2,
                "Name": "Planowany PM"
            },
            {
                "Id": 3,
                "Name": "Spotkanie Produkcyjne(z liderami)"
            },
            {
                "Id": 4,
                "Name": "Szkolenie operatora"
            },
        ];
    }
}