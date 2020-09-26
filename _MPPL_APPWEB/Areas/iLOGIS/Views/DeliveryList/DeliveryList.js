var DeliveryList = function (_trainId, loopQty) {
    console.log("DeliveryList extended");
    DeliveryListAbstract.call(this, _trainId, loopQty);

    //this.Refresh = function () {
    //    console.log("DeliveryList.Refresh");
    //};
};

var DeliveryListAbstract = function (_trainId, loopQty) {
    console.log("DeliveryListAbstract");

    var self = this;
    var trainId = _trainId;
    var workorders;
    var selectedLineId = 0;
    var lines = [];
    var prodIndicatorPosition = [0, 0, 0]; //[woIndex, leftParam, qtyOfCurrentWo]
    var limiterPosition = [6, 0, 0]; //[woIndex, leftParam, qtyOfCurrentWo]
    var limiterMoveQty = loopQty;
    var setupLimiter = false;
    var vc = new VersionController(0);
    var selectedItemRow = 0;

    //AJAX DATA
    this.Refresh = function () {
        console.log("DeliveryListAbstract.Refresh");
        $(".selectedRow").removeClass("selectedRow");
        $("#rowTree").removeClass("hidden");
        $("#loading").html(ShowLoadingSnippet());
        $("#rowTwo").addClass("hidden");

        self.ClearData();

        GetWorkorders().then(
            function (result) {
                GetItemsForWorkorders().then(
                    function (result) {
                        RefreshCalculations();
                    },
                    function (error) {
                        console.log("Refresh.GetItemsForWorkorders. " + error);
                    }
                );
            },
            function (error) {
                console.log("Refresh.GetWorkorders. " + error);
                ShowNoDataInfo();
            }
        );
    };
    function RefreshCalculations() {

        lines = Array.from(new Set($('.orderHeader').map(function () { return $(this).attr('data-lineId'); })));

        CalculateWorkorderQtyProducedSinceStart();
        AnalyzeWhereItemIsUsed();
        DrawProdIndicator(); //line
        DrawProdIndicatorsEstimated(); //line
        CalculateCoverage();
        DrawLimiters();

        //self.FilterItemsByDemand(true);
        AddDoubleTap();

        if (lines.length > 1) {
            self.CountSummaryForLines();
        }
        else {
            CountSummary();
            _SetupLimiter();
        }

        filteredDemand = true;
        self.FilterItemsByDemand(true);

        $("#rowTwo").removeClass("hidden");
        $("#rowTree").addClass("hidden");
        $("#loading").html("");

        var itemRow = $("#itemRows").children().eq(selectedItemRow);
        $(itemRow).addClass("selectedRow");
        var itemRowH = $("#itemRowHeaders").children().eq(selectedItemRow);
        $(itemRowH).addClass("selectedRow");
        var itemRowS = $("#itemSummaryRows").children().eq(selectedItemRow);
        $(itemRowS).addClass("selectedRow");
    }
    function ShowNoDataInfo() {

        lines = Array.from(new Set($('.orderHeader').map(function () { return $(this).attr('data-lineId'); })));

        $("#rowTwo").removeClass("hidden");
        $("#rowTree").addClass("hidden");
        $("#loading").html("");

        var itemRow = $("#itemRows").children().eq(selectedItemRow);
        $(itemRow).addClass("selectedRow");
        var itemRowH = $("#itemRowHeaders").children().eq(selectedItemRow);
        $(itemRowH).addClass("selectedRow");
        var itemRowS = $("#itemSummaryRows").children().eq(selectedItemRow);
        $(itemRowS).addClass("selectedRow");
    }

    this.ClearData = function () {
        //clear headers
        $(".orderHeader").find("#prodIndicatorSquare").remove();
        $(".orderHeader").find("#prodIndicator").remove();
        $(".orderHeader").find("#limiter").remove();
        $(".orderHeader").each(function (i) {
            if (i > 0) {
                $(this).html('');
                $(this).detach();
            }
        });
        //clear rows Headers
        $(".itemRowHeader").each(function (i) {
            if (i > 0) {
                $(this).html('');
                $(this).detach();
            }
        });
        $(".prodIndicatorSmall").remove();
        //clear rows Data
        $(".itemRowData").each(function (i) {
            if (i > 0) {
                $(this).html('');
                $(this).detach();
            }
        });
        $(".itemRowSummary").each(function (i) {
            if (i > 0) {
                $(this).html('');
                $(this).detach();
            }
        });

        $(".itemRow").removeClass("sDanger");
        $(".itemRow").removeClass("sSuccess");
        $(".itemRow").removeClass("sWarning");
        $(".row").removeClass("selectedRow");
        $(".itemRowData").find(".barCovered").detach();
        $(".itemRowData").find(".lbBox").detach();
        $(".itemRowData").find(".woItemDataCell").detach();
        $(".itemRowData").attr("data-itemdeliveredqty", 0);
        $(".itemRow").attr("data-itemid", 0);
        $(".itemRow").attr("data-workstationid", 0);
        $(".itemRowHeader").attr("data-itemid", 0);
        $(".itemRowHeader").find(".code").text(0);
        $(".itemRowHeader").find(".name").text(0);
        $(".itemRowHeader").find(".wrkst").text(0);
        $(".itemRowHeader").find(".box").text(0);
    };
    function GetWorkorders() {
        return new Promise((resolve, reject) => {
            var json = new JsonHelper();
            var woJson = json.GetPostData("/iLogis/DeliveryList/GetWorkorders", { trainId });
            woJson.done(function (data) {
                vc.CheckVersion(data.version);
                if (data.woList.length > 0) {
                    workorders = data.woList;
                    resolve();
                }
                else {
                    reject("No workorders found");
                }
            });
        });
    }
    function GetItemsForWorkorders() {
        return new Promise((resolve, reject) => {
            var woIds = workorders.map(a => a.Id);
            var json = new JsonHelper();
            var woJson = json.GetPostData("/iLogis/DeliveryList/GetItemsForWorkorders", { trainId, woIds: woIds });
            woJson.done(function (data) {
                let workorderItemsList = data.WorkorderItems;
                //console.log(itemsForWO);
                for (var w = 0; w < workorders.length; w++)
                {
                    let _workorderItems = workorderItemsList.filter(x => x.WoId == workorders[w].Id);

                    if (_workorderItems.length > 0) {
                        workorders[w].Items = _workorderItems[0].Items;
                        DrawWorkorderHeader(workorders[w], w, _workorderItems[0].DataStatus);
                        PutItemsOfWorkorder(workorders[w]);
                    }
                }
                resolve();
            });
        });
        
    }
    this.Verify = function () {
        var json = new JsonHelper();
        $(".orderHeader").css("color", "#135970");
        $(".orderHeader").each(function (i) {
            var header = $(this);
            let woId = $(this).attr("data-woid");
            var woJson = json.GetPostData("/iLogis/DeliveryList/VerifyDeliveryList", { trainId, woId });
            woJson.done(function (data) {
                $(header).css("color", "");
                self.SetupLimiter();
            });
        });
    }

    //DRAW GRID HEADERS&DATA
    function DrawWorkorderHeader(workorder, i, dataStatus = 0) {

        var itm = $(".orderHeader")[0];
        var cln = itm;

        if (i > 0) {
            cln = itm.cloneNode(true);
        }
        $(cln).attr("data-woId", workorder.Id);
        $(cln).attr("data-qty", workorder.Qty);
        $(cln).attr("data-qtyIn", workorder.QtyIn);
        $(cln).attr("data-qtyOut", workorder.QtyOut);
        $(cln).attr("data-startTime", workorder.StartTimeStr);
        $(cln).attr("data-endTime", workorder.EndTimeStr);
        $(cln).attr("data-processingTime", workorder.ProcessingTime);
        $(cln).attr("data-lastScanDate", new moment(workorder.LastScanDate).format("YYYY-MM-DD HH:mm:ss"));
        $(cln).attr("data-qtyProducedSinceStart", 0);
        $(cln).attr("data-lineId", workorder.LineId);
        $(cln).attr("data-lineName", workorder.LineName);

        $(cln).removeClass("line101");
        $(cln).removeClass("line103");
        $(cln).removeClass("line104");
        $(cln).addClass("line" + workorder.LineName);

        $(cln).find(".pnc1").text(workorder.ItemCode.toString().substr(0, 6));
        $(cln).find(".pnc2").text(workorder.ItemCode.toString().substr(6, 3));
        $(cln).find(".qty").text(workorder.Qty);
        $(cln).find(".time").text(new moment(workorder.StartTime.toString()).format("HH:mm"));

        if (workorder.Notice != null && workorder.Notice.length > 2) {
            $(cln).css("border-top-color", "red");
        }
        else {
            $(cln).css("border-top-color", "transparent");
        }

        switch (dataStatus) {
            case 2: $(cln).css("background-color", "#fffe0057"); break;
            case 1: $(cln).css("background-color", "#0025ff57"); break;
            default: $(cln).css("background-color", "transparent");
        }

        $("#orderHeaders").append(cln);
    }
    function PutItemsOfWorkorder(wo) {
        var woItems = wo.Items;

        //Add not exisiting items to the list
        for (var k = 0; k < woItems.length; k++) {
            if (_ItemRowNotExists(woItems[k].ItemWMSId)) {
                let indx = self._FindSortedPlace(woItems[k]);
                self._DrawItemHeader(woItems[k], indx);
                self._DrawItemDataRow(woItems[k], indx);
                self._DrawItemSummaryRow(woItems[k], indx);
            }
        }

        //Add cells with item-wo qty
        var itemWMSId = 0;
        var itemRowDataList = $(".itemRowData");
        var qty = 0;
        var qtyDelivered = 0;
        var item = {};
        var bomQty = 0;

        for (var m = 0; m < itemRowDataList.length; m++) {
            itemWMSId = $(itemRowDataList[m]).attr("data-itemId");
            item = wo.Items.filter(x => x.ItemWMSId == itemWMSId);
            if (item.length > 0) {
                bomQty = item[0].BomQty;
                qty = wo.Qty * bomQty;
                qtyDelivered = Math.min(item[0].QtyDelivered, wo.Qty * bomQty);
                _DrawWoItemDataCell(itemWMSId, wo.Id, qty, 0, qtyDelivered, bomQty);
            }
            else {
                _DrawWoItemDataCell(itemWMSId, wo.Id, 0, 0, 0, 0);
            }
        }
    }
    this._FindSortedPlace = function (item) {
        let index = 0;
        let wrkstOrder = 0;
        $(".itemRowHeader").each(function (i) {
            wrkstOrder = parseInt(this.attributes["data-wrkstorder"].value);
            if (item.WorkstationOrder > wrkstOrder) {
                index = i;
                //return false;
            }
        });
        return index;
    }
    this._DrawItemHeader = function (deliveryListItemViewModel, index) {
        //let k = $(".itemRowHeader").length;
        let id_1st = parseInt($($(".itemRowHeader")[0]).attr("data-itemid"));
        var itm = $(".itemRowHeader")[0];
        var cln = id_1st > 0 ? itm.cloneNode(true) : itm;
        $(cln).attr("data-itemId", deliveryListItemViewModel.ItemWMSId);
        $(cln).attr("data-workstationId", deliveryListItemViewModel.WorkstationId);
        $(cln).attr("data-wrkstOrder", deliveryListItemViewModel.WorkstationOrder);
        $(cln).attr("data-wrkstProductsFromIn", deliveryListItemViewModel.WorkstationProductsFromIn);
        $(cln).attr("data-wrkstProductsFromOut", deliveryListItemViewModel.WorkstationProductsFromOut);
        $(cln).find(".wrkst").text(deliveryListItemViewModel.Workstation);
        $(cln).find(".code").text(deliveryListItemViewModel.Code);
        $(cln).find(".name").text(deliveryListItemViewModel.Name);
        $(cln).find(".box").text(deliveryListItemViewModel.QtyPerPackage);
        $(cln).find(".itemRow").attr("data-workstationId", deliveryListItemViewModel.WorkstationId);
        $(cln).find(".itemRow").attr("data-itemId", deliveryListItemViewModel.Id);

        $("#itemRowHeaders").insertAt(index, cln);
    };
    this._DrawItemDataRow = function (deliveryListItemViewModel, index) {
        //let k = $(".itemRowHeader").length;
        let id_1st = parseInt($($(".itemRowData")[0]).attr("data-itemid"));
        var itm2 = $("#itemRows .itemRow")[0];
        var cln2 = id_1st > 0 ? itm2.cloneNode(true) : itm2;
        $(cln2).find(".woItemDataCell").each(function () {
            $(this).attr("data-itemId", deliveryListItemViewModel.ItemWMSId);
            $(this).attr("data-qtydelivered", 0);
        });
        $(cln2).find(".qty").each(function () {
            $(this).text(0);
        });
        $(cln2).attr("data-itemId", deliveryListItemViewModel.ItemWMSId);
        $(cln2).attr("data-workstationId", deliveryListItemViewModel.WorkstationId);
        $("#itemRows").insertAt(index, cln2);
    };
    this._DrawItemSummaryRow = function (deliveryListItemViewModel, index) {
        //let k = $(".itemRowHeader").length;
        let id_1st = parseInt($($(".itemRowSummary")[0]).attr("data-itemid"));
        var itm2 = $("#itemSummaryRows .itemRow")[0];
        var cln2 = id_1st > 0 ? itm2.cloneNode(true) : itm2;

        $(cln2).attr("data-itemId", deliveryListItemViewModel.ItemWMSId);
        $(cln2).attr("data-workstationId", deliveryListItemViewModel.WorkstationId);
        $("#itemSummaryRows").insertAt(index, cln2);
    };
    function _ItemRowNotExists(itemWMSId) {
        var itemRow = $(document).find('[data-itemId="' + itemWMSId + '"]');
        return itemRow == null || !(itemRow.length > 0);
    }
    function _DrawWoItemDataCell(itemWMSId, woId, qty, qtyPerPackage, qtyDelivered, BomQty) {
        var itemRow3 = $('.itemRowData[data-itemId="' + itemWMSId + '"]');

        var qtyElement = $("<span>").addClass("qty").addClass("hidden").text(qty);
        //var qtyPerPackageElement = $("<span>").addClass("qtyPerPackage").text(qtyPerPackage);
        var cln3 = $('<div>').attr('class', 'woItemDataCell');

        //$(cln3).attr("data-itemId", 0);
        $(cln3).attr("data-itemId", itemWMSId);
        $(cln3).attr("data-woId", woId);
        $(cln3).attr("data-qtyDelivered", qtyDelivered);
        $(cln3).attr("data-BomQty", BomQty);
        $(cln3).append($(qtyElement));
        //$(cln3).append($(qtyPerPackageElement));
        $(itemRow3).find(".woItemDataCells").append(cln3);
    }

    //PRODINDICATORS - BLUE LINES
    function DrawProdIndicator() {

        var prodIndicator = $("#prodIndicator");

        if (prodIndicator.length > 0) {
            $(prodIndicator).detach();
        }

        var highestDate = new moment("2000-01-01");
        $(".orderHeader").each(function () {
            if (selectedLineId == 0 || parseInt(this.getAttribute("data-lineId")) == selectedLineId) {
                var date = new moment(this.getAttribute("data-lastScanDate"));
                if (date > highestDate) {
                    highestDate = date;
                    //console.log(highestDate);
                }
            }
        }).each(function (index) {
            if (selectedLineId == 0 || parseInt(this.getAttribute("data-lineId")) == selectedLineId) {
                var thisDate = new moment(this.getAttribute("data-lastScanDate"));
                var qty = parseInt($(this).attr("data-qty"));
                var qtyIn = parseInt($(this).attr("data-qtyIn"));

                if (highestDate._i == thisDate._i) {
                    //var left = parseInt(50 * qtyIn / qty);
                    //$(this).append($("<div id='prodIndicator' style='left: " + left + "px'>"));
                    $(this).attr("data-current", "1");
                }

                if (qtyIn >= qty) {
                    //console.log(qtyIn + " / " + qty);
                    //let otherLinesWos = SkipWOFromAnoderLine(index);
                    prodIndicatorPosition[0] = index; //+ 1;
                    prodIndicatorPosition[1] = 0;
                    prodIndicatorPosition[2] = 0;
                    $(this).append($("<div id='prodIndicatorSquare' style='width: 50px'>"));
                }
                else if (qtyIn > 0 && qtyIn < qty) {
                    //let otherLinesWos = SkipWOFromAnoderLine(index);
                    var width = parseInt(50 * qtyIn / qty);
                    prodIndicatorPosition[0] = index; // + otherLinesWos - 1;
                    prodIndicatorPosition[1] = width;
                    prodIndicatorPosition[2] = qtyIn;
                    $(this).append($("<div id='prodIndicatorSquare' style='width: " + width + "px'>"));
                }
            }
        });


    }
    function SkipWOFromAnoderLine(index) {

        let otherLinesWos = 1;

        if (selectedLineId != 0) {
            let i2 = index + 1;
            let len1 = $(".orderHeader").length;
            let slid = $($(".orderHeader")[i2]).attr("data-lineId");
            while (slid != selectedLineId && i2 < len1) {
                otherLinesWos++;
                slid = $($(".orderHeader")[i2]).attr("data-lineId");
                i2++;
            }
            otherLinesWos--;
        }
        return otherLinesWos;
    }
    function DrawProdIndicatorsEstimated() {
        $(".itemRowHeader").each(function (i) {
            _DrowProdIndicatorEstimated(i);
        });
    }
    function _DrowProdIndicatorEstimated(itemRowIndex) {
        let estimatedProducedQty = 0;
        let qtyProducedSinceStart = 0;
        let notOutPrevOrders = 0;
        let woQtyPlanned = 0;
        let woQtyOut = 0;
        let woQtyIn = 0;
        let lineId = 0;
        let lineCapacity = 200;

        let itemRowHeader = $(".itemRowHeader").eq(itemRowIndex);
        let itemRow = $(".itemRowData").eq(itemRowIndex);
        let productsFromIn = parseInt($(itemRowHeader[0]).attr("data-wrkstproductsfromin"));
        let productsFromOut = parseInt($(itemRowHeader[0]).attr("data-wrkstproductsfromout"));
        let workorderHeaders = $(".orderHeader");
        let totalInQty = parseInt($(workorderHeaders[0]).attr("data-qtyProducedSinceStart"));
        let totalOutQty = parseInt($(workorderHeaders[0]).attr("data-qtyOutSinceStart"));

        $(itemRow[0]).find(".woItemDataCell").each(function (i) {
            qtyProducedSinceStart = parseInt($(workorderHeaders[i]).attr("data-qtyProducedSinceStart"));
            notOutPrevOrders = i > 0 ? parseInt($(workorderHeaders[i - 1]).attr("data-qtyNotOutSinceStart")) : 0;
            lineId = parseInt($(workorderHeaders[i]).attr("data-lineId"));

            if (qtyProducedSinceStart > 0 && (selectedLineId == 0 || lineId == selectedLineId)) {
                woQtyPlanned = parseInt($(workorderHeaders[i]).attr("data-qty"));
                woQtyOut = parseInt($(workorderHeaders[i]).attr("data-qtyOut"));
                woQtyIn = parseInt($(workorderHeaders[i]).attr("data-qtyIn"));

                let qtyEstimationFromIn = Math.max(qtyProducedSinceStart - productsFromIn, 0);
                let qtyEstimationFromOut = woQtyOut > 0 ?
                    woQtyOut + productsFromOut :
                    notOutPrevOrders * -1 + productsFromOut;

                qtyEstimationFromIn = Math.min(qtyEstimationFromIn, woQtyIn);
                qtyEstimationFromOut = Math.min(qtyEstimationFromOut, woQtyIn);

                let inImpact = 1 - productsFromIn / lineCapacity;
                let outImpact = 1 - productsFromOut / lineCapacity;

                if (selectedLineId >= 0) {
                    let weightedAverage = qtyEstimationFromIn * inImpact + qtyEstimationFromOut * outImpact;
                    estimatedProducedQty = parseInt(Math.min(weightedAverage, woQtyIn));
                    //estimatedProducedQty = Math.max(
                    //    Math.min(qtyEstimationFromIn, woQtyIn),
                    //    Math.min(qtyEstimationFromOut, woQtyIn)
                    //);
                }
                else {
                    estimatedProducedQty = Math.max(
                        Math.max(qtyEstimationFromIn, woQtyIn),
                        Math.min(qtyEstimationFromOut, woQtyIn)
                    );
                }

                $(this).attr("data-qtyUsed", estimatedProducedQty);
                if (estimatedProducedQty > 0) {
                    let left = parseInt(49 * estimatedProducedQty / woQtyPlanned) - 2;
                    $(itemRow[0]).find(".prodIndicatorSmall").remove();
                    $(this).append($("<div class='prodIndicatorSmall' style='left: " + left + "px' data-estimatedProducedQty='" + estimatedProducedQty + "'>"));
                }
            }
        });
    }

    //LIMITERS - ORANGE LINES
    this.SetupLimiter = function () {
        setupLimiter = true;
        self.Refresh();
    };
    function _SetupLimiter() {
        if (setupLimiter == true) {
            _SetupLimiterPosition();
            self.MoveLimiter(1);
            setupLimiter = false;
        }
    }
    function _SetupLimiterPosition() {
        //[woIndex, leftParam, qtyOfCurrentWo]
        limiterPosition[0] = prodIndicatorPosition[0];
        limiterPosition[1] = prodIndicatorPosition[1];
        limiterPosition[2] = 10 * (((prodIndicatorPosition[2] % 10) > 0 ? 1 : 0) + parseInt(prodIndicatorPosition[2] / 10));
    }
    this.MoveLimiter = function (d) {
        _MoveLimiter(d);
        _DrawLimitersRows();
        CountSummary();
        self.FilterItemsByDemand(true);
    };
    function _MoveLimiter(d) {
        let woElement = null;
        let woQty = 0;
        let qtyToMove = limiterMoveQty;
        let lineId = 0;
        let headerLength = $(".orderHeader").length;

        if (d > 0) {
            while (qtyToMove > 0 && limiterPosition[0] < headerLength) {
                woElement = $(".orderHeader")[limiterPosition[0]];
                lineId = $(woElement).attr("data-lineId");

                if (selectedLineId == 0 || lineId == selectedLineId) {
                    woQty = parseInt($(woElement).attr("data-qty"));
                    qtyToEnd = Math.max(0, woQty - limiterPosition[2]);

                    if (0 < qtyToEnd && qtyToEnd > qtyToMove) {
                        limiterPosition[2] += qtyToMove;
                        qtyToMove -= qtyToEnd;
                    }
                    else {
                        qtyToMove -= qtyToEnd;
                        limiterPosition[0]++;
                        limiterPosition[2] = 0;
                    }
                }
                else {
                    limiterPosition[0]++;
                }
                _DrawLimiterHeader();
            }
            limiterPosition[1] = Math.min(49, parseInt(49 * (limiterPosition[2]) / woQty));
        }
        else {
            while (qtyToMove > 0) {
                woElement = $(".orderHeader")[limiterPosition[0]];
                woQty = parseInt($(woElement).attr("data-qty"));
                qtyToEnd = Math.max(0, limiterPosition[2]);
                if (0 < qtyToEnd && qtyToEnd > qtyToMove) {
                    limiterPosition[2] -= qtyToMove;
                    qtyToMove -= qtyToEnd;
                }
                else {
                    qtyToMove -= qtyToEnd;
                    limiterPosition[0]--;
                    limiterPosition[2] = parseInt($($(".orderHeader")[limiterPosition[0]]).attr("data-qty"));
                }
            }
            limiterPosition[1] = Math.min(49, parseInt(49 * (limiterPosition[2]) / woQty));
        }
        //DrawLimiters();
        _DrawLimiterHeader();
    }

    function DrawLimiters() {
        console.log("Draw Limiters");
        _DrawLimiterHeader();
        _DrawLimitersRows();
    }
    function _DrawLimiterHeader() {
        var limiter = $("#limiter");
        if (limiter.length > 0) {
            $(limiter).detach();
        }
        woElement = $(".orderHeader")[limiterPosition[0]];
        $(woElement).append($("<div id='limiter' style='left:" + limiterPosition[1] + "px'>"));
    }
    function _DrawLimitersRows() {
        $(".itemRowHeader").each(function (i) {
            _DrawLimiterRowEstimated(i);
        });
    }
    function _DrawLimiterRowEstimated(itemRowIndex) {
        //console.log("DrawLimiterEstimated");

        var estimatedProducedQty = 0;
        var qtyProducedSinceStart = 0;
        var woQtyPlanned = 0;
        var qtyRequestedToDeliver = CalculateLimiterQtyToDeliver();

        var itemRowHeader = $(".itemRowHeader").eq(itemRowIndex);
        var itemRow = $(".itemRowData").eq(itemRowIndex);
        var workorderHeaders = $(".orderHeader");
        var i2 = 0;

        $(itemRow[0]).find(".woItemDataCell").each(function (i) {
            //setup initial on the same cell where prodIndicator is
            var prodIndicatorSmall = $($(this).find(".prodIndicatorSmall")[0]);

            if (prodIndicatorSmall != null && prodIndicatorSmall.length > 0) {
                var left = parseInt($(prodIndicatorSmall).css('left')) + 4;
                $(itemRow[0]).find(".limiterSmall").remove();
                $(this).append($("<div class='limiterSmall' data-qtyDemanded='" + qtyRequestedToDeliver + "' style='left: " + left + "px'>"));
                i2 = i;
            }
        });
        $(itemRow[0]).find(".woItemDataCell").each(function (i) {
            //move limiter to another cell
            let lineId = $($(".orderHeader")[i]).attr("data-lineId");

            if ((i >= i2 && qtyRequestedToDeliver > 0) && (selectedLineId == 0 || lineId == selectedLineId)) {
                estimatedProducedQty = parseInt($($(this).find(".prodIndicatorSmall")[0]).attr("data-estimatedProducedQty"));
                estimatedProducedQty = estimatedProducedQty > 0 ? estimatedProducedQty : 0;
                woQtyPlanned = parseInt($(workorderHeaders[i]).attr("data-qty"));
                gtyToEndOfOrder = woQtyPlanned - estimatedProducedQty;

                var left = Math.min(49, parseInt(49 * (estimatedProducedQty + qtyRequestedToDeliver) / woQtyPlanned));

                $(itemRow[0]).find(".limiterSmall").remove();
                $(this).append($("<div class='limiterSmall' data-qtyDemanded='" + (estimatedProducedQty + qtyRequestedToDeliver) + "' style='left: " + left + "px'>"));
                qtyRequestedToDeliver -= gtyToEndOfOrder;
            }
        });
    }
    function CalculateLimiterQtyToDeliver() {
        var limiterIndex = $("#limiter").closest(".orderHeader").index();
        var cells = $(".orderHeader");
        var woQtyPlannedSum = 0;
        var woQtyInSum = 0;
        for (var j = 0; j < cells.length && j <= limiterIndex; j++) {
            if (selectedLineId == 0 || $(cells[j]).attr("data-lineId") == selectedLineId) {
                let cellQty = parseInt($(cells[j]).attr("data-qty"));
                let limiterQty = cellQty > 0 ? limiterPosition[2] : 0;
                woQtyPlannedSum += j == limiterIndex ? limiterQty : cellQty;
                woQtyInSum += parseInt($(cells[j]).attr("data-qtyin"));
            }
        }

        return woQtyPlannedSum - woQtyInSum;
    }

    //POST PROCESSING
    function CalculateWorkorderQtyProducedSinceStart() {

        let orderHeaders = $(".orderHeader");
        let currentOrderQty = 0;
        let qtyIn = 0;
        let qtyOut = 0;
        let qtyProducedSinceStart = 0;
        let qtyNotOutSinceStart = 0;
        //let lineId = 0;

        for (let i = 0; i < orderHeaders.length; i++) {
            currentOrderQty = parseInt(orderHeaders[i].attributes["data-qty"].value);
            //lineId = parseInt(orderHeaders[i].attributes["data-lineId"].value);

            //if (selectedLineId == 0 || selectedLineId == lineId) {
            qtyIn = parseInt(orderHeaders[i].attributes["data-qtyIn"].value);
            qtyOut = parseInt(orderHeaders[i].attributes["data-qtyOut"].value);
            qtyProducedSinceStart = qtyIn;
            qtyNotOutSinceStart += qtyIn - qtyOut;

            if (qtyIn >= currentOrderQty || qtyOut >= currentOrderQty) {
                for (let j = i + 1; j < orderHeaders.length; j++) {
                    currentOrderQty = parseInt(orderHeaders[j].attributes["data-qty"].value);
                    qtyIn = parseInt(orderHeaders[j].attributes["data-qtyIn"].value);
                    qtyOut = parseInt(orderHeaders[j].attributes["data-qtyOut"].value);
                    qtyProducedSinceStart += qtyIn;

                    if (qtyIn < currentOrderQty) { break; }
                }
            }
            else if (qtyIn <= 0) {
                break;
            }
            $(orderHeaders[i]).attr("data-qtyProducedSinceStart", qtyProducedSinceStart);
            $(orderHeaders[i]).attr("data-qtyNotOutSinceStart", qtyNotOutSinceStart);
            //}
        }
    }

    var cellsQty = [];
    function AnalyzeWhereItemIsUsed() {

        var table = $("#table");
        var rows1 = $(".itemRowData");

        for (var i = 0; i < rows1.length; i++) {
            var qty = 0;
            var empty = 0;
            var itemWMSId = $(rows1[i]).attr("data-itemid");
            var cells = $(rows1[i]).find(".woItemDataCell");

            for (var j = 0; j < cells.length; j++) {
                var cell = $(cells[j]);
                qty = parseFloat($(cell).find(".qty").text());

                if (qty > 0) {
                    cellsQty.push(j);
                    $(cells[j]).addClass("itemActive");
                }
                else {
                    $(cells[j]).addClass("itemInactive");
                    _MergeAndSumCells(i, itemWMSId);
                    empty++;
                }
            }
            _MergeAndSumCells(i, itemWMSId);
        }

    }
    function _MergeAndSumCells(row, itemWMSId) {

        var sum = 0;
        var value = 0;
        //var opak = parseInt($(table.rows[row].cells[0]).text());
        var opak = parseInt($('.itemRowHeader[data-itemid="' + itemWMSId + '"]').find(".box").text());
        row = $('.itemRowData[data-itemid="' + itemWMSId + '"]');
        var cells = $(row).find(".woItemDataCell");
        //console.log("opak qty: " + opak);
        var lbBoxText = "";
        var width = 50;

        if (cellsQty.length > 0) {
            //console.log(cellsQty);
            for (c = 0; c < cellsQty.length; c++) {
                //value = parseFloat($(table.rows[row].cells[cellsQty[c]]).text());
                value = parseFloat($(cells[cellsQty[c]]).text());
                sum += value > 0 ? value : 0;
            }
            //console.log("Suma:" + sum);
            if (cellsQty.length > 1) {
                lbBoxText = parseInt(sum / opak) + " [+" + sum % opak + "szt]";
                width = width * cellsQty.length;
                //console.log("Opakowań:" + lbBoxText);
            }
            else {
                lbBoxText = parseInt(sum / opak) + "[" + sum % opak + "]";
                //console.log("Opakowań:" + lbBoxText);
            }
            //$(table.rows[row].cells[cellsQty[0]]).prepend('<div class="lbBox" style="width: ' + (width - 1) + 'px">' + lbBoxText + '</div>');
            $(cells[cellsQty[0]]).prepend('<div class="lbBox" style="width: ' + (width - 1) + 'px">' + lbBoxText + '</div>');
            cellsQty = [];
        }
    }

    //-------------COVERAGE----------------
    function CalculateCoverage() {
        totalRows = $("#itemRows .itemRow").length - 1;
        processedRows = 0;

        $("#itemRows .itemRow").each(function () {
            var itemWMSId = $(this).attr("data-itemid");
            var wrkstId = $(this).attr("data-workstationid");
            var totalQtyDelivered = 0;
            $(this).find(".woItemDataCell").each(function () {
                var qt = parseInt($(this).attr("data-qtyDelivered"));
                totalQtyDelivered += qt > 0 ? qt : 0;
            });
            self._CalculateItemCoverage(itemWMSId, wrkstId, totalQtyDelivered);
            processedRows++;

            //warunek odpala DrawLimiter1 na końcu pętli
            //if (processedRows >= totalRows) {
            //}
        });
    }
    function CalculateCoverageJson() {
        totalRows = $("#itemRows .itemRow").length - 1;
        processedRows = 0;

        $("#itemRows .itemRow").each(function () {
            let itemWMSId = $(this).attr("data-itemid");
            let wrkstId = $(this).attr("data-workstationid");
            let jsQ = new JsonHelper().GetPostData("/iLOGIS/DeliveryList/GetItemDetails", { "trainId": trainId, "itemWMSId": itemWMSId, "workstationId": wrkstId });
            jsQ.done(function (data) {
                self._CalculateItemCoverage(data.ItemWMSId, data.WorkstationId, data.TotalQtyDelivered);
                processedRows++;

                //warunek odpala DrawLimiter_ na końcu pętli
                if (processedRows >= totalRows) {
                    DrawLimiters();
                    CountSummary();
                    self.FilterItemsByDemand();
                    $("#rowTwo").removeClass("hidden");
                    $("#rowTree").addClass("hidden");
                    $("#loading").html("");
                }
            });
        });
    }
    this._CalculateItemCoverage = function (itemWMSId, workstationId, totalQtyDelivered) {
        var availableQty = totalQtyDelivered;
        var $itemRow = $(".itemRow[data-itemId='" + itemWMSId + "'][data-workstationId='" + workstationId + "']");
        $itemRow.attr("data-itemDeliveredQty", totalQtyDelivered);

        $itemRow.find(".woItemDataCell").each(function (i) {
            $(this).find(".barCovered").remove();
            var itemWoQty = $($(this).find(".qty")[0]).text();

            if (itemWoQty < availableQty || itemWoQty == 0) {
                if (availableQty >= 0) {
                    $(this).prepend('<div class="barCovered" style="width: ' + 49 + 'px"></div>');
                }
            }
            else {
                if (availableQty > 0) {
                    $(this).prepend('<div class="barCovered" style="width: ' + (49 * availableQty / itemWoQty).toFixed(2) + 'px"></div>');
                }
            }
            availableQty -= itemWoQty;
        });
    };

    //------------ROW SUMMARY---------------
    var rowSummaryData = { itemRow: null, rowIndex: 0, qtyToDeliver: 0, minutesToEndOfStock: 0, remainingStock: 0, itemDeliveredQty: 0, itemDemandSum: 0, itemUsedQty: 0 };

    this.CountSummaryForLines = function () {
        console.log("DeliveryList.CountSummaryForLines");
        var table = $("#table");
        var rows1 = $(".itemRowData");
        var limiterIndex = 0; //$("#limiter").closest(".orderHeader").index();
        var currentProdTime = null;

        for (var i = 0; i < rows1.length; i++) {

            rowSummaryDataTotal = { itemRow: null, qtyToDeliver: 0, minutesToEndOfStock: 0, remainingStock: 0, itemDeliveredQty: 0, itemDemandSum: 0, itemUsedQty: 0 };

            for (var l = 0; l < lines.length; l++) {
                selectedLineId = lines[l];
                $(".orderHeader").find("#prodIndicatorSquare").remove();
                $(".orderHeader").find("#prodIndicator").remove();
                DrawProdIndicator();
                setupLimiter = true;
                //_SetupLimiter();
                _SetupLimiterPosition();
                _MoveLimiter(1);
                _DrowProdIndicatorEstimated(i);
                _DrawLimiterRowEstimated(i);
                currentProdTime = _CheckCurrentProdTime(rows1[i]);
                limiterIndex = $($(rows1[i]).find(".limiterSmall")[0]).closest(".woItemDataCell").index();

                rowSummaryData = _countSummaryOfRow(rows1[i], i, limiterIndex, currentProdTime);

                rowSummaryDataTotal.itemRow = rowSummaryData.itemRow;
                rowSummaryDataTotal.rowIndex = rowSummaryData.rowIndex;
                rowSummaryDataTotal.itemDeliveredQty = rowSummaryData.itemDeliveredQty;
                rowSummaryDataTotal.itemDemandSum += rowSummaryData.itemDemandSum;
                rowSummaryDataTotal.itemUsedQty += rowSummaryData.itemUsedQty;
                rowSummaryDataTotal.qtyToDeliver = rowSummaryDataTotal.itemDemandSum - rowSummaryDataTotal.itemDeliveredQty;
                rowSummaryDataTotal.minutesToEndOfStock = Math.min(rowSummaryDataTotal.minutesToEndOfStock, rowSummaryData.minutesToEndOfStock);
                rowSummaryDataTotal.remainingStock = rowSummaryDataTotal.itemDeliveredQty - rowSummaryDataTotal.itemUsedQty;
            }
            rowSummaryDataTotal.qtyToDeliver = Math.max(0, rowSummaryDataTotal.qtyToDeliver);
            self._putSummaryOfRow(rowSummaryDataTotal);
        }

        $(".orderHeader").find("#prodIndicatorSquare").remove();
        $(".orderHeader").find("#prodIndicator").remove();
        selectedLineId = 0;
        DrawProdIndicator();
        $(".prodIndicatorSmall").remove();
        $(".limiterSmall").remove();
        $(".orderHeader").find("#limiter").remove();

    };
    function CountSummary() {
        console.log("DeliveryList.CountSummary");
        var table = $("#table");
        var rows1 = $(".itemRowData");
        var limiterIndex = 0; //$("#limiter").closest(".orderHeader").index();
        var currentProdTime = null;

        for (var i = 0; i < rows1.length; i++) {
            currentProdTime = _CheckCurrentProdTime(rows1[i]);
            limiterIndex = $($(rows1[i]).find(".limiterSmall")[0]).closest(".woItemDataCell").index();
            rowSummaryData = _countSummaryOfRow(rows1[i], i, limiterIndex, currentProdTime);
            self._putSummaryOfRow(rowSummaryData);
        }
    }
    function _countSummaryOfRow(itemRow, rowIndex, limiterIndex, currentProdTime) {

        var itemDatacells = $(itemRow).find(".woItemDataCell");
        var itemDeliveredQty = $(itemRow).attr("data-itemDeliveredQty");                //total
        var itemDemandSum = _CalculateDemandOfRowCells(itemDatacells, limiterIndex);    //for line
        var itemUsedQty = _CalculateItemUsedQtyOfRowCells(itemDatacells, limiterIndex); //for line
        var qtyToDeliver = Math.max(0, itemDemandSum - itemDeliveredQty);
        var coveragedTime = _CheckCoverageTime(itemRow);
        var minutesToEndOfStock = parseInt((coveragedTime - currentProdTime) / 60000);
        var remainingStock = itemDeliveredQty - itemUsedQty;

        //console.log(selectedLineId + " qty to deliver: " + qtyToDeliver);

        rowSummaryData = { itemRow, rowIndex, qtyToDeliver, minutesToEndOfStock, remainingStock, itemDeliveredQty, itemDemandSum, itemUsedQty };
        return rowSummaryData;

        //_putSummaryOfRow(itemRow, qtyToDeliver, minutesToEndOfStock, remainingStock, itemUsedQty);
    }
    //funkcja publiczna na potrzeby dziedziczenia
    this._putSummaryOfRow = function (rsd) { //itemRow, qtyToDeliver, minutesToEndOfStock, remainingStock, itemUsedQty) {

        var summaryRow = $($(".itemRowSummary")[rsd.rowIndex]);
        var el = $($(summaryRow).find(".summary")[0]);


        var itemWMSId = $(summaryRow).attr("data-itemid");
        var qtyPerPackage = parseInt($('.itemRowHeader[data-itemid="' + itemWMSId + '"]').find(".box").text());

        $(el).find(".summary1").text(rsd.qtyToDeliver);
        $(el).find(".summary2").text(_summaryTimeFormatter(rsd.minutesToEndOfStock));
        $(el).find(".summary3A").text(parseInt(rsd.qtyToDeliver / qtyPerPackage));
        $(el).find(".summary3B").text("[+" + rsd.qtyToDeliver % qtyPerPackage + "szt]");
        $(el).attr("data-remainingStock", rsd.remainingStock);
        $(el).attr("data-itemUsedQty", rsd.itemUsedQty);
        $(rsd.itemRow).removeClass("sWarning");
        $(summaryRow).removeClass("sWarning");
        $(rsd.itemRow).removeClass("sDanger");
        $(summaryRow).removeClass("sDanger");
        $(rsd.itemRow).removeClass("sSuccess");
        $(summaryRow).removeClass("sSuccess");
        if (rsd.qtyToDeliver == 0) {
            $(rsd.itemRow).addClass("sSuccess");
            $(summaryRow).addClass("sSuccess");
        }
        else if (rsd.minutesToEndOfStock < 20) {
            $(rsd.itemRow).addClass("sDanger");
            $(summaryRow).addClass("sDanger");
        }
        else if (rsd.minutesToEndOfStock < 60) {
            $(rsd.itemRow).addClass("sWarning");
            $(summaryRow).addClass("sWarning");
        }
    }
    function _summaryTimeFormatter(minutes) {
        let value = Math.abs(parseInt(minutes));
        let tu = " min";
        let sign = minutes >= 0 ? "+" : "-";

        if (value > 60) {
            tu = " h";
            value = value / 60;
        }
        if (value > 1440) {
            tu = " d";
            value = value / 1440;
        }
        return sign + value.toFixed(0).toString() + tu;
    }

    function _CalculateDemandOfRowCells(cells, limiterIndex) {
        var itemDemandSum = 0;
        var cellQty = 0;
        for (var j = 0; j < cells.length && j <= limiterIndex; j++) {
            let lineId = $($(".orderHeader")[j]).attr("data-lineId");
            if (selectedLineId == 0 || lineId == selectedLineId) {
                cellQty = parseInt($(cells[j]).find(".qty").text());

                if (j == limiterIndex && cellQty > 0) {
                    cellQty = parseInt($($(cells[j]).find(".limiterSmall")[0]).attr("data-qtydemanded"));
                }
                itemDemandSum += cellQty;
            }
        }
        return itemDemandSum;
    }
    function _CalculateItemUsedQtyOfRowCells(cells, limiterIndex) {
        var itemUsedQty = 0;
        var qtyUsed = 0;
        for (var j = 0; j < cells.length && j <= limiterIndex; j++) {
            let lineId = $($(".orderHeader")[j]).attr("data-lineId");
            if (selectedLineId == 0 || lineId == selectedLineId) {
                var itemQty = parseInt($(cells[j]).find(".qty").text());
                if (itemQty > 0) {
                    qtyUsed = parseInt($(cells[j]).attr("data-qtyused")) * parseFloat($(cells[j]).attr("data-BomQty"));
                    itemUsedQty += qtyUsed > 0 ? qtyUsed : 0;
                }
            }
        }
        return itemUsedQty;
    }
    function _CheckCurrentProdTime(itemRow) {
        //var st = new moment($("#prodIndicator").closest(".orderHeader").attr("data-startTime"));
        ////var et = new moment($("#prodIndicator").closest(".orderHeader").attr("data-endTime"));
        //var pt = parseInt($("#prodIndicator").closest(".orderHeader").attr("data-processingTime"));
        //var qty = $("#prodIndicator").closest(".orderHeader").attr("data-qty");
        //var qtyin = $("#prodIndicator").closest(".orderHeader").attr("data-qtyin");

        //prodIndicatorSmall
        let prodIndicator = $($(itemRow).find(".prodIndicatorSmall")[0]);
        let prodIndicatorCellIndex = $(prodIndicator).closest(".woItemDataCell").index();
        let headerCell = $($(".orderHeader")[prodIndicatorCellIndex]);

        let st = new moment($(headerCell).attr("data-startTime"));
        let pt = parseInt($(headerCell).attr("data-processingTime"));
        let qty = parseInt($(headerCell).attr("data-qty"));
        let qtyin = parseInt($(prodIndicator).attr("data-estimatedproducedqty"));

        let currentProdTime = moment(st).add(pt * qtyin / qty, 'seconds');
        return currentProdTime;
    }
    function _CheckCoverageTime(itemRow) {
        //barCovered
        lastBarCovered = $(itemRow).find(".barCovered");
        if (lastBarCovered.length > 0) {
            var percent = $(lastBarCovered[lastBarCovered.length - 1]).width() / 49;
            var st = new moment($($(".orderHeader")[lastBarCovered.length - 1]).attr("data-startTime"));
            var pt = $($(".orderHeader")[lastBarCovered.length - 1]).attr("data-processingtime");
            var coverageTime = moment(st).add(pt * percent, 'seconds');
            return coverageTime;
        }

        return null;
    }

    //Workorder POPUP and Filters
    var wnd = {};
    function OpenWindow() {
        self.CloseWorkorderPopup();
        wnd = new PopupWindow(850, 200, 140, 280);
    }
    this.ShowWorkorderPopup = function (woId) {
        var wo = workorders.filter(x => x.Id == woId)[0];

        if (wo != null) {
            OpenWindow();
            wnd.Init("workorder", "Szczegóły Zlecenia");
            wnd.Show(
                $("<div>")
                    .append(_WorkorderDetailRow("ID", wo.Id, "", "colorGray"))
                    .append(_WorkorderDetailRow("NR ZAM:", wo.Number, "", "letterSpacing-2"))
                    .append(_WorkorderDetailRow("PNC:", wo.ItemCode, "", "textBold letterSpacing-3"))
                    .append(_WorkorderDetailRow("MODEL:", wo.ItemName, "", "colorGray"))
                    .append(_WorkorderDetailRow("DATA:", moment(wo.StartTime).format("YYYY-MM-DD HH:mm")))
                    .append(_WorkorderDetailRow("ILOŚĆ:", wo.Qty + " / " + wo.QtyTotal))
                    .append(_WorkorderDetailRow("NOTKA:", wo.Notice, "", "colorYellow"))
                    //.append(_WorkorderDetailRow(":", wo.))
                    //.append($("<div>").text("ID: " + wo.Id))
                    //.append($("<div>").text("NR ZAM: " + wo.Number))
                    //.append($("<div>").text("PNC: " + wo.ItemCode))
                    //.append($("<div>").text("MODEL: " + wo.ItemName))
                    //.append($("<div>").text("DATA: " + moment(wo.StartTime).format("YYYY-MM-DD HH:mm") ))
                    //.append($("<div>").text("ILOŚĆ: " + wo.Qty))
                    //.append($("<div>").text("NOTKA: " + wo.Notice))
            );
        }
    };
    this.CloseWorkorderPopup = function () {
        if (wnd != null && wnd instanceof PopupWindow) {
            try {
                wnd.Close();
            }
            catch (ex) {
                console.log(ex);
            }
        }
    };
    function _WorkorderDetailRow(headerText, valueText, headerClass="", ValueClass="") {
        return $("<div class='row'>")
            .append($("<div class='col-3 WoDetailsHeaderText " + headerClass +"'>").text(headerText))
            .append($("<div class='col-9 WoDetailsValueText " + ValueClass +"'>").text(valueText));
    }

    var filteredOrderId = 0;
    this.FilterItemsByOrder = function (woId) {

        self.UnfilterItemsByOrder();
        self.UnfilterItemsByDemand();

        var cells = $('.woItemDataCell[data-woid="' + woId + '"]');
        for (var r = 1; r < cells.length; r++) {
            if (parseInt($(cells[r]).find(".qty").text()) <= 0) {
                var itemWMSId = $(cells[r]).parents(".itemRowData").attr("data-itemid");
                //console.log("hide: " + itemId);
                //$('.itemRowHeader[data-itemid="' + itemWMSId + '"]').addClass("hidden");
                //$('.itemRowData[data-itemid="' + itemWMSId + '"]').addClass("hidden");
                $('.itemRow[data-itemid="' + itemWMSId + '"]').addClass("hidden");
            }
        }
        $('[data-woId="' + woId + '"]').addClass("selected");

        self.SetFilteredOrderId(woId);
        self.ShowWorkorderPopup(woId);

    };
    this.UnfilterItemsByOrder = function () {
        $('.itemRow').removeClass("hidden");
        $('div').removeClass("selected");
        $('[data-woId="' + filteredOrderId + '"]').removeClass("selected");
        self.SetFilteredOrderId(0);
        self.CloseWorkorderPopup();
    };

    var filteredDemand = false;
    this.FilterItemsByDemand = function (force = false) {

        if (force == true && filteredDemand == false) {
            return false;
        }
        else if (force == true && filteredDemand == true) {
            filteredDemand = false;
        }

        if (filteredDemand == false)
        {
            self.UnfilterItemsByOrder();
            self.UnfilterItemsByDemand();

            var cells = $('.itemRowSummary .summary .summary1');
            for (var r = 0; r < cells.length; r++) {
                if (parseInt($(cells[r]).text()) <= 0) {
                    var itemWMSId = $(cells[r]).parents(".itemRowSummary").attr("data-itemid");
                    $('.itemRow[data-itemid="' + itemWMSId + '"]').addClass("hidden");
                }
            }
            $('div').removeClass("selected");
            $('.summaryHeader').addClass("selected");
            filteredDemand = true;
        }
        else {
            self.UnfilterItemsByDemand();
        }
    };
    this.UnfilterItemsByDemand = function () {
        $('.itemRow').removeClass("hidden");
        $('.summaryHeader').removeClass("selected");
        filteredDemand = false;
    };

    
    //-------------------------------GETTERS&SETTERS---------------------------
    this.SetFilteredOrderId = function (v) { filteredOrderId = v; };
    this.GetFilteredOrderId = function () { return filteredOrderId; };
    this.SetSelectedItemRow = function (v) { selectedItemRow = v; };

    var itemManageDL = new ItemManageDL(self, _trainId, loopQty);
    this.ItemManage = function () { return itemManageDL; };
};