import { Component, OnInit } from '@angular/core';
import { FaIconLibrary } from '@fortawesome/angular-fontawesome';
import { faFile, faFlag, faUser, faCircle, faCarrot, faCaretUp } from '@fortawesome/free-solid-svg-icons';

@Component({
  selector: 'app-manage-conference-dashboard',
  templateUrl: './manage-conference-dashboard.component.html',
  styleUrls: ['./manage-conference-dashboard.component.css']
})
export class ManageConferenceDashboardComponent implements OnInit {

  // There need to be a better way to do this, but until then this shit will do!
  public faCircle = faCircle;

  public faCaretUp = faCaretUp

  constructor() {

  }

  ngOnInit(): void {
  }

}
