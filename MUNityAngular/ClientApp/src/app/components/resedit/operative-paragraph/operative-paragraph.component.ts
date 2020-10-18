import { Component, OnInit, Input, Renderer2, ElementRef, ViewChild } from '@angular/core';
import { ResolutionService } from '../../../services/resolution.service';
import { Notice } from '../../../models/notice.model';
import { NoticeTag } from '../../../models/notice-tag.model';
import { UserService } from '../../../services/user.service';
import { WindowPosition } from '../../../models/window-position.model';
import { OperativeParagraph } from "../../../models/resolution/operative-paragraph.model";
import { Resolution } from 'src/app/models/resolution/resolution.model';

@Component({
  selector: 'app-operative-paragraph',
  templateUrl: './operative-paragraph.component.html',
  styleUrls: ['./operative-paragraph.component.css']
})
export class OperativeParagraphComponent implements OnInit {

  @Input() paragraph: OperativeParagraph;

  @Input() resolution: Resolution;

  @Input() index: number;

  @ViewChild('operativeTextArea') textArea: ElementRef;

  hideNotices = true;

  newComment: Notice = new Notice();

  newTag: NoticeTag = new NoticeTag();

  // N for neutral nothing chaged, S for success, E for error
  public saveState: string = 'N';

  noticeWindowLeft = 0;
  noticeWindowTop = 0;

  firstShowNotices = true;

  waitToSave = false;

  saveRequestCount = 0;

  constructor(private renderer: Renderer2, private service: ResolutionService, private userService: UserService) { }

  ngOnInit() {
    this.newTag.type = "primary";
  }

  onKey(event: any) {
    this.paragraph.text = event.target.value;

    if (!this.waitToSave) {
      this.saveChanges();
    }
  }

  get path(): string {
    return this.service.getPathOfOperativeParagraph(this.paragraph, this.resolution);
  }

  async saveChanges() {
    this.waitToSave = true;
    await this.delay(3000);

    this.service.savePublicResolution(this.resolution).subscribe(n => {
      this.saveState = 'S';
      this.waitToSave = false;
    },
      err => {
        this.saveState = 'E';
        this.saveRequestCount++;
        // try to save again
        this.saveChanges();
      });

    //Wait for a few seconds before trying to save
    // this.service.changeOperativeParagraph(this.paragraph).subscribe(n => {
    //   this.saveState = 'S';
    //   this.waitToSave = false;
    // },
    //   err => {
    //     this.saveState = 'E';
    //     this.saveRequestCount++;
    //     // try to save again
    //     this.saveChanges();
    //   });
  }

  delay(ms: number) {
    return new Promise(resolve => setTimeout(resolve, ms));
  }

  delete() {
    this.service.removeOperativeParagraph(this.resolution.resolutionId, this.paragraph.operativeParagraphId);
  }

  moveUp() {
    this.service.moveOperativeParagraphUp(this.resolution.resolutionId, this.paragraph.operativeParagraphId);
  }

  moveDown() {
    this.service.moveOperativeParagraphDown(this.resolution.resolutionId, this.paragraph.operativeParagraphId);
  }

  moveLeft() {
    this.service.moveOperativeParagraphLeft(this.resolution.resolutionId, this.paragraph.operativeParagraphId);
  }

  moveRight() {
    this.service.moveOperativeParagraphRight(this.resolution.resolutionId, this.paragraph.operativeParagraphId);
  }

  showNotices(val) {
    if (this.firstShowNotices) {
      this.noticeWindowLeft = val.x;
      this.noticeWindowTop = val.y;
      this.firstShowNotices = false;
    }
    this.hideNotices = false;
  }

  toggleNotices() {
    this.hideNotices = !this.hideNotices;
  }

  addNotice() {
    //this.paragraph.Notices.push(notice);
    this.service.changeOperativeParagraphNotice(this.paragraph, this.newComment).subscribe(n => {
      //this.paragraph.Notices.push(n);
      this.newComment.text = '';
      this.newComment.title = '';
      this.newComment.tags = [];

    });
  }

  addTag() {
    const tagClone = new NoticeTag();
    tagClone.text = this.newTag.text;
    tagClone.type = this.newTag.type;
    tagClone.id = Date.now().toString();
    this.newComment.tags.push(tagClone);
    this.newTag.text = '';

  }

  removeTag(tag: NoticeTag) {
    const index = this.newComment.tags.indexOf(tag);
    this.newComment.tags.splice(index, 1);
  }

  deleteNotice(notice: Notice) {
    const index = this.paragraph.notices.indexOf(notice);
    if (index != -1) {
      this.paragraph.notices.splice(index, 1);
    }
    this.service.changeOperativeParagraphNotices(this.paragraph).subscribe();
  }

  noticeRead(notice: Notice) {
    //if (this.userService.currentUser != null) {
    //  let name = this.userService.currentUser.Username;

    //  if (this.userService.currentUser.Forename != null && this.userService.currentUser.Lastname != null) {
    //    name = this.userService.currentUser.Forename + ' ' + this.userService.currentUser.Lastname;
    //  }

    //  if (!notice.ReadBy.includes(name)) {
    //    notice.ReadBy.push(name);
    //  }
    //}
    //this.service.changeOperativeParagraphNotice(this.paragraph, notice).subscribe();
  }

  noticeUpdated(notice: Notice) {
    this.service.changeOperativeParagraphNotice(this.paragraph, notice).subscribe();
  }

  moveStopped(val: WindowPosition) {
    this.noticeWindowLeft = val.Left;
    this.noticeWindowTop = val.Top;
  }

  textAreaBlur() {
    this.service.currentOperativeParagraph = null;
  }

  textAreaFocus() {
    this.service.currentOperativeParagraph = this.paragraph;
  }
}
