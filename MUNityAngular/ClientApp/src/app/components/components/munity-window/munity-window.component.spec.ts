import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { MunityWindowComponent } from './munity-window.component';

describe('MunityWindowComponent', () => {
  let component: MunityWindowComponent;
  let fixture: ComponentFixture<MunityWindowComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ MunityWindowComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(MunityWindowComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
