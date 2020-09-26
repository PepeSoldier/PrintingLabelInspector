class Stock {

    stocksGrid = {};
    stockWarehouseDetailsGrid = {};
    stockLocationDetailsGrid = {};
    filterFormId = "";

    constructor(_filterFormId) {
        console.log("Stock.Class.Constructor");

        this.StocksGrid_RowSelected = this.StocksGrid_RowSelected.bind(this);
        this.StockWarehouseDetailsGrid_RowSelected = this.StockWarehouseDetailsGrid_RowSelected.bind(this);

        this.Actions();
        this.filterFormId = _filterFormId;

        this.stocksGrid = new StocksGrid("#stocksGrid", this.StocksGrid_RowSelected);
        this.stockWarehouseDetailsGrid = new StockWarehouseDetailsGrid("#stockWarehouseDetailsGrid", this.StockWarehouseDetailsGrid_RowSelected);
        this.stockLocationDetailsGrid = new StockLocationDetailsGrid("#stockLocationDetailsGrid");

        this.stocksGrid.InitGrid("100%", "", this.SumQtyOnMRPAndNoMRPWarehouses);
        this.stockWarehouseDetailsGrid.InitGrid();
        this.stockLocationDetailsGrid.InitGrid();
    }

    Refreshgrid(){
        let filters = $(this.filterFormId).serializeToJSON();
        this.stocksGrid.RefreshGrid(filters);
        this.stockLocationDetailsGrid.RefreshGrid(filters);
        this.stockWarehouseDetailsGrid.RefreshGrid(filters);
    }

    StocksGrid_RowSelected(args) {
        console.log("Callback_StocksGrid_RowSelected");
        $(this.filterFormId).find('[name="warehouseName"]').val(args.item.WarehouseName);
        $(this.filterFormId).find('[name="warehouseName"]').focus();
        $(this.filterFormId).find('[name="warehouseName"]').select();
        this.Refreshgrid();
    }
    StockWarehouseDetailsGrid_RowSelected(args) {

        console.log("Callback_StockWarehouseDetailsGrid_RowSelected");
        console.log(args.event.target.cellIndex);

        if (args.event.target.cellIndex == 1) {
            $(this.filterFormId).find('[name="warehouseLocationName"]').val(args.item.WarehouseLocationName);
            $(this.filterFormId).find('[name="warehouseLocationName"]').focus();
            $(this.filterFormId).find('[name="warehouseLocationName"]').select();
        }
        else if(args.event.target.cellIndex == 2) {
            $(this.filterFormId).find('[name="itemUoM"]').val(ConvertUoM(args.item.ItemUoM));
            $(this.filterFormId).find('[name="itemName"]').val(args.item.ItemName);
            $(this.filterFormId).find('[name="itemCode"]').val(args.item.ItemCode);
            $(this.filterFormId).find('[name="itemCode"]').focus();
            $(this.filterFormId).find('[name="itemCode"]').select();
        }
        this.Refreshgrid();
    }

    SumQtyOnMRPAndNoMRPWarehouses(data) {
        console.log("SumQtyOnMRPAndNoMRPWarehouses");

        let qtyNoMRP = 0;
        let qtyMRP = 0;
        let qtyTotal = 0;

        data.forEach(function (item) {
            console.log(item);
            if (item.WarehouseIsMRP == true) {
                qtyMRP += item.CurrentQtyinPackage;
            }
            else {
                qtyNoMRP += item.CurrentQtyinPackage;
            }
            qtyTotal += item.CurrentQtyinPackage;
        });

        $("#qtyNoMRP").text(qtyNoMRP);
        $("#qtyMRP").text(qtyMRP);
        $("#qtyTotal").text(qtyTotal);
    }

    Actions() {
        let self = this;
        $(document).off("click", "#searchButton");
        $(document).on("click", "#searchButton", function () {
            console.log("click");
            self.Refreshgrid();
        });

        $(document).off("keypress");
        $(document).on("keypress" ,function (event) {
            if (event.which == 13) {
                self.Refreshgrid();
            }
        });

        ItemWMSAutcomplete("", "#itemCode", "#itemName", "#itemUoM");
        //ItemWMSAutcomplete("", ".itemCodeColumn input", "");
    }
}


