import { Component, OnInit, Output, EventEmitter, ViewEncapsulation } from '@angular/core';
import { MatSnackBar } from '@angular/material';
import { WebcamImage, WebcamUtil, WebcamInitError } from 'ngx-webcam';
import { Subject, Observable } from 'rxjs';

@Component({
  selector: 'app-configuration-page',
  templateUrl: './configuration-page.component.html',
  styleUrls: ['./configuration-page.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class ConfigurationPageComponent implements OnInit {
  public selectedImage: any;
  public imageFile: any;
  public webcamBase64Image:any;
  public showWebcam = false;
  public webcamAvailable : boolean = false;
  private isWebcamImage: boolean = false;
  private trigger: Subject<void> = new Subject<void>();
  
  constructor(private snackBar:MatSnackBar) { }

  @Output() getRecommendationEmitter : EventEmitter<any> = new EventEmitter<any>();

  ngOnInit() {
    WebcamUtil.getAvailableVideoInputs()
    .then((mediaDevices: MediaDeviceInfo[]) => {
      this.webcamAvailable = mediaDevices.length > 0 ? true : false;
    });
  }

  public triggerSnapshot(): void {
    this.trigger.next();
  }

  public get triggerObservable(): Observable<void> {
    return this.trigger.asObservable();
  }

  public handleInitError(error: WebcamInitError): void {
    this.showWebcam = false;
    this.webcamAvailable = false;
  }

  public handleImage(webcamImage: WebcamImage): void {
    this.imageFile = webcamImage.imageAsDataUrl;
    this.webcamBase64Image = webcamImage.imageAsBase64;
    this.isWebcamImage = true;
  }

  private dataURItoBlob(dataURI) {
    const byteString = window.atob(dataURI);
    const arrayBuffer = new ArrayBuffer(byteString.length);
    const int8Array = new Uint8Array(arrayBuffer);
    for (let i = 0; i < byteString.length; i++) {
      int8Array[i] = byteString.charCodeAt(i);
    }
    const blob = new Blob([int8Array], { type: 'image/png' });    
    return blob;
  }

  public toggleWebcam(): void {
    this.imageFile = null;
    this.showWebcam = !this.showWebcam;
  }

  public uploadImage(files: any) { 
    this.isWebcamImage = false;
    this.showWebcam = false;
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
      if(this.isWebcamImage){
        var imageBlob = this.dataURItoBlob(this.webcamBase64Image);
        this.selectedImage = new File([imageBlob], 'imageName', { type: 'image/png' });
      }   
      this.getRecommendationEmitter.emit(this.selectedImage);
    }
    else
      this.openSnackBar("Not so fast! You must add an image first.")
  }

  private openSnackBar(message: string) {
    this.snackBar.open(message, '', {verticalPosition: "top", duration: 5000});
  }
}
