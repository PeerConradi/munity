import { Component, OnInit, Input } from '@angular/core';
import { ResolutionService } from '../../../services/resolution.service';

@Component({
  selector: 'app-preamble-paragraph',
  templateUrl: './preamble-paragraph.component.html',
  styleUrls: ['./preamble-paragraph.component.css']
})
export class PreambleParagraphComponent implements OnInit {

  @Input() paragraph: any;

  @Input() resolutionid: string;

  @Input() index: number;

  constructor(private service: ResolutionService) { }

  onKey(event: any) {
    this.paragraph.Text = event.target.value;
    this.service.changePreambleParagraph(this.resolutionid, this.paragraph.ID, event.target.value);
  }

  ngOnInit() {
  }

  delete() {
    console.log('Delete: ' + this.paragraph.ID);
  }
}
