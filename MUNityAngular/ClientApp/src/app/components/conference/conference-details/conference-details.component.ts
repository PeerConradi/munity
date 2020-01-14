import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, ParamMap } from '@angular/router';
import { switchMap } from 'rxjs/operators';
import { ConferenceServiceService } from '../../../services/conference-service.service';
import { UserService } from '../../../services/user.service';
import { Conference } from '../../../models/conference.model';

@Component({
  selector: 'app-conference-details',
  templateUrl: './conference-details.component.html',
  styleUrls: ['./conference-details.component.css']
})
export class ConferenceDetailsComponent implements OnInit {

  conference: Conference;

  constructor(private route: ActivatedRoute, private conferenceService: ConferenceServiceService,
  private userService: UserService) { }

  ngOnInit() {
    this.route.params.subscribe(params => {
      console.log(params.id);
      this.conferenceService.getConference(params.id).subscribe(n => {
        this.conference = n;
        console.log(n);
      });
    })
  }

  addCommittee() {
    console.log('Start adding committee' + this.conference.id);
    this.conferenceService.addCommittee(this.conference.id, 'Neues Gremium', 'Gremium das neu ist', 'NG', 'das', '').subscribe(n => {
      console.log(n);
    },
      err => {
        console.error('Conference was not added:');
        console.log(err);
      });
  }

}
