import { Component, OnInit, Input } from '@angular/core';
import { Speakerlist } from '../../../models/speakerlist.model';
import { SpeakerListService } from '../../../services/speaker-list.service';

@Component({
  selector: 'app-speakerlist-panel',
  templateUrl: './speakerlist-panel.component.html',
  styleUrls: ['./speakerlist-panel.component.css']
})
export class SpeakerlistPanelComponent implements OnInit {

  @Input() speakerlist: Speakerlist;

  constructor(private speakerlistService: SpeakerListService) { }

  ngOnInit() {
  }

  //addSpeaker() {
  //  const s = this.presetDelegations.find(n => n.Name == this.addSpeakerSelection);
  //  this.speakerlistService.addSpeaker(this.speakerlist.ID, s.ID).subscribe();
  //}

  //addQuestion() {
  //  const s = this.presetDelegations.find(n => n.Name == this.addQuestionSelection);
  //  this.speakerlistService.addQuestion(this.speakerlist.ID, s.ID).subscribe();
  //}

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
      this.speakerlistService.startSpeaker(this.speakerlist.ID, this.speakerlist.RemainingSpeakerTime.TotalSeconds).subscribe();
    }
  }

  toggleQuestion() {
    if (this.speakerlist.Status == 2) {
      this.speakerlistService.stopTimer(this.speakerlist.ID).subscribe();
    } else {
      this.speakerlistService.startQuestion(this.speakerlist.ID, this.speakerlist.RemainingQuestionTime.TotalSeconds).subscribe();
    }
  }

}
