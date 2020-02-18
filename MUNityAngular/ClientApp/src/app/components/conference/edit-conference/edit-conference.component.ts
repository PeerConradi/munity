import { Component, OnInit } from '@angular/core';
import { Conference } from '../../../models/conference.model';
import { ActivatedRoute } from '@angular/router';
import { ConferenceServiceService } from '../../../services/conference-service.service';
import { UserService } from '../../../services/user.service';

@Component({
  selector: 'app-edit-conference',
  templateUrl: './edit-conference.component.html',
  styleUrls: ['./edit-conference.component.css']
})
export class EditConferenceComponent implements OnInit {

  conference: Conference;

  constructor(private route: ActivatedRoute, private conferenceService: ConferenceServiceService,
    private userService: UserService) { }

  ngOnInit() {
    this.route.params.subscribe(params => {
      console.log(params.id);
      this.conferenceService.getConference(params.id).subscribe(n => {
        this.conference = n;
        console.log(this.conference);
      });
    })
  }

  foo() {
    console.log('bar');
  }

}
