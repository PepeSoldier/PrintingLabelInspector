
function PopupWindow(width, height, top = 100, left = 100, flex = false) {
    var self = this;
    var width1 = width;
    var height1 = height;
    var top1 = top;
    var left1 = left;
    var flex1 = flex;
    var style = setStyle();
    var windowSelector = "";
    var window = null;
    var _onCloseCallback = null;
    var _parentSelector = null;

    this.Init = function (windowId, title, onCloseCallback = null) {
        _onCloseCallback = onCloseCallback;
        _parentSelector = //parentSelector != null ? parentSelector : document.body;
            _parentSelector = document.body;

        if ($(document).find(windowSelector).length > 0) {
            $(windowSelector + " .WindowTitle span").text(title);
            $(windowSelector + " .WindowContent2").html(ShowLoadingSnippet());
        } else {
            windowId = windowId + Math.floor(Math.random() * 9999);
            windowSelector = "#" + windowId;

            window =
                '<div id="' + windowId + '" class="PopupWindow" ' + style + '>' +
                    (flex1 == true? GetTemplate_Flex(title) : GetTemplate_(title)) +
                '</div>';
            //$('#overlay').html(window);

            $(document).on("click", windowSelector, function () {
                //$(this).css("z-index", HighestZIndex());
                MoveOnTop();
            });
            $(document).on("click", windowSelector + " .btnClose", function () {
                self.Close();
            });
            $(windowSelector).keyup(function (e) {
                if (e.keyCode === 27) { self.Close(); } //escape btn
            });


            $(_parentSelector).append($(window));
            MoveOnTop();
            $(windowSelector).draggable({
                start: function (event, ui) { MoveOnTop(); },
            });
            //console.log("window appeared");
            //$(windowSelector + " .WindowContent").html("Loading view...");
            $(windowSelector + " .WindowContent2").html(ShowLoadingSnippet());
        }
    };
    this.SetPosition = function (top, left) {

    };
    this.Show = function (htmlContent) {
        //$('#overlay, #overlay-back').fadeIn(250);

        //$(document.body).append($(window));
        //MoveOnTop();
        //$(windowSelector).draggable({
        //    start: function (event, ui) { MoveOnTop(); },
        //});
        $(windowSelector + " .WindowContent2").html(htmlContent);
    };
    this.Close = function (executeCallback = true) {
        //$('#overlay, #overlay-back').fadeOut();
        //$('#overlay').html("");
        $(windowSelector).remove();
        if (_onCloseCallback != null && executeCallback == true) {
            _onCloseCallback();
        }
    };
    this.AddClass = function (className) {
        $(window).addClass(className);
    };

    this.WindowDivSelector = windowSelector;
    this.DivSelector = function () { return windowSelector; };

    function setStyle() {
        let width2 = SetPxOrPrecent(width1);
        let height2 = SetPxOrPrecent(height1);
        let top2 = SetPxOrPrecent(top1.toString());
        let left2 = SetPxOrPrecent(left1.toString());
        let style =
            'style="min-width: ' + width2 +
            '; min-height: ' + height2 +
            '; top: ' + top2 +
            '; left: ' + left2;

        if (width2.includes("px", 1))
            style += '; width: ' + width2;

        //if (height2.includes("px", 1))
        //    style += '; height: ' + height2;

        style += ';"';

        return style;
    };
    function MoveOnTop() {
        var hzIndex = HighestZIndex();
        $(windowSelector).css("z-index", hzIndex);
        $(".modal-backdrop").css("z-index", hzIndex + 1);
        $(".modal").css("z-index", hzIndex + 2);
    }
    function HighestZIndex() {
        var maxZ = Math.max.apply(null, $.map($('body > *:not(.alert-fixed):not(.modal)'), function (e, n) {
            //if ($(e).css('position') == 'absolute')
            return parseInt($(e).css('z-index')) || 1;
        }));

        //console.log("HighestZIndex " + maxZ);
        return maxZ + 1;
    }
    function SetPxOrPrecent(value) {
        var val = '';
        if (!(value > 0) && (value.includes("%", 1) || value.includes("px", 1))) {
            val = value;
        }
        else {
            val = value + 'px';
        }
        return val;
    }
    function GetTemplate_(title) {
        return '' +
        '<div style="position: relative; min-height: 100%; width: 100%;">' +
            '<div class="WindowTitle"><span>' + title + '</span>' +
            '<div class="btn btn-danger btnClose">X</div></div>' +
            '<div class="WindowContent WindowContent2">' +
            '</div > ' +
            '<div style="clear: both;"></div>' +
        '</div>';
    }
    function GetTemplate_Flex(title) {
        return '' +
        '<div style="position: relative; min-height: 100%; width: 100%;">' +
            '<div class="WindowTitle"><span>' + title + '</span>' +
            '<div class="btn btn-danger btnClose">X</div></div>' +
            '<div class="WindowContent WindowContentFlex">' +
                '<div class="h-100 d-flex flex-column">' +
                    '<div class="" style="min-height:100%; position: relative;">' +
                        '<div class="row flex-grow-1 mb-auto no-gutters" id="warehousesContainer">' +
                            '<div class="col-12 WindowContent2" style="overflow-y:scroll;">' +
                            '</div>' +
                        '</div>' +
                    '</div>' +
                '</div>' +
            '</div > ' +
            '<div style="clear: both;"></div>' +
        '</div>';
    }
};