import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { WorkWithResolutionComponent } from './work-with-resolution.component';

describe('WorkWithResolutionComponent', () => {
  let component: WorkWithResolutionComponent;
  let fixture: ComponentFixture<WorkWithResolutionComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ WorkWithResolutionComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(WorkWithResolutionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
