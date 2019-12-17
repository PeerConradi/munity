import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SignalrtestComponent } from './signalrtest.component';

describe('SignalrtestComponent', () => {
  let component: SignalrtestComponent;
  let fixture: ComponentFixture<SignalrtestComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SignalrtestComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SignalrtestComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
