var TransporterLog = function (gridDivSelector) {
    GridBulkUpdate.call(this, gridDivSelector);
    var self = this;
    this.divSelector = gridDivSelector;
    this.gridHelper = new GridHelper("TransporterLog", "/iLogis/WMS");
    this.EnumTransporterLogEntryType = GetEnumTransporterLogEntryType();

    function GetEnumTransporterLogEntryType() {
        return [
            { "Id": 0, "Name": "-" },
            { "Id": 10, "Name": "Incoming" },
            { "Id": 20, "Name": "Locating" },
            { "Id": 50, "Name": "Picking" },
            { "Id": 60, "Name": "Delivery" }
        ];
    }
};

TransporterLog.prototype = Object.create(GridBulkUpdate.prototype);
TransporterLog.prototype.constructor = TransporterLog;

TransporterLog.prototype.InitGrid = function () {
    console.log("Init TransporterLog Grid");
    var grid = this;
    var transportersList = new GridHelper("Transporter", "/iLogis/Config").GetList(false, { filter: { Deleted: false } }).responseJSON.data;

    $(this.divSelector).jsGrid({
        width: "100%", height: function () { return $(this).height() > 400 ? ($(this).height()).toString() + "px" : "400px"; },
        inserting: false, editing: false, sorting: false, filtering: true, exporting: true,
        paging: true, pageLoading: true, pageSize: 100,
        confirmDeleting: false,
        onItemDeleting: function (args) {
            grid.gridHelper.onItemDeletingBehavior(args, grid.divSelector);
        },
        fields: [
            //{ name: "Id", type: "text", title: "Id", width: 40, filtering: false, editing: false, inserting: false },
            { name: "TransporterId", type: "select", title: "Transporter", width: 100, items: transportersList, valueField: "Id", textField: "Name" },
            //{ name: "Transporter.Name", type: "text", title: "Transporter", width: 80},
            { name: "Location", type: "text", title: "Lokacja", width: 90},
            { name: "WorkorderNumber", type: "text", title: "Numer zlecenia", width: 80 },
            { name: "ProductItemCode", type: "text", title: "Produkt", width: 80, css: ""},
            { name: "ItemWMS.Item.Code", type: "text", title: "Kod Art.", width: 80, css: "textWhiteBold" },
            { name: "ItemWMS.Item.Name", type: "text", title: "Nazwa Art.", width: 190, css: "textBlue", filtering: false},
            { name: "ItemQty", type: "text", title: "Ilosc", width: 50, filtering: false },
            { name: "EntryType", title: "Rodzaj", type: "select", items: this.EnumTransporterLogEntryType, valueField: "Id", textField: "Name", width: 100},
            { name: "Comment", type: "text", title: "Komentarz", width: 100, filtering: false },
            { name: "User.UserName", type: "text", title: "User", width: 60, filtering: false},
            {
                name: "TimeStamp", type: "date", title: "Utworzony", width: 180, filtering: true,
                itemTemplate: function (value, item) { return new moment(value).format("YYYY-MM-DD HH:mm:ss"); },
                filterTemplate: function () {

                    var today = new Date();
                    var hour = today.getHours();
                    today.setHours(hour - 24);
                    today.setHours(0);
                    today.setMinutes(0);

                    var today2 = new Date();
                    var hour2 = today2.getHours();
                    today2.setHours(hour2 + 24);
                    today2.setHours(0);
                    today2.setMinutes(0);

                    var datetime1 = new moment(today).format("YYYY-MM-DD HH:mm");
                    var datetime2 = new moment(today2).format("YYYY-MM-DD HH:mm");

                    this._fromPicker = $("<input class='datetimepicker dateFrom' autocomplete='fdsa123' style='width:49%;'>").attr("value", datetime1);
                    this._toPicker = $("<input class='datetimepicker dateTo' autocomplete='fdsa123' style='width:49%;'>").attr("value", datetime2);
                    return $("<div>").append(this._fromPicker).append(this._toPicker);
                },
                filterValue: function () {
                    return {
                        DateFrom: this._fromPicker.val(),
                        DateTo: this._toPicker.val()
                    };
                }
            },
            { type: "control", width: 50, modeSwitchButton: true, editButton: false, deleteButton: false }
        ],
        controller: this.gridHelper.DB,
        onDataLoading: function (args) {
            console.log("grid data loading");
            args.filter.DateFrom = args.filter.TimeStamp.DateFrom;
            args.filter.DateTo = args.filter.TimeStamp.DateTo;
            console.log(args);
        }, 
        onDataLoaded: function () {
            console.log("grid data loaded");
        }
    });

    InitDatepickers();
};
TransporterLog.prototype.CreateNewGridInstance = function (divSelector) {
    return new TransporterLog(divSelector);
};
this.RefreshGrid = function () {
    $(divSelector).jsGrid("search");
};


