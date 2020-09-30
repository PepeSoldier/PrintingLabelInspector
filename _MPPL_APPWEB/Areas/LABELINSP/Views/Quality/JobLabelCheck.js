
var ImageObject = function () {
    this.src;
    this.title;
    this.name;
};

var JobLabelCheck = function (markNew = true) {
    var threadClock = null;

    
    this.StartClock = function () {
        threadClock = setInterval(function () {
            var dateTimeNow = new moment(new Date());
            $("#clockHeaderMid").html(dateTimeNow.format("HH:mm:ss"));
        }, 1000);
    };
    this.StopClock = function () {
        console.log("threadClock----------------------------------------------------------");
        console.log(threadClock);
        //console.log("clearTimeout");
        window.clearInterval(threadClock);
        threadClock = null;
        console.log(threadClock);
    };

    this.LoadDataBySerialNumber = function (serialNumber, workstationIds) {
        console.log("tekstsasa");
        var ajax = AjaxPost("/LABELINSP/QUALITY/PackingLabelGetData", { serialNumber, workstationIds});
        ajax.done(function (packingLabelViewModel) {           
            console.log(packingLabelViewModel.PackingLabelTests);
            var uniqueTestNames = {};
            let view = {
                Test: [],
                WorkorderNo: packingLabelViewModel.PackingLabel.OrderNo,
                ItemCode: packingLabelViewModel.PackingLabel.ItemCode,
                ItemName: packingLabelViewModel.PackingLabel.ItemName
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

            RenderTemplate("#testPackingLabelTemplate", "#joblistColumns", view);
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
        let imageFront = '/Uploads/' + serialNumber + '_F' + '.JPG';
        let imageSide = '/Uploads/' + serialNumber + '_S' + '.JPG';
        let imageRear = '/Uploads/' + serialNumber + '_R' + '.JPG';

        PutPhoto(imageFront, "#frontPhoto");
        PutPhoto(imageSide, "#sidePhoto");
        PutPhoto(imageRear, "#rearPhoto");
    }

    function PutPhoto(photo, selector) {
        var img = new Image();
        img.src = window.location.origin + photo;          //'url(\'' + '/_MPPL_APPWEB' + photo + '\')';
        img.className = "img-fluid";
        $(selector).append(img);
    }
}