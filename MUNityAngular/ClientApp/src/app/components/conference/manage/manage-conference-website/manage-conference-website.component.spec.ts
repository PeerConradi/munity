import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ManageConferenceWebsiteComponent } from './manage-conference-website.component';

describe('ManageConferenceWebsiteComponent', () => {
  let component: ManageConferenceWebsiteComponent;
  let fixture: ComponentFixture<ManageConferenceWebsiteComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ManageConferenceWebsiteComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ManageConferenceWebsiteComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
