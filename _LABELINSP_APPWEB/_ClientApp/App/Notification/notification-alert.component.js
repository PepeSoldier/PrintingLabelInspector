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
var NotificationAlertComponent = /** @class */ (function () {
    function NotificationAlertComponent() {
        this.message = { MessageTypeName: "", Message: "", UserName: "" };
        this.timedOut = false;
        console.log("Alert z angulara");
    }
    NotificationAlertComponent.prototype.ngOnInit = function () {
        setTimeout(function () {
            this.timedOut = true;
        }.bind(this), 5000);
    };
    NotificationAlertComponent.prototype.ngOnDestroy = function () {
        console.log("destroy");
    };
    __decorate([
        core_1.Input(),
        __metadata("design:type", Object)
    ], NotificationAlertComponent.prototype, "message", void 0);
    NotificationAlertComponent = __decorate([
        core_1.Component({
            selector: 'app-notification-alert',
            templateUrl: './notification-alert.component.html',
            styleUrls: ['./notification-alert.component.css']
        }),
        __metadata("design:paramtypes", [])
    ], NotificationAlertComponent);
    return NotificationAlertComponent;
}());
exports.NotificationAlertComponent = NotificationAlertComponent;
//# sourceMappingURL=notification-alert.component.js.map