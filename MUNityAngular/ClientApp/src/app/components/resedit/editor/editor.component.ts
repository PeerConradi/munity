import { Component, OnInit, Input } from '@angular/core';
import { ActivatedRoute } from "@angular/router";
import { interval, Subscription } from 'rxjs';
import { ResolutionService } from '../../../services/resolution.service';
import { Resolution } from '../../../models/resolution.model';
import { OperativeSection } from 'src/app/models/operative-section.model';
import { NotifierService } from 'angular-notifier';

@Component({
  selector: 'app-editor',
  templateUrl: './editor.component.html',
  styleUrls: ['./editor.component.css']
})
export class EditorComponent implements OnInit {

  @Input('resolution')



  public set resolution(v: Resolution) {
    if (v != null) {
      this.model = v;
      if (v.OperativeSections.length > 0)
        this.amendmentTargetSection = v.OperativeSections[0];
      console.log(this.model);
    }
  }


  public get resolution(): Resolution {
    return this.model;
  }

  public amendmentModalActive: boolean = false;

  amendmentTargetSection: OperativeSection;

  addAmendmentType: string = "delete";

  constructor(private service: ResolutionService, private route: ActivatedRoute, private notifier: NotifierService) { }

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
      console.log('Suche resolution mit der ID: ' + id);
      this.service.getResolution(id).subscribe(n => {
        let readyState = this.service.connectionReady;
        this.model = n;
        console.log(n);
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
    this.service.addPreambleParagraph(this.model.ID).subscribe(data => { console.log('Erfolg!') }, err => {
      if (err.status == 404) {
        this.notifier.notify('error', 'Ohh nein - Es besteht keine Verbindung zum Server oder die Resolution exisitert nicht.');
      }
      else {
        this.notifier.notify('error', 'Das hat aus unbekannten Gründen nicht geklappt');
      }
      console.log(err)
    });
  }

  openAddAmendmentModal() {
    this.amendmentModalActive = true;
  }

  closeAddAmendmentModal() {
    this.amendmentModalActive = false;
  }

  addAmendmentTypeSelected(newValue) {
    console.log(newValue);
    this.addAmendmentType = newValue;
  }

  addAmendmentTargetSelected(target) {
    console.log(target);
    console.log(target.ID);
    this.amendmentTargetSection = target;
  }
}
