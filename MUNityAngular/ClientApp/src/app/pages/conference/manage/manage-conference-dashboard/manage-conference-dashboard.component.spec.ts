import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ManageConferenceDashboardComponent } from './manage-conference-dashboard.component';

describe('ManageConferenceDashboardComponent', () => {
  let component: ManageConferenceDashboardComponent;
  let fixture: ComponentFixture<ManageConferenceDashboardComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ManageConferenceDashboardComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ManageConferenceDashboardComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
