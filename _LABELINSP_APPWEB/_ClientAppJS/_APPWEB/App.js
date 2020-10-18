$(document).ready(function () {
    new LeftMenu().Init();
    new SidebarWrapper().Init();
    
    console.log("document ready (App.js)");
    elem = document.documentElement;
    //new Alert().GetAlertsService(new User().name, 1000);  
});
$(window).resize(function () {
    $("#log").append("<div>Handler for .resize() called.</div>");
});

var iSkin = 0;
var skins = ["darkSkin", "defaultSkin", "nasaSkin"];
$(document).on("click", ".currentPageName", function () {
    //console.log("change skin");
    //$("body").removeClass();
    //$("body").addClass(skins[iSkin % 3]);
    //iSkin++;
});
$(document).on("click", "#swichScreenMode", function () {
    SwitchFullScreenMode();
});

var paddingL = "";
var top1 = "";
var hide = false;
var elem = document.documentElement;
function SwitchFullScreenMode() {
    console.log("swichScreenMode");
    hide = !hide;

    if (hide) {
        openFullscreen();
    }
    else {

        closeFullscreen();
    }
}
function openFullscreen() {
    paddingL = $("#wrapper").css("padding-left");
    top1 = $("#wrapper").css("top");
    console.log(paddingL);
    console.log(top1);
    PrepareFullScreenView();
    if (elem.requestFullscreen) {
        elem.requestFullscreen();
    } else if (elem.mozRequestFullScreen) { /* Firefox */
        elem.mozRequestFullScreen();
    } else if (elem.webkitRequestFullscreen) { /* Chrome, Safari and Opera */
        elem.webkitRequestFullscreen();
    } else if (elem.msRequestFullscreen) { /* IE/Edge */
        elem.msRequestFullscreen();
    }
}
function PrepareFullScreenView() {
    $(".onlyFullScreen").removeClass("hidden");
    $(".notInFullScreen").addClass("hidden");
    $("body").addClass("FullScreenMode");
    $("#topBar").addClass("hidden");
    $("#sidebar-wrapper-left").addClass("hidden");
    $("#wrapper").css("padding-left", 0);
    $("#wrapper").css("top", 0);
}
function closeFullscreen() {
    $(".onlyFullScreen").addClass("hidden");
    $(".notInFullScreen").removeClass("hidden");
    $("body").removeClass("FullScreenMode");
    $("#topBar").removeClass("hidden");
    $("#sidebar-wrapper-left").removeClass("hidden");
    $("#wrapper").css("padding-left", paddingL);
    $("#wrapper").css("top", top1);

    if (document.exitFullscreen) {
        document.exitFullscreen();
    } else if (document.mozCancelFullScreen) { /* Firefox */
        document.mozCancelFullScreen();
    } else if (document.webkitExitFullscreen) { /* Chrome, Safari and Opera */
        document.webkitExitFullscreen();
    } else if (document.msExitFullscreen) { /* IE/Edge */
        document.msExitFullscreen();
    }
}

jQuery.fn.insertAt = function (index, element) {
    var lastIndex = this.children().length;
    if (index < 0) {
        $(this).prepend(element);
    }
    else if (index >= lastIndex) {
        $(this).append(element);
    }
    else {
        //this.children().eq(index).before(this.children().last());
        $(element).insertAfter($(this).children().eq(index));
    }
    return this;
};

function AjaxPost (url, filters) {
    console.log("AjaxPost: " + url);
    return $.ajax({
        async: true, type: "POST", data: filters,
        url: url,
        success: function (data) {
            
        },
        error: function (xhr, ajaxOptions, thrownError) {
            console.log("AjaxPost Error: " + thrownError);
        }
    });
}
function AjaxGet(url, filters) {
    console.log("AjaxGet: " + url);
    return $.ajax({
        async: true, type: "POST", data: filters,
        url: url,
        success: function (data) {

        },
        error: function (xhr, ajaxOptions, thrownError) {
            console.log("AjaxGet Error: " + thrownError);
        }
    });
}

