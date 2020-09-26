function StopUnplannedGrid(gridDivSelector, reportId, machineId, defaultHour) {

    var self = this;
    var divSelector = gridDivSelector;

    //nowe reasony, trzeba podać do modelu machineId i zamienic selectList z selectList2
    var gridHelperReasons = new GridHelper("ReasonsByEntryTypeAndMachineId", "/ONEPROD/OEECreateReport");
    var selectList = gridHelperReasons.GetList(false, { machineId: machineId, entryType: 30 }).responseJSON;

    var gridHelper = new GridHelper("Stoppage", "/ONEPROD/OEECreateReport");
    gridHelper.SetFilter({ ReportId: reportId, EntryType: 30 });
    
    var sgh = new StopGridHelper(divSelector, reportId, gridHelper.DB, selectList, 30, "OPT(min)", defaultHour);

    this.InitGrid = function () {
        sgh.GenerateGrid();
    };
    this.RefreshGrid = function () {
        $(divSelector).jsGrid("search");
    };

    function GetData() {
        return [
            {
                "Godz": "09:15",
                "Powod": 1,
                "Ur": 25,
                "Opt": 15,
            },
            {
                "Godz": "19:15",
                "Powod": 2,
                "Ur": 15,
                "Opt": 13,
            }
        ];
    }
    function GetSelectList() {
        return [
            {
                "Id": 1,
                "Name": "Awaria Zasilania"
            },
            {
                "Id": 2,
                "Name": "Wypadek w Pracy"
            },
            {
                "Id": 3,
                "Name": "Wizyta Klienta"
            },
            {
                "Id": 4,
                "Name": "Brak części"
            },
        ];
    }
}