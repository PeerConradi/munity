import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { XlabelComponent } from './xlabel.component';

describe('XlabelComponent', () => {
  let component: XlabelComponent;
  let fixture: ComponentFixture<XlabelComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ XlabelComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(XlabelComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
