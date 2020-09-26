function StopUnplannedChangeOverGrid(gridDivSelector, reportId, machineId, defaultHour) {

    var self = this;
    var divSelector = gridDivSelector;

    //nowe reasony, trzeba podać do modelu machineId i zamienic selectList z selectList2
    var gridHelperReasons = new GridHelper("ReasonsByEntryTypeAndMachineId", "/ONEPROD/OEECreateReport");
    var selectList = gridHelperReasons.GetList(false, { machineId: machineId, entryType: 33 }).responseJSON;

    var gridHelper = new GridHelper("Stoppage", "/ONEPROD/OEECreateReport");
    gridHelper.SetFilter({ ReportId: reportId, EntryType: 33 });
    
    var sgh = new StopGridHelper(divSelector, reportId, gridHelper.DB, selectList, 33, "OPT(min)", defaultHour);

    this.InitGrid = function () {
        sgh.GenerateGrid();
    };
    this.RefreshGrid = function () {
        $(divSelector).jsGrid("search");
    };

    function GetData() {
        return [
            {
                "Godz": "09:11",
                "Powod": 2,
                "Ur": 5,
                "Opt": 17,
            },
            {
                "Godz": "17:25",
                "Powod": 1,
                "Ur": 55,
                "Opt": 43,
            }
        ];
    }
    //Ta funkcja generujaca dane do wyrzucenia po skofigurowaniu db.
    function GetSelectList() {
        return [
            {
                "Id": 1,
                "Name": "Przezbrojenie narzędzi"
            },
            {
                "Id": 2,
                "Name": "Przezbrojenie kręgów"
            },
            {
                "Id": 3,
                "Name": "Przezbrojenie formy"
            },
            {
                "Id": 4,
                "Name": "Przezbrojenie maszyny"
            },
        ];
    }
}