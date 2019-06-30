import { Component, OnInit, Input, SimpleChanges, Output, EventEmitter, HostListener, ViewChild, ElementRef } from '@angular/core';
import { MatSliderChange } from '@angular/material';
import { AnimationBuilder, animate, style } from '@angular/animations';
import { Subscription } from 'rxjs/internal/Subscription';
import { Observable } from 'rxjs/internal/Observable';
import { interval } from 'rxjs';
import { AppService, ProgressTimeout } from '../shared/service/AppService';
import { environment } from '../../environments/environment';

@Component({
  selector: 'app-music-player',
  templateUrl: './music-player.component.html',
  styleUrls: ['./music-player.component.scss']
})

export class MusicPlayerComponent{
    @Input() videoId : string;
    @Input() title : string;
    @Input() artists : string[];
    @Output() videoEmitter : EventEmitter<any> = new EventEmitter<any>();

    @ViewChild('progressBar') progressBar: ElementRef;

    private isPlaying : boolean = true;
    private looping : boolean = false;
    private totalDuration: number = 0;
    private curentDuration: number = 0;
    private videoStarted:boolean = false;
    private step:number = 1;
    public currentTime: string = "00:00";

    playerVars = {
      autoplay: "1"
    };

    private player : any;
    private ytEvent : any;

    constructor(private timeoutManager:ProgressTimeout){
      this.timeoutManager.unsubscribe();
    }

    clearTimeout(){
      
    }

    ngOnInit() {
      this.timeoutManager.unsubscribe();
    }

    ngOnChanges(changes: SimpleChanges): void {
      if(changes['videoId']) {
        this.timeoutManager.unsubscribe();
        if(this.player && this.videoId)
        {
          this.videoStarted = false;
          this.timeoutManager.unsubscribe();
          this.player.loadVideoById(this.videoId);
        }
      }
    }

    onStateChange(event) {
      this.ytEvent = event.data;
      if (event.data == YT.PlayerState.UNSTARTED) {
        this.videoStarted = false;
        this.timeoutManager.unsubscribe();
        $("#progress-bar").css("width","0%");
      }
      else if (event.data == YT.PlayerState.PLAYING) {
        environment.isVideoPaused = false;
        if(this.videoStarted == false)
        {
          environment.currentSeconds = 0;
          this.timeoutManager.unsubscribe();
          this.videoStarted = true;
          this.totalDuration = this.player.getDuration();
          this.step = (100/this.totalDuration);
          this.playVideo();
        }
      }
      else if (event.data == YT.PlayerState.PAUSED) {
          this.timeoutManager.unsubscribe();
          environment.isVideoPaused = true;
      }
      else if(event.data == YT.PlayerState.ENDED){
      {
          this.timeoutManager.unsubscribe();
          this.videoStarted = false;

          if(this.looping == false)
          {
            this.videoEmitter.emit("end");
          }
          else
          {
            $("#progress-bar").css("width","0%");
            this.playVideo();
          }
      }
      }
    }

    playAudio(){
      if(!this.videoStarted) {
        this.timeoutManager.unsubscribe();
        return;
      }
      if(this.isPlaying)
        this.pauseVideo();
      else
        this.playVideo();
    }

    savePlayer(player) {
      this.player = player;
      this.setVolume(50);
      this.timeoutManager.unsubscribe();
    }

    setVolumeMouseMove(event:any){
      if(!this.videoStarted) {
        this.timeoutManager.unsubscribe();
        return;
      }
      var volumeLevel = $('#volumeLevel')
      if(volumeLevel.is(':active'))
      {
        volumeLevel.find('em').css('width', event.pageX - volumeLevel.offset().left);
        var vol = volumeLevel.find('em').width() / volumeLevel.width() * 100;
        this.setVolume(vol);
      }
    }

    setVolumeMouseClick(event:any){
      if(!this.videoStarted) {
        this.timeoutManager.unsubscribe();
        return;
      }
      if(this.player.isMuted())
      {
        $("#volume").toggleClass('fa-volume-up fa-volume-off');
        this.player.unMute();
      }
      var volumeLevel = $('#volumeLevel')
      volumeLevel.find('em').css('width', event.pageX - volumeLevel.offset().left);
      var vol = volumeLevel.find('em').width() / volumeLevel.width() * 100;
      this.setVolume(vol);
    }

    playVideo() {
      if(!this.videoStarted && this.looping == false) {
        this.timeoutManager.unsubscribe();
        return;
      }
      environment.currentSeconds = 0;
      environment.isVideoPaused = false;
      this.configurePlay();
      this.player.playVideo();
    }
    
