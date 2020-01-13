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

  constructor(private route: ActivatedRoute, private conferenceSerivce: ConferenceServiceService,
  private userService: UserService) { }

  ngOnInit() {
    this.route.params.subscribe(params => {
      console.log(params.id);
      this.conferenceSerivce.getConference(params.id).subscribe(n => {
        this.conference = n;
      });
    })
  }

}
