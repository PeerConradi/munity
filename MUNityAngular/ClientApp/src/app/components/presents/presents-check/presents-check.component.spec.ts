import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PresentsCheckComponent } from './presents-check.component';

describe('PresentsCheckComponent', () => {
  let component: PresentsCheckComponent;
  //let fixture: ComponentFixture<PresentsCheckComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PresentsCheckComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    //fixture = TestBed.createComponent(PresentsCheckComponent);
    //component = fixture.componentInstance;
    //fixture.detectChanges();
    component = new PresentsCheckComponent(null, null, null, null);
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
