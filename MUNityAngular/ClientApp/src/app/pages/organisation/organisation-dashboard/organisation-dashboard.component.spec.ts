import { ComponentFixture, TestBed } from '@angular/core/testing';

import { OrganisationDashboardComponent } from './organisation-dashboard.component';

describe('OrganisationDashboardComponent', () => {
  let component: OrganisationDashboardComponent;
  let fixture: ComponentFixture<OrganisationDashboardComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ OrganisationDashboardComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(OrganisationDashboardComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
