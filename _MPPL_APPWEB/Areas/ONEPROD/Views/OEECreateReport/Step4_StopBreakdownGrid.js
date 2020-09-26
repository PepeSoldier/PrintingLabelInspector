function StopBreakdownGrid(gridDivSelector, reportId, machineId, defaultHour) {

    var self = this;
    var divSelector = gridDivSelector;

    //nowe reasony, trzeba podać do modelu machineId i zamienic selectList z selectList2
    var gridHelperReasons = new GridHelper("ReasonsByEntryTypeAndMachineId", "/ONEPROD/OEECreateReport");
    var selectList = gridHelperReasons.GetList(false, { machineId: machineId, entryType: 31 }).responseJSON;
   
    var gridHelper = new GridHelper("Stoppage", "/ONEPROD/OEECreateReport");
    gridHelper.SetFilter({ ReportId: reportId, EntryType: 31 });
    
    var sgh = new StopGridHelper(divSelector, reportId, gridHelper.DB, selectList, 31, "UR(min)", defaultHour);

    this.InitGrid = function () {
        sgh.GenerateGrid();
    };
    this.RefreshGrid = function () {
        $(divSelector).jsGrid("search");
    };

    function GetData() {
        return [
            {
                "Godz": "21:41",
                "Powod": 1,
                "Ur": 35,
                "Opt": 7,
            },
            {
                "Godz": "18:25",
                "Powod": 3,
                "Ur": 15,
                "Opt": 3,
            }
        ];
    }
    function GetSelectList() {
        return [
            {
                "Id": 1,
                "Name": "Awaria pieca"
            },
            {
                "Id": 2,
                "Name": "Awaria Prasy"
            },
            {
                "Id": 3,
                "Name": "Awaria elektryczna"
            },
            {
                "Id": 4,
                "Name": "Awaria mechaniczna"
            },
        ];
    }
}