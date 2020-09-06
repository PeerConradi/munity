import { Component, OnInit, Input } from '@angular/core';
import { Conference } from '../../../models/conference.model';
import { ActivatedRoute, Router } from '@angular/router';
import { ConferenceService } from '../../../services/conference-service.service';
import { UserService } from '../../../services/user.service';

@Component({
  selector: 'app-conference-menu',
  templateUrl: './conference-menu.component.html',
  styleUrls: ['./conference-menu.component.css']
})
export class ConferenceMenuComponent implements OnInit {

  committeeMenuCollapse = true;
  delegationMenuCollapse = true;
  teamMenuCollapse = true;
  publicRelationMenuCollapse = true;
  participantsMenuCollapse = true;

  conference: Conference;

  constructor(private conferenceService: ConferenceService, private router: Router) { }

  ngOnInit() {

    // this.conference = this.conferenceService.currentConference;
    this.conference = new Conference();
    
  }

}
