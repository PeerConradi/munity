import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CreateConferenceComponent } from './create-conference.component';
import { FormBuilder } from '@angular/forms';

describe('CreateConferenceComponent', () => {
  let component: CreateConferenceComponent;
  //let fixture: ComponentFixture<CreateConferenceComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CreateConferenceComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    //fixture = TestBed.createComponent(CreateConferenceComponent);
    //component = fixture.componentInstance;
    //fixture.detectChanges();
    let formBuild = new FormBuilder();
    //let spy = spyOn(formBuild, 'group');
    component = new CreateConferenceComponent(formBuild, null, null, null);
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
