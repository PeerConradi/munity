import { Component, OnInit } from '@angular/core';
import { SpeakerListService } from '../../services/speaker-list.service';
import { ConferenceService } from '../../services/conference-service.service';

@Component({
  selector: 'app-default-layout',
  templateUrl: './default-layout.component.html',
  styleUrls: ['./default-layout.component.css']
})
export class DefaultLayoutComponent implements OnInit {

  showRightPanel: boolean = false;

  showLeftPanel: boolean = false;

  constructor(public speakerListService: SpeakerListService, public conferenceService: ConferenceService) { }

  ngOnInit() {
  }

  toggleRightPanel() {
    this.showRightPanel = !this.showRightPanel;
  }

}
