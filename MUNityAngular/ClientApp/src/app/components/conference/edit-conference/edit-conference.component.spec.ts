import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { EditConferenceComponent } from './edit-conference.component';

describe('EditConferenceComponent', () => {
  let component: EditConferenceComponent;
  //let fixture: ComponentFixture<EditConferenceComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ EditConferenceComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    //fixture = TestBed.createComponent(EditConferenceComponent);
    //component = fixture.componentInstance;
    //fixture.detectChanges();
    component = new EditConferenceComponent(null, null, null);
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
