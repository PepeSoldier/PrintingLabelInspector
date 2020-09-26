
function ChangeDeclarationDate(_prodLogId, callback) {

    var self = this;
    var productionLogId = _prodLogId;
    

    this.Init = function () { };

    this.Execute = function () {
        console.log("ChangeDeclarationDate.Execute");
        
        let json = new JsonHelper().GetPostData("/ONEPROD/OEEReportOnline/ChangeDeclarationDateExecute", { productionLogId });
        json.done(function (data) {
            new Alert().Show("success", "Data deklaracji została zmieniona");
            callback();
        });
        json.fail(function () {
            new Alert().Show("danger", "Operacja nie powiodła się");
            callback();
        });
    };
 
    $(document).off("click", "#btnExecute");
    $(document).on("click", "#btnExecute", function () {
        console.log("btnExecute");
        self.Execute();
    });
}

