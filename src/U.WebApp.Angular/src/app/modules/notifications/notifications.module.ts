import {NgModule} from '@angular/core';
import {BrowserModule} from '@angular/platform-browser';
import {
  MatButtonModule,
  MatButtonToggleModule,
  MatDividerModule, MatExpansionModule,
  MatIconModule, MatListModule, MatMenuModule,
  MatPaginatorModule, MatRadioModule,
  MatSidenavModule,
  MatTableModule
} from "@angular/material";
import {InfiniteScrollModule} from "ngx-infinite-scroll";
import {SharedModule} from "../shared/shared.module";
import {SignalrService} from "./services/signalr.service";
import {NotificationCardComponent} from "./components/notification-card/notification-card.component";
import {NotificationSettingsComponent} from "./components/notification-settings/notification-settings.component";
import {NotificationsComponent} from "./components/notifications-list/notifications-list.component";
import {NotificationService} from "./services/notification.service";

@NgModule({
  imports: [
    BrowserModule,
    SharedModule,
    MatTableModule,
    MatPaginatorModule,
    MatDividerModule,
    InfiniteScrollModule,
    MatIconModule,
    MatSidenavModule,
    MatButtonToggleModule,
    MatRadioModule,
    MatButtonModule,
    MatListModule,
    MatExpansionModule,
    MatMenuModule
  ],
  declarations:
    [
    NotificationsComponent,
    NotificationCardComponent,
    NotificationCardComponent,
    NotificationSettingsComponent
    ],
  exports:
    [
    NotificationsComponent,
    NotificationCardComponent,
    InfiniteScrollModule,
    NotificationSettingsComponent
  ],
  providers:
    [
    SignalrService,
    NotificationService
    ]
})
export class NotificationsModule {

}
