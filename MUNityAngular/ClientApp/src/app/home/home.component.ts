import { Component } from '@angular/core';
import { ConferenceServiceService } from '../services/conference-service.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
})
export class HomeComponent {

  public conferences;

  constructor(private conferenceSerice: ConferenceServiceService) {
    this.conferences = conferenceSerice.getAllConferences().subscribe(n => {
      this.conferences = n;
      console.log(this.conferences);
    });

  }

}
