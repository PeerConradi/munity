import { Component, OnInit } from '@angular/core';
import { ConferenceService } from '../../../services/conference-service.service';
import { AdminService } from '../../../services/admin.service';

@Component({
  selector: 'app-conference-list',
  templateUrl: './conference-list.component.html',
  styleUrls: ['./conference-list.component.css']
})
export class ConferenceListComponent implements OnInit {

  public conferences = [];

  constructor(private conferenceService: ConferenceService, private adminService: AdminService) { }

  ngOnInit() {
    this.conferences = null;
    this.adminService.getConferences().subscribe(n => {
      this.conferences = n;
    });
  }

  rename(id: string) {
    //this.conferenceService.changeConferenceName(id, 'Rofl').subscribe();
  }

}
