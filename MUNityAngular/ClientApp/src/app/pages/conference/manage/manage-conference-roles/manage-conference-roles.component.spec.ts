import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ManageConferenceRolesComponent } from './manage-conference-roles.component';

describe('ManageConferenceRolesComponent', () => {
  let component: ManageConferenceRolesComponent;
  //let fixture: ComponentFixture<ManageConferenceRolesComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ManageConferenceRolesComponent]
    })
      .compileComponents();
  }));

  beforeEach(() => {
    // fixture = TestBed.createComponent(ManageConferenceRolesComponent);
    // component = fixture.componentInstance;
    // fixture.detectChanges();
    component = new ManageConferenceRolesComponent(null, null);
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
