
function ItemAutocompleteOEE(valueFieldSelector, textFieldSelector, machineId, cycleTimeSelector) {

    $(valueFieldSelector).focus(function () {
        console.log("AC focus");
        $(this).autocomplete("search");
    });
    
    $(valueFieldSelector).autocomplete({
        minLength: 0,
        source: function (request, response) {
            $.ajax({
                url: "/ONEPROD/OEE/AncAutocomplete",
                type: "GET",
                dataType: "json",
                data: { Prefix: request.term, MachineId: machineId },
                success: function (data) {
                    response($.map(data, function (item) {
                        return {
                            label: item.TextField,
                            value: item.ValueField,
                            cycleTime: parseFloat(item.Data1)
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
            $(cycleTimeSelector).val(ui.item.cycleTime);
            return true;
        }
        //,messages: {
        //    noResults: "", results: function () { }
        //}
    });
    $(valueFieldSelector).autocomplete().autocomplete("instance")._renderItem = function (ul, item) {
        console.log("ItemAutocompleteOEE Render");

        return $("<li>")
            .append(
                "<div>" + item.value +
                "<br><span style='font-size: 12px; color: blue;'>" +
                item.label +
                "</span></div>")
            .appendTo(ul);
    };
    $(valueFieldSelector).autocomplete().autocomplete("instance")._resizeMenu = function () {
        var maxZ = Math.max.apply(null, $.map($('body > *'), function (e, n) {
            return parseInt($(e).css('z-index')) || 1;
        }));
        this.menu.element.css("z-index", maxZ + 1);
        this.menu.element.outerWidth(327);
    };
}