import { Component, OnInit } from '@angular/core';
import { ConferenceServiceService } from '../../../services/conference-service.service';

@Component({
  selector: 'app-conference-list',
  templateUrl: './conference-list.component.html',
  styleUrls: ['./conference-list.component.css']
})
export class ConferenceListComponent implements OnInit {

  public conferences = [];

  constructor(private conferenceService: ConferenceServiceService) { }

  ngOnInit() {
    this.conferences = null;
    this.conferenceService.getAllConferences().subscribe(n => {
      this.conferences = n;
      console.log(this.conferences);
    });
  }

  rename(id: string) {
    console.log('Renaming: ' + id);
    this.conferenceService.changeConferenceName(id, '123456', 'Rofl').subscribe(n => { console.log('success')}, error => {
      console.log('Well this sucks: ');
      console.log(error);
    });
  }

}
