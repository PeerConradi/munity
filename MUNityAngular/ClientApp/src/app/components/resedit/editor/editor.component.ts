import { Component, OnInit } from '@angular/core';
import  * as resService from '../../../services/resolution.service';
import { ActivatedRoute } from "@angular/router";
import { interval, Subscription } from 'rxjs';

@Component({
  selector: 'app-editor',
  templateUrl: './editor.component.html',
  styleUrls: ['./editor.component.css']
})
export class EditorComponent implements OnInit {

  constructor(private service: resService.ResolutionService, private route: ActivatedRoute) { }

  public model: resService.Resolution;

  saveSubscription: Subscription;

  ngOnInit() {
    let id = this.route.snapshot.queryParamMap.get("id");
    if (id != null) {
      this.service.getResolution(id).subscribe(n => {
        let readyState = this.service.connectionReady;
        this.model = n;
        this.service.subscribeToResolution(this.model.ID);
        this.service.addResolutionListener(this.model);
        console.log(this.model);

        //const source = interval(2000);
        //this.saveSubscription = source.subscribe(val => console.log('Try save'));
      });
    }
  }

  addPreambleParagraph() {
    console.log('Request addPreambleParagraph at: ' + this.model.ID);
    this.service.addPreambleParagraph(this.model.ID);
  }

}
