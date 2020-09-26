
function PackageAutcomplete2(idFieldSelector, codeFieldSelector) {

    $(codeFieldSelector).autocomplete({
        minLength: 0,
        source: function (request, response) {
            console.log("PackageAutcomplete2");
            console.log(request.term);
            $.ajax({
                url: "/iLOGIS/Config/PackageAutocomplete",
                type: "POST",
                dataType: "json",
                data: { p: request.term, w: null, h: null, d: null },
                success: function (data) {
                    console.log("ac data");
                    console.log(data);
                    response($.map(data, function (item) {
                        
                        return {
                            //label: item.Data1,
                            //value: item.TextField,
                            label: item.TextField,
                            value: item.Data1,
                            id: item.ValueField,
                            dimensions: item.Data2,
                            palletH: item.Data3,
                            packagesPerPallet: item.Data4
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
            console.log("new select");
            console.log(codeFieldSelector);
            $(idFieldSelector).val(ui.item.id);
            //$(codeFieldSelector).val(ui.item.value);//ui.item.label); //code
            $(codeFieldSelector).val(ui.item.label);//ui.item.label); //code
            //$(nameCodeSelector).val(ui.item.label);//ui.item.label); //name
            //$(palletHFieldSelector).val(ui.item.palletH);
            //$(packagesPerPalletFieldSelector).val(ui.item.packagesPerPallet);
            return true;
        }
    });
    $(codeFieldSelector).autocomplete().autocomplete("instance")._renderItem = function (ul, item) {
        console.log("rysuje wyniki autokomplita");

        return $("<li>")
            .append(
                "<div>" + item.value +
                "<br><span style='font-size: 12px; color: blue;'>" +
                item.label + " [" + item.dimensions + "]" +
                "</span></div>")
            .appendTo(ul);
    };
    $(codeFieldSelector).autocomplete().autocomplete("instance")._resizeMenu = function () {
        this.menu.element.outerWidth(300);
    };
}

function PackageAutcomplete(idFieldSelector, codeFieldSelector, palletHFieldSelector, packagesPerPalletFieldSelector) {

    $(codeFieldSelector).autocomplete({
        minLength: 0,
        source: function (request, response) {
            console.log("p ac");
            console.log(request);
            var dataJS = "{" + request.term + "}";
            console.log(dataJS);
            var dataST = addQuotes(dataJS);
            console.log(dataST);
            var dataArr = JSON.parse(dataST);
            console.log(dataArr);

            $.ajax({
                url: "/iLOGIS/Config/PackageAutocomplete",
                type: "POST",
                dataType: "json",
                data: dataArr,
                success: function (data) {
                    response($.map(data, function (item) {
                        return {
                            //label: item.Data1,
                            //value: item.TextField,
                            label: item.TextField,
                            value: item.Data1,
                            id: item.ValueField,
                            dimensions: item.Data2,
                            palletH: item.Data3,
                            packagesPerPallet: item.Data4
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
            console.log("new select");
            console.log(codeFieldSelector);
            $(idFieldSelector).val(ui.item.id);
            //$(codeFieldSelector).val(ui.item.value);//ui.item.label); //code
            $(codeFieldSelector).val(ui.item.label);//ui.item.label); //code
            //$(nameCodeSelector).val(ui.item.label);//ui.item.label); //name
            $(palletHFieldSelector).val(ui.item.palletH);
            $(packagesPerPalletFieldSelector).val(ui.item.packagesPerPallet);
            return true;
        }
    });
    $(codeFieldSelector).autocomplete().autocomplete("instance")._renderItem = function (ul, item) {
        console.log("rysuje wyniki autokomplita");

        return $("<li>")
            .append(
                "<div>" + item.value +
                "<br><span style='font-size: 12px; color: blue;'>" +
                item.label + " [" +  item.dimensions + "]" +
                "</span></div>")
            .appendTo(ul);
    };
    $(codeFieldSelector).autocomplete().autocomplete("instance")._resizeMenu = function () {
        this.menu.element.outerWidth(300);
    };

    function addQuotes(str) {
        str = str.replace(/\s+/g, '');
        var str2 = "";
        for (var i = 0; i < str.length; i++) {
            if (i > 0 && (str[i] == '{' || str[i] == '}' || str[i] == ':' || str[i] == ','))
                str2 += '"';
            if (i > 0 && (str[i - 1] == '{' || str[i - 1] == '}' || str[i - 1] == ':' || str[i - 1] == ','))
                str2 += '"';
            str2 += str[i];
        }
        return str2;
    }
}