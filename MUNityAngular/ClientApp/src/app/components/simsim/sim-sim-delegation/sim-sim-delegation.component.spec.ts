import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SimSimDelegationComponent } from './sim-sim-delegation.component';

describe('SimSimDelegationComponent', () => {
  let component: SimSimDelegationComponent;
  //let fixture: ComponentFixture<SimSimDelegationComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SimSimDelegationComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    //fixture = TestBed.createComponent(SimSimDelegationComponent);
    component = new SimSimDelegationComponent(null);
    //fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
