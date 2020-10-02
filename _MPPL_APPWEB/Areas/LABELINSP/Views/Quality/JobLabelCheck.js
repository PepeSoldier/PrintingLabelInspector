
var JobLabelCheck = function (markNew = true) {
    var threadClock = null;
    

    this.StartClock = function () {
        threadClock = setInterval(function () {
            var dateTimeNow = new moment(new Date());
            $("#clockHeaderMid").html(dateTimeNow.format("HH:mm:ss"));
        }, 1000);
    };

    this.StopClock = function () {
        window.clearInterval(threadClock);
        threadClock = null;
        console.log(threadClock);
    };

    this.LoadDataBySerialNumber = function (serialNumber) {
        var ajax = AjaxPost("/LABELINSP/QUALITY/PackingLabelGetData", { serialNumber });
        var uniqueTestNames = {};
        var view = {
            isChecking: true,
            Test: [],
            WorkorderNo: "",
            ItemCode: "",
            ItemName: ""
        };
        ajax.done(function (packingLabelViewModel) {           
            console.log(packingLabelViewModel);
            view.WorkorderNo = packingLabelViewModel.PackingLabel.OrderNo;
            view.ItemCode = packingLabelViewModel.PackingLabel.ItemCode;
            view.ItemName = packingLabelViewModel.PackingLabel.ItemName;
            if (packingLabelViewModel.PackingLabelTests.length != 0) {
                view.isChecking = true;
                uniqueTestNames = {};
                
                // Pobranie unikatowych nazw z tablicy testów
                uniqueTestNames = packingLabelViewModel.PackingLabelTests.map(item => item.TestName).filter((value, index, self) => self.indexOf(value) === index);
                
                for (let i = 0; i < uniqueTestNames.length; i++) {
                    //Pobranie wszystkich 3 wynikow dla jednego testu
                    let allTestsForUniqueTestName = packingLabelViewModel.PackingLabelTests.where(x => x.TestName == uniqueTestNames[i]);
                    let viewModel = {};
                
                    //Przypisanie wartości do viewModel'u
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
            } else {
                view.isChecking = false;
                uniqueTestNames = ["Test A", "Test B", "Test C", "Test D", "Test E"];
                for (let i = 0; i < uniqueTestNames.length; i++) {
                    let viewModel = {};
                    viewModel.TestName = uniqueTestNames[i];
                    viewModel.ExpectedValue = "";
                    viewModel.FrontResult = false;
                    viewModel.FrontExpectedValue = "XXX";
                    viewModel.SideResult = false;
                    viewModel.SideExpectedValue = "XXX";
                    viewModel.RearResult = false;
                    viewModel.RearExpectedValue = "XXX";
                    view.Test.push(viewModel);
                }
            }
            console.log("24-");
            RenderTemplate("#testPackingLabelTemplate", "#contenView", view);
            ClearPhotos();

            if (packingLabelViewModel.PackingLabelTests.length != 0) {
                LoadAndPutPhotos(serialNumber);
            }
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