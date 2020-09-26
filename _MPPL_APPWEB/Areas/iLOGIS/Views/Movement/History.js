//iLOGIS MovementHistory

var MovementHistory = function (gridDivSelector){
	GridBulkUpdate.call(this, gridDivSelector);
	var self = this;
	this.divSelector = gridDivSelector;
	Actions(self);
	this.gridHelper = new GridHelper("History", "/iLogis/Movement");
	//this.gridHelper.SetFilter({ warehouseName: warehouse,warehouseLocationName: warehouseLocation, itemCode:itemCode })
};

MovementHistory.prototype = Object.create(GridBulkUpdate.prototype);
MovementHistory.prototype.constructor = MovementHistory;

MovementHistory.prototype.InitGrid = function () {
	var grid = this;
	$(this.divSelector).jsGrid({
		width: "100%", height: "740px",
		bulkUpdate: false,
		inserting: false, editing: false, sorting: false, filtering: false,
		paging: true, pageLoading: true, pageSize: 20,
		confirmDeleting: false, deleting:false,
		fields: [
            { name: "Id", type: "text", title: "Id", width: 40, visible: false },
			{ name: "ItemCode", type: "text", title: "Kod Produktu", width: 120, css: "textWhiteBold" },
			{ name: "ItemName", type: "text", title: "Nazwa Produktu", width: 210, css: "textGrayout text-truncate" },
			{ name: "QtyMoved", type: "text", title: "Ilość", width: 60, itemTemplate: function (value, item) { return '<td style="color: ' + (value > 0 ? "#5cde5c" : "#f0826d") + '">' + value + '</td>';  } },
			{ name: "UnitOfMeasure", type: "text", title: "JM", width: 35, css: "textGrayout", itemTemplate: function (value, item) { return ConvertUoM(value); } },
			{ name: "SourceAccountingWarehouseCode", type: "text", title: "Mag. ksieg.", width: 60 },
			{ name: "SourceWarehouseName", type: "text", title: "Mag(źródło)", width: 210, css: "" },
			{ name: "SourceLocationName", type: "text", title: "Lokacja(źródło)", width: 140, css: "textLightOrange" },
			{ name: "SourceStockUnitSerialNumber", type: "text", title: "SN(źródło)", width: 100, css: "textBlue" },
			{ name: "ICON", type: "text", title: "", width: 40 ,align:"center", filtercss: "fltr", headercss: "hdr", css: "cellArrow", editing: false, filtering: false, itemTemplate(value, item) { return '<i class="fas fa-long-arrow-alt-right"></i>'; } },
			{ name: "DestinationAccountingWarehouseCode", type: "text", title: "Mag. ksieg.", width: 60 },
			{ name: "DestinationWarehouseName", type: "text", title: "Mag(cel)", width: 210, css: "" },
			{ name: "DestinationLocationName", type: "text", title: "Lokacja(cel)", width: 140, css: "textLightOrange" },
			{ name: "DestinationStockUnitSerialNumber", type: "text", title: "SN(cel)", width: 100, css: "textBlue"},
			{ name: "MovementType", type: "text", title: "Ruch", width: 60, css: "textPink"},
			{ name: "UserName", type: "text", title: "Użytkownik", width: 100, css: "textGrayout" },
			{ name: "MovementDate", type: "text", title: "Data przesunięcia", width: 140, css: "textGrayout" },
            //{ name: "Akcje2", type: "control", width: 100, modeSwitchButton: false, editButton: false, deleteButton:false }
		],
		controller: grid.gridHelper.DB,
		onDataLoaded: function () {
			console.log("grid data loaded");
		},
		rowClick: function(args) {

		},
	});
	this.grid = $(this.divSelector).data("JSGrid");
};
MovementHistory.prototype.CreateNewGridInstance = function (divSelector) {
	return new MovementHistory(divSelector);
};
MovementHistory.prototype.RefreshGrid = function (filterData) {
	if (filterData != null) {
		this.gridHelper.SetFilter(filterData);
	}
	$(this.divSelector).jsGrid("search");
};

