import { Component, OnInit } from '@angular/core';
import { UserService } from '../../../services/user.service';

@Component({
  selector: 'app-logout',
  templateUrl: './logout.component.html',
  styleUrls: ['./logout.component.css']
})
export class LogoutComponent implements OnInit {

  private logoutSuccess = false;

  constructor(private userSerivce: UserService) { }

  ngOnInit() {
    this.userSerivce.logout().then(n => {
      this.logoutSuccess = n;
    });
  }

}
