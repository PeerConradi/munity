import { Component, OnInit } from '@angular/core';
import { FormGroup, Validators, FormBuilder } from '@angular/forms';
import { UserService } from 'src/app/services/user.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-login-form',
  templateUrl: './login-form.component.html',
  styleUrls: ['./login-form.component.css']
})
export class LoginFormComponent implements OnInit {

  // TODO: define a login success event
  // TODO: login failed event

  public loginForm: FormGroup;
  public loading = false;
  public submitted = false;
  public success = false;
  public error = false;

  inputFocused = false;

  constructor(private formBuilder: FormBuilder, public userService: UserService, private routerService: Router) { }

  ngOnInit() {

    this.loginForm = this.formBuilder.group({
      username: ['', Validators.required],
      password: ['', Validators.required]
    });

    if (this.userService.session) {
      this.routerService.navigate(['/']);
    }
  }

  // convenience getter for easy access to form fields
  get f() { return this.loginForm.controls; }


  onSubmit() {
    this.submitted = true;

    // stop here if form is invalid
    if (this.loginForm.invalid) {
      return;
    }


    this.loading = true;


    this.userService.login(this.f.username.value, this.f.password.value).then(n => {
      this.loading = false;
      if (n) {
        this.error = false;
        this.success = true;
        this.routerService.navigate(['/']);
      } else {
        this.loading = false;
        this.error = true;
        this.success = false;
      }
    });

  }

  fieldGotFocus() {
    this.inputFocused = true;
  }

  fieldLostFocus() {
    this.inputFocused = false;
  }

}
