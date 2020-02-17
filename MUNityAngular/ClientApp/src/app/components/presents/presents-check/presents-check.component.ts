import { Component, OnInit } from '@angular/core';
import { ConferenceServiceService } from '../../../services/conference-service.service';
import { ActivatedRoute } from '@angular/router';
import { Delegation } from '../../../models/delegation.model';

@Component({
  selector: 'app-presents-check',
  templateUrl: './presents-check.component.html',
  styleUrls: ['./presents-check.component.css']
})
export class PresentsCheckComponent implements OnInit {

  allDelegations: Delegation[];

  presentDelegations: Delegation[] = [];

  missingDelegations: Delegation[] = [];

  committeeId: string;

  get absoluteMajority(): number {
    if (this.presentDelegations.length <= 0)
      return 0;
    return Math.floor(this.presentDelegations.length / 2) + 1;
  }

  get twoThirdMajority(): number {
    if (this.presentDelegations.length <= 0)
      return 0;
    return Math.floor(this.presentDelegations.length / 3 * 2) + 1;
  }

  get presentFactor(): number {
    if (this.presentDelegations.length == 0 && this.missingDelegations.length == 0)
      return 0;
    return Math.round(this.presentDelegations.length / (this.presentDelegations.length + this.missingDelegations.length) * 100);
  }

  constructor(private conferenceSerivce: ConferenceServiceService, private route: ActivatedRoute) { }

  ngOnInit() {
    //Get the ID of the context committee
    let id: string = null;
    this.route.params.subscribe(params => {
      id = params.id;
    })
    if (id == null) {
      id = this.route.snapshot.queryParamMap.get('id');
    }
    this.committeeId = id;
    if (id != null) {
      this.conferenceSerivce.getDelegationsOfCommittee(id).subscribe(n => {
        this.allDelegations = n;
      });
    }
  }

  setPresent(delegation: Delegation) {
    const index = this.allDelegations.indexOf(delegation);
    if (index > -1) {
      this.allDelegations.splice(index, 1);
    }
    this.presentDelegations.push(delegation);
  }

  setMissing(delegation: Delegation) {
    const index = this.allDelegations.indexOf(delegation);
    if (index > -1) {
      this.allDelegations.splice(index, 1);
    }
    this.missingDelegations.push(delegation);
  }
}
