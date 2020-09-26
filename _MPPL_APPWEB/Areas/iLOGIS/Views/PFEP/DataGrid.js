
var DataGrid = function (gridDivSelector) {
    GridBulkUpdate.call(this, gridDivSelector);
    this.divSelector = gridDivSelector;
    this.gridHelper = new GridHelper("Data", "/iLOGIS/PFEP");
};

function UrlExists(url) {
    //var http = new XMLHttpRequest();
    //http.open('HEAD', url, false);
    //http.send();
    //return http.status != 404;
    console.log("check url exists");
    $.get(url)
        .done(function () {
            console.log("url exists");
            return true;
        }).fail(function () {
            console.log("url not exists");
            return false;
        });
}

DataGrid.prototype = Object.create(GridBulkUpdate.prototype);
DataGrid.prototype.constructor = DataGrid;


DataGrid.prototype.UrlExists = function (url) {
    //var http = new XMLHttpRequest();
    //http.open('HEAD', url, false);
    //http.send();
    //return http.status != 404;
    console.log("check url exists");
    $.get(url)
        .done(function () {
            console.log("url exists");
            return true;
        }).fail(function () {
            console.log("url not exists");
            return false;
        });
};
DataGrid.prototype.InitGrid = function () {
    console.log("Init DataGrid");
    var bulkUpdate1 = this.bulkUpdateItem;
    var ids1 = this.idsTable;
    $(this.divSelector).jsGrid({
        title: "PFEP",
        width: "100%", 
        height: function () { return $(this).height() > 400 ? ($(this).height()).toString() + "px" : "400px"; },
        bulkUpdate: bulkUpdate1, ids: ids1,
        inserting: false, editing: true, sorting: false, filtering: true,
        paging: true, pageLoading: true, pageSize: 20, exporting: true,
        confirmDeleting: false,
        onItemDeleting: function (args) {
            this.gridHelper.onItemDeletingBehavior(args, this.divSelector);
        },
        fields: [
            //{ name: "ItemId", type: "text", title: "ItemId", width: 50, filtering: false, editing: false, inserting: false },
            { name: "ItemCode", type: "text", title: "Kod Artykułu", width: 110, css: "textWhiteBold", editing: false }, //itemTemplate: function (value, item) { return  }
            { name: "ItemName", type: "text", title: "Nazwa Artykułu", width: 160, css: "textBlue text-truncate", editing: false },
            //{ name: "ItemGroupId", type: "text", title: "Gr.Id", width: 50 },
            //{ name: "ItemGroupName", type: "text", title: "Grupa", width: 100 },
            //{ name: "WorkstationLineId", type: "text", title: "LineId", width: 50 },
            { name: "WorkstationLineName", type: "text", title: "Linia", width: 50, editing: false },
            //{ name: "WorkstationId", type: "text", title: "S.Id", width: 50 },
            { name: "WorkstationName", type: "text", title: "Stanowisko", width: 80, css: "textPink", editing: false },
            { name: "MaxBomQty", type: "text", title: "Szt / Stan", width: 40, css: "", editing: false }, //---------------
            { name: "MaxPackages", type: "text", title: "Opk / Stan", width: 40, css: "", editing: false, filtering: false }, //---------------
            {
                name: "CheckOnly", type: "checkbox", title: "kontr?", width: 60, css: "", editing: false, filtering: false, itemTemplate: function (value, item) {
                    var cell = "";
                    var checked = value == true ? "checked" : "";
                    var activeInactive = value == true ? "active" : "inactive";
                    return "<div class='itemControll " + activeInactive + "'><input type='checkbox' disabled " + checked + "></div>";
                }
            }, //---------------

            //{ name: "WorkstationSortOrder", type: "text", title: "Sekw.", width: 50 },
            //{ name: "PackageId", type: "text", title: "Op.Id", width: 50 },
            { name: "PackageCode", type: "text", title: "Kod opakow.", width: 100, editing: false },
            { name: "PackageName", type: "text", title: "Opakowanie", width: 160, css: "textGreen", editing: false },
            { name: "QtyPerPackage", type: "text", title: "Szt / Opk", width: 55, css: "", editing: false, filtering: false }, //---------------
            { name: "QtyPerPallet", type: "text", title: "Szt / Pal", width: 55, css: "", editing: false, filtering: false }, //---------------
            
            { name: "PackageD", type: "text", title: "DŁ", width: 40, editing: false },
            { name: "PackageW", type: "text", title: "SZER", width: 40, editing: false },
            { name: "PackageH", type: "text", title: "WYS", width: 40, editing: false },
            { name: "PackageReturnable", type: "checkbox", title: "ZWR?", width: 40, editing: false },
            { name: "DEF", type: "text", title: "DEF", width: 40, editing: false },
            { name: "PREFIX", type: "text", title: "PREFIX", width: 60, editing: false },
            { name: "BC", type: "text", title: "BC", width: 40, editing: false },
            { name: "Class", type: "text", title: "klasa", width: 40, editing: false },
            { name: "ItemCreatedDate", type: "date", title: "Utworzony", width: 130, css: "textDark", editing: false },
            {
                name: "ItemDeleted", type: "checkbox", title: "Aktywny?", width: 60, editing: false, itemTemplate: function (value, item) {
                    var cell = "";
                    var checked = value == true ? "checked" : "";
                    var activeInactive = value == true ? "active" : "inactive";
                    return "<div class='cellDeleted " + activeInactive + "'><input type='checkbox' disabled " + checked + "></div>";
                }
            },
            {
                width: 50, filtering: false, editing: false, inserting: false,
                itemTemplate: function (value, item) {
                    return $("<button>").html('<i class="fas fa-history"></i>')
                        .addClass("btn btn-sm btn-info btnShowChangeLog")
                        .attr("data-transporterId", item.Id)
                        .on("click", function () { new PFEPChangeLog().ShowLogByParentObjectDescription("Item", item.ItemCode); });
                }
            },
            { type: "pdfManageField", title: "Karta Pakowa", width: 75 },
            // { name: "", type: "text", title: "", width: 200 },
            //this.ManageColumn(),
            { type: "control", width: 100, modeSwitchButton: true, editButton: true, deleteButton: false }
        ],
        controller: this.gridHelper.DB,
        onDataLoaded: function () {
            console.log("grid data loaded");
        },
        rowClick: function (args) { }
    });
}
DataGrid.prototype.CreateNewGridInstance = function (divSelector) {
    return new DataGrid(divSelector);
}
var PdfManageField = function (config) {
    jsGrid.Field.call(this, config);
};

