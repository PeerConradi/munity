import { Component, OnInit, Input } from '@angular/core';
import { Conference } from '../../../models/conference/conference.model';
import { ActivatedRoute, Router } from '@angular/router';
import { ConferenceService } from '../../../services/conference-service.service';
import { UserService } from '../../../services/user.service';
import { Observable } from 'rxjs';

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
  roleMenuCollapse = true;

  public conference: Conference;

  constructor(public conferenceService: ConferenceService, private router: Router) { }

  ngOnInit() {
    console.log('ask for current conference')
    this.conference = this.conferenceService.currentConference;

  }

}
