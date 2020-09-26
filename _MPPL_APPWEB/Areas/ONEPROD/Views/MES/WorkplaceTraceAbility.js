function MesWorkplaceTraceability(_mesWorkplace)
{
    var self = this;
    var wnd = null;
    let mesWorkplace = _mesWorkplace;
    this.SelectedTrolley = { Id: 0, Name: "", Utilization: 0, maxQtyInPackage: 0, maxPackagesPerPallet: 1, pickingStrategy: 0 };
    this.TrolleyItems = [{ StockUnitId: 0, ItemCode: 0, SerialNumber: 0, Qty: 0 }];

    this.Init = function () {
        $("#trolley").css("display", "block");
        Actions();
    };
    this.CreateStockUnit = function (itemId, qty, maxQtyPerPackage, serialNumber) {
        new JsonHelper().GetPostData("/iLOGIS/StockUnit/CreateNew", {
            itemId,
            qty,
            maxQtyPerPackage,
            locationId: self.SelectedTrolley.Id,
            serialNumber,
            print: false
        })
        .done(function (response)
        {
            if (response.status > 0) {
                new Alert().Show("danger", TranslateStatus(response.status));
            } else {
                self.SelectedTrolley.Utilization = response.data.WarehouseLocationUtilization;
                RefreshTrolleyButton();
            }
            console.log("StockUnitCreated");
        })
        .fail(function () {
            new Alert().Show("danger", "Wystąpił problem podczas tworzenia opakowania");
        });
    };
    this.DeleteStockUnit = function (serialNumber) {
        let jsh = new JsonHelper().GetPostData("/iLOGIS/StockUnit/DeleteStockUnit",
            {
                serialNumber
            });
        jsh.done(function (response) {
            if (response.status > 0) {
                new Alert().Show("danger", TranslateStatus(response.status));
            } else {
                self.SelectedTrolley.Utilization = response.data.WarehouseLocationUtilization;
                RefreshTrolleyButton();
            }
            console.log("StockUnitDeleted");
        });
        jsh.fail(function () {
            new Alert().Show("danger", "Wystąpił problem");
        });
    };
    this.VerifyTrolley = function (_qty) {
        let isOK = false;

        if (self.SelectedTrolley.Id > 0) {
            if (self.SelectedTrolley.Utilization < 1) {
                console.log("check required utilization");
                if (1 - self.SelectedTrolley.Utilization >= GetRequiredUtilization(_qty)) {
                    isOK = true;
                }
                else {
                    new Alert().Show("danger", "Wybrana ilośc nie zmieści się na wózku");
                }
            }
            else {
                new Alert().Show("danger", "Wybrany wózek jest pełny");
            }
        }
        else {
            new Alert().Show("danger", "Nie wybrano wózka");
        }

        return isOK;
    };
    this.DetachTrolley = function () {
        _DetachTrolley();
    };

    function SaveProductionLogTraceability(prodLogId, barcode, workorderId) {
        let jsPLT = new JsonHelper().GetPostData("/ONEPROD/MES/SaveProductionLogTraceability",
            {
                prodLogId: prodLogId,
                workorderId: workorderId
            });
        jsPLT.done(function (data) {
            if (data != null) {
                console.log(data);
            }
        });
        jsPLT.fail(function () {
            new Alert().Show("danger", "Wystąpił problem. Zapis loga nie powiódł się");
        });
    }
    function ShowTrolleyScanWindow() {
        console.log("ShowTrolleyScan");

        if (wnd != null) {
            wnd.Close();
            wnd = null;
        }
        else {
            wnd = new PopupWindow(630, 472, 470, 1273);
            wnd.Init("windowTrolleyScan", 'Wózek', DestroyTrolleyScanWindow);
            wnd.Show("loading...");

            var template = $('#TrolleyNumberTemplate').html();
            wnd.Show(Mustache.render(template, null));
            GetTrolleyItems();
            //$("#trolleyBarcode").val(self.SelectedTrolley.Name);
        }
    }
    function DestroyTrolleyScanWindow() {
        wnd = null;
    }
    function AttachTrolley(trolleyName) {

        _DetachTrolley();

        let itemId = mesWorkplace.MesWorkplaceWorkorder.SelectedWorkorder.ItemId;

        if (itemId <= 0) {
            new Alert().Show("danger", "Najpierw wybierz zamówienie");
            return;
        }

        let temp = trolleyName.split(' ');
        trolleyName = temp.length > 1 ? temp[1] : temp[0];

        let jh = new JsonHelper();
        let json = jh.GetPostData("/iLOGIS/WarehouseLocation/GetByNameAndTypeForItemId", {
                name: trolleyName,
                itemId,
                type: 30
            });
        json.done(function (trolley) {
            console.log("Workplace.GetTrolleyByName.Done");

            if (trolley.maxQtyInPackage > 0) {
                self.SelectedTrolley = trolley;
                RefreshTrolleyButton();
            }
            else {
                _DetachTrolley();
                new Alert().Show("warning", "Brak definicji powiązania wybranego artykułu z wózkiem");
            }
            wnd.Close();
        });
        json.fail(function () {
            new Alert().Show("danger", "Pobranie danych wózka nie powiodło się");
            //$("#trolleyName").html('<span style="color:red;">Nie znaleziono</span>');
        });
    }
    function _DetachTrolley() {
        self.SelectedTrolley = { Id: 0, Name: "", Utilization: 0, maxQtyInPackage: 0, maxPackagesPerPallet: 1, pickingStrategy: 0 };
        GetTrolleyItems();
        RefreshTrolleyButton();
    }
    function RefreshTrolleyButton() {
        $("#trolleyName").text(self.SelectedTrolley.Name);
        $("#trolleyName").attr("data-trollyeId", self.SelectedTrolley.Id);

        if (self.SelectedTrolley.Id > 0) {
            $("#trolleyInfo").text((self.SelectedTrolley.Utilization * 100).toFixed(2) + "%");
        } else {
            $("#trolleyInfo").text("");
        }
    }
    function GetTrolleyItems() {

        let locName = self.SelectedTrolley.Name;
        locName = locName.length > 0 ? locName : "#locationNotSelected#";

        $("#gridtrolleyitems").html("");

        var stockLocationDetailsGrid = new StockLocationDetailsGrid("#gridtrolleyitems");
        stockLocationDetailsGrid.InitGrid("300px", "");
        stockLocationDetailsGrid.RefreshGrid({ warehouseLocationName: locName });

        //let jh = new JsonHelper();
        //let json = jh.GetPostData("/iLOGIS/StockUnit/StockLocationDetailsGetList", {
        //    warehouseLocationName: locName
        //});
        //json.done(function (browseWarehouseViewModel) {
        //    console.log("Workplace.GetTrolleyByName.Done");
        //    console.log(browseWarehouseViewModel);
        //});
        //json.fail(function () {
        //    new Alert().Show("danger", "Pobranie artykułów nie powiodło się");
        //});
    }
    function GetRequiredUtilization(_qty) {
        let requiredUtilization = 1;

        if (self.SelectedTrolley.pickingStrategy == 1) {
            let qtyToBeLocalized = _qty;
            let qtyPerPackage = self.SelectedTrolley.maxQtyInPackage;
            let packagesPerPallet = self.SelectedTrolley.maxPackagesPerPallet;

            if (packagesPerPallet != 0 && qtyPerPackage != 0) {
                requiredUtilization = 1*qtyToBeLocalized / (packagesPerPallet * qtyPerPackage);
            }
        }
        else {
            let packagesPerPallet = self.SelectedTrolley.maxPackagesPerPallet;
            requiredUtilization = packagesPerPallet != 0 ? 1 / packagesPerPallet : 1;

            if (_qty > self.SelectedTrolley.maxQtyInPackage && self.SelectedTrolley.maxQtyInPackage != 0) {
                requiredUtilization *= _qty / self.SelectedTrolley.maxQtyInPackage;
            }
        }

        return requiredUtilization;
    }

    function TranslateStatus(status) {

        switch (status) {
            case 300: return "Artykuł nie został znaleziony w Magazynie.";
            case 100: return "Lokacja nie została znaleziona.";
            case 120: return "Lokacja jest Pełna.";
            case 125: return "Na lokacji nie ma już wystarczjąco miejsca.";
            case 140: return "Dla niektórych artykułów nie znaleziono lokacji.";
            case 400: return "Nie znaleziono informacji o sposobie pakowania.";
        }
    }
    function Actions() {
        $(document).off("click", "#trolley");
        $(document).on("click", "#trolley", function () {
            ShowTrolleyScanWindow();
        });

        $(document).off("click", "#btnAttachTrolley");
        $(document).on("click", "#btnAttachTrolley", function () {
            let trolleyName = $("#trolleyBarcode").val();
            AttachTrolley(trolleyName);
        });
        $(document).off("click", "#btnDetachTrolley");
        $(document).on("click", "#btnDetachTrolley", function () {
            _DetachTrolley();
        });
    }
}