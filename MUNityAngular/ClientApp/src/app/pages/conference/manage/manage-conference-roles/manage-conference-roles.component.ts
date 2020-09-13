import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Conference } from 'src/app/models/conference/conference.model';
import { ConferenceService } from 'src/app/services/conference-service.service';
import { faEdit } from '@fortawesome/free-solid-svg-icons';

@Component({
  selector: 'app-manage-conference-roles',
  templateUrl: './manage-conference-roles.component.html',
  styleUrls: ['./manage-conference-roles.component.css']
})
export class ManageConferenceRolesComponent implements OnInit {

  public faEdit = faEdit;

  public conference: Conference;

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

    }

  }


}