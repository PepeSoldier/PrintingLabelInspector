  $(document).ready(function () {
    InitDatepickers();
    InitAutocompetes();
    console.log("document ready myscripts.js");
});

var baseUrl = '/';

function BaseUrl() {
    this.link = Get();

    function Get() {
        return $('#BaseUrl').attr('href');
    }
}
function User() {
    this.name = Get();
    function Get() {
        return $('#CurrentUser').attr('userName');
    }
}

function ShowLoadingSnippetWithOverlay() {
    $("#wrapper").append('<div id="overlay_black">' +ShowLoadingSnippet() +'</div>');
}
function RemoveLoadingSnippetWithOverlay() {
    return $("#overlay_black").remove();
}
function ShowLoadingSnippet() {
    return '<div id="loading"><h2>Proszę czekać...</h2><ul class="bokeh"><li></li><li></li><li></li></ul></div>';
}
function ShowLoadingSnippetOnElement(selector) {
    $(selector).html(ShowLoadingSnippet());
}
function RemoveLoadingSnippetFromElement(selector) {
    $(selector).html("");
}
function ShowLoadingSnippetCreatingPickingList() {
    return '<div id="loading"><h2>Proszę czekać Picking Lista jest tworzona ...</h2><ul class="bokeh"><li></li><li></li><li></li></ul></div>';
}
function ShowLoadingSnippetNoText() {
    return '<div id="loading"><ul class="bokeh"><li></li><li></li><li></li></ul></div>';
}

function InitDatepickers() {
    //------------------------------------------------DATETIMEPICKERS
    var dateFrom = $("#dateFrom").text();
    var dateTo = $("#dateTo").text();
    let months_ = ['Styczeń', 'Luty', 'Marzec', 'Kwiecień', 'Maj', 'Czerwiec', 'Lipiec', 'Sierpień', 'Wrzesień', 'Październik', 'Listopad', 'Grudzień'];
    let days_ = ["Nd", "Pn", "Wt", "Śr", "Cz", "Pt", "Sb"];

    $.datetimepicker.setLocale('pl');
    $(".datetimepicker").datetimepicker({
        i18n: {
            pl: {
                months: months_,
                dayOfWeek: days_
            }
        },
        dayOfWeekStart: 1,
        timepicker: true,
        format: 'Y-m-d H:i'
    });
    $.datetimepicker.setLocale('pl');
    $(".datepicker").datetimepicker({
        i18n: {
            pl: {
                months: months_,
                dayOfWeek: days_
            }
        },
        timepicker: false,
        format: 'Y-m-d'
    });

    $(".datetimepickerBeg").datetimepicker({
        i18n: {
            pl: {
                months: months_,
                dayOfWeek: days_
            }
        },
        timepicker: true,
        format: 'Y-m-d H:i',
        minDate: dateFrom,
        maxDate: dateTo
    });
    $(".datetimepickerEnd").datetimepicker({
        i18n: {
            pl: {
                months: months_,
                dayOfWeek: days_
            }
        },
        timepicker: true,
        format: 'Y-m-d H:i',
        minDate: dateFrom,
        maxDate: dateTo
    });
}
function InitAutocompetes() {
   //Line Autocomplete
    $(".LineBox").autocomplete({
        source: function (request, response) {
            $.ajax({
                url: "/Print/LineAutocomplete",
                type: "POST",
                dataType: "json",
                data: { Prefix: request.term },
                success: function (data) {
                    response($.map(data, function (item) {
                        return {
                            label: item
                        };
                    }));
                }
            });
        },
        select: function (event, ui) {
            $(this).val(ui.item.label);
            return false;
        },
        messages: {
            noResults: "", results: function () { }
        }
    });

    $(".PNCBox").autocomplete({
        source: function (request, response) {
            $.ajax({
                url: "/Print/PNCAutocomplete",
                type: "POST",
                dataType: "json",
                data: { Prefix: request.term },
                success: function (data) {
                    response($.map(data, function (item) {
                        return {
                            label: item
                        };
                    }));
                }
            });
        },
        select: function (event, ui) {
            $(this).val(ui.item.label);
            return false;
        },
        messages: {
            noResults: "", results: function () { }
        }
    });
    
    $(".OrdNumberBox").autocomplete({
        source: function (request, response) {
            $.ajax({
                url: "/Print/OrderNumberAutocomplete",
                type: "POST",
                dataType: "json",
                data: { Prefix: request.term },
                success: function (data) {
                    response($.map(data, function (item) {
                        return {
                            label: item
                        };
                    }));
                }
            });
        },
        select: function (event, ui) {
            $(this).val(ui.item.label);
            return false;
        },
        messages: {
            noResults: "", results: function () { }
        }
    });

    $(".ClientNameAC").autocomplete({
        source: function (request, response) {
            $.ajax({
                url: "/PFEP/PackingInstruction/ClientNameAutoComplete",
                type: "POST",
                dataType: "json",
                data: { Prefix: request.term },
                success: function (data) {
                    response($.map(data, function (item) {
                        return {
                            label: item
                        };
                    }));
                }
            });
        }
    });
}

