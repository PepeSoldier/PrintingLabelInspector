"use strict";
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};
Object.defineProperty(exports, "__esModule", { value: true });
var core_1 = require("@angular/core");
var notification_service_1 = require("./notification.service");
var notification_alert_component_1 = require("./notification-alert.component");
var NotificationComponent = /** @class */ (function () {
    function NotificationComponent(notificationService, factoryResolver) {
        this.notificationService = notificationService;
        this.factoryResolver = factoryResolver;
        this.title = "P";
        this.alive = true;
        notificationService.RegisterObserver(this);
    }
    NotificationComponent.prototype.ReceiveNotification = function (msg) {
        this.addDynamicComponent(msg);
    };
    NotificationComponent.prototype.addDynamicComponent = function (msg) {
        var factory = this.factoryResolver.resolveComponentFactory(notification_alert_component_1.NotificationAlertComponent);
        var component = factory.create(this.viewContainerRef.parentInjector);
        component.instance.message = msg;
        this.viewContainerRef.insert(component.hostView);
        this.removeElements();
    };
    NotificationComponent.prototype.removeElements = function () {
        var i = this.viewContainerRef.length;
        var j = 0;
        var cmp;
        if (i > 10) {
            while (j < 7) {
                this.viewContainerRef.get(0).destroy();
                j++;
            }
        }
    };
    __decorate([
        core_1.ViewChild('dynamic', { read: core_1.ViewContainerRef }),
        __metadata("design:type", core_1.ViewContainerRef)
    ], NotificationComponent.prototype, "viewContainerRef", void 0);
    NotificationComponent = __decorate([
        core_1.Component({
            selector: 'app-notification',
            templateUrl: './notification.component.html',
            entryComponents: [notification_alert_component_1.NotificationAlertComponent]
        }),
        __metadata("design:paramtypes", [notification_service_1.NotificationService,
            core_1.ComponentFactoryResolver])
    ], NotificationComponent);
    return NotificationComponent;
}());
exports.NotificationComponent = NotificationComponent;
//# sourceMappingURL=notification.component.js.map