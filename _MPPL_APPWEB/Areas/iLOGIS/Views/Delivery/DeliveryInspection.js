
function DeliveryInspectionAbstract(supplierId, deliveryId, deliveryItemListGroupGuid) {
    let viewModel = {
        SupplierId: supplierId,
        DeliveryId: deliveryId,
        Id: deliveryId,
        Guid: deliveryItemListGroupGuid,
        SupplierCode: "",
        SupplierName: "",
        DocumentNumber: "",
        DocumentDate: "",
    };

    this.RenderInfoHeader = function () {
        console.log("RenderInfoHeader");
        GetDeliveryData().then(
            function (result) {
                console.log(viewModel);
                fetch('/Areas/iLOGIS/Views/Delivery/DeliveryInspectionInfoHeader.Template.cshtml')
                    .then((response) => response.text())
                    .then((template) => {
                        $("#deliveryInspectionInfoHeader").html(Mustache.render(template, viewModel));
                    });
            },
            function (error) {
            }
        );
        //RenderTemplate("#deliveryInspectionInfoHeaderTemplate", "#deliveryInspectionInfoHeader", viewModel);
    };

    function GetDeliveryData() {
        
        return new Promise((resolve, reject) =>
        {
            let deliveryJson = new JsonHelper().GetPostData("/iLogis/Delivery/DeliveryGetList",
                {
                    filter: viewModel, pageIndex: 1, pageSize: 1
                });
            deliveryJson.done(function (response) {
                console.log("----------------------------------------------");
                console.log(response);
                let deliveryViewModel = response.data != null && response.data.length > 0 ? response.data[0] : null;
                if (deliveryViewModel != null) {
                    viewModel.SupplierId = deliveryViewModel.SupplierId;
                    viewModel.SupplierCode = deliveryViewModel.SupplierCode;
                    viewModel.SupplierName = deliveryViewModel.SupplierName;
                    viewModel.DocumentId = deliveryViewModel.DocumentId;
                    viewModel.DocumentNumber = deliveryViewModel.DocumentNumber;
                    viewModel.DocumentDate = deliveryViewModel.DocumentDate;
                    viewModel.DeliveryDocuments = response.data;
                    viewModel.IsManyDeliveryDocuments = response.data.length > 1 ? true : false;
                    viewModel.Guid = deliveryViewModel.Guid;
                }
                resolve();
            });
            deliveryJson.fail(function () {
                new Alert().Show("danger", "Dokument nie został odnaleziony");
                reject();
            });            
        });
    }
}

