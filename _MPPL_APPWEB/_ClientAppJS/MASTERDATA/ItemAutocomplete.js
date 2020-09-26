
function ItemAutcomplete(idFieldSelector, codeFieldSelector, nameCodeSelector = "") {
    console.log("ItemAutocomplete Init");
    $(codeFieldSelector).autocomplete({
        minLength: 0,
        source: function (request, response) {
            $.ajax({
                url: "/MASTERDATA/Item/Autocomplete",
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
                    }));
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    console.log("error: " + thrownError);
                }
            });
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
