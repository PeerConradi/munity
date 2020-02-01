import { Component, OnInit, Input } from '@angular/core';
import { Resolution } from '../../../models/resolution.model';
import { UserService } from '../../../services/user.service';
import { ResolutionService } from '../../../services/resolution.service';

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

  onlineid: string = null;

  publicView: boolean = false;

  constructor(private userService: UserService, private resolutionService: ResolutionService) { }

  ngOnInit() {
    //this.updateOnlineInfos();
  }

  updateOnlineInfos() {
    this.resolutionService.getAdvancedInfos(this.resolution.ID).subscribe(n => {
      this.onlineid = n.OnlineCode;
      this.publicView = n.PublicRead;
      console.log(n);
    });
  }

  onEnterTitle(value: string) { this.resolutionService.changeTitle(this.resolution.ID, value); }

  onEnterCommittee(value: string) { this.resolutionService.changeCommittee(this.resolution.ID, value); }

  onEnterSubmitter(value: string) { }
}
