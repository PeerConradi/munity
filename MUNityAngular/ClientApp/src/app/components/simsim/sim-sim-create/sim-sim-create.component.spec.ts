import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SimSimCreateComponent } from './sim-sim-create.component';
import { FormBuilder } from '@angular/forms';

describe('SimSimCreateComponent', () => {
  let component: SimSimCreateComponent;
  //let fixture: ComponentFixture<SimSimCreateComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SimSimCreateComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    //fixture = TestBed.createComponent(SimSimCreateComponent);
    //component = fixture.componentInstance;
    //fixture.detectChanges();
    component = new SimSimCreateComponent(new FormBuilder(), null, null);
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
