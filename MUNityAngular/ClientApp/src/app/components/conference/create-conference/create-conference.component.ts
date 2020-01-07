import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, FormControl } from '@angular/forms';
import { NgbCalendar, NgbDateParserFormatter, NgbDate } from '@ng-bootstrap/ng-bootstrap';
import { ConferenceServiceService } from '../../../services/conference-service.service';
import { Conference } from '../../../models/conference.model';

@Component({
  selector: 'app-create-conference',
  templateUrl: './create-conference.component.html',
  styleUrls: ['./create-conference.component.css']
})
export class CreateConferenceComponent implements OnInit {

  public createForm;
  startdatemodel: NgbDate;
  enddatemodel: NgbDate;

  public createdConference = null;
  public info: string;

  public requested: boolean = false;
  public conferenceCreated: boolean = false;

  constructor(private formBuilder: FormBuilder, private service: ConferenceServiceService) {
    this.createForm = this.formBuilder.group({
      name: '',
      fullname: '',
      abbreviation: '',
      password: ''
    });
    
  }

  

  ngOnInit() {
  }

  onSubmit(data) {
    console.log(data);
    let conference: Conference = new Conference();
    conference.Name = data.name;
    conference.FullName = data.fullname;
    conference.Abbreviation = data.abbreviation;
    let startDate: Date = new Date(this.startdatemodel.year, this.startdatemodel.month, this.startdatemodel.day);
    let endDate: Date = new Date(this.enddatemodel.year, this.enddatemodel.month, this.enddatemodel.day);

    conference.StartDate = startDate;
    conference.EndDate = endDate;
    this.requested = true;
    this.info = '';
    this.service.createConference(conference, data.password).subscribe(n => {
      //this.conferenceCreated = true;
      this.createdConference = n;
      console.log(n);
    });
  }

}
