import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
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

  @ViewChild('newDelegationNameInput') newDelegationNameInput: ElementRef;

  public conference: Conference;

  public delegations: Delegation[];

  public newDelegationName: string;

  public newDelegationFullName: string;

  public newDelegationAbbreviation: string;

  constructor(private conferenceService: ConferenceService, private route: ActivatedRoute) { }

  async ngOnInit() {
    let id: string = null;

    this.route.params.subscribe(params => {
      id = params.id;
    });

    if (id != null) {
      console.log(id);
      this.conference = await this.conferenceService.getConference(id).toPromise();
      this.conferenceService.currentConference = this.conference;
      this.delegations = await this.conferenceService.getDelegationsOfConference(id).toPromise();
      console.log(this.delegations)
    }

  }

  addDelegation() {
    if (this.newDelegationName != null && this.newDelegationName.length > 0 && this.newDelegationFullName != null && this.newDelegationFullName.length > 0) {
      let newDelegation: Delegation = new Delegation();
      newDelegation.delegationId = this.conferenceService.generateId(25);
      newDelegation.name = this.newDelegationName;
      newDelegation.fullName = this.newDelegationFullName;
      newDelegation.abbreviation = this.newDelegationAbbreviation;
      this.delegations.push(newDelegation);
      this.newDelegationFullName = '';
      this.newDelegationName = '';
      this.newDelegationAbbreviation = ''
      this.newDelegationNameInput.nativeElement.focus();
    }
  }

  getDelegationImagePath(delegation: Delegation): string {
    if (delegation.abbreviation.length > 0) {
      let imgName = delegation.abbreviation.toLowerCase();
      if (this.conferenceService.knownAbbreviations.indexOf(imgName) != -1) {
        return '/assets/img/flags/small/' + delegation.abbreviation.toLowerCase() + '.png';
      }
    }
    //Default Image
    return '/assets/img/flags/small/un.png';
  }
}