import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { ActivatedRoute } from '@angular/router';
import { CommitteeDetailsComponent } from './committee-details.component';
import { AppComponent } from "../../../app.component";
import { RouterModule } from '@angular/router';
import { ResolutionService } from "../../../services/resolution.service";
import { ConferenceService } from "../../../services/conference-service.service";
import { HttpClient } from '@angular/common/http';

describe('CommitteeDetailsComponent', () => {
  let component: CommitteeDetailsComponent;
  let fixture: ComponentFixture<CommitteeDetailsComponent>;
  let router = new RouterModule(null, null);
  let activatedRoute = new ActivatedRoute();
  let httpClient = new HttpClient(null);
  let resolutionService = new ResolutionService(httpClient, null, null, null);
  let conferenceService = new ConferenceService(null, null, null);

  beforeEach(async(() => {
    TestBed.configureTestingModule({
        declarations: [  CommitteeDetailsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    //fixture = TestBed.createComponent(CommitteeDetailsComponent);
    //component = fixture.componentInstance;
    component = new CommitteeDetailsComponent(activatedRoute, resolutionService, conferenceService);
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
