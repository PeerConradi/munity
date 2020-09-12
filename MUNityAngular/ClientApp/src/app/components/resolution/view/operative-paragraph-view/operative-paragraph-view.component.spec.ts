import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { OperativeParagraphViewComponent } from './operative-paragraph-view.component';

describe('OperativeParagraphViewComponent', () => {
  let component: OperativeParagraphViewComponent;
  let fixture: ComponentFixture<OperativeParagraphViewComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [OperativeParagraphViewComponent]
    })
      .compileComponents();
  }));

  beforeEach(() => {
    // fixture = TestBed.createComponent(OperativeParagraphViewComponent);
    // component = fixture.componentInstance;
    // fixture.detectChanges();
    component = new OperativeParagraphViewComponent(null);
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
