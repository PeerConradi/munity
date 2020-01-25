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

  @Input() resolution: Resolution;

  constructor(private userService: UserService, private resolutionService: ResolutionService) { }

  ngOnInit() {
  }

  onEnterTitle(value: string) { this.resolutionService.changeTitle(this.resolution.ID, value); }

}
