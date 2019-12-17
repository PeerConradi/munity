import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { OperativeParagraphComponent } from './operative-paragraph.component';

describe('OperativeParagraphComponent', () => {
  let component: OperativeParagraphComponent;
  let fixture: ComponentFixture<OperativeParagraphComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ OperativeParagraphComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(OperativeParagraphComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
