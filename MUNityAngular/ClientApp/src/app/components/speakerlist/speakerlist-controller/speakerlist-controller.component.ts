import { Component, OnInit } from '@angular/core';
import { Delegation } from 'src/app/models/delegation.model';
import { ConferenceServiceService } from '../../../services/conference-service.service';
import { TimeSpan } from '../../../models/TimeSpan';
import { Speakerlist } from '../../../models/speakerlist.model';
import { SpeakerListService } from '../../../services/speaker-list.service';
import { ActivatedRoute } from '@angular/router';
import { NotifierService } from 'angular-notifier';
import { TypeaheadMatch } from 'ngx-bootstrap/typeahead/public_api';

@Component({
  selector: 'app-speakerlist-controller',
  templateUrl: './speakerlist-controller.component.html',
  styleUrls: ['./speakerlist-controller.component.css']
})
export class SpeakerlistControllerComponent implements OnInit {

  public speakerlist: Speakerlist;

  addSpeakerSelection: string;
  addQuestionSelection: string;
  presetDelegations: Delegation[] = [];

  addGuestSpeakerModalOpened = false;
  secretaryModalOpened = false;
  interval: NodeJS.Timeout;
  timeLeft: number;
  deleteItems: any;
  lastSpeakerOrder: Delegation[] = [];

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

        this.speakerlist = list;
        this.speakerlistService.subscribeToSpeakerlist(list.PublicId.toString());
        this.speakerlistService.addSpeakerlistListener(this.speakerlist);

        list.Speakers.forEach(n => {
          this.lastSpeakerOrder.push(n);
        });
        
        const sTime = new TimeSpan(list.RemainingSpeakerTime.TotalMilliseconds,0,0,0,0);
        this.speakerlist.RemainingSpeakerTime = sTime;
        const qTime = new TimeSpan(list.RemainingQuestionTime.TotalMilliseconds, 0, 0, 0, 0);
        this.speakerlist.RemainingQuestionTime = qTime;

        const tsTime = new TimeSpan(0, 0, 0, 0, 0);
        tsTime.addSeconds(list.Speakertime.TotalSeconds);
        this.speakerlist.Speakertime = tsTime;

        const tqTime = new TimeSpan(list.Questiontime.TotalMilliseconds, 0, 0, 0, 0);
        this.speakerlist.Questiontime = tqTime;

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
      //Status 0: Beide Listen gestoppt
      //Status 1: Redebeitrag
      //Status 2: Frage/Kurzbemerkung
      if (this.speakerlist.Status == 1) {
        this.speakerlist.RemainingSpeakerTime.addSeconds(-1);
      } else if (this.speakerlist.Status == 2) {
        this.speakerlist.RemainingQuestionTime.addSeconds(-1);
      }
    }, 1000);
  }

  listOrderChanged(newOrder) {
    console.log(newOrder);
  }

  removeSpeaker() {
    const items: Delegation[] = this.deleteItems;
    if (items != null && items.length > 0) {
      items.forEach(n => {
        console.log('remove: ');
        console.log(n);
        this.speakerlistService.removeSpeaker(this.speakerlist.ID, n.ID).subscribe();
      });
      this.deleteItems = [];
    }
    
    //this.speakerlistService.removeSpeaker(this.speakerlist.ID, id);
  }

  reorderSpeakers() {
    //Zunächst ignorieren wir hier quereinsteiger
    if (this.lastSpeakerOrder.length == this.speakerlist.Speakers.length) {
      let orderChanged = false;

      this.speakerlist.Speakers.forEach((val, index) => {

        if (val.ID != this.lastSpeakerOrder[index].ID) {
          orderChanged = true;
        }
      });
      if (orderChanged) {
        console.log('Reorder');

        this.lastSpeakerOrder = [];
        this.speakerlist.Speakers.forEach(n => {
          this.lastSpeakerOrder.push(n);
        });
        this.speakerlistService.reorderSpeaker(this.speakerlist.ID, this.speakerlist.Speakers).subscribe();
      }
    } else {
      this.lastSpeakerOrder = [];
      this.speakerlist.Speakers.forEach(n => {
        this.lastSpeakerOrder.push(n);
      });
    }
    
  }

  addSpeaker() {
    const s = this.presetDelegations.find(n => n.Name == this.addSpeakerSelection);
    if (s != null) {
      this.speakerlistService.addSpeaker(this.speakerlist.ID, s.ID).subscribe();
    } else {
      const newDelegation: Delegation = new Delegation();
      newDelegation.Name = this.addSpeakerSelection;
      newDelegation.FullName = this.addSpeakerSelection;
      this.speakerlistService.addSpeakerModel(this.speakerlist.ID, newDelegation).subscribe();
    }
    
  }

  onAddSpeakerSelected(val: TypeaheadMatch) {
    if (val !== null) {
      const del: Delegation = val.item;
      this.speakerlistService.addSpeakerModel(this.speakerlist.ID, del).subscribe(n => {
        this.addSpeakerSelection = '';
      });
    }
  }

  onAddQuestionSelected(val: TypeaheadMatch) {
    if (val !== null) {
      const del: Delegation = val.item;
      this.speakerlistService.addQuestionModel(this.speakerlist.ID, del).subscribe(n => {
        this.addQuestionSelection = '';
      });
    }
  }

  addQuestion() {
    let s = this.presetDelegations.find(n => n.Name == this.addQuestionSelection);
    if (s == null) {
      s = new Delegation();
      s.Name = this.addQuestionSelection;
      s.FullName = this.addQuestionSelection;
    }
    this.speakerlistService.addQuestionModel(this.speakerlist.ID, s).subscribe();
  }

  nextSpeaker() {
    this.speakerlistService.nextSpeaker(this.speakerlist.ID).subscribe();
  }

  nextQuestion() {
    this.speakerlistService.nextQuestion(this.speakerlist.ID).subscribe();
  }

  toggleSpeaker() {
    if (this.speakerlist.Status == 1) {
      this.speakerlistService.stopTimer(this.speakerlist.ID).subscribe();
    } else {
      this.speakerlistService.startSpeaker(this.speakerlist.ID).subscribe();
    }
  }

  startAnswer() {
    this.speakerlistService.startAnswer(this.speakerlist.ID).subscribe();
  }

  setSpeakertime(val: string) {
    this.speakerlistService.setSpeakertime(this.speakerlist.ID, val).subscribe();
  }

  setQuestiontime(val: string) {
    this.speakerlistService.setQuestionTime(this.speakerlist.ID, val).subscribe();
  }

  toggleQuestion() {
    if (this.speakerlist.Status == 2) {
      this.speakerlistService.stopTimer(this.speakerlist.ID).subscribe();
    } else {
      this.speakerlistService.startQuestion(this.speakerlist.ID).subscribe();
    }
  }

  clearSpeaker() {
    this.speakerlistService.clearSpeaker(this.speakerlist.ID).subscribe();
  }

  clearQuestion() {
    this.speakerlistService.clearQuestion(this.speakerlist.ID).subscribe();
  }

  closeAddGuestSpeakerModal() {
    this.addGuestSpeakerModalOpened = false;
  }

  openGuestSpeakerDialog() {
    this.addGuestSpeakerModalOpened = true;
  }

  openSecretaryModal() {
    this.secretaryModalOpened = true;
  }

  closeSecretaryModal() {
    this.secretaryModalOpened = false;
  }

  dockSpeakerlist() {
    this.speakerlistService.currentList = this.speakerlist;
  }
}
