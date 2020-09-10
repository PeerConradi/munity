import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { RegisterFormComponent } from './register-form.component';
import { FormBuilder } from '@angular/forms';

describe('RegisterFormComponent', () => {
  let component: RegisterFormComponent;
  //let fixture: ComponentFixture<RegisterFormComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [RegisterFormComponent]
    })
      .compileComponents();
  }));

  beforeEach(() => {
    // fixture = TestBed.createComponent(RegisterFormComponent);
    // component = fixture.componentInstance;
    // fixture.detectChanges();
    let formBuilder = new FormBuilder();
    component = new RegisterFormComponent(formBuilder, null, null,);
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
