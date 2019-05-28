import { Component, OnInit, Input, SimpleChanges, Output, EventEmitter } from '@angular/core';
import { MatSliderChange } from '@angular/material';

@Component({
  selector: 'app-music-player',
  templateUrl: './music-player.component.html',
  styleUrls: ['./music-player.component.scss']
})

export class MusicPlayerComponent{
    @Input() videoId :string;
    @Output() endVideoEmitter : EventEmitter<any> = new EventEmitter<any>();

    playerVars = {
      autoplay: "1"
    };

    private player : any;
    private ytEvent : any;

    constructor(){
    }

    ngOnChanges(changes: SimpleChanges): void {
      if(changes['videoId']) {
        if(this.player && this.videoId)
          this.player.loadVideoById(this.videoId);
      }
    }

    onStateChange(event) {
      this.ytEvent = event.data;
      if (event.data == YT.PlayerState.UNSTARTED) {
        this.playVideo();
      }
      else if(event.data == YT.PlayerState.ENDED){
          this.endVideoEmitter.emit("videoEnded");
      }
    }

    savePlayer(player) {
      this.player = player;
    }
    
    playVideo() {
      this.player.playVideo();
    }
    
    pauseVideo() {
      this.player.pauseVideo();
    }

    stopVideo() {
      this.player.stopVideo();
    }

    setLoop(loop:boolean){
      this.player.setLoop(loop);
    }

    seekTo(seconds:Number)
    {
        this.player.seekTo(seconds, true);
    }

    muteVideo(){
      if(this.player.isMuted())
        this.player.unMute();
      else
        this.player.mute();
    }

    setVolume(event: MatSliderChange)
    {
      this.player.setVolume(event.value * 10);
    }
}
