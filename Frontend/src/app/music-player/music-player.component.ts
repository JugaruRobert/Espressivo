import { Component, OnInit, Input, SimpleChanges, Output, EventEmitter, HostListener, ViewChild, ElementRef } from '@angular/core';
import { MatSliderChange } from '@angular/material';
import { AnimationBuilder, animate, style } from '@angular/animations';

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
    private progressTimeout: any;
    private videoStarted:boolean = false;
    private step:number = 1;
    public currentTime: string = "00:00";

    playerVars = {
      autoplay: "1"
    };

    private player : any;
    private ytEvent : any;

    constructor(){}

    ngOnChanges(changes: SimpleChanges): void {
      if(changes['videoId']) {
        if(this.player && this.videoId)
        {
          this.videoStarted = false;
          clearInterval(this.progressTimeout);
          this.player.loadVideoById(this.videoId);
        }
      }
    }

    onStateChange(event) {
      this.ytEvent = event.data;
      if (event.data == YT.PlayerState.UNSTARTED) {
        this.videoStarted = false;
        clearInterval(this.progressTimeout);
        $("#progress-bar").css("width","0%");
      }
      else if (event.data == YT.PlayerState.PLAYING) {
        if(this.videoStarted == false)
        {
          this.videoStarted = true;
          this.totalDuration = this.player.getDuration();
          this.step = (100/this.totalDuration);
          this.playVideo();
        }
      }
      else if(event.data == YT.PlayerState.ENDED){
      {
          clearInterval(this.progressTimeout);
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
      if(this.isPlaying)
        this.pauseVideo();
      else
        this.playVideo();
    }

    savePlayer(player) {
      this.player = player;
      this.setVolume(50);
    }

    setVolumeMouseMove(event:any){
      if(!this.videoStarted)
        return;
      var volumeLevel = $('#volumeLevel')
      if(volumeLevel.is(':active'))
      {
        volumeLevel.find('em').css('width', event.pageX - volumeLevel.offset().left);
        var vol = volumeLevel.find('em').width() / volumeLevel.width() * 100;
        this.setVolume(vol);
      }
    }

    setVolumeMouseClick(event:any){
      if(!this.videoStarted)
        return;
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
      this.configurePlay();
      this.player.playVideo();
    }
    
    configurePlay(){
      if(!this.videoStarted)
        return;
      $("#playBtn").hide();
      $("#pauseBtn").show();
      this.isPlaying = true;
      clearInterval(this.progressTimeout);
      this.progressTimeout = setInterval(() =>{
        var previousWidth = $("#progress-bar").width() / $('#progress-bar').parent().width() * 100;
        var currentWidth = previousWidth + this.step + "%"
        $("#progress-bar").css("width",currentWidth);
      },1000);
    }

    pauseVideo() {
      this.configurePause();
      this.player.pauseVideo();
    }

    configurePause(){
      if(!this.videoStarted)
        return;
      $("#pauseBtn").hide();
      $("#playBtn").show();
      this.isPlaying = false;
      clearInterval(this.progressTimeout);
    }
    
    stopVideo() {
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
      if(!this.videoStarted)
        return;
      this.looping = !this.looping;
      this.player.setLoop(this.looping);
      if(this.looping)
        $("#loopImg").css("opacity","1");
      else
        $("#loopImg").css("opacity","0.5");
    }

    setSeekToMouseClick(event:any){
      if(!this.videoStarted)
        return;
      var progress = $('#progress');
      var progressBar = $("#progress-bar");
      progressBar.css('width', event.pageX - progress.offset().left);
      var width = progressBar.width() / progressBar.parent().width() * 100;
      this.seekTo(width / this.step);
    }

    setSeekToMouseMove(event:any){
      if(!this.videoStarted)
        return;

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
      if(!this.videoStarted)
        return;
      $("#volume").toggleClass('fa-volume-up fa-volume-off');
      if(this.player.isMuted())
        this.player.unMute();
      else
        this.player.mute();
    }

    setVolume(volume: number)
    {
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
