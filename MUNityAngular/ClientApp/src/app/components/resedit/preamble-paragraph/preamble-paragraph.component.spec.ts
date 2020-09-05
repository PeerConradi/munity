import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PreambleParagraphComponent } from './preamble-paragraph.component';

describe('PreambleParagraphComponent', () => {
  let component: PreambleParagraphComponent;
  //let fixture: ComponentFixture<PreambleParagraphComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PreambleParagraphComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    //fixture = TestBed.createComponent(PreambleParagraphComponent);
    //component = fixture.componentInstance;
    //fixture.detectChanges();
    component = new PreambleParagraphComponent(null, null);
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
