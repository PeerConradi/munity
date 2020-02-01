import { Component, OnInit } from '@angular/core';
import { UserService } from '../../../services/user.service';
import { FormBuilder } from '@angular/forms';
import { NotifierService } from 'angular-notifier';

@Component({
  selector: 'app-account-settings',
  templateUrl: './account-settings.component.html',
  styleUrls: ['./account-settings.component.css']
})
export class AccountSettingsComponent implements OnInit {

  changePasswordForm;
  passdontmatch = false;

  constructor(private userService: UserService,private formBuilder: FormBuilder,private notifier: NotifierService) {
    this.changePasswordForm = this.formBuilder.group({
      oldpassword: '',
      newpassword: '',
      confirmpassword: ''
    });
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

  ngOnInit() {
  }

}
