import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SpeakerlistStartupComponent } from './speakerlist-startup.component';

describe('SpeakerlistStartupComponent', () => {
  let component: SpeakerlistStartupComponent;
  let fixture: ComponentFixture<SpeakerlistStartupComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SpeakerlistStartupComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SpeakerlistStartupComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
