import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import {BrowserAnimationsModule} from '@angular/platform-browser/animations';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import {MatMenuModule, MatIconModule, MatDialogModule, MatButtonModule, MatSliderModule, MatProgressBarModule} from '@angular/material';
import { UserProfileComponent } from './user-profile/user-profile.component';
import { DashboardComponent } from './dashboard/dashboard.component';
import { HttpClientModule } from '@angular/common/http';
import { MusicPlayerComponent } from './music-player/music-player.component';
import { YoutubePlayerModule } from 'ng2-youtube-player';
import {CarouselModule} from "angular2-carousel";
import * as $ from 'jquery';
import { MusicCarouselComponent } from './music-carousel/music-carousel.component';

@NgModule({
  declarations: [
    AppComponent,
    UserProfileComponent,
    DashboardComponent,
    MusicPlayerComponent,
    MusicCarouselComponent
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
    CarouselModule
  ],
  entryComponents: [UserProfileComponent],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
