import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import {BrowserAnimationsModule} from '@angular/platform-browser/animations';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import {MatMenuModule, MatIconModule, MatDialogModule, MatButtonModule, MatSliderModule, MatProgressBarModule, MatTabsModule, MatSnackBarModule, MatExpansionModule, MatInputModule, MatCheckboxModule, MatAutocompleteModule, MatProgressSpinnerModule} from '@angular/material';
import { UserProfileComponent } from './user-profile/user-profile.component';
import { DashboardComponent } from './dashboard/dashboard.component';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { MusicPlayerComponent } from './music-player/music-player.component';
import { YoutubePlayerModule } from 'ng2-youtube-player';
import {CarouselModule} from "angular2-carousel";
import * as $ from 'jquery';
import { MusicCarouselComponent } from './music-carousel/music-carousel.component';
import { LoginPageComponent } from './login-page/login-page.component';
import { ReactiveFormsModule } from '@angular/forms';
import { LoginFormComponent } from './login-form/login-form.component';
import { RegisterFormComponent } from './register-form/register-form.component';
import { JwtInterceptor } from './shared/library/jwtInterceptor';
import { ErrorInterceptor, HTTPStatus } from './shared/library/errorInterceptor';
import { AppService } from './shared/service/AppService';
import { ApiUrlBuilder } from './shared/service/ApiUrlBuilder';
import { FirstLoginInformationComponent } from './first-login-information/first-login-information.component';
import { ConfigurationPageComponent } from './configuration-page/configuration-page.component';

@NgModule({
  declarations: [
    AppComponent,
    UserProfileComponent,
    DashboardComponent,
    MusicPlayerComponent,
    MusicCarouselComponent,
    LoginPageComponent,
    LoginFormComponent,
    RegisterFormComponent,
    FirstLoginInformationComponent,
    ConfigurationPageComponent
  ],
  imports: [
    BrowserAnimationsModule,
    BrowserModule,
    AppRoutingModule,
    MatMenuModule,
    MatIconModule,
    MatDialogModule,
    MatButtonModule,
    HttpClientModule,
    YoutubePlayerModule,
    MatSliderModule,
    MatProgressBarModule,
    CarouselModule,
    MatTabsModule,
    ReactiveFormsModule,
    MatSnackBarModule,
    MatExpansionModule,
    MatInputModule,
    MatCheckboxModule,
    MatAutocompleteModule,
    MatProgressSpinnerModule
  ],
  entryComponents: [UserProfileComponent],
  providers: [
    AppService,
    ApiUrlBuilder,
    HTTPStatus,
    { provide: HTTP_INTERCEPTORS, useClass: JwtInterceptor, multi: true },
    { provide: HTTP_INTERCEPTORS, useClass: ErrorInterceptor, multi: true }],
  bootstrap: [AppComponent]
})
export class AppModule { }
