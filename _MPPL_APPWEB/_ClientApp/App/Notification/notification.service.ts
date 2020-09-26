import { Injectable } from '@angular/core';
import { Http } from '@angular/http';
import { IMessage } from './models/message';
import { Observable } from 'rxjs/Observable';
import 'rxjs/Rx';
import { IntervalObservable } from 'rxjs/observable/IntervalObservable';

@Injectable()

export class NotificationService implements Patterns.Interfaces.IObservable {

    private observers: Array<Patterns.Interfaces.IObserver>;
    
    constructor(private http: Http) {
        console.log("angular notification service");
        this.observers = new Array<Patterns.Interfaces.IObserver>();
        this.StartThread();
    }
    
    getNotifications(): Observable<IMessage[]> {
        console.log("Notifications Angular Service");
        this.http.get("/Test/AlertTest").forEach((r) => console.log(r));
        return this.http.post("/Base/GetAlerts", {userName: "K"}).map((res) => res.json())
    }
    
    StartThread() {
        IntervalObservable.create(2000)
            .subscribe(() => {
                this.getNotifications().subscribe(
                    data => this.NotifyObservers(data));
        });
    }

    RegisterObserver(observer: Patterns.Interfaces.IObserver): void {
        this.observers.push(observer);
    }
    RemoveObserver(observer: Patterns.Interfaces.IObserver): void {
    }
    NotifyObservers(data: IMessage[]): void {

        if (data != null) {
            data.forEach(mgs => {
                mgs.Message = mgs.Message;
                this.observers.forEach(
                    obs => obs.ReceiveNotification(mgs)
                )
            });
        }
        else {
            console.log("null data");
        }
    }
}