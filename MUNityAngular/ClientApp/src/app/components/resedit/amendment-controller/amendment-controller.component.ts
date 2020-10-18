import { Component, OnInit, Input } from '@angular/core';
import { AbstractAmendment } from '../../../models/resolution/abstract-amendment.model';
import { ResolutionService } from '../../../services/resolution.service';
import { Resolution } from '../../../models/resolution/resolution.model';
import { DeleteAmendment } from '../../../models/resolution/delete-amendment.model';
import { ChangeAmendment } from '../../../models/resolution/change-amendment.model';
import { OperativeSection } from '../../../models/resolution/operative-section.model';
import { AmendmentInspector } from '../../../models/resolution/amendment-inspector';
import { OperativeParagraph } from "../../../models/resolution/operative-paragraph.model";

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
      this.resolutionService.removeAmendment(this.resolution.resolutionId, this.amendment.id);
    }
  }

  denyAmendment() {
    if (this.amendment != null && this.resolution != null) {
      this.resolutionService.denyAmendment(this.resolution.resolutionId, this.amendment.id).subscribe();
    }
  }

  get paragraph(): OperativeParagraph {
    return this.resolution.operativeSection.paragraphs.find(n => n.operativeParagraphId == this.amendment.targetSectionId);
  }

  activeChanged(val) {
    //this.amendment.activated = val;
    this.resolutionService.savePublicResolution(this.resolution).subscribe();
    if (this.amendment.activated) {
      //this.resolutionService.savePublicResolution(this.resolution);
      //this.resolutionService.deactivateAmendment(this.resolution.resolutionId, this.amendment.id);
    } else {
      //this.resolutionService.activateAmendment(this.resolution.resolutionId, this.amendment.id);
    }

  }

  submitAmendment() {
    this.resolutionService.submitAmendment(this.resolution.resolutionId, this.amendment.id);
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
