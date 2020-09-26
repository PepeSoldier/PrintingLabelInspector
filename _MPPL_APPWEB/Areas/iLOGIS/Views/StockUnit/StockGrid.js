//iLOGIS StocksGrid

var StocksGrid = function (gridDivSelector, _rowClickCallback){
	GridBulkUpdate.call(this, gridDivSelector);
	var self = this;
	this.divSelector = gridDivSelector;
	this.rowClickCallback = _rowClickCallback;
	this.gridHelper = new GridHelper("Stock", "/iLogis/StockUnit");
	//this.gridHelper.SetFilter({ warehouseName: warehouse,warehouseLocationName: warehouseLocation, itemCode:itemCode })
};

StocksGrid.prototype = Object.create(GridBulkUpdate.prototype);
StocksGrid.prototype.constructor = StocksGrid;

StocksGrid.prototype.InitGrid = function (height, WarehouseDescriptionClass, onDataLoadedCallback) {
	var grid = this;
	let _height = height != null ? height : $(this.divSelector).height() > 400 ? ($(this.divSelector).height()).toString() + "px" : "294px";

	$(this.divSelector).jsGrid({
		
		width: "100%", height: function () { return _height; },
		bulkUpdate: false,
		inserting: false, editing: false, sorting: false,
		paging: false, pageLoading: false, pageSize: 20,
		confirmDeleting: false, filtering: false,
		fields: [
			{ name: "WarehouseCode", type: "text", title: "Magazyn", width: 80},
			{ name: "WarehouseName", type: "text", title: "Opis", width: 200, css: WarehouseDescriptionClass + " textGrayout" },
			{
				name: "CurrentQtyinPackage", type: "text", title: "Ilość", width: 50,
				itemTemplate: function(value, item) { return '<td style="color: ' + (value > 0 ? "#5cde5c" : "#f0826d") + '">' + (value != 0 ? value : "") + '</td>'; }
			},
			{
				name: "UnitOfMeasure", type: "text", title: "JM", width: 40, css: "textGrayout",
				itemTemplate: function (value, item) { return ConvertUoM(value); }
			}
		],
		controller: grid.gridHelper.DB,
		onDataLoaded: function () {
			console.log("grid data loaded");
			if (onDataLoadedCallback != null) {
				onDataLoadedCallback(this.data);
			}
		},
		rowDoubleClick: function (args) {
			//$("#itemCode").val(args.item.ItemCode);
			//$("#warehouseLocation").val(args.item.WarehouseLocationName);
			//$("#warehouse").val(args.item.WarehouseName);
			//Refreshgrid();
			if (grid.rowClickCallback != null) {
				grid.rowClickCallback(args);
			}
		},
		rowClick: function(args) {
			
		}
	});
	this.grid = $(this.divSelector).data("JSGrid");
};
StocksGrid.prototype.CreateNewGridInstance = function (divSelector) {
	return new StocksGrid(divSelector);
};
StocksGrid.prototype.RefreshGrid = function (filterData) {
	if (filterData != null) {
		this.gridHelper.SetFilter(filterData);
	}
	$(this.divSelector).jsGrid("search");
};
