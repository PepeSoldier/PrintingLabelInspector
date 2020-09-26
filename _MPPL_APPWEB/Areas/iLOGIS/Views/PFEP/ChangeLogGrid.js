
var PFEPChangeLog = function () {

    this.ShowLogByObjectId = function (mode, objectId) {
        var wnd = new PopupWindow("70%", "70%", 140, 110);
        wnd.Init("ChangeLog", "Historia Zmian");
        wnd.Show("loading...");
        var jsQ;
        console.log(objectId);

        jsQ = new JsonHelper().GetData("/iLOGIS/PFEP/ChangeLog", { "mode": mode, "objectId": objectId });
        jsQ.done(function (data) {
            wnd.Show(data);
        });
    };

    this.ShowLogByObjectDescription = function (mode, objectDescription) {
        var wnd = new PopupWindow("70%", "70%", 140, 110);
        wnd.Init("ChangeLog", "Historia Zmian");
        wnd.Show("loading...");
        var jsQ;

        jsQ = new JsonHelper().GetData("/iLOGIS/PFEP/ChangeLog", { "mode": mode, "objectDescription": objectDescription });
        jsQ.done(function (data) {
            wnd.Show(data);
        });
    };
    this.ShowLogByParentObjectDescription = function (mode, parentObjectDescription) {
        var wnd = new PopupWindow("70%", "70%", 140, 110);
        wnd.Init("ChangeLog", "Historia Zmian");
        wnd.Show("loading...");
        var jsQ;

        jsQ = new JsonHelper().GetData("/iLOGIS/PFEP/ChangeLog", { "mode": mode, "parentObjectDescription": parentObjectDescription });
        jsQ.done(function (data) {
            wnd.Show(data);
        });
    };
};



var ChangeLogGrid = function (gridDivSelector, parameters) {
    GridDefault.call(this, gridDivSelector);
    this.mode = parameters.Mode;
    //this.mode = "Workstation";
    //var mode = "Item";
    //var mode = "AutomaticRule";
    //var mode = "Transporter";
    //var mode = "Package";

    this.divSelector = gridDivSelector;
    this.gridHelper = new GridHelper("ChangeLog", "/iLOGIS/PFEP");
    this.gridHelper.SetFilter({ mode: this.mode }, "mode");
};

ChangeLogGrid.prototype = Object.create(GridDefault.prototype);
ChangeLogGrid.prototype.constructor = ChangeLogGrid;

ChangeLogGrid.prototype.InitGrid = function () {
    console.log("Init DataGrid");
    var self = this;
    var bulkUpdate1 = this.bulkUpdateItem;
    var ids1 = this.idsTable;
    $(this.divSelector).jsGrid({
        title: "PFEP",
        width: "100%",
        height: function () { return $(this).height() > 400 ? ($(this).height()).toString() + "px" : "400px"; },
        bulkUpdate: bulkUpdate1,
        ids: ids1,
        inserting: false, editing: false, sorting: false, filtering: true,
        paging: true, pageLoading: true, pageSize: 20, exporting: true,
        confirmDeleting: false,

        fields: [
            this.GenerateFieldId(),
            this.GenerateFieldDescription(),
            { name: "ObjectName", type: "text", title: "Tabela", width: 90, css: "textBlue", editing: false },
            { name: "FieldName", type: "text", title: "Pole", width: 120, editing: false },
            { name: "FieldDisplayName", type: "text", title: "Pole Opis", width: 80, headercss: "hdr", css: "textPink", editing: false },
            { name: "OldValue", type: "text", title: "Stara Wartosc", width: 90, filtercss: "fltr", headercss: "hdr", css: "cellOldVal", editing: false, filtering: false },
            { name: "ICON", type: "text", title: "", width: 20, filtercss: "fltr", headercss: "hdr", css: "cellArrow", editing: false, filtering: false, itemTemplate(valie, item) { return '<i class="fas fa-long-arrow-alt-right"></i>'; } },
            { name: "NewValue", type: "text", title: "Nowa Wartosc", width: 90, filtercss: "fltr", headercss: "hdr", css: "cellNewVal", editing: false, filtering: false },
            { name: "User.UserName", type: "text", title: "User", width: 80, css: "", editing: false },
            {
                name: "Date", type: "date", title: "Data", width: 120, css: "", editing: false, filtering: false,
                itemTemplate: function (value, item) { return new moment(value).format('Y-MM-DD HH:mm:ss'); }
            },
            { type: "control", width: 60, modeSwitchButton: true, editButton: false, deleteButton: false }
        ],
        controller: this.gridHelper.DB,
        onDataLoading: function (args) {
            args.filter.Mode = this.mode;
        }, 
        onDataLoaded: function () {
            console.log("grid data loaded");
            self.InitAutocompletes();
        },
        rowClick: function (args) { }
    });
};
ChangeLogGrid.prototype.CreateNewGridInstance = function (divSelector) {
    return new ChangeLogGrid(divSelector);
};
ChangeLogGrid.prototype.GenerateFieldId = function () {
    var field = {};
    var fieldName = "";
    var fieldTitle = "";
    switch (this.mode) {
        case "Item": fieldName = "ParentObjectId"; fieldTitle = "P.Id"; break;
        case "Workstation": fieldName = "ObjectId"; fieldTitle = "P.Id"; break;
        case "AutomaticRule": fieldName = "ObjectId"; fieldTitle = "Id"; break;
        case "Transporter": fieldName = "ObjectId"; fieldTitle = "P.Id"; break;
        case "Package": fieldName = "ObjectId"; fieldTitle = "P.Id"; break;
        default: fieldName = "ObjectId"; fieldTitle = "P.Id"; break;
    }

    field = { name: fieldName, type: "text", title: fieldTitle, width: 40, css: "tbItemId", editing: false, filtering: true };

    field.filterTemplate = function () {
        var $filterControl = jsGrid.fields.text.prototype.filterTemplate.call(this);
        return $filterControl.val(parameters[fieldName]);
    };

    return field;
};
ChangeLogGrid.prototype.GenerateFieldDescription = function () {
    var field = {};
    var fieldName = "";
    var fieldTitle = "";

    switch (this.mode) {
        case "Item": fieldName = "ParentObjectDescription"; fieldTitle = "Kod Art."; break;
        case "Workstation": fieldName = "ObjectDescription"; fieldTitle = "Nazwa Stanowiska"; break;
        case "AutomaticRule": fieldName = "ObjectDescription"; fieldTitle = "PREFIX"; break;
        case "Transporter": fieldName = "ObjectDescription"; fieldTitle = "Nazwa Transp."; break; 
        case "Package": fieldName = "ObjectDescription"; fieldTitle = "Nazwa Opak."; break;
        default: fieldName = "ObjectDescription"; fieldTitle = "Kod Art."; break;
    }

    field = { name: fieldName, title: fieldTitle, type: "text", width: 140, filtering: true, insertcss: "tbItemCode", pageSize: 100, css: "itemCodeColumn textWhiteBold" };

    field.filterTemplate = function () {
        var $filterControl = jsGrid.fields.text.prototype.filterTemplate.call(this);
        return $filterControl.val(parameters[fieldName]);
    };

    return field;

};

ChangeLogGrid.prototype.InitAutocompletes = function () {
    ItemAutcomplete(".tbItemId input", ".itemCodeColumn input", "");
    //ItemAutcomplete("", ".itemCodeColumn input", "");
};