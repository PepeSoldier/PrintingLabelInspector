
function JobItemConfigGrid(gridDivSelector) {

    var self = this;
    var divSelector = gridDivSelector;
    var gridHelper = new GridHelper("JobItemConfig", "/OTHER/VisualControl");
    //var resourceGroups = new GridHelper("ResourceGroup", "/ONEPROD/Configuration").GetList(false, null).responseJSON;
    //var processes = new GridHelper("Process", "/ONEPROD/Configuration").GetList(false, { Name: null, ParentId: -1, Deleted: 0 }).responseJSON;
    var types = GetTypes();
    var locations = GetLocations();
    var cameras = GetCameras();

    this.InitGrid = function () {
       
        $(divSelector).jsGrid({
            width: "100%",
            inserting: true, editing: true, sorting: true, paging: true, filtering: true, pageSize: 100,
            confirmDeleting: false,
            onItemDeleting: function (args) {
                gridHelper.onItemDeletingBehavior(args, divSelector);
            },
            onItemUpdating: function (args) {
                console.log("updating the item");
            },
            fields: [
                { name: "Id", type: "text", title: "Id", width: 75, editing: false, filtering: false, css: "hidden" },
                { name: "ItemId", type: "text", title: "Id ANC", width: 20, filtering: false, insertcss: "tbItemId", editcss: "tbItemId" },
                //{ name: "ItemCode", type: "text", title: "Kod ANC", width: 150 },
                {
                    name: "ItemCode", title: "Kod ANC", type: "text", width: 25, filtering: true, insertcss: "tbItemCode", pageSize: 100, css: "itemCodeColumn",
                    editTemplate: function (value, item) {
                        this._editPicker = $('<input type="text" id="tbItemCode">').val(value);
                        ItemAutcomplete(".tbItemId input", this._editPicker, ".tbItemName input");
                        return this._editPicker;
                    },
                    editValue: function () { return this._editPicker.val(); }
                },
                {
                    name: "ItemName", title: "Nazwa ANC", type: "text", width: 65, filtering: false, insertcss: "tbItemName", editcss: "tbItemName",
                    //editTemplate: function (value, item) { return value; }, editValue: function () { return ""; },
                    insertTemplate: function (value, item) { this._editPickerName = $('<input type="text" disabled>').val(value); return this._editPickerName; },
                    insertValue: function () { return this._editPickerName.val(); },
                    editTemplate: function (value, item) { this._editPickerName = $('<input type="text" disabled>').val(value); return this._editPickerName; },
                    editValue: function () { return this._editPickerName.val(); }
                },
                //{ name: "ItemName", type: "text", title: "Nazwa ANC", width: 90, editing: false, filtering: false },
                { name: "PairNo", type: "text", title: "Nr Pary", width: 20, editing: true, filtering: true },
                {
                    name: "JobNo", type: "text", title: "Nr JOB'a", width: 20, editing: true, filtering: true,
                    itemTemplate: function (value, item) {
                        return $("<div>")
                            .addClass("badge")
                            .addClass("badgeShadow")
                            .css("background-color", JobColor(value))
                            .css("font-size", "14px")
                            .css("color", "white")
                            .css("text-shadow", "1px 1px 2px rgba(0,0,0,0.5)")
                            .text(value);
                    },
                },
                {
                    name: "CameraNo", title: "Kamera", type: "select", items: cameras, valueField: "Id", textField: "Name", width: 20, editing: true, filtering: true,
                    itemTemplate: function (value, item) {
                        return $("<div>")
                            .addClass("badge")
                            .addClass("badgeShadow")
                            .css("background-color", CameraColor(value))
                            .css("font-size", "14px")
                            .css("color", "white")
                            .css("text-shadow", "1px 1px 2px rgba(0,0,0,0.5)")
                            .text("CAM." + value);
                    },
                },
                { name: "Location", title: "Lokacja", type: "select", items: locations, valueField: "Id", textField: "Name", width: 50, filtering: true },
                { name: "Type", title: "Typ", type: "select", items: types, valueField: "Id", textField: "Name", width: 50, filtering: true },
                { name: "Description", type: "text", title: "Opis", width: 120, editing: true, filtering: true },
                { name: "Akcje", type: "control", width: 50, modeSwitchButton: true, editButton: true, inserting: false },
            ],
            rowClass: function (item, itemIndex) { return "rowJob" + item.JobNo; },
            controller: gridHelper.DB,
            onDataLoaded: function () {
                console.log("grid data loaded-2");
            },
            onRefreshed: function (args) {
                console.log("grid refreshed");
                ItemAutcomplete(".tbItemId input", ".tbItemCode input", ".tbItemName input");
                ColorTable();
            },
            //rowClick: function (args) { }
        });
    };
    this.RefreshGrid = function (filter) {
        gridHelper.SetFilter(filter, "filterVal");
        $(divSelector).jsGrid("search");
    };

    function GetCameras() {
        return [
            { "Id": 1, "Name": "CAM.1" },
            { "Id": 2, "Name": "CAM.2" },
            { "Id": 3, "Name": "CAM.3" },
        ];
    }
    function GetTypes() {
        return [
            { "Id": 0, "Name": "-" },
            { "Id": 1, "Name": "Styropian" },
            { "Id": 2, "Name": "Deska" },
            { "Id": 3, "Name": "Karton" },
        ];
    }
    function GetLocations() {
        return [
            { "Id": 0, "Name": "-" },
            { "Id": 1, "Name": "Przód" },
            { "Id": 2, "Name": "Tył" },
            { "Id": 4, "Name": "Lewy Bok" },
            { "Id": 8, "Name": "Prawy Bok" },
            { "Id": 16, "Name": "Przód i Tył" },
        ];
    }

    function JobColor(number) {
        switch (number) {
            case 1: return "#1486FF";
            case 2: return "#FFDD00";
            case 3: return "#58CC23";
            case 4: return "#C34CFF";
            case 5: return "#458072";
            case 6: return "#805138";
            case 7: return "#25C8CC";
            case 8: return "#CCC31E";
            default: return "#FF2600";
        }
    }

    function CameraColor(number) {
        switch (number) {
            case 1: return "#ABA782";
            case 2: return "#D4CFA1";
            case 3: return "#878467";
            default: return "#FF2600";
        }
    }
    function RowBg(val) {
        if (val % 2 == 1) {
            return "rowShadow";
        }
        else {
            return "";
        }
    }

    function ColorTable() {

        console.log("color table");
        var table = $(document).find(".jsgrid-table")[1];
        //for (var i = 1, row; row = table.rows[i]; i++) {
        c = 0;
        for (var i = 1; i < table.rows.length; i++) {
            //iterate through rows
            //rows would be accessed using the "row" variable assigned in the for loop
            //for (var j = 0, col; col = row.cells[j]; j++) {
            //    //iterate through columns
            //    //columns would be accessed using the "col" variable assigned in the for loop
            //}
            if ($(table.rows[i].cells[4]).text() != $(table.rows[i-1].cells[4]).text()) {
                c++;
            }
            $(table.rows[i]).addClass(RowBg(c));
            console.log($(table.rows[i].cells[4]).text());
        }
    }
}