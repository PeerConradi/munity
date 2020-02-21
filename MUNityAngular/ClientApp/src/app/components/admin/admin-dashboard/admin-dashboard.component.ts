import { Component, OnInit } from '@angular/core';
import { UserService } from '../../../services/user.service';
import { AdminService } from '../../../services/admin.service';

@Component({
  selector: 'app-admin-dashboard',
  templateUrl: './admin-dashboard.component.html',
  styleUrls: ['./admin-dashboard.component.css']
})
export class AdminDashboardComponent implements OnInit {

  isAdmin: boolean = false;

  resolutionsInSQL: number = 0;
  resolutionsInMGDB: number = 0;
  userCount: number = 0;
  conferenceCount: number = 0;
  conferenceCacheCount: number = 0;

  constructor(private userService: UserService, public adminService: AdminService) { }

  ngOnInit() {
    this.userService.getIsAdmin().subscribe(n => {
      this.isAdmin = n;
    });
    this.adminService.getResolutionDatabaseCount().subscribe(n => this.resolutionsInSQL = n);
    this.adminService.getResolutionMongoDbCount().subscribe(n => this.resolutionsInMGDB = n);
    this.adminService.getUserCount().subscribe(n => this.userCount = n);
    this.adminService.getConferenceCount().subscribe(n => this.conferenceCount = n);
    this.adminService.getConferenceCacheCount().subscribe(n => this.conferenceCacheCount = n);
  }

}
