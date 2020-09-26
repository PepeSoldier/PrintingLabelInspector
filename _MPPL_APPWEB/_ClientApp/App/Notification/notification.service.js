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
var http_1 = require("@angular/http");
require("rxjs/Rx");
var IntervalObservable_1 = require("rxjs/observable/IntervalObservable");
var NotificationService = /** @class */ (function () {
    function NotificationService(http) {
        this.http = http;
        console.log("angular notification service");
        this.observers = new Array();
        this.StartThread();
    }
    NotificationService.prototype.getNotifications = function () {
        console.log("Notifications Angular Service");
        this.http.get("/Test/AlertTest").forEach(function (r) { return console.log(r); });
        return this.http.post("/Base/GetAlerts", { userName: "K" }).map(function (res) { return res.json(); });
    };
    NotificationService.prototype.StartThread = function () {
        var _this = this;
        IntervalObservable_1.IntervalObservable.create(2000)
            .subscribe(function () {
            _this.getNotifications().subscribe(function (data) { return _this.NotifyObservers(data); });
        });
    };
    NotificationService.prototype.RegisterObserver = function (observer) {
        this.observers.push(observer);
    };
    NotificationService.prototype.RemoveObserver = function (observer) {
    };
    NotificationService.prototype.NotifyObservers = function (data) {
        var _this = this;
        if (data != null) {
            data.forEach(function (mgs) {
                mgs.Message = mgs.Message;
                _this.observers.forEach(function (obs) { return obs.ReceiveNotification(mgs); });
            });
        }
        else {
            console.log("null data");
        }
    };
    NotificationService = __decorate([
        core_1.Injectable(),
        __metadata("design:paramtypes", [http_1.Http])
    ], NotificationService);
    return NotificationService;
}());
exports.NotificationService = NotificationService;
//# sourceMappingURL=notification.service.js.map