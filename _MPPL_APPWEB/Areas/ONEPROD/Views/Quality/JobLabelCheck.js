
var img2 = function() {
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
}