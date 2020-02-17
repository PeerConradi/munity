import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, FormControl } from '@angular/forms';
import { ConferenceServiceService } from '../../../services/conference-service.service';
import { Conference } from '../../../models/conference.model';

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

  constructor(private formBuilder: FormBuilder, private service: ConferenceServiceService) {
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

    console.log(data);
    let conference: Conference = new Conference();
    conference.Name = data.name;
    conference.FullName = data.fullname;
    conference.Abbreviation = data.abbreviation;
    let startDate: Date = data.timespan[0];
    let endDate: Date = data.timespan[1];

    conference.StartDate = startDate;
    conference.EndDate = endDate;
    //this.requested = true;
    //this.info = '';
    //this.service.createConference(conference).subscribe(n => {
    //  //this.conferenceCreated = true;
    //  this.createdConference = n;
    //  console.log(n);
    //},
    //  err => {
    //    this.error = true;
    //    this.error_msg = err;
    //  });
  }

}
