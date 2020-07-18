import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { Notice } from '../../../models/notice.model';
import { UserService } from '../../../services/user.service';

@Component({
  selector: 'app-notice',
  templateUrl: './notice.component.html',
  styleUrls: ['./notice.component.css']
})
export class NoticeComponent implements OnInit {

  @Input() notice: Notice;

  @Output()
  onDelete: EventEmitter<any> = new EventEmitter<any>();

  @Output()
  onRead: EventEmitter<any> = new EventEmitter<any>();

  @Output()
  onUpdate: EventEmitter<any> = new EventEmitter<any>();

  constructor() { }

  ngOnInit() {

  }

  deleteMe() {
    this.onDelete.emit(this.notice);
  }

  markRead() {
    this.onRead.emit(this.notice);
  }

  update() {
    this.onUpdate.emit(this.notice);
  }

  get readbyNames(): string {
    let text = '';
    this.notice.readBy.forEach(n => text += n + ', ');
    text = text.substr(0, text.length - 2);
    return text;
  }
}
