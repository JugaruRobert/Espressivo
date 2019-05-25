import { Component, OnInit, Input, ViewChild, EventEmitter, SimpleChanges, HostListener } from '@angular/core';
import { CarouselComponent } from 'angular2-carousel';
import { AppService } from '../shared/service/AppService';

export enum KEY_CODE {
  UP_ARROW = 38,
  DOWN_ARROW = 40,
  RIGHT_ARROW = 39,
  LEFT_ARROW = 37
}

@Component({
  selector: 'app-music-carousel',
  templateUrl: './music-carousel.component.html',
  styleUrls: ['./music-carousel.component.scss']
})

export class MusicCarouselComponent implements OnInit {
  @HostListener('window:keyup', ['$event'])
  keyEvent(event: KeyboardEvent) {
    if(event.keyCode == KEY_CODE.LEFT_ARROW){
      if(this.tracks.length > 0)
        this.carousel.slidePrev();
    }

    if(event.keyCode == KEY_CODE.RIGHT_ARROW){
      if(this.tracks.length > 0)
        this.carousel.slideNext();
    }
  }
  
  public slideTransitionFinished:any;
  public currentIndex = -1;
  @Input() tracks : any[];
  
  constructor() {
  }

  @ViewChild('carousel') carousel: CarouselComponent;

  ngOnChanges(changes: SimpleChanges): void {
    if(changes['tracks']) {
      setTimeout(()=>{   
        this.carousel.reInit();
        this.currentIndex = 0;
      }, 0);
    }
  }
    
  ngOnInit() {
  }

  slideChange(event:any){
    if(this.currentIndex == event.activeIndex)
      return;
    clearTimeout(this.slideTransitionFinished);
    this.slideTransitionFinished = setTimeout(()=>{this.currentIndex = event.activeIndex;},500);
  }

  goToSlide(index: number){
    this.carousel.slideTo(index);
    this.currentIndex = index;
  }
}