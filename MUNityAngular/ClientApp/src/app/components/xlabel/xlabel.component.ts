import { Component, OnInit, Input } from '@angular/core';

@Component({
  selector: 'app-xlabel',
  templateUrl: './xlabel.component.html',
  styleUrls: ['./xlabel.component.css']
})
export class XlabelComponent implements OnInit {

  @Input() value: string;

  isEditing = false;

  constructor() { }

  ngOnInit() {
  }

  valueChanged() {
    this.isEditing = false;
    
  }

}
