import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SpeakerlistPanelComponent } from './speakerlist-panel.component';

describe('SpeakerlistPanelComponent', () => {
  let component: SpeakerlistPanelComponent;
  let fixture: ComponentFixture<SpeakerlistPanelComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SpeakerlistPanelComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SpeakerlistPanelComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
