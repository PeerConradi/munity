import { Component, OnInit } from '@angular/core';
import { SpeakerListService } from '../../../services/speaker-list.service';
import { ConferenceServiceService } from '../../../services/conference-service.service';
import { ActivatedRoute } from '@angular/router';
import { Speakerlist } from '../../../models/speakerlist.model';
import { TimeSpan } from '../../../models/TimeSpan';
import { Delegation } from '../../../models/delegation.model';

@Component({
  selector: 'app-speakerlist-view',
  templateUrl: './speakerlist-view.component.html',
  styleUrls: ['./speakerlist-view.component.css']
})
export class SpeakerlistViewComponent implements OnInit {

  speakerlist: Speakerlist;
  interval: NodeJS.Timeout;

  constructor(private conferenceService: ConferenceServiceService,
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
        this.speakerlistService.subscribeToSpeakerlist(list.PublicId.toString());
        this.speakerlistService.addSpeakerlistListener(this.speakerlist);



        const sTime = new TimeSpan(list.RemainingSpeakerTime.TotalMilliseconds, 0, 0, 0, 0);
        this.speakerlist.RemainingSpeakerTime = sTime;
        const qTime = new TimeSpan(list.RemainingQuestionTime.TotalMilliseconds, 0, 0, 0, 0);
        this.speakerlist.RemainingQuestionTime = qTime;

        //const tsTime = new TimeSpan(0, 0, 0, 0, 0);
        //tsTime.addSeconds(list.Speakertime.TotalSeconds);
        //this.speakerlist.Speakertime = tsTime;

        //const tqTime = new TimeSpan(list.Questiontime.TotalMilliseconds, 0, 0, 0, 0);
        //this.speakerlist.Questiontime = tqTime;
      })
    }

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

  getMediumImage(delegation: Delegation): string {
    if (delegation.TypeName == 'COUNTRY') {
      return '/assets/img/flags/medium/' + delegation.IconName + '.png';
    }
    //Default Image
    return '/assets/img/flags/medium/un.png';
  }

  getDelegationImagePath(delegation: Delegation): string {
    if (delegation.TypeName == 'COUNTRY') {
      return '/assets/img/flags/small/' + delegation.IconName + '.png';
    }
    //Default Image
    return '/assets/img/flags/small/un.png';
  }

}
