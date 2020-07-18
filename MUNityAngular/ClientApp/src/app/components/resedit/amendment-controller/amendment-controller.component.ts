import { Component, OnInit, Input } from '@angular/core';
import { AbstractAmendment } from '../../../models/abstract-amendment.model';
import { ResolutionService } from '../../../services/resolution.service';
import { Resolution } from '../../../models/resolution.model';
import { DeleteAmendment } from '../../../models/delete-amendment.model';
import { ChangeAmendment } from '../../../models/change-amendment.model';
import { OperativeSection } from '../../../models/operative-section.model';
import { AmendmentInspector } from '../../../models/amendment-inspector';
import { OperativeParagraph } from "../../../models/operative-paragraph.model";

@Component({
  selector: 'app-amendment-controller',
  templateUrl: './amendment-controller.component.html',
  styleUrls: ['./amendment-controller.component.css']
})
export class AmendmentControllerComponent implements OnInit {

  @Input() resolution: Resolution;

  @Input() amendment: AbstractAmendment;

  activeState: boolean = false;

  constructor(private resolutionService: ResolutionService) { }

  ngOnInit() {
    
  }

  removeAmendment() {
    if (this.amendment != null && this.resolution != null) {
      this.resolutionService.removeAmendment(this.resolution.resolutionId, this.amendment.ID);
    }
  }

  denyAmendment() {
    if (this.amendment != null && this.resolution != null) {
      this.resolutionService.denyAmendment(this.resolution.resolutionId, this.amendment.ID).subscribe();
    }
  }

  get paragraph(): OperativeParagraph {
    return AmendmentInspector.getSectionForAmendment(this.resolution, this.amendment);
  }

  activeChanged(val) {
    if (this.amendment.activated) {
      this.resolutionService.deactivateAmendment(this.resolution.resolutionId, this.amendment.ID);
    } else {
      this.resolutionService.activateAmendment(this.resolution.resolutionId, this.amendment.ID);
    }
      
  }

  submitAmendment() {
    this.resolutionService.submitAmendment(this.resolution.resolutionId, this.amendment.ID);
  }

  isDeleteAmendment(val): boolean {
    return val instanceof DeleteAmendment;
  }

  isAbstractAmendment(val): boolean {
    return val instanceof AbstractAmendment;
  }


  isChangeAmendment(val): boolean {
    return val instanceof ChangeAmendment;
  }

}
