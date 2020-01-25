import { Component, OnInit } from '@angular/core';
import { ResolutionService } from '../../../services/resolution.service';
import { UserService } from '../../../services/user.service';
import { ResolutionInformation } from '../../../models/resolution-information.model';

@Component({
  selector: 'app-myresolutions',
  templateUrl: './myresolutions.component.html',
  styleUrls: ['./myresolutions.component.css']
})
export class MyresolutionsComponent implements OnInit {

  resolutions: ResolutionInformation[];

  constructor(private resolutionService: ResolutionService, private userSerivce: UserService) { }

  ngOnInit() {
    if (this.userSerivce.isLoggedIn) {
      this.resolutionService.getMyResolutions().subscribe(n => {
        console.log(n);
        this.resolutions = n;
      });
    }
  }

}
