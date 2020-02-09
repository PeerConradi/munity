import { Component, OnInit, Input } from '@angular/core';
import { ResolutionService } from '../../../services/resolution.service';
import { PreambleParagraph } from '../../../models/preamble-paragraph.model';

@Component({
  selector: 'app-preamble-paragraph',
  templateUrl: './preamble-paragraph.component.html',
  styleUrls: ['./preamble-paragraph.component.css']
})
export class PreambleParagraphComponent implements OnInit {

  @Input() paragraph: PreambleParagraph;

  @Input() resolutionid: string;

  @Input() index: number;

  constructor(private service: ResolutionService) { }

  onKey(event: any) {
    if (this.paragraph.ResolutionID == null) {
      this.paragraph.ResolutionID = this.resolutionid;
    }
    this.paragraph.Text = event.target.value;
    this.service.changePreambleParagraph(this.paragraph);
  }

  ngOnInit() {
  }

  delete() {
    this.service.removePrembleParagraph(this.resolutionid, this.paragraph.ID);
  }

  moveUp() {
    this.service.movePrembleParagraphUp(this.resolutionid, this.paragraph.ID);
  }

  moveDown() {
    this.service.movePrembleParagraphDown(this.resolutionid, this.paragraph.ID);
  }
}
