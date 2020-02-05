import { Component, OnInit, TemplateRef } from '@angular/core';
import { ActivatedRoute, ParamMap } from '@angular/router';
import { ConferenceServiceService } from '../../../services/conference-service.service';
import { UserService } from '../../../services/user.service';
import { Conference } from '../../../models/conference.model';
import { BsModalService, BsModalRef } from 'ngx-bootstrap/modal';
import { NotifierService } from 'angular-notifier';
import { Delegation } from '../../../models/delegation.model';

@Component({
  selector: 'app-conference-details',
  templateUrl: './conference-details.component.html',
  styleUrls: ['./conference-details.component.css']
})
export class ConferenceDetailsComponent implements OnInit {

  conference: Conference;
  modalRef: BsModalRef;
  newcommitteename: string;
  newcommitteeabbreviation: string;
  newcommitteeparent: string;
  newcommitteearticle: string;
  presetDelegations: Delegation[] = [];
  addDelegationSelection: Delegation = null;

  constructor(private route: ActivatedRoute, private conferenceService: ConferenceServiceService,
    private userService: UserService, private modalService: BsModalService, private notifier: NotifierService) { }

  ngOnInit() {
    this.route.params.subscribe(params => {
      console.log(params.id);
      this.conferenceService.getConference(params.id).subscribe(n => {
        this.conference = n;
        console.log(n);
      });
    });
    this.conferenceService.getAllDelegations().subscribe(n => {
      this.presetDelegations = n;
    })
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

    console.log('Start adding committee' + this.conference.ID + ', ' +
      this.newcommitteename + ', ' +
      this.newcommitteeabbreviation + ', ' +
      this.newcommitteeparent);
    this.conferenceService.addCommittee(this.conference.ID, this.newcommitteename,
      this.newcommitteename, this.newcommitteeabbreviation, this.newcommitteearticle, this.newcommitteeparent).subscribe(n => {
        console.log(n);
        this.conference.Committees.push(n);
    },
        err => {
          //this.notifier.notify('error', 'Committee was not added, something went wrong check the log for informations!');
          console.error('Conference was not added:');
          console.log(err);
      });
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

}
