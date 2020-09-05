import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SimSimViewComponent } from './sim-sim-view.component';

describe('SimSimViewComponent', () => {
  let component: SimSimViewComponent;
 // let fixture: ComponentFixture<SimSimViewComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SimSimViewComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    //fixture = TestBed.createComponent(SimSimViewComponent);
    component = new SimSimViewComponent(null, null, null, null, null, null);
    //fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
