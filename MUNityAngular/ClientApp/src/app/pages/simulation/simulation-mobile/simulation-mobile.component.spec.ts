import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SimulationMobileComponent } from './simulation-mobile.component';

describe('SimulationMobileComponent', () => {
  let component: SimulationMobileComponent;
  let fixture: ComponentFixture<SimulationMobileComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SimulationMobileComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SimulationMobileComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
