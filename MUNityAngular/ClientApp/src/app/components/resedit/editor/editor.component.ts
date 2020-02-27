import { Component, OnInit, Input, TemplateRef } from '@angular/core';
import { ActivatedRoute } from "@angular/router";
import { interval, Subscription, of } from 'rxjs';
import { ResolutionService } from '../../../services/resolution.service';
import { Resolution } from '../../../models/resolution.model';
import { OperativeSection } from 'src/app/models/operative-section.model';
import { NotifierService } from 'angular-notifier';
import { AbstractAmendment } from '../../../models/abstract-amendment.model';
import { AmendmentInspector } from '../../../models/amendment-inspector';
import { Title } from '@angular/platform-browser';
import { ConferenceServiceService } from '../../../services/conference-service.service';
import { Delegation } from '../../../models/delegation.model';
import { AddAmendment } from '../../../models/add-amendment.model';
import { ChangeResolutionHeaderRequest } from '../../../models/requests/change-resolution-header-request';
import { Conference } from '../../../models/conference.model';
import { Committee } from '../../../models/committee.model';
import { BsModalService, BsModalRef } from 'ngx-bootstrap';

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
      if (v.OperativeSections.length > 0) {
        this.amendmentTargetSection = v.OperativeSections[0];
      }
    }
  }


  public amendmentInspector: AmendmentInspector = new AmendmentInspector();

  
  public get resolution(): Resolution {
    return this.model;
  }

  public amendmentModalActive = false;

  amendmentTargetSection: OperativeSection;

  newAmendmentNewText: string;

  newamendmentDelegation: string = '';

  addAmendmentType = 'delete';

  amendmentTargetPosition: number;

  isLoading: boolean = true;

  canEdit: boolean = null;

  notFound = false;

  allDelegations: string[] = [];

  userConferences: Conference[] = [];

  selectedConference: Conference;

  selectedCommittee: Committee;


  constructor(private service: ResolutionService, private route: ActivatedRoute, private notifier: NotifierService,
    private titleService: Title, private conferenceService: ConferenceServiceService) {
    this.titleService.setTitle('ResaOnline')
  }

  public model: Resolution;

  saveSubscription: Subscription;

  ngOnInit() {
    let id: string = null;
    this.route.params.subscribe(params => {
      id = params.id;
    })
    if (id == null) {
      id = this.route.snapshot.queryParamMap.get('id');
    }

    if (id != null) {
      //Überprüfen ob überhaupt bearbeitet werden darf
      this.service.canIEditResolution(id).subscribe(canEditResolution => {
        this.isLoading = false;
        this.canEdit = canEditResolution;

        if (canEditResolution) {
          this.service.getResolution(id).subscribe(n => {
            let readyState = this.service.connectionReady;
            //this.service.OrderAmendments(n);
            this.model = n;
            this.service.subscribeToResolution(this.model.ID);
            this.service.addResolutionListener(this.model, this.amendmentInspector);
            this.amendmentInspector.allAmendments = this.service.OrderAmendments(this.model);

            if (n.OperativeSections.length > 0) {
              this.amendmentTargetSection = n.OperativeSections[0];
              this.newAmendmentNewText = n.OperativeSections[0].Text;
            }

            this.titleService.setTitle(this.model.Topic);
          }, err => {
              //TODO: 404 check or differet error
              this.notFound = true;
          });
                  }
      }, err => {
        //TODO: 404 check or differet error
        this.notFound = true;
      });
    }

    this.conferenceService.getAllDelegations().subscribe(n => {
      n.forEach(d => {
        this.allDelegations.push(d.Name);
      });
    });

    this.conferenceService.getAllConferences().subscribe(n => {
      this.userConferences = n;
    });
  }

  pushResolution() {
    this.service.pushResolution(this.resolution).subscribe(n => { });
  }

  addPreambleParagraph() {
    this.service.addPreambleParagraph(this.model.ID).subscribe(data => { }, err => {
      if (err.status == 404) {
        this.notifier.notify('error', 'Ohh nein - Es besteht keine Verbindung zum Server oder die Resolution exisitert nicht.');
      }
      else {
        this.notifier.notify('error', 'Das hat aus unbekannten Gründen nicht geklappt');
      }
    });
  }

  openAddAmendmentModal() {
    this.amendmentModalActive = true;
  }

  closeAddAmendmentModal() {
    this.amendmentModalActive = false;
  }

  addAmendmentTypeSelected(newValue) {
    this.addAmendmentType = newValue;
  }

  addAmendmentTargetSelected(target) {
    this.newAmendmentNewText = this.amendmentTargetSection.Text;
  }

  addAmendmentTargetPositionSelected(target) {
    //Nothing to do: feeling cute may delete late
  }

  addOperativeParagraph() {
    this.service.addOperativeParagraph(this.model.ID).subscribe(data => { }, err => {
      if (err.status == 404) {
        this.notifier.notify('error', 'Ohh nein - Es besteht keine Verbindung zum Server oder die Resolution exisitert nicht.');
      }
      else {
        this.notifier.notify('error', 'Das hat aus unbekannten Gründen nicht geklappt');
      }
    });
  }

  addDeleteAmendment() {
    const resolutionid = this.model.ID;
    const sectionid = this.model.OperativeSections[0].ID;
    this.service.addDeleteAmendment(resolutionid, sectionid, 'Unknown');
  }

  get endIndex(): number {
    return this.model.OperativeSections.length + 1;
  }

  createNewAmendment() {
    const type: string = this.addAmendmentType;
    const target: OperativeSection = this.amendmentTargetSection;
    const newText: string = this.newAmendmentNewText;

    if (type === 'delete') {
      this.service.addDeleteAmendment(this.model.ID, target.ID, this.newamendmentDelegation).subscribe();
    } else if (type === 'change') {
      this.service.addChangeAmendment(this.model.ID, target.ID, this.newamendmentDelegation, newText).subscribe();
    } else if (type === 'move') {
      this.service.addMoveAmendment(this.model.ID, target.ID, this.newamendmentDelegation, this.amendmentTargetPosition).subscribe();
    } else if (type === 'add') {
      const amendment = new AddAmendment();
      amendment.TargetResolutionID = this.resolution.ID;
      amendment.SubmitterName = this.newamendmentDelegation;
      amendment.NewText = newText;
      amendment.TargetPosition = this.amendmentTargetPosition;
      this.service.addAddAmendment(amendment).subscribe();
    }

    this.amendmentModalActive = false;
  }

  updateTitle() {
    const request = this.baseRequest();
    if (request.title != null && request.title != '') {
      this.service.updateHeader(request);
    }
  }

  baseRequest(): ChangeResolutionHeaderRequest {
    const r = new ChangeResolutionHeaderRequest();
    r.committee = this.resolution.CommitteeName;
    r.resolutionId = this.resolution.ID;
    r.supporters = this.resolution.SupporterNames;
    r.title = this.resolution.Topic;
    r.submitterName = this.resolution.SubmitterName;
    return r;
  }

  conferenceSelected() {
    //console.log(this.selectedConference);
  }

  createConferenceConnection(committee: Committee) {
    this.service.linkResolutionToCommittee(this.resolution.ID, committee.ID).subscribe();
  }
}
