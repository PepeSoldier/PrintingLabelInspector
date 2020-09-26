//iLOGIS StocksGrid

var StockLocationDetailsGrid = function (gridDivSelector){
	GridBulkUpdate.call(this, gridDivSelector);
	var self = this;
	this.divSelector = gridDivSelector;
	
	this.gridHelper = new GridHelper("StockLocationDetails", "/iLogis/StockUnit");
	//this.gridHelper.SetFilter({ warehouseName: warehouse,warehouseLocationName: warehouseLocation, itemCode:itemCode })
};

StockLocationDetailsGrid.prototype = Object.create(GridBulkUpdate.prototype);
StockLocationDetailsGrid.prototype.constructor = StockLocationDetailsGrid;

StockLocationDetailsGrid.prototype.InitGrid = function (height) {
	var grid = this;
	let _height = height != null ? height : $(this.divSelector).height() > 400 ? ($(this.divSelector).height()).toString() + "px" : "294px";

	let i = 0;
	$(this.divSelector).jsGrid({
		width: "100%", height: function () { return _height; },
		bulkUpdate: false,
		inserting: false, editing: false, sorting: false,
		paging: true, pageLoading: true, pageSize: 100, pageButtonCount: 5,
		confirmDeleting: false, filtering: false,
		fields: [
			{
				name: "Index", title: "", type: "number", width: 30, align: "center", itemTemplate: function () { i++; return i; }
			},
			{
				name: "ItemCode", type: "text", title: "Kod Art.", width: 100, css: "textWhiteBold"
			},
			{
				name: "SerialNumber", type: "text", title: "Nr. Seryjny", width: 90, css: "textBlue"
			},
			{
				name: "CurrentQtyinPackage", type: "text", title: "Ilość", width: 50,
				itemTemplate: function (value, item) { return '<td style="color: ' + (value > 0 ? "#5cde5c" : "#f0826d") + '">' + (value != 0 ? value : "") + '</td>'; }
			},
			{
				name: "ReservedQty", type: "text", title: "Res.", width: 50,
				itemTemplate: function (value, item) { return '<td style="color: ' + (value > 0 ? "#d9cb59" : "#607c85") + '">' + (value != 0 ? value : "") + '</td>'; }
			},
			{
				name: "UnitOfMeasure", type: "text", title: "JM", width: 35, css: "textGrayout",
				itemTemplate: function (value, item) { return ConvertUoM(value); }
			},
			{
				name: "Status", type: "text", title: "S", width: 20,
				itemTemplate: function (value, item) { return grid.TranslateStatus(value); }
			},
			{
				width: 40, filtering: false, editing: false, inserting: false,
				itemTemplate: function (value, item) {
					return $("<button>").html('<i class="fas fa-print"></i>')
						.addClass("btn btn-sm btn-info btnPrintLabelForStockUnit")
						.attr("data-stockUnitId", item.Id)
						.attr("data-stockUnitSerialNumber", item.SerialNumber)
						.on("click", function () { PrintLabelForStockUnit(item.Id, item.SerialNumber); });
				}
			},
		],
		controller: grid.gridHelper.DB,
		onDataLoading: function (args) {
			i = 0;
		},
		onDataLoaded: function () {
			console.log("grid data loaded");
		},
		rowDoubleClick: function (args) {
			//$("#itemCode").val(args.item.ItemCode);
			//$("#warehouseLocation").val(args.item.WarehouseLocationName);
			//$("#warehouse").val(args.item.WarehouseName);
			//Refreshgrid();
		},
		rowClick: function(args) {

		}
	});
	this.grid = $(this.divSelector).data("JSGrid");
};
StockLocationDetailsGrid.prototype.InitGridExtended = function (height) {
	var grid = this;
	let _height = height != null ? height : $(this.divSelector).height() > 400 ? ($(this.divSelector).height()).toString() + "px" : "294px";
	
	let i = 0;
	$(this.divSelector).jsGrid({
		width: "100%", height: function () { return _height; },
		bulkUpdate: false,
		inserting: false, editing: false, sorting: false,
		paging: true, pageLoading: true, pageSize: 10,
		confirmDeleting: false, filtering: false,
		fields: [
			{ name: "Index", title: "L.P.", type: "number", width: 40, align: "center", itemTemplate: function () { i++; return i; } },
			{ name: "WarehouseCode", type: "text", title: "Magazyn", width: 65, css: "" },
			{ name: "WarehouseLocationName", type: "text", title: "Lokacja", width: 65, css: "" },
			{ name: "ItemCode", type: "text", title: "Kod Art.", width: 75, css: "textWhiteBold" },
			{ name: "SerialNumber", type: "text", title: "Nr. Seryjny", width: 90, css: "textBlue" },
			{
				name: "CurrentQtyinPackage", type: "text", title: "Ilość", width: 50,
				itemTemplate: function (value, item) { return '<td style="color: ' + (value > 0 ? "#5cde5c" : "#f0826d") + '">' + (value != 0 ? value : "") + '</td>'; }
			},
			{
				name: "UnitOfMeasure", type: "text", title: "JM", width: 40, css: "textGrayout",
				itemTemplate: function (value, item) { return ConvertUoM(value); }
			},
			{
				width: 40, filtering: false, editing: false, inserting: false,
				itemTemplate: function (value, item) {
					return $("<button>").html('<i class="fas fa-long-arrow-alt-right"></i>')
						.addClass("btn btn-sm btn-info btnSelectStockUnit")
						.attr("data-stockUnitId", item.Id)
						.on("click", function () { SelectStockUnit(item.Id); });
				}
			},
		],
		controller: grid.gridHelper.DB,
		onDataLoading: function (args) {
			i = 0;
		},
		onDataLoaded: function () {
			console.log("grid data loaded");
		},
		rowDoubleClick: function (args) {
			//$("#itemCode").val(args.item.ItemCode);
			//$("#warehouseLocation").val(args.item.WarehouseLocationName);
			//$("#warehouse").val(args.item.WarehouseName);
			//Refreshgrid();
		},
		rowClick: function (args) {

		}
	});
	this.grid = $(this.divSelector).data("JSGrid");
};

StockLocationDetailsGrid.prototype.CreateNewGridInstance = function (divSelector) {
	return new StockLocationDetailsGrid(divSelector);
};
StockLocationDetailsGrid.prototype.RefreshGrid = function (filterData) {
	
	if (filterData != null) {
		this.gridHelper.SetFilter(filterData);
	}
	$(this.divSelector).jsGrid("search");
};
StockLocationDetailsGrid.prototype.TranslateStatus = function (status) {

	switch (status) {
		case 0: return "";
		case 1: return "A";
		case 2: return "R";
		case 3: return "B";
		case 4: return "Q";
		case 50: return "!";
	}
};

