import { Component, OnInit, Input, TemplateRef } from '@angular/core';
import { ActivatedRoute } from "@angular/router";
import { interval, Subscription, of } from 'rxjs';
import { ResolutionService } from '../../../services/resolution.service';
import { Resolution } from '../../../models/resolution/resolution.model';
import { OperativeSection } from 'src/app/models/resolution/operative-section.model';
import { NotifierService } from 'angular-notifier';
import { AbstractAmendment } from '../../../models/resolution/abstract-amendment.model';
import { AmendmentInspector } from '../../../models/resolution/amendment-inspector';
import { Title } from '@angular/platform-browser';
import { ConferenceService } from '../../../services/conference-service.service';
import { Delegation } from '../../../models/conference/delegation.model';
import { AddAmendment } from '../../../models/resolution/add-amendment.model';
import { ChangeResolutionHeaderRequest } from '../../../models/requests/change-resolution-header-request';
import { Conference } from '../../../models/conference/conference.model';
import { Committee } from '../../../models/conference/committee.model';
import { BsModalService, BsModalRef } from 'ngx-bootstrap';
import { OperativeParagraph } from "../../../models/resolution/operative-paragraph.model";
import { UserService } from "../../../services/user.service";
import { PreambleParagraph } from "../../../models/resolution/preamble-paragraph.model";
import { ResolutionHeader } from 'src/app/models/resolution/resolution-header.model';
import { Preamble } from 'src/app/models/resolution/preamble.model';
import { timeStamp } from 'console';

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
    }
  }


  public amendmentInspector: AmendmentInspector = new AmendmentInspector();


  public get resolution(): Resolution {
    return this.model;
  }

  public amendmentModalActive = false;

  amendmentTargetSection: OperativeParagraph;

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

  isPublic: boolean = false;


  constructor(private service: ResolutionService, private route: ActivatedRoute, private notifier: NotifierService,
    private titleService: Title, private conferenceService: ConferenceService, private userService: UserService) {
    if (this.titleService != null) {
      this.titleService.setTitle('ResaOnline');
    }

  }

  public model: Resolution;

  saveSubscription: Subscription;

  ngOnInit() {
    let id: string = null;


    this.route.params.subscribe(params => {
      id = params.id;
    });
    if (id == null) {
      id = this.route.snapshot.queryParamMap.get('id');
    }

    if (id != null) {
      // in testmode just create a new document
      if (id === 'test') {
        this.isLoading = false;
        this.resolution = new Resolution();
        this.resolution.header = new ResolutionHeader();
        this.resolution.header.name = 'Test Resolution';
        this.resolution.header.topic = 'Titel';
        this.resolution.preamble = new Preamble();
        this.resolution.operativeSection = new OperativeSection();
        return;
      }

      // Get the public version of this document. If this returns forbidden you are not allowed
      // to edit this resolution
      if (!this.userService.session) {
        this.service.getPublicResolution(id).subscribe(n => {
          console.log(n);
          this.isPublic = true;
          this.model = n;
          this.isLoading = false;
          console.log(this.model);
          //this.service.subscribeToResolution(this.model.resolutionId);
          //this.service.addResolutionListener(this.model, this.amendmentInspector);
          //this.amendmentInspector.allAmendments = this.service.OrderAmendments(this.model);
        });

      }

    }

    this.conferenceService.getAllDelegations().subscribe(n => {
      n.forEach(d => {
        this.allDelegations.push(d.name);
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
    const paragraph = new PreambleParagraph();
    this.model.preamble.paragraphs.push(paragraph);

    if (this.isPublic) {
      this.service.savePublicResolution(this.model).subscribe();
    }
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
    this.newAmendmentNewText = this.amendmentTargetSection.operativeParagraphId;
  }

  addAmendmentTargetPositionSelected(target) {
    //Nothing to do: feeling cute may delete late
  }

  addOperativeParagraph() {
    const paragraph = new OperativeParagraph();
    paragraph.operativeParagraphId = this.service.generateId();
    this.model.operativeSection.paragraphs.push(paragraph);

    if (this.isPublic) {
      this.service.savePublicResolution(this.model).subscribe();
    }
  }

  addDeleteAmendment() {
    const resolutionid = this.model.resolutionId;
    const sectionid = this.model.operativeSection.paragraphs[0].operativeParagraphId;
    this.service.addDeleteAmendment(resolutionid, sectionid, 'Unknown');
  }

  get endIndex(): number {
    return this.model.operativeSection.paragraphs.length + 1;
  }

  createNewAmendment() {
    const type: string = this.addAmendmentType;
    const target: OperativeParagraph = this.amendmentTargetSection;
    const newText: string = this.newAmendmentNewText;

    if (type === 'delete') {
      this.service.addDeleteAmendment(this.model.resolutionId, target.operativeParagraphId, this.newamendmentDelegation).subscribe();
    } else if (type === 'change') {
      this.service.addChangeAmendment(this.model.resolutionId, target.operativeParagraphId, this.newamendmentDelegation, newText).subscribe();
    } else if (type === 'move') {
      this.service.addMoveAmendment(this.model.resolutionId, target.operativeParagraphId, this.newamendmentDelegation, this.amendmentTargetPosition).subscribe();
    } else if (type === 'add') {
      const amendment = new AddAmendment();
      amendment.TargetResolutionID = this.resolution.resolutionId;
      amendment.submitterName = this.newamendmentDelegation;
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
    r.committee = this.resolution.header.committeeName;
    r.resolutionId = this.resolution.resolutionId;
    r.supporters = this.resolution.header.supporters;
    r.title = this.resolution.header.topic;
    r.submitterName = this.resolution.header.submitterName;
    return r;
  }

  conferenceSelected() {
    //console.log(this.selectedConference);
  }

  createConferenceConnection(committee: Committee) {
    this.service.linkResolutionToCommittee(this.resolution.resolutionId, committee.committeeId).subscribe();
  }
}
