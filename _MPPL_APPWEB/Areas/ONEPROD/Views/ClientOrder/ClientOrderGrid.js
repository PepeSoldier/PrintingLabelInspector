
function ClientOrderGrid(gridDivSelector, clientName) {

    var self = this;
    var divSelector = gridDivSelector;
    var clients = GetClients();
    var gridHelper = new GridHelper("", "/ONEPROD/ClientOrder");
    //var types = GetTypes();
    var UOMs = GetUOMs();
    var _clientName = clientName;

    this.InitGrid = function () {
        
        $(divSelector).jsGrid({
            width: "100%",
            inserting: true, editing: true, sorting: false, paging: false, filtering: true,
            confirmDeleting: false,
            onItemDeleting: function (args) {
                gridHelper.onItemDeletingBehavior(args, divSelector);
            },
            fields: PrepareFields(),
            controller: gridHelper.DB,
            onDataLoaded: function () { },
            rowClick: function (args) {}
        });
    };

    this.RefreshGrid = function () {
        $(divSelector).jsGrid("search");
    };

    function PrepareFields() {
        console.log("klient: " + _clientName);

        switch (_clientName) {
            case "Electrolux": return PrepareFieldsElectrolux(); break;
            default: return PrepareFieldsDefault(); break;
        }
    }
    function PrepareFieldsElectrolux() {
        return [
            { name: "Id", type: "text", title: "Id", width: 35, filtering: false, editing: false, inserting: false },
            { name: "Resource", type: "text", title: "Linia", width: 90 },
            { name: "ItemCode", title: "Kod", type: "text", width: 70 },
            { name: "ItemName", title: "Nazwa", type: "text", width: 100 },
            { name: "OrderNo", type: "text", title: "Nr Zamówienia", width: 130, filtering: true },
            { name: "Qty_Total", title: "Ilość", type: "text", width: 80 },
            { name: "Qty_Produced", title: "Ilość Gotowa", type: "text", width: 80 },
            { name: "StartDate", title: "Data pocz.", type: "date", width: 100 },
            { name: "EndDate", title: "Data konc.", type: "date", width: 100 },
            { name: "LastUpdateDate", title: "Aktualizacja", type: "date", width: 100, filtering: false, inserting: false, editing: false },
            { name: "Akcje2", type: "control", width: 100, modeSwitchButton: true, editButton: true }
        ]
    }
    function PrepareFieldsDefault() {
        return [
            { name: "Id", type: "text", title: "Id", width: 35, filtering: false, editing: false, inserting: false },
            { name: "Resource", type: "text", title: "Zasób", width: 90 },
            { name: "ClientItemCode", title: "Kod Klienta", type: "text", width: 70 },
            { name: "ClientItemName", title: "Nazwa", type: "text", width: 100 },
            { name: "ItemCode", title: "Kod", type: "text", width: 70 },
            { name: "ItemName", title: "Nazwa", type: "text", width: 100 },
            { name: "OrderNo", type: "text", title: "Nr Zamówienia", width: 130, filtering: true },
            {
                name: "ClientId", type: "select", title: "Klient", width: 180,
                items: clients, valueField: "Id", textField: "Name",
                itemTemplate: function (value, item) {
                    var client = clients.find(x => x.Id == value);
                    return client != null ? client.Name : " - ";
                }
            },
            { name: "Qty_Total", title: "Ilość", type: "text", width: 80 },
            { name: "Qty_Produced", title: "Ilość Gotowa", type: "text", width: 80 },
            {
                name: "UnitOfMeasure", type: "select", title: "J.M.", width: 60, filtering: false,
                items: UOMs, valueField: "Id", textField: "Name",
            },
            { name: "StartDate", title: "Data pocz.", type: "date", width: 100 },
            { name: "EndDate", title: "Data konc.", type: "date", width: 100 },
            { name: "LastUpdateDate", title: "Aktualizacja", type: "date", width: 100, filtering: false, inserting: false, editing: false },
            { name: "Akcje2", type: "control", width: 100, modeSwitchButton: true, editButton: true }
        ]
    }

    function GetClients() {
        var arr = [
            {
                "Id": null,
                "Name": "-",
            }
        ];

        var arrClients = new GridHelper("Client", "/DEF/MasterData").GetList(false, null).responseJSON;

        return arr.concat(arrClients);
    }

    function GetUOMs() {
        return [
            {
                "Id": 0,
                "Name": "szt",
            },
            {
                "Id": 1,
                "Name": "kg",
            },
            {
                "Id": 2,
                "Name": "m",
            }
        ];
    }
}