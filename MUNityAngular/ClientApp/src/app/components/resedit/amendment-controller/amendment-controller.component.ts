import { Component, OnInit, Input } from '@angular/core';
import { AbstractAmendment } from '../../../models/abstract-amendment.model';
import { ResolutionService } from '../../../services/resolution.service';
import { Resolution } from '../../../models/resolution.model';
import { DeleteAmendment } from '../../../models/delete-amendment.model';
import { ChangeAmendment } from '../../../models/change-amendment.model';
import { OperativeSection } from '../../../models/operative-section.model';
import { AmendmentInspector } from '../../../models/amendment-inspector';

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
      this.resolutionService.removeAmendment(this.resolution.ID, this.amendment.ID);
    }
  }

  get paragraph(): OperativeSection {
    return AmendmentInspector.getSectionForAmendment(this.resolution, this.amendment);
  }

  activeChanged(val) {
    if (this.amendment.Activated === true) {
      this.resolutionService.deactivateAmendment(this.resolution.ID, this.amendment.ID);
    } else {
      this.resolutionService.activateAmendment(this.resolution.ID, this.amendment.ID);
    }
      
  }

  submitAmendment() {
    this.resolutionService.submitAmendment(this.resolution.ID, this.amendment.ID);
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
