import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SimSimCreateComponent } from './sim-sim-create.component';

describe('SimSimCreateComponent', () => {
  let component: SimSimCreateComponent;
  let fixture: ComponentFixture<SimSimCreateComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SimSimCreateComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SimSimCreateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
