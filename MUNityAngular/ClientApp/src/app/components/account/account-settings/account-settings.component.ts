import { Component, OnInit } from '@angular/core';
import { UserService } from '../../../services/user.service';
import { FormBuilder } from '@angular/forms';
import { NotifierService } from 'angular-notifier';
import { User } from '../../../models/user.model';

@Component({
  selector: 'app-account-settings',
  templateUrl: './account-settings.component.html',
  styleUrls: ['./account-settings.component.css']
})
export class AccountSettingsComponent implements OnInit {

  changePasswordForm;
  passdontmatch = false;
  user: User;

  constructor(public userService: UserService,private formBuilder: FormBuilder,private notifier: NotifierService) {
    this.changePasswordForm = this.formBuilder.group({
      oldpassword: '',
      newpassword: '',
      confirmpassword: ''
    });
  }

  ngOnInit() {
    this.userService.getCurrentUser().subscribe(n => this.user = n);
  }

  updateProfile() {
    this.userService.updateUserinfo(this.user).subscribe();
  }

  onChangePassword(data) {
    if (data.newpassword !== data.confirmpassword) {
      this.passdontmatch = true;
    } else {
      this.passdontmatch = false;
      this.userService.changePassword(data.oldpassword, data.newpassword).subscribe(n => {
        console.log(n);
        this.userService.setSessionkey(n);
        this.notifier.notify('success', 'Passwort wurde geändert.')
      }, err => {
          this.notifier.notify('error', 'Passwort konnte nicht geändert werden.')
      });
    }
    console.log(data);
  }

  

}
