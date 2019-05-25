import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { MusicCarouselComponent } from './music-carousel.component';

describe('MusicCarouselComponent', () => {
  let component: MusicCarouselComponent;
  let fixture: ComponentFixture<MusicCarouselComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ MusicCarouselComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(MusicCarouselComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
