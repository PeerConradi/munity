import { Component, OnInit, Input } from '@angular/core';
import { Resolution } from '../../../models/resolution.model';
import { AbstractAmendment } from '../../../models/abstract-amendment.model';
import { ResolutionService } from '../../../services/resolution.service';
import { ActivatedRoute } from '@angular/router';
import { AmendmentInspector } from '../../../models/amendment-inspector';
import { OperativeSection } from '../../../models/operative-section.model';

@Component({
  selector: 'app-work-with-resolution',
  templateUrl: './work-with-resolution.component.html',
  styleUrls: ['./work-with-resolution.component.css']
})
export class WorkWithResolutionComponent implements OnInit {

  @Input() resolution: Resolution;

  @Input() allAmendments: AbstractAmendment[];

  amendmentModalActive: boolean = false;

  loadError = false;

  amendmentDetailType: string = 'delete';

  detailAmendments: AbstractAmendment[];

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
        this.resolution = n;
        this.service.subscribeToResolution(this.resolution.ID);
        this.service.addResolutionListener(this.resolution, new AmendmentInspector());
      }, err => {
          this.loadError = true;
      });
    }
  }

  ngOnInit() {
  }

  getDeleteAmendment(val: OperativeSection) {
    this.detailAmendments = this.resolution.DeleteAmendments.filter(n => n.TargetSectionID == val.ID);
    this.amendmentModalActive = true;
    this.amendmentDetailType = 'delete';
  }

  getChangeAmendment(val: OperativeSection) {
    this.detailAmendments = this.resolution.ChangeAmendments.filter(n => n.TargetSectionID == val.ID);
    this.amendmentModalActive = true;
    this.amendmentDetailType = 'change';
  }

  closeAmendmentDetailModal() {
    this.amendmentModalActive = false;
  }

}
