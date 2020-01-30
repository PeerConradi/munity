import { Component, OnInit } from '@angular/core';
import { UserService } from '../services/user.service';

@Component({
  selector: 'app-secondary-nav',
  templateUrl: './secondary-nav.component.html',
  styleUrls: ['./secondary-nav.component.css']
})
export class SecondaryNavComponent implements OnInit {

  isSecMenuExpanded: boolean = false;

  constructor(private userService: UserService) { }

  ngOnInit() {
    
  }

  collapse() {
    this.isSecMenuExpanded = false;
  }

  toggle() {
    console.log('toggle');
    this.isSecMenuExpanded = !this.isSecMenuExpanded;
  }

}
