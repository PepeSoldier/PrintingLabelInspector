//-------------------GRID-DEFAULT------------------------------
var GridDefault = function(gridDivSelector) {
    var self = this;
    this.divSelector = gridDivSelector;
    this.gridHelper = new GridHelper("", "/AREA/Controller");
    this.grid = {};

    this.KeysActivate();
};

GridDefault.prototype = {
    RefreshGrid: function () {
        $(this.divSelector).jsGrid("search");
    },
    InitGrid: function () {
        $(this.divSelector).jsGrid({
            width: "100%",
            fields: [
                { name: "Id", type: "text", title: "Id", width: 50, filtering: false, editing: false, inserting: false },
                { name: "Name", type: "text", title: "Nazwa", width: 200, filtering: false, editing: false, inserting: false },
                { type: "control", width: 100, modeSwitchButton: true, editButton: true }
            ],
            controller: this.gridHelper.DB,
            onDataLoaded: function () { console.log("grid data loaded"); },
            rowClick: function (args) { },
        });
    },
    KeysActivate: function () {
        var selector = this.divSelector;
        var ctrlDown = false;

        $(this.divSelector).attr("tabindex", 9999); //tabindex is required to make keydown working inside .jsgrid

        $(document).on('keydown', ".jsgrid", (event) => {
            if (event.which === 17) {
                ctrlDown = true;
            }
        });
        $(document).on('keyup', ".jsgrid", (event) => {
            if (event.which === 17) {
                ctrlDown = false;
            }
        });
        $(document).on('keydown', ".jsgrid", (event) => {
            console.log("keydown .jsgrid");
            if (event.which > 0) {
                var _ff = $(":focus").closest(".jsgrid-filter-row");
                var _if = $(":focus").closest(".jsgrid-insert-row");
                filterFocused = _ff.length > 0;
                insertFocused = _if.length > 0;
                //console.log("key pressed: " + event.which);
                //CTRL+ARROWS
                if (ctrlDown && event.which === 37) {
                    args[1]._grid.openPage(args[1]._grid.pageIndex - 1);
                    return true;
                }
                if (ctrlDown && event.which === 39) {
                    args[1]._grid.openPage(args[1]._grid.pageIndex + 1);
                    return true;
                }
                //CTRL+N
                if (ctrlDown && event.which === 78) {
                    console.log("insert new");
                    args[1]._grid.option("inserting", true);
                    return false;
                }
                //ARROWS
                if (!(filterFocused || insertFocused)) {
                    if (event.which === 37)
                        args[1]._grid.ChangeCell(-1, 0);
                    else if (event.which === 39)
                        args[1]._grid.ChangeCell(1, 0);
                    else if (event.which === 38)
                        args[1]._grid.ChangeCell(0, -1);
                    else if (event.which === 40)
                        args[1]._grid.ChangeCell(0, 1);
                }
                //ENTER
                if (event.which === 13) {
                    if (filterFocused) {
                        $(":focus").blur();
                        $(selector).jsGrid("search").done(function () {
                            args[1]._grid.ChangeCell(0, 0);
                        });
                    }
                    else if (insertFocused) {
                        $(":focus").blur();
                        var insert = $(selector).jsGrid("insertItem");
                        insert.done(function () {
                            console.log("row inserted");
                        });
                    }
                    else if (args[1]._grid._editingRow != null) {
                        var updateJson = $(selector).jsGrid("updateItem");
                        updateJson.done(function () {
                            args[1]._grid.ChangeCell(0, 1);
                            args[1]._grid.editItem(args[1]._grid.data[args[1]._grid.selectedRowIndex]);
                            args[1]._grid.FocusOnInput();
                        });
                    }
                    else {
                        args[1]._grid.editItem(args[1]._grid.data[args[1]._grid.selectedRowIndex]);
                        args[1]._grid.FocusOnInput();
                    }
                    return true;
                }
                //F2
                if (event.which === 113) {
                    if (args[1]._grid._editingRow == null) {
                        args[1]._grid.editItem(args[1]._grid.data[args[1]._grid.selectedRowIndex]);
                    }
                    args[1]._grid.FocusOnInput();
                    return true;
                }
                //F3
                if (event.which === 114) {
                    args[1]._grid.FocusOnFilter();
                    return false;
                }
                //F4 (CTRL+N)
                if (event.which === 115) {
                    args[1]._grid.FocusOnInsert();
                    return false;
                }
                //F5
                if (ctrlDown && event.which === 116) {
                    console.log("ctrl+f5-refresh everything");
                    return true;
                }
                if (!ctrlDown && event.which === 116) {
                    $(selector).jsGrid("search").done(function () {
                        args[1]._grid.ChangeCell(0, 0);
                    });
                    return false;
                }
                //ESC
                if (event.which === 27) {
                    if (filterFocused) {
                        $(":focus").blur();
                        args[1]._grid.ChangeCell(0, 0);
                    }
                    else if (insertFocused) {
                        $(":focus").blur();
                        args[1]._grid.option("inserting", false);
                        args[1]._grid.option("filtering", true);
                        args[1]._grid.ChangeCell(0, 0);
                    } else {
                        $(selector).jsGrid("cancelEdit");
                        $(this.divSelector).focus();
                    }
                    return true;
                }
            }
        });
    }
};

