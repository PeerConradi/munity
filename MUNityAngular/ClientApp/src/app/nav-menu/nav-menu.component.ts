import { Component, OnInit } from '@angular/core';
import { UserService } from '../services/user.service';

@Component({
  selector: 'app-nav-menu',
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.css']
})
export class NavMenuComponent implements OnInit {
  isExpanded = false;

  isAdmin = false;

  showTopBox = false;

  constructor(public userSerivce: UserService) {

  }

  ngOnInit() {

  }

  collapse() {
    this.isExpanded = false;
  }

  toggle() {
    this.isExpanded = !this.isExpanded;
  }

  vip() {
    this.userSerivce.justForVIP().subscribe(n => console.log(n), err => console.log(err));
  }
}
