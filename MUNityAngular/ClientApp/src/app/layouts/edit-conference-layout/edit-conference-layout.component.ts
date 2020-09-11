import { Component, OnInit } from '@angular/core';
import { Conference } from '../../models/conference/conference.model';
import { ActivatedRoute, Router } from '@angular/router';
import { ConferenceService } from '../../services/conference-service.service';

@Component({
  selector: 'app-edit-conference-layout',
  templateUrl: './edit-conference-layout.component.html',
  styleUrls: ['./edit-conference-layout.component.css']
})
export class EditConferenceLayoutComponent implements OnInit {


  constructor() { }

  ngOnInit() {

  }

}
