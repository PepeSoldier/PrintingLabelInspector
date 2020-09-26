

function MesWorkplace(_workplaceId, _workplace)
{
    var self = this;
    let workplace = _workplace;
    var wnd = null;
    //var resourceId = 0;
    var serialNoResourceId = 0; //_serialNoResourceId;    
    var interval = null;
    var startShiftTime = null;
    var isBusy = false;
    var isTraceabilityON = false;
    let raportHourlySaved = false;

    //Public GETTERS, SETTERS
    this.Id = _workplaceId;
    this.MesWorkplaceBuffer = {};
    this.MesWorkplaceTraceability = {};
    this.MesWorkplaceWorkorder = {};
    this.SelectedTrolleyQty = 0;
    this.ReportOnline = {};
    this.IsTraceability = workplace.IsTraceability;
    //this.SelectedTrolleyId = 339;

    this.Init = function () {
        self.Id = workplace.Id;
        isTraceabilityON = workplace.IsTraceability;
        serialNoResourceId = workplace.ResourceGroupId != null? workplace.ResourceGroupId : workplace.ResourceId;

        Actions();
        startShiftTime = CalcStartShiftTime(0);
        StartInterval();

        if (isTraceabilityON == true) {
            console.log("Traceability is ON");
            self.MesWorkplaceTraceability = new MesWorkplaceTraceability(this);
            self.MesWorkplaceTraceability.Init();

            self.MesWorkplaceBuffer = new MesWorkplaceBuffer(this);
            self.MesWorkplaceBuffer.Init();
        }

        if (workplace.IsReportOnline == true) {
            self.ReportOnline = new ReportOnline(workplace.Id, workplace.ResourceId, self);
            self.ReportOnline.Init("#rightMiddle");
        }

        self.MesWorkplaceWorkorder = new MesWorkplaceWorkorder(this);
        self.MesWorkplaceWorkorder.Init();
    };

    function StartInterval() {
        StopInterval();
        interval = setInterval(function () {
            Thread();
        }, 5000);
    }
    function StopInterval() {
        window.clearInterval(interval);
        interval = null;
    }
    function Thread() {
        let currentMinute = GetCurrentMinute();
        //console.log("Current Minute: " + currentMinute);
        if (currentMinute < 480 && currentMinute % 60 != 0) {
            raportHourlySaved = false;
        }
        //Zapis co godzinę
        if (currentMinute < 480 && currentMinute % 60 == 0 && raportHourlySaved == false) {
            raportHourlySaved = true;
            if (workplace.IsReportOnline == true) {
                let producedQty = parseInt($("#prodEntryType-10").text());

                if (producedQty > 0) {
                    $("#btnReport").click();
                    self.ReportOnline.Save(false);
                }
            }
        }

        //Zapis na koniec zmiany
        if (currentMinute >= 480) {
            StopInterval();
            let producedQty = parseInt($("#prodEntryType-10").text());

            if (workplace.IsReportOnline == true && producedQty > 0) {
                $("#btnReport").click();
                self.ReportOnline.Save(true);
            }
            else {
                document.getElementById('logoutFormOneprodMes').submit();
            }
        }
    }
    function GetCurrentMinute() {
        var end1 = moment(new Date());
        var dur = moment.duration(end1.diff(startShiftTime));
        var minute = parseInt(dur.asMinutes());
        return minute > 480 ? 481 : minute;
    }

    this.OpenPartialyConfirmationWindow = function () {
        wnd = new PopupWindow(850, 200);
        wnd.Init("windowWorkorderConfirmedQty", 'Wprowadz ilość ' + self.MesWorkplaceWorkorder.SelectedWorkorder.Id);
        wnd.Show("loading...");

        $.get("/ONEPROD/MES/ConfirmWorkorder/?workorderId=" + self.MesWorkplaceWorkorder.SelectedWorkorder.Id + "&workplaceId=" + self.Id,
            function (data) {    
                wnd.Show(data);
        });
    };
    this.ConfirmWorkorderPartialy = function (_qty) {
        var pmrow = $(document).find("#planMonitorRow_" + self.MesWorkplaceWorkorder.SelectedWorkorder.Id);
        console.log(_qty);
        $.ajax({
            url: "/ONEPROD/MES/ConfirmWorkorder",
            type: "POST",
            data: '{workorderId: ' + self.MesWorkplaceWorkorder.SelectedWorkorder.Id + ', qty: ' + _qty + '}',
            contentType: 'application/json; charset=utf-8',
            success: function (result) {
                $(pmrow).html($(result + " .planMonitorRow").html());
            }
        });
    };
    function GenerateSerialNumber(resourceId) {
        return new Promise((resolve, reject) => {
            new JsonHelper().GetPostData("/ONEPROD/MES/GenerateSerialNumber", { resourceId: resourceId })
                .done(function (serialNumber) {
                    console.log("Workplace.jsGenSerialNo.done");
                    resolve(serialNumber);
                })
                .fail(function () {
                    console.log("Workplace.jsGenSerialNo.fail");
                    new Alert().Show("danger", "Wygenerowanie numeru seryjnego nie powiodło się");
                    reject();
                });
        });
    }
    this.ConfirmProduction = function ()
    {
        let _qty = mesWorkplace.SelectedTrolleyQty;

        if (CheckConditionsForDeclaration(_qty))
        {
            $("#btn1Piece").addClass("brdActive");

            GenerateSerialNumber(serialNoResourceId).then(
                function (serialNumber) {
                    $("#btn1Piece").removeClass("brdActive");
                    PrintLabel(_qty, serialNumber);
                    self.ConfirmWorkorderByTrolleyQty(_qty, serialNumber);

                    if (serialNumber < 0) 
                        new Alert().Show("danger", "Wygenerowanie numeru seryjnego nie powiodło się");
                    if ($(".selectTrolleyQty.selectedRow").hasClass("customTrolleyQty"))
                        self.SelectWorkorder();
                },
                function () {
                    $("#btn1Piece").removeClass("brdActive");
                }
            );
        }        
    };
    this.ScrapProduction = function () {
        if(!CheckConditionsForDeclaration(1)) return;

        self.CustomScrapQty().then(
            (_qty) => {
                if (CheckConditionsForDeclaration(_qty))
                {
                    GenerateSerialNumber(serialNoResourceId).then(
                        function (serialNumber) {
                            self.ScrapWorkorderByTrolleyQty(_qty, serialNumber);

                            if (serialNumber < 0)
                                new Alert().Show("danger", "Wygenerowanie numeru seryjnego nie powiodło się");
                            if ($(".selectTrolleyQty.selectedRow").hasClass("customTrolleyQty"))
                                self.SelectWorkorder();
                        },
                        function() { }
                    );
                }
            },
            () => {
                new Alert().Show("info", "Deklaracja scrapu została przerwana");
            }
        );
        
    };
    this.ConfirmWorkorderByTrolleyQty = function (_qty, _serialNumber) {

        if (isBusy != true)
        {
            isBusy = true;
            $("#btn1Piece").addClass("brdActive");

            let jsConfirmWo = new JsonHelper().GetPostData("/ONEPROD/MES/ConfirmWorkorderByTrolleyQty",
                {
                    workorderId: self.MesWorkplaceWorkorder.SelectedWorkorder.Id,
                    qty: _qty,
                    serialNo: _serialNumber,
                    workplaceId: self.Id
                });
            jsConfirmWo.done(function (jsonModel) {
                isBusy = false;
                $("#btn1Piece").removeClass("brdActive");

                self.MesWorkplaceWorkorder.SelectedWorkorder.RemainQty -= _qty;
                self.MesWorkplaceWorkorder.RefreshConfirmedWorkorders(jsonModel.Data.updatedWorkordersIds);

                if (isTraceabilityON) {
                    self.MesWorkplaceTraceability.CreateStockUnit(
                        self.MesWorkplaceWorkorder.SelectedWorkorder.ItemId,
                        _qty,
                        _qty,
                        _serialNumber
                    );
                }

                if (workplace.IsReportOnline == true) {
                    $("#labelShift").attr("data-minusHours", 0);
                    self.ReportOnline.SetBtnShiftText();
                    //self.ReportOnline.Refresh(true); //wykomentowane bo dziala juz signalR
                }
            });
            jsConfirmWo.fail(function () {
                isBusy = false;
                $("#btn1Piece").removeClass("brdActive");
                new Alert().Show("danger", "Wystąpił problem. Deklaracja nie powiodła się");
            });
        }
        //else {
        //    new Alert().Show("warning", "Trwa przetwarzanie poprzedniej operacji. Proszę czekać...");
        //}
    };
    this.ScrapWorkorderByTrolleyQty = function (_qty, _serialNo)
    {
        if (isBusy != true) {
            isBusy = true;
            let jsConfirmWo = new JsonHelper().GetPostData("/ONEPROD/MES/ConfirmWorkorderByTrolleyQty",
                {
                    workorderId: self.MesWorkplaceWorkorder.SelectedWorkorder.Id,
                    qty: _qty,
                    serialNo: _serialNo,
                    workplaceId: self.Id,
                    entryType: 12
                });
            jsConfirmWo.done(function (jsonModel) {
                isBusy = false;
                self.MesWorkplaceWorkorder.RefreshConfirmedWorkorders(jsonModel.Data.updatedWorkordersIds);
                //if (report != null) {
                if (workplace.IsReportOnline == true) {
                    $("#labelShift").attr("data-minusHours", 0);
                    self.ReportOnline.SetBtnShiftText();
                    self.ReportOnline.Refresh();

                    if (jsonModel.Data.createdProdLogsIds.length > 0) {
                        self.ReportOnline.ShowReasonSelectorWindow(0, jsonModel.Data.createdProdLogsIds[0]);
                    }
                }
            });
            jsConfirmWo.fail(function () {
                isBusy = false;
                new Alert().Show("danger", "Wystąpił problem. Deklaracja nie powiodła się");
            });
        }
    };
    this.SelectTrolleyQty = function (element) {
        let text = $($(element).find(".trolleyQty")[0]).text();
        if (text == "?") {
            self.CustomTrolleyQty(element);
        }
        else
        {
            self.SelectedTrolleyQty = parseInt(text);
            $("#btn1Piece .trolleyQty").text(self.SelectedTrolleyQty);
            $("#actionButtons .selectTrolleyQty").removeClass("selectedRow");
            $(element).addClass("selectedRow");

            self.MesWorkplaceWorkorder.SetStatusActive();
        }
    };

    this.CustomScrapQty = function (element) {
        return new Promise((resolve, reject) => {
            let template =
                '<div id="qtyEditor" class="row no-gutters">' +
                '<div class="col-12" id="wrkstInventory" style="text-align: center;">' +
                '<div class="row no-gutters">' +
                '<div class="col-12">Wprowadź ilość</div>' +
                '<div class="col-12"><input id="typedQty" inputmode="none" style="height: 55px;width: 316px;font-size: 40px;padding: 0 10px;"></div>' +
                '</div>' +
                '</div>' +
                '<div class="col-12" id="kaypad113"></div>' +
                '<div class="col-12 mt-4" style="text-align: center;">' +
                '<div id="btnSetQtyScrap" class="btn btn-lg btn-success" style="height:60px;width:200px;font-size:30px;">Potwierdz</div>' +
                '</div>' +
                '</div>';

            var wnd = new PopupWindow(370, 200, 367, 500);
            wnd.Init("typeQtyWnd", "Wprowadz zescrapowaną ilość", reject);
            wnd.Show(template);

            var keypad = new KeyPad("#typedQty", "#kaypad113");
            keypad.Init();

            $("#qtyEditor").unbind();
            $("#qtyEditor").on("click", "#btnSetQtyScrap", function () {
                let typedQty = $("#typedQty").val();
                let qty = parseInt(typedQty.length > 0 ? typedQty : "0");
                console.log("qty:" + qty);
                $($(element).find(".trolleyQty")[0]).text(qty);
                wnd.Close(false);
                resolve(qty);
            });
        });
    };

    this.CustomTrolleyQty  = function (element) {
        let template =
            '<div id="qtyEditor" class="row no-gutters">' +
            '<div class="col-12" id="wrkstInventory" style="text-align: center;">' +
            '<div class="row no-gutters">' +
            '<div class="col-12">Wprowadź ilość</div>' +
            '<div class="col-12"><input id="typedQty" inputmode="none" style="height: 55px;width: 316px;font-size: 40px;padding: 0 10px;"></div>' +
            '</div>' +
            '</div>' +
            '<div class="col-12" id="kaypad113"></div>' +
            '<div class="col-12 mt-4" style="text-align: center;">' +
            '<div id="btnSetQty" class="btn btn-lg btn-success">Potwierdz</div>' +
            '</div>' +
            '</div>';

        var wnd = new PopupWindow(370, 200, 380, 1480);
        wnd.Init("typeQtyWnd", "Wprowadz wyprodukowaną ilość");
        wnd.Show(template);

        var keypad = new KeyPad("#typedQty", "#kaypad113");
        keypad.Init();

        $("#qtyEditor").unbind();
        $("#qtyEditor").on("click", "#btnSetQty", function () {
            let textNew = $("#typedQty").val();
            $($(element).find(".trolleyQty")[0]).text(textNew);
            wnd.Close();
            self.SelectTrolleyQty(element);
            $($(element).find(".trolleyQty")[0]).text("?");
        });

    };
    this.WorkorderChanged = function (hasItemCodeChanged) {

        $("#btn1Piece .trolleyQty").text(0);        

        if (hasItemCodeChanged == true)
        {
            if (isTraceabilityON == true) {
                self.MesWorkplaceTraceability.DetachTrolley();
            }
        }
    };

    function CheckConditionsForDeclaration(_qty)
    {
        let isOK = false;
        let woRemainQty = mesWorkplace.MesWorkplaceWorkorder.SelectedWorkorder.RemainQty;

        if (woRemainQty > 0 && _qty > 0) {
            isOK = true;

            if (isTraceabilityON == true) {
                isOK = self.MesWorkplaceTraceability.VerifyTrolley(_qty);   
            }
        }
        else {
            if (_qty <= 0)
                new Alert().Show("danger", "Wybierz ilość do zadeklarowania");
            else
                new Alert().Show("danger", "Wybierz nowe zlecenie. Obecnie wybrane jest zakończone");
        }

        return isOK;
    }
    function PrintLabel(_qty, _serialNo) {
        let jsPrintoutLabel = new JsonHelper().GetPostData("/ONEPROD/MES/PrintoutLabel",
            {
                "workplaceId": self.Id,
                "workorderId": self.MesWorkplaceWorkorder.SelectedWorkorder.Id,
                "qty": _qty,
                "serialNo": _serialNo
            });
        jsPrintoutLabel.done(function (isPrinted) {
            if (isPrinted == false) {   
                new Alert().Show("danger", "Wydruk etykiety nie powiódł się");
            }
        });
        jsPrintoutLabel.fail(function () {
            new Alert().Show("danger", "Wydruk etykiety nie powiódł się");
        });
    }
    this.PrintTestLabel = function () {
        $.ajax({
            url: "/ONEPROD/MES/PrintTestLabel",
            type: "POST",
            data: '{workplaceId:' + self.Id + '}',
            contentType: 'application/json; charset=utf-8',
            success: function (serialNo) {
                new Alert().Show("success", "Wydrukowano testową etykietę");
            },
            error: function () {
                new Alert().Show("danger", "Wystąpił problem. Wydruk nie powiódł się");
            }
        });
    };
    
    function CalcStartShiftTime(shift_relative) {
        var today = new Date();
        var hour = today.getHours();
        today.setHours(hour + 2);
        hour = today.getHours();
        var shiftEndHour = hour < 8 ? 6 : hour < 16 ? 14 : 22;

        today.setSeconds(0); today.setMinutes(0); today.setMilliseconds(0);
        today.setHours(shiftEndHour - (8 * (shift_relative + 1)));
        return new moment(today);
    }
    function CalcEndShiftTime(shift_relative) {
        var today = new Date();
        var hour = today.getHours();
        today.setHours(hour + 2);
        hour = today.getHours();
        var shiftEndHour = hour < 8 ? 6 : hour < 16 ? 14 : 22;

        today.setSeconds(0); today.setMinutes(0); today.setMilliseconds(0);
        today.setHours(shiftEndHour - (8 * shift_relative));
        return new moment(today);
    }  

    function Actions() {
        $(document).off("click", ".selectTrolleyQty");
        $(document).on("click", ".selectTrolleyQty ", function () {
            self.SelectTrolleyQty(this);
        });

        $(document).off("keypress");
        $(document).on("keypress", function (event) {
            console.log(event.which);
            console.log("keypress (keykode: " + event.which + ")");
            if (event.which == 13 || event.which == 98) {
                $("#btn1Piece").click();
            }
        });

        $(document).off("click", ".confirmWorkorderPartially");
        $(document).on("click", ".confirmWorkorderPartially", function () {
            self.OpenPartialyConfirmationWindow();
        });

        $(document).off("click", "#ConfirmWorkorderFormSubmit");
        $(document).on("click", "#ConfirmWorkorderFormSubmit", function () {
            var _qty = $("#qty").val();
            self.ConfirmWorkorderPartialy(_qty);
        });

        $(document).off("click", ".confirmWorkorderTrolley");
        $(document).on("click", ".confirmWorkorderTrolley", function () {
            self.ConfirmProduction();
        });

        $(document).off("click", "#btnScrap");
        $(document).on("click", "#btnScrap", function () {
            //self.ScrapWorkorderByTrolleyQty(0);
            self.ScrapProduction();
        });

        $(document).off("click", ".btnBufferQty");
        $(document).on("click", ".btnBufferQty", function () {
            let val = parseInt($(this).attr("data-value"));
            let qty = parseInt($(".formWorkplaceBuffer #Qty").val());
            qty = qty > 0 ? qty : 0;
            $(".formWorkplaceBuffer #Qty").val(qty + val);
        });

        $(document).off("click", "#btnPrintTestLabel");
        $(document).on("click", "#btnPrintTestLabel", function () {
            console.log("test print");
            self.PrintTestLabel();
        });
    }
}