import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ConferenceMenuComponent } from './conference-menu.component';

describe('ConferenceMenuComponent', () => {
  let component: ConferenceMenuComponent;
  let fixture: ComponentFixture<ConferenceMenuComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ConferenceMenuComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ConferenceMenuComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
