jsGrid.Grid.prototype.ids = [1, 2, 3];

$.extend(
    jsGrid.ControlField.prototype, {
        _createExportButton: function () {
            return this._createGridButton("jsgrid-export-button", "export data", function (grid) {
                grid.ExportExcel();
            });
        },
        filterTemplate: function () {
            var $result = this._createSearchButton();
            $result = this.clearFilterButton ? $result.add(this._createClearFilterButton()) : $result;
            $result = this._grid.exporting ? $result.add(this._createExportButton()) : $result;
            return $result;
        }
});

$.extend( 
    jsGrid.Grid.prototype, {
        //height: function () {
        //    let height_ = $(this).height();
        //    console.log(height_ + " wysokosc");
        //    let retH = height_ >= 500 ? (height_).toString() + "px" : "auto";
        //    return retH;
        //    //$(this.__container).height() > 400? ($(this.__container).height()).toString() + "px" : "600px";
        //},
        title: "",
        exporting: false,
        selectedRowIndex: 0,
        selectedCellIndex: 0,
        onItemInserted: function (args) {
            $(this._insertRow[0]).find(".jsgrid-insert-button").css('display', 'unset');
        },
        onItemInserting: function (args) {
            $(this._insertRow[0]).find(".jsgrid-insert-button").css('display', 'none');
        },
        confirmDeleting: false,
        bulkUpdate: false,
        onItemUpdating: function(args) {
            if(args.grid.bulkUpdate == true){
                console.log('bulkUpdate');
                args.cancel = true;
                args.grid._controller.updateManyItems(args.item, args.grid.ids);
            }else{
                console.log('normal');
            }
        },

        _updateRow: function($updatingRow, editedItem) {
            var updatingItem = $updatingRow.data("JSGridItem"),
                updatingItemIndex = this._itemIndex(updatingItem),
                updatedItem = $.extend(true, {}, updatingItem, editedItem);

            var args = this._callEventHandler(this.onItemUpdating, {
                row: $updatingRow,
                item: updatedItem,
                itemIndex: updatingItemIndex,
                previousItem: updatingItem
            });

            return this._controllerCall("updateItem", updatedItem, args.cancel, function(loadedUpdatedItem) {
                var previousItem = $.extend(true, {}, updatingItem);

                if (loadedUpdatedItem.Error == true) {
                    updatedItem = loadedUpdatedItem.Item;
                }else{
                    //sprawdza czy coś zwrócono z kontrolera na podstawie Id 
                    if (loadedUpdatedItem.Id != null && loadedUpdatedItem.Id > 0) {
                        updatedItem = loadedUpdatedItem;
                    }
                    else {
                        updatedItem = $.extend(true, updatingItem, editedItem);
                    }
                }
               
                var $updatedRow = this._finishUpdate($updatingRow, updatedItem, updatingItemIndex);

                this._callEventHandler(this.onItemUpdated, {
                    row: $updatedRow,
                    item: updatedItem,
                    itemIndex: updatingItemIndex,
                    previousItem: previousItem,
                });
            });
        },

        bulkUpdateItemFunction: function (item, ids) { 
            console.log("buItem - ids");
            console.log(ids);
        },            
        onItemDeleting: function (args) {
            //console.log(args);
            if (!args.item.deleteConfirmed) {
                args.cancel = true;
                bootbox.confirm({
                    message: "Jesteś pewny, że chcesz to usunąć?",
                    size: 'small',
                    buttons: {
                        cancel: {
                            label: '<i class="fa fa-times"></i> NIE'
                        },
                        confirm: {
                            label: '<i class="fa fa-check"></i> TAK'
                        }
                    },
                    callback: function (result) {
                        if (result == true) {
                            args.item.deleteConfirmed = true;
                            args.grid._controller.deleteItem(args.item);
                            $(args.row).remove();
                        }
                    }
                });
            }
        },
        rowClick: function (args) {
            //console.log("row click proto");
            var cell = args.event.target;
            var row = $(cell).closest("tr");
            this.SelectCell(row[0].rowIndex, cell.cellIndex);
        },
        finishDelete: function (deletedItem, deletedItemIndex) {
            var grid = this._grid;
            grid.option("data").splice(deletedItemIndex, 1);
            //console.log("delete finished kk");
            //grid.reset();
        },
        addCustomRow: function (customItem) {
            var grid = this;
            grid.option("data").push(customItem);
            grid.refresh();
        },
        rowDoubleClick: function (args) {
            //console.log(args);
            var cell = args.event.target;
            var row = $(cell).closest("tr");
            this.SelectCell(row[0].rowIndex, cell.cellIndex);

            this.editItem(this.data[this.selectedRowIndex]);
            this.FocusOnInput();
        },
        ChangeCell: function (dirH, dirV) {
            if (this._editingRow == null) {
                this.SelectCell(this.selectedRowIndex + dirV, this.selectedCellIndex + dirH)
            }
        },
        SelectCell: function (r, c) {
            //check in rowIndex in range of number of rows
            this.selectedRowIndex = this.selectedRowIndex == null ||this.selectedRowIndex > this.data.length ? 0 : this.selectedRowIndex;
            //deselect old cell
            //$(this._bodyGrid[0].rows[this.selectedRowIndex].cells[this.selectedCellIndex]).removeClass("selectedCell");
            //deselect all cells//deselect all cells
            $(this._bodyGrid[0]).find(".jsgrid-cell").removeClass("selectedCell");
            //refresh coordinates, take care about borders
            let rowUpperBound = this.paging == true ? Math.min(this.pageSize, this.data.length) : this.data.length;
            let colUpperBound = this.fields.length;

            this.selectedRowIndex = (r >= 0) ? ((r < rowUpperBound) ? r : rowUpperBound - 1) : 0;
            this.selectedCellIndex = (c >= 0) ? ((c < colUpperBound) ? c : colUpperBound - 1) : 0;
            //select new cell
            $(this._bodyGrid[0].rows[this.selectedRowIndex].cells[this.selectedCellIndex]).addClass("selectedCell");
        },
        FocusOnInput: function () {
            var input = $(this._bodyGrid[0].rows[this.selectedRowIndex].cells[this.selectedCellIndex]).find("input, select");
            $(input).focus();
            $(input).select();
        },
        FocusOnFilter: function () {
            var filtering = this.option("filtering");
            if (filtering == false) {
                this.option("inserting", false);
                this.option("filtering", true);
            }
            var tableH = $(this._headerGrid[0]).find(".jsgrid-filter-row");
            if (tableH.length > 0) {
                var input = $(tableH[0].cells[this.selectedCellIndex]).find("input, select");
                if (!(input.length > 0)) {
                    input = $(tableH[0]).find("input, select");
                }
                if (input.length > 0) {
                    $(input[0]).focus();
                    $(input[0]).select();
                }
            }
        },
        FocusOnInsert: function () {
            var inserting = this.option("inserting");
            if (inserting == false) {
                this.option("inserting", true);
                this.option("filtering", false);
            }
            var tableH = $(this._headerGrid[0]).find(".jsgrid-insert-row");
            if (tableH.length > 0) {
                input = $(tableH[0]).find("input, select");
                if (input.length > 0) {
                    $(input[0]).focus();
                    $(input[0]).select();
                }
            }
        },
        ExportExcel: function () {
            console.log("ExportExcel");
            let title = this.title != null ? this.title : "";
            let headers = this.fields;
            let filter = this.getFilter();
            filter = filter || (this.filtering ? this.getFilter() : {});

            $.extend(filter, this._loadStrategy.loadParams(), this._sortingParams());
            filter.pageSize = 100000;

            var args = this._callEventHandler(this.onDataLoading, { filter: filter });

            this._controllerCall("loadData", filter, args.cancel, function (loadedData) {
                let data = this.paging ? loadedData.data : loadedData;
                JSONToCSVConvertor(data, title, true, headers);
            });
        }
    });

