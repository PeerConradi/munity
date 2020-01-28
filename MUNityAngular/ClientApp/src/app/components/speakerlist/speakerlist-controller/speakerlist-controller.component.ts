import { Component, OnInit } from '@angular/core';
import { Delegation } from 'src/app/models/delegation.model';

@Component({
  selector: 'app-speakerlist-controller',
  templateUrl: './speakerlist-controller.component.html',
  styleUrls: ['./speakerlist-controller.component.css']
})
export class SpeakerlistControllerComponent implements OnInit {

  speakers: Delegation[] = [];

  addGuestSpeakerModalOpened = false;
  secretaryModalOpened = false;

  listStyle = {
    width: '100%', //width of the list defaults to 300,
    height: '250px', //height of the list defaults to 250,
    dropZoneHeight: '10px' // height of the dropzone indicator defaults to 50
  }

  constructor() {

  }

  ngOnInit() {
    for (let i = 0; i < 10; i++) {
      const delegation = new Delegation();
      delegation.ID = 'del' + i;
      delegation.Name = 'Delegation ' + i;
      this.speakers.push(delegation);
    }
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
