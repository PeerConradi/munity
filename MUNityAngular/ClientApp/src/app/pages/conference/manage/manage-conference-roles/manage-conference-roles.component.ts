import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Conference } from 'src/app/models/conference/conference.model';
import { ConferenceService } from 'src/app/services/conference-service.service';
import { faEdit } from '@fortawesome/free-solid-svg-icons';
import { Delegation } from 'src/app/models/conference/delegation.model';
import * as r from 'src/app/models/conference/roles';

@Component({
  selector: 'app-manage-conference-roles',
  templateUrl: './manage-conference-roles.component.html',
  styleUrls: ['./manage-conference-roles.component.css']
})
export class ManageConferenceRolesComponent implements OnInit {

  public faEdit = faEdit;

  public conference: Conference;

  public delegations: Delegation[] = [];

  public delegateRoles: r.Roles.DelegateRole[] = [];

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
      this.delegations = await this.conferenceService.getDelegationsOfConference(this.conference.conferenceId).toPromise();
      this.delegateRoles = await this.conferenceService.getDelegateRolesOfConferece(this.conference.conferenceId).toPromise();
      console.log('Delegate Roles: ');
      console.log(this.delegateRoles);
    }

  }

  addDelegate() {
    let del = new r.Roles.DelegateRole();
    del.roleId = Math.random() * 1000;
    del.roleName = '';
    this.delegateRoles.push(del);
  }

}