import { Component, Input } from '@angular/core';
import { IntervalObservable } from 'rxjs/observable/IntervalObservable';
import { IMessage } from './models/message';

@Component({
    selector: 'app-notification-alert',
    templateUrl: './notification-alert.component.html',
    styleUrls: ['./notification-alert.component.css']
})
export class NotificationAlertComponent {

    @Input() message: IMessage = { MessageTypeName: "", Message: "", UserName: "" };
    public timedOut: boolean = false;

    constructor() {
        console.log("Alert z angulara");
    }

    ngOnInit() {
        setTimeout(function () {
            this.timedOut = true;
        }.bind(this), 5000);
    }

    ngOnDestroy() {
        console.log("destroy");
    }
}