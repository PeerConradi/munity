import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ResolutionHomeComponent } from './resolution-home.component';

describe('ResolutionHomeComponent', () => {
  let component: ResolutionHomeComponent;
  let fixture: ComponentFixture<ResolutionHomeComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ResolutionHomeComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ResolutionHomeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
