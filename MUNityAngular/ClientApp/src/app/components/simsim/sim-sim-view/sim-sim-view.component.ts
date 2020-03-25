import { Component, OnInit } from '@angular/core';
import { Speakerlist } from '../../../models/speakerlist.model';
import { TimeSpan } from '../../../models/TimeSpan';

@Component({
  selector: 'app-sim-sim-view',
  templateUrl: './sim-sim-view.component.html',
  styleUrls: ['./sim-sim-view.component.css']
})
export class SimSimViewComponent implements OnInit {

  speakerlist: Speakerlist;

  constructor() { }

  ngOnInit() {
    let slist = new Speakerlist();
    slist.Speakertime = new TimeSpan(0, 0, 3, 0, 0);
    slist.Questiontime = new TimeSpan(0, 30, 0, 0, 0);
    slist.RemainingSpeakerTime = new TimeSpan(0, 0, 3, 0, 0);
    slist.RemainingQuestionTime = new TimeSpan(0, 30, 0, 0, 0);
    slist.Status = 0;
    this.speakerlist = slist;
  }

}
