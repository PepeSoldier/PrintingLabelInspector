function StopGrid(gridDivSelector, reportId, machineId, defaultHour, reasonTypeId) {

    var self = this;
    var divSelector = gridDivSelector;
    //var _reasonTypeId = reasonTypeId;

    //nowe reasony, trzeba podać do modelu machineId i zamienic selectList z selectList2
    var gridHelperReasons = new GridHelper("ReasonsByEntryTypeAndMachineId", "/ONEPROD/OEECreateReport");
    var selectList = gridHelperReasons.GetList(false, { machineId: machineId, reasonTypeId: reasonTypeId }).responseJSON;
   
    var gridHelper = new GridHelper("Stoppage", "/ONEPROD/OEECreateReport");
    gridHelper.SetFilter({ ReportId: reportId, reasonTypeId: reasonTypeId });
    
    var sgh = new StopGridHelper(divSelector, reportId, gridHelper.DB, selectList, reasonTypeId, "UR(min)", defaultHour);

    this.InitGrid = function () {
        console.log("StopGrid.InitGrid");
        if (selectList.length > 0) {
            $("#col_Grid_" + reasonTypeId).removeClass("hidden");
            sgh.GenerateGrid();
        }
    };
    this.RefreshGrid = function () {
        $(divSelector).jsGrid("search");
    };
}


function StopGrid2(gridDivSelector, reportId, machineId, defaultHour, reasonTypeId) {

    var self = this;
    var divSelector = gridDivSelector;
    
    var gridHelperReasons = new GridHelper("ReasonsTypes", "/ONEPROD/OEECreateReport");
    var selectList = gridHelperReasons.GetList(false, { machineId: machineId }).responseJSON;

    var gridHelper = new GridHelper("Stoppage", "/ONEPROD/OEECreateReport");
    gridHelper.SetFilter({ ReportId: reportId, reasonTypeId: reasonTypeId });

    this.InitGrid = function () {
        console.log("StopGrid2.InitGrid");
        $("#col_Grid_5").removeClass("hidden");

        $(divSelector).jsGrid({
            width: "100%", inserting: true, editing: true, sorting: false, paging: false, autoload: true, filtering: false,
            controller: gridHelper.DB,
            fields: [
                { name: "Id", title: "Id", type: "text", width: 30, css: "hidden" },
                { name: "ReportId", type: "text", width: 40, css: "hidden", insertValue: reportId, insertTemplate: Templt() },
                {
                    name: "Hour", title: "Godz.", type: "text", width: 20,
                    insertTemplate: function (val) { return InsrtHourTemplt(this); },
                    insertValue: function () { return this._insertPicker.val(); }
                },
                { name: "ReasonTypeId", title: "Kategoria Postoju", type: "select", items: selectList, valueField: "Id", textField: "Name", width: 80 },
                
                {
                    name: "UsedTime", title: "UR(min)", type: "number", width: 20, editing: false,
                    itemTemplate: function (value, item) { return parseFloat((value / 60).toFixed(4)); },
                    insertTemplate: function (val) { return InsrtUsedTimeTemplt(this, 0); },
                    editTemplate: function (val) { return InsrtUsedTimeTemplt(this, val); },
                    insertValue: function () { return this._insertUsedTimePicker.val() * 60; },
                    editValue: function () { return this._insertUsedTimePicker.val() * 60; }
                },
                { type: "control", width: 40 }
            ],
            onItemUpdated: function (args) {
                console.log("item updated");
                console.log(args);

                //let gridArrEl = grids.find(x => x.reasonTypeId = args.item.ReasonTypeId);
                //gridArrEl.grid.RefreshGrid();

                $("#jsGridStop_" + args.item.ReasonTypeId).jsGrid("loadData");
                $(divSelector).jsGrid("loadData");
            }
        });

    };
    this.RefreshGrid = function () {
        $(divSelector).jsGrid("search");
    };

    function Templt() { return $("<td>"); }   
    function InsrtUsedTimeTemplt(field, val) {
        field._insertUsedTimePicker = $("<input>").val(val / 60);
        return field._insertUsedTimePicker;
    }
    function InsrtHourTemplt(field) {
        field._insertPicker = $("<input>").val(defaultHour);
        return field._insertPicker;
    }    
}