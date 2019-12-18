import { Component, OnInit } from '@angular/core';
import { ResolutionService } from '../../../services/resolution.service';

@Component({
  selector: 'app-res-view',
  templateUrl: './res-view.component.html',
  styleUrls: ['./res-view.component.css']
})
export class ResViewComponent implements OnInit {

  constructor(public service: ResolutionService) { }

  ngOnInit() {
  }

}
