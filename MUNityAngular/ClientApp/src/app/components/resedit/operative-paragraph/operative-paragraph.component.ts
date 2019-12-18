import { Component, OnInit, Input, Renderer2, ElementRef, ViewChild } from '@angular/core';
import { ResolutionService } from '../../../services/resolution.service';

@Component({
  selector: 'app-operative-paragraph',
  templateUrl: './operative-paragraph.component.html',
  styleUrls: ['./operative-paragraph.component.css']
})
export class OperativeParagraphComponent implements OnInit {

  @Input() paragraph: any;

  constructor(private renderer: Renderer2, private service: ResolutionService) { }

  ngOnInit() {
  }

  delete() {
    console.log('Delete: ' + this.paragraph.ID);
  }

}
