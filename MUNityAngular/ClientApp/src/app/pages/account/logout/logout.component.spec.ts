import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { LogoutComponent } from './logout.component';

describe('LogoutComponent', () => {
  let component: LogoutComponent;
  //let fixture: ComponentFixture<LogoutComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ LogoutComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    //fixture = TestBed.createComponent(LogoutComponent);
    //component = fixture.componentInstance;
    //fixture.detectChanges();
    component = new LogoutComponent(null);
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
