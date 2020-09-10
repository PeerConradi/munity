import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { LoginFormComponent } from './login-form.component';
import { FormBuilder } from '@angular/forms';

describe('LoginFormComponent', () => {
  let component: LoginFormComponent;
  //let fixture: ComponentFixture<LoginFormComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [LoginFormComponent]
    })
      .compileComponents();
  }));

  beforeEach(() => {
    // fixture = TestBed.createComponent(LoginFormComponent);
    // component = fixture.componentInstance;
    // fixture.detectChanges();
    let formBuilder = new FormBuilder();
    component = new LoginFormComponent(formBuilder, null, null);
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
