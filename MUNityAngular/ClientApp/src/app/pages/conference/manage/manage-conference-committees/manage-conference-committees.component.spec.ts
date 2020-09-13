import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { FormBuilder } from '@angular/forms';

import { ManageConferenceCommitteesComponent } from './manage-conference-committees.component';

describe('ManageConferenceCommitteesComponent', () => {
  let component: ManageConferenceCommitteesComponent;
  //let fixture: ComponentFixture<ManageConferenceCommitteesComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ManageConferenceCommitteesComponent]
    })
      .compileComponents();
  }));

  beforeEach(() => {
    //fixture = TestBed.createComponent(ManageConferenceCommitteesComponent);
    //component = fixture.componentInstance;
    //fixture.detectChanges();
    let formBuilder = new FormBuilder();
    component = new ManageConferenceCommitteesComponent(null, null, null, null, null, formBuilder);
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
