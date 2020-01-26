import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { MunityBoxComponent } from './munity-box.component';

describe('MunityBoxComponent', () => {
  let component: MunityBoxComponent;
  let fixture: ComponentFixture<MunityBoxComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ MunityBoxComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(MunityBoxComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
