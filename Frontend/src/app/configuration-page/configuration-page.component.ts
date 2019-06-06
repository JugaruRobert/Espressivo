import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { MatSnackBar } from '@angular/material';

@Component({
  selector: 'app-configuration-page',
  templateUrl: './configuration-page.component.html',
  styleUrls: ['./configuration-page.component.scss']
})
export class ConfigurationPageComponent implements OnInit {
  public selectedImage: any;
  public imageFile: any;
  constructor(private snackBar:MatSnackBar) { }

  @Output() getRecommendationEmitter : EventEmitter<any> = new EventEmitter<any>();

  ngOnInit() {
  }

  public uploadImage(files: any) { 
    this.selectedImage = files[0];
    var reader = new FileReader();
    reader.readAsDataURL(files[0]); 
    reader.onload = (_event) => { 
      this.imageFile = reader.result; 
    }    
  }

  removeImage() {
    this.imageFile = null;
  }

  getRecommendations(){
    if(this.imageFile)
    {
      this.getRecommendationEmitter.emit(this.selectedImage);
    }
    else
      this.openSnackBar("Not so fast! You must select an image first.")
  }

  private openSnackBar(message: string) {
    this.snackBar.open(message, '', {verticalPosition: "top", duration: 5000});
  }
}
