import { Component, OnInit } from '@angular/core';
import { ConferenceService } from '../../../services/conference-service.service';
import { ActivatedRoute } from '@angular/router';
import { Delegation } from '../../../models/conference/delegation.model';
import { PresenceService } from '../../../services/presence.service';
import { Presence } from '../../../models/presence.model';
import { NotifierService } from 'angular-notifier';

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

  constructor(private conferenceSerivce: ConferenceService, private route: ActivatedRoute, private presenceService: PresenceService,
  private notifier: NotifierService) { }

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
    let index = this.allDelegations.indexOf(delegation);
    if (index > -1) {
      this.allDelegations.splice(index, 1);
    }
    index = this.missingDelegations.indexOf(delegation);
    if (index > -1) {
      this.missingDelegations.splice(index, 1);
    }
    this.presentDelegations.push(delegation);
  }

  setMissing(delegation: Delegation) {
    let index = this.allDelegations.indexOf(delegation);
    if (index > -1) {
      this.allDelegations.splice(index, 1);
    }
    index = this.presentDelegations.indexOf(delegation);
    if (index > -1) {
      this.presentDelegations.splice(index, 1);
    }
    this.missingDelegations.push(delegation);
  }

  reset() {
    this.presentDelegations.forEach(n => this.allDelegations.push(n));
    this.missingDelegations.forEach(n => this.allDelegations.push(n));
    this.presentDelegations = [];
    this.missingDelegations = [];
  }

  saveList() {
    const model = new Presence();
    model.Present = this.presentDelegations;
    model.Absent = this.missingDelegations;
    model.Remaining = this.allDelegations;
    model.CommitteeId = this.committeeId;
    this.presenceService.savePresence(model).subscribe(n => this.notifier.notify('success', 'Liste gespeichert!'));
  }
}
