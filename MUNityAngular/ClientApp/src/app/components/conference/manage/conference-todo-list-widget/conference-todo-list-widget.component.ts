import { Component, OnInit } from '@angular/core';

import { faQuestionCircle, faPlus } from '@fortawesome/free-solid-svg-icons';

@Component({
  selector: 'app-conference-todo-list-widget',
  templateUrl: './conference-todo-list-widget.component.html',
  styleUrls: ['./conference-todo-list-widget.component.css']
})
export class ConferenceTodoListWidgetComponent implements OnInit {

  public faQuestionCircle = faQuestionCircle;

  public faPlus = faPlus;

  constructor() { }

  ngOnInit(): void {
  }

}
