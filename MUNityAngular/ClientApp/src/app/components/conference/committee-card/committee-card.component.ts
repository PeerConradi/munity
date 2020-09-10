import { Component, OnInit, Input } from '@angular/core';
import { Committee } from '../../../models/conference/committee.model';
import { ConferenceService } from '../../../services/conference-service.service';
import { CommitteeStatus } from '../../../models/conference/committee-status.model';
import { Presence } from '../../../models/presence.model';
import { PresenceService } from '../../../services/presence.service';

@Component({
  selector: 'app-committee-card',
  templateUrl: './committee-card.component.html',
  styleUrls: ['./committee-card.component.css']
})
export class CommitteeCardComponent implements OnInit {

  @Input() committee: Committee

  status: CommitteeStatus

  presence: Presence;

  constructor(private service: ConferenceService, private presenceService: PresenceService) { }

  ngOnInit() {
    this.service.getCommitteeStatus(this.committee.CommitteeId).subscribe(n => this.status = n);
    this.presenceService.getLatestPresence(this.committee.CommitteeId).subscribe(n => this.presence = n); 
  }

}
