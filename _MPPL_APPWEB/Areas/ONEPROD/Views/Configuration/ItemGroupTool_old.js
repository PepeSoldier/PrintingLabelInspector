
function ItemGroupTool() {

    this.Init = function () {
        $("#pctAddForm").submit(function (e) {
            e.preventDefault();
            AddTool();
        });
        $("#pctAddSubmit").click(function (e) {
            $("#pctAddForm").submit();
        });
        $(".btnPctDelete").click(function (e) {
            DeleteTool(this);
        });
    }

    function RelodTools(partCategoryId) {
        console.log("reload");
        $.get(BaseUrl + "ConfigurationAPS/ItemGroupTool/" + partCategoryId, function (data) {
            $("#ItemGroupT").html($(data + " #ItemGroupT"));
        });
    }
    function AddTool() {
        $.ajax({
            url: BaseUrl + 'ConfigurationAPS/ItemGroupToolAdd',
            type: 'post',
            data: $('#pctAddForm').serialize(),
            success: function (result) {
                RelodTools(result);
            }
        });
    }
    function DeleteTool(elBtn) {
        var pctId = $(elBtn).attr("id").split("_")[1];
        $.ajax({
            url: BaseUrl + 'ConfigurationAPS/ItemGroupToolDelete',
            type: 'post',
            data: { id: pctId },
            success: function (result) {
                RelodTools(result);
            }
        });
    }
}