function ParseDate(x) {
    return moment(x).format().split("T")[0];
}

function WhDocBrowseMobile() {
    var self = this;

    var viewModel = {
        WhDocuments: {}
    };

    this.Init = function () {
        console.log("WhDocBrowseMobile.Init");
        GetWhDocuments();
        Actions();
    };

    function GetWhDocuments() {
        var ReturnJson = new JsonHelper().GetPostData("/iLogis/WhDoc/WhDocumentGetList",
            { Status: 20, pageIndex: 1, pageSize: 999 });
        ReturnJson.done(function (response) {
            viewModel.WhDocuments = response.data; //type: whDocumentAbstractViewModelList
            //console.log(response.data);
            Render();
        });
    }

    function Render() {
        let templateName = "WhDocumentWZ.Template.cshtml";
        let vm = viewModel;

        for (let i = 0; i < viewModel.WhDocuments.length; i++) {
            RenderTemplate("#WhDocTemplate", "#whDocuments", viewModel.WhDocuments[i], true);
        }
    }
    function Actions() {
        $(document).off("click", ".whDoc");
        $(document).on("click", ".whDoc", function (event) {
            let id = parseInt($(this).attr("data-id"));
            window.location = "/iLOGIS/WhDoc/IndexSignMobile/" + id;
        });
    }
} 
