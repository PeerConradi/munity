import { Component, OnInit } from '@angular/core';
import { User } from '../../../models/user.model';
import { AdminService } from '../../../services/admin.service';

@Component({
  selector: 'app-user-management',
  templateUrl: './user-management.component.html',
  styleUrls: ['./user-management.component.css']
})
export class UserManagementComponent implements OnInit {

  allUsers: User[] = null;

  constructor(private adminService: AdminService) { }

  ngOnInit() {
    this.adminService.getUsers().subscribe(n => {
      this.allUsers = n;
    });
  }

}
