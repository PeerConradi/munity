import { Component, OnInit, TemplateRef } from '@angular/core';
import { Conference } from '../../../../models/conference.model';
import { BsModalRef, BsModalService } from 'ngx-bootstrap';
import { Delegation } from '../../../../models/delegation.model';
import { Committee } from '../../../../models/committee.model';
import { ActivatedRoute } from '@angular/router';
import { ConferenceServiceService } from '../../../../services/conference-service.service';
import { UserService } from '../../../../services/user.service';
import { NotifierService } from 'angular-notifier';

@Component({
  selector: 'app-manage-conference-committees',
  templateUrl: './manage-conference-committees.component.html',
  styleUrls: ['./manage-conference-committees.component.css']
})
export class ManageConferenceCommitteesComponent implements OnInit {

  conference: Conference
  modalRef: BsModalRef;
  newcommitteename: string;
  newcommitteeabbreviation: string;
  newcommitteeparent: string;
  newcommitteearticle: string;
  presetDelegations: Delegation[] = [];
  filteredDelegations: Delegation[] = [];
  addDelegationSelection: Delegation = null;

  constructor(private route: ActivatedRoute, private conferenceService: ConferenceServiceService,
    private userService: UserService, private modalService: BsModalService, private notifier: NotifierService) { }

  ngOnInit() {
    this.route.params.subscribe(params => {
      this.conferenceService.getConference(params.id).subscribe(n => {
        this.conference = n;
        this.filteredDelegations = this.conference.Delegations;
      });
    });
    this.conferenceService.getAllDelegations().subscribe(n => {
      this.presetDelegations = n;
      
    });
  }

  getDelegationById(id: string): Delegation {
    return this.conference.Delegations.find(n => n.ID == id);
  }

  removeDelegationFromCommittee(committee: Committee, delegationid: string) {
    console.log('TODO: removeDelegationFromCommittee');
    console.log(committee);
    console.log(delegationid);
  }

  isDelegationInCommittee(delegation: Delegation, committee: Committee) {
    const amount = committee.DelegationList.filter(n => n == delegation.ID).length;
    return (amount > 0);
  }

  openModal(template: TemplateRef<any>) {
    this.modalRef = this.modalService.show(template);
  }

  addCommittee() {
    if (this.newcommitteename == null || this.newcommitteename == '') {
      return;
    }

    if (this.newcommitteeparent == null) {
      this.newcommitteeparent = '';
    }

    const newCommittee = new Committee();
    newCommittee.Name = this.newcommitteename;
    newCommittee.FullName = this.newcommitteename;
    newCommittee.Abbreviation = this.newcommitteeabbreviation;
    newCommittee.Article = this.newcommitteearticle;
    newCommittee.ResolutlyCommittee = this.newcommitteeparent;
    this.conferenceService.addCommittee(this.conference.ID, newCommittee).subscribe(n => {
      this.conference.Committees.push(n);
    });
  }

  onSearchKey(event: any) { // without type info
    const filter: string = event.target.value;
    if (filter != null && filter != '') {
      this.filteredDelegations = this.conference.Delegations.filter(n => n.Name.includes(filter));
    } else {
      this.filteredDelegations = this.conference.Delegations;
    }
  }

  addDelegationClick() {
    if (this.addDelegationSelection == null) {
      this.notifier.notify('error', 'Zuerst muss eine Delegation ausgewählt werden. Ist diese nicht vorhanden lege bitte eine neue Delegation an.');
    } else {
      this.conferenceService.addDelegationToConference(this.conference.ID, this.addDelegationSelection.ID, 1, 1).subscribe(n => {
        this.conference.Delegations.push(n);
      });
    }
  }

  toggleDelegationInCommittee(delegation: Delegation, committee: Committee) {
    if (committee.DelegationList.find(n => n == delegation.ID) == null) {
      this.conferenceService.addDelegationToCommittee(committee.ID, delegation.ID, 1, 1).subscribe(n => {
        if (committee.DelegationList.find(n => n == delegation.ID) == null) {
          committee.DelegationList.push(delegation.ID);
        }
      });
    }
    
  }

}
