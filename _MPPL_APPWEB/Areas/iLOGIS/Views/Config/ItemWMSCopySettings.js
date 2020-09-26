var ItemWMSCopySettings = function () {

    var self = this;

    this.Copy = function (itemCode, itemCodeNew) {
        var jsh = new JsonHelper();
        var json = jsh.GetPostData("/iLogis/Config/ItemWMSCopySettings", { itemCode, itemCodeNew });
        json.done(function (done) {
            if (done == true) {
                new Alert().Show("success", "Kopiowanie zakończone powodzeniem");
            } else {
                new Alert().Show("warning", "Kopiowanie nie powiodło się. Sprawdz czy podane kody istnieją");
            }
        });
        json.fail(function () {
            new Alert().Show("error", "Wystąpił problem podczas kopiowania");
        });
    };

    this.OpenViewInWindow = function () {
        var wnd = new PopupWindow(900, 300, 120, 490);
        wnd.Init("ItemWMSCopySettings", "Kopiowanie ustawień artykułu");
        wnd.Show("loading");

        $.get("/iLogis/Config/ItemWMSCopySettings", function (data) {
            wnd.Show(data);
        });
    };
};
