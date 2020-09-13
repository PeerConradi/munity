import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ConferenceService } from '../../../../services/conference-service.service';
import { ActivatedRoute } from '@angular/router';
import { Conference } from '../../../../models/conference/conference.model';
import * as r from '../../../../models/conference/roles';
import { NotifierService } from 'angular-notifier';
import { of } from 'rxjs';

@Component({
  selector: 'app-manage-conference-team-roles',
  templateUrl: './manage-conference-team-roles.component.html',
  styleUrls: ['./manage-conference-team-roles.component.css']
})
export class ManageConferenceTeamRolesComponent implements OnInit {

  public conference: Conference;

  addRoleForm: FormGroup;

  roles: r.Roles.TeamRole[] = [];


  constructor(private formBuilder: FormBuilder, private conferenceService: ConferenceService, private route: ActivatedRoute,
    private notifier: NotifierService) { }




  async ngOnInit() {
    let id: string = null;

    this.route.params.subscribe(params => {
      id = params.id;
    });

    if (id != null) {
      console.log(id);
      this.conference = await this.conferenceService.getConference(id).toPromise();
      this.conferenceService.currentConference = this.conference;
      this.conferenceService.getTeamRoles(id).subscribe(n => {
        this.roles = n;
      })

    }


    this.addRoleForm = this.formBuilder.group({
      name: ['', Validators.required],
      description: ['', Validators.required],
      minCount: ['', Validators.required],
      maxCount: ['', Validators.required]
    });
  }

  getTeamRolesOfGroup(g: r.Roles.TeamRoleGroup): r.Roles.TeamRole[] {
    if (this.roles == null) return [];
    return this.roles.filter(n => n.teamRoleGroupId == g.teamRoleGroupId);
  }


  get f() { return this.addRoleForm.controls; }

  addRole() {
    if (this.addRoleForm.invalid) {
      return;
    }


  }

}
