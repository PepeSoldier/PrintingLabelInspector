function DeliveryInspectionBlindCheck(supplierId, deliveryId, deliveryItemListGroupGuid) {
    
    DeliveryInspectionAbstract.call(this, supplierId, deliveryId, deliveryItemListGroupGuid);

    var self = this;
    //var supplierId = "";
    //var deliveryId = "";
    var keyPad = null;
    var itemWMSId = 0;
    var itemCode = "";
    var numberOfPackages = 0;
    var qtyInPackage = 0;
    var Sum = 0;
    var packageItemId = 0;

    var viewModel = {
        PackageItemOptions: []
    };
    var viewModelPackageItemOption = {
        Id: 0,
        PackageId: 0,
        PackageCode: "",
        PackageName: "",
        ItemWMSId: 0,
        QtyPerPackages: 0,
        PackagesPerPallet: "",
        WarehouseId: 0,
        WarehouseLocationTypeId: 0,
        PickingStrategy: 0,
    };

    this.Init = function () {
        Render();
        Actions();
        PackageAutcomplete2("#SelectedPackageId", "#packageDef");
        //GetDeliveryData();
    };

    function Save() {
        if (CheckProperData()) {
            
            var JsonHelp = new JsonHelper();
            let ReturnJson = JsonHelp.GetPostData("/iLogis/Delivery/DeliveryInspectionBlindCheckAddItem",
                {
                    deliveryId,
                    deliveryItemListGroupGuid,
                    supplierId,
                    itemCode,
                    numberOfPackages,
                    qtyInPackage
                });
            ReturnJson.done(function (jsonModel) {
                if (jsonModel.Message == "NoError") {
                    Form_Clear();
                    new Alert().Show("success", "Zapisano");
                }
                else {
                    new Alert().Show(jsonModel.MessageTypeString, jsonModel.Message);
                }
            });
            ReturnJson.fail(function () {
                new Alert().Show("danger", "Dokument nie został odnaleziony");
            });
        }
    }

    
    function CheckProperData() {
        itemCode = Form_GetItemCode();
        numberOfPackages = Form_GetNumberOfPackage();
        qtyInPackage = Form_GetQtyInPackage();
        if (itemCode !== "") {
            if (isSumProper()) {
                return true;
            } else {
                //bootBoxAlert("numberOfPackages", "Niepoprawnie wpisana ilość");
                new Alert().Show("warning", "Niepoprawnie wpisana ilość");
                $("#numberOfPackage").focus();
                return false;
            }
        }
        else {
            //bootBoxAlert("itemCode", "Uzupełnij Kod Produktu");
            new Alert().Show("warning", "Uzupełnij Kod Produktu");
            $("#itemCode").focus();
            return false;
        }
    }
    function isSumProper() {
        Sum = Form_GetQtyInPackage() * Form_GetNumberOfPackage();
        if (isNaN(Sum)) {
            return false;
        } else {
            return true;
        }
    }
   
    function Form_Clear() {
        $("#itemCode").val("");
        $("#numberOfPackage").val("");
        $("#qtyInPackage").val("");
        $("#sum").val("");
        Sum = undefined;
    }
    function Form_GetItemCode() {
        return $("#itemCode").val();
    }
    function Form_GetNumberOfPackage() {
        return parseFloat($("#numberOfPackage").val());
    }
    function Form_GetQtyInPackage() {
        return parseFloat($("#qtyInPackage").val());
    }
    function Form_SetSum() {
        console.log("Form_SetSum");
        isSumProper();
        if (isSumProper) {
            $("#sum").val(Sum);
        }
        //Form_GetPackageItems();
    }
    function Form_GetPackageItems() {
        let itemCode = $("#itemCode").val();
        let numberOfPackage = $("#numberOfPackage").val();
        let qtyInPackage = $("#qtyInPackage").val();
        console.log(itemCode);
        var JsonHelp = new JsonHelper();
        let ReturnJson = JsonHelp.GetPostData("/iLogis/Delivery/GetPackageItems", {
            itemCode: itemCode,
            maxQtyPerPackage: qtyInPackage,
            packageId: 0
        });
        ReturnJson.done(function (data) {
            if (data.itemWMS != null) {
                itemWMSId = data.itemWMS.Id;
                itemName = data.itemWMS.Name;
                itemUoM = ConvertUoM(data.itemWMS.UnitOfMeasure);
            }
            else {
                itemWMSId = 0;
                itemName = "";
                itemUoM = "?";
            }
            $("#itemName").text(itemName);
            $("#unitOfMeasure").val(itemUoM);

            if (itemWMSId > 0) {
                $("#sectionPackageItemSelector").removeClass("hidden");
                $("#sectionPackageItemDef").addClass("hidden");
                $("#sectionPackages").addClass("hidden");
                $("#sectionSummary").addClass("hidden");
                viewModel.PackageItemOptions = data.PackageItemViewModelList;
                RenderPackageItemOptions();
            }
            else {
                $("#sectionPackageItemSelector").addClass("hidden");
            }

            //if (packageItemViewModel.length > 0) {
            //    console.log("PackageItem found!");
            //    console.log(packageItemViewModel);

            //    let pi = packageItemViewModel[0];

            //    $("#PackingMethod").text(
            //        pi.PackageName + " - " + pi.QtyPerPackage + "/" + pi.PackagesPerPallet
            //    );
            //}
        });
        ReturnJson.fail(function (event) {
            new Alert().Show("danger", "Dokument nie został odnaleziony");
            
        });
    }

    function Form_SelectPackageItem(qtyPerPackage) {
        $("#sectionPackageItemSelector").addClass("hidden");
        $("#sectionPackageItemDef").addClass("hidden");
        $("#sectionPackages").removeClass("hidden");
        $("#sectionSummary").removeClass("hidden");
        $("#qtyInPackage").val(qtyPerPackage);
        $("#numberOfPackage").focus();
    }

    function UpdatePackageItemDisableFields(){
        $("#packageDef").attr("disabled", "true");
        $("#selectInputWarehouse").attr("disabled", "true").css("background-color","#12292e");
        $("#selectInputWarehouseLocationType").attr("disabled", "true").css("background-color", "#12292e");
    }

    function UpdatePackageItemEnableFields() {
        $("#packageDef").removeAttr("disabled", "true");
        $("#selectInputWarehouse").removeAttr("disabled", "true").css("background-color", "");
        $("#selectInputWarehouseLocationType").removeAttr("disabled", "true").css("background-color", "");
    }

    function UpdatePackageItemSetData(packageItem) {
        $("#SelectedPackageId").val(packageItem.PackageId);
        $("#packageDef").val(packageItem.PackageName);
        $("#qtyInPackageDef").val(packageItem.QtyPerPackage);
        $("#packagesOnPalletDef").val(packageItem.PackagesPerPallet);
        $("#selectInputWarehouse").val(packageItem.WarehouseId);
        $("#selectInputWarehouseLocationType").val(packageItem.WarehouseLocationTypeId);
        $("#selectInputPickingStrategy").val(packageItem.PickingStrategy);
    }

    function Form_ShowAddPackageItem(packageItem) {
        $("#sectionPackageItemSelector").addClass("hidden");
        $("#sectionPackageItemDef").removeClass("hidden");
        $("#sectionPackages").addClass("hidden");
        $("#sectionSummary").addClass("hidden");
        $("#packageCodeDef").focus();
        if (packageItem != null) {
            UpdatePackageItemDisableFields();
            UpdatePackageItemSetData(packageItem[0]);
            $("#btnAddPackageItem").text("Aktualizuj");
        } else {
            UpdatePackageItemEnableFields();
            UpdatePackageItemSetData(viewModelPackageItemOption);
            $("#btnAddPackageItem").text("Dodaj");
        }
    }

    function AddPackageItem() {
        console.log("AddPackageItem");

        var packageItem = {
            Id: packageItemId,
            PackageId: parseInt($("#SelectedPackageId").val()),
            ItemWMSId: itemWMSId,
            WarehouseId: parseInt($("#selectInputWarehouse").val()),
            WarehouseLocationTypeId: parseInt($("#selectInputWarehouseLocationType").val()),
            PickingStrategy: parseInt($("#selectInputPickingStrategy").val()),
            QtyPerPackage: parseFloat($("#qtyInPackageDef").val()),
            PackagesPerPallet: parseInt($("#packagesOnPalletDef").val())
        };

        var JsonHelp = new JsonHelper();
        let ReturnJson = JsonHelp.GetPostData("/iLogis/Config/PackageItemUpdate",
            { item: packageItem });
        ReturnJson.done(function (jsonModel) {
            console.log("zapisano packageItem");
            new Alert().Show("success", "Zapis powiódł się");
            Form_GetPackageItems();
        });
        ReturnJson.fail(function () {
            new Alert().Show("danger", "Zapis nie powiódł się");
        });
    }

    function Render() {
        RenderTemplate("#deliveryInspectionBlindCheckTemplate", "#deliveryInspectionBlindCheck", 0);
        self.RenderInfoHeader();
        RenderDropDownLists();
    }
    function RenderDropDownLists() {
        let whJson = new JsonHelper().GetPostData("/iLogis/Config/WarehouseGetList", { filter: {}, pageIndex: 1, pageSize: 100 });
        whJson.done(function(WarehouseList) {
            console.log("warehouses: down.");
            console.log(WarehouseList);
            let selectList = WarehouseList.data.map(x => ({ "Value": x.Id, "Text": x.Name, "Selected": false }));
            FillDropDownList("#selectInputWarehouse", selectList, false);
        });
        let whLTJson = new JsonHelper().GetPostData("/iLogis/Config/WarehouseLocationTypeGetList", { filter: {}, pageIndex: 1, pageSize: 100 });
        whLTJson.done(function(WarehouseLocationTypeList) {
            console.log("Location Types:");
            let selectList = WarehouseLocationTypeList.data.map(x => ({ "Value": x.Id, "Text": x.Name, "Selected": false }));
            FillDropDownList("#selectInputWarehouseLocationType", selectList, false);
        });
        let psJson = new JsonHelper().GetPostData("/iLogis/Config/PickingStrategyGetList", {});
        psJson.done(function(SelectListItems) {
            console.log("Picking Strategies:");
            FillDropDownList("#selectInputPickingStrategy", SelectListItems, false);
        });
    }
    function RenderKeypad() {   
    }
    function RenderPackageItemOptions() {
        RenderTemplate("#deliveryInspectionPackageItemOptionTemplate", "#sectionPackageItemOptions", viewModel);
    }


    function DeletePackageItem() {
        let packageItem = viewModel.PackageItemOptions.where(x => x.Id == packageItemId);
        packageItem[0].Deleted = true;
        var JsonHelp = new JsonHelper();
        let ReturnJson = JsonHelp.GetPostData("/iLogis/Config/PackageItemUpdate",
            { item: packageItem[0] });
        ReturnJson.done(function (jsonModel) {
            console.log("usunięto packageItem");
            new Alert().Show("success", "Usunięto");
            Form_GetPackageItems();
        });
        ReturnJson.fail(function () {
            new Alert().Show("danger", "Zapis nie powiódł się");
        });
    }

    function Actions() {

        $(document).off("change", "#itemCode");
        $(document).on("keyup", "#itemCode", function (e) {
            Form_GetPackageItems();
        });

        $(document).off("change", "#numberOfPackage, #qtyInPackage");
        $(document).on("keyup", "#numberOfPackage, #qtyInPackage", function (e) {
            console.log("change");
            Form_SetSum();
        });

        $(document).off('focusout', ".multiply");
        $(document).on('focusout', ".multiply", function () {
            Form_SetSum();
        });

        $(document).off('click', "#confirmDelivery");
        $(document).on("click", "#confirmDelivery", function () {
            Save();
        });

        $(document).off('click', "#deletePackageItem");
        $(document).on("click", "#deletePackageItem", function () {
            DeletePackageItem();
        });

        $(document).off('click', "#editPackageItem");
        $(document).on("click", "#editPackageItem", function () {
            let packageItem = viewModel.PackageItemOptions.where(x => x.Id == packageItemId);
            Form_ShowAddPackageItem(packageItem);
        });
        

        $(document).off('click', "#btnAddPackageItem");
        $(document).on("click", "#btnAddPackageItem", function () {
            AddPackageItem();
        });
        $(document).off('click', "#btnPackageItemBack");
        $(document).on("click", "#btnPackageItemBack", function () {
            Form_GetPackageItems();
        });

        $(document).off('click', ".PackageItemOptionRow");
        $(document).on("click", ".PackageItemOptionRow", function () {
            let qtyPerPackage = parseFloat($(this).attr("data-QtyPerPackage"));
            packageItemId = $(this).attr("data-packageItemId");
            Form_SelectPackageItem(qtyPerPackage);
        });
        $(document).off('click', ".PackageItemAddOptionRow");
        $(document).on("click", ".PackageItemAddOptionRow", function () {
            packageItemId = 0;
            Form_ShowAddPackageItem();
        });

        $(document).off("focus", ".keypadNumbers, .keypadLetters");
        $(document).on("focus", ".keypadNumbers, .keypadLetters", function () {
            try {
                keyPad.Close();
                keyPad == null;
            }
            catch(ex){
                console.log(ex);
            }

            if ($(this).attr("id") == "itemCode") {
                keyPad = new KeyPadDigitDoubleRows($(this), "#kaypad116", "P");
                keyPad.Init();
                keyPad.SetOnPutcharCallback(Form_GetPackageItems);
            }
            else {
                keyPad = new KeyPadDigitDoubleRows($(this), "#kaypad116", "P");
                keyPad.Init();
                keyPad.SetOnPutcharCallback(Form_SetSum);
            }
        });

        //$(document).off("focusout", "#itemCode")
        //$(document).on("focusout", "#itemCode", function () {
        //    keypadLocation.Close();
        //});

        $(document).off("click", "#btnJumpToSummary");
        $(document).on("click", "#btnJumpToSummary", function () {
            let deliveryItemListGroupGuid = $("#deliveryInfo").attr("data-guid");
            
            window.location.hash = "#/iLOGIS/Delivery/DeliveryInspectionSummary/?supplierId=" + supplierId + "&deliveryId=" + deliveryId + "&deliveryItemListGroupGuid=" + deliveryItemListGroupGuid;
        });

        $(document).off("click", "#closeButtonBlindCheck");
        $(document).on("click", "#closeButtonBlindCheck", function () {
            var set = true;
            if (filters == undefined) {
                var filters = {
                    supplierCode: "",
                    supplierName: "",
                    itemCodeDeliv: "",
                }
            }
            window.location.hash = "#/iLOGIS/Delivery/DeliveryInspection/?chooseSupplier=" + set + "&supplierCode=" + filters.supplierCode + "&supplierName=" + filters.supplierName + "&itemCodeDeliv=" + filters.itemCodeDeliv;
        });

        $(document).off("click", "#expandDocumentsButton");
        $(document).on("click", "#expandDocumentsButton", function () {
            if ($("#documents").hasClass("hidden")) {
                $("#expandDocumentsButton").text("ZWIŃ");
                $("#documents").removeClass("hidden");
            } else {
                $("#expandDocumentsButton").text("ROZWIŃ");
                $("#documents").addClass("hidden");
            }
        });
    }
}