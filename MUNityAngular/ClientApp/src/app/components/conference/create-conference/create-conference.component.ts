import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, FormControl } from '@angular/forms';
import { ConferenceService } from '../../../services/conference-service.service';
import { Conference } from '../../../models/conference/conference.model';
import { Router } from '@angular/router';
import { UserService } from '../../../services/user.service';

@Component({
  selector: 'app-create-conference',
  templateUrl: './create-conference.component.html',
  styleUrls: ['./create-conference.component.css']
})
export class CreateConferenceComponent implements OnInit {

  public createForm;

  public createdConference = null;
  error: boolean = false;
  error_msg: any = null;
  public info: string;

  public requested: boolean = false;
  public conferenceCreated: boolean = false;

  forbidden = true;

  constructor(private formBuilder: FormBuilder, private service: ConferenceService, private router: Router, private userService: UserService) {
    this.createForm = this.formBuilder.group({
      name: '',
      fullname: '',
      abbreviation: '',
      timespan: null
    });

  }



  ngOnInit() {

  }

  onSubmit(data) {
    //TODO: validate Timespan
    let conference: Conference = new Conference();
    conference.name = data.name;
    conference.fullName = data.fullname;
    conference.abbreviation = data.abbreviation;
    let startDate: Date = data.timespan[0];
    let endDate: Date = data.timespan[1];

    conference.startDate = startDate;
    conference.endDate = endDate;
    //this.requested = true;
    //this.info = '';
    this.service.createConference(conference).subscribe(n => {
      //this.conferenceCreated = true;
      this.createdConference = n;
      this.router.navigateByUrl('/mc/overview/' + n.conferenceId);
    },
      err => {
        this.error = true;
        this.error_msg = err;
      });
  }

}
