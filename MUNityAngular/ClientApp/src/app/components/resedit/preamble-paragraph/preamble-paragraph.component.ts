import { Component, OnInit, Input } from '@angular/core';
import { ResolutionService } from '../../../services/resolution.service';
import { PreambleParagraph } from '../../../models/preamble-paragraph.model';
import { Notice } from '../../../models/notice.model';
import { NoticeTag } from '../../../models/notice-tag.model';
import { WindowPosition } from '../../../models/window-position.model';
import { UserService } from '../../../services/user.service';

@Component({
  selector: 'app-preamble-paragraph',
  templateUrl: './preamble-paragraph.component.html',
  styleUrls: ['./preamble-paragraph.component.css']
})
export class PreambleParagraphComponent implements OnInit {

  @Input() paragraph: PreambleParagraph;

  @Input() resolutionid: string;

  @Input() index: number;

  hideNotices = true;

  newComment: Notice = new Notice();

  newTag: NoticeTag = new NoticeTag();

  noticeWindowLeft = 0;
  noticeWindowTop = 0;

  firstShowNotices = true;

  constructor(private service: ResolutionService, private userService: UserService) { }

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
    this.service.changePreambleParagraphNotice(this.paragraph, this.newComment).subscribe(n => {
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
    this.service.changePreambleParagraphNotices(this.paragraph).subscribe();
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
    //this.service.changePreambleParagraphNotice(this.paragraph, notice).subscribe();
  }

  noticeUpdated(notice: Notice) {
    this.service.changePreambleParagraphNotice(this.paragraph, notice).subscribe();
  }

  moveStopped(val: WindowPosition) {
    this.noticeWindowLeft = val.Left;
    this.noticeWindowTop = val.Top;
  }
}
