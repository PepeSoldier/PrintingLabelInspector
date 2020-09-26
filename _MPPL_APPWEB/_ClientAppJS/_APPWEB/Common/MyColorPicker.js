function MyColorPicker() {

    this.Init = function (selector, callback) {
        console.log("init MyColorPicker. Selector: " + selector);
        $(selector).ColorPicker({
            color: '#0000ff',
            onShow: function (colpkr) {
                $(colpkr).fadeIn(500);
                return false;
            },
            onHide: function (colpkr) {
                $(colpkr).fadeOut(500);
                return false;
            },
            onSubmit: function (hsb, hex, rgb, el) {
                callback(hsb, hex, rgb, el);
            },
            onBeforeShow: function () {
                var ele2 = this;
                var color1 = getBgColorHex($(ele2).children().first());
                $(this).ColorPickerSetColor(color1);
            }
        });
    };

    function getBgColorHex(elem) {
        var color = elem.css('background-color');
        var hex;
        if (color.indexOf('#') > -1) {
            //for IE
            hex = color;
        } else {
            var rgb = color.match(/\d+/g);
            hex = '#' + ('0' + parseInt(rgb[0], 10).toString(16)).slice(-2) + ('0' + parseInt(rgb[1], 10).toString(16)).slice(-2) + ('0' + parseInt(rgb[2], 10).toString(16)).slice(-2);
        }
        return hex;
    }
}

function MyColorPicker2() {

    this.Init = function (element, cp) {
        console.log("init MyColorPicker2.");
        $(element).ColorPicker({
            color: '#0000ff',
            onShow: function (colpkr) {
                $(colpkr).fadeIn(500);
                return false;
            },
            onHide: function (colpkr) {
                $(colpkr).fadeOut(500);
                return false;
            },
            onSubmit: function (hsb, hex, rgb, el) {
                //callback(hsb, hex, rgb, el);
                cp.css("background-color", "#" + hex);
                $(el).ColorPickerHide();
            },
            onBeforeShow: function () {
                var ele2 = this;
                var color1 = getBgColorHex(cp);
                $(this).ColorPickerSetColor(color1);
            }
        });
    }

    function getBgColorHex(elem) {
        var color = elem.css('background-color')
        var hex;
        if (color.indexOf('#') > -1) {
            //for IE
            hex = color;
        } else {
            var rgb = color.match(/\d+/g);
            hex = '#' + ('0' + parseInt(rgb[0], 10).toString(16)).slice(-2) + ('0' + parseInt(rgb[1], 10).toString(16)).slice(-2) + ('0' + parseInt(rgb[2], 10).toString(16)).slice(-2);
        }
        return hex;
    }
}