PdfManageField.prototype = new jsGrid.Field({
    align: "center",
    itemTemplate: function (value, item) {
        //var url = 'Uploads/' + item.ItemId + '-20.xlsx';
        var url = 'Uploads/' + item.PackingCardUrl;

        if (item.PackingCardUrl != "") {
            var $previewButton = '<a href="' + url + '" data-toggle="tooltip" title="Pobierz XLSX">' +
                '<div class ="btn btn-primary">' + '<span class="fas fa-download"></span>' +
                '</div>' +
                '</a>';
        }
        else {
            var gridtemp = this._grid;
            var $previewButton = '';
        }

        var $parentDiv = $("<div>").attr("id", "UploadFile_1")
                                   .append($previewButton);
        return $("<tr>").append($parentDiv);
    },
    editTemplate: function (value, item) {
        if (item.PackingCardUrl == "") {
            var $deleteButton = '';
            var $uploadButton = $("<input>").attr("type", "file")
                                            .attr("id", "Upload")
                                            .attr("class", "fileInput")
                                            .text("UP")
                                            .on("click", function () {
                                                UploadDocument(item.ItemId);
                                            });
        } else {
            var $uploadButton = '';
            var $deleteButton = $("<button>").attr("type", "button")
                                        .text("Usuń")
                                        .addClass("btn")
                                        .addClass("btn-danger")
                                        .on("click", function () { DeleteDocument(item.ItemId, 20); });
        }

        var $parentDiv = $("<div>").attr("id", "UploadFile_1")
                                   .append($uploadButton)
                                   .append($deleteButton);
        return $("<tr>").append($parentDiv);
    },
});
jsGrid.fields.pdfManageField = PdfManageField;

var PreviewDocument = function (itemId) {
    
}

var UploadDocument = function (value) {
    var attachment = new Attachment("#UploadFile_1", "Attachment", "Upload", value, UploadCallback, 20);
    attachment.Init();
}

var DeleteDocument = function (value) {
    var attachment = new Attachment("", "Attachment", "Delete", value, UploadCallback, 20);
    attachment.Delete();

}

var UploadCallback = function (gridTemp) {
    bootbox.alert("Plik został " + gridTemp, function () { });
};
