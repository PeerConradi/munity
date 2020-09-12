import { Component, OnInit } from '@angular/core';
import { of } from 'rxjs';
import { Conference } from 'src/app/models/conference/conference.model';
import { Delegation } from 'src/app/models/conference/delegation.model';
import { ConferenceService } from 'src/app/services/conference-service.service';

@Component({
  selector: 'app-manage-conference-delegations',
  templateUrl: './manage-conference-delegations.component.html',
  styleUrls: ['./manage-conference-delegations.component.css']
})
export class ManageConferenceDelegationsComponent implements OnInit {

  public conference: Conference;

  constructor(private conferenceService: ConferenceService) { }

  ngOnInit() {
    this.conference = new Conference();
    this.conference.name = "Test Konferenz";
    let delegationOne = new Delegation();
    delegationOne.name = "Test Delegation";
    //this.conference.del = [];
    //this.conference.Delegations.push(delegationOne);
    this.conferenceService.currentConference = this.conference;
  }

}
