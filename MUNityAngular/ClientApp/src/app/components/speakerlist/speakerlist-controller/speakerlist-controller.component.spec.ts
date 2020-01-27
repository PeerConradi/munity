import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SpeakerlistControllerComponent } from './speakerlist-controller.component';

describe('SpeakerlistControllerComponent', () => {
  let component: SpeakerlistControllerComponent;
  let fixture: ComponentFixture<SpeakerlistControllerComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SpeakerlistControllerComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SpeakerlistControllerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
