import { Component, OnInit } from '@angular/core';
import { Delegation } from 'src/app/models/conference/delegation.model';
import { ConferenceService } from '../../../services/conference-service.service';
import { TimeSpan } from '../../../models/TimeSpan';
import { Speaker, Speakerlist } from '../../../models/speakerlist.model';
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
  lastSpeakerOrder: Speaker[] = [];
  lastQuestionOrder: Speaker[] = [];

  constructor(private conferenceService: ConferenceService,
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
        this.speakerlistService.subscribeToSpeakerlist(list.publicId.toString());
        this.speakerlistService.addSpeakerlistListener(this.speakerlist);

        list.speakers.forEach(n => {
          this.lastSpeakerOrder.push(n);
        });

        list.questions.forEach(n => {
          this.lastQuestionOrder.push(n);
        })

        const sTime = new TimeSpan(list.remainingSpeakerTime.totalMilliseconds, 0, 0, 0, 0);
        this.speakerlist.remainingSpeakerTime = sTime;
        const qTime = new TimeSpan(list.remainingQuestionTime.totalMilliseconds, 0, 0, 0, 0);
        this.speakerlist.remainingQuestionTime = qTime;

        const tsTime = new TimeSpan(0, 0, 0, 0, 0);
        tsTime.addSeconds(list.speakertime.totalSeconds);
        this.speakerlist.speakertime = tsTime;

        const tqTime = new TimeSpan(list.questiontime.totalMilliseconds, 0, 0, 0, 0);
        this.speakerlist.questiontime = tqTime;

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
      if (this.speakerlist.status == 1) {
        this.speakerlist.remainingSpeakerTime.addSeconds(-1);
      } else if (this.speakerlist.status == 2) {
        this.speakerlist.remainingQuestionTime.addSeconds(-1);
      }
    }, 1000);
  }

  listOrderChanged(newOrder) {

  }

  removeSpeaker() {
    const items: Delegation[] = this.deleteItems;
    if (items != null && items.length > 0) {
      items.forEach(n => {
        this.speakerlistService.removeSpeaker(this.speakerlist.id, n.delegationId).subscribe();
      });
      this.deleteItems = [];
    }

    //this.speakerlistService.removeSpeaker(this.speakerlist.ID, id);
  }

  reorderSpeakers() {
    //Zunächst ignorieren wir hier quereinsteiger
    if (this.lastSpeakerOrder.length == this.speakerlist.speakers.length) {
      let orderChanged = false;

      this.speakerlist.speakers.forEach((val, index) => {

        if (val.id != this.lastSpeakerOrder[index].id) {
          orderChanged = true;
        }
      });
      if (orderChanged) {
        this.lastSpeakerOrder = [];
        this.speakerlist.speakers.forEach(n => {
          this.lastSpeakerOrder.push(n);
        });
        this.speakerlistService.reorderSpeaker(this.speakerlist.id, this.speakerlist.speakers).subscribe();
      }
    } else {
      this.lastSpeakerOrder = [];
      this.speakerlist.speakers.forEach(n => {
        this.lastSpeakerOrder.push(n);
      });
    }

  }

  reorderQuestions() {
    if (this.lastQuestionOrder.length == this.speakerlist.questions.length) {
      let orderChanged = false;

      this.speakerlist.questions.forEach((val, index) => {

        if (val.id != this.lastQuestionOrder[index].id) {
          orderChanged = true;
        }
      });
      if (orderChanged) {
        this.lastQuestionOrder = [];
        this.speakerlist.questions.forEach(n => {
          this.lastQuestionOrder.push(n);
        });
        this.speakerlistService.reorderQuestion(this.speakerlist.id, this.speakerlist.questions).subscribe();
      }
    } else {
      this.lastQuestionOrder = [];
      this.speakerlist.questions.forEach(n => {
        this.lastSpeakerOrder.push(n);
      });
    }
  }

  addSpeaker() {
    const s = this.presetDelegations.find(n => n.name == this.addSpeakerSelection);
    if (s != null) {
      // outdated because speakerlist no longer works with Delegation
      this.speakerlistService.addSpeaker(this.speakerlist.id, s.delegationId).subscribe();
    } else {
      const newDelegation: Speaker = new Speaker();
      newDelegation.name = this.addSpeakerSelection;
      this.speakerlistService.addSpeakerModel(this.speakerlist.id, newDelegation).subscribe();
    }

  }

  addQuestion() {
    let s = this.presetDelegations.find(n => n.name == this.addQuestionSelection);
    if (s != null) {
      // outdated because speaker list no longer works with Delegation
      this.speakerlistService.addQuestion(this.speakerlist.id, s.delegationId).subscribe();
    }
    else {
      const newQuestion: Speaker = new Speaker();
      newQuestion.name = this.addQuestionSelection;
      this.speakerlistService.addQuestionModel(this.speakerlist.id, newQuestion).subscribe();
    }
  }

  onAddSpeakerSelected(val: TypeaheadMatch) {
    if (val !== null) {
      const del: Speaker = val.item;
      this.speakerlistService.addSpeakerModel(this.speakerlist.id, del).subscribe(n => {
        this.addSpeakerSelection = '';
      });
    }
  }

  onAddQuestionSelected(val: TypeaheadMatch) {
    if (val !== null) {
      const del: Speaker = val.item;
      this.speakerlistService.addQuestionModel(this.speakerlist.id, del).subscribe(n => {
        this.addQuestionSelection = '';
      });
    }
  }



  nextSpeaker() {
    this.speakerlistService.nextSpeaker(this.speakerlist.id).subscribe();
  }

  nextQuestion() {
    this.speakerlistService.nextQuestion(this.speakerlist.id).subscribe();
  }

  toggleSpeaker() {
    if (this.speakerlist.status == 1) {
      this.speakerlistService.stopTimer(this.speakerlist.id).subscribe();
    } else {
      this.speakerlistService.startSpeaker(this.speakerlist.id).subscribe();
    }
  }

  startAnswer() {
    this.speakerlistService.startAnswer(this.speakerlist.id).subscribe();
  }

  setSpeakertime(val: string) {
    this.speakerlistService.setSpeakertime(this.speakerlist.id, val).subscribe();
  }

  setQuestiontime(val: string) {
    this.speakerlistService.setQuestionTime(this.speakerlist.id, val).subscribe();
  }

  toggleQuestion() {
    if (this.speakerlist.status == 2) {
      this.speakerlistService.stopTimer(this.speakerlist.id).subscribe();
    } else {
      this.speakerlistService.startQuestion(this.speakerlist.id).subscribe();
    }
  }

  clearSpeaker() {
    this.speakerlistService.clearSpeaker(this.speakerlist.id).subscribe();
  }

  clearQuestion() {
    this.speakerlistService.clearQuestion(this.speakerlist.id).subscribe();
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
