import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from "@angular/router";
import { interval, Subscription } from 'rxjs';
import { ResolutionService } from '../../../services/resolution.service';
import { Resolution } from '../../../models/resolution.model';

@Component({
  selector: 'app-editor',
  templateUrl: './editor.component.html',
  styleUrls: ['./editor.component.css']
})
export class EditorComponent implements OnInit {

  constructor(private service: ResolutionService, private route: ActivatedRoute) { }

  public model: Resolution;

  saveSubscription: Subscription;

  ngOnInit() {
    let id: string = null;
    this.route.params.subscribe(params => {
      id = params.id;
    })
    if (id == null)
      id = this.route.snapshot.queryParamMap.get("id");

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