function InitFancyBox3() {
    $("[data-fancybox]").fancybox({
        // Options will go here
        animationEffect: false
    });
}
function InitCreateChild(observationId) {
    $('#AddActionFromObservation').click(function () {
        var url = window.location.pathname;
        var id = url.substring(url.lastIndexOf('/') + 1);
        $.ajax({
            url: baseUrl + "Action/CreateActionFromObservation",
            type: "POST",
            dataType: "json",
            data: { observationId: observationId },
            success: function () {
                location.reload();
            }
        });
    });
}
function InitFileSelect() {
    $('.fileInput').on('fileselect', function (event) {

        console.log("upload start");
        var type = $(this).attr('id').split('_')[1];
        var Id = $(this).attr('id').split('_')[2];

        if (window.FormData !== undefined) {
            var fileUpload = $(this).get(0);
            var files = fileUpload.files;
            UploadFile(Id, files, type);
        }
        else {
            alert("FormData is not supported.");
        }
    });
}
function InitDateField() {

    var MyDateField = function (config) {
        jsGrid.Field.call(this, config);
    };

    MyDateField.prototype = new jsGrid.Field({
        sorter: function (date1, date2) {
            var zm1 = moment(date1);
            var zm2 = moment(date2);
            return zm1 - zm2;
        },

        itemTemplate: function (value) {
            var zm = moment(value).format("YYYY-MM-DD");
            if (zm == "1500-10-01") {
                return "";
            } else {
                return zm;
            }
        },

        filterTemplate: function () {
            var now = new Date();
            this._fromPicker = $("<input>").datepicker({
                dateFormat: 'yy-mm-dd',
                firstDay: 1,
                dayNamesMin: ["Ndz", "Pn", "Wt", "Śr", "Czw", "Pt", "Sb"],
                monthNames: ["Styczeń", "Luty", "Marzec", "Kwiecień", "Maj", "Czerwiec", "Lipiec", "Sierpień", "Wrzesień", "Październik", "Listopad", "Grudzień"],
                showAnim: "blind"
            });
            return $("<div>").append(this._fromPicker);
        },
        filterValue: function () {
            return this._fromPicker[0].value;
        }
    });
    return MyDateField;
}
function InitJsGridDefaults() {
    jsGrid.setDefaults({
        height: "980px",
        width: "100%",
        autoload: true,
        editing: true,
        filtering: true,
        pagerFormat: "Strony: {pierwsza} {poprzednia} {strony} {następna} {ostatnia}  {pageIndex} of {pageCount}",
        pagePrevText: "Poprzednia",
        pageNextText: "Następna",
        pageFirstText: "Pierwsza",
        pageLastText: "Ostatnia",
        pageNavigatorNextText: "...",
        pageNavigatorPrevText: "..."
    });
}

function FillDropDownList(dropDownListSelector, data, addDefaultOption) {
    let selectedIntex = 0;
    let dropdown = $(dropDownListSelector)[0];

    if (addDefaultOption == true) {
        let defaultOption = document.createElement('option');
        defaultOption.text = '';
        defaultOption.value = -1;
        dropdown.add(defaultOption);
    }

    let option;
    for (let i = 0; i < data.length; i++)
    {
        option = document.createElement('option');
        option.text = data[i].Text;
        option.value = data[i].Value;

        if (data[i].Selected == true) {
            selectedIntex = i + (addDefaultOption == true? 1 : 0);
        }
        dropdown.add(option);
    }

    dropdown.prop('selectedIndex', selectedIntex);
}

//needed for grid example iLogis PackageItem.js
$(document).on("focus", ".inputDisabled input", function () {
    console.log("ten jest disabled");
    //$(this).parent().next().find("input").focus();
    $(this).blur();
});

$.fn.removeClassPrefix = function (prefix) {
    this.each(function (i, el) {
        var classes = el.className.split(" ").filter(function (c) {
            return c.lastIndexOf(prefix, 0) !== 0;
        });
        el.className = $.trim(classes.join(" "));
    });
    return this;
};

