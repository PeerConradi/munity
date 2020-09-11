import { Component, OnInit } from '@angular/core';
import { ResolutionService } from '../../../services/resolution.service';
import { UserService } from '../../../services/user.service';
import { ResolutionInformation } from '../../../models/resolution/resolution-information.model';
import { ResolutionAdvancedInfo } from '../../../models/resolution/resolution-advanced-info.model';

@Component({
  selector: 'app-myresolutions',
  templateUrl: './myresolutions.component.html',
  styleUrls: ['./myresolutions.component.css']
})
export class MyresolutionsComponent implements OnInit {

  resolutions: ResolutionAdvancedInfo[];

  constructor(public resolutionService: ResolutionService, public userSerivce: UserService) { }

  ngOnInit() {
    if (this.userSerivce.session) {
      this.resolutionService.getMyResolutions().subscribe(n => {
        console.log(n);
        this.resolutions = n;
      });
    }
  }

  refreshList() {
    this.resolutions = null;
    this.resolutionService.getMyResolutions().subscribe(n => {
      this.resolutions = n;
    });
  }

}
