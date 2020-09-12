import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { FaIconLibrary } from '@fortawesome/angular-fontawesome';
import { faFile, faFlag, faUser, faCircle, faCarrot, faCaretUp } from '@fortawesome/free-solid-svg-icons';
import { Conference } from 'src/app/models/conference/conference.model';
import { ConferenceService } from 'src/app/services/conference-service.service';

@Component({
  selector: 'app-manage-conference-dashboard',
  templateUrl: './manage-conference-dashboard.component.html',
  styleUrls: ['./manage-conference-dashboard.component.css']
})
export class ManageConferenceDashboardComponent implements OnInit {

  // There need to be a better way to do this, but until then this shit will do!
  public faCircle = faCircle;

  public faCaretUp = faCaretUp

  public conference: Conference;

  constructor(private conferenceService: ConferenceService, private route: ActivatedRoute) {

  }

  async ngOnInit() {
    let id: string = null;

    this.route.params.subscribe(params => {
      id = params.id;
    });

    if (id != null) {
      this.conference = await this.conferenceService.getConference(id).toPromise();
      this.conferenceService.currentConference = this.conference;
      console.log('current conference set!');
    }
  }

}
