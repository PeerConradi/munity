import { Component, OnInit, Input, Renderer2, ElementRef, ViewChild } from '@angular/core';
import { ResolutionService } from '../../../services/resolution.service';
import { Notice } from '../../../models/notice.model';
import { OperativeSection } from '../../../models/operative-section.model';
import { NoticeTag } from '../../../models/notice-tag.model';
import { UserService } from '../../../services/user.service';
import { WindowPosition } from '../../../models/window-position.model';

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

  newComment: Notice = new Notice();

  newTag: NoticeTag = new NoticeTag();

  noticeWindowLeft = 0;
  noticeWindowTop = 0;

  firstShowNitices = true;

  constructor(private renderer: Renderer2, private service: ResolutionService, private userService: UserService) { }

  ngOnInit() {
    this.newTag.Type = "primary";
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

  showNotices(val) {
    if (this.firstShowNitices) {
      this.noticeWindowLeft = val.x;
      this.noticeWindowTop = val.y;
      this.firstShowNitices = false;
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
      this.newComment.Text = '';
      this.newComment.Title = '';
      this.newComment.Tags = [];

    });
  }

  addTag() {
    const tagClone = new NoticeTag();
    tagClone.Text = this.newTag.Text;
    tagClone.Type = this.newTag.Type;
    tagClone.Id = Date.now().toString();
    this.newComment.Tags.push(tagClone);
    this.newTag.Text = '';

  }

  removeTag(tag: NoticeTag) {
    const index = this.newComment.Tags.indexOf(tag);
    this.newComment.Tags.splice(index, 1);
  }

  deleteNotice(notice: Notice) {
    const index = this.paragraph.Notices.indexOf(notice);
    if (index != -1) {
      this.paragraph.Notices.splice(index, 1);
    }
    this.service.changeOperativeParagraphNotices(this.paragraph).subscribe();
  }

  noticeRead(notice: Notice) {
    if (this.userService.currentUser != null) {
      let name = this.userService.currentUser.Username;

      if (this.userService.currentUser.Forename != null && this.userService.currentUser.Lastname != null) {
        name = this.userService.currentUser.Forename + ' ' + this.userService.currentUser.Lastname;
      }

      if (!notice.ReadBy.includes(name)) {
        notice.ReadBy.push(name);
      }
    }
    this.service.changeOperativeParagraphNotice(this.paragraph, notice).subscribe();
  }

  noticeUpdated(notice: Notice) {
    this.service.changeOperativeParagraphNotice(this.paragraph, notice).subscribe();
  }

  moveStopped(val: WindowPosition) {
    this.noticeWindowLeft = val.Left;
    this.noticeWindowTop = val.Top;
  }
}
