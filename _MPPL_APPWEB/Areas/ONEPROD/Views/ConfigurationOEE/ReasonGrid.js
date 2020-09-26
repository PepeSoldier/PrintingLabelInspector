function ReasonGrid(gridDivSelector, machines, machineReasons) {

    var self = this;
    var divSelector = gridDivSelector;
    var gridHelper = new GridHelper("Reason", "/ONEPROD/ConfigurationOEE");
    //var clients = gridHelper.GetList(false, null).responseJSON;
    var gridHelperReasonTypes = new GridHelper("ReasonTypes", "/ONEPROD/ConfigurationOEE");
    var gridHelperReasonGroups = new GridHelper("ReasonGroups", "/ONEPROD/ConfigurationOEE");
    var types = [{ "Id": -1, "Name": "" }];
    var groups = [{ "Id": -1, "Name": "" }];
    types = types.concat(gridHelperReasonTypes.GetList(false, {}).responseJSON);
    groups = groups.concat(gridHelperReasonGroups.GetList(false, {}).responseJSON);
    
    this.InitGrid = function () {
        
        $(divSelector).jsGrid({
            width: "100%",
            height: "860px",
            inserting: true,
            editing: true,
            sorting: false,
            paging: false,
            filtering: true,
            fields: PrepareFields(), //numer of columns depends on numbers of machines
            controller: gridHelper.DB,
            onDataLoading: function (args) {
                //gridHelper.SetFilter({ machineId });
            },
            onItemEditing: function (args) {
                console.log("startEdycji");
            },
            onDataLoaded: function () {
                console.log("grid data loaded");
                $("input:checkbox").prop('checked', false);
                CheckConnections($(divSelector));  
            },
            rowClick: function (args) { },
            onItemUpdated: function (args) {
                console.log(args);
                CheckConnections($(args.row[0]));
            }
        });

        $(document).on("click", ".cellMachine", function () {
            var machineId = parseInt($(this).find(".cellMachineDiv").attr('data'));
            var reasonId = parseInt($($(this).closest('tr')[0].cells[0]).text());
            if (machineId > 0 && reasonId > 0) {
                ReasonMachineConnect(reasonId, machineId, $(this).find(".cellMachineDiv"));
            }
        });
    };
    this.RefreshGrid = function () {
        $(divSelector).jsGrid("search");
    };

    function PrepareFields() {
        var fields = [];

        fields.push({ name: "Id", type: "text", title: "Id", width: 5, editing: false, inserting: false, css: "hidden", filtering: false });
        fields.push({ name: "Name", type: "text", title: "Nazwa", width: 150 });
        fields.push({ name: "NameEnglish", type: "text", title: "Nazwa (Angielski)", width: 150 });
        fields.push({ name: "ReasonTypeId", title: "TYP WPISU", type: "select", items: types, valueField: "Id", textField: "Name", width: 140 });
        fields.push({
            name: "IsGroup", title: "Grupa", type: "checkbox", width: 30, itemTemplate: function (value, item) {
                var cell = "";
                var checked = value == true ? "checked" : "";
                var activeInactive = value == true ? "active" : "inactive";
                return "<div class='cellGroup " + activeInactive + "'><input type='checkbox' disabled " + checked + "></div>";
            } });
        fields.push({ name: "GroupId", title: "Grupa", type: "select", items: groups, valueField: "Id", textField: "Name", width: 140 });
        fields.push(gridHelper.ColumColor(40, "Color", "Kolor"));
        fields.push(gridHelper.ColumColor(40, "ColorGroup", "Kolor Grupy"));
        
        for (i = 0; i < machines.length; i++){
            fields.push({
                name: "Machine" + machines[i].Value, type: "checkbox", title: machines[i].Text, width: 25, css: "cellMachine", headercss: "", filtering: true,
                headerTemplate: "<div class='rotatedHeader'>" + machines[i].Text + "</div>",
                editValue: GetValue(), editTemplate: Templt(),
                insertValue: GetValue(), insertTemplate: Templt(),
                filterValue: GetValue(), filterTemplate: TempltFilter(machines[i].Value),
                itemTemplate: "<div class='cellMachineDiv' data='" + machines[i].Value + "'></div>"
            });
        }

        fields.push({ name: "Akcje2", type: "control", width: 45, modeSwitchButton: true, editButton: true });

        return fields;
    }
    function CheckConnections($startingElement) {
        console.log("ReasonGrid.CheckConnections");
        $(".cellMachineDiv").removeClass("OK");
        $startingElement.find(".cellMachine").each(function ()
        {
            var machineId = $(this).find(".cellMachineDiv").attr('data');
            var reasonId = $($(this).closest('tr')[0].cells[0]).text();
            var x = machineReasons.find(x => x.MachineId == machineId && x.ReasonId == reasonId);

            if (x != null) {
                $(this).find(".cellMachineDiv").addClass("OK");
            }
        });
    }
    function ReasonMachineConnect(reasonId, machineId, $cell) {
        $.ajax({
            async: true, type: "POST", data: { reasonId, machineId },
            url: "/ONEPROD/ConfigurationOEE/ReasonMachineConnect",
            success: function (data) {
                if (data.Id > 0) {
                    $cell.addClass("OK");
                    machineReasons.push({ Id: data.Id, MachineId: machineId, ReasonId: reasonId, Deleted: false });
                }
                else {
                    $cell.removeClass("OK");
                    var x = machineReasons.find(x => x.MachineId == machineId && x.ReasonId == reasonId);
                    machineReasons = machineReasons.filter(function (value, index, arr) {
                        return value != x;
                    });
                }
            },
            error: function (xhr, ajaxOptions, thrownError) {
                $cell.toggleClass("ERROR");
            }
        });
    }
    function SaveSelectedColor(hsb, hex, rgb, el) {
        console.log("wybrano color");
        console.log(hex);
        
        $(el).children().first().css('backgroundColor', '#' + hex);
        $(el).ColorPickerHide();

        var el2 = $(el).children().first();
        //var input = el2.find('[id="Color"]');
        var columnName = el2.attr("id");
        var itemId = el2.attr("itemId");
        var color = "#" + hex;

        console.log("ItemId: " + itemId + ", " + columnName + ": " + color);

        //$.ajax({
        //    url: BaseUrl + "/ONEPROD/Configuration/ItemGroupUpdateColor",
        //    data: '{ Id: ' + itemId + ', ' + columnName + ': "' + color + '"}',
        //    type: 'POST',
        //    contentType: 'application/json; charset=utf-8',
        //    success: function (data) {

        //    }
        //});

    }
        
    function Templt() { return $("<td>"); }
    function TempltFilter(id) { return '<input type="checkbox" class="resourceFilter" data-resourceId="' + id + '">'; }
    function GetValue() { return function () { return 0; }; }

    $(document).on("click", ".resourceFilter", function () {
        var resourceId = $(this).attr("data-resourceId");
        var checked = this.checked;
        if (!this.checked) {
            console.log("odfiltruj " + resourceId);
            $("#reasonsGrid .jsgrid-grid-body tr").removeClass("hidden");
        }
        else {
            console.log("filtruj " + resourceId);
            $("#reasonsGrid .jsgrid-grid-body tr").addClass("hidden");
            $(".cellMachineDiv.OK[data=" + resourceId + "]").closest("tr").removeClass("hidden");
        }
        $("input:checkbox").prop('checked', false);
        $(this).prop('checked', checked);
    });
    
}
