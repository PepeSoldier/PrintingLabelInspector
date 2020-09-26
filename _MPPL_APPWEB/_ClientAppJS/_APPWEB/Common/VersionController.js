function VersionController(_moduleName = "", _autoupdate = false) {
    var self = this;
    var runningVersion = 0;
    var autoUpdate = _autoupdate;
    var moduleName = _moduleName;

    Init();

    function Init() {
        console.log("VersionController.Init");
    }

    function GetRunningVersion() {
        let version = localStorage.getItem(moduleName);
        varsion = version != null ? version : 0;

        console.log("VersionController.GetRunningVersion() = " + version);
        return version;
    }
    function SetRunningVersion(_currentVersion) {
        localStorage.setItem(moduleName, _currentVersion);
    }
    function ReloadAndRefresh(_currentVersion) {
        SetRunningVersion(_currentVersion);
        
        window.parent.caches.delete("call").then(function () {
            new Alert().Show("warning", "Zaktualizowano aplikację...");
            window.location.reload(true);
        });
    }

    this.SetAutoUpdate = function () { autoUpdate = true; };
    this.CheckVersion = function (_currentVersion) {
        if (runningVersion > 0 && _currentVersion > runningVersion) {
            if (autoUpdate == true) {
                location.reload(true);
            }
            else {
                let wnd_check = new PopupWindow(850, 200, 140, 380);
                wnd_check.Init("Version", "Aktualizacja");
                wnd_check.Show(
                    $("<div>")
                        .append($("<div>")
                            .append("h3")
                            .text("Pojawiła się nowa wersja systemu"))
                        .append($("<div>")
                            .addClass("btn btn-default")
                            .text("aktualizuj")
                            .on("click", function () { location.reload(true); return false; }))
                );
            }
        }
        else {
            runningVersion = _currentVersion;
        }
    };


    this.CheckForUpdates = function (controllerUrl) {

        localStorage.getItem(moduleName);

        let json = new JsonHelper().GetPostData(controllerUrl, { moduleName: moduleName });
        json.done(function (currentVersion) {
            console.log("VersionController.GetLatestVersionNumber() = " + currentVersion);
            self.CheckVersion2(currentVersion);
        });
    };
    this.CheckVersion2 = function (_currentVersion) {
        let rv = GetRunningVersion();
        if (rv == 0 || _currentVersion > rv) {
            if (autoUpdate == true) {
                ReloadAndRefresh(_currentVersion);
            }
            else {
                let wnd_check = new PopupWindow(850, 200, 140, 380);
                wnd_check.Init("Version", "Aktualizacja");
                wnd_check.Show(
                    $("<div>")
                        .append($("<div>")
                            .append("h3")
                            .text("Pojawiła się nowa wersja systemu"))
                        .append($("<div>")
                            .addClass("btn btn-default")
                            .text("aktualizuj")
                            .on("click", function () {
                                ReloadAndRefresh(_currentVersion);
                                return false;
                            }))
                );
            }
        }
        else {
           SetRunningVersion(_currentVersion);
        }
    };

}