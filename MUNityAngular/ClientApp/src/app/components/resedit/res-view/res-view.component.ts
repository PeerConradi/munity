import { Component, OnInit, Input } from '@angular/core';
import { ResolutionService } from '../../../services/resolution.service';
import { Resolution } from 'src/app/models/resolution.model';
import { ActivatedRoute } from '@angular/router';
import { AbstractAmendment } from '../../../models/abstract-amendment.model';
import { AmendmentInspector } from '../../../models/amendment-inspector';

@Component({
  selector: 'app-res-view',
  templateUrl: './res-view.component.html',
  styleUrls: ['./res-view.component.css']
})
export class ResViewComponent implements OnInit {

  @Input() resolution: Resolution;

  @Input() allAmendments: AbstractAmendment[];

  constructor(public service: ResolutionService, private route: ActivatedRoute) {
    let id: string = null;
    this.route.params.subscribe(params => {
      id = params.id;
    })
    if (id == null) {
      id = this.route.snapshot.queryParamMap.get('id');
    }

    if (id != null) {
      this.service.getResolution(id).subscribe(n => {
        console.log('Search resolution: ' + id);
        let readyState = this.service.connectionReady;
        this.resolution = n;
        this.service.subscribeToResolution(this.resolution.ID);
        this.service.addResolutionListener(this.resolution, new AmendmentInspector());
      });
    }}

  ngOnInit() {
  }

}
