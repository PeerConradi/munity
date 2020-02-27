import { Component, OnInit, TemplateRef } from '@angular/core';
import { ActivatedRoute, ParamMap } from '@angular/router';
import { ConferenceServiceService } from '../../../services/conference-service.service';
import { UserService } from '../../../services/user.service';
import { Conference } from '../../../models/conference.model';
import { BsModalService, BsModalRef } from 'ngx-bootstrap/modal';
import { NotifierService } from 'angular-notifier';
import { Delegation } from '../../../models/delegation.model';
import { Committee } from '../../../models/committee.model';

@Component({
  selector: 'app-conference-details',
  templateUrl: './conference-details.component.html',
  styleUrls: ['./conference-details.component.css']
})
export class ConferenceDetailsComponent implements OnInit {

  conference: Conference;
  

  constructor(private route: ActivatedRoute, public conferenceService: ConferenceServiceService,
    private userService: UserService, private modalService: BsModalService, private notifier: NotifierService) { }

  ngOnInit() {
    this.route.params.subscribe(params => {
      this.conferenceService.getConference(params.id).subscribe(n => {
        this.conference = n;
      });
    });
  }

  

}
