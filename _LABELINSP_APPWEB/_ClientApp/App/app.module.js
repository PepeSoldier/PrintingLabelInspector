"use strict";
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
Object.defineProperty(exports, "__esModule", { value: true });
var core_1 = require("@angular/core");
var http_1 = require("@angular/http");
var platform_browser_1 = require("@angular/platform-browser");
var app_component_1 = require("./app.component");
var notification_alert_component_1 = require("./Notification/notification-alert.component");
var notification_panel_component_1 = require("./Notification/notification-panel.component");
var notification_component_1 = require("./Notification/notification.component");
var notification_service_1 = require("./Notification/notification.service");
var AppModule = /** @class */ (function () {
    function AppModule() {
    }
    AppModule = __decorate([
        core_1.NgModule({
            imports: [platform_browser_1.BrowserModule, http_1.HttpModule],
            declarations: [app_component_1.AppComponent, notification_component_1.NotificationComponent, notification_alert_component_1.NotificationAlertComponent, notification_panel_component_1.NotificationPanelComponent],
            bootstrap: [notification_component_1.NotificationComponent, notification_panel_component_1.NotificationPanelComponent],
            providers: [notification_service_1.NotificationService]
        })
    ], AppModule);
    return AppModule;
}());
exports.AppModule = AppModule;
//# sourceMappingURL=app.module.js.map