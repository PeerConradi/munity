import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ResOptionsComponent } from './res-options.component';

describe('ResOptionsComponent', () => {
  let component: ResOptionsComponent;
  //let fixture: ComponentFixture<ResOptionsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ResOptionsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    //fixture = TestBed.createComponent(ResOptionsComponent);
    //component = fixture.componentInstance;
    //fixture.detectChanges();
    component = new ResOptionsComponent(null, null, null);
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