function KeyPadWithChars(outputSelector, placeDivSelector) {
    KeyPad.call(this, outputSelector, placeDivSelector);

    this.DrawKeypad = function () {
        if ($(placeDivSelector).find("#keypad").length <= 0) {

            this.keyDiv = {};
            $(placeDivSelector).append(this.keypadDiv[0]);

            
            keyDiv = $("<div>").attr("class", "keypad-key").attr("data-value", "A").text("A");
            $(this.keypadDiv[0]).append(keyDiv[0]);
            keyDiv = $("<div>").attr("class", "keypad-key").attr("data-value", "W").text("W");
            $(this.keypadDiv[0]).append(keyDiv[0]);
            keyDiv = $("<div>").attr("class", "keypad-key").attr("data-value", "Z").text("Z");
            $(this.keypadDiv[0]).append(keyDiv[0]);
            

            for (var i = 9; i >= 0; i--) {
                keyDiv = $("<div>").attr("class", "keypad-key").attr("data-value", i).text(i);
                $(this.keypadDiv[0]).append(keyDiv[0]);
            }

            keyDiv = $("<div>")
                .attr("class", "keypad-key-delete")
                .attr("data-value", "")
                .attr("id", "btnClear")
                .css("width", "207px")
                .text("<--");
            $(this.keypadDiv[0]).append(keyDiv[0]);
        }
    };

}
function KeyPad(outputSelector, placeDivSelector) {
    var _outputDivSelector = outputSelector;
    this.keypadDiv = $("<div>").attr("id", "keypad");
    var self = this;
        
    this.Init = function () {
        self.DrawKeypad();
    };
    this.DrawKeypad = function () {
        if ($(placeDivSelector).find("#keypad").length <= 0) {

            this.keyDiv = {};
            $(placeDivSelector).append(this.keypadDiv[0]);

            for (var i = 9; i >= 0; i--) {
                keyDiv = $("<div>").attr("class", "keypad-key").attr("data-value", i).text(i);
                $(this.keypadDiv[0]).append(keyDiv[0]);
            }

            keyDiv = $("<div>")
                .attr("class", "keypad-key-delete")
                .attr("data-value", "")
                .attr("id", "btnClear")
                .css("width", "207px")
                .text("<--");
            $(this.keypadDiv[0]).append(keyDiv[0]);
        }
    };
    this.ChangeOutputField = function (outputSelector) {
        _outputDivSelector = outputSelector;
    };

    $(document).off('click', placeDivSelector + " .keypad-key");
    $(document).on("click", placeDivSelector + " .keypad-key", function () {
        console.log("put char");
        let val1 = $(_outputDivSelector).val();
        $(_outputDivSelector).val(val1.toString() + $(this).attr("data-value").toString());
        $(_outputDivSelector).focus();
    });
    $(document).off('click', placeDivSelector + " .keypad-key-delete");
    $(document).on("click", placeDivSelector + " .keypad-key-delete", function () {
        console.log("delete chars");
        let text = $(_outputDivSelector).val();

        if (text.length > 1) {
            text = text.substring(0, text.length - 1);
        } else {
            text = "";
        }
        $(_outputDivSelector).val(text);
        $(_outputDivSelector).focus();
    });
}
function KeyPadMini(outputSelector, placeDivSelector) {
    var _outputDivSelector = outputSelector;
    this.keypadDiv = $("<div>").attr("id", "keypad");
    var self = this;

    this.Init = function () {
        self.DrawKeypad();
    };
    this.DrawKeypad = function () {
        if ($(placeDivSelector).find("#keypad").length <= 0) {

            this.keyDiv = {};
            $(placeDivSelector).append(this.keypadDiv[0]);

            for (var i = 9; i >= 0; i--) {
                keyDiv = $("<div>").attr("class", "keypad-key").attr("data-value", i).text(i);
                $(this.keypadDiv[0]).append(keyDiv[0]);
            }

            keyDiv = $("<div>")
                .attr("class", "keypad-key-delete")
                .attr("data-value", "")
                .attr("id", "btnClear")
                .css("width", "207px")
                .text("<--");
            $(this.keypadDiv[0]).append(keyDiv[0]);
        }
    };
    this.ChangeOutputField = function (outputSelector) {
        _outputDivSelector = outputSelector;
    };

    $(document).off('click', placeDivSelector + " .keypad-key");
    $(document).on("click", placeDivSelector + " .keypad-key", function () {
        console.log("put char");
        let val1 = $(_outputDivSelector).val();
        $(_outputDivSelector).val(val1.toString() + $(this).attr("data-value").toString());
        $(_outputDivSelector).focus();
    });
    $(document).off('click', placeDivSelector + " .keypad-key-delete");
    $(document).on("click", placeDivSelector + " .keypad-key-delete", function () {
        console.log("delete chars");
        let text = $(_outputDivSelector).val();

        if (text.length > 1) {
            text = text.substring(0, text.length - 1);
        } else {
            text = "";
        }
        $(_outputDivSelector).val(text);
        $(_outputDivSelector).focus();
    });
}

function SelectActiveMenuLink(hrefAction) {
    //ho to use?
    //SelectActiveMenuLink(@Html.Raw(Json.Encode(ViewContext.RouteData.Values["action"])));
    console.log("select active");
    var active = $("a.confMenu[data-action='" + hrefAction + "']");
    $(active).addClass("selected");
}