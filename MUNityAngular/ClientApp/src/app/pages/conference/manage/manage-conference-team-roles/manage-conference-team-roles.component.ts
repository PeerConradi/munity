import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ConferenceService } from '../../../../services/conference-service.service';
import { ActivatedRoute } from '@angular/router';
import { Conference } from '../../../../models/conference/conference.model';
import { TeamRole } from '../../../../models/team-role.model';
import { NotifierService } from 'angular-notifier';

@Component({
  selector: 'app-manage-conference-team-roles',
  templateUrl: './manage-conference-team-roles.component.html',
  styleUrls: ['./manage-conference-team-roles.component.css']
})
export class ManageConferenceTeamRolesComponent implements OnInit {

  conference: Conference;

  addRoleForm: FormGroup;

  roles: TeamRole[] = [];

  constructor(private formBuilder: FormBuilder, private conferenceService: ConferenceService, private route: ActivatedRoute,
    private notifier: NotifierService) { }


  ngOnInit() {
    let id: string = null;

    this.route.params.subscribe(params => {
      id = params.id;
    });

    if (id === 'test') {
      this.conference = this.conferenceService.getTestConference();
    }
    else {
      this.conferenceService.getConference(id).subscribe(n => {
        this.conference = n;
      });
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

  get f() { return this.addRoleForm.controls; }

  addRole() {
    if (this.addRoleForm.invalid) {
      return;
    }

    const role = new TeamRole();
    role.ConferenceId = this.conference.conferenceId;
    role.Name = this.addRoleForm.value.name;
    role.Description = this.addRoleForm.value.description;
    role.MinCount = this.addRoleForm.value.minCount;
    role.MaxCount = this.addRoleForm.value.maxCount;
    this.conferenceService.addTeamRole(role).subscribe(n => {
      this.addRoleForm.reset();
      this.roles.push(role);
    })
  }

}
