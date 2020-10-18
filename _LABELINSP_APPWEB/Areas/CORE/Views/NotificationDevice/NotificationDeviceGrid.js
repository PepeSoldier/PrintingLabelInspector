var NotificationDeviceGrid = function (gridDivSelector) {
    console.log("NotificationDeviceGrid");
    GridBulkUpdate.call(this, gridDivSelector);
    //var self = this;
    this.divSelector = gridDivSelector;
    this.gridHelper = new GridHelper("NotificationDevice", "/CORE/NotificationDevice");
};

NotificationDeviceGrid.prototype = Object.create(GridBulkUpdate.prototype);
NotificationDeviceGrid.prototype.constructor = NotificationDeviceGrid;

NotificationDeviceGrid.prototype.InitGrid = function () {
    var grid = this;
    $(this.divSelector).jsGrid({
        width: "100%",
        bulkUpdate: false,
        inserting: false, editing: false, sorting: false,
        paging: true, pageLoading: true, pageSize: 20,
        confirmDeleting: false, filtering: false,
        fields: [
            { name: "UserName", type: "text", title: "Użytkownik", width: 40, },
            { name: "RegisterDate", type: "text", title: "Data rejestracji", width: 40, },
            {
                name: "Test", width: 20, filtering: false, editing: false, inserting: false,
                itemTemplate: function (value, item) {
                    //console.log(item);
                    return $("<button>").html('<i class="fas fa-comment-dots"></i>')
                        .addClass("btn btn-sm btn-info btnOpen")
                        .attr("data-userId", item.UserId)
                        .on("click", function () {
                            TestNotification(item.UserId, item.PushEndpoint)
                        });
                }
            },
            {
                name: "Usuń", width: 20, filtering: false, editing: false, inserting: false,
                itemTemplate: function (value, item) {
                    return $("<button>").html('<i class="fa fa-trash"></i>')
                        .addClass("btn btn-sm btn-danger btnOpen")
                        .on("click", function () {
                            DeleteDevice(item.Id)
                        });
                }
            }
        ],
        controller: grid.gridHelper.DB,
        onDataLoaded: function () { },
        rowClick: function (args) {
            console.log(args.item);
        }
    });
    this.grid = $(this.divSelector).data("JSGrid");
};
NotificationDeviceGrid.prototype.CreateNewGridInstance = function (divSelector) {
    return new NotificationDeviceGrid(divSelector);
};

NotificationDeviceGrid.prototype.RefreshGrid = function (filterData) {
    if (filterData != null) {
        this.gridHelper.SetFilter(filterData);
    }
    $(this.divSelector).jsGrid("search");
};

NotificationDeviceGrid.prototype.AddDevice = function (vapidPublicKey) {
    var serviceWorker = '/serviceworker.js';
    var isSubscribed = false;
    // Application Server Public Key defined in Views/Device/Create.cshtml
    if (typeof vapidPublicKey === 'undefined') {
        errorHandler('Vapid public key is undefined.');
        return;
    }

    Notification.requestPermission().then(function (status) {
        if (status === 'denied') {
            errorHandler('[Notification.requestPermission] Browser denied permissions to notification api.');
            alert("Dodaj strone do zaufanych w chrome://flags -> Insecure origins treated as secure")
        } else if (status === 'granted') {
            console.log('[Notification.requestPermission] Initializing service worker.');
            initialiseServiceWorker(serviceWorker);
            subscribe(vapidPublicKey);
        }
    });
};

function AddNotificationDevice(pushEndpoint, pushP256DH, pushAuth) {

    var notificationDevice = {
        PushEndpoint: pushEndpoint,
        PushP256DH: pushP256DH,
        PushAuth: pushAuth
    }
    var JsonHelp = new JsonHelper();
    var ReturnJson = JsonHelp.GetPostData("/CORE/NotificationDevice/NotificationDeviceCreateNew", { device: notificationDevice });
    ReturnJson.done(function (jsonModel) {
        if (jsonModel.Data == null) {
            new Alert().Show(jsonModel.MessageTypeString, jsonModel.Message);
        } else {
            new Alert().Show(jsonModel.MessageTypeString, jsonModel.Message);
            Refresh();
        }
    });
}

