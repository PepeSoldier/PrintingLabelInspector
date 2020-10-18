
var LabelInspector = function (markNew = true) {
    var self = this;
    var threadClock = null;
    var ViewModel = null;
    let waitSeconds = 0;
    let waitScreen = false;
    
    this.Init = function () {
        console.log("LabelInspector.Init");
        ViewModel = {
            isChecking: false,
            Inspection: [],
            WorkorderNo: "",
            ItemCode: "",
            ItemName: "",
            Barcode: "oczekuję na skan..."
        };
        Render();
        ClearPhotos();
    };

    this.StartClock = function () {
        threadClock = setInterval(function () {
            var dateTimeNow = new moment(new Date());
            $("#clockHeaderMid").html(dateTimeNow.format("HH:mm:ss"));
            waitSeconds++;
            if (waitSeconds > 15 && !waitScreen) {
                waitScreen = true;
                self.Init();
            }
        }, 1000);
    };

    this.StopClock = function () {
        window.clearInterval(threadClock);
        threadClock = null;
        console.log(threadClock);
    };

    this.LoadDataBySerialNumber = function (serialNumber, barcode = "") {
        waitSeconds = 0;
        waitScreen = false;
        var ajax = AjaxPost("/LABELINSP/QUALITY/WorkorderLabelGetData", { serialNumber });
        var uniqueTestNames = {};
        ViewModel = {
            isChecking: true,
            Inspection: [],
            WorkorderNo: "",
            ItemCode: "",
            ItemName: "",
            Barcode: barcode
        };
        ajax.done(function (labelinspViewModel) {
            console.log(labelinspViewModel);

            if (labelinspViewModel.WorkorderLabel != null) {
                ViewModel.WorkorderNo = labelinspViewModel.WorkorderLabel.Workorder.WorkorderNumber;
                ViewModel.ItemCode = labelinspViewModel.WorkorderLabel.Workorder.ItemCode;
                ViewModel.ItemName = labelinspViewModel.WorkorderLabel.Workorder.ItemName;
                ViewModel.isChecking = true;

                if (labelinspViewModel.WorkorderLabelInspections.length != 0) {
                    uniqueTestNames = {};
                    uniqueTestNames = labelinspViewModel.WorkorderLabelInspections.map(item => item.TestName).filter((value, index, self) => self.indexOf(value) === index);

                    for (let i = 0; i < uniqueTestNames.length; i++) {
                        //Pobranie wszystkich 3 wynikow dla jednego testu
                        let allTestsForUniqueTestName = labelinspViewModel.WorkorderLabelInspections.where(x => x.TestName == uniqueTestNames[i]);
                        let viewModelInspection = {};

                        //Przypisanie wartości do viewModel'u
                        viewModelInspection.TestName = allTestsForUniqueTestName[0].TestName;
                        viewModelInspection.ExpectedValue = _getExpectedValue(allTestsForUniqueTestName[0]);

                        viewModelInspection.ActualValue_F = _getActualValue(allTestsForUniqueTestName.find(x => x.LabelType == 0));
                        viewModelInspection.ActualValue_S = _getActualValue(allTestsForUniqueTestName.find(x => x.LabelType == 1));
                        viewModelInspection.ActualValue_R = _getActualValue(allTestsForUniqueTestName.find(x => x.LabelType == 2));

                        viewModelInspection.Result_F = allTestsForUniqueTestName.find(x => x.LabelType == 0)?.Result;
                        viewModelInspection.Result_S = allTestsForUniqueTestName.find(x => x.LabelType == 1)?.Result;
                        viewModelInspection.Result_R = allTestsForUniqueTestName.find(x => x.LabelType == 2)?.Result;

                        viewModelInspection.ResultClass_F = _GetResultClass(viewModelInspection.Result_F);
                        viewModelInspection.ResultClass_S = _GetResultClass(viewModelInspection.Result_S);
                        viewModelInspection.ResultClass_R = _GetResultClass(viewModelInspection.Result_R);

                        viewModelInspection.ActualValueIcon_F = _GetResultIcon(viewModelInspection.Result_F);
                        viewModelInspection.ActualValueIcon_S = _GetResultIcon(viewModelInspection.Result_S);
                        viewModelInspection.ActualValueIcon_R = _GetResultIcon(viewModelInspection.Result_R);

                        if (viewModelInspection.ExpectedValue.length > 0 && viewModelInspection.ExpectedValue[0] == '-') {
                            viewModelInspection.ResultClass_F = "testResultUnknown";
                            viewModelInspection.ResultClass_S = "testResultUnknown";
                            viewModelInspection.ResultClass_R = "testResultUnknown";
                            viewModelInspection.ActualValueIcon_F = "";
                            viewModelInspection.ActualValueIcon_S = "";
                            viewModelInspection.ActualValueIcon_R = "";
                        }

                        ViewModel.Inspection.push(viewModelInspection);
                    }
                }
            }
            else {
                ViewModel.isChecking = false;
                uniqueTestNames = ["Test A", "Test B", "Test C", "Test D", "Test E"];
                for (let i = 0; i < uniqueTestNames.length; i++) {
                    ViewModel.Barcode = "000000000000000000000";
                    ViewModel.WorkorderNo = "1600000000";
                    ViewModel.ItemCode = "911000000";
                    ViewModel.ItemName = "?";

                    let viewModelInspection = {};

                    viewModelInspection.TestName = uniqueTestNames[i];
                    viewModelInspection.ExpectedValue = "?";
                    viewModelInspection.ActualValue_F = "---";
                    viewModelInspection.ActualValue_S = "---";
                    viewModelInspection.ActualValue_R = "---";
                    viewModelInspection.Result_F = false;
                    viewModelInspection.Result_S = false;
                    viewModelInspection.Result_R = false;
                    viewModelInspection.ActualValueIcon_F = '<i class="fas fa-question-circle"></i>';
                    viewModelInspection.ActualValueIcon_S = '<i class="fas fa-question-circle"></i>';
                    viewModelInspection.ActualValueIcon_R = '<i class="fas fa-question-circle"></i>';

                    ViewModel.Inspection.push(viewModelInspection);
                }
            }
            
            Render();
            ClearPhotos();

            if (labelinspViewModel.WorkorderLabel != null) {
                LoadAndPutPhotos(serialNumber);
            }
        });
    };

    function _getActualValue(obj) {
        if (obj != null) {
            return obj.ActualValueText != null ? obj.ActualValueText : obj.ActualValue;
        }
        else {
            return 0;
        }
    }
    function _getExpectedValue(obj) {
        if (obj != null) {
            return obj.ExpectedValueText != null? obj.ExpectedValueText : obj.ExpectedValue;
        }
        else {
            return 0;
        }
    }

    function _GetResultClass(result) {
        switch (result) {
            case true: return "testResultPositive";
            case false: return "testResultFalse";
            case null: return "testResultUnknown";
        }
    }
    function _GetResultIcon(result) {
        switch (result) {
            case true: return '<i class="fas fa-check-circle"></i>';
            case false: return '<i class="fas fa-times-circle"></i>';
            case null: return '';
        }
    }


    function ClearPhotos() {
        $("#frontPhoto").html("");
        $("#image_F").html("");
        $("#image_S").html("");
        $("#image_R").html("");
    }
    function LoadAndPutPhotos(serialNumber) {
        let imageFront = '/Labels/' + serialNumber + '_F' + '.PNG';
        let imageSide = '/Labels/' + serialNumber + '_S' + '.PNG';
        let imageRear = '/Labels/' + serialNumber + '_R' + '.PNG';

        PutBigPhoto(serialNumber);

        PutPhoto(imageFront, "#image_F");
        PutPhoto(imageSide, "#image_S");
        PutPhoto(imageRear, "#image_R");
    }
    function PutPhoto(filePath, selector) {
        try {
            var img = new Image();
            img.src = window.location.origin + filePath;          //'url(\'' + '/_MPPL_APPWEB' + photo + '\')';
            img.className = "img-fluid";
            img.onerror = function () { console.log("Error: Unable to load image " + filePath); };
            $(selector).append(img);
        } catch (e) {
            console.log("Unable to load image " + filePath);
        }
    }
    function PutBigPhoto(serialNumber, i = 0) {
        let imageTypes = ['F', 'S', 'R'];
        let filePath = '/Labels/' + serialNumber + '_' + imageTypes[i] + '.PNG';

        $("#frontPhoto").html("");

        var img = new Image();
        img.src = window.location.origin + filePath;
        img.className = "img-fluid";
        img.onerror = function () { if (i < imageTypes.length) { PutBigPhoto(serialNumber, i + 1); } };
        $("#frontPhoto").append(img);
        
        //try {
            //var img = new Image();
            //img.src = window.location.origin + '/Labels/04129757_R.PNG';
            //img.className = "img-fluid";
            //img.onerror = function () { console.log("Lipton"); };
            //$("#frontPhoto").append(img);
        //} catch (e) {
        //    console.log("kecz-------------------------");
        //}

        //$.get(filePath)
        //    .done(function (image) {
        //        var img = new Image();
        //        img.src = window.location.origin + filePath;
        //        img.className = "img-fluid";
        //        $(selector).append(img);
        //    }).fail(function () {
        //        if (i < imageTypes.length) {
        //            PutBigPhoto(i + 1);
        //        }
        //    });
    }

    function Render() {
        RenderTemplate("#WorkorderLabelTemplate", "#contenView", ViewModel);
    }
};