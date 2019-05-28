import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { FirstLoginInformationComponent } from './first-login-information.component';

describe('FirstLoginInformationComponent', () => {
  let component: FirstLoginInformationComponent;
  let fixture: ComponentFixture<FirstLoginInformationComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ FirstLoginInformationComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(FirstLoginInformationComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
