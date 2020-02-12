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

  hideNotices = true;

  constructor(private renderer: Renderer2, private service: ResolutionService) { }

  ngOnInit() {
  }

  onKey(event: any) {
    this.paragraph.Text = event.target.value;
    this.service.changeOperativeParagraph(this.paragraph);
  }

  delete() {
    this.service.removeOperativeParagraph(this.resolutionid, this.paragraph.ID);
  }

  moveUp() {
    this.service.moveOperativeParagraphUp(this.resolutionid, this.paragraph.ID);
  }

  moveDown() {
    this.service.moveOperativeParagraphDown(this.resolutionid, this.paragraph.ID);
  }

  moveLeft() {
    this.service.moveOperativeParagraphLeft(this.resolutionid, this.paragraph.ID);
  }

  moveRight() {
    this.service.moveOperativeParagraphRight(this.resolutionid, this.paragraph.ID);
  }
  toggleNotices() {
    this.hideNotices = !this.hideNotices;
  }
}
