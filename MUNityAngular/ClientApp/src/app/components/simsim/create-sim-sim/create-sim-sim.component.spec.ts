import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CreateSimSimComponent } from './create-sim-sim.component';

describe('CreateSimSimComponent', () => {
  let component: CreateSimSimComponent;
  let fixture: ComponentFixture<CreateSimSimComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CreateSimSimComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CreateSimSimComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
