import { Component, Input, OnInit } from '@angular/core';
import { Delegation } from 'src/app/models/conference/delegation.model';
import { ConferenceService } from 'src/app/services/conference-service.service';

import { faEdit } from '@fortawesome/free-solid-svg-icons';
import { Committee } from 'src/app/models/conference/committee.model';
import { Conference } from 'src/app/models/conference/conference.model';
import { Roles } from 'src/app/models/conference/roles';

@Component({
  selector: 'app-edit-delegation-row',
  templateUrl: './edit-delegation-row.component.html',
  styleUrls: ['./edit-delegation-row.component.css']
})
export class EditDelegationRowComponent implements OnInit {

  @Input() public conference: Conference;

  public faEdit = faEdit;

  public isEditing: boolean = false;

  @Input() public delegations: Delegation[] = [];

  @Input() public role: Roles.DelegateRole;


  constructor(private conferenceService: ConferenceService) { }

  async ngOnInit() {
    
  }

}
