import { Component, OnInit, Input } from '@angular/core';
import { ResolutionService } from '../../../services/resolution.service';
import { PreambleParagraph } from '../../../models/resolution/preamble-paragraph.model';
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

  saveState = 'N';

  constructor(private service: ResolutionService, private userService: UserService) { }

  onKey(event: any) {
    this.paragraph.text = event.target.value;
    this.service.changePreambleParagraph(this.paragraph).subscribe(n => {
      this.saveState = 'S';
    }, err => {
      this.saveState = 'E';
    });
  }

  ngOnInit() {
  }

  delete() {
    this.service.removePrembleParagraph(this.resolutionid, this.paragraph.preambleParagraphId);
  }

  moveUp() {
    this.service.movePrembleParagraphUp(this.resolutionid, this.paragraph.preambleParagraphId);
  }

  moveDown() {
    this.service.movePrembleParagraphDown(this.resolutionid, this.paragraph.preambleParagraphId);
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
