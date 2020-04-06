import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SimSimStartupComponent } from './sim-sim-startup.component';

describe('SimSimStartupComponent', () => {
  let component: SimSimStartupComponent;
  let fixture: ComponentFixture<SimSimStartupComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SimSimStartupComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SimSimStartupComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
