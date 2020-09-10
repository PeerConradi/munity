import { Component, OnInit, Input } from '@angular/core';
import { Resolution } from '../../../models/resolution.model';
import { UserService } from '../../../services/user.service';
import { ResolutionService } from '../../../services/resolution.service';
import { Delegation } from '../../../models/conference/delegation.model';
import { ConferenceService } from '../../../services/conference-service.service';
import { ChangeResolutionHeaderRequest } from '../../../models/requests/change-resolution-header-request';
import { ResolutionAdvancedInfo } from '../../../models/resolution-advanced-info.model';

@Component({
  selector: 'app-res-options',
  templateUrl: './res-options.component.html',
  styleUrls: ['./res-options.component.css']
})
export class ResOptionsComponent implements OnInit {
  private _resolution: Resolution;

  @Input('resolution')
  set resolution(val: Resolution) {
    this._resolution = val;
    this.updateOnlineInfos();
  }
  get resolution() { return this._resolution;}

  allDelegations: string[] = [];

  resolutionSubmitter: string;

  documentInfo: ResolutionAdvancedInfo = null;

  constructor(private userService: UserService, private resolutionService: ResolutionService,
  private conferenceService: ConferenceService) { }

  ngOnInit() {
    //this.updateOnlineInfos();
    this.resolutionSubmitter = this.resolution.header.submitterName;
  }

  updateOnlineInfos() {
    this.resolutionService.getAdvancedInfos(this.resolution.resolutionId).subscribe(n => {
      this.documentInfo = n;
    });

    this.conferenceService.getAllDelegations().subscribe(n => {
      n.forEach(d => {
        this.allDelegations.push(d.FullName);
      });
    });
  }

  publicModeChanged(e) {
    this.resolutionService.changePublicReadMode(this.resolution.resolutionId, this.documentInfo.PublicRead).subscribe(n => {
      this.documentInfo.OnlineCode = n;
    });
  }

  onEnterTitle(value: string) {
    const request = this.baseRequest();
    request.title = value;
    this.resolutionService.updateHeader(request);
  }

  onEnterCommittee(value: string) {
    const request = this.baseRequest();
    request.committee = value;
    this.resolutionService.updateHeader(request);
  }

  onEnterSubmitter(value: string) {
    const request = this.baseRequest();
    request.submitterName = value;
    this.resolutionService.updateHeader(request);
  }

  baseRequest(): ChangeResolutionHeaderRequest {
    const r = new ChangeResolutionHeaderRequest();
    r.committee = this.resolution.header.committeeName;
    r.resolutionId = this.resolution.resolutionId;
    r.supporters = this.resolution.header.supporters;
    r.title = this.resolution.header.topic;
    r.submitterName = this.resolution.header.submitterName;
    return r;
  }
}
