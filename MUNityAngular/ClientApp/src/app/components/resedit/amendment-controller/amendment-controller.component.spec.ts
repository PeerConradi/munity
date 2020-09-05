import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AmendmentControllerComponent } from './amendment-controller.component';
import { ResolutionService } from "../../../services/resolution.service";

describe('AmendmentControllerComponent', () => {
  let component: AmendmentControllerComponent;
  let resaService = new ResolutionService(null, null, null, '');

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AmendmentControllerComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    //fixture = TestBed.createComponent(AmendmentControllerComponent);
    component = new AmendmentControllerComponent(resaService);
    //fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
