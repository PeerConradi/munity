import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CreateConferenceComponent } from './create-conference.component';

describe('CreateConferenceComponent', () => {
  let component: CreateConferenceComponent;
  let fixture: ComponentFixture<CreateConferenceComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CreateConferenceComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CreateConferenceComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
