import { Component, OnInit } from '@angular/core';
import { ResolutionService } from '../../../services/resolution.service';

@Component({
  selector: 'app-editor',
  templateUrl: './editor.component.html',
  styleUrls: ['./editor.component.css']
})
export class EditorComponent implements OnInit {

  constructor(private service: ResolutionService) { }

  public model;

  ngOnInit() {
    this.service.getResolution('test').subscribe(n => {
      this.model = n;
      console.log(this.model);
    });
    
  }

}
