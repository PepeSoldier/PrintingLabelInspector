
function EmployeeAutocomplete(valueFieldSelector, textFieldSelector, shiftCode) {

    $(valueFieldSelector).autocomplete({
        minLength: 0,
        source: function (request, response) {
            $.ajax({
                url: "/ONEPROD/OEE/EmployeeAutocomplete",
                type: "GET",
                dataType: "json",
                data: { Prefix: request.term, shiftCode: shiftCode },
                success: function (data) {
                    response($.map(data, function (item) {
                        return {
                            label: item.TextField,
                            value: item.ValueField
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
            $(valueFieldSelector).val(ui.item.value);
            $(textFieldSelector).val(ui.item.label);
            return true;
        }
    });
    $(valueFieldSelector).autocomplete().autocomplete("instance")._renderItem = function (ul, item) {
        console.log("rysuje wyniki autokomplita");

        return $("<li>")
            .append(
                "<div>" + item.value + "</div>")
            .appendTo(ul);
    }
    $(valueFieldSelector).autocomplete().autocomplete("instance")._resizeMenu =  function() {
        this.menu.element.outerWidth(300);
    };
}