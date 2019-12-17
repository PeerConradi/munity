import { Component, OnInit, Input } from '@angular/core';

@Component({
  selector: 'app-preamble-paragraph',
  templateUrl: './preamble-paragraph.component.html',
  styleUrls: ['./preamble-paragraph.component.css']
})
export class PreambleParagraphComponent implements OnInit {

  @Input() paragraph: any;

  @Input() index: number;

  constructor() { }


  ngOnInit() {
  }

}
