//iLOGIS StocksGrid

var StockWarehouseDetailsGrid = function (gridDivSelector, _rowClickCallback){
	GridBulkUpdate.call(this, gridDivSelector);
	var self = this;
	this.divSelector = gridDivSelector;
	this.SelectedItem = null;
	this.SelectedRowIndex = 0;
	this.rowClickCallback = _rowClickCallback;
	this.gridHelper = new GridHelper("StockWarehouseDetails", "/iLogis/StockUnit");
	//this.gridHelper.SetFilter({ warehouseName: warehouse,warehouseLocationName: warehouseLocation, itemCode:itemCode })
};

StockWarehouseDetailsGrid.prototype = Object.create(GridBulkUpdate.prototype);
StockWarehouseDetailsGrid.prototype.constructor = StockWarehouseDetailsGrid;

StockWarehouseDetailsGrid.prototype.InitGrid = function (height,ItemNameClass) {
	var grid = this;
	let _height = height != null ? height : $(this.divSelector).height() > 400 ? ($(this.divSelector).height()).toString() + "px" : "294px";

	console.log($(this.divSelector).height());

	$(this.divSelector).jsGrid({
		width: "100%", height: function () { return _height; },
		bulkUpdate: false,
		inserting: false, editing: false, sorting: false,
		paging: true, pageLoading: true, pageSize: 100, pageButtonCount: 5,
		confirmDeleting: false, filtering: false,
		fields: [
			{ name: "AccountingWarehouseCode", type: "text", title: "Mag.Ks", width: 50 },
			{ name: "WarehouseLocationName", type: "text", title: "Lokacja", width: 50, css: "textLightOrange" },
            //{ name: "WarehouseLocationType", type: "text", title: "Typ", width: 65 },
			{ name: "ItemCode", type: "text", title: "Kod Artykuł", width: 80, css:"text-center textWhiteBold" },
			{ name: "ItemName", type: "text", title: "Nazwa Artykułu", width: 110, css: ItemNameClass + " textGrayout text-truncate" },
			{
				name: "CurrentQtyinPackage", type: "text", title: "Ilość", width: 45,
				itemTemplate: function (value, item) { return '<td style="color: ' + (value > 0 ? "#5cde5c" : "#f0826d") + '">' + (value != 0 ? value : "") + '</td>'; }
			},
			{
				name: "ReservedQty", type: "text", title: "Res.", width: 45,
				itemTemplate: function (value, item) { return '<td style="color: ' + (value > 0 ? "#d9cb59" : "#607c85") + '">' + (value != 0 ? value : "") + '</td>'; }
			},
			{
				name: "UnitOfMeasure", type: "text", title: "JM", width: 35, css: "textGrayout",
				itemTemplate: function (value, item) { return ConvertUoM(value); }
			}
            //{ name: "SerialNumber", type: "text", title: "Nr. Seryjny", width: 65},
            //{ name: "PercentageOccupation", type: "text", title: "Wypełnienie", width: 65 }
		],
		controller: grid.gridHelper.DB,
		onDataLoaded: function () {
			console.log("grid data loaded");
		},
		rowDoubleClick: function (args) {
			grid.rowClickCallback(args);
		},
		rowClick: function (args) {
			$(grid.grid._bodyGrid[0].rows[grid.SelectedRowIndex]).mouseleave();
			grid.SelectedItem = args.item;
			grid.SelectedRowIndex = args.itemIndex;
		}
	});
	this.grid = $(this.divSelector).data("JSGrid");
};
StockWarehouseDetailsGrid.prototype.CreateNewGridInstance = function (divSelector) {
	return new StockWarehouseDetailsGrid(divSelector);
};
StockWarehouseDetailsGrid.prototype.RefreshGrid = function (filterData) {
	if (filterData != null) {
		this.gridHelper.SetFilter(filterData);
	}
	$(this.divSelector).jsGrid("search");
};
