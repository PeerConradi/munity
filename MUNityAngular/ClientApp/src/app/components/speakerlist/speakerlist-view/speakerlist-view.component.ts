import { Component, OnInit, Input } from '@angular/core';
import { SpeakerListService } from '../../../services/speaker-list.service';
import { ConferenceService } from '../../../services/conference-service.service';
import { ActivatedRoute } from '@angular/router';
import { Speakerlist } from '../../../models/speakerlist.model';
import { TimeSpan } from '../../../models/TimeSpan';
import { Delegation } from '../../../models/conference/delegation.model';

@Component({
  selector: 'app-speakerlist-view',
  templateUrl: './speakerlist-view.component.html',
  styleUrls: ['./speakerlist-view.component.css']
})
export class SpeakerlistViewComponent implements OnInit {

  private _speakerlist: Speakerlist;

  @Input('speakerlist')
  public set speakerlist(list: Speakerlist) {
    console.log('Speakerlist changed: ')
    console.log(list);
    if (list != null) {
      this._speakerlist = list;
      if (list.publicId != null) {
        this.speakerlistService.subscribeToSpeakerlist(list.publicId.toString());
        this.speakerlistService.addSpeakerlistListener(this._speakerlist);
      }
      const sTime = new TimeSpan(list.remainingSpeakerTime.totalMilliseconds, 0, 0, 0, 0);
      this._speakerlist.remainingSpeakerTime = sTime;
      const qTime = new TimeSpan(list.remainingQuestionTime.totalMilliseconds, 0, 0, 0, 0);
      this._speakerlist.remainingQuestionTime = qTime;
    } else {
      this._speakerlist = null;
    }
  }

  public get speakerlist(): Speakerlist {
    return this._speakerlist;
  }

  public get remainingSpeakerTime(): string {
    if (this.speakerlist == null || this.speakerlist.remainingSpeakerTime == null)
      return '--:--';
    return this.speakerlist.remainingSpeakerTime.toString();
  }

  public get remainingQuestionTime(): string {
    if (this.speakerlist == null || this.speakerlist.remainingQuestionTime == null)
      return '--:--';
    return this.speakerlist.remainingQuestionTime.toString();
  }

  @Input() sizing: string = "big";

  interval: NodeJS.Timeout;

  constructor(private conferenceService: ConferenceService,
    private speakerlistService: SpeakerListService,
    private route: ActivatedRoute) { }

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
        this.speakerlist = list;
        //this.speakerlistService.subscribeToSpeakerlist(list.PublicId.toString());
        //this.speakerlistService.addSpeakerlistListener(this.speakerlist);
      });
    }

    this.interval = setInterval(() => {
      //Status 0: Beide Listen gestoppt
      //Status 1: Redebeitrag
      //Status 2: Frage/Kurzbemerkung
      if (this.speakerlist != null && this.speakerlist.remainingSpeakerTime != null && this.speakerlist.remainingQuestionTime != null)
        if (this.speakerlist.status == 1) {
          this.speakerlist.remainingSpeakerTime.addSeconds(-1);
        } else if (this.speakerlist.status == 2) {
          this.speakerlist.remainingQuestionTime.addSeconds(-1);
        }
    }, 1000);
  }

  getMediumImage(delegation: Delegation): string {
    // if (delegation.type == 'COUNTRY') {
    //   return '/assets/img/flags/medium/' + delegation.roleShort + '.png';
    // }
    //Default Image
    return '/assets/img/flags/medium/un.png';
  }

  getDelegationImagePath(delegation: Delegation): string {
    // if (delegation.type == 'COUNTRY') {
    //   return '/assets/img/flags/small/' + delegation.roleShort + '.png';
    // }
    //Default Image
    return '/assets/img/flags/small/un.png';
  }

}
