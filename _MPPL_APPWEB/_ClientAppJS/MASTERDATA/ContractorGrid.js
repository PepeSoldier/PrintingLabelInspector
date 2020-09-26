
function ContractorGrid(gridDivSelector) {

    var self = this;
    var divSelector = gridDivSelector;
    var gridHelper = new GridHelper("Contractor", "/MasterData/Contractor");

    this.InitGrid = function () {
        
        $(divSelector).jsGrid({
            width: "100%",
            inserting: true, editing: true, sorting: false, paging: false, filtering: true,
            confirmDeleting: false,
            fields: [
                { name: "Id", type: "text", title: "Id", width: 35, filtering: false, editing: false, inserting: false },
                { name: "Name", type: "text", title: "Nazwa", width: 190, filtering: true },
                { name: "Code", type: "text", title: "Kod", width: 100, filtering: true },
                { name: "Country", type: "text", title: "Kraj", width: 120, filtering: true },
                { name: "Language", type: "text", title: "Język", width: 120, filtering: false },
                { name: "NIP", type: "text", title: "NIP", width: 120, filtering: false },
                { name: "ContactPersonName", type: "text", title: "Osoba kontaktowa", width: 120, filtering: false },
                { name: "ContactPhoneNumber", type: "text", title: "Numer telefonu", width: 120, filtering: false },
                { name: "ContactEmail", type: "text", title: "Adres email", width: 120, filtering: false },
                { name: "ContactAdress", type: "text", title: "Adres", width: 120, filtering: false },
                { name: "Akcje2", type: "control", width: 100, modeSwitchButton: true, editButton: true }
            ],
            controller: gridHelper.DB,
            onDataLoaded: function () { },
            rowClick: function (args) {}
        });
    };

    this.RefreshGrid = function () {
        $(divSelector).jsGrid("search");
    };
}