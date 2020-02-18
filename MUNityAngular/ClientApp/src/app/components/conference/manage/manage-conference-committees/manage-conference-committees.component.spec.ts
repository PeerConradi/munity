import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ManageConferenceCommitteesComponent } from './manage-conference-committees.component';

describe('ManageConferenceCommitteesComponent', () => {
  let component: ManageConferenceCommitteesComponent;
  let fixture: ComponentFixture<ManageConferenceCommitteesComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ManageConferenceCommitteesComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ManageConferenceCommitteesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
