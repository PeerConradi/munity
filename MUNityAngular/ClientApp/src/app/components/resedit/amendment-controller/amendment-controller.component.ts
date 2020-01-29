import { Component, OnInit, Input } from '@angular/core';
import { AbstractAmendment } from '../../../models/abstract-amendment.model';
import { ResolutionService } from '../../../services/resolution.service';
import { Resolution } from '../../../models/resolution.model';

@Component({
  selector: 'app-amendment-controller',
  templateUrl: './amendment-controller.component.html',
  styleUrls: ['./amendment-controller.component.css']
})
export class AmendmentControllerComponent implements OnInit {

  @Input() resolution: Resolution;

  @Input() amendment: AbstractAmendment;

  constructor(private resolutionService: ResolutionService) { }

  ngOnInit() {
  }

  removeAmendment() {
    if (this.amendment != null && this.resolution != null) {
      this.resolutionService.removeAmendment(this.resolution.ID, this.amendment.ID);
    }
  }

}
