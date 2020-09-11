import { Component, OnInit, TemplateRef } from '@angular/core';
import { Conference } from '../../../../models/conference/conference.model';
import { BsModalRef, BsModalService } from 'ngx-bootstrap';
import { Delegation } from '../../../../models/conference/delegation.model';
import { Committee } from '../../../../models/conference/committee.model';
import { ActivatedRoute } from '@angular/router';
import { ConferenceService } from '../../../../services/conference-service.service';
import { UserService } from '../../../../services/user.service';
import { NotifierService } from 'angular-notifier';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Observable, Subject } from 'rxjs';

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
  isAddingCommittee: boolean = false;

  constructor(private route: ActivatedRoute, private conferenceService: ConferenceService,
    private userService: UserService, private modalService: BsModalService, private notifier: NotifierService,
    private formBuilder: FormBuilder) { }

  ngOnInit() {
    this.conference = new Conference();
  }

  getDelegationById(id: string) {

  }

  removeDelegationFromCommittee(committee: Committee, delegationid: string) {

  }

  isDelegationInCommittee(delegation: Delegation, committee: Committee) {

  }

  openModal(template: TemplateRef<any>) {
    this.modalRef = this.modalService.show(template);
  }

  get acf() { return this.addCommitteeForm.controls; }

  addCommittee() {

  }

  onSearchKey(event: any) { // without type info

  }

  addDelegationClick() {

  }

  toggleDelegationInCommittee(delegation: Delegation, committee: Committee) {

  }

}
