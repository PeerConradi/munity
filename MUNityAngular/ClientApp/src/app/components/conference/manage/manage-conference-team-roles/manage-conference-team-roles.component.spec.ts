import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ManageConferenceTeamRolesComponent } from './manage-conference-team-roles.component';

describe('ManageConferenceTeamRolesComponent', () => {
  let component: ManageConferenceTeamRolesComponent;
  //let fixture: ComponentFixture<ManageConferenceTeamRolesComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ManageConferenceTeamRolesComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    //fixture = TestBed.createComponent(ManageConferenceTeamRolesComponent);
    //component = fixture.componentInstance;
    //fixture.detectChanges();
    component = new ManageConferenceTeamRolesComponent(null, null, null, null);
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
