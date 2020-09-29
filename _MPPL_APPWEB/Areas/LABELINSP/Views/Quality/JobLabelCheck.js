
var ImageObject = function () {
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
        console.log("tekstsasa");
        var ajax = AjaxPost("/_LABELINSP_APPWEB/LABELINSP/QUALITY/PackingLabelGetData", { serialNumber });
        ajax.done(function (packingLabelViewModel) {

            $("#workorderNo").text(packingLabelViewModel.PackingLabel.OrderNo);
            $("#itemCode").text(packingLabelViewModel.PackingLabel.ItemCode);
            $("#itemName").text(packingLabelViewModel.PackingLabel.ItemName);
            
            ClearPhotos();
            LoadAndPutPhotos(serialNumber);
            console.log(packingLabelViewModel.PackingLabelTests);
            var uniqueTestNames = {};
            let view = {
                Test : [],
            }
            // Pobranie unikatowych nazw z tablicy testów
            uniqueTestNames = packingLabelViewModel.PackingLabelTests.map(item => item.TestName).filter((value, index, self) => self.indexOf(value) === index);

            for (let i = 0; i < uniqueTestNames.length; i++) {
                //Pobranie wszystkich 3 wynikow dla jednego testu
                let allTestsForUniqueTestName = packingLabelViewModel.PackingLabelTests.where(x => x.TestName == uniqueTestNames[i]);
                let viewModel = {};
                viewModel.TestName = allTestsForUniqueTestName[0].TestName;
                viewModel.ExpectedValue = allTestsForUniqueTestName[0].ActualValue;
                viewModel.FrontResult = allTestsForUniqueTestName.find(x => x.LabelType == 0).Result;
                viewModel.FrontExpectedValue = allTestsForUniqueTestName.find(x => x.LabelType == 0).ActualValue;
                viewModel.SideResult = allTestsForUniqueTestName.find(x => x.LabelType == 1).Result;
                viewModel.SideExpectedValue = allTestsForUniqueTestName.find(x => x.LabelType == 1).ActualValue;
                viewModel.RearResult = allTestsForUniqueTestName.find(x => x.LabelType == 2).Result;
                viewModel.RearExpectedValue = allTestsForUniqueTestName.find(x => x.LabelType == 2).ActualValue;
                view.Test.push(viewModel);
            }

            console.log("Tutaj tablica");
            console.log(view.Test);

            RenderTemplate("#testPackingLabelTemplate", "#testView", view);
        });
    }


    function ClearPhotos() {
        $("#frontPhoto").html("");
        $("#sidePhoto").html("");
        $("#rearPhoto").html("");
    }

    function LoadAndPutPhotos(serialNumber) {
        let imageFront = '/Uploads/' + serialNumber + '_F' + '.JPG';
        let imageSide = '/Uploads/' + serialNumber + '_S' + '.JPG';
        let imageRear = '/Uploads/' + serialNumber + '_R' + '.JPG';

        PutPhoto(imageFront, "#frontPhoto");
        PutPhoto(imageSide, "#sidePhoto");
        PutPhoto(imageRear, "#rearPhoto");
    }

    function PutPhoto(photo, selector) {
        var img = new Image();
        img.src = window.location.origin + "/_LABELINSP_APPWEB" + photo;          //'url(\'' + '/_MPPL_APPWEB' + photo + '\')';
        img.className = "img-fluid";
        $(selector).append(img);
    }
}