function TestNotification(userId, pushEndpoint) {
    var JsonHelp = new JsonHelper();
    var ReturnJson = JsonHelp.GetPostData("/CORE/NotificationDevice/TestNotification", { userId, pushEndpoint });
    ReturnJson.done(function (jsonModel) {
        if (jsonModel.Data == null) {
            console.log("Udało sie");
        } else {
            new Alert().Show(jsonModel.MessageTypeString, jsonModel.Message);
            console.log(jsonModel.Data);
        }
    });
}


function DeleteDevice(deviceId) {
    var JsonHelp = new JsonHelper();
    var ReturnJson = JsonHelp.GetPostData("/CORE/NotificationDevice/NotificationDeviceDelete", { id: deviceId });
    ReturnJson.done(function (jsonModel) {
        if(jsonModel.Data == null) {
            new Alert().Show(jsonModel.MessageTypeString, jsonModel.Message);
        } else {
            new Alert().Show(jsonModel.MessageTypeString, jsonModel.Message);
            Refresh();
        }
    });
}

function initialiseServiceWorker(serviceWorker) {
    if ('serviceWorker' in navigator) {
        navigator.serviceWorker.register(serviceWorker).then(handleSWRegistration);
    } else {
        errorHandler('[initialiseServiceWorker] Service workers are not supported in this browser.');
    }
};

function handleSWRegistration(reg) {
    if (reg.installing) {
        console.log('Service worker installing');
    } else if (reg.waiting) {
        console.log('Service worker installed');
    } else if (reg.active) {
        console.log('Service worker active');
    }

    initialiseState(reg);
}

// Once the service worker is registered set the initial state
function initialiseState(reg) {
    // Are Notifications supported in the service worker?
    if (!(reg.showNotification)) {
        errorHandler('[initialiseState] Notifications aren\'t supported on service workers.');
        return;
    }

    // Check if push messaging is supported
    if (!('PushManager' in window)) {
        errorHandler('[initialiseState] Push messaging isn\'t supported.');
        return;
    }

    // We need the service worker registration to check for a subscription
    navigator.serviceWorker.ready.then(function (reg) {
        // Do we already have a push message subscription?
        reg.pushManager.getSubscription()
            .then(function (subscription) {
                isSubscribed = subscription;
                if (isSubscribed) {
                    console.log('User is already subscribed to push notifications');
                } else {
                    console.log('User is not yet subscribed to push notifications');
                }
            })
            .catch(function (err) {
                console.log('[req.pushManager.getSubscription] Unable to get subscription details.', err);
            });
    });
}

function subscribe(applicationServerPublicKey) {
    navigator.serviceWorker.ready.then(function (reg) {
        var subscribeParams = { userVisibleOnly: true };

        //Setting the public key of our VAPID key pair.
        var applicationServerKey = urlB64ToUint8Array(applicationServerPublicKey);
        subscribeParams.applicationServerKey = applicationServerKey;
        reg.pushManager.subscribe(subscribeParams)
            .then(function (subscription) {
                isSubscribed = true;
                var p256dh = base64Encode(subscription.getKey('p256dh'));
                var auth = base64Encode(subscription.getKey('auth'));
                AddNotificationDevice(subscription.endpoint, p256dh, auth);
            })
            .catch(function (e) {
                errorHandler('[subscribe] Unable to subscribe to push', e);
            });
    });
}

function errorHandler(message, e) {
    if (typeof e == 'undefined') {
        e = null;
    }

    console.error(message, e);
    $("#errorMessage").append('<li>' + message + '</li>').parent().show();
}

function urlB64ToUint8Array(base64String) {
    var padding = '='.repeat((4 - base64String.length % 4) % 4);
    var base64 = (base64String + padding)
        .replace(/\-/g, '+')
        .replace(/_/g, '/');

    var rawData = window.atob(base64);
    var outputArray = new Uint8Array(rawData.length);

    for (var i = 0; i < rawData.length; ++i) {
        outputArray[i] = rawData.charCodeAt(i);
    }
    return outputArray;
}

function base64Encode(arrayBuffer) {
    return btoa(String.fromCharCode.apply(null, new Uint8Array(arrayBuffer)));
}



