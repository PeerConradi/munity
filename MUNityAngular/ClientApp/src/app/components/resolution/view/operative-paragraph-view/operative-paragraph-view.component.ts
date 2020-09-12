import { Component, Input, OnInit } from '@angular/core';
import { AbstractAmendment } from 'src/app/models/resolution/abstract-amendment.model';
import { ChangeAmendment } from 'src/app/models/resolution/change-amendment.model';
import { OperativeParagraph } from 'src/app/models/resolution/operative-paragraph.model';
import { OperativeSection } from 'src/app/models/resolution/operative-section.model';
import { Resolution } from 'src/app/models/resolution/resolution.model';
import { ResolutionService } from 'src/app/services/resolution.service';

@Component({
  selector: 'app-operative-paragraph-view',
  templateUrl: './operative-paragraph-view.component.html',
  styleUrls: ['./operative-paragraph-view.component.css']
})
export class OperativeParagraphViewComponent implements OnInit {

  @Input() paragraph: OperativeParagraph;

  @Input() resolution: Resolution;

  get path(): string {
    return this.resolutionService.getPathOfOperativeParagraph(this.paragraph, this.resolution);
  }

  searchOperativeParagraph(id: string, paragraph: OperativeParagraph, pathArray: OperativeParagraph[]): OperativeParagraph {
    let result = paragraph.children.find(n => n.operativeParagraphId === id);
    if (result) return paragraph;
    paragraph.children.forEach(n => this.searchOperativeParagraph(id, n, pathArray));
  }

  constructor(private resolutionService: ResolutionService) { }

  ngOnInit(): void {
  }

  get activeAmendment(): AbstractAmendment {
    let amendment = this.resolution.operativeSection.deleteAmendments.find(n => n.targetSectionId === this.paragraph.operativeParagraphId && n.activated);
    if (amendment == null) {
      amendment = this.resolution.operativeSection.changeAmendments.find(n => n.targetSectionId === this.paragraph.operativeParagraphId && n.activated);
    }
    if (amendment == null) {
      amendment = this.resolution.operativeSection.moveAmendments.find(n => n.targetSectionId === this.paragraph.operativeParagraphId && n.activated);
    }

    return amendment;
  }

  get activeChangeAmendment(): ChangeAmendment {
    return this.resolution.operativeSection.changeAmendments.find(n => n.targetSectionId === this.paragraph.operativeParagraphId && n.activated);
  }

  get presets(): string {
    return "<style>ul { list-style-type: lower-alpha; margin-left: 50Px; }ul ul { list-style-type: lower-roman; } </style>";
  }

}
