import { Component, OnInit, Input, Renderer2, ElementRef, ViewChild } from '@angular/core';
import { ResolutionService } from '../../../services/resolution.service';

@Component({
  selector: 'app-operative-paragraph',
  templateUrl: './operative-paragraph.component.html',
  styleUrls: ['./operative-paragraph.component.css']
})
export class OperativeParagraphComponent implements OnInit {

  @Input() paragraph: any;

  @Input() resolutionid: string;

  @Input() index: number;

  constructor(private renderer: Renderer2, private service: ResolutionService) { }

  ngOnInit() {
  }

  onKey(event: any) {
    this.paragraph.Text = event.target.value;
    console.log('New Text: ' + event.target.value);
    this.service.changeOperativeParagraph(this.resolutionid, this.paragraph.ID, event.target.value);
  }

  delete() {
    console.log('Delete: ' + this.paragraph.ID);
  }

}