function DeliveryInspection() {
    DeliveryInspectionAbstract.call(this, 0, 0);

    var self = this;
    var supplierName = "";
    var supplierId = 0;
    var deliveryItemListId = 0;
    var deliveryItemListGroupGuid = "";
    this.Init = function () {
        Render();
        Actions();
    };

    this.InitChooseSupplier = function (filters) {
        GetDeliveries(filters);
        Actions();
    };

    var viewModel = {
        SelectedSupplierId: null,
        SupplierCode: null,
        SupplierName2: null,
        Deliveries: [{ Id: 0, DocumentNumber: "", DocumentDate: "" }],
        MarginBottom: 'mb-auto',
    };
   
    function GetDeliveries(filters) {
        var JsonHelp = new JsonHelper();
        let ReturnJson = JsonHelp.GetPostData("/iLogis/Delivery/DeliveryInspectionGetDeliveries", {
            SupplierName: filters.supplierName,
            SupplierCode: filters.supplierCode,
            ItemCode: filters.itemCodeDeliv
        });
        ReturnJson.done(function (deliveryViewModelList) {
            console.log("deliveryViewModelList");
            console.log(deliveryViewModelList);
            viewModel.Deliveries = [];
            deliveryViewModelList.forEach(function (deliveryVM)
            {
                viewModel.SupplierName2 = deliveryVM.SupplierName;
                viewModel.SelectedSupplierId = deliveryVM.SupplierId;
                viewModel.SupplierCode = deliveryVM.SupplierCode;
                viewModel.Deliveries.push({
                    Id: deliveryVM.Id,
                    LocateProgress: deliveryVM.LocateProgress,
                    LocateAssignedProgress: deliveryVM.LocateAssignedProgress,
                    UnLocateProgress: 100 - Math.max(deliveryVM.LocateProgress, deliveryVM.LocateAssignedProgress),
                    DocumentNumber: deliveryVM.DocumentNumber,
                    DocumentDate: deliveryVM.DocumentDate,
                    SupplierName: deliveryVM.SupplierName,
                    Status: deliveryVM.Status,
                    StateIcon: GetStateIcon(deliveryVM.Status),
                    SupplierId: deliveryVM.SupplierId,
                    Guid: deliveryVM.Guid,
                    HasGuid: (deliveryVM.Guid == null || deliveryVM.Guid == "") ? false : true,
                });
            });
            if (viewModel.SupplierName2 == null || viewModel.SupplierName2.length <= 0) {
                new Alert().Show("warning","Nie znaleziono dostaw");
            }

            let suppliers = Array.from(new Set(viewModel.Deliveries.map(x => x.SupplierName)));

            if (suppliers.length > 1) {
                viewModel.SupplierName2 = "Wielu Dostawców";
            }
            Render();
        });
        ReturnJson.fail(function () {
            new Alert().Show("success", "Dokument nie  odnaleziony");
        });
    }

    function ChooseDelivery() {
        if ($(".selectedDeliveryListItem .row").length > 1) {
            deliveryItemListGroupGuid = $(".selectedDeliveryListItem .row").parent().attr("data-guid");
        } else {
            deliveryItemListId = $(".selectedDeliveryListItem .row").attr("data-id");
        }
        supplierId = $(".selectedDeliveryListItem .row").attr("data-supplierId");
        window.location.hash = "#/iLOGIS/Delivery/DeliveryInspectionBlindCheck/?supplierId=" + supplierId + "&deliveryId=" + deliveryItemListId + "&deliveryItemListGroupGuid=" + deliveryItemListGroupGuid;
    }

    function GetStateIcon(state) { 
        console.log(state);
        stateIcon = "";

        if (state <= 0) {
            stateIcon = "far fa-play-circle";
        }
        else if (state == 10) {
            stateIcon = "fas fa-blind";
        }
        else if (state == 20 || 30) {
            stateIcon = "fas fa-flag-checkered";
        }
        else if (state == 25) {
            stateIcon = "fas fa-exclamation-circle";
        }
        
        return stateIcon;
    };
    
    function Render() {
        RenderTemplate("#deliveryInspectionTemplate", "#iLogisWMSDeliveryInspection", viewModel);
        //$($(".deliveryListItem")[0]).addClass("selectedDeliveryListItem");
    }

    function CreateGroup() {
        let _deliveryIds = [];

        $.each($.find(".selectedDeliveryListItem"), function () {
            _deliveryIds.push($(this).children().attr("data-id"));
        });

        var JsonHelp = new JsonHelper();
        let ReturnJson = JsonHelp.GetPostData("/iLogis/Delivery/DeliveryInspectionCreateGroup", {
            deliveryIds: _deliveryIds,
        });
        ReturnJson.done(function (deliveryIds) {
            deliveryIds.forEach(function (item) {
                let delivery = viewModel.Deliveries.find(x => x.Id == item.Id);
                delivery.Guid = item.Guid;
                delivery.HasGuid =  true;
            })
            Render();
            new Alert().Show("success", "Udało się utworzyć grupę");
        });
    }

    function SelectingDeliveries(item) {
        let guid = $(item).attr("data-guid");
        if ($(item).hasClass("selectedDeliveryListItem")) {
                if (guid == "") {
                    $(item).removeClass("selectedDeliveryListItem");
                } else {
                    RemoveBySelector(".selectedDeliveryListItem");
                }
        } else {
                if (guid != "") {
                    RemoveBySelector(".selectedDeliveryListItem");
                    let guidArr = $.find("[data-guid='" + guid + "']");
                    guidArr.forEach(function (itm) {
                        $(itm).addClass("selectedDeliveryListItem");
                    });

                } else {
                    RemoveBySelector("[data-guid != '']");
                    $(item).addClass("selectedDeliveryListItem");
                }
            }
    }

    function RemoveBySelector(selector) {
        $.each($.find(selector), function (item) {
            $(this).removeClass("selectedDeliveryListItem");
        });
    }

    function isMultiSelectActive() {
        return $("#multiSelect").hasClass("active");
    }

    function SelectAllWithTheSameGuid(guid) {
        let guidArr = $.find("[data-guid='" + guid + "']");
        guidArr.forEach(function (itm) {
            $(itm).addClass("selectedDeliveryListItem");
        });
    }

    function DeleteGroup() {
        let _groupGuid = $(".selectedDeliveryListItem").attr("data-guid");
        let isLocationStarted = $(".selectedDeliveryListItem").find(".locateStatus").attr("data-locateAssignedProgress") > 0 ? true : false;

        if (isLocationStarted) {
            bootbox.alert("Nie można rozdzielić ze względu na rozpoczęcie lokalizowania komponentów");
        } else {
            var JsonHelp = new JsonHelper();
        let ReturnJson = JsonHelp.GetPostData("/iLogis/Delivery/DeliveryDeleteGroup", {
           deliveryGroupGuid: _groupGuid,
        });
            ReturnJson.done(function (deliveryListByGroup) {
            deliveryListByGroup.forEach(function (delivery) {
                let deliveryRow = viewModel.Deliveries.find(x => x.Id == delivery.Id);
                if (deliveryRow != null) {
                    deliveryRow.Guid = delivery.Guid;
                    deliveryRow.HasGuid = false;
                }
            })
            Render();
            new Alert().Show("success", "Udało się usunąć grupę");
        });

        }
    }

    function Actions() {
        $(document).off("click", "#chooseSupplier");
        $(document).on("click", "#chooseSupplier", function () {
            filters.supplierCode = $("#supplierCode").val();
            filters.supplierName = $("#supplierName").val(); 
            filters.itemCodeDeliv = $("#itemCodeDeliv").val();
            GetDeliveries(filters);
        });

        $(document).off("click", ".deliveryListItem");
        $(document).on("click", ".deliveryListItem", function () {
            let guid = $(this).attr("data-guid");
            if (isMultiSelectActive()) {
                SelectingDeliveries(this);
            } else {
                if (guid != "") {
                    RemoveBySelector(".deliveryListItem");
                    SelectAllWithTheSameGuid(guid);
                    $("#crtDeliveryGroup").addClass("hidden");
                    $("#rmvDeliveryGroup").removeClass("hidden");
                } else {
                    RemoveBySelector(".deliveryListItem");
                    $("#rmvDeliveryGroup").addClass("hidden");
                    $(this).addClass("selectedDeliveryListItem");
                }
            }
        });

        $(document).off("click", "#multiSelect");
        $(document).on("click", "#multiSelect", function () {
            if (isMultiSelectActive()) { // Wyłączanie multiselektu.
                $(this).removeClass("active");
                $("#crtDeliveryGroup").addClass("hidden");
                $("#rmvDeliveryGroup").addClass("hidden");
                RemoveBySelector(".deliveryListItem");
            } else { // Włączanie multiselektu.
                $(this).addClass("active");
                $("#crtDeliveryGroup").removeClass("hidden");
                $("#rmvDeliveryGroup").addClass("hidden");
                RemoveBySelector(".deliveryListItem");
            }
        });

        $(document).off("click", "#createDeliveryGroup");
        $(document).on("click", "#createDeliveryGroup", function () {
            let numberOfSelectedDeliveryListItem = $.find(".selectedDeliveryListItem");
            if (numberOfSelectedDeliveryListItem.length < 2) {
                bootbox.alert("Zaznacz conajmniej 2 pozycje nieprzypisane do żadnej grupy pochodące od jednego dostawcy.");
            } else {

                let selectedDeliveryItems = $(".selectedDeliveryListItem").toArray();
                let supplierName = $(selectedDeliveryItems[0]).find(".supplierNameText").text().trim()
                let isSupplierTheSame = true;;
                selectedDeliveryItems.forEach(function (item) {
                    if (supplierName != $(item).find(".supplierNameText").text().trim()){
                        isSupplierTheSame = false;
                    }
                });

                if (isSupplierTheSame == false) {
                    bootbox.alert("Zaznaczone pozycje muszą być od jednego dostawcy");
                } else {
                CreateGroup();
                }
            }
        });

        $(document).off("click", "#removeDeliveryGroup");
        $(document).on("click", "#removeDeliveryGroup", function () {
            DeleteGroup();
        });

        $(document).off("click", "#backToStartButton");
        $(document).on("click", "#backToStartButton", function () {
            let startViewModel = {
                SelectedSupplierId: null,
                SupplierCode: null,
                SupplierName2: null,
                Deliveries: [{ Id: 0, DocumentNumber: "", DocumentDate: "" }],
                MarginBottom: 'mb-auto',
            };
            viewModel = startViewModel;
            Render();
        });

        $(document).off("click", "#confirmButton");
        $(document).on("click", "#confirmButton", function () {
            let selectedItems = $(".selectedDeliveryListItem").length;
            if (selectedItems > 0) {
                ChooseDelivery();
            } else {
                bootbox.alert("Zaznacz pozycję do inspekcji");
            }
        });
    }
}