import { Component, OnInit, Input } from '@angular/core';
import { Resolution } from '../../../models/resolution/resolution.model';
import { AbstractAmendment } from '../../../models/resolution/abstract-amendment.model';
import { ResolutionService } from '../../../services/resolution.service';
import { ActivatedRoute } from '@angular/router';
import { AmendmentInspector } from '../../../models/resolution/amendment-inspector';
import { OperativeSection } from '../../../models/resolution/operative-section.model';
import { OperativeParagraph } from "../../../models/resolution/operative-paragraph.model";

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
    if (route == null)
      return;

    let id: string = null;
    this.route.params.subscribe(params => {
      id = params.id;
    })
    if (id == null) {
      id = this.route.snapshot.queryParamMap.get('id');
    }

    if (id != null) {
      this.service.getResolution(id).subscribe(n => {
        this.resolution = n;
        this.service.subscribeToResolution(this.resolution.resolutionId);
        this.service.addResolutionListener(this.resolution, new AmendmentInspector());
      }, err => {
        this.loadError = true;
      });
    }
  }

  ngOnInit() {
  }

  getDeleteAmendment(val: OperativeParagraph) {
    this.detailAmendments = this.resolution.operativeSection.deleteAmendments.filter(n => n.targetSectionId === val.operativeParagraphId);
    this.amendmentModalActive = true;
    this.amendmentDetailType = 'delete';
  }

  getChangeAmendment(val: OperativeParagraph) {
    this.detailAmendments = this.resolution.operativeSection.changeAmendments.filter(n => n.targetSectionId === val.operativeParagraphId);
    this.amendmentModalActive = true;
    this.amendmentDetailType = 'change';
  }

  closeAmendmentDetailModal() {
    this.amendmentModalActive = false;
  }

}
