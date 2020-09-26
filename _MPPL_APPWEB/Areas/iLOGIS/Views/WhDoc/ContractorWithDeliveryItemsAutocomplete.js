function ContractorWithDeliveryItemsAutocomplete(idFieldSelector, nameFieldSelector) {
    $(nameFieldSelector).autocomplete({
        minLength: 0,
        source: function (request, response) {
            $.ajax({
                url: "/iLOGIS/WhDoc/ContractorWithDeliveryItemsAutocomplete",
                type: "POST",
                dataType: "json",
                data: { prefix: request.term },
                success: function (data) {
                    response($.map(data, function (item) {
                        return {
                            label: item.Contractor.Name,
                            adress: item.Contractor.ContactAdress,
                            docItems: item.WhDocumentItemLightList,
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
            $(idFieldSelector).val(ui.item.label);
            $('[name=ContractorAdress]').val(ui.item.adress);

            $(".itemRow input[type=text]").val("");

            for (let i = 0; i < ui.item.docItems.length; i++) {
                $('[name=ItemCode_' + i + ']').val(ui.item.docItems[i].ItemCode);
                $('[name=ItemName_' + i + ']').val(ui.item.docItems[i].ItemName);
                $('[name=DisposedQty_' + i + ']').val(ui.item.docItems[i].DisposedQty);
                $('[name=UnitOfMeasure_' + i + ']').val(ui.item.docItems[i].UnitOfMeasure);
                $('[name=IssuedQty_' + i + ']').val(ui.item.docItems[i].IssuedQty);
                $('[name=UnitPrice_' + i + ']').val(ui.item.docItems[i].UnitPrice.toFixed());
                $('[name=UnitPriceChange_' + i + ']').val(ui.item.docItems[0].UnitPrice.toFixed(2).toString().split('.')[1]);
                $('[name=Value_' + i + ']').val(ui.item.docItems[i].Value.toFixed());
                $('[name=ValueChange_' + i + ']').val(ui.item.docItems[i].Value.toFixed(2).toString().split('.')[1]);
            }
            return true;
        }
    });
}
