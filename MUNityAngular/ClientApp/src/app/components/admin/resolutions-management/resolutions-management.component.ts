import { Component, OnInit } from '@angular/core';
import { UserService } from '../../../services/user.service';
import { AdminService } from '../../../services/admin.service';

@Component({
  selector: 'app-resolutions-management',
  templateUrl: './resolutions-management.component.html',
  styleUrls: ['./resolutions-management.component.css']
})
export class ResolutionsManagementComponent implements OnInit {

  isAdmin = false;

  resolutionsInSQL: number = 0;
  resolutionsInMGDB: number = 0;

  isRestoring = false;
  isPurging = false;

  constructor(private userService: UserService, private adminService: AdminService) { }

  ngOnInit() {
    //this.userService.getIsAdmin().subscribe(n => {
    //  this.isAdmin = n;
    //});
    this.adminService.getResolutionDatabaseCount().subscribe(n => this.resolutionsInSQL = n);
    this.adminService.getResolutionMongoDbCount().subscribe(n => this.resolutionsInMGDB = n);

  }

  restoreResolutions() {
    this.isRestoring = true;
    this.adminService.restoreResolutions().subscribe(n => {
      this.isRestoring = false;
      this.adminService.getResolutionDatabaseCount().subscribe(n => this.resolutionsInSQL = n);
      this.adminService.getResolutionMongoDbCount().subscribe(n => this.resolutionsInMGDB = n);
    });
  }

  purgeResolutions() {
    this.isPurging = true;
    this.adminService.purgeResolutions().subscribe(n => {
      this.isPurging = false;
      this.adminService.getResolutionDatabaseCount().subscribe(n => this.resolutionsInSQL = n);
      this.adminService.getResolutionMongoDbCount().subscribe(n => this.resolutionsInMGDB = n);
    });
  }

}
