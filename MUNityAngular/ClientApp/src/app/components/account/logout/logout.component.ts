import { Component, OnInit } from '@angular/core';
import { UserService } from '../../../services/user.service';

@Component({
  selector: 'app-logout',
  templateUrl: './logout.component.html',
  styleUrls: ['./logout.component.css']
})
export class LogoutComponent implements OnInit {

  public logoutSuccess = false;

  constructor(public userSerivce: UserService) { }

  ngOnInit() {
    this.userSerivce.logout().then(n => {
      this.logoutSuccess = n;
    });
  }

}
