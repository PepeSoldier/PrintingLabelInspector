var DeliveryListMultiLines = function (_trainId, loopQty) {
    console.log("DeliveryListMultiLines extended");
    DeliveryListAbstract.call(this, _trainId, loopQty);

    var lines = [35, 37, 38];
    var self = this;

    this.Refresh = function () {
        console.log("DeliveryListMultiLines.Refresh");
        $(".selectedRow").removeClass("selectedRow");
        $("#rowTree").removeClass("hidden");
        $("#loading").html(ShowLoadingSnippet());
        $("#rowTwo").addClass("hidden");
        this.ClearData();
        //ClearData();
        //GetWorkorders(RefreshCalculations);
        GetItemsToBeLoaded();
    };
    function RefreshCalculations() {
        //UNUSED
        console.log("DeliveryListMultiLines.RefreshCalculations. Function not used ");
    }
    function GetWorkorders(callback) {
       //UNUSED
        console.log("DeliveryListMultiLines.GetWorkorders. Function not used ");
    }
    //new function
    function GetItemsToBeLoaded() {

        var json = new JsonHelper();
        var woJson = json.GetPostData("/iLogis/DeliveryList/CalculateDemandMultiLines", { trainId });
        woJson.done(function (data) {

            var deliveryListItemViewModel = data.list;
            var now = new moment();

            console.log(new moment(data.loopDateTo).format('YYYY-MM-DD HH:mm'));
            $("#loopDates").text("");
            $("#loopDates").text(new moment(data.loopDateFrom).format('YYYY-MM-DD HH:mm') + " - " + new moment(data.loopDateTo).format('HH:mm'));

            for (var k = 0; k < deliveryListItemViewModel.length; k++) {
                let indx = self._FindSortedPlace(deliveryListItemViewModel[k]);
                self._DrawItemHeader(deliveryListItemViewModel[k], indx);
                self._DrawItemDataRow(deliveryListItemViewModel[k], indx);
                self._DrawItemSummaryRow(deliveryListItemViewModel[k], indx);

                var toDeliver = deliveryListItemViewModel[k].QtyRequested - deliveryListItemViewModel[k].QtyDelivered;
                var diff = moment.duration(new moment(deliveryListItemViewModel[k].MaxCoveredTime).diff(now));

                //console.log("covered time " + dlItemsList[k].Code );
                //console.log(new moment(dlItemsList[k].MaxCoveredTime));
                //console.log(diff);

                rowSummaryData = {
                    itemRow: $($(".itemRowData[data-itemid=" + deliveryListItemViewModel[k].ItemWMSId + "]")[indx]),
                    rowIndex: $($(".itemRowData[data-itemid=" + deliveryListItemViewModel[k].ItemWMSId + "]")).index(),
                    qtyToDeliver: toDeliver < 0 ? 0 : toDeliver,
                    minutesToEndOfStock: diff._milliseconds / 60000,
                    remainingStock: 0,
                    itemDeliveredQty: deliveryListItemViewModel[k].QtyDelivered,
                    itemDemandSum: deliveryListItemViewModel[k].QtyRequested,
                    itemUsedQty: deliveryListItemViewModel[k].QtyUsed
                };
                self._putSummaryOfRow(rowSummaryData);

                $("#rowTwo").removeClass("hidden");
                $("#rowTree").addClass("hidden");
                $("#loading").html("");
            }
            //callback();
        });
    }

    function DrawWorkorderHeader(workorder, i) {
        //NO WO HEADERS IN THIS VIEW
        console.log("DeliveryListMultiLines.DrawWorkorderHeader. Function not used ");
    }
    function _DrawWoItemDataCell(itemId, woId, qty, qtyPerPackage, qtyDelivered, BomQty) {
        //NO ITEM CELLS IN THIS VIEW
        console.log("DeliveryListMultiLines._DrawWoItemDataCell. Function not used ");
    }
};