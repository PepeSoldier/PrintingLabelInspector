function ItemWMSAutcomplete(idFieldSelector, codeFieldSelector, nameCodeSelector = "", UoMSelector = "") {

    $(codeFieldSelector).autocomplete({
        minLength: 0,
        source: function (request, response) {
            $.ajax({
                url: "/iLOGIS/Config/ItemWMSAutocomplete",
                type: "POST",
                dataType: "json",
                data: { prefix: request.term },
                success: function (data) {
                    response($.map(data, function (item) {
                        return {
                            label: item.Data1,
                            value: item.TextField,
                            id: item.ValueField,
                            uom: item.Data6
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
            //console.log(ui);
            $(idFieldSelector).val(ui.item.id);
            $(codeFieldSelector).val(ui.item.value);//ui.item.label);
            $(nameCodeSelector).val(ui.item.label);//ui.item.label);
            $(UoMSelector).val(ConvertUoM(ui.item.uom));//ui.item.label);
            $(UoMSelector).attr("data-UoM", ui.item.uom);//ui.item.label);
            return true;
        }
    });
    $(codeFieldSelector).autocomplete().autocomplete("instance")._renderItem = function (ul, item) {
        console.log("rysuje wyniki autokomplita");

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
