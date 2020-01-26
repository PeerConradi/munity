import { Component, OnInit, Input } from '@angular/core';
import { ResolutionService } from '../../../services/resolution.service';
import { Resolution } from 'src/app/models/resolution.model';

@Component({
  selector: 'app-res-view',
  templateUrl: './res-view.component.html',
  styleUrls: ['./res-view.component.css']
})
export class ResViewComponent implements OnInit {

  model: Resolution;

  @Input() resolution: Resolution;

  constructor(public service: ResolutionService) { }

  ngOnInit() {
  }

}
