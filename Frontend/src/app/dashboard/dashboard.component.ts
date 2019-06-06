import { Component, OnInit, ViewEncapsulation, EventEmitter, Output } from '@angular/core';
import { AppService } from '../shared/service/AppService';
import { ApiUrlBuilder } from '../shared/service/ApiUrlBuilder';
import { MatDialog } from '@angular/material';
import { UserProfileComponent } from '../user-profile/user-profile.component';
import { Router } from '@angular/router';
import { HTTPStatus } from '../shared/library/errorInterceptor';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.scss'],
  providers:[AppService,ApiUrlBuilder],
  encapsulation: ViewEncapsulation.None
})
export class DashboardComponent implements OnInit {
  public tracks: any[] = [];
  private userGenres = [];
  private allGenres = [];
  private userArtists = [];
  public isLoading:boolean = true;

  constructor(public dialog: MatDialog,
    private appService: AppService,
    private router: Router,
    private httpStatus:HTTPStatus) {
      this.httpStatus.getHttpStatus().subscribe((status: boolean) => {this.isLoading = status});
    }

   openUserProfileModal(): void {
    const dialogRef = this.dialog.open(UserProfileComponent, {
      width: '700px',
      data: { 
        allGenres: this.allGenres,
        userGenres: this.userGenres,
        userArtists: this.userArtists
      }
    });
  }

  signOut():void{
    this.appService.logout();
    this.router.navigate(['']);
  }

  ngOnInit() {
    this.getAllGenres();
    let currentUser = JSON.parse(localStorage.getItem('currentUser'));
    if (!currentUser)
      this.router.navigate(['']);
    else
      this.getUserPreferences(currentUser.ID);
  }

  getUserPreferences(userID:string){
    this.appService.getPreferences(userID).subscribe((data) =>{
      this.userArtists = data.Artists;
      this.userGenres = data.Genres.map(genre => genre.Name);
      
      if(this.userArtists.length == 0 && this.userGenres.length == 0)
      {
        this.router.navigate(['preferences']);
      }
    })
  }

  getAllGenres(){
    this.appService.getGenres().subscribe(genres => {
        this.allGenres = genres;
    });
  }

  getRecommendations(imageFile:any): void{
   this.httpStatus.setHttpStatus(true);
   this.appService.getRecommendations(imageFile).subscribe((data) => {
     if(this.tracks.length == 0)
     {
       this.tracks = data;
       console.log(this.tracks);
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
        this.httpStatus.setHttpStatus(false);
    });
  }

  backToMainPage() {
    $("#backImage").css("background-image","url('../../assets/images/background.jpg')");
    this.tracks = [];
  }
}