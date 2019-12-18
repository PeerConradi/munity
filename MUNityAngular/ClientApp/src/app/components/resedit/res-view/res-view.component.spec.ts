import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ResViewComponent } from './res-view.component';

describe('ResViewComponent', () => {
  let component: ResViewComponent;
  let fixture: ComponentFixture<ResViewComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ResViewComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ResViewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
