import { Component, OnInit } from '@angular/core';
import { Conference } from 'src/app/models/conference.model';
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
    this.conference.Name = "Test Konferenz";
    let delegationOne = new Delegation();
    delegationOne.Name = "Test Delegation";
    this.conference.Delegations = [];
    this.conference.Delegations.push(delegationOne);
    this.conferenceService.currentConference = this.conference;
  }

}
