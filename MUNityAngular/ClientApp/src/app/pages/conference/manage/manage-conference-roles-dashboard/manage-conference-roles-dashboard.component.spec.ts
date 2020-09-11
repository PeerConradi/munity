import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ManageConferenceRolesDashboardComponent } from './manage-conference-roles-dashboard.component';

describe('ManageConferenceRolesDashboardComponent', () => {
  let component: ManageConferenceRolesDashboardComponent;
  let fixture: ComponentFixture<ManageConferenceRolesDashboardComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ManageConferenceRolesDashboardComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ManageConferenceRolesDashboardComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
