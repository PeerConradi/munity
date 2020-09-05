import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SpeakerlistControllerComponent } from './speakerlist-controller.component';
import { ConferenceService } from "../../../services/conference-service.service";
import { SpeakerListService } from "../../../services/speaker-list.service";

describe('SpeakerlistControllerComponent', () => {
  let component: SpeakerlistControllerComponent;
  //let fixture: ComponentFixture<SpeakerlistControllerComponent>;
  let conferenceService = new ConferenceService(null, null, '');
  let speakerlistService = new SpeakerListService(null, '', null)

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SpeakerlistControllerComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    //fixture = TestBed.createComponent(SpeakerlistControllerComponent);
    component = new SpeakerlistControllerComponent(conferenceService, speakerlistService, null, null);
    //fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
