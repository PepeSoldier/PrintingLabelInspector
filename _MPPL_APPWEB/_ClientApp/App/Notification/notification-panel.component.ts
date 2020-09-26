import { Component, ComponentFactoryResolver, Injectable, Inject, ReflectiveInjector, ViewChild, ViewContainerRef } from '@angular/core';
import { NotificationService } from './notification.service';
import { IMessage } from './models/message';
import { Observable } from 'rxjs/Observable';
import { IntervalObservable } from "rxjs/observable/IntervalObservable";

@Component({
    selector: 'app-notification-panel',
    templateUrl: './notification-panel.component.html',
    styleUrls: ['./notification-panel.component.css']
})

export class NotificationPanelComponent implements Patterns.Interfaces.IObserver {

    //@ViewChild('dynamic', { read: ViewContainerRef }) viewContainerRef: ViewContainerRef
    private alive: boolean;
    private title = "P";

    constructor(private notificationService: NotificationService, private factoryResolver: ComponentFactoryResolver)
    {
        console.log("not-panel-init");
        this.alive = true;
        notificationService.RegisterObserver(this);
    }
    
    addDynamicComponent(msg: IMessage) {
        
    }

    ReceiveNotification(msg: IMessage) {
        this.addDynamicComponent(msg);
    }
}