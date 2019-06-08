import { Component, OnInit, Inject, ViewChild, Output } from '@angular/core';
import { MAT_DIALOG_DATA, MatSnackBar, MatDialogRef } from '@angular/material';
import { DialogData } from '../app.component';
import { FormControl, FormGroup, Validators, FormBuilder, EmailValidator } from '@angular/forms';
import { AppService } from '../shared/service/AppService';
import { Router } from '@angular/router';
import { User } from '../shared/models/User';
import { BehaviorSubject } from 'rxjs';

@Component({
  selector: 'app-user-profile',
  templateUrl: './user-profile.component.html',
  styleUrls: ['./user-profile.component.scss']
})
export class UserProfileComponent implements OnInit {
  @Output() 
  public genres: any[];
  public artists: string[];
  public selectedArtists: any[] = [];
  public selectedArtistsIsDirty: boolean = false;
  public selectedGenres: any[] = [];
  public selectedGenresIsDirty: boolean = false;
  public searchArtist: FormControl = new FormControl();
  private numberCalls = 0;
  private successSubject: BehaviorSubject<number> = new BehaviorSubject<number>(0);
  profileSettingsForm: FormGroup;
  submitted = false;
  
  private usernameValue:string;
  private emailValue:string;
  private searchHandler:any;
  private searchDelay: number = 500;

  constructor(@Inject(MAT_DIALOG_DATA) public data: any,
              private formBuilder: FormBuilder,
              private service:AppService,
              private snackBar: MatSnackBar,
              private router:Router,
              private dialogRef: MatDialogRef<UserProfileComponent>) { 
                this.successSubject.asObservable().subscribe(() =>
                  {
                    if (this.numberCalls > 0 && this.successSubject.value === this.numberCalls) {
                      this.dialogRef.close();
                    }
                  }
                )
              }

  ngOnInit() {
    $("#closeDialog").blur()
    this.profileSettingsForm = this.formBuilder.group({
      username: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]]
    });
    this.setUserData();
    this.populateData();
  }

  populateData(){
    if(this.data.allGenres)
      this.genres = this.data.allGenres;
    else
      this.getGenres();

    if(this.data.userArtists)
      this.selectedArtists = this.data.userArtists;
    else
      this.closeDialogAndShowError();

    if(this.data.userGenres)
      this.selectedGenres = this.data.userGenres;
    else
      this.closeDialogAndShowError();
  }

  closeDialogAndShowError(){
    this.dialogRef.close();
    this.openSnackBar("An error has occured!");
    location.reload();
  }

  setUserData(){
    let currentUser = JSON.parse(localStorage.getItem('currentUser'));
    if(currentUser)
    {
      this.profileSettingsForm.get('username').setValue(currentUser.Username);
      this.profileSettingsForm.get('email').setValue(currentUser.Email);
    }
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
        if(artists.artists)
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
        {
          this.selectedArtistsIsDirty = true;
          this.selectedArtists.push({"ArtistID":artistID,"Name":artistName});
        }
      }
  }

  unselectArtist(index:number){
    if (index >= 0 && index < this.selectedArtists.length) {
        this.selectedArtistsIsDirty = true;
        this.selectedArtists.splice(index, 1);
    }    
  }

  get f() { return this.profileSettingsForm.controls; }

  validateAndSave(){
    this.submitted = true;

    if (this.profileSettingsForm.invalid) {
        return;
    }

    if(this.selectedArtists.length == 0 && this.selectedGenres.length == 0)
    {
      this.openSnackBar("You need to select at least one genre or artist");
      return;
    }

    let currentUser = JSON.parse(localStorage.getItem('currentUser'));
    if(currentUser)
    {
      this.successSubject.next(0);
      this.numberCalls == 0;
      this.updateUserInformation(currentUser.ID);
      this.updatePreferences(currentUser.ID);
    }
    else
    {
      this.openSnackBar("An error has occured!");
      this.service.logout();
      this.router.navigate([]);
    }
  }

  updateUserInformation(userID:string){
    this.usernameValue = this.profileSettingsForm.get('username').value;
    this.emailValue = this.profileSettingsForm.get('email').value;

    this.numberCalls++;
    this.service.updateUser(userID,this.usernameValue,this.emailValue).subscribe(()=>{
      this.successSubject.next(this.successSubject.value + 1);
      this.updateLocalUser();
    });
  }

  updateLocalUser(){
    let currentUser = JSON.parse(localStorage.getItem('currentUser'));
    if(currentUser)
    {
      var user = new User();
      user.ID = currentUser.ID;
      user.Username = this.usernameValue;
      user.Email = this.emailValue
      user.Token = currentUser.Token;

      localStorage.setItem('currentUser', JSON.stringify(user));
    }
    else
    {
      this.openSnackBar("An error has occured!");
      this.service.logout();
      this.router.navigate([]);
    }
  }
       
  updatePreferences(userID:string)
  {
    if(this.selectedArtists.length > 0 || this.selectedArtistsIsDirty == true)
    {
      this.numberCalls++;
      this.service.insertUserArtists(userID, this.selectedArtists).subscribe(()=>{
        this.successSubject.next(this.successSubject.value + 1);
      });
    }

    if(this.selectedGenres.length > 0 || this.selectedGenresIsDirty == true)
    {
      this.numberCalls++;
      this.service.insertUserGenres(userID, this.selectedGenres).subscribe(()=>{
        this.successSubject.next(this.successSubject.value + 1);
      });
    }
  }

  private openSnackBar(message: string) {
    this.snackBar.open(message, '', {verticalPosition: "top", duration: 5000});
  }

  genreCheckboxChange(checked:boolean,genreName:string){
    genreName = genreName.charAt(0).toUpperCase() + genreName.slice(1);
    if(checked == true)
    {
      if(this.selectedGenres.indexOf(genreName) == -1)
      {
        this.selectedGenresIsDirty = true;
        this.selectedGenres.push(genreName)
      }
    }
    else
    {
      var index = this.selectedGenres.indexOf(genreName) ;
      if(index != -1)
      {
        this.selectedGenresIsDirty = true;
        this.selectedGenres.splice(index, 1);
      }
    }
  }

  isChecked(genre:string)
  {
    if(this.selectedGenres.indexOf(genre) != -1)
      return true;
    return false;
  }
}
