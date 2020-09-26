var recentSelectedReasonsStoppage = [];
var recentSelectedReasonsProduction = [];

function OEEReasonSelector2(containerId, resourceId, callback, splitAllowed) {

    var reasons = GetReasons(resourceId);
    var self = this;
    var containerSelector = containerId;
    var containerSelectorBack = "#Reasons0";
    var _resourceId = resourceId;
    var _entryId = 0;
    var _entryType = 0;

    $(containerSelector + ", #RecentSelectedReasons").on("click", ".reason1", function () {
        console.log("reason1 click");
        $("#parentReasonNameText").text($(this).text());
        self.ShowReasons(parseInt($(this).attr("data-reasonId")), _entryId);
    });
    $(containerSelectorBack).on("click", ".backBtn", function () {
        console.log("reason2 click");
        $("#parentReasonNameText").text($(this).text());
        self.ShowReasons(parseInt($(this).attr("data-reasonId")), _entryId);
    });
    $(containerSelector).on("click", ".selectReason", function () {
        var id = parseInt($(this).attr("data-reasonId"));
        self.SelectReason(id);
    });

    this.ShowReasons = function (parentId, entryId) {
        console.log("OEEReasonSelector2.ShowReasons");
        _entryId = entryId;
        var content = "";
        var back = 0;
        var subReasons = 0;
        $(containerSelector).html("");

        for (i = 0; i < reasons.length; i++) {
            content = "";
            if (reasons[i].ParentId === parentId) {
                content = DrawReason(reasons[i]);
                $(containerSelector).append(content);
                subReasons++;
            }
            if (reasons[i].Id === parentId) {
                back = reasons[i].ParentId;
            }
        }

        if (!(subReasons > 0)) {
            self.SelectReason(parentId);
        }
        else {
            console.log(parentId);

            //$("#backBtn").html("");
            if (Math.abs(parentId) > 0 && Math.abs(parentId) != 1) {
                $("#backBtn").removeClass("disabled");
                $("#backBtn").addClass("backBtn");
                //$("#backBtn").attr("id", 'reason_' + back);
                $("#backBtn").attr("data-reasonId", back);
            }
            else {
                $("#backBtn").addClass("disabled");
                $("#backBtn").removeClass("backBtn");
               // $("#backBtn").attr("id", null);
                $("#backBtn").attr("data-reasonId", null);
            }

            content = ShowRecentSelectedReasons(parentId, content);
            
        }
    };
    this.SelectReason = function (reasonId) {
        //$('#overlay, #overlay-back').fadeOut();
        var selectedReason = reasons.find(x => x.Id === reasonId);        
        SaveRecentSelectedReasons(selectedReason);

        console.log("selectedReason");
        console.log(selectedReason);

        callback(_entryId, selectedReason.ReasonTypeId, selectedReason.Id);
    };

    function DrawReason(reason) {
        let reasonDiv = "";
        reasonDiv += '<div class="resonbox">';
        reasonDiv += '<div class="reason1" id="reason_' + reason.Id + '" data-reasonId="' + reason.Id + '" style="border-top: 5px solid ' + reason.ColorGroup + ';">' + reason.Name + '</div>';

        if (reason.SubreasonsCount == 0) {
            reasonDiv += '<div class="selectReason" id="selectReson_' + reason.Id + '" data-reasonId="' + reason.Id + '">wybierz</div>';
        }
        else {
            reasonDiv += '<div class="showSubReasons"><div class="showSubReasonsText">wejdz</div><i class="fas fa-level-up-alt fa-rotate-90"></i></div>';
        }

        reasonDiv += '</div>';
        return reasonDiv;
    }

    function SaveRecentSelectedReasons(selectedReason) {
        let count = 3;
        //let recentSelectedReasons = Math.abs(selectedReason.ParentId) == 1 || Math.abs(selectedReason.ParentId) >= 20  ? recentSelectedReasonsStoppage : recentSelectedReasonsProduction;


        if (Math.abs(selectedReason.ParentId) == 1 || Math.abs(selectedReason.ParentId) >= 20) {
            if (Math.abs(selectedReason.ParentId) > 0) {
                let t = recentSelectedReasonsStoppage.filter(x => x.Id == selectedReason.Id);

                if (t.length <= 0) {
                    recentSelectedReasonsStoppage.push(selectedReason);
                }

                if (recentSelectedReasonsStoppage.length > count) {
                    recentSelectedReasonsStoppage = recentSelectedReasonsStoppage.slice(recentSelectedReasonsStoppage.length - count, recentSelectedReasonsStoppage.length);
                }
            }
        }
        else {
            if (Math.abs(selectedReason.ParentId) > 0) {
                let t = recentSelectedReasonsProduction.filter(x => x.Id == selectedReason.Id);

                if (t.length <= 0) {
                    recentSelectedReasonsProduction.push(selectedReason);
                }

                if (recentSelectedReasonsProduction.length > count) {
                    recentSelectedReasonsProduction = recentSelectedReasonsProduction.slice(recentSelectedReasonsProduction.length - count, recentSelectedReasonsProduction.length);
                }
            }
        }

        
    }
    function ShowRecentSelectedReasons(parentId, content) {
        let drawnCount = 0;
        let recentSelectedReasons = Math.abs(parentId) == 1 || Math.abs(parentId) >= 20 ? recentSelectedReasonsStoppage : recentSelectedReasonsProduction;

        //$("#RecentSelectedReasons").addClass("hidden");
        $("#RecentSelectedReasons").html("");

        if (Math.abs(parentId) == 1 || Math.abs(parentId) == 0) //1 - stoppages, 0 - production/scrap
        {
            console.log("ShowRecentSelectedReasons");
            $("#RecentSelectedReasons").removeClass("hidden");
            $("#RecentSelectedReasons").html("");

            for (i = recentSelectedReasons.length - 1; i >= 0; i--) {
                let reason = recentSelectedReasons[i];
                let parentReason = reasons.filter(x => x.Id == reason.ParentId);
                let parentReasonName = parentReason.length > 0 ? parentReason[0].Name : "-";
                content = "";
                content += '<div class="resonbox">';
                content += '<div class="reason1" id="reason_' + reason.Id + '" data-reasonId="' + reason.Id + '">';
                content += '<div class="reasonParentName">' + parentReasonName + '</div>';
                content += '<div class="reasonName">' + reason.Name + '</div>';
                content += '</div>';
                content += '</div>';
                $("#RecentSelectedReasons").append(content);
                drawnCount++;
            }
            //if (recentSelectedReasons.length > 0) {
            //    $("#RecentSelectedReasons").removeClass("hidden");
            //}
        }
        else {
            $("#RecentSelectedReasons").addClass("hidden");
        }

        for (i = drawnCount; i < 3; i++) {
            content = "";
            content += '<div class="resonbox">';
            content += '<div class="" id="reason_">';
            content += '<div class=""></div>';
            content += '<div class=""></div>';
            content += '</div>';
            content += '</div>';
            $("#RecentSelectedReasons").append(content);
        }

        //split
        if (Math.abs(parentId) == 1) {
            content = ShowSplitButton(content);
        }
        else if (Math.abs(parentId) == 0) {
            content = ShowChangeDeclarationDateButton(content);
        }
        else {
            content = ShowEmptyButton(content);
        }

        return content;
    }

    function ShowEmptyButton(content) {
        content = "";
        content += '<div class="resonbox" style="width: 385px;background-color: #366c803b;">';
        content += '<div class="row no-gutters" id="reason_" style="height: 100%;">';
        content += '<div class="col-8"></div>';
        content += '<div class="col-4"></div>';
        content += '</div>';
        content += '</div>';
        $("#RecentSelectedReasons").append(content);
        return content;
    }
    function ShowSplitButton(content) {
        let btnClass = "cutStoppageDisabled";
        let btnId = "btnSplitDisabled";

        if (splitAllowed == true) {
            btnClass = "cutStoppage";
            btnId = "btnSplit";
        }

        content = "";
        content += '<div class="resonbox ' + btnClass + '" style="width: 385px;">';
        content += '<div class="row no-gutters" id="' + btnId + '" style="height: 100%;" data-id="' + _entryId + '">';
        content += '<div class="col-8" style="font-size: 40px;line-height: 100px;">Podziel</div>';
        content += '<div class="col-4"><div style="height:100%;width:100%;background:#366c803b;font-size: 60px;"><span class="fas fa-cut"></span></div></div>';
        content += '</div>';
        content += '</div>';

        $("#RecentSelectedReasons").append(content);
        

        return content;
    }
    function ShowChangeDeclarationDateButton(content) {
        content = "";
        content += '<div class="resonbox changeDeclarationDate" style="width: 385px;">';
        content += '<div class="row no-gutters" id="btnChangeDeclarationDate" style="height: 100%;" data-id="' + _entryId + '">';
        content += '<div class="col-8" style="font-size: 40px;line-height: 100px;">Zmień Datę</div>';
        content += '<div class="col-4"><div style="height:100%;width:100%;background:#366c803b;font-size: 60px;"><span class="fas fa-calendar-day"></span></div></div>';
        content += '</div>';
        content += '</div>';
        $("#RecentSelectedReasons").append(content);
        return content;
    }

    function GetReasons(resourceId) {
        var resosns1;
        $.ajax({
            async: false,
            global: false,
            url: "/ONEPROD/OEEReportOnline/GetReasons?resourceId=" + resourceId,
            dataType: "json",
            type: "GET",
            success: function (ReasonSelectorItemList) {
                resosns1 = ReasonSelectorItemList;
            }
        });
        return resosns1;
    }
}

