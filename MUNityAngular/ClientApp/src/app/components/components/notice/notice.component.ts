import { Component, OnInit, Input } from '@angular/core';
import { Notice } from '../../../models/notice.model';

@Component({
  selector: 'app-notice',
  templateUrl: './notice.component.html',
  styleUrls: ['./notice.component.css']
})
export class NoticeComponent implements OnInit {

  @Input() notice: Notice;

  constructor() { }

  ngOnInit() {
  }

}
