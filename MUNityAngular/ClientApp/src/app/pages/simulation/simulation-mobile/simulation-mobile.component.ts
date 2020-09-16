import { Component, OnInit } from '@angular/core';
import { Resolution } from 'src/app/models/resolution/resolution.model';
import { ResolutionService } from 'src/app/services/resolution.service';

@Component({
  selector: 'app-simulation-mobile',
  templateUrl: './simulation-mobile.component.html',
  styleUrls: ['./simulation-mobile.component.css']
})
export class SimulationMobileComponent implements OnInit {

  public resolution: Resolution;

  constructor(private resolutionService: ResolutionService) { }

  ngOnInit(): void {
    this.resolution = this.resolutionService.getTestResolution();
  }

}