// jQuery SerializeToJSON v1.4.1
// github.com/raphaelm22/jquery.serializeToJSON
!function (e) { "function" == typeof define && define.amd ? define(["jquery"], e) : "object" == typeof module && module.exports ? module.exports = function (r, t) { return void 0 === t && (t = "undefined" != typeof window ? require("jquery") : require("jquery")(r)), e(t), t } : e(jQuery) }(function (e) { "use strict"; e.fn.serializeToJSON = function (r) { var t = { settings: e.extend(!0, {}, e.fn.serializeToJSON.defaults, r), getValue: function (r) { var t = r.val(); if (r.is(":radio") && (t = r.filter(":checked").val() || null), r.is(":checkbox") && (t = e(r).prop("checked")), this.settings.parseBooleans) { var n = (t + "").toLowerCase(); "true" !== n && "false" !== n || (t = "true" === n) } var i = this.settings.parseFloat.condition; return void 0 !== i && ("string" == typeof i && r.is(i) || "function" == typeof i && i(r)) && (t = this.settings.parseFloat.getInputValue(r), t = Number(t), this.settings.parseFloat.nanToZero && isNaN(t) && (t = 0)), t }, createProperty: function (r, t, n, i) { for (var a = r, s = 0; s < n.length; s++) { var u = n[s]; if (s === n.length - 1) { var l = i.is("select") && i.prop("multiple"); l && null !== t ? (a[u] = new Array, Array.isArray(t) ? e(t).each(function () { a[u].push(this) }) : a[u].push(t)) : "undefined" != typeof a[u] ? i.is("[type='hidden']") || (a[u] = t) : a[u] = t } else { var o = /\[\w+\]/g.exec(u), c = null != o && o.length > 0; if (c) { u = u.substr(0, u.indexOf("[")), this.settings.associativeArrays ? a.hasOwnProperty(u) || (a[u] = {}) : Array.isArray(a[u]) || (a[u] = new Array), a = a[u]; var f = o[0].replace(/[\[\]]/g, ""); u = f } a.hasOwnProperty(u) || (a[u] = {}), a = a[u] } } }, includeUncheckValues: function (r, t) { e(":radio", r).each(function () { var r = 0 === e("input[name='" + this.name + "']:radio:checked").length; r && t.push({ name: this.name, value: null }) }), e("select[multiple]", r).each(function () { null === e(this).val() && t.push({ name: this.name, value: null }) }) }, serializeArray: function (e) { var r = /\r?\n/g, t = /^(?:submit|button|image|reset|file)$/i, n = /^(?:input|select|textarea|keygen)/i, i = /^(?:checkbox|radio)$/i; return e.map(function () { var e = jQuery.prop(this, "elements"); return e ? jQuery.makeArray(e) : this }).filter(function () { var e = this.type; return this.name && !jQuery(this).is(":disabled") && n.test(this.nodeName) && !t.test(e) && (this.checked || !i.test(e)) }).map(function (e, t) { var n = jQuery(this).val(); return null == n ? null : Array.isArray(n) ? jQuery.map(n, function (e) { return { name: t.name, value: e.replace(r, "\r\n"), elem: t } }) : { name: t.name, value: n.replace(r, "\r\n"), elem: t } }).get() }, serializer: function (r) { var t = this, n = this.serializeArray(e(r)); this.includeUncheckValues(r, n); var i = {}; for (var a in n) if (n.hasOwnProperty(a)) { var s = n[a], u = e(s.elem), l = t.getValue(u), o = s.name.split("."); t.createProperty(i, l, o, u) } return i } }; return t.serializer(this) }, e.fn.serializeToJSON.defaults = { associativeArrays: !0, parseBooleans: !0, parseFloat: { condition: void 0, nanToZero: !0, getInputValue: function (e) { return e.val().split(",").join("") } } } });