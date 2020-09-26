
function TraceabilityGrid(gridDivSelector) {

    var self = this;
    var divSelector = gridDivSelector;
    var gridHelper = new GridHelper("Traceability", "/ONEPROD/Mes");

    this.InitGrid = function () {
        $(divSelector).jsGrid({
            width: "100%",
            inserting: false, editing: false, sorting: false, filtering: false,
            confirmDeleting: false, autoload: false, paging: true, pageLoading: true, pageSize: 15,
            onProductionLogDeleting: function (args) {
                gridHelper.onItemDeletingBehavior(args, divSelector);
            },
            fields: [
                { name: "Id", type: "text", title: "Id", width: 35, filtering: false, visible:false },
                { name: "ResourceName", type: "text", title: "Zasób", width: 65, css: "textBlue" },
                { name: "ItemCode", type: "text", title: "Nr Artykułu", width: 50, css: "textWhiteBold" },
                { name: "ItemName", type: "text", title: "Nazwa Artykułu", width: 120, css: "textDark" },
                { name: "SerialNo", type: "text", title: "Serial No", width: 50, css: "textGreen text-right" },
                //{ name: "WorkplaceName", type: "text", title: "Item Code", width: 75 },
                //{ name: "CostCenter", type: "text", title: "CK", width: 70 },
                { name: "DeclaredQty", type: "number", title: "Qty declared", width: 35 },
                { name: "WorkorderTotalQty", type: "number", title: "Qty planned", width: 35, filtering: false },
                { name: "ClientWorkOrderNumber", type: "text", title: "WO", width: 60, css: "text-center" },
                { name: "InternalWorkOrderNumber", type: "text", title: "Intern.WO", width: 70 },
                { name: "ReasonName", type: "text", title: "Powód", width: 65, css: "" },
                { name: "UserName", type: "text", title: "Użytkownik", width: 45, css: "textDark" },
                { name: "TimeStamp", type: "text", title: "Data", width: 75, filtering: false, cellRenderer: FormatDate, css: "textDark" },
                {
                    width: 20, filtering: false, editing: false, inserting: false,
                    itemTemplate: function (value, item) {
                        return $("<button>").html('<i class="fas fa-bezier-curve"></i>')
                            .addClass("btn btn-sm btn-info btnOpen")
                            .on("click", function () {
                                SelectedRow(item.Id);
                            });
                    }
                }
            ],
            controller: gridHelper.DB,
            onDataLoaded: function () {
                console.log("grid data loaded");
            },
            rowClick: function (args) {
                var TraceabilitId = args.item.Id;
                //SelectedRow(TraceabilitId);
            }
        });
    };
    this.RefreshGrid = function (filter) {
        gridHelper.SetFilter(filter);
        $(divSelector).jsGrid("search");
    };

    function FormatDate(value, item) {
        return $("<td>").append(moment(moment(value).toDate()).format("YYYY-MM-DD HH:mm:ss"));
    }
    function SelectedRow(selectedId) {
        console.log("Selected row");
        ShowTreantWindow(selectedId);
    }
    function ShowTreantWindow(selectedId) {
        try {
            if (wnd !== null)
                wnd.Close();
        }
        catch (e) {
            console.log("catch exception");
        }

        wnd = new PopupWindow(1160, 800, 143, 78);
        wnd.Init("TreantWindow", "Przepływ produktu");
        wnd.AddClass("TreantWindow");
        wnd.Show("loading...");

        let jsTraceabilityTreant = new JsonHelper().GetPostDataAwait("/ONEPROD/MES/TraceabilityTreant", {});
        jsTraceabilityTreant.done(function (view) {
            wnd.Show(view);
            let jsGetNodes = new JsonHelper().GetPostData("/ONEPROD/MES/GetTreantBase",
                {
                    productionLogId: selectedId,
                });
            console.log("TraceabilityTreant");

            jsGetNodes.done(function (data) {
                console.log(data);
                var traceabilityTreant = new TraceabilityTreant("#tree-simple", data);
                traceabilityTreant.RefreshChart();
            });
            jsGetNodes.fail(function (e) {
                console.log(e);
            });
        });
    }
}