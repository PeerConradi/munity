import { Component, OnInit } from '@angular/core';
import { NotifierService } from 'angular-notifier';
import { ConferenceService } from '../../../../services/conference-service.service';
import { ActivatedRoute } from '@angular/router';
import { Conference } from '../../../../models/conference/conference.model';
import { TeamRole } from '../../../../models/team-role.model';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { UserService } from '../../../../services/user.service';
import { User } from '../../../../models/user.model';
import { UserConferenceRole } from '../../../../models/user-conference-role.model';

@Component({
  selector: 'app-manage-conference-team',
  templateUrl: './manage-conference-team.component.html',
  styleUrls: ['./manage-conference-team.component.css']
})
export class ManageConferenceTeamComponent implements OnInit {

  conference: Conference;

  roles: TeamRole[] = [];

  team: UserConferenceRole[] = [];

  addMemberForm: FormGroup;

  constructor(private formBuilder: FormBuilder, private conferenceService: ConferenceService, private route: ActivatedRoute,
    private notifier: NotifierService, private authService: UserService) { }

  get f() { return this.addMemberForm.controls; }

  checkUsername() {
    const data = this.addMemberForm.value;
    const username = data.username;
    this.authService.checkUsername(username).subscribe(n => {
      if (n == true) {
        this.f.username.setErrors({ 'found': true });
      } else {
        this.f.username.setErrors({ 'found': false });
      }
    });
  }

  ngOnInit() {
    this.route.params.subscribe(params => {
      this.conferenceService.getConference(params.id).subscribe(n => {
        this.conference = n;
      });
      this.conferenceService.getTeamRoles(params.id).subscribe(n => {
        this.roles = n;
      })
      this.conferenceService.getTeam(params.id).subscribe(n => {
        this.team = n;
      });
    });

    this.addMemberForm = this.formBuilder.group({
      username: ['', Validators.required],
      role: ['', Validators.required]
    });
  }

  addMember() {
    this.conferenceService.addTeamMember(this.addMemberForm.value.username, this.addMemberForm.value.role).subscribe(n => {
      this.addMemberForm.reset();
    })
  }

}
