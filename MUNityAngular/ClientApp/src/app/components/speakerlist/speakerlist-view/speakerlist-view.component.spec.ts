import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SpeakerlistViewComponent } from './speakerlist-view.component';

describe('SpeakerlistViewComponent', () => {
  let component: SpeakerlistViewComponent;
  let fixture: ComponentFixture<SpeakerlistViewComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SpeakerlistViewComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SpeakerlistViewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
