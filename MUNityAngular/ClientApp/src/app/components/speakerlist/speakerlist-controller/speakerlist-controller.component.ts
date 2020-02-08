import { Component, OnInit } from '@angular/core';
import { Delegation } from 'src/app/models/delegation.model';
import { ConferenceServiceService } from '../../../services/conference-service.service';
import { Time } from '@angular/common';
import { Observable } from 'rxjs';
import { TimeSpan } from '../../../models/TimeSpan';
import { Speakerlist } from '../../../models/speakerlist.model';
import { SpeakerListService } from '../../../services/speaker-list.service';
import { ActivatedRoute } from '@angular/router';
import { NotifierService } from 'angular-notifier';

@Component({
  selector: 'app-speakerlist-controller',
  templateUrl: './speakerlist-controller.component.html',
  styleUrls: ['./speakerlist-controller.component.css']
})
export class SpeakerlistControllerComponent implements OnInit {

  public speakerlist: Speakerlist;

  addSpeakerSelection: string;
  presetDelegations: Delegation[] = [];

  addGuestSpeakerModalOpened = false;
  secretaryModalOpened = false;
  interval: NodeJS.Timeout;
  timeLeft: number;


  constructor(private conferenceService: ConferenceServiceService,
    private speakerlistService: SpeakerListService,
    private route: ActivatedRoute,
    private notifier: NotifierService) {

  }

  ngOnInit() {
    let id: string = null;
    this.route.params.subscribe(params => {
      id = params.id;
    })
    if (id == null) {
      id = this.route.snapshot.queryParamMap.get('id');
    }

    if (id != null) {
      //Get the Speakerlist
      this.speakerlistService.getSpeakerlistById(id).subscribe(list => {
        //In the beginning the object should be set, because TimeSpan is not correctly converted.
        const remainingTimeString: string = list.RemainingSpeakerTime.toString();

        this.speakerlist = list;
        this.speakerlistService.subscribeToSpeakerlist(list.PublicId.toString());
        this.speakerlistService.addSpeakerlistListener(this.speakerlist);
        

        
        const sTime = new TimeSpan(0,0,0,0,0);
        sTime.fromString(remainingTimeString);
        this.speakerlist.RemainingSpeakerTime = sTime;
      })
    }

    //Load the default Preset Delegations
    //Later the Delegations that are bound to the Conference should be loaded to the list
    //NOT ONLY THE ONES THAT ARE BOUND TO THE COMMITTEE!
    this.conferenceService.getAllDelegations().subscribe(n => {
      this.presetDelegations = n;
    });

    //Der Time läuft die ganze Zeit und behält einfach nur den Status der Redeliste im Auge.
    //Die Zeiten müssen hin und wieder einmal gesynct werden.
    this.interval = setInterval(() => {
      if (this.speakerlist.Status == 1) {
        this.speakerlist.RemainingSpeakerTime.addSeconds(-1);
      }
    }, 1000);
  }

  listOrderChanged(newOrder) {
    console.log(newOrder);
  }

  removeSpeaker(id) {
    //let index = -1;
    //this.speakers.forEach(n => {
    //  if (n.ID == id) {
    //    index = this.speakers.indexOf(n);
    //  }
    //});
    //if (index !== -1) {
    //  this.speakers.splice(index, 1);
    //}
    this.speakerlistService.removeSpeaker(this.speakerlist.ID, id);
  }

  addSpeaker() {
    console.log('add Speaker');
    //console.log(this.addSpeakerSelection);
    const s = this.presetDelegations.find(n => n.Name == this.addSpeakerSelection);
    //console.log(s);
    //const list: Delegation[] = [];
    //this.speakers.forEach(n => {
    //  list.push(n);
    //});
    //if (s != null) {
    //  list.push(s);
    //}
    //this.speakers = list;
    //console.log(this.speakers);
    this.speakerlistService.addSpeaker(this.speakerlist.ID, s.ID).subscribe(n => {
      this.notifier.notify('success', s.Name + " zur Redeliste hinzufügt.");
    });
  }

  nextSpeaker() {
    this.speakerlistService.nextSpeaker(this.speakerlist.ID).subscribe();
  }

  startSpeakerlist() {
    this.speakerlistService.startSpeaker(this.speakerlist.ID, this.speakerlist.RemainingSpeakerTime.TotalSeconds).subscribe();
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
