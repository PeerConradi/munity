import { Component, OnInit, Input } from '@angular/core';
import { ResolutionService } from '../../../services/resolution.service';
import { Resolution } from 'src/app/models/resolution/resolution.model';
import { ActivatedRoute } from '@angular/router';
import { AbstractAmendment } from '../../../models/resolution/abstract-amendment.model';
import { AmendmentInspector } from '../../../models/resolution/amendment-inspector';
import { OperativeSection } from '../../../models/resolution/operative-section.model';
import { ChangeAmendment } from '../../../models/resolution/change-amendment.model';
import { OperativeParagraph } from "../../../models/resolution/operative-paragraph.model";

@Component({
  selector: 'app-res-view',
  templateUrl: './res-view.component.html',
  styleUrls: ['./res-view.component.css']
})
export class ResViewComponent implements OnInit {

  @Input() public resolution: Resolution = null;

  @Input() allAmendments: AbstractAmendment[];

  private resolutionRouteId: string;

  constructor(public service: ResolutionService, private route: ActivatedRoute) {
    let id: string = null;
    this.route.params.subscribe(params => {
      this.resolutionRouteId = params.id;
    })
  }

  get supporters(): string {
    if (this.resolution?.header?.supporters == null) return '';
    let rtrn: string = '';
    this.resolution.header.supporters.forEach(n => {
      rtrn += n + ', ';
    });
    if (rtrn.length > 2) rtrn = rtrn.substr(0, rtrn.length - 2);
    return rtrn;
  }

  get presets(): string {
    return "<style>ul { list-style-type: lower-alpha; margin-left: 50Px; }ul ul { list-style-type: lower-roman; } </style>";
  }

  ngOnInit() {
    if (this.resolution == null) {

      // loading a resolution from a given id is not needed if the resolution is bounded

      console.log('Lade Resolution: ' + this.resolutionRouteId);
      //if (id == null) {
      //  id = this.route.snapshot.queryParamMap.get('id');
      //}

      if (this.resolutionRouteId != null) {
        this.service.getResolution(this.resolutionRouteId).subscribe(n => {
          if (n != null) {
            let readyState = this.service.connectionReady;
            this.resolution = n;
            this.service.subscribeToResolution(this.resolution.resolutionId);
            this.service.addResolutionListener(this.resolution, new AmendmentInspector());
          }
        });
      }
    }
    else {
      console.log('Resolution ist schon gesetzt!')
      console.log(this.resolution);
    }

  }


}
