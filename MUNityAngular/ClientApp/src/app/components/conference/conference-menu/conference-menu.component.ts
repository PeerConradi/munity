import { Component, OnInit, Input } from '@angular/core';
import { Conference } from '../../../models/conference.model';
import { ActivatedRoute, Router } from '@angular/router';
import { ConferenceServiceService } from '../../../services/conference-service.service';
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

  constructor(private route: ActivatedRoute, private conferenceService: ConferenceServiceService, private router: Router) { }

  ngOnInit() {
    const routes = this.router.url.split('/');
    const id = routes[routes.length - 1];
    this.conferenceService.getConference(id).subscribe(n => {
      console.log(n);
      this.conference = n;
    });
  }

}
