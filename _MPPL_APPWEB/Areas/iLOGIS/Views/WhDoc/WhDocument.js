function ParseDate(x) {
    return moment(x).format().split("T")[0];
}

function WhDocument() {
    var self = this;

    var viewModel = {
        Id: 0,
        ContractorId: 0,
        ContractorName: "",
        ContractorAdress: "", 
        DocumentNumber: "",
        CostCenter: "",
        CostPayer: "",
        Reason: "",
        TruckPlateNumbers: "",
        TrailerPlateNumbers: "",
        ReferrenceDocument: "",
        CreatorName: "",
        IssuerName: "",
        ApproverId: "",
        ApproverName: "",
        DocumentDate: "",
        IssueDate: "",
        StampTime: "",
        DocumentType: "",
        isSigned: false,
        ItemsTotalQty: 0,
        ItemsCount: 0,
        QrCode: "",
        Notice: "",
        MeansOfTransport:"",
        editMode: false,
        viewMode: "hidden",
        isApproved: false,
        WhDocumentItems: [],
    };

    this.Init = function (_viewModel) {
        console.log("WhDocument.Init");
        _setViewModel(_viewModel);
        console.log(viewModel.isApproved);
        Render();
        Actions();
    };
    this.SetQrCode = function (qrCode) {
        console.log("qrCode");
        viewModel.QrCode = qrCode;
    };
    this.Issue = function (callback) {
        var ReturnJson = new JsonHelper().GetPostData("/iLogis/WhDoc/Issue",
            { Id: viewModel.Id });
        ReturnJson.done(function (jsonModel) {

            if (jsonModel.Status > 0) {
                new Alert().Show(jsonModel.MessageTypeString, jsonModel.Message);
            }
            else {
                if (jsonModel.Data != null) {
                    _setViewModel(jsonModel.Data);
                    Render();
                    Actions();
                    callback(viewModel.Id);
                }
                else {
                    new Alert().Show("danger", "Próba podpisania dokumentu nie powiodła się");
                }
            }
        });
    };
    function _setViewModel(_viewModel) {
        viewModel = _viewModel;
        viewModel.editMode = "hidden";
        viewModel.viewMode = "";
        viewModel.WhDocumentItems = _prepareItemsData();
        viewModel.isApproved = viewModel.Status >= 20 && viewModel.Status != 25; //Rejected powinno być mniejsze od 20.
        viewModel.isSigned = viewModel.Status >= 30;
    }
    function _prepareItemsData()
    {
        let whDocumentItems = [];

        for (let i = 0; i < 10; i++)
        {
            if (viewModel.WhDocumentItems != null && viewModel.WhDocumentItems.length > i)
            {
                viewModel.WhDocumentItems[i].Position = i.toString();
                viewModel.WhDocumentItems[i].UnitPriceChange = viewModel.WhDocumentItems[i].UnitPrice.toFixed(2).toString().split('.')[1];
                viewModel.WhDocumentItems[i].UnitPrice = viewModel.WhDocumentItems[i].UnitPrice.toFixed();
                viewModel.WhDocumentItems[i].ValueChange = viewModel.WhDocumentItems[i].Value.toFixed(2).toString().split('.')[1];
                viewModel.WhDocumentItems[i].Value = viewModel.WhDocumentItems[i].Value.toFixed();
                viewModel.WhDocumentItems[i].Id = viewModel.WhDocumentItems[i].Id;
                viewModel.WhDocumentItems[i].UnitOfMeasureString = _getUnitOfMeasure(viewModel.WhDocumentItems[i].UnitOfMeasure);
                
                whDocumentItems.push(viewModel.WhDocumentItems[i]);
            }
            else
            {
                whDocumentItems.push(
                    {
                        Position: i.toString(), Id: "", WhDocumentId: viewModel.Id, 
                        ItemCode: "", ItemName: "", IssuedQty: null, DisposedQty: null,
                        UnitOfMeasure: "1", UnitPrice: "", UnitPriceChange: "", Value: "", ValueChange: ""
                    }
                );
            }
        }
        return whDocumentItems;
    }

    function Save() {
        viewModel = _convertFormToViewModel($("#formId")[0], true);
        _getApproverData();
        if (_isProperValidation()) {
            viewModel.QrCode = "";
            var ReturnJson = new JsonHelper().GetPostData("/iLogis/WhDoc/Update", { item: viewModel });
            ReturnJson.done(function (jsonModel) {
                if (jsonModel.Data == null) {
                    new Alert().Show(jsonModel.MessageTypeString, jsonModel.Message);
                } else {
                    new Alert().Show(jsonModel.MessageTypeString, jsonModel.Message);
                    SwitchToViewMode();
                }
            });
        }      
    } 
    function _convertFormToViewModel(formArray, viewMode = "")
    {
        //serialize data function
        for (var i = 0; i < formArray.length; i++)
        {
            if (formArray[i]['name'].includes("_"))
            {
                let name = formArray[i]['name'].split("_", 1);
                let index = formArray[i]['name'].split("_", 2)[1];

                if (name == "ItemCode") {
                    if (formArray[i]['value'] == "") {
                        i += 8;
                        continue;
                    }
                }

                if (name[0].includes("Change"))
                {
                    viewModel.WhDocumentItems[index][name] = formArray[i]['value'];
                    name[0] = name[0].replace("Change", "");

                    if (viewMode == "") {
                        viewModel.WhDocumentItems[index][name] = parseInt(formArray[i - 1]['value']) + (parseInt(formArray[i]['value']) / 100);
                    }
                }
                else {
                    viewModel.WhDocumentItems[index][name] = formArray[i]['value'];
                }
            }
            else {
                viewModel[formArray[i]['name']] = formArray[i]['value'];
            }
        }
        return viewModel;
    }
    function _getApproverData() {
        viewModel.ApproverId = $("#ApproverId").val();
        if (viewModel.ApproverId != "") {
            viewModel.ApproverName = $("#ApproverId").find('option:selected').text();
        }
        else {
            viewModel.ApproverName = "";
        }
    }
    function _isProperValidation() {
        if (viewModel.ApproverId == "") {
            bootbox.alert("Wybierz zatwierdzającego");
        } else if (viewModel.ContractorName == "") {
            bootbox.alert("Wybierz odbiorcę");
        } else if (viewModel.WhDocumentItems.find(x => x.ItemCode != "") == undefined) {
            bootbox.alert("Brak produktów");
        }else {
            return true;
        }
    }

    function Edit() {
        if (viewModel.isApproved == true || viewModel.isSigned == true)
        {
            bootbox.confirm({
                message: "Jesteś pewny, że chcesz edytować zatwierdzoną wz?",
                size: 'small',
                buttons: {
                    cancel: { label: '<i class="fa fa-times"></i> NIE' },
                    confirm: { label: '<i class="fa fa-check"></i> TAK' }
                },
                callback: function (result) {
                    if (result == true) {
                        _setStatusToEdit().then(
                            () => { SwitchToEditMode(); }
                        );   
                    }
                }
            });
        }
        else {
            SwitchToEditMode();
        }
    }
    function _setStatusToEdit() {
        return new Promise((resolve, reject) => {
            var JsonHelp = new JsonHelper();
            var ReturnJson = JsonHelp.GetPostData("/iLogis/WhDoc/SetStatusToEdit", { id: viewModel.Id });
            ReturnJson.done(function (jsonModel) {
                if (jsonModel.Status == 0) {
                    resolve();
                }
                else {
                    new Alert().Show(jsonModel.MessageTypeString, jsonModel.Message);
                    reject();
                }
            });
        });
    }

    function SwitchToViewMode() {
        $("#ApproverListId").addClass("hidden");
        $("#Save").addClass("hidden");
        $("#View").addClass("hidden");
        $("#btnClearItems").addClass("hidden");
        $("#Edit").removeClass("hidden");
        $("#btnSignInit").removeClass("hidden");
        $("#btnPdf").removeClass("hidden");

        viewModel = _convertFormToViewModel($("#formId")[0], true);
        _getApproverData();

        viewModel.editMode = "hidden";
        viewModel.viewMode = "";
        Render();
    }
    function SwitchToEditMode() {
        $("#ApproverListId").removeClass("hidden");
        $("#Save").removeClass("hidden");
        $("#View").removeClass("hidden");
        $("#btnClearItems").removeClass("hidden");
        $("#Edit").addClass("hidden");
        $("#btnSignInit").addClass("hidden");
        $("#btnPdf").addClass("hidden");
        

        viewModel.isApproved = false;
        viewModel.editMode = "";
        viewModel.viewMode = "hidden";
        Render();
    }
    function ClearItems() {
        $(".itemRow input[type=text]").val("");
    }

    function Render() {

        if (viewModel.isSigned == false)
            DisableElement("#btnPdf");
        else
            EnableElement("#btnPdf");

        let templateName = "WhDocumentWZ.Template.cshtml";
        let vm = viewModel;
        fetch('/Areas/iLOGIS/Views/WhDoc/' + templateName)
            .then((response) => response.text())
            .then((template) => {
                $("#WhDocumentWZ").html(Mustache.render(template, vm));
                if (viewModel.editMode == "") {
                    for (let i = 0; i < 10; i++) {
                        ItemWZAutcomplete("#itemCodeAutocomplete", '[name=' + "ItemCode_" + i + ']', '[name=' + "ItemName_" + i + ']');
                    }
                    debugger;
                    ContractorWithDeliveryItemsAutocomplete(".contractorName", '[name=ContractorName]');
                    InitDatepickers();
                    _render_initSelectedOption();
                }
                else {
                    _render_setSelectedOption();
                }
            });
    }
    function _render_initSelectedOption() {
        let uomArray = $(".selectUnitOfMeasure").toArray();
        uomArray.forEach(function (item, index) {
            let itemVal = $(item).attr("data-val");
            $(item).val(itemVal);
        });
    }
    function _render_setSelectedOption() {
        let uomArray = $(".selectUnitOfMeasureText").toArray();
        uomArray.forEach(function (item, index) {
            let itemVal = _getUnitOfMeasure($(item).attr("data-val"));
            $(item).text(itemVal);
        });
    }
    function _getUnitOfMeasure(val) {
        let retVal = "";

        if (val == 1) retVal = "szt";
        else if (val == 2) retVal = "kg";
        else retVal = "";

        return retVal;
    }

    function ItemWZAutcomplete(idFieldSelector, codeFieldSelector, nameCodeSelector = "") {
        $(codeFieldSelector).autocomplete({
            minLength: 0,
            source: function (request, response) {
                $.ajax({
                    url: "/iLOGIS/WhDoc/ItemWZAutocomplete",
                    type: "POST",
                    dataType: "json",
                    data: { prefix: request.term },
                    success: function (data) {
                        response($.map(data, function (item) {
                            return {
                                label: item.Data1,
                                value: item.TextField,
                                id: item.ValueField
                            };
                        }))
                    },
                    error: function (xhr, ajaxOptions, thrownError) {
                        console.log("error: " + thrownError);
                    }
                })
            },
            focus: function (event, ui) {
                return true;
            },
            select: function (event, ui) {
                console.log(ui);
                $(idFieldSelector).val(ui.item.id);
                $(codeFieldSelector).val(ui.item.value);//ui.item.label);
                $(nameCodeSelector).val(ui.item.label);//ui.item.label);
                return true;
            }
        });
        $(codeFieldSelector).autocomplete().autocomplete("instance")._renderItem = function (ul, item) {
            return $("<li>")
                .append(
                    "<div>" + item.value +
                    "<br><span style='font-size: 12px; color: blue;'>" +
                    item.label +
                    "</span></div>")
                .appendTo(ul);
        }
        $(codeFieldSelector).autocomplete().autocomplete("instance")._resizeMenu = function () {
            this.menu.element.outerWidth(300);
        };
    }

    function Actions() {
        //nazywaj przyciski przedrostkiem "btn"
        $(document).off("click", "#Save");
        $(document).on("click", "#Save", function (event) {
            Save();
        });
        //nazywaj przyciski przedrostkiem "btn"
        $(document).off("click", "#View");
        $(document).on("click", "#View", function (event) {
            SwitchToViewMode();
        });
        //nazywaj przyciski przedrostkiem "btn"
        $(document).off("click", "#Edit");
        $(document).on("click", "#Edit", function (event) {
            Edit();
        });

        $(document).off("click", "#btnPdf");
        $(document).on("click", "#btnPdf", function (event) {
            let id = $(this).attr("data-id");
            var win = window.open("/Uploads/WhDocuments/WZ_" + id + ".pdf", '_blank');
            win.focus();
        });

        $(document).off("click", "#btnClearItems");
        $(document).on("click", "#btnClearItems", function (event) {
            ClearItems();
        });
    }
} 
