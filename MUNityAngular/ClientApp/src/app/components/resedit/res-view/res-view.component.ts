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

  @Input() resolution: Resolution;

  @Input() allAmendments: AbstractAmendment[];

  constructor(public service: ResolutionService, private route: ActivatedRoute) {

    let id: string = null;
    this.route.params.subscribe(params => {
      id = params.id;
    })
    //if (id == null) {
    //  id = this.route.snapshot.queryParamMap.get('id');
    //}

    if (id != null) {
      this.service.getResolution(id).subscribe(n => {
        if (n != null) {
          let readyState = this.service.connectionReady;
          this.resolution = n;
          this.service.subscribeToResolution(this.resolution.resolutionId);
          this.service.addResolutionListener(this.resolution, new AmendmentInspector());
        }
      });
    }
  }

  get presets(): string {
    return "<style>ul { list-style-type: lower-alpha; margin-left: 50Px; }ul ul { list-style-type: lower-roman; } </style>";
  }

  ngOnInit() {
  }

  activeAmendment(section: OperativeParagraph): AbstractAmendment {
    let amendment = this.resolution.operativeSection.deleteAmendments.find(n => n.targetSectionId === section.operativeParagraphId && n.activated);
    if (amendment == null) {
      amendment = this.resolution.operativeSection.changeAmendments.find(n => n.targetSectionId === section.operativeParagraphId && n.activated);
    }
    if (amendment == null) {
      amendment = this.resolution.operativeSection.moveAmendments.find(n => n.targetSectionId === section.operativeParagraphId && n.activated);
    }

    return amendment;
  }

  activeChangeAmendment(section: OperativeSection): ChangeAmendment {
    return this.resolution.operativeSection.changeAmendments.find(n => n.targetSectionId === section.operativeSectionId && n.activated);
  }
}
