
module Patterns.Interfaces {

    export interface IObservable {
        RegisterObserver(observer: IObserver): void;
        RemoveObserver(observer: IObserver): void;
        NotifyObservers(data: any): void;
    }

    export interface IObserver {
        ReceiveNotification(message: any): void;
    }
}