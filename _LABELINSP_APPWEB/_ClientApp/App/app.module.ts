import { NgModule, ComponentFactoryResolver } from '@angular/core';
import { HttpModule } from '@angular/http';
import { BrowserModule } from '@angular/platform-browser';

import { AppComponent } from './app.component';
import { NotificationAlertComponent } from './Notification/notification-alert.component';
import { NotificationPanelComponent } from './Notification/notification-panel.component';
import { NotificationComponent } from './Notification/notification.component';
import { NotificationService } from './Notification/notification.service';

@NgModule({
  imports: [BrowserModule, HttpModule ],
  declarations: [AppComponent, NotificationComponent, NotificationAlertComponent, NotificationPanelComponent ],
  bootstrap: [NotificationComponent, NotificationPanelComponent],
  providers: [NotificationService ]
})
export class AppModule {


}
