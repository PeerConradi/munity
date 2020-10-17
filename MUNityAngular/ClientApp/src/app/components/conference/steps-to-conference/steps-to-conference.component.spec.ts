import { ComponentFixture, TestBed } from '@angular/core/testing';

import { StepsToConferenceComponent } from './steps-to-conference.component';

describe('StepsToConferenceComponent', () => {
  let component: StepsToConferenceComponent;
  let fixture: ComponentFixture<StepsToConferenceComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ StepsToConferenceComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(StepsToConferenceComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
