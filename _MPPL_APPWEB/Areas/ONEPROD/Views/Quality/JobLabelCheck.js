
var ImageObject = function() {
    this.src;
    this.title;
    this.name;
}

var JobLabelCheck = function (markNew = true) {
    var threadClock = null;

    var p = false;
    this.StartClock = function () {
        threadClock = setInterval(function () {
            if (p == true) {
                var today = new moment(new Date());
                $("#clockHeaderMid").html(today.format("HH:mm:ss"));
            }
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

    this.LoadDataBySerialNumber = function (serialNumber) {
        var ajax = AjaxPost("/_MPPL_APPWEB/ONEPROD/Quality/PackingLabelGetData", { serialNumber });
        ajax.done(function (packingLabelViewModel) {
            
            //$("#jobListItems").html("");
            //$("#orderDetails").html("");

            //if (jobListVM.JobListDataFound == false || jsdata.error == true) {
            //    $("#joblistDataFound").css("display", "block");
            //    $("#workorderNo").html('<span style="color: red;">' + jsdata.message + '</span>');
            //}

            //let counterVal = RefreshWorkOrderCounter(orderAndCounterArray, jobListVM.WorkorderNo, jobListVM.WorkorderQtyPlanned);

            $("#workorderNo").text(packingLabelViewModel.PackingLabel.OrderNo);
            $("#itemCode").text(packingLabelViewModel.PackingLabel.ItemCode);
            $("#itemName").text(packingLabelViewModel.PackingLabel.ItemName);
            
            ClearPhotos();
            LoadAndPutPhotos(serialNumber);
        });
    }
 

    function ClearPhotos() {
        $("#frontPhoto").html("");
        $("#sidePhoto").html("");
        $("#rearPhoto").html("");
    }

    function LoadAndPutPhotos(serialNumber) {
        let imageFront = '/photos/' + serialNumber + '_F' + '.JPG';
        let imageSide = '/photos/' + serialNumber + '_S' + '.JPG';
        let imageRear = '/photos/' + serialNumber + '_R' + '.JPG';

        PutPhoto(imageFront,"#frontPhoto");
        PutPhoto(imageSide,"#sidePhoto");
        PutPhoto(imageRear,"#rearPhoto");
    }

    function PutPhoto(photo, selector) {
            var img = new Image();
            img.src = window.location.origin + "/_MPPL_APPWEB" + photo;          //'url(\'' + '/_MPPL_APPWEB' + photo + '\')';
            img.className = "img-fluid";
            $(selector).append(img);
    }  
}