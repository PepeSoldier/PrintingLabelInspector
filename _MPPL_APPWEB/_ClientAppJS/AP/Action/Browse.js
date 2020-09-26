//--------------Classes for Browse View----------------

//class ActionBrowse
function ActionBrowse() {

    this.ShowHideChildrenActions = function (element) {
        var action = $(element).find('span').attr('id').split('_')[0];
        var id = $(element).find('span').attr('id').split('_')[1];
        var tr = $(element).parents('tr:first');

        if (action == "SubactionsShow") {
            ChangePlusMinusIcon(tr, id, 0);
            ShowChildrenActions(id, tr);
        }

        if (action == "SubactionsHide")
        {
            ChangePlusMinusIcon(tr, id, 1);
            HideChildrenActions(id, tr);
        }
    }
    
    function ShowChildrenActions(id, tr) {

        var trNew;

        $.ajax({
            url: new BaseUrl().link + "AP/Action/GetChildrenActions",
            type: "POST",
            dataType: "json",
            data: { id: id },
            success: function (actions) {
                actions.forEach(function (action) {
                    trNew = tr.clone();
                    trNew.insertAfter(tr);
                    fillData(trNew, action);
                });
            },
        });        
    }
    function HideChildrenActions(id, tr) {
        $(".SubAction_" + id).remove();
    }
    function ChangePlusMinusIcon(tr, id, showplus)
    {
        if (showplus == 0)
        {
            $(tr).find(".subactionsShow").html('<span class="fa fa-minus-square-o" id="SubactionsHide_' + id + '"></span>');
        }
        else
        {
            $(tr).find(".subactionsShow").html('<span class="fa fa-plus-square-o" id="SubactionsShow_' + id + '"></span>');
        }
    }
    function fillData(trNew, action) {
        $(trNew).addClass("SubAction_" + action.ParentActionId);
        $(trNew).find(".class1").attr("class", "class1 SubAction " + action.Variable1);
        $(trNew).find(".ActionCreator").text(action.Creator.FullName);
        $(trNew).find(".ActionDateCreated").text(moment(action.DateCreated).format('YYYY-MM-DD'));
        $(trNew).find(".ActionTitle").text(action.Title);
        $(trNew).find(".ActionType").text(action.Type.Name);
        $(trNew).find(".ActionArea").text(action.Area.Name);
        $(trNew).find(".ActionWorkstation").text(action.Workstation.Name);
        $(trNew).find(".ActionShiftCode").text(action.ShiftCode.Name);
        $(trNew).find(".actionCategory").text(action.Category.Name);
        $(trNew).find(".ActionAssigned").text((action.Assigned != null) ? action.Assigned.Name : 'N/A');
        $(trNew).find(".ActionPlannedEndDate").text(moment(action.PlannedEndDate).format('YYYY-MM-DD'));
        $(trNew).find(".ActionDepartment").text((action.Department != null) ? action.Department.Name : 'N/A');
        $(trNew).find(".subactionsCount").text(action.SubactionsCount);
        $(trNew).find(".statePicture").html('<img src="/Content/Images/state' + action.State + '.png"/>');
        $(trNew).find(".ActionLink").html($(trNew).find(".ActionLink").html().replace(action.ParentActionId, action.Id));

        if (action.SubactionsCount > 0)
        {
            $(trNew).find(".subactionsShow").html('<span class="fa fa-plus-square-o" id="SubactionsShow_' + action.Id + '"></span>');
        }
        else {
            $(trNew).find(".subactionsShow").html('');
        }
    }
}
//class GridBrowse
function GridBrowse(div) {
    var parentDiv = div
    var FilterData;
    var currentPage = 1;
    var rowsOnPage = 10;
    var states = "";

    this.InitLoad = function () {
        this.Refresh();
    }
    this.ChangePage = function (page) {
        currentPage = page;
        BrowseGrid(prepareFilters());
    }
    this.Refresh = function () {
        currentPage = 1;
        FilterData = readFiltersData();
        BrowseGrid(prepareFilters());
    }
    this.ApplyFilters = function () {
        FilterData = readFiltersData();
        BrowseGrid(prepareFilters());
    }

    function BrowseGrid(filters) {
        $.post(new BaseUrl().link + "AP/Action/BrowseGrid", filters, function (res) {
            $(parentDiv).html(res);
            RefreshStatusCount(res);
        });
    }
    function prepareFilters() {
        return FilterData +
            '&' + $.param({ CurrentPage: currentPage, RowsOnPage: rowsOnPage, States: states });
    }
    function readFiltersData() {
        var formData = $("#BrowseFilterForm").serialize();
        var formData2 = $("#BrowseFilter2Form").serialize();

        states = "";
        $(".StatusCheckBoxes input[type=checkbox]").each(function () {
            if ($(this).prop("checked") == false) {
                if (states != "") {
                    states = states + ",";
                }
                console.log("b");
                states = states + $(this).attr("id").split('_')[1];
            }
        });

        states = "[" + states + "]";

        return formData + '&' + formData2;
    }
    function RefreshStatusCount(res)
    {
        var counts = $(res).find("#statusesCount").text();
        var c = counts.split("!")[1];
        var count = c.split("-");
        var id = 0;

        for (var i = 0; i < count.length; i++) {
            if (count[i] >= 0) {
                $("#cbStatusVal_" + id).text(count[i]);
                id++;
            }
        }
    }
}