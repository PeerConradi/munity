import { Component, OnInit } from '@angular/core';
import { UserService } from '../../../services/user.service';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
  loginForm: FormGroup;
  loading = false;
  submitted = false;
  success = false;
  error = false;

  inputFocused = false;

  constructor(private formBuilder: FormBuilder, private userService: UserService) { }

  ngOnInit() {

    this.loginForm = this.formBuilder.group({
      username: ['', Validators.required],
      password: ['', Validators.required]
    });
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
