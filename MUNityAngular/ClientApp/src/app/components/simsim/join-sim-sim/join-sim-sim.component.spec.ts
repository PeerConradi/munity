import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { JoinSimSimComponent } from './join-sim-sim.component';

describe('JoinSimSimComponent', () => {
  let component: JoinSimSimComponent;
  let fixture: ComponentFixture<JoinSimSimComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ JoinSimSimComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(JoinSimSimComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