function RenderTemplate(templateID, contentDivSelector, object, isAppend = false) {
    let template = $(templateID).html();
    let rendered = Mustache.render(template, object);
    if (isAppend == false) {
        $(contentDivSelector).html("");
    }
    $(contentDivSelector).append(rendered);
}
function InitOnScan(callback) {
    UninitOnScan();
    onScan.attachTo(document, {
        scanButtonKeyCode: "0",
        scanButtonLongPressTime: 500,
        timeBeforeScantest: 100,
        avgTimeByChar: 50,
        minLength: 3,
        suffixKeyCodes: [9, 13], // enter-key expected at the end of a scan
        prefixKeyCodes: [],
        ignoreIfFocusOn: false,
        stopPropagation: true,
        preventDefault: true,
        reactToKeydown: true,
        reactToPaste: false,
        singleScanQty: 1,
        onScan: function (sCode, iQty) {
            console.log("onScan: " + sCode);
            callback(sCode, iQty);
        },
        onScanError: function (oDebug) {
            $("#consola2").text('err: ' + JSON.stringify(oDebug));
            console.log("eerrrnno");
            console.log(oDebug);
        },
        keyCodeMapper: function (oEvent) {
            //if (oEvent.which == 35) return '#';
            return onScan.decodeKeyEvent(oEvent);
        }
    });
}
function UninitOnScan() {
    console.log("UninitOnScan");
    try {
        onScan.detachFrom(document);
    } catch (e) {
        console.log("e");
        console.log(e);
    }
}
function SimulateScan(text) {
    for (let char = 0; char < text.length; char++) {
        console.log(text[char]);
        SimulateKey(text[char].charCodeAt(0));
    }
}
function SimulateKey(keyCode, type, modifiers) {
    var evtName = typeof type === "string" ? "key" + type : "keydown";
    var modifier = typeof modifiers === "object" ? modifier : {};

    var event = document.createEvent("HTMLEvents");
    event.initEvent(evtName, true, false);
    event.keyCode = keyCode;

    for (var i in modifiers) {
        event[i] = modifiers[i];
    }

    document.dispatchEvent(event);
}

function doLink(url, data) {
    var templ = "#" + url + "/?";
    var zmienna = 0;
    Object.keys(data).forEach(function (key) {
        templ += zmienna == 0 ? key + "=" + data[key] : "&" + key + "=" + data[key];
        zmienna += 1;
    });
    return templ;
}
function convertFormToArray(formArray) {//serialize data function

    var returnArray = {};
    for (var i = 0; i < formArray.length; i++) {
        returnArray[formArray[i]['name']] = formArray[i]['value'];
    }
    return returnArray;
}

function FormatDotNetDate(date) {
    return new moment(date).format("YYYY-MM-DD");
}
function FormatDotNetDateTime(date) {
    return new moment(date).format("YYYY-MM-DD HH:mm:ss");
}
function ConvertUoM(intValue) {

    switch (intValue) {
        case 0: return "?";
        case 1: return "szt";
        case 2: return "kg";
        case 4: return "kWh";
        case 5: return "M3";
        case 6: return "GJ";

        case 15: return "l";
        case 151: return "ml";

        case 16: return "g";

        case 3: return "m";
        case 31: return "mm";
        case 32: return "cm";

        case 40: return "m2";
        case 41: return "m3";

        case 50: return "FT";

        case 9901: return "BAG";
        case 9902: return "BOTTLE";
        case 9903: return "BUSHEL";
        case 9904: return "CAN";
        case 9905: return "CAR";
        case 9906: return "CASE";

        default: return "?";

        
    }
}
function UnitOfMeasuresList() {
    return [
        {"Id" : 0,   "Name": "?"},
        {"Id" : 1,   "Name": "szt"},
        {"Id" : 2,   "Name": "kg"},
        {"Id" : 4,   "Name": "kWh"},
        //{"Id" : 5,   "Name": "M3"},
        {"Id" : 6,   "Name": "GJ"},
        {"Id" : 15,  "Name": "l"},
        {"Id" : 151, "Name": "ml"},
        {"Id" : 16,  "Name": "g"},
        {"Id" : 3,   "Name": "m"},
        {"Id" : 31,  "Name": "mm"},
        {"Id" : 32,  "Name": "cm"},
        {"Id" : 40,  "Name": "m2"},
        {"Id" : 41,  "Name": "m3"},
        {"Id" : 50,  "Name": "FT"},
        {"Id" : 9901,"Name": "BAG"},
        {"Id" : 9902,"Name": "BOTTLE"},
        {"Id" : 9903,"Name": "BUSHEL"},
        {"Id" : 9904,"Name": "CAN"},
        {"Id" : 9905,"Name": "CAR"},
        {"Id" : 9906,"Name": "CASE"}
    ];
}

function DisableElement(selector) {
    $(selector).css("pointer-events", "none");
    $(selector).attr("disabled", "true");
}
function EnableElement(selector) {
    $(selector).css("pointer-events", "auto");
    $(selector).removeAttr("disabled");
}