import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { AppService } from '../shared/service/AppService';
import { ApiUrlBuilder } from '../shared/service/ApiUrlBuilder';
import { MatDialog } from '@angular/material';
import { UserProfileComponent } from '../user-profile/user-profile.component';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.scss'],
  providers:[AppService,ApiUrlBuilder],
  encapsulation: ViewEncapsulation.None
})
export class DashboardComponent implements OnInit {

  constructor(public dialog: MatDialog,
    private appService: AppService) {}

  openDialog(): void {
    const dialogRef = this.dialog.open(UserProfileComponent, {
      width: '250px'
    });

    dialogRef.afterClosed().subscribe(result => {
      console.log('The dialog was closed');
    });
  }

  ngOnInit() {
  }

  getRecommendations(): void{
    this.appService.getRecommendations();
  }
}
