function PickingListShared() {
    var parametersH_Array = [-1, 0, 1];
    this.urlPickingList = "/iLOGIS/PickingList/PickingList";
    this.urlPickingListGetList = "/iLOGIS/PickingList/GetList";
    this.urlPickingListCreate = "/iLOGIS/PickingList/Create";
    this.urlPickingListCreateMany = "/iLOGIS/PickingList/CreateMany";
    this.urlGetPickingListsForAllPickers = "/iLOGIS/PickingList/GetPickingListsForAllPickers";
    this.urlPickingListSetStatus = "/iLOGIS/PickingList/SetStatus";

    this.urlGetPickers = "/iLOGIS/PickingList/GetPickers";
    this.urlPickingListItem = "/iLOGIS/PickingListItem/PickingListItem";
    this.urlPickingListItemGetList = "/iLOGIS/PickingListItem/GetList";
    this.urlPickingListItemManage = "/iLOGIS/PickingListItem/Manage";
    this.urlPickingListItemManageGetData = "/iLOGIS/PickingListItem/ManageGetData";
    this.urlPickingListItemManageSave = "/iLOGIS/PickingListItem/ManageSave";

    this.urlPickingListPlatformLocationUpdate = "/iLOGIS/PickingListItem/PickingListPlatformLocationUpdate";

    this.urlPickingListSummary = "/iLOGIS/PickingListItem/Summary";
    this.urlPickingListLineFeed = "/iLOGIS/LineFeedList/LineFeed";

    this.GetStateIcon = function (state) {
        let stateIcon = "";

        if (state == 10 || state == 15) {
            stateIcon = "fas fa-play-circle";
        }
        else if (state == 20) {
            stateIcon = "far fa-play-circle";
        }
        else if (state == 30) {
            stateIcon = "fas fa-dolly";
        }
        else if (state == 40) {
            stateIcon = "fas fa-exclamation";
        }
        else if (state == 50) {
            stateIcon = "fas fa-flag-checkered ";
        }
        else if (state == 60) {
            stateIcon = "fas fa-exclamation";
        }
        
        return stateIcon;
    };

    this.GetStateIconLineFeed = function (state) {
        let stateIcon = "";

        if (state == 0 || state == 20) {
            stateIcon = "fas fa-play-circle";
        }
        else if (state == 30) {
            stateIcon = "fas fa-shipping-fast";
        }
        else if (state == 40) {
            stateIcon = "fas fa-exclamation";
        }
        else if (state == 50) {
            stateIcon = "fas fa-flag-checkered ";
        }
        else if (state == 60) {
            stateIcon = "fas fa-exclamation";
        }

        return stateIcon;
    };

    this.FormatWarehouseLocationName = function (item) {
        //if (item != "BRAK") {
        //    return (item.slice(0, 2) + "-" +
        //        item.slice(2, 4) + "-" +
        //        item.slice(4, 6) + "-" +
        //        item.slice(6, 8));
        //} else {
        //    return item;
        //}
        return item;
    };

    this.ValidatePlatform = function (platformLocationName) {
        platfPrefix = platformLocationName.slice(0, 1);
        platDigits = platformLocationName.slice(1);
        if (platformLocationName == "") {
            bootbox.alert("Uzupełnij pole Miejsce Platformy", function () {
                $("#PlatformPosition_").addClass("required");
            });
            $('.bootbox').on('hidden.bs.modal', function () {
                $("#PlatformPosition_").focus();
            });

            return false;
        }
        else if (platfPrefix != "L" || isNaN(parseInt(platDigits)) || platformLocationName.length >= 5) {
            bootbox.alert("Żle opisana lokacja Platformy (L...)", function () {
                $("#PlatformPosition_").addClass("required");
            });
            $('.bootbox').on('hidden.bs.modal', function () {
                $("#PlatformPosition_").focus();
            });  
            return false;
        }

        return true;
    };

    this.SetHValue = function(lastValParameterH, direction) {
        let arrayLength = parametersH_Array.length;
        let arrayIndex = parametersH_Array.indexOf(lastValParameterH);

        arrayIndex += direction;
        arrayIndex = Math.max(0, arrayIndex);
        arrayIndex = Math.min(arrayLength - 1, arrayIndex);

        return parametersH_Array[arrayIndex];
    };

}