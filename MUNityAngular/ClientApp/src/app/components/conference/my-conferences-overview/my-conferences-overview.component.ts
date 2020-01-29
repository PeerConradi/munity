import { Component, OnInit } from '@angular/core';
import { UserService } from '../../../services/user.service';
import { ConferenceServiceService } from '../../../services/conference-service.service';

@Component({
  selector: 'app-my-conferences-overview',
  templateUrl: './my-conferences-overview.component.html',
  styleUrls: ['./my-conferences-overview.component.css']
})
export class MyConferencesOverviewComponent implements OnInit {

  conferences = [];
  

  constructor(private userService: UserService, private conferenceService: ConferenceServiceService) { }

  ngOnInit() {
    this.conferenceService.getAllConferences().subscribe(success => {
      this.conferences = success;
    });
  }

}