import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { FormGroup, Validators, FormBuilder, FormControl } from '@angular/forms';
import { AppService } from '../shared/service/AppService';

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
  public searchArtist: FormControl = new FormControl();

  private searchHandler:any;
  private searchDelay: number = 500;
  
  constructor(private service:AppService) { }

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
      this.selectedArtists.push({"artistName":artistName,"artistID":artistID});
  }

  unselectArtist(index:number){
    if (index >= 0 && index < this.selectedArtists.length) {
        this.selectedArtists.splice(index, 1);
    }    
  }
}
