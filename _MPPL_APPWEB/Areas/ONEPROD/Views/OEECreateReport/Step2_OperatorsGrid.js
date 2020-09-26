
function OperatorsGrid(gridDivSelector, reportId, shiftCode) {

    var self = this;
    var divSelector = gridDivSelector;
    var gridHelper = new GridHelper("ReportEmployees", "/ONEPROD/OEECreateReport");
    var types = GetTypes();
    var skills = GetSkills();
    var repId = reportId;

    this.InitGrid = function () {
        console.log(repId);
        $(divSelector).jsGrid({
            width: 800,
            inserting: true,
            editing: true,
            sorting: false,
            paging: false,
            autoload: true,
            filtering: true,
            deleteConfirm: "Jesteś pewny, że chcesz usunąć operatora?",
            onRefreshed: function (args) {
                EmployeeAutocomplete(".tbEmployeeInsert input", "", shiftCode);
            },
            controller: gridHelper.DB,
            fields: [
                { name: "Id", css: "hidden", type: "text", width: 100, filtering: false },
                { name: "ReportId", type: "text", width: 40, css: "hidden",
                    insertValue: ReprtId(), insertTemplate: Templt(), filterTemplate: Templt(), filterValue: ReprtId()
                },
                { name: "EmployeeName", title: "Operator", type: "text", width: 100, insertcss: "tbEmployeeInsert", filtering: false },
                { name: "enumOperatorType", title: "Typ", type: "select", items: types, valueField: "Id", textField: "Name", width: 100, filtering: false },
                { name: "SkillsCount", title: "Skills", type: "select", items: skills, valueField: "Id", textField: "Name", width: 30, filtering: false },

                { type: "control", width: 40 }
            ]
        });
    };
    this.RefreshGrid = function () {
        $(divSelector).jsGrid("search");
    };

    function ReprtId() { return function() { return repId; }; }
    function Templt() { return $("<td>"); }

    function GetTypes() {
        return [
            {
                "Id": 1,
                "Name": "Ustawiacz"
            },
            {
                "Id": 2,
                "Name": "Operator"
            },
            {
                "Id": 3,
                "Name": "Prowadzący"
            }
        ];
    }
    function GetSkills() {
        return [
            {
                "Id": 1,
                "Name": "A"
            },
            {
                "Id": 2,
                "Name": "B"
            },
            {
                "Id": 3,
                "Name": "C"
            }
        ];
    }
}