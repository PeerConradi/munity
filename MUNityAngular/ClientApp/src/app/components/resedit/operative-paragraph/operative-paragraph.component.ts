import { Component, OnInit, Input, Renderer2, ElementRef, ViewChild } from '@angular/core';
import { ResolutionService } from '../../../services/resolution.service';
import { Notice } from '../../../models/notice.model';
import { OperativeSection } from '../../../models/operative-section.model';

@Component({
  selector: 'app-operative-paragraph',
  templateUrl: './operative-paragraph.component.html',
  styleUrls: ['./operative-paragraph.component.css']
})
export class OperativeParagraphComponent implements OnInit {

  @Input() paragraph: OperativeSection;

  @Input() resolutionid: string;

  @Input() index: number;

  @ViewChild('operativeTextArea', null) textArea: ElementRef;

  hideNotices = true;

  newNoticeText: string;

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

  addNotice() {
    console.log(this.newNoticeText);
    const notice = new Notice();
    notice.Id = new Date().getTime().toString();
    notice.Title = 'Titel';
    notice.Text = this.newNoticeText;
    this.paragraph.Notices.push(notice);
    this.service.changeOperativeParagraphNotices(this.paragraph, notice);
  }
}
