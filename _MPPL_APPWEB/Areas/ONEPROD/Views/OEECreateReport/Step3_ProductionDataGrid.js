function ProductionDataGrid(gridDivSelector, reportId, machineId, areaId) {

    var self = this;
    var divSelector = gridDivSelector;
    var gridHelper = new GridHelper("ProductionData", "/ONEPROD/OEECreateReport");
    var gridHelperReasonTypes = new GridHelper("ReasonTypesProduction", "/ONEPROD/OEE");
    var gridHelperReasons = new GridHelper("ReasonProduction", "/ONEPROD/OEE");
    var types = gridHelperReasonTypes.GetList(false, {}).responseJSON;
    var reasons = gridHelperReasons.GetList(false, {}).responseJSON;
    
    var selectReasonVal = 0;

    //var types = GetTypes();
    var repId = reportId;

    this.InitGrid = function () {
        $(divSelector).jsGrid({
            width: '100%',
            inserting: true, editing: true, sorting: false, paging: false,
            autoload: true, filtering: true,
            //deleteConfirm: "Czy napewno chcesz usunąć ten element?",
            controller: gridHelper.DB,
            fields: GenerateColumns(),
            onRefreshed: function (args) {
                console.log("grid refreshed");
                ItemAutocompleteOEE(".tbAncInsert input", ".tbAncInsertInsert input", machineId, ".tbCTInsert input");
            },
            onItemInserted: function (args) {
                console.log("item inserted");
                console.log(args);
                if (args.item.Id === 0) {
                    alert("element nie został zapisany");
                }
            },
            editItem: function (item) {
                console.log("editItem ProdDataGrid");
                var $row = this.rowByItem(item);
                if ($row.length) {
                    this._editRow($row);
                }
                DrawReasonSelectOptions($row.prev(), true);
            }
        });

        $(document).off('change, click', ".selectReasonTypeId select");
        $(document).on("change, click", ".selectReasonTypeId select, .jsgrid-edit-button", function () {
            console.log("change1");
            DrawReasonSelectOptions(this, false);
        });
        $(document).off("click", ".jsgrid-mode-button");
        $(document).on("click", ".jsgrid-mode-button", function () {
            console.log("change3");
            DrawReasonSelectOptions($(".jsgrid-insert-row"), false);
        });
        $(document).off('change, click', ".selectReasonId select");
        $(document).on("change, click", ".selectReasonId select", function () {
            console.log("change2");
            //DrawReasonSelectOptions(this,  true);
        });
    };
    this.RefreshGrid = function () {
        $(divSelector).jsGrid("search");
    };

    function DrawReasonSelectOptions(el,isRow) {
        var tr = isRow == true? el : $(el).closest("tr");
        var selectReasonTypeVal = $(tr).find(".selectReasonTypeId select").val();
        var selectReason = $(tr).find(".selectReasonId select");

        selectReasonVal = $(tr).find(".ReasonTypeIdTmp").text();
        $(selectReason).find("option").remove();

        var result = reasons.filter(x => x.ReasonTypeId == selectReasonTypeVal);
        $.each(result, function() {
            $(selectReason).append($("<option />").val(this.Id).text(this.Name));
        });
        $(selectReason).val(selectReasonVal);
    }

    function GenerateColumns() {
        var columns = [
            {
                name: "ReportId", type: "text", width: 40, css: "hidden",
                insertValue: ReprtId(), insertTemplate: Templt(), filterTemplate: Templt(), filterValue: ReprtId(), editValue: ReprtId()
            },
            { name: "Id", type: "number", width: 20, editing: false, inserting: false, filtering: false },
            { name: "ReasonTypeIdTmp", type: "number", width: 20, css: "hidden ReasonTypeIdTmp", editing: false, inserting: false, filtering: false, itemTemplate: function (value, item) { return item.ReasonId;} },
            { name: "ReasonTypeId", title: "TYP WPISU", type: "select", items: types, valueField: "Id",css: "selectReasonTypeId", textField: "Name", width: 60, filtering: false },
            {
                name: "ReasonId", title: "POWÓD", type: "select", items: reasons, valueField: "Id", css: "selectReasonId", textField: "Name", width: 100, filtering: false,
                itemTemplate: function (value, item) {
                    console.log(value);
                    let reason = reasons.filter(x => x.Id == value);
                    return reason.length > 0? reason[0].Name : "";
                }
            },
            {
                name: "ItemCode", title: "DETAL ANC", type: "text", width: 60, filtering: false, insertcss: "tbAncInsert",
                editTemplate: function (value, item) { this._editPicker = $('<input type="text" disabled>').val(value); return this._editPicker; },
                editValue: function () { return this._editPicker.val(); }
            },
            {
                name: "ItemName", title: "NAZWA DETALU", type: "text", width: 120, filtering: false, insertcss: "tbAncInsertInsert",
                //editTemplate: function (value, item) { return value; }, editValue: function () { return ""; },
                insertTemplate: function (value, item) { this._editPickerName = $('<input type="text" disabled>').val(value); return this._editPickerName; },
                insertValue: function () { return this._editPickerName.val(); },
                editTemplate: function (value, item) { this._editPickerName = $('<input type="text" disabled>').val(value); return this._editPickerName; },
                editValue: function () { return this._editPickerName.val(); }
            },
            { name: "ProdQty", title: "ILOŚĆ WYPROD", type: "text", width: 50, filtering: false },
            {
                name: "CycleTime", title: "CT [sec]", type: "text", width: 50, filtering: false, insertcss: "tbCTInsert",
                insertTemplate: function (value, item) { this._editPickerCT = $('<input type="text" disabled>').val(value); return this._editPickerCT; },
                insertValue: function () { return this._editPickerCT.val(); },
                editTemplate: function (value, item) { this._editPickerCT = $('<input type="text" disabled>').val(value); return this._editPickerCT; },
                editValue: function () { return this._editPickerCT.val(); }
            },
            {
                name: "ProductionCycleTime", title: "PRD.CT [sec]", type: "text", width: 50, filtering: false,
                insertTemplate: function () {
                    var input = this.__proto__.insertTemplate.call(this);
                    input.val(0)
                    return input;
                }
            },
            {
                name: "UsedTime", title: "CZAS [min]", type: "text", width: 50,
                itemTemplate: function (value, item) { return (value/60).toFixed(2); },
                editValue: function () { return 0; },
                editTemplate: function () { return ''; },
                insertValue: function () { return 0; },
                insertTemplate: function () { return ''; },
                filtering: false
            }
        ];
        GenerateAreaDependentColumns(columns);
        columns.push({ type: "control", width: 60 });

        return columns;
    }
    function GenerateAreaDependentColumns(columns) {
        if (areaId == 4) {
            columns.push({ name: "CoilId", title: "NUMER KRĘGU", type: "number", width: 40, filtering: false })
            columns.push({
                name: "FormWeightProcess", title: "WAGA FORM. PROC.", type: "text", width: 50, filtering: false,
                insertTemplate: function () {
                    var input = this.__proto__.insertTemplate.call(this);
                    input.val(0)
                    return input;
                }
            });
            columns.push({
                name: "FormWeightScrap", title: "WAGA FORM. SCR.", type: "text", width: 50, filtering: false,
                insertTemplate: function () {
                    var input = this.__proto__.insertTemplate.call(this);
                    input.val(0)
                    return input;
                }
            });
            columns.push({
                name: "PaperWeight", title: "PAPIER", type: "text", width: 50, filtering: false,
                insertTemplate: function () {
                    var input = this.__proto__.insertTemplate.call(this);
                    input.val(0)
                    return input;
                }
            });
            columns.push({
                name: "TubeWeight", title: "TUBA", type: "text", width: 50, filtering: false,
                insertTemplate: function () {
                    var input = this.__proto__.insertTemplate.call(this);
                    input.val(0)
                    return input;
                }
            });
        }
        else {
            columns.push({ name: "FAKE", title: "-", type: "text", width: 240, filtering: false });
        }
    }

    function ReprtId() { return function () { return repId; }; }
    function Templt() { return $("<td>"); }
    function GetTypes() {
        //return [
        //    {
        //        "Id": 10, "Name": "Produkcja"
        //    },
        //    {
        //        "Id": 11, "Name": "Scrap Materiałowy"
        //    },
        //    //{
        //    //    "Id": 12, "Name": "Scrap Procesowy"
        //    //},
        //    {
        //        "Id": 13, "Name": "Scrap Procesowy - rysy"
        //    },
        //    {
        //        "Id": 14, "Name": "Scrap Procesowy - wgnioty"
        //    },
        //    {
        //        "Id": 15, "Name": "Scrap Procesowy - pekniecia"
        //    },
        //    {
        //        "Id": 16, "Name": "Scrap Procesowy - marszczenia"
        //    }
        //];
    }
    function GetClients() {
        return [
            {
                "LP": 1,
                "TYP WPISU": 2,
                "DETAL ANC": 800300400,
                "NAZWA DETALU": "DRZWI LEWE",
                "NUMER KRĘGU": 3,
                "ILOŚĆ WYPROD": 1000,
                "WAGA FORMATEK": 800,
                "WAGA FORMATEK2": 10,
                "PAPIER": 2,
                "TUBA": 6,
            },
            {
                "LP": 2,
                "TYP WPISU": 1,
                "DETAL ANC": 700500500,
                "NAZWA DETALU": "DRZWI PRAWE",
                "NUMER KRĘGU": 4,
                "ILOŚĆ WYPROD": 1200,
                "WAGA FORMATEK": 1800,
                "WAGA FORMATEK2": 40,
                "PAPIER": 4,
                "TUBA": 7,
            },
        ]
    }
}