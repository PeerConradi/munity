import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-committee-details',
  templateUrl: './committee-details.component.html',
  styleUrls: ['./committee-details.component.css']
})
export class CommitteeDetailsComponent implements OnInit {

  colorScheme = {
    domain: ['#5AA454', '#A10A28']
  };

  view: any[] = [700, 400];

  data = [
    {
      "name": "Anwesend",
      "value": 12
    },
    {
      "name": "Abwesend",
      "value": 8
    },
  ]

  constructor() { }

  ngOnInit() {

  }

}
