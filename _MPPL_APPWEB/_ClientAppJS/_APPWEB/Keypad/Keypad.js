
function KeyPad(outputSelector, placeDivSelector) {
    this.keypadDiv = $("<div>").attr("id", "keypad");
    var _outputDivSelector = outputSelector;
    var self = this;
    var onPutcharCallback = function() { };

    this.Init = function () {
        self.DrawKeypad();
        self.Actions();
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
    this.SetOnPutcharCallback = function (callback) {
        onPutcharCallback = callback;
    };
    this.PutChar = function (_pressedKey) {
        console.log("put char");
        let val1 = $(_outputDivSelector).val();
        $(_outputDivSelector).val(val1.toString() + $(_pressedKey).attr("data-value").toString());
        $(_outputDivSelector).focus();
        onPutcharCallback();
    };
    this.DeleteChar = function () {
        console.log("delete char");
        let text = $(_outputDivSelector).val();

        if (text.length > 1) {
            text = text.substring(0, text.length - 1);
        } else {
            text = "";
        }
        $(_outputDivSelector).val(text);
        $(_outputDivSelector).focus();
        onPutcharCallback();
    };
    this.Close = function () {
        console.log("close keypad");
        $(placeDivSelector).find("#keypad").remove();
    };

    this.Actions = function () {
        $(document).off('click', placeDivSelector + " .keypad-key");
        $(document).on("click", placeDivSelector + " .keypad-key", function () {
            self.PutChar(this);
        });
        $(document).off('click', placeDivSelector + " .keypad-key-delete");
        $(document).on("click", placeDivSelector + " .keypad-key-delete", function () {
            self.DeleteChar();
        });
        $(document).off('click', placeDivSelector + " .keypad-key-close");
        $(document).on("click", placeDivSelector + " .keypad-key-close", function () {
            self.Close();
        });
    };
}

function KeyPadWithChars(outputSelector, placeDivSelector) {
    KeyPad.call(this, outputSelector, placeDivSelector);

    var self = this;

    this.Init = function () {
        self.DrawKeypad();
        self.Actions();
    };

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

function KeyPadDigitDoubleRowsWithoutClose(outputSelector, placeDivSelector, sign) {
    KeyPadWithChars.call(this, outputSelector, placeDivSelector);

    var _outputDivSelector = outputSelector;
    var self = this;

    this.Init = function () {
        self.DrawKeypad();
        self.Actions();
    };

    this.DrawKeypad = function () {
        if ($(placeDivSelector).find("#keypad").length <= 0) {
            this.keyDiv = {};
            this.keypadDiv = $("<div>").attr("id", "keypad");
            $(placeDivSelector).append(this.keypadDiv[0]);

            for (var i = 6; i >= 0; i--) {
                keyDiv = $("<div>").attr("class", "keypad-key-ddr").attr("data-value", i).text(i);
                $(this.keypadDiv[0]).append(keyDiv[0]);
            }
            for (var i = 9; i >= 7; i--) {
                keyDiv = $("<div>").attr("class", "keypad-key-ddr").attr("data-value", i).text(i);
                $(this.keypadDiv[0]).append(keyDiv[0]);
            }
            keyDiv = $("<div>").attr("class", "keypad-key-ddr").attr("data-value", sign).text(sign);
            $(this.keypadDiv[0]).append(keyDiv[0]);

            keyDiv = $("<div>")
                .attr("class", "keypad-key-delete-ddr")
                .attr("data-value", "")
                .attr("id", "btnClear")
                .css("width", "133")
                .css("line-height", "41px")
                .text("<--");
            $(this.keypadDiv[0]).append(keyDiv[0]);
        }
    };

    this.Actions = function () {
        $(document).off('click', placeDivSelector + " .keypad-key-ddr");
        $(document).on("click", placeDivSelector + " .keypad-key-ddr", function () {
            self.PutChar(this);
        });
        $(document).off('click', placeDivSelector + " .keypad-key-delete-ddr");
        $(document).on("click", placeDivSelector + " .keypad-key-delete-ddr", function () {
            self.DeleteChar();
        });
        $(document).off('click', placeDivSelector + " .keypad-key-close");
        $(document).on("click", placeDivSelector + " .keypad-key-close", function () {
            self.Close();
        });
    };
}

function KeyPadDigitDoubleRows(outputSelector, placeDivSelector, sign) {
    KeyPadWithChars.call(this, outputSelector, placeDivSelector);

    var _outputDivSelector = outputSelector;
    var self = this;

    this.Init = function () {
        self.DrawKeypad();
        self.Actions();
    };

    this.DrawKeypad = function () {
        if ($(placeDivSelector).find("#keypad").length <= 0) {
            this.keyDiv = {};
            this.keypadDiv = $("<div>").attr("id", "keypad");
            $(placeDivSelector).append(this.keypadDiv[0]);

            var i = 0;
            for (i = 6; i >= 0; i--) {
                keyDiv = $("<div>").attr("class", "keypad-key-ddr").attr("data-value", i).text(i);
                $(this.keypadDiv[0]).append(keyDiv[0]);
            }
            for (i = 9; i >= 7; i--) {
                keyDiv = $("<div>").attr("class", "keypad-key-ddr").attr("data-value", i).text(i);
                $(this.keypadDiv[0]).append(keyDiv[0]);
            }
            keyDiv = $("<div>").attr("class", "keypad-key-ddr").attr("data-value", sign).text(sign);
            $(this.keypadDiv[0]).append(keyDiv[0]);

            keyDiv = $("<div>")
                .attr("class", "keypad-key-delete-ddr")
                .attr("data-value", "")
                .attr("id", "btnClear")
                .css("width", "88")
                .css("line-height", "41px")
                .text("<--");
            $(this.keypadDiv[0]).append(keyDiv[0]);

            keyDiv = $("<div>")
                .attr("class", "keypad-key-close btn-danger fas fa-times")
                .attr("data-value", "")
                .attr("id", "btnClose")
                .css("font-size", "40px");
            $(this.keypadDiv[0]).append(keyDiv[0]);
        }
    };

    this.Actions = function () {
        $(document).off('click', placeDivSelector + " .keypad-key-ddr");
        $(document).on("click", placeDivSelector + " .keypad-key-ddr", function () {
            self.PutChar(this);
        });
        $(document).off('click', placeDivSelector + " .keypad-key-delete-ddr");
        $(document).on("click", placeDivSelector + " .keypad-key-delete-ddr", function () {
            self.DeleteChar();
        });

        $(document).off('click', placeDivSelector + " .keypad-key-close");
        $(document).on("click", placeDivSelector + " .keypad-key-close", function () {
            self.Close();
        });
    };
}
