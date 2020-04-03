import { Component, OnInit, TemplateRef } from '@angular/core';
import { Conference } from '../../../../models/conference.model';
import { BsModalRef, BsModalService } from 'ngx-bootstrap';
import { Delegation } from '../../../../models/delegation.model';
import { Committee } from '../../../../models/committee.model';
import { ActivatedRoute } from '@angular/router';
import { ConferenceServiceService } from '../../../../services/conference-service.service';
import { UserService } from '../../../../services/user.service';
import { NotifierService } from 'angular-notifier';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';

@Component({
  selector: 'app-manage-conference-committees',
  templateUrl: './manage-conference-committees.component.html',
  styleUrls: ['./manage-conference-committees.component.css']
})
export class ManageConferenceCommitteesComponent implements OnInit {

  conference: Conference
  modalRef: BsModalRef;
  addCommitteeForm: FormGroup;
  presetDelegations: Delegation[] = [];
  filteredDelegations: Delegation[] = [];
  addDelegationSelection: Delegation = null;
  addCommitteeSubmitted: boolean = false;
  errorAddingCommittee: string = null;

  constructor(private route: ActivatedRoute, private conferenceService: ConferenceServiceService,
    private userService: UserService, private modalService: BsModalService, private notifier: NotifierService,
  private formBuilder: FormBuilder) { }

  ngOnInit() {
    this.addCommitteeForm = this.formBuilder.group({
      name: ['', Validators.required],
      article: ['', Validators.required],
      abbreviation: ['', [Validators.required, Validators.maxLength(6)]],
      parentcommittee: [''],
    });
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

  get acf() { return this.addCommitteeForm.controls; }

  addCommittee() {
    this.addCommitteeSubmitted = true;
    if (this.addCommitteeForm.invalid)
      return;

    let data = this.addCommitteeForm.value;

    const newCommittee = new Committee();
    newCommittee.Name = data.name;
    newCommittee.FullName = data.name;
    newCommittee.Abbreviation = data.abbreviation;
    newCommittee.Article = data.article;
    newCommittee.ResolutlyCommittee = data.parentcommittee;
    this.conferenceService.addCommittee(this.conference.ConferenceId, newCommittee).subscribe(n => {
      this.conference.Committees.push(n);
      this.addCommitteeSubmitted = false;
      this.addCommitteeForm.reset();
      this.errorAddingCommittee = null;
    }, err => {
        console.log(err);
        this.errorAddingCommittee = err.error;
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
      this.conferenceService.addDelegationToConference(this.conference.ConferenceId, this.addDelegationSelection.ID, 1, 1).subscribe(n => {
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

  getCommitteeDelegationCount(committee: Committee): number {
    return 0;
  }

}
