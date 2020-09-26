
function WorkstationAutcomplete(idFieldSelector, codeFieldSelector, lineFieldSelector) {

    $(codeFieldSelector).autocomplete({
        minLength: 0,
        source: function (request, response) {
            $.ajax({
                url: "/MASTERDATA/Workstation/Autocomplete",
                type: "POST",
                dataType: "json",
                data: { prefix: request.term, lineName: $(lineFieldSelector).val() },
                success: function (data) {
                    response($.map(data, function (item) {
                        return {
                            label: item.TextField,
                            value: item.Data1,
                            id: item.ValueField,
                            lineName: item.Data2
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
            console.log("wybrano: " + ui.item.label);
            $(codeFieldSelector).val(ui.item.label);//ui.item.label);
            $(idFieldSelector).val(ui.item.id);
            $(lineFieldSelector).val(ui.item.lineName);
            //$(nameCodeSelector).val(ui.item.label);//ui.item.label);
            console.log("koniec autocomplete");
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