import { Component, OnInit } from '@angular/core';
import { SpeakerListService } from '../../../services/speaker-list.service';
import { ConferenceServiceService } from '../../../services/conference-service.service';
import { ActivatedRoute } from '@angular/router';
import { Speakerlist } from '../../../models/speakerlist.model';
import { TimeSpan } from '../../../models/TimeSpan';

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
        //In the beginning the object should be set, because TimeSpan is not correctly converted.
        const remainingTimeString: string = list.RemainingSpeakerTime.toString();
        const remainingQuestionTimeString: string = list.RemainingQuestionTime.toString();

        this.speakerlist = list;
        this.speakerlistService.subscribeToSpeakerlist(list.PublicId.toString());
        this.speakerlistService.addSpeakerlistListener(this.speakerlist);



        const sTime = new TimeSpan(0, 0, 0, 0, 0);
        sTime.fromString(remainingTimeString);
        this.speakerlist.RemainingSpeakerTime = sTime;
        const qTime = new TimeSpan(0, 0, 0, 0, 0);
        qTime.fromString(remainingQuestionTimeString);
        this.speakerlist.RemainingQuestionTime = qTime;
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

}
