import { Component, OnInit } from '@angular/core';
import { Registration } from 'src/app/models/registration.model';
import { Validators, FormBuilder, FormGroup } from '@angular/forms';
import { UserService } from 'src/app/services/user.service';
import { NotifierService } from 'angular-notifier';

@Component({
  selector: 'app-register-form',
  templateUrl: './register-form.component.html',
  styleUrls: ['./register-form.component.css']
})
export class RegisterFormComponent implements OnInit {

  registerForm: FormGroup;
  submitted = false;
  created = false;
  error = false;
  loading = false;
  errmsg = "";

  constructor(private formBuilder: FormBuilder, private authService: UserService, private notifier: NotifierService) { }

  ngOnInit() {
    this.registerForm = this.formBuilder.group({
      username: ['', Validators.required],
      forname: ['', Validators.required],
      lastname: ['', Validators.required],
      email: ['', Validators.required, Validators.email],
      password: ['', [Validators.required, Validators.minLength(6)]],
      confirmpassword: ['', [Validators.required, Validators.minLength(6)]],
      birthday: ['', [Validators.required]],
      agb: [false, Validators.pattern('true')]
    });
  }

  // convenience getter for easy access to form fields
  get f() { return this.registerForm.controls; }

  checkUsername() {
    const data = this.registerForm.value;
    const username = data.username;
    this.authService.checkUsername(username).subscribe(n => {
      if (n) {
        this.f.username.setErrors({ 'taken': true });
      } else {
        this.f.username.setErrors({ 'taken': false });
      }
    });
  }

  onSubmit() {
    this.submitted = true;

    this.checkUsername();

    // stop here if form is invalid
    if (this.registerForm.invalid) {
      console.log('Es gibt fehler im registrierungsformular!');
      console.log(this.registerForm.errors);
      console.log(this.f.username.errors);
      return;
    }


    let data = this.registerForm.value;

    if (data.password !== data.confirmpassword) {
      this.f.password.setErrors({ 'incorrect': true });
      this.f.confirmpassword.setErrors({ 'incorrect': true });
      return;
    }

    this.loading = true;
    const model: Registration = new Registration();
    model.username = data.username;
    model.forename = data.forname;
    model.lastname = data.lastname;
    model.password = data.password;
    model.mail = data.email;
    model.birthday = data.birthday;
    this.authService.register(model).subscribe(
      msg => { this.created = true; this.loading = false; },
      error => {
        this.error = true; this.loading = false;
        this.errmsg = error;
        console.log(error);
      });
  }

}
