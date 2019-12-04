import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ConferenceBoxComponent } from './conference-box.component';

describe('ConferenceBoxComponent', () => {
  let component: ConferenceBoxComponent;
  let fixture: ComponentFixture<ConferenceBoxComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ConferenceBoxComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ConferenceBoxComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
