function ActionActivity() {
    var activitiesDiv = "#Activities";
    var baseUrl = new BaseUrl().link + 'AP/';
    var self = this;

    this.ShowAddForm = function (actionId, meetingId, divId) {
        $(divId).load(baseUrl + "ActionActivity/AddForm", { "id": actionId, "meetingId": meetingId });
    }
    this.LoadListAction = function (actionId) {
        $(activitiesDiv).load(baseUrl + "ActionActivity/ListAction", { "id": actionId });
    }    
    this.LoadListMeeting = function (actionId, meetingId) {
        console.log("listMeeting");
        $(activitiesDiv).load(baseUrl + "ActionActivity/ListMeeting", { "id": actionId, "meetingId": meetingId });
    } 
    this.Add = function (actionId, ActivityText, meetindId) {

        $.ajax({
            url: baseUrl + "ActionActivity/Add",
            type: "POST",
            dataType: "json",
            data: { "ActionId": actionId, "ActionActivity.ActivityDescription": ActivityText },
            success: function () {
                new Alert().GetAlerts(new User().name);
                if (meetindId > 0){
                    self.LoadListMeeting(actionId, meetindId)
                }
                else {
                    self.LoadListAction(actionId);
                }
            }
        });
    }
    this.Delete = function (activityId) {

        $.ajax({
            url: baseUrl + "ActionActivity/Delete",
            type: "POST",
            dataType: "json",
            data: { "id": activityId },
            success: function (actionId) {
                new Alert().GetAlerts(new User().name);
                self.LoadListAction(actionId);
            }
        });
    }
    this.DeletePhoto = function (attachmentId, photoFrameDiv) {
        $.ajax({
            url: baseUrl + "Attachment/Delete",
            type: "POST",
            dataType: "json",
            data: { "id": attachmentId },
            success: function () {
                new Alert().GetAlerts(new User().name);
                $('[attachmentId="' + attachmentId + '"]').remove();
                return false;
            }
        });
    }
}


$(document).on("click", "#AddActivityAction", function () {

    var text = $("#ActionActivity_ActivityDescription").val();
    var actionId = $("#ActionId").val();
    new ActionActivity().Add(actionId, text, 0);
    $("#ActionActivity_ActivityDescription").val("");
})
$(document).on("click", "#AddActivityMeeting", function () {

    var text = $("#ActionActivity_ActivityDescription").val();
    var actionId = $("#ActionId").val();
    var meetingId = $("#MeetingId").val();
    new ActionActivity().Add(actionId, text, meetingId);
    $("#ActionActivity_ActivityDescription").val("");
})
$(document).on("click", ".DeleteActivity", function () {

    if (!confirm("Czy na pewno chcesz usunąć tą aktywność?")) {
        e.preventDefault();
    } else {
        var activityId = $(this).attr("activityId");
        new ActionActivity().Delete(activityId);
    }
})
$(document).on("click", ".EditAttachments", function () {
    $(".deleteAtt").toggle();
})

$(document).on("click", ".EditAttachmentsAc", function () {
    var zm = $(this).attr("activityId");
    $(".deleteAttAc").each(function (index) {
        if (this.getAttribute("parentid") === zm) {
            $(this).toggle();
        }
    });
})



$(document).on("click", ".DeleteActivityPhoto", function (e) {
    e.stopPropagation();
    e.preventDefault();
    if (!confirm("Czy na pewno chcesz usunąć ten obiekt?")) {
        e.preventDefault();
    } else {
        var attachmentId = $(this)[0].getAttribute("attachmentid");
        new ActionActivity().DeletePhoto(attachmentId);
    }
})