function Actions(self) {
	$(document).off("click", "#searchButton");
	$(document).on("click", "#searchButton", function () {
		let filters = $("#formStock").serializeToJSON();
		self.RefreshGrid(filters);
	});
}


// jQuery SerializeToJSON v1.4.1
// github.com/raphaelm22/jquery.serializeToJSON
!function (e) { "function" == typeof define && define.amd ? define(["jquery"], e) : "object" == typeof module && module.exports ? module.exports = function (r, t) { return void 0 === t && (t = "undefined" != typeof window ? require("jquery") : require("jquery")(r)), e(t), t } : e(jQuery) }(function (e) { "use strict"; e.fn.serializeToJSON = function (r) { var t = { settings: e.extend(!0, {}, e.fn.serializeToJSON.defaults, r), getValue: function (r) { var t = r.val(); if (r.is(":radio") && (t = r.filter(":checked").val() || null), r.is(":checkbox") && (t = e(r).prop("checked")), this.settings.parseBooleans) { var n = (t + "").toLowerCase(); "true" !== n && "false" !== n || (t = "true" === n) } var i = this.settings.parseFloat.condition; return void 0 !== i && ("string" == typeof i && r.is(i) || "function" == typeof i && i(r)) && (t = this.settings.parseFloat.getInputValue(r), t = Number(t), this.settings.parseFloat.nanToZero && isNaN(t) && (t = 0)), t }, createProperty: function (r, t, n, i) { for (var a = r, s = 0; s < n.length; s++) { var u = n[s]; if (s === n.length - 1) { var l = i.is("select") && i.prop("multiple"); l && null !== t ? (a[u] = new Array, Array.isArray(t) ? e(t).each(function () { a[u].push(this) }) : a[u].push(t)) : "undefined" != typeof a[u] ? i.is("[type='hidden']") || (a[u] = t) : a[u] = t } else { var o = /\[\w+\]/g.exec(u), c = null != o && o.length > 0; if (c) { u = u.substr(0, u.indexOf("[")), this.settings.associativeArrays ? a.hasOwnProperty(u) || (a[u] = {}) : Array.isArray(a[u]) || (a[u] = new Array), a = a[u]; var f = o[0].replace(/[\[\]]/g, ""); u = f } a.hasOwnProperty(u) || (a[u] = {}), a = a[u] } } }, includeUncheckValues: function (r, t) { e(":radio", r).each(function () { var r = 0 === e("input[name='" + this.name + "']:radio:checked").length; r && t.push({ name: this.name, value: null }) }), e("select[multiple]", r).each(function () { null === e(this).val() && t.push({ name: this.name, value: null }) }) }, serializeArray: function (e) { var r = /\r?\n/g, t = /^(?:submit|button|image|reset|file)$/i, n = /^(?:input|select|textarea|keygen)/i, i = /^(?:checkbox|radio)$/i; return e.map(function () { var e = jQuery.prop(this, "elements"); return e ? jQuery.makeArray(e) : this }).filter(function () { var e = this.type; return this.name && !jQuery(this).is(":disabled") && n.test(this.nodeName) && !t.test(e) && (this.checked || !i.test(e)) }).map(function (e, t) { var n = jQuery(this).val(); return null == n ? null : Array.isArray(n) ? jQuery.map(n, function (e) { return { name: t.name, value: e.replace(r, "\r\n"), elem: t } }) : { name: t.name, value: n.replace(r, "\r\n"), elem: t } }).get() }, serializer: function (r) { var t = this, n = this.serializeArray(e(r)); this.includeUncheckValues(r, n); var i = {}; for (var a in n) if (n.hasOwnProperty(a)) { var s = n[a], u = e(s.elem), l = t.getValue(u), o = s.name.split("."); t.createProperty(i, l, o, u) } return i } }; return t.serializer(this) }, e.fn.serializeToJSON.defaults = { associativeArrays: !0, parseBooleans: !0, parseFloat: { condition: void 0, nanToZero: !0, getInputValue: function (e) { return e.val().split(",").join("") } } } });
