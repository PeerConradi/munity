import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { WorkWithResolutionComponent } from './work-with-resolution.component';
import { ResolutionService } from "../../../services/resolution.service";

describe('WorkWithResolutionComponent', () => {
  let component: WorkWithResolutionComponent;
  //let fixture: ComponentFixture<WorkWithResolutionComponent>;
  let resaService = new ResolutionService(null, null, null, '');

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ WorkWithResolutionComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    //fixture = TestBed.createComponent(WorkWithResolutionComponent);
    component = new WorkWithResolutionComponent(resaService, null);
    //fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
