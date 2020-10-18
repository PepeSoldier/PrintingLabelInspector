import { Component, ComponentFactoryResolver, Injectable, Inject, ReflectiveInjector, ViewChild, ViewContainerRef } from '@angular/core';
import { NotificationService } from './notification.service';
import { IMessage } from './models/message';
import { Observable } from 'rxjs/Observable';
import { IntervalObservable } from "rxjs/observable/IntervalObservable";
import { NotificationAlertComponent } from './notification-alert.component';

@Component({
    selector: 'app-notification',
    templateUrl: './notification.component.html',
    entryComponents: [NotificationAlertComponent ]
})

export class NotificationComponent implements Patterns.Interfaces.IObserver {

    @ViewChild('dynamic', { read: ViewContainerRef }) viewContainerRef: ViewContainerRef
    private alive: boolean;
    private title = "P";
    
    constructor(private notificationService: NotificationService,
                private factoryResolver: ComponentFactoryResolver)
    {
        this.alive = true;
        notificationService.RegisterObserver(this);
    }

    ReceiveNotification(msg: IMessage) {
        this.addDynamicComponent(msg);
    }
    
    addDynamicComponent(msg: IMessage) {
        const factory = this.factoryResolver.resolveComponentFactory(NotificationAlertComponent);
        const component = factory.create(this.viewContainerRef.parentInjector);
        component.instance.message = msg;
        this.viewContainerRef.insert(component.hostView);
        this.removeElements();
    }

    removeElements() {
        let i = this.viewContainerRef.length;
        let j = 0;
        let cmp: NotificationAlertComponent;

        if (i > 10) {
            while (j < 7)
            {
                this.viewContainerRef.get(0).destroy();
                j++;
            }
        }
    }

}