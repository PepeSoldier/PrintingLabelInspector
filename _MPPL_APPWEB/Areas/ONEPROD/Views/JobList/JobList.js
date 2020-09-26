
var img2 = function() {
    this.src;
    this.title;
    this.name;
}

var JobList = function (markNew = true) {
    var self = this;
    var orderAndCounterArray = [];
    var prevItems = [];
    var currentWorkstations = "";
    var threadClock = null;
    LoadTemplate();

    var p = false;
    this.StartClock = function () {
        threadClock = setInterval(function () {
            //console.log("thread");
            if (p == true) {
                //console.log("tick");
                var today = new moment(new Date());
                $("#clockHeaderMid").html(today.format("HH:mm:ss"));
            }
            //console.log("pulse");
            if (markNew == true) {
                $(".photoFrameWarning").toggleClass("photoFrameWarning2");
            }

            p = !p;
        }, 500);
    };
    this.StopClock = function () {
        console.log("threadClock----------------------------------------------------------");
        console.log(threadClock);
        //console.log("clearTimeout");
        window.clearInterval(threadClock);
        threadClock = null;
        console.log(threadClock);
    };
    this.GetItems = function () {
        var barcode = $("#barcode").val();
        var workstationIds = $("#workstationId").val();
        var lastWo = CheckWorkstationsNames();

        $("#jobListItems").html("");
        $("#orderDetails").html("");
        $("#joblistDataFound").css("display", "none");
        $(".photoFrameWarning").removeClass("photoFrameWarning");
        $(".photoFrameWarning2").removeClass("photoFrameWarning2");

        console.log("GetData");
        var ajax = AjaxPost("/ONEPROD/JobList/GetItems", { workstationIds, barcode });
        ajax.done(function (jsdata) {

            var jobListVM = jsdata.data;
            console.log(jobListVM);

            $("#jobListItems").html("");
            $("#orderDetails").html("");

            if (jobListVM.JobListDataFound == false || jsdata.error == true) {
                $("#joblistDataFound").css("display", "block");
                $("#workorderNo").html('<span style="color: red;">' + jsdata.message + '</span>');
            }

            let counterVal = RefreshWorkOrderCounter(orderAndCounterArray, jobListVM.WorkorderNo, jobListVM.WorkorderQtyPlanned);

            $("#workorderNo").text(jobListVM.WorkorderNo);
            $("#itemCode").text(jobListVM.ItemCode);
            $("#itemName").text(jobListVM.ItemName);
            $("#QtyPlanned").text(jobListVM.WorkorderQtyPlanned - counterVal);

            ClearPhotos(jobListVM);
            LoadAndPutPhotos(jobListVM);

            prevItems = jobListVM.JobListItems;
        });
    };
    this.GetOptionalPackData = function (prefix) {
        var barcode = $("#barcode").val();

        $("#orderDetails").html("");
        $("#joblistDataFound").css("display", "none");
        $(".photoFrameWarning").removeClass("photoFrameWarning");
        $(".photoFrameWarning2").removeClass("photoFrameWarning2");

        console.log("GetKitData");
        var ajax = AjaxPost("/ONEPROD/JobList/GetKitItems", { prefix, barcode });
        ajax.done(function (jsdata) {

            var jobListVM = jsdata.data;
            console.log(jobListVM);
            $("#orderDetails").html("");

            $("#itemCode" + prefix).text(jobListVM.ItemCode);

            $($('.code .dValue')).text("");
            $($('.qty .dValue')).text("");

            ClearPhotos(jobListVM);
            photoCount = jobListVM != null && jobListVM.JobListItems != null ? jobListVM.JobListItems.length : 0;

            for (var i = 0; i < photoCount; i++) {
                LoadPhotoOP(jobListVM, i);
            }

            prevItems = jobListVM.JobListItems;
        });
    };
    this.GetKitData = function (prefix) {
        var barcode = $("#barcode").val();
        
        $("#jobListItems").html("");
        $("#orderDetails").html("");
        $("#joblistDataFound").css("display", "none");
        $(".photoFrameWarning").removeClass("photoFrameWarning");
        $(".photoFrameWarning2").removeClass("photoFrameWarning2");

        console.log("GetKitData");
        var ajax = AjaxPost("/ONEPROD/JobList/GetKitItems", { prefix, barcode });
        ajax.done(function (jsdata) {

            var jobListVM = jsdata.data;
            console.log(jobListVM);

            $("#jobListItems").html("");
            $("#orderDetails").html("");
            
            $("#itemCode" + prefix).text(jobListVM.ItemCode);
            $("#workorderNo").text(jobListVM.WorkorderNo);

            photoCount = jobListVM != null && jobListVM.JobListItems != null ? jobListVM.JobListItems.length : 0;

            for (var i = 0; i < photoCount; i++) {
                PutItemOnTheList(jobListVM, i);
            }
            
            prevItems = jobListVM.JobListItems;
        });
    };
    this.GetItemsByPrefixes = function () {
        //prefixesToDisplay
        var barcode = $("#barcode").val();
        var workstationIds = $("#workstationId").val();
        var lastWo = CheckWorkstationsNames();

        $("#jobListItems").html("");
        $("#orderDetails").html("");
        $("#joblistDataFound").css("display", "none");
        $(".photoFrameWarning").removeClass("photoFrameWarning");
        $(".photoFrameWarning2").removeClass("photoFrameWarning2");

        console.log("GetData");
        var ajax = AjaxPost("/ONEPROD/JobList/GetItemsByPrefixes", { prefixes: prefixesToDisplay, barcode });
        ajax.done(function (jsdata) {

            var jobListVM = jsdata.data;
            console.log(jobListVM);

            if (prefixesToDisplay.length <= 0) {
                LoadPrefixesToDisplay();
            }

            $("#jobListItems").html("");
            $("#orderDetails").html("");

            if (jobListVM.JobListDataFound == false || jsdata.error == true) {
                $("#joblistDataFound").css("display", "block");
                $("#workorderNo").html('<span style="color: red;">' + jsdata.message + '</span>');
            }

            let counterVal = RefreshWorkOrderCounter(orderAndCounterArray, jobListVM.WorkorderNo, jobListVM.WorkorderQtyPlanned);

            $("#workorderNo").text(jobListVM.WorkorderNo);
            $("#itemCode").text(jobListVM.ItemCode);
            $("#itemName").text(jobListVM.ItemName);
            $("#QtyPlanned").text(jobListVM.WorkorderQtyPlanned - counterVal);

            ClearPhotos(jobListVM);
            LoadAndPutPhotos(jobListVM);

            prevItems = jobListVM.JobListItems;
        });
    };

    function RefreshWorkOrderCounter(orderNumbArray, workorderNo, qtyPlanned) {
        if (!isOrderNumberInArray(orderAndCounterArray, workorderNo)) {
            addWorkOrderWithEmptyCounter(orderAndCounterArray, workorderNo, qtyPlanned);
        }
        let counter = increaseCounterByWorkorderNo(orderAndCounterArray, workorderNo);
        window.localStorage.setItem("orderAndCounterArray", orderAndCounterArray);
        orderNumbArray = orderNumbArray.filter(x => x.counter < qtyPlanned);
        return counter;
    }
    function addWorkOrderWithEmptyCounter(orderNumbArray, workorderNo, qtyPlanned) {
        var itemArray = {
            "WorkOrder": workorderNo,
            "Counter": 0,
            "QrtPlanned": qtyPlanned
        };
        orderNumbArray.push(itemArray);
    }
    function increaseCounterByWorkorderNo(orderNumbArray, workorderNo) {
        var indexWithWorkOrder = orderAndCounterArray.map((o) => o.WorkOrder).indexOf(workorderNo);
        orderAndCounterArray[indexWithWorkOrder].Counter += 1;
        return orderAndCounterArray[indexWithWorkOrder].Counter ;
    }
    function isOrderNumberInArray(orderNumbArray, orderNumber) {
        return orderNumbArray.map((o) => o.WorkOrder).includes(orderNumber);
    }
    

    //file://10.26.10.90/c$/public/GC2G/117181721.JPG
    var photos = [];
    var photoCount = -1;
    var itemCodes;
    var prefixesToDisplay = [];

    function CheckWorkstationsNames() {
        var workstationIds = $("#workstationId").val();
        if (currentWorkstations != workstationIds) {
            var ajax = AjaxPost("/ONEPROD/JobList/GetSelectedWorkstationsName", { workstationIds })
            ajax.done(function (data) {
                $("#workstationsNames").text(data);
            });
            currentWorkstations = workstationIds;
        }
    }

    function ClearPhotos(data) {
        let photoDivs = $(".photo");
        let pItemCode = "";

        $(".photoCodeVal").text("");
        $(".photoQtyVal").text("");

        if (data != null && data.JobListItems != null) {
            itemCodes = data.JobListItems.map(a => a.ItemCode);

            for (var i = 0; i < photoDivs.length; i++) {
                pItemCode = $(photoDivs[i]).attr("data-itemCode");
                if (!(itemCodes.includes(pItemCode))) {
                    $(photoDivs[i]).attr("data-itemCode", "");
                    $(photoDivs[i]).css("background-image", "");
                }
            }
        }

        //usuwanie z tablicy photo, zdjęć kodów, które nie występują już na liście
        photos = photos.filter(function (item) {
            return itemCodes.includes(item.title);
        });

    }
    function LoadAndPutPhotos(jobListVM) {
        
        let photoCodes = photos.map(a => a.title);

        photoCount = jobListVM != null && jobListVM.JobListItems != null? jobListVM.JobListItems.length : 0;

        for (var i = 0; i < photoCount; i++) {
            PutItemOnTheList(jobListVM, i);

            //ładuj zdjęcie które nie znajduje sie w tablicy photoCodes (nowe zdjecia)
            if (!(photoCodes.includes(jobListVM.JobListItems[i].ItemCode)))
            {
                if (prefixesToDisplay.includes(jobListVM.JobListItems[i].Prefix))
                {
                    LoadPhoto(jobListVM, i);
                }
            }
            else {
                console.log(jobListVM.JobListItems[i].ItemCode + ".jpg already loaded.");
            }   
        }
    }
    function LoadPhoto(data, i) {
        //var imgSrc = 'http://10.26.10.90/photos/' + data.JobListItems[i].ItemCode + '.JPG';
        var imgSrc = '/photos/' + data.JobListItems[i].ItemCode + '.JPG';
        var photoClass = "";
        img = new img2(); //new Image();
        img.src = imgSrc;
        img.name = data.JobListItems[i].Prefix;
        img.title = data.JobListItems[i].ItemCode;
        img.qty = data.JobListItems[i].Qty;

        var newItem = CheckIfNewItem(data.JobListItems[i].ItemCode);
        var newItemClass = newItem ? "photoFrameWarning" : "";

        //photos.push(img);
        PutPhoto2(img, newItemClass);
    }
    function PutPhoto2(photo, newItemClass) {
                
        //let photoDiv = $("#prefix-" + photo.name); //name is prefix value
        let photoDiv = $('.photo[data-prefixes*="' + photo.name + '"]'); //name is prefix value

        if (photoDiv != null && photoDiv.length > 0) {
            let itemCodeInDiv = $(photoDiv).attr("data-itemCode");

            if (itemCodeInDiv != null && itemCodeInDiv.length < 1) {
                $(photoDiv).css('background-image', 'url(\'' + photo.src + '\')');
                $(photoDiv).attr("data-itemCode", photo.title);
                $(photoDiv).addClass(newItemClass);
            } else {
                console.log("Already on place: " + itemCodeInDiv);
            }
        }

        $(photoDiv).find(".photoCodeVal").text(Split3Chars(photo.title));
        //$(photoDiv).find(".photoCodeVal").attr("data-test", photo.title);
        $(photoDiv).find(".photoQtyVal").text(photo.qty);
    }

    function LoadPhotoOP(data, i) {
        //var imgSrc = 'http://10.26.10.90/photos/' + data.JobListItems[i].ItemCode + '.JPG';
        var imgSrc = '/photos/' + data.JobListItems[i].ItemCode + '.JPG';
        var photoClass = "";
        img = new img2(); //new Image();
        img.src = imgSrc;
        img.name = data.JobListItems[i].Qty;
        img.title = data.JobListItems[i].ItemCode;

        var newItem = CheckIfNewItem(data.JobListItems[i].ItemCode);
        var newItemClass = newItem ? "photoFrameWarning" : "";

        //photos.push(img);
        PutPhotoOP(img, newItemClass, i);
    }
    function PutPhotoOP(photo, newItemClass, i) {

        //let photoDiv = $("#prefix-" + photo.name); //name is prefix value
        let photoDiv = $('.photo')[i]; //name is prefix value
        $($('.code .dValue')[i]).text(photo.title);
        $($('.qty .dValue')[i]).text(photo.name);

        if (photoDiv != null) {
            $(photoDiv).css('background-image', 'url(\'' + photo.src + '\')');
            $(photoDiv).attr("data-itemCode", photo.title);
            $(photoDiv).addClass(newItemClass);
        }
    }

    function LoadTemplate() {
        var templateName = $("#templateName").val();
        var ajax = AjaxGet("/ONEPROD/JobList/LoadTemplate", { templateName });
        ajax.done(function (data) {
            $("#photos").html(data);
            LoadPrefixesToDisplay();
        });
    }
    function LoadPrefixesToDisplay() {
        prefixesToDisplay = [];
        $(".photo").each(function () {
            let prefixesStr = $(this).attr("data-prefixes");
            prefixesStr = prefixesStr.replace(' ', '');
            let prefixes = prefixesStr.split(",");
            prefixesToDisplay = prefixesToDisplay.concat(prefixes);
        });
    }
    function PutItemOnTheList(data, i) {
        var newItem = CheckIfNewItem(data.JobListItems[i].ItemCode);
        var newClass = newItem && markNew == true ? "newItem" : "";

        var el =
            '<div class="row no-gutters ' + newClass + '">' +
            '<div class="col-3 item itemCode">' +
            data.JobListItems[i].ItemCode +
            '</div>' +
            '<div class="col-7 item itemName">' +
            //data.JobListItems[i].Prefix + " - " + data.JobListItems[i].ItemName +
            data.JobListItems[i].ItemName +
            '</div>' +
            '<div class="col-2 item itemQty">' +
            data.JobListItems[i].Qty +
            '</div>' +
            '</div>';

        $("#jobListItems").append($(el));
    }
    function UrlExists(url) {
        //var http = new XMLHttpRequest();
        //http.open('HEAD', url, false);
        //http.send();
        //return http.status != 404;

        return $.get(url)
            .done(function () {
                return true;
            }).fail(function () {
                return false;
            })
    }
    function CheckIfNewItem(itemCode) {

        var x = prevItems != null? prevItems.filter(x => (x.ItemCode == itemCode)) : null;

        if (x == null || x.length == 0) {
            return true;
        }
        else {
            return false;
        }
    }
    function Split3Chars(code) {
        let splitted = "";
        let char = 0;
        while (code.length >= char) {
            if (char > 0) {
                splitted += " ";
            }
            splitted += code.substring(char, char + 3);
            char += 3;
        }

        //if (splitted[splitted.length - 1] == " ") { }
        return splitted;
    }
}