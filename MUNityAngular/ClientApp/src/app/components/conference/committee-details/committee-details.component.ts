import { Component, OnInit } from '@angular/core';
import { ResolutionService } from '../../../services/resolution.service';
import { ConferenceService } from '../../../services/conference-service.service';
import { ActivatedRoute } from '@angular/router';
import { Committee } from '../../../models/conference/committee.model';
import { ResolutionAdvancedInfo } from '../../../models/resolution-advanced-info.model';
import { CommitteeStatus } from '../../../models/conference/committee-status.model';

@Component({
  selector: 'app-committee-details',
  templateUrl: './committee-details.component.html',
  styleUrls: ['./committee-details.component.css']
})
export class CommitteeDetailsComponent implements OnInit {

  committee: Committee;

  resolutions: ResolutionAdvancedInfo[] = [];

  status: CommitteeStatus;

  colorScheme = {
    domain: ['#5AA454', '#A10A28']
  };

  view: any[] = [700, 400];

  data = [
    {
      "name": "Anwesend",
      "value": 12
    },
    {
      "name": "Abwesend",
      "value": 8
    },
  ]

  constructor(private route: ActivatedRoute, private resolutionService: ResolutionService,
    private conferenceSerivce: ConferenceService) { }

  ngOnInit() {
    if (this.route == null)
      return;

    this.route.params.subscribe(params => {
      this.conferenceSerivce.getCommittee(params.id).subscribe(n => {
        this.committee = n;
      });

      this.resolutionService.getResolutionsOfCommittee(params.id).subscribe(n => {
        this.resolutions = n;
      });
      this.status = new CommitteeStatus();
      this.conferenceSerivce.getCommitteeStatus(params.id).subscribe(n => {
        if (n != null) {
          this.status = n;
        }
        
      });
    });
  }

  updateStatus() {
    this.status.CommitteeId = this.committee.CommitteeId;
    this.conferenceSerivce.setCommitteeStatus(this.status).subscribe();
  }

}
