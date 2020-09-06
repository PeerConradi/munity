import { Component, OnInit } from '@angular/core';

import { faEllipsisH } from '@fortawesome/free-solid-svg-icons';

@Component({
  selector: 'app-committee-list',
  templateUrl: './committee-list.component.html',
  styleUrls: ['./committee-list.component.css']
})
export class CommitteeListComponent implements OnInit {

  public faEllipsisH = faEllipsisH;

  constructor() { }

  ngOnInit(): void {
  }

}