//-------------------GRID-BULK-UPDATE--------------------------
var GridBulkUpdate = function(gridDivSelector) {
    //GridDefault.call(this, gridDivSelector);
    this.idsTable = [];
    this.bulkUpdateItem = false;
    this.divSelector = gridDivSelector;
    this.InitMultiCheckBoxColumn(gridDivSelector);

    GridDefault.prototype.KeysActivate.call(this);
}

GridBulkUpdate.prototype = Object.create(GridDefault.prototype);
GridBulkUpdate.prototype.constructor = GridBulkUpdate;
GridBulkUpdate.prototype = {
    SetBulkUpdate: function (bulkUpdateOption) {
        //console.log("this.divSelector");
        //console.log(this.divSelector);
        //var grid = $(this.divSelector).data("JSGrid");
        //grid.bulkUpdate = bulkUpdateOption;
        this.bulkUpdateItem = bulkUpdateOption;
    },
    SetIdsTable: function (idsOtpion) {
        //var grid = $(this.divSelector).data("JSGrid");
        //grid.idsTable = idsOtpion;
        this.idsTable = idsOtpion;
    },
    CreateNewGridInstance: function (divSelector) {
        return new GridBulkUpdate(divSelector);
    },
    RefreshGrid: function () {
        var grid = $(this.divSelector).data("JSGrid");

        if (grid.bulkUpdate == false) {
            $(this.divSelector).jsGrid("search");
        } else {
            grid.data.push({ Id: "" });
            grid.search();
        };
    },
    InitGrid: function () {
        $(this.divSelector).jsGrid({
            width: "100%", height: $(gridDivSelector).height(),
            fields: [
                { name: "Id", type: "text", title: "Id", width: 50, filtering: false, editing: false, inserting: false },
                { name: "Name", type: "text", title: "Nazwa", width: 200, filtering: false, editing: false, inserting: false },
                //ManageColumn(),
                { type: "control", width: 100, modeSwitchButton: true, editButton: true }
            ],
            controller: this.gridHelper.DB,
            onDataLoaded: function () { console.log("grid data loaded"); },
            rowClick: function (args) { },
        });
    },
    ManageColumn: function () {
        console.log("edit multi");
        if (this.bulkUpdateItem == false) {
            return { name: "checkBoxItem", type: "multiCheckBoxField", title: "Multi", width: 50, align: "center", filtering: false };
        }
        else {
            return {};
        }
    },
    InitMultiCheckBoxColumn(divSelector1) {
        var selfGrid = this;
        var MultiCheckBoxField = function (config) {
            jsGrid.Field.call(this, config);
        };
        var SelectAll = function () {
            var $items = $(divSelector1).jsGrid("option", "data");

            $items.forEach(function (entry) {
                $(entry)["0"].checkBoxItem = true;
                $(divSelector1).jsGrid("fieldOption", "checkBoxItem", "checked", true);
            });
        }
        var UnSelectAll = function () {
            var $items = $(divSelector1).jsGrid("option", "data");

            $items.forEach(function (entry) {
                $(entry)["0"].checkBoxItem = false;
                $(divSelector1).jsGrid("fieldOption", "checkBoxItem", "checked", false);
            });
        }
        var EditSelected = function () {
            var $items = $(divSelector1).jsGrid("option", "data");
            var selectedIds = [];

            $items.forEach(function (entry) {
                if ($($(divSelector1).jsGrid("rowByItem", entry)["0"]).find("[name='checkBoxItem']").is(":checked")) {
                    selectedIds.push(entry.Id);
                }
            });
            ShowWindowPackageItemGroup(selectedIds);
        }
        var ShowWindowPackageItemGroup = function (selectedIds) {
            console.log("klick");
            wnd = new PopupWindow(1900, 400);
            wnd.Init("windowItemGroupTL", "Grupowe zmiany wartości");
            wnd.Show("<div id='test'></div>");

            var gridPopup = selfGrid.CreateNewGridInstance("#test");
            gridPopup.SetBulkUpdate(true);
            gridPopup.SetIdsTable(selectedIds);
            gridPopup.InitGrid();
            gridPopup.RefreshGrid();
        }

        MultiCheckBoxField.prototype = new jsGrid.Field({
            sorter: "number",
            align: "center",
            autosearch: true,
            itemTemplate: function (value, item) {
                return this._createCheckbox().prop({
                    checked: value,
                    disabled: false
                });
            },
            headerTemplate: function () {
                var $selectButton = $("<button>").attr("type", "button").addClass("btnGridMultiCheckBox").text("Sel")
                    .on("click", function () { SelectAll(); })
                var $unselectButton = $("<button>").attr("type", "button").addClass("btnGridMultiCheckBox").text("Uns")
                    .on("click", function () { UnSelectAll(); });
                var $manageButton = $("<button>").attr("type", "button").addClass("btnGridMultiCheckBox").text("Manage").css("width", "72px")
                    .on("click", function () { EditSelected(); });
                return $("<tr>")
                    .append($selectButton)
                    .append($unselectButton)
                    .append($manageButton);
            },
            filterTemplate: function () {
                if (!this.filtering)
                    return "";

                var grid = this._grid,
                    $result = this.filterControl = this._createCheckbox();
                $result.prop({ readOnly: true, indeterminate: true });

                $result.on("click", function () {
                    var $cb = $(this);

                    if ($cb.prop("readOnly")) {
                        $cb.prop({ checked: false, readOnly: false });
                    }
                    else if (!$cb.prop("checked")) {
                        $cb.prop({ readOnly: true, indeterminate: true });
                    }
                });

                if (this.autosearch) {
                    $result.on("click", function () {
                        grid.search();
                    });
                }

                return $result;
            },
            insertTemplate: function () {
                if (!this.inserting)
                    return "";

                return this.insertControl = this._createCheckbox();
            },
            editTemplate: function (value) {
                if (!this.editing)
                    return this.itemTemplate.apply(this, arguments);

                var $result = this.editControl = this._createCheckbox();
                $result.prop("checked", value);
                return $result;
            },
            filterValue: function () {
                return this.filterControl.get(0).indeterminate
                    ? undefined
                    : this.filterControl.is(":checked");
            },
            insertValue: function () {
                return this.insertControl.is(":checked");
            },
            editValue: function () {
                return this.editControl.is(":checked");
            },
            _createCheckbox: function () {
                return $("<input>").attr("type", "checkbox").attr("name", "checkBoxItem");
            }
        });
        jsGrid.fields.multiCheckBoxField = MultiCheckBoxField;
    },
}

//--------------------------------------------------------------

