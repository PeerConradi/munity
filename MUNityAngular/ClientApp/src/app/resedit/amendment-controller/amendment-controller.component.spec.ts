import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AmendmentControllerComponent } from './amendment-controller.component';

describe('AmendmentControllerComponent', () => {
  let component: AmendmentControllerComponent;
  let fixture: ComponentFixture<AmendmentControllerComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AmendmentControllerComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AmendmentControllerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
