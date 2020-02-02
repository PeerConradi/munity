import { Component, OnInit } from '@angular/core';
import { UserService } from '../../../services/user.service';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { Router } from '@angular/router';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
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

    if (this.userService.isLoggedIn) {
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
