import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { FormGroup, Validators, FormBuilder, FormControl } from '@angular/forms';
import { AppService } from '../shared/service/AppService';
import { MatSnackBar } from '@angular/material';
import { Router } from '@angular/router';

@Component({
  selector: 'app-first-login-information',
  templateUrl: './first-login-information.component.html',
  styleUrls: ['./first-login-information.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class FirstLoginInformationComponent implements OnInit {
  public genres: any[];
  public artists: string[];
  public selectedArtists: any[] = [];
  public selectedGenres: any[] = [];
  public searchArtist: FormControl = new FormControl();

  private searchHandler:any;
  private searchDelay: number = 500;
  
  constructor(private service:AppService,private snackBar: MatSnackBar,private router:Router) { }

  ngOnInit() {
    this.getGenres();
  }

  getGenres(){
    this.service.getGenres().subscribe(genres => {
        this.genres = genres;
    });
  }

  onKeyUp(){
    clearTimeout(this.searchHandler);
    this.searchHandler = setTimeout(() => {this.getArtists(this.searchArtist.value)}, this.searchDelay);
  }

  onKeyDown(){
    clearTimeout(this.searchHandler);
  }

  getArtists(value:string){
    this.artists = [];
    if(value && value.trim().length > 0)
    {
      this.service.getArtists(value).subscribe(artists => {
        this.artists = artists.artists.items;
      });
    }
  }

  addArtist(artistName:string,artistID:string){
      this.artists = [];
      this.searchArtist.setValue("");

      if(this.selectedArtists.length >= 120)
        this.openSnackBar("You cannot select more that 120 artists!");
      else
      {
        var foundArtist = this.selectedArtists.filter(artist => artist.ArtistID == artistID);
        if(foundArtist.length == 0)
          this.selectedArtists.push({"ArtistID":artistID,"Name":artistName});
      }
  }

  unselectArtist(index:number){
    if (index >= 0 && index < this.selectedArtists.length) {
        this.selectedArtists.splice(index, 1);
    }    
  }

  validateAndSave(){
    if(this.selectedArtists.length == 0 && this.selectedGenres.length == 0)
      this.openSnackBar("You need to select at least one genre or artist");
    else
    {
      let currentUser = JSON.parse(localStorage.getItem('currentUser'));
      if(currentUser)
      {
        if(this.selectedArtists.length > 0)
          this.service.insertUserArtists(currentUser.Username, this.selectedArtists);

        if(this.selectedGenres.length > 0)
          this.service.insertUserGenres(currentUser.Username, this.selectedGenres);
        
        //this.router.navigate(['dashboard']);
      }
      else
      {
        this.openSnackBar("An error has occured!");
        this.service.logout();
        this.router.navigate([]);
      }
    }
  }

  private openSnackBar(message: string) {
    this.snackBar.open(message, '', {verticalPosition: "top", duration: 6000});
  }

  genreCheckboxChange(checked:boolean,genreName:string){
    genreName = genreName.charAt(0).toUpperCase() + genreName.slice(1);
    if(checked == true)
    {
      if(this.selectedGenres.indexOf(genreName) == -1)
        this.selectedGenres.push(genreName)
    }
    else
    {
      var index = this.selectedGenres.indexOf(genreName) ;
      if(index != -1)
        this.selectedGenres.splice(index, 1);
    }
  }
}
