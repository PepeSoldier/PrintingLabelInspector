function ActionShow()
{
    var childActionsDiv = "#ChildActions";
    var baseUrl = new BaseUrl().link + 'AP/';
    var self = this;

    this.ShowChildrenActions = function (actionId) {
        $(childActionsDiv).load(baseUrl + "Action/ShowChildrenActions", { "id": actionId });
    }
    this.CreateChild = function () {
        var url = window.location.pathname;
        var id = url.substring(url.lastIndexOf('/') + 1);
        $.ajax({
            url: baseUrl + "Action/CreateChild",
            type: "POST",
            dataType: "json",
            data: { "ParentId": id },
            success: function (ActionId) {
                new Alert().GetAlerts(new User().name);
                self.ShowChildrenActions(ActionId);
            }
        });
    }
    this.ChangeAssigned = function (actionId, assignedId) {
       
        $.ajax({
            type: "POST",
            url: baseUrl + "Action/ChangeAssigned",
            data: { "AssignedId": assignedId, "ActionId": actionId },
            success: function (actId) {
                new Alert().GetAlerts(new User().name);
                self.LoadActivities(actId);
            }
        });
    }
    this.ChangeStatus = function (actionId, state) {
        
        $.ajax({
            type: "POST",
            url: baseUrl + "Action/ChangeStatus",
            data: { "ActionStateEnum": state, "ActionId": actionId },
            success: function (actId) {
                new Alert().GetAlerts(new User().name);
                self.LoadActivities(actId);
            }
        });
    }
}


$(document).on("change", "#AssignedId", function () {
    var assignedId = $("#AssignedId option:selected").val();
    var actionId = $("#ActionId").val();
    new ActionShow().ChangeAssigned(actionId, assignedId);

});
$(document).on("change", "#ActionStateEnum", function () {
    var state = $("#ActionStateEnum option:selected").val();
    var actionId = $("#ActionId").val();
    new ActionShow().ChangeStatus(actionId, state);
});
$(document).on("click", "#CreateChild", function () {

    if (!confirm("Czy na pewno dodać podakcję do tej akcji?")) {
        e.preventDefault();
    } else {
        new ActionShow().CreateChild();
    }
})
$(document).on("click", "#AddAttachmentLink", function () {

    var input = $(document.createElement("input"));
    input.attr("type", "file");
    // add onchange handler if you wish to get the file :)
    input.trigger("click"); // opening dialog
    //return false; // avoiding navigation

    input.on("change", function () {
        alert(this.files[0].name);
    });

});