import { Component, OnInit, ViewEncapsulation, EventEmitter, Output } from '@angular/core';
import { AppService } from '../shared/service/AppService';
import { ApiUrlBuilder } from '../shared/service/ApiUrlBuilder';
import { MatDialog } from '@angular/material';
import { UserProfileComponent } from '../user-profile/user-profile.component';
import { Router } from '@angular/router';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.scss'],
  providers:[AppService,ApiUrlBuilder],
  encapsulation: ViewEncapsulation.None
})
export class DashboardComponent implements OnInit {
  public tracks: any[] = [];
  
  constructor(public dialog: MatDialog,
    private appService: AppService,
    private router: Router) {}

   openUserProfileModal(): void {
    const dialogRef = this.dialog.open(UserProfileComponent, {
      width: '700px'
    });
  }

  signOut():void{
    this.appService.logout();
    this.router.navigate(['']);
  }

  ngOnInit() {
    let currentUser = JSON.parse(localStorage.getItem('currentUser'));
    if (!currentUser)
      this.router.navigate(['']);
  }

  getRecommendations(): void{
   this.appService.getRecommendations().subscribe((data) => {
     if(this.tracks.length == 0)
     {
       this.tracks = data;
       if(this.tracks.length > 0)
       {
         var image = "url('" + this.tracks[0].Images[0].Url  + "')";
         $("#backImage").css("background-image",image);
       }
     }
     else
        data.forEach(element => {
            this.tracks.push(element);
        });   
      console.log(this.tracks)});
  }
}
