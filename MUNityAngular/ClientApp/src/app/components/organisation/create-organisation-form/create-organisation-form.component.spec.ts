import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CreateOrganisationFormComponent } from './create-organisation-form.component';

describe('CreateOrganisationFormComponent', () => {
  let component: CreateOrganisationFormComponent;
  let fixture: ComponentFixture<CreateOrganisationFormComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CreateOrganisationFormComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(CreateOrganisationFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