    configurePlay(){
      if(!this.videoStarted && this.looping == false) {
        this.timeoutManager.unsubscribe();
        return;
      }
      $("#playBtn").hide();
      $("#pauseBtn").show();
      this.isPlaying = true;
      this.timeoutManager.unsubscribe();
      environment.currentSeconds = 0;

      if(!this.timeoutManager.getTimeout() && environment.attached == false)
      {
        this.timeoutManager.subscribe(()=>{   
          if(environment.isVideoPaused == true)
          {
            this.timeoutManager.unsubscribe();
          }
          else if(environment.currentSeconds != 0 &&
            (environment.currentSeconds + 1) % 60 != new Date().getUTCSeconds())
          {
            this.timeoutManager.unsubscribe();
          }
          else     
          {
            var previousWidth = $("#progress-bar").width() / $('#progress-bar').parent().width() * 100;
            var currentWidth = previousWidth + this.step + "%";
            $("#progress-bar").css("width",currentWidth);
            environment.currentSeconds = new Date().getUTCSeconds();
          }
        });
      }
    }

    pauseVideo() {
      if(!this.videoStarted) {
        this.timeoutManager.unsubscribe();
        return;
      }
      environment.isVideoPaused = true;
      this.configurePause();
      this.player.pauseVideo();
    }

    configurePause(){
      if(!this.videoStarted) {
        this.timeoutManager.unsubscribe();
        return;
      }
      $("#pauseBtn").hide();
      $("#playBtn").show();
      this.isPlaying = false;
      this.timeoutManager.unsubscribe();
    }
    
    stopVideo() {
      if(!this.videoStarted) {
        this.timeoutManager.unsubscribe();
        return;
      }
      this.configurePause();
      this.player.stopVideo();
    }

    nextVideo(){
      this.videoEmitter.emit("next");
    }

    previousVideo(){
      this.videoEmitter.emit("prev");
    }

    setLoop(){
      if(!this.videoStarted) {
        this.timeoutManager.unsubscribe();
        return;
      }
      this.looping = !this.looping;
      this.player.setLoop(this.looping);
      if(this.looping)
        $("#loopImg").css("opacity","1");
      else
        $("#loopImg").css("opacity","0.5");
    }

    setSeekToMouseClick(event:any){
      if(!this.videoStarted) {
        this.timeoutManager.unsubscribe();
        return;
      }
      var progress = $('#progress');
      var progressBar = $("#progress-bar");
      progressBar.css('width', event.pageX - progress.offset().left);
      var width = progressBar.width() / progressBar.parent().width() * 100;
      this.seekTo(width / this.step);
    }

    setSeekToMouseMove(event:any){
      if(!this.videoStarted) {
        this.timeoutManager.unsubscribe();
        return;
      }

      var progress = $('#progress')
      var timeSpan = $('#currentTime')

      var timeWidth = event.pageX - progress.offset().left;
      timeSpan.css('left', timeWidth - 18 + "px");
      var timeWidthPercentage = (timeWidth / progress.width() * 100) / this.step;
      this.currentTime = this.formatTime(timeWidthPercentage);

      if(progress.is(':active'))
      {
        var progressBar = $("#progress-bar");
        progressBar.css('width', event.pageX - progress.offset().left);
        var width = progressBar.width() / progressBar.parent().width() * 100;
        this.seekTo(width / this.step);
      }
    }

    seekTo(seconds:Number)
    {
        this.player.seekTo(seconds, true);
    }

    muteVideo(){
      if(!this.videoStarted) {
        this.timeoutManager.unsubscribe();
        return;
      }
      $("#volume").toggleClass('fa-volume-up fa-volume-off');
      if(this.player.isMuted())
        this.player.unMute();
      else
        this.player.mute();
    }

    setVolume(volume: number)
    {
      if(volume <= 2 && $("#volume").hasClass("fa-volume-up"))
        $("#volume").toggleClass('fa-volume-up fa-volume-off');
      else if(volume > 0 && $("#volume").hasClass("fa-volume-off"))
        $("#volume").toggleClass('fa-volume-up fa-volume-off');
      this.player.setVolume(volume);
    }

    hideCurrentTime(){
      $("#currentTime").hide();
    }

    showCurrentTime(event:any){
      $("#currentTime").show();
    }

    formatTime(currentTime:number):string{
      if(currentTime < 0)
      {
        $("#currentTime").hide();
        return;
      }
      var hours   = Math.floor(currentTime / 3600);
      var minutes:any = Math.floor((currentTime - (hours * 3600)) / 60);
      var seconds:any = Math.floor(currentTime - (hours * 3600) - (minutes * 60));

      if (minutes < 10) {minutes = "0"+minutes;}
      if (seconds < 10) {seconds = "0"+seconds;}
      return minutes+':'+seconds;
    }
}