var MyDateField = function (config) {
    //console.log("MyDateField");
    //console.log(config);
    jsGrid.Field.call(this, config);
};
MyDateField.prototype = new jsGrid.Field({
    //sorter: function (date1, date2) { return new Date(date1) - new Date(date2);},
    itemTemplate: function (value) {
        return moment(value).format('YYYY-MM-DD HH:mm');
    },
    insertTemplate: function (value) {
        if (this.inserting) {
            return this._insertPicker = $("<input class='datetimepicker'>");
        }
        else {
            return this._insertPicker = null;
        }
    },
    filterTemplate: function (value) {
        if (this.filtering) {
            return this._filterPicker = $("<input class='datetimepicker'>");
        }
        else {
            return this._filterPicker = null;
        }
    },
    editTemplate: function (value) {
        if (this.editing) {
            return this._editPicker = $("<input class='datetimepicker'>").val(moment(value).format('YYYY-MM-DD HH:mm'));
        }
        else {
            return this._editPicker = $("<div class=''>").text(moment(value).format('YYYY-MM-DD HH:mm'));
        }
    },
    insertValue: function () { return this._insertPicker.val(); },
    filterValue: function () { return this._filterPicker.val(); },
    editValue: function () { return this._editPicker.val(); }
});
jsGrid.fields.date = MyDateField;

