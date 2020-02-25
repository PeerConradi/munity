import { Component, OnInit } from '@angular/core';
import { UserService } from '../services/user.service';

@Component({
  selector: 'app-nav-menu',
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.css']
})
export class NavMenuComponent implements OnInit{
  isExpanded = false;

  isAdmin = false;

  constructor(public userSerivce: UserService) {
    
  }

  ngOnInit() {
    if (this.userSerivce.isLoggedIn) {
      this.userSerivce.getIsAdmin().subscribe(n => {
        this.isAdmin = n;
      });
    }
  }

  collapse() {
    this.isExpanded = false;
  }

  toggle() {
    this.isExpanded = !this.isExpanded;
  }
}
