import { Component, Input, OnInit } from '@angular/core';

import { faEllipsisH } from '@fortawesome/free-solid-svg-icons';
import { Committee } from 'src/app/models/conference/committee.model';

@Component({
  selector: 'app-committee-list',
  templateUrl: './committee-list.component.html',
  styleUrls: ['./committee-list.component.css']
})
export class CommitteeListComponent implements OnInit {

  @Input() committees: Committee[];

  public faEllipsisH = faEllipsisH;

  constructor() { }

  ngOnInit(): void {
  }

}
