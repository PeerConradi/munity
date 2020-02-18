import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ManageConferenceTeamComponent } from './manage-conference-team.component';

describe('ManageConferenceTeamComponent', () => {
  let component: ManageConferenceTeamComponent;
  let fixture: ComponentFixture<ManageConferenceTeamComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ManageConferenceTeamComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ManageConferenceTeamComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