function GridHelper(objName, controllerUrl = "",divSelector = "") {
    this.onItemDeletingBehavior = function(args, divSelector){
        if (!args.item.deleteConfirmed) {
            args.cancel = true;
            bootbox.confirm({
                message: "Jesteś pewny, że chcesz to usunąć?",
                size: 'small',
                buttons: {
                    cancel: {
                        label: '<i class="fa fa-times"></i> NIE'
                    },
                    confirm: {
                        label: '<i class="fa fa-check"></i> TAK'
                    }
                },
                callback: function (result) {
                    if (result == true) {
                        args.item.deleteConfirmed = true;
                        $(divSelector).jsGrid("deleteItem", args.item);
                    }
                }
            });
        }
    };

    var self = this;
    var objectName = controllerUrl + "/" + objName;
    var filterPlus = null;
    var _filter = null;

    this.DB = function () {
        var db = {
            loadData: function (filter) { return self.GetList(true, filter); },
            insertItem: function (item) { return self.Update(item); },
            updateItem: function (item) { return self.Update(item); },
            deleteItem: function (item) { return self.Delete(item); },
            updateManyItems: function (item, ids) { return self.UpdateMany(item, ids); },
        };
        return db;
    };
    this.SetFilter = function (filterPlus = {}, filterName = "") {
        
        this.filterPlus = filterPlus;
        this.filterExtendName = {
            filterName: this.filterPlus
        };
        //console.log(this.filterExtendName);
    };
    this.GetFilter = function () {
        return _filter;
    };

    this.GetList = function (async, filter) {
        $.extend(filter, this.filterExtendName);       
        var mixFilter = filter;
        console.log("GridHelper.GetList");
        if (this.filterPlus !== null && this.filterPlus != undefined) {
            mixFilter = Object.assign(filter, this.filterPlus);
        }
        //console.log(mixFilter);

        _filter = mixFilter;

        return $.ajax({
            async: async, type: "POST", data: mixFilter,
            url: objectName + "GetList",
            success: function (data) {
                //console.log(data);
            },
            error: function (xhr, ajaxOptions, thrownError) {
                //new Alert().Show("danger", thrownError);
            }
        });
    };
    this.Update = function (item) {
        //console.log(item);
        return $.ajax({
            async: false,
            type: "POST",
            data: item,
            url: objectName + "Update",
            success: function (message) {
                if(message.Error == true){
                    new Alert().Show("danger", "Zapis nie udał się", true);
                }else{
                    new Alert().Show("success", "Zapis zakończony", true);
                }
            },
            error: function (xhr, ajaxOptions, thrownError) {
                new Alert().Show("danger", "Zapis nie powiódł się. " + thrownError, true);
            }
        });
    };
    this.Delete = function (item) {
        return $.ajax({
            async: true, type: "POST", data: item,
            url: objectName + "Delete",
            success: function (message) {
                new Alert().Show("success", "Usunięto", true);
            },
            error: function (xhr, ajaxOptions, thrownError) {
                new Alert().Show("danger", "Usuwanie nie powiodło się. " + thrownError, true);
            }
        });
    };
    this.UpdateMany = function (item, ids) {
        //console.log(item);
        console.log("GridHelper.UpdateMany");
        return $.ajax({
            async: true, type: "POST", data: { item, ids },
            url: objectName + "GroupUpdate",
            success: function (message) {
                if(message == false){
                    new Alert().Show("danger", "Zapis nie udał się", true);
                }else{
                    new Alert().Show("success", "Zapis zakończony", true);
                }
            },
            error: function (xhr, ajaxOptions, thrownError) {
                new Alert().Show("danger", "Zapis nie powiódł się. " + thrownError, true);
            }
        });
    };
        
    //-------COLUMN-TEMPLATES-----------------------------
    //---------------------------------------------------
    this.ColumColor = function (_width, _name = "Color", _title = "KOLOR") {
        return {
            name: _name, title: _title, type: "text", inserting: false, filtering: false, width: _width > 0? _width : 80,
            editTemplate: ColumnColorEditItemTemplate(),
            itemTemplate: ColumnColorItemTemplate(),
            editValue: ColumnColorValue()
        };
    };
    this.ColumColorEmpty = function () {
        return {
            name: "Color2", title: "KOLOR", type: "text", inserting: false, width: '80',filtering:false,
            editTemplate: ColumnColorEditItemTemplate(),
            itemTemplate: ColumnColorItemTemplate(),
            editValue: ColumnColorValue()
        };
    };

    ColumnColorValue = function () {
        return function () {
            if (this != undefined && this._editPicker != undefined)
                return this._editPicker.css("background-color");
            else
                return "";
        }
    };
    ColumnColorItemTemplate = function () {
        return function (value, item) {
            return '<div class="colorSelector"><div id="Color_' + item.Id + '" style="background-color: ' + value + ';"></div></div>';
        };
    };
    ColumnColorEditItemTemplate = function () {
        return function (value, item) {
            $el = $('<div class="colorColumn">');
            $cs = $('<div class="colorSelector">');
            $colorPresenter = $('<div class="colorPresenter" style="background-color: ' + value + '">');
            $picker = $('<div class="fas fa-paint-brush" style="float: left; min-width:24px; min-height:24px; padding: 2px 5px 0 5px;"></div>');
            $random = $('<div class="fab fa-pinterest" style="float: left; min-width:24px; min-height:24px; padding: 2px 5px 0 5px;"></div>');

            $el.append($cs);
            $el.append($picker);
            $el.append($random);
            $cs.append($colorPresenter);

            this._editPicker = $colorPresenter;

            $random.click(function () {
                let colorCol = $(this).closest(".colorColumn");
                $(colorCol).find(".colorPresenter").css("background-color", "rgb(" + Math.floor(Math.random() * 255) + "," + Math.floor(Math.random() * 255) + "," + Math.floor(Math.random() * 255) + ")");
            });
            new MyColorPicker2().Init($picker, $colorPresenter);

            return $el;
        };
    };
}
function JSONToCSVConvertor(JSONData, ReportTitle, ShowLabel, headers) {
    //If JSONData is not an object then JSON.parse will parse the JSON string in an Object
    var arrData = typeof JSONData != 'object' ? JSON.parse(JSONData) : JSONData;
    var CSV = '';
    var row = "";

    if (ShowLabel) {
        row = "";
        for (index = 0; index < headers.length; index++) {
            row += headers[index].title + ';';  //Now convert each value to string and comma-seprated
        }
        row = row.slice(0, -1);
        CSV += row + '\r\n';
    }

    //1st loop is to extract each row
    for (var i = 0; i < arrData.length; i++) {
        row = "";
        let name = "";
        let value = "";
        //2nd loop will extract each column and convert it in string comma-seprated
        for (index = 0; index < headers.length; index++) { 
            name = headers[index].name;
            if (name != null) {
                value = arrData[i][name];
                value = (value == null || value == "undefined" || value == "null") ? "" : value;

                if (value != "" && headers[index].type == "date") {
                    value = moment(value).format('YYYY-MM-DD HH:mm');
                }
            }
            else {
                value = "";
            }
            row += '"' + value + '";';
        }

        row.slice(0, row.length - 1);
        CSV += row + '\r\n'; //add a line break after each row
    }

    if (CSV == '') {
        alert("Invalid data");
        return;
    }

    var filename = "GridDownload_" + ReportTitle.replace(/ /g, "_");
    var blob = new Blob([CSV], { type: 'text/csv;charset=utf-8;' });

    if (navigator.msSaveBlob) { // IE 10+
        navigator.msSaveBlob(blob, filename);
    } else {
        var link = document.createElement("a");
        if (link.download !== undefined) {
            var url = URL.createObjectURL(blob);
            link.setAttribute("href", url);
            link.style = "visibility:hidden";
            link.download = filename + ".csv";
            document.body.appendChild(link);
            link.click();
            document.body.removeChild(link);
        }
    }
}