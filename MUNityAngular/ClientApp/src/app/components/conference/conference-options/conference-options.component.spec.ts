import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ConferenceOptionsComponent } from './conference-options.component';

describe('ConferenceOptionsComponent', () => {
  let component: ConferenceOptionsComponent;
  let fixture: ComponentFixture<ConferenceOptionsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ConferenceOptionsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ConferenceOptionsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
