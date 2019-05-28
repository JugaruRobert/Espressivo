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

  openDialog(): void {
    const dialogRef = this.dialog.open(UserProfileComponent, {
      width: '250px'
    });

    dialogRef.afterClosed().subscribe(result => {
      console.log('The dialog was closed');
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
      this.tracks = data;
      else
        data.forEach(element => {
            this.tracks.push(element);
        });   
      console.log(this.tracks)});
  }
}
