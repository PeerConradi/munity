import { Component, OnInit } from '@angular/core';
import { Delegation } from 'src/app/models/delegation.model';
import { ConferenceServiceService } from '../../../services/conference-service.service';

@Component({
  selector: 'app-speakerlist-controller',
  templateUrl: './speakerlist-controller.component.html',
  styleUrls: ['./speakerlist-controller.component.css']
})
export class SpeakerlistControllerComponent implements OnInit {

  speakers: Delegation[] = [];
  addSpeakerSelection: string;
  presetDelegations: Delegation[] = [];

  addGuestSpeakerModalOpened = false;
  secretaryModalOpened = false;

  listStyle = {
    width: '100%', //width of the list defaults to 300,
    height: '250px', //height of the list defaults to 250,
    dropZoneHeight: '10px' // height of the dropzone indicator defaults to 50
  }

  constructor(private conferenceService: ConferenceServiceService) {

  }

  ngOnInit() {
    this.conferenceService.getAllDelegations().subscribe(n => {
      this.presetDelegations = n;
    });
    this.speakers = [];
  }

  listOrderChanged(newOrder) {
    console.log(newOrder);
  }

  removeSpeaker(id) {
    let index = -1;
    this.speakers.forEach(n => {
      if (n.ID == id) {
        index = this.speakers.indexOf(n);
      }
    });
    if (index !== -1) {
      this.speakers.splice(index, 1);
    }
  }

  addSpeaker() {
    console.log('add Speaker');
    console.log(this.addSpeakerSelection);
    const s = this.presetDelegations.find(n => n.Name == this.addSpeakerSelection);
    console.log(s);
    const list: Delegation[] = [];
    this.speakers.forEach(n => {
      list.push(n);
    });
    if (s != null) {
      list.push(s);
    }
    this.speakers = list;
    console.log(this.speakers);
    
  }

  closeAddGuestSpeakerModal() {
    this.addGuestSpeakerModalOpened = false;
  }

  openGuestSpeakerDialog() {
    console.log('open');
    this.addGuestSpeakerModalOpened = true;
  }

  openSecretaryModal() {
    this.secretaryModalOpened = true;
  }

  closeSecretaryModal() {
    this.secretaryModalOpened = false;
  }
}
