self.addEventListener('push', function (event) {
    if (!(self.Notification && self.Notification.permission === 'granted')) {
        return;
    }
    var data = {};
    if (event.data) {
        data = event.data.json();
    }


    var title = data.notification.title;/*.split("/",1);*/
    var message = data.notification.body;
    var icon = "logo_Implea.png";
    
    event.waitUntil(self.registration.showNotification(title, {
        body: message,
        icon: icon,
        badge: data.notification.badge,
        vibrate: [200, 100, 200, 100, 200, 100, 200],
    }));
});

self.addEventListener('notificationclick', function (event) {
    event.notification.close();
    console.log(event.notification.badge);
    clients.openWindow(event.notification.badge);
});

self.addEventListener('notificationclose', function (event) {
    event.notification.close();
});


