import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SimSimControlsComponent } from './sim-sim-controls.component';

describe('SimSimControlsComponent', () => {
  let component: SimSimControlsComponent;
  //let fixture: ComponentFixture<SimSimControlsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SimSimControlsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    //fixture = TestBed.createComponent(SimSimControlsComponent);
    //component = fixture.componentInstance;
    //fixture.detectChanges();
    component = new SimSimControlsComponent(null, null);
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
