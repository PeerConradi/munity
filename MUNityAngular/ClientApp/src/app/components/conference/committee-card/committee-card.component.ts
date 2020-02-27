import { Component, OnInit, Input } from '@angular/core';
import { Committee } from '../../../models/committee.model';
import { ConferenceServiceService } from '../../../services/conference-service.service';
import { CommitteeStatus } from '../../../models/committee-status.model';

@Component({
  selector: 'app-committee-card',
  templateUrl: './committee-card.component.html',
  styleUrls: ['./committee-card.component.css']
})
export class CommitteeCardComponent implements OnInit {

  @Input() committee: Committee

  status: CommitteeStatus

  constructor(private service: ConferenceServiceService) { }

  ngOnInit() {
    this.service.getCommitteeStatus(this.committee.ID).subscribe(n => this.status = n);
  }

}
