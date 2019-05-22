import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import {BrowserAnimationsModule} from '@angular/platform-browser/animations';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import {MatMenuModule, MatIconModule, MatDialogModule, MatButtonModule} from '@angular/material';
import { UserProfileComponent } from './user-profile/user-profile.component';
import { DashboardComponent } from './dashboard/dashboard.component';
import { HttpClientModule } from '@angular/common/http';

@NgModule({
  declarations: [
    AppComponent,
    UserProfileComponent,
    DashboardComponent
  ],
  imports: [
    BrowserAnimationsModule,
    BrowserModule,
    AppRoutingModule,
    MatMenuModule,
    MatIconModule,
    MatDialogModule,
    MatButtonModule,
    HttpClientModule
  ],
  entryComponents: [UserProfileComponent],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
