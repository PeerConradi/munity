import { Component, OnInit } from '@angular/core';
import { Resolution } from 'src/app/models/resolution.model';
import { Preamble } from 'src/app/models/preamble.model';
import { OperativeSection } from 'src/app/models/operative-section.model';
import { PreambleParagraph } from 'src/app/models/preamble-paragraph.model';

@Component({
  selector: 'app-all-components',
  templateUrl: './all-components.component.html',
  styleUrls: ['./all-components.component.css']
})
export class AllComponentsComponent implements OnInit {

  reso: Resolution;

  constructor() {
    const res = new Resolution();
    this.reso = res;
  }

  ngOnInit() {

  }

}
