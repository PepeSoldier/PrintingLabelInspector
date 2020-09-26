//iLOGIS BrowseGrid

var WhDocumentGrid = function (gridDivSelector) {
    console.log("BrowseGridInit");
    GridBulkUpdate.call(this, gridDivSelector);
    var self = this;
    this.divSelector = gridDivSelector;
    this.gridHelper = new GridHelper("WhDocument", "/iLogis/WhDoc");
    this.status = GetStatus();

    function GetStatus() {
        return [
            { "Id": 0, "Name": "Init" },
            { "Id": 10, "Name": "Oczekujący" },
            { "Id": 20, "Name": "Potwierdzony" },
            { "Id": 25, "Name": "Odrzucony" },
            { "Id": 30, "Name": "Podpisany" },
            { "Id": 50, "Name": "Zakończony" },
        ];
    }
};

WhDocumentGrid.prototype = Object.create(GridBulkUpdate.prototype);
WhDocumentGrid.prototype.constructor = WhDocumentGrid;

WhDocumentGrid.prototype.InitGrid = function () {

    var grid = this;
    $(this.divSelector).jsGrid({
        width: "100%",
        bulkUpdate: false,
        inserting: false, editing: false, sorting: false,
        paging: true, pageLoading: true, pageSize: 20,
        confirmDeleting: false, filtering: false,
        fields: [
            { name: "Id", type: "text", title: "Id", width: 25, css: "textDark"},
            { name: "DocumentNumber", type: "text", title: "Nr dok.", width: 40, css: "textGreen" },
            { name: "DocumentDate", type: "date", title: "Data dokumentu", width: 40, css: "textGrayout" },
            { name: "ContractorName", type: "text", title: "Dostawca", width: 60, css: "text-truncate textBlue" },
            //{ name: "CostCenter", type: "text", title: "Centrum Kosztów", width: 100 },
            //{ name: "ReferrenceDocument", type: "text", title: "Nr faktury", width: 40 },
            //{ name: "DocumentType", type: "string", title: "Typ dokumentu", width: 60 },
            { name: "ApproverName", type: "string", title: "Zatwierdzający", width: 80},
            { name: "Status", type: "select", title: "Status", items: this.status, valueField: "Id", textField: "Name", width: 40, css: "textLightOrange"},
            { name: "CreatorName", type: "string", title: "Utworzył", width: 80},
            { name: "SecurityApproverName", type: "string", title: "Ochrona", width: 60},
            { name: "StampTime", type: "date", title: "Data wprowadzenia", width: 40},
            //{ name: "isSigned", type: "checkbox", title: "Podpisano?", width: 25},
            { name: "ItemsCount", type: "text", title: "L. Art.", width: 30 },
            {
                title: "Otwórz",width: 20, filtering: false, editing: false, inserting: false,
                itemTemplate: function (value, item) {
                    //console.log(item);
                    return $("<button>").html('<i class="fas fa-pencil-alt"></i>')
                        .addClass("btn btn-sm btn-info btnOpen")
                        .attr("data-userId", item.Id)
                        .on("click", function () {
                            window.open("/iLogis/WhDoc/Index/" + item.Id, '_blank');
                        });
                }
            },
            {
                title: "Zatw.", width: 20, filtering: false, editing: false, inserting: false,
                itemTemplate: function (value, item) {
                    //console.log(item);
                    return $("<button>").html('<i class="fas fa-check"></i>')
                        .addClass("btn btn-sm btn-info btnOpen")
                        .attr("data-userId", item.Id)
                        .on("click", function () {
                            window.open("/iLogis/WhDoc/IndexMobile/" + item.Id, '_blank');
                        });
                }
            },
            {
                title: "Usuń", width: 20, filtering: false, editing: false, inserting: false,
                itemTemplate: function (value, item) {
                    return $("<button>").html('<i class="fa fa-trash"></i>')
                        .addClass("btn btn-sm btn-danger btnOpen")
                        .attr("data-userId", item.Id)
                        .on("click", function () {
                            Delete(item.Id);
                        });
                }
            },
        ],
        controller: grid.gridHelper.DB,
        onDataLoaded: function () { },
        rowClick: function (args) {
            console.log(args.item);
        },
        rowClass: function (item, itemIndex) {
            return item.Deleted == true? "rowDeleted" : "";
        }
    });
    this.grid = $(this.divSelector).data("JSGrid");
};
WhDocumentGrid.prototype.CreateNewGridInstance = function (divSelector) {
    return new WhDocumentGrid(divSelector);
};

WhDocumentGrid.prototype.RefreshGrid = function (filterData) {
    if (filterData != null) {
        this.gridHelper.SetFilter(filterData);
    }
    $(this.divSelector).jsGrid("search");
};


