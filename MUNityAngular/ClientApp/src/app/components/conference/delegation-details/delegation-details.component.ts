/**
 * This components has a conference context. Different conferences
 * share the same Delegation, but every delegation is a new group in every
 * conference so this component doesn't use the delegationId but the
 * delegation-conference-link.
 *
 * This link functions as a sort of groupId that can be used to get posts, members
 * and other information about a delegation
 **/

import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-delegation-details',
  templateUrl: './delegation-details.component.html',
  styleUrls: ['./delegation-details.component.css']
})
export class DelegationDetailsComponent implements OnInit {

  constructor() { }

  ngOnInit() {
  }

}
