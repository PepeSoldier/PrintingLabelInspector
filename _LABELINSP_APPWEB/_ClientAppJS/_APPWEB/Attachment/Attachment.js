

function Attachment(parentDivId, controllerName1, actionName1, objectId1, callback,parentType)
{
    var self = this;
    var parentDiv = parentDivId;
    var controllerName = controllerName1;
    var actionName = actionName1;
    var objectId = objectId1;
    var callbackFunc = callback;
    var parentTypeToUpload = parentType;
    this.Init = function ()
    {
        $(parentDivId + ' .fileInput').change(function (evt) {
            if (window.FormData !== undefined) {
                var fileUpload = $(this).get(0);
                var files = fileUpload.files;
                UploadFile(files);
            }
            else {
                console.log("FormData is not supported.");
            }
        });
    }

    this.Delete = function ()
    {
        console.log("Delete Document");
        var fileData = new FormData();
        fileData.append('parentObjectId', objectId1);    //Adding one more key to FormData object
        fileData.append('parentType', parentType)
        $.ajax({
            url: "/" + controllerName + "/" + actionName,  //"@Url.Action("UploadPhoto","Photo")",
            type: "POST",
            contentType: false,                         // Not to set any content header
            processData: false,                         // Not to process data
            data: fileData,
            success: function (result) {
                    callback(result);
            },
            error: function (err) {
            },
        }).done(function () {
        });
    }

    function UploadFile(files)
    {
        UploadStart(objectId);
        var fileData = new FormData();                  // Create FormData object
        for (var i = 0; i < files.length; i++) {
            fileData.append(files[i].name, files[i]);   // Looping over all files and add it to FormData object
        }
        fileData.append('parentObjectId', objectId);    //Adding one more key to FormData object
        fileData.append('parentType', parentTypeToUpload)
        $.ajax({
            url: "/" + controllerName + "/" + actionName,  //"@Url.Action("UploadPhoto","Photo")",
            type: "POST",
            contentType: false,                         // Not to set any content header
            processData: false,                         // Not to process data
            data: fileData,
            success: function (result) {
                //UploadStop(objectId);
                //UploadSuccess(result);
                callback(result);
            },
            error: function (err) {
                UploadStop(objectId);
                alert(err.UploadErrorMessage);
            },
        }).done(function () {
            console.log("tutaj bnlad");
        });
    }
    function UploadStart(id) {
        $('#loadingDiv_' + id).show();
        $('#btnUpload_' + id).hide();
    };
    function UploadStop(id) {
        $('#loadingDiv_' + id).hide();
        $('#btnUpload_' + id).show();
    };
    function UploadSuccess(results) {
        results.forEach(function (result1) {
            callback(results);
        });
    }

}