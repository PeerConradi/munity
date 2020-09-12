import { Injectable, Inject } from '@angular/core';
import * as signalR from "@aspnet/signalr";
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { UserService } from './user.service';
import { Resolution } from '../models/resolution/resolution.model';
import { PreambleParagraph } from '../models/resolution/preamble-paragraph.model';
import { ResolutionInformation } from '../models/resolution/resolution-information.model';
import { OperativeSection } from '../models/resolution/operative-section.model';
import { DeleteAmendment } from '../models/resolution/delete-amendment.model';
import { AbstractAmendment } from '../models/resolution/abstract-amendment.model';
import { AmendmentInspector } from '../models/resolution/amendment-inspector';
import { ChangeAmendment } from '../models/resolution/change-amendment.model';
import { NotifierService } from 'angular-notifier';
import { ResolutionAdvancedInfo } from '../models/resolution/resolution-advanced-info.model';
import { AddAmendment } from '../models/resolution/add-amendment.model';
import { ChangeResolutionHeaderRequest } from '../models/requests/change-resolution-header-request';
import { Notice } from '../models/notice.model';
import { OperativeParagraph } from "../models/resolution/operative-paragraph.model";
import { ResolutionHeader } from '../models/resolution/resolution-header.model';
import { Preamble } from '../models/resolution/preamble.model';

@Injectable({
  providedIn: 'root'
})
export class ResolutionService {

  private _hubConnection: signalR.HubConnection;

  private baseUrl: string;

  private connectionid: string;

  public hasError: boolean = false;

  //public resolution: Resolution;

  public orderedAmendments: AbstractAmendment[] = [];

  public connectionReady: boolean = false;

  public stack: stackElement[] = [];

  constructor(private httpClient: HttpClient, private userService: UserService,
    private notifyService: NotifierService,
    @Inject('BASE_URL') baseUrl: string) {
    this.baseUrl = baseUrl;
  }

  public generateId() {
    let text = "";
    let possible = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUFVXYZ1234567890";
    for (let i = 0; i < 30; i++) {
      text += possible.charAt(Math.floor(Math.random() * possible.length));
    }
    return text;
  }

  public createResolution() {
    return this.httpClient.get<Resolution>(this.baseUrl + 'api/Resolution/Create');
  }

  public createPublicResolution() {
    return this.httpClient.get<Resolution>(this.baseUrl + 'api/Resolution/CreatePublic');
  }

  public getPublicResolution(id: string) {
    if (id === 'test') return of<Resolution>(this.getTestResolution());
    return this.httpClient.get<Resolution>(this.baseUrl + 'api/Resolution/GetPublic?id=' + id);
  }

  public getResolution(id: string) {
    if (id === 'test') return of<Resolution>(this.getTestResolution());
    let headers = new HttpHeaders();
    headers = headers.set('id', id);
    let options = { headers: headers };
    return this.httpClient.get<Resolution>(this.baseUrl + 'api/Resolution/Get', options);
  }

  public savePublicResolution(resolution: Resolution) {
    return this.httpClient.patch<Resolution>(this.baseUrl + 'api/Resolution/UpdatePublicResolution', resolution);
  }

  public getMyResolutions() {
    let headers = new HttpHeaders();
    let options = { headers: headers };
    return this.httpClient.get<ResolutionAdvancedInfo[]>(this.baseUrl + 'api/Resolution/MyResolutions', options);
  }

  public pushResolution(resolution: Resolution) {
    return this.httpClient.put(this.baseUrl + 'api/Resolution/PutResolution', resolution);
  }

  //SignalR Part
  public addResolutionListener = (model: Resolution, inspector: AmendmentInspector) => {
    this._hubConnection.on('ResolutionChanged', (resolution: Resolution) => {
      model.header = resolution.header;
      model.operativeSection = resolution.operativeSection;
      model.preamble = resolution.preamble;
    });

    this._hubConnection.on('PreambleParagraphAdded', (position: number, id: string, text: string) => {
      let paragraph: PreambleParagraph = new PreambleParagraph();
      paragraph.preambleParagraphId = id;
      paragraph.text = text;
      model.preamble.paragraphs.push(paragraph);
      //this.resolution.OperativeSections.filter(n => n.ID == id)[0].Text = text;
    });

    this._hubConnection.on('OperativeParagraphAdded', (position: number, sectionModel: OperativeParagraph) => {
      model.operativeSection.paragraphs.push(sectionModel);
      //this.resolution.OperativeSections.filter(n => n.ID == id)[0].Text = text;
    });

    //this._hubConnection.on('PreambleParagraphChanged', (paragraph: PreambleParagraph) => {

    //  let target = model.Preamble.Paragraphs.find(n => n.ID == paragraph.ID);
    //  if (target != null && target.Text != paragraph.Text) {
    //    target.Text = paragraph.Text;
    //  }
    //  if (target != null && target.Notices != paragraph.Notices) {
    //    target.Notices = paragraph.Notices;
    //  }

    //});

    //this._hubConnection.on('OperativeParagraphChanged', (paragraph: OperativeSection) => {

    //  let target = model.OperativeSections.find(n => n.ID == paragraph.ID);
    //  if (target != null && target.Text != paragraph.Text) {
    //    target.Text = paragraph.Text;

    //  }
    //  if (target != null && target.Notices != paragraph.Notices) {
    //    target.Notices = paragraph.Notices;
    //  }
    //});

    //this._hubConnection.on('ResolutionSaved', (date: Date) => {
    //  model.lastSaved = date;
    //});

    //this._hubConnection.on('TitleChanged', (title: string) => {
    //  model.Topic = title;
    //});

    //this._hubConnection.on('CommitteeChanged', (newcommitteename: string) => {
    //  model.CommitteeName = newcommitteename;
    //});

    //this._hubConnection.on('SubmitterChanged', (newsubmittername: string) => {
    //  model.SubmitterName = newsubmittername;
    //})

    //this._hubConnection.on('PreambleSectionOrderChanged', (paragraphs: PreambleParagraph[]) => {
    //  model.Preamble.Paragraphs = paragraphs;
    //});

    //this._hubConnection.on('OperativeSectionOrderChanged', (paragraphs: OperativeSection[]) => {
    //  model.OperativeSections = paragraphs;
    //});

    //this._hubConnection.on('PreambleParaghraphRemoved', (paragraphs: PreambleParagraph[]) => {
    //  model.Preamble.Paragraphs = paragraphs;
    //});

    //this._hubConnection.on('OperativeParagraphRemoved', (resolution: Resolution) => {
    //  model.OperativeSections = resolution.OperativeSections;
    //  model.ChangeAmendments = resolution.ChangeAmendments;
    //  model.MoveAmendments = resolution.MoveAmendments;
    //  model.DeleteAmendments = resolution.DeleteAmendments;
    //  model.AddAmendmentsSave = resolution.AddAmendmentsSave;

    //  inspector.allAmendments = this.OrderAmendments(model);
    //});

    ///**
    // * One Amendment was deleted (maybe because of a typo or it was wrong not because it was Denied!)
    // **/
    //this._hubConnection.on('AmendmentRemoved', (resolution: Resolution, amendment: AbstractAmendment) => {
    //  //Fuck it! we update everything who cares about traffic?!
    //  model.OperativeSections = resolution.OperativeSections;
    //  model.ChangeAmendments = resolution.ChangeAmendments;
    //  model.DeleteAmendments = resolution.DeleteAmendments;
    //  model.MoveAmendments = resolution.MoveAmendments;
    //  //this.OnAmendmentRemoved(model, amendment);
    //  inspector.allAmendments = this.OrderAmendments(model);
    //  //inspector.allAmendments = this.OnAmendmentRemoved(model, amendment);
    //});

    //this._hubConnection.on('AmendmentDenied', (resolution: Resolution, amendment: AbstractAmendment) => {
    //  //Fuck it! we update everything who cares about traffic?!
    //  model.OperativeSections = resolution.OperativeSections;
    //  model.ChangeAmendments = resolution.ChangeAmendments;
    //  model.DeleteAmendments = resolution.DeleteAmendments;
    //  model.MoveAmendments = resolution.MoveAmendments;
    //  //this.OnAmendmentRemoved(model, amendment);
    //  inspector.allAmendments = this.OrderAmendments(model);
    //  //inspector.allAmendments = this.OnAmendmentRemoved(model, amendment);
    //});

    ///**
    // * Wenn ein Änderungsantrag eingeht wird eine OnAmendmendmentAdded aufgerufen, welcher
    // * diesen in eine der vier Listen einsortiert. Danach wird die Gesamtliste aller
    // * Änderungsanträge neu sortiert und zurückgegeben
    // **/
    //this._hubConnection.on('DeleteAmendmentAdded', (amendment: DeleteAmendment) => {
    //  inspector.allAmendments = this.OnDeleteAmendmentAdded(model, amendment);
    //});

    //this._hubConnection.on('ChangeAmendmentAdded', (amendment: ChangeAmendment) => {
    //  inspector.allAmendments = this.onChangeAmendmentAdded(model, amendment);
    //});

    //this._hubConnection.on('MoveAmendmentAdded', (resolution: Resolution, amendment: ChangeAmendment) => {
    //  model.OperativeSections = resolution.OperativeSections;
    //  model.MoveAmendments = resolution.MoveAmendments;
    //  model.DeleteAmendments = resolution.DeleteAmendments;
    //  model.ChangeAmendments = resolution.ChangeAmendments;
    //  model.AddAmendmentsSave = resolution.AddAmendmentsSave;
    //  inspector.allAmendments = this.OrderAmendments(model);
    //});

    //this._hubConnection.on('AddAmendmentAdded', (resolution: Resolution, amendment: ChangeAmendment) => {
    //  model.OperativeSections = resolution.OperativeSections;
    //  model.MoveAmendments = resolution.MoveAmendments;
    //  model.DeleteAmendments = resolution.DeleteAmendments;
    //  model.ChangeAmendments = resolution.ChangeAmendments;
    //  model.AddAmendmentsSave = resolution.AddAmendmentsSave;
    //  inspector.allAmendments = this.OrderAmendments(model);
    //});

    //this._hubConnection.on('AmendmentActivated', (resolution: Resolution, amendment: AbstractAmendment) => {
    //  //Diese beiden Änderungsanträge sind etwas komplexer und erfordern, dass die Struktur der
    //  //Operativen Abschnitte neu geladen werden, da sich dort etwas in der Vorschau ändert
    //  if (amendment.Type === 'move' || amendment.Type === 'add') {
    //    model.OperativeSections = resolution.OperativeSections;
    //  }

    //  const a = this.findAmendment(model, amendment.ID);
    //  a.Activated = true;
    //});

    //this._hubConnection.on('AmendmentDeactivated', (resolution: Resolution, amendment: AbstractAmendment) => {
    //  if (amendment.Type === 'move' || amendment.Type === 'add') {
    //    model.OperativeSections = resolution.OperativeSections;
    //  }
    //  const a = this.findAmendment(model, amendment.ID);
    //  a.Activated = false;
    //});

    this._hubConnection.on('AmendmentSubmitted', (resolution: Resolution) => {
      model.header = resolution.header;
      model.operativeSection = resolution.operativeSection;
      model.preamble = resolution.preamble;

      inspector.allAmendments = this.OrderAmendments(model);
    });

  }

  public findAmendment(resolution: Resolution, amendmentid: string): AbstractAmendment {
    var a = resolution.operativeSection.deleteAmendments.find(n => n.id === amendmentid);
    if (a == null) {
      //Search inside Change Amendments
      a = resolution.operativeSection.changeAmendments.find(n => n.id === amendmentid);
    }

    if (a == null) {
      //Search inside Move Amendments
      a = resolution.operativeSection.moveAmendments.find(n => n.id === amendmentid);
    }

    if (a == null) {
      //Search inside Add Amendments
      a = resolution.operativeSection.addAmendments.find(n => n.id === amendmentid);
    }

    return a;
  }

  public addPreambleParagraph(resolutionid: string) {
    let headers = new HttpHeaders();
    headers = headers.set('resolutionid', resolutionid);
    let options = { headers: headers };
    return this.httpClient.get(this.baseUrl + 'api/Resolution/AddPreambleParagraph', options);
  }

  public addOperativeParagraph(resolutionid: string) {
    let headers = new HttpHeaders();
    headers = headers.set('resolutionid', resolutionid);
    let options = { headers: headers };
    return this.httpClient.get(this.baseUrl + 'api/Resolution/AddOperativeParagraph', options);
  }


  public updateHeader(request: ChangeResolutionHeaderRequest) {
    let headers = new HttpHeaders();
    let options = { headers: headers };
    this.httpClient.patch(this.baseUrl + 'api/Resolution/ChangeHeader', request, options).subscribe(data => { }, err => { this.notifyService.notify('error', 'Das hat nicht geklappt :('); });
  }

  public changeOperativeParagraph(paragraph: OperativeParagraph) {
    let headers = new HttpHeaders();
    return this.httpClient.put<OperativeSection>(this.baseUrl + 'api/Resolution/UpdateOperativeSection',
      paragraph, { headers: headers });
  }

  public changeOperativeParagraphNotice(paragraph: OperativeParagraph, notice: Notice) {
    return this.httpClient.put<Notice>(this.baseUrl + 'api/Resolution/UpdateOperativeSectionNotice', notice);
  }

  public changePreambleParagraphNotice(paragraph: PreambleParagraph, notice: Notice) {
    return this.httpClient.put<Notice>(this.baseUrl + 'api/Resolution/UpdatePreambleParagraphNotice', notice);
  }

  public changeOperativeParagraphNotices(paragraph: OperativeParagraph) {
    return this.httpClient.patch<OperativeSection>(this.baseUrl + 'api/Resolution/UpdateOperativeSectionNotices', paragraph);
  }

  public changePreambleParagraphNotices(paragraph: PreambleParagraph) {
    return this.httpClient.patch<OperativeSection>(this.baseUrl + 'api/Resolution/UpdatePreambleParagraphNotices', paragraph);
  }

  public changePreambleParagraph(paragraph: PreambleParagraph) {
    return this.httpClient.put<PreambleParagraph>(this.baseUrl + 'api/Resolution/UpdatePreambleParagraph',
      paragraph);
  }

  public movePrembleParagraphUp(resolutionid: string, paragraphid: string) {

    this.httpClient.patch(this.baseUrl + 'api/Resolution/MovePreambleParagraphUp', null).subscribe(data => { }, err => { this.notifyService.notify('error', 'Das hat nicht geklappt :('); });
  }

  public movePrembleParagraphDown(resolutionid: string, paragraphid: string) {
    let headers = new HttpHeaders();
    headers = headers.set('Content-Type', 'application/json; charset=utf-8');
    headers = headers.set('resolutionid', resolutionid);
    headers = headers.set('paragraphid', paragraphid);
    let options = { headers: headers };
    this.httpClient.patch(this.baseUrl + 'api/Resolution/MovePreambleParahraphDown', null,
      options).subscribe(data => { }, err => { this.notifyService.notify('error', 'Das hat nicht geklappt :('); });
  }

  public removePrembleParagraph(resolutionid: string, paragraphid: string) {
    let headers = new HttpHeaders();
    headers = headers.set('Content-Type', 'application/json; charset=utf-8');
    headers = headers.set('resolutionid', resolutionid);
    headers = headers.set('paragraphid', paragraphid);
    let options = { headers: headers };
    this.httpClient.delete(this.baseUrl + 'api/Resolution/RemovePreambleParagraph',
      options).subscribe(data => { }, err => { this.notifyService.notify('error', 'Das hat nicht geklappt :('); });
  }

  public moveOperativeParagraphUp(resolutionid: string, paragraphid: string) {
    let headers = new HttpHeaders();
    headers = headers.set('Content-Type', 'application/json; charset=utf-8');
    headers = headers.set('resolutionid', resolutionid);
    headers = headers.set('paragraphid', paragraphid);
    let options = { headers: headers };
    this.httpClient.patch(this.baseUrl + 'api/Resolution/MoveOperativeParagraphUp', null,
      options).subscribe(data => { }, err => { this.notifyService.notify('error', 'Das hat nicht geklappt :('); });
  }

  public moveOperativeParagraphDown(resolutionid: string, paragraphid: string) {
    let headers = new HttpHeaders();
    headers = headers.set('Content-Type', 'application/json; charset=utf-8');
    headers = headers.set('resolutionid', resolutionid);
    headers = headers.set('paragraphid', paragraphid);
    let options = { headers: headers };
    this.httpClient.patch(this.baseUrl + 'api/Resolution/MoveOperativeParagraphDown', null,
      options).subscribe(data => { }, err => { this.notifyService.notify('error', 'Das hat nicht geklappt :('); });
  }

  public moveOperativeParagraphLeft(resolutionid: string, paragraphid: string) {
    let headers = new HttpHeaders();
    headers = headers.set('Content-Type', 'application/json; charset=utf-8');
    headers = headers.set('resolutionid', resolutionid);
    headers = headers.set('paragraphid', paragraphid);
    let options = { headers: headers };
    this.httpClient.patch(this.baseUrl + 'api/Resolution/MoveOperativeParagraphLeft', null,
      options).subscribe(data => { }, err => { this.notifyService.notify('error', 'Das hat nicht geklappt :('); });
  }

  public moveOperativeParagraphRight(resolutionid: string, paragraphid: string) {
    let headers = new HttpHeaders();
    headers = headers.set('Content-Type', 'application/json; charset=utf-8');
    headers = headers.set('resolutionid', resolutionid);
    headers = headers.set('paragraphid', paragraphid);
    let options = { headers: headers };
    this.httpClient.patch(this.baseUrl + 'api/Resolution/MoveOperativeParagraphRight', null,
      options).subscribe(data => { }, err => { this.notifyService.notify('error', 'Das hat nicht geklappt :('); });
  }

  public removeOperativeParagraph(resolutionid: string, paragraphid: string) {
    let headers = new HttpHeaders();
    headers = headers.set('Content-Type', 'application/json; charset=utf-8');
    headers = headers.set('resolutionid', resolutionid);
    headers = headers.set('paragraphid', paragraphid);
    let options = { headers: headers };
    this.httpClient.delete(this.baseUrl + 'api/Resolution/RemoveOperativeParagraph',
      options).subscribe(data => { }, err => { this.notifyService.notify('error', 'Das hat nicht geklappt :('); });
  }

  public getAdvancedInfos(resolutionid: string) {
    let headers = new HttpHeaders();
    headers = headers.set('Content-Type', 'application/json; charset=utf-8');
    headers = headers.set('id', resolutionid);
    let options = { headers: headers };
    return this.httpClient.get<ResolutionAdvancedInfo>(this.baseUrl + 'api/Resolution/GetResolutionInfos',
      options);
  }

  public canIEditResolution(resolutionid: string) {
    let headers = new HttpHeaders();
    headers = headers.set('Content-Type', 'application/json; charset=utf-8');
    headers = headers.set('id', resolutionid);
    let options = { headers: headers };
    return this.httpClient.get<boolean>(this.baseUrl + 'api/Resolution/CanAuthEditResolution',
      options);
  }

  public denyAmendment(resolutionid: string, amendmentid: string) {
    let headers = new HttpHeaders();
    headers = headers.set('resolutionid', resolutionid);
    headers = headers.set('amendmentid', amendmentid);
    return this.httpClient.patch(this.baseUrl + 'api/Resolution/DenyAmendment', null, { headers: headers });
  }

  public changePublicReadMode(resolutionid: string, mode: boolean) {
    if (this.userService.session) {
      //Soll zunächst nur möglich sein wenn man auch eingeloggt ist.
      //Dieser Schutz muss aber auch Serverside implementiert werden!
      const authString = this.userService.sessionkey();

      let headers = new HttpHeaders();
      headers = headers.set('content-type', 'application/json; charset=utf-8');
      headers = headers.set('resolutionid', resolutionid);
      let modetext = 'false';
      if (mode) {
        modetext = 'true';
      }
      headers = headers.set('pmode', modetext);
      let options = { headers: headers };
      return this.httpClient.patch<string>(this.baseUrl + 'api/Resolution/ChangePublicReadMode', null,
        options);
    }

  }

  public subscribeToResolution(id: string) {
    const builder = new signalR.HubConnectionBuilder().withUrl(this.baseUrl + 'resasocket');
    this._hubConnection = builder.build();
    this._hubConnection.serverTimeoutInMilliseconds = 9600000;
    this._hubConnection
      .start()
      .then(() => {
        var hub = this._hubConnection;

        var connectionUrl: string = hub["connection"].transport.webSocket.url;
        this.connectionid = connectionUrl.split('=')[1];
        this.connectionReady = true;

        let headers = new HttpHeaders();
        headers = headers.set('content-type', 'application/json; charset=utf-8');
        headers = headers.set('id', id);
        headers = headers.set('connectionid', this.connectionid);
        let options = { headers: headers };
        this.httpClient.put(this.baseUrl + 'api/Resolution/SubscribeToResolution', null,
          options).subscribe(data => { }, err => { this.notifyService.notify('error', 'Das hat nicht geklappt :('); });
      })
      .catch(err => {
        this.hasError = true;
      });
  }

  public addDeleteAmendment(resolutionid: string, paragraphid: string, submitter: string) {

    let headers = new HttpHeaders();
    headers = headers.set('content-type', 'application/json; charset=utf-8');
    headers = headers.set('resolutionid', resolutionid);
    headers = headers.set('sectionid', paragraphid);
    headers = headers.set('submittername', encodeURI(submitter + '|'));
    let options = { headers: headers };
    return this.httpClient.post(this.baseUrl + 'api/Resolution/AddDeleteAmendment', null,
      options);
  }

  public addChangeAmendment(resolutionid: string, paragraphid: string, submitter: string, newtext: string) {
    let headers = new HttpHeaders();
    headers = headers.set('content-type', 'application/json; charset=utf-8');
    headers = headers.set('resolutionid', resolutionid);
    const amendmentModel = new ChangeAmendment();
    amendmentModel.targetSectionId = paragraphid;
    amendmentModel.newText = newtext;
    amendmentModel.submitterName = submitter;
    let options = { headers: headers };
    return this.httpClient.post(this.baseUrl + 'api/Resolution/AddChangeAmendment', amendmentModel, options);
  }

  public addMoveAmendment(resolutionid: string, paragraphid: string, submitter: string, newPosition: number) {
    let headers = new HttpHeaders();
    headers = headers.set('content-type', 'application/json; charset=utf-8');
    headers = headers.set('resolutionid', resolutionid);
    headers = headers.set('sectionid', paragraphid);
    headers = headers.set('submittername', encodeURI(submitter + '|'));
    headers = headers.set('newposition', newPosition.toString());
    let options = { headers: headers };
    return this.httpClient.post(this.baseUrl + 'api/Resolution/AddMoveAmendment', null, options);
  }

  public addAddAmendment(amendment: AddAmendment) {
    let headers = new HttpHeaders();
    headers = headers.set('content-type', 'application/json; charset=utf-8');
    return this.httpClient.post<AddAmendment>(this.baseUrl + 'api/Resolution/AddAddAmendment',
      amendment, { headers: headers });
  }

  removeAmendment(resolutionid: string, amendmentid: string) {
    let headers = new HttpHeaders();
    headers = headers.set('content-type', 'application/json; charset=utf-8');
    headers = headers.set('resolutionid', resolutionid);
    headers = headers.set('amendmentid', amendmentid);
    let options = { headers: headers };
    this.httpClient.delete(this.baseUrl + 'api/Resolution/RemoveAmendment',
      options).subscribe(data => { }, err => { this.notifyService.notify('error', 'Das hat nicht geklappt :('); });
  }

  activateAmendment(resolutionid: string, amendmentid: string) {
    let headers = new HttpHeaders();
    headers = headers.set('content-type', 'application/json; charset=utf-8');
    headers = headers.set('resolutionid', resolutionid);
    headers = headers.set('amendmentid', amendmentid);
    let options = { headers: headers };
    this.httpClient.patch(this.baseUrl + 'api/Resolution/ActivateAmendment', null,
      options).subscribe(data => { }, err => { this.notifyService.notify('error', 'Das hat nicht geklappt :('); });
  }

  deactivateAmendment(resolutionid: string, amendmentid: string) {
    let headers = new HttpHeaders();
    headers = headers.set('content-type', 'application/json; charset=utf-8');
    headers = headers.set('resolutionid', resolutionid);
    headers = headers.set('amendmentid', amendmentid);
    let options = { headers: headers };
    this.httpClient.patch(this.baseUrl + 'api/Resolution/DeactivateAmendment', null,
      options).subscribe(data => { }, err => { this.notifyService.notify('error', 'Das hat nicht geklappt :('); });
  }

  submitAmendment(resolutionid: string, amendmentid: string) {
    let headers = new HttpHeaders();
    headers = headers.set('content-type', 'application/json; charset=utf-8');
    headers = headers.set('resolutionid', resolutionid);
    headers = headers.set('amendmentid', amendmentid);
    let options = { headers: headers };
    this.httpClient.patch(this.baseUrl + 'api/Resolution/SubmitAmendment', null,
      options).subscribe(data => { }, err => { this.notifyService.notify('error', 'Das hat nicht geklappt :('); });
  }

  public getResolutionsOfConference(conferenceid: string) {
    let headers = new HttpHeaders();
    headers = headers.set('conferenceid', conferenceid);
    return this.httpClient.get<ResolutionAdvancedInfo[]>(this.baseUrl + 'api/Resolution/GetResolutionsOfConference',
      { headers: headers });
  }

  public getResolutionsOfCommittee(committeeid: string) {
    let headers = new HttpHeaders();
    headers = headers.set('committeeid', committeeid);
    return this.httpClient.get<ResolutionAdvancedInfo[]>(this.baseUrl + 'api/Resolution/GetResolutionsOfCommittee',
      { headers: headers });
  }

  public linkResolutionToConference(resolutionid: string, conferenceid: string) {
    let headers = new HttpHeaders();
    headers = headers.set('resolutionid', resolutionid);
    headers = headers.set('conferenceid', conferenceid);
    return this.httpClient.patch(this.baseUrl + 'api/Resolution/LinkResolutionToConference', null, { headers: headers });
  }

  public linkResolutionToCommittee(resolutionid: string, committeeid: string) {
    let headers = new HttpHeaders();
    headers = headers.set('resolutionid', resolutionid);
    headers = headers.set('committeeid', committeeid);
    return this.httpClient.patch(this.baseUrl + 'api/Resolution/LinkResolutionToCommittee', null, { headers: headers });
  }

  public onDeleteAmendmentAdded(resolution: Resolution, amendment: DeleteAmendment): AbstractAmendment[] {
    if (resolution.operativeSection.deleteAmendments.find(n => n.id === amendment.id) == null) {

      resolution.operativeSection.deleteAmendments.push(amendment);
      const target = resolution.operativeSection.paragraphs.find(n => n.operativeParagraphId === amendment.targetSectionId);
      if (target != null) {
        // target.deleteAmendmentCount += 1;
      }
      return this.OrderAmendments(resolution);
    }
  }

  public onChangeAmendmentAdded(resolution: Resolution, amendment: ChangeAmendment): AbstractAmendment[] {
    if (resolution.operativeSection.changeAmendments.find(n => n.id == amendment.id) == null) {
      resolution.operativeSection.changeAmendments.push(amendment);
      const target = resolution.operativeSection.paragraphs.find(n => n.operativeParagraphId == amendment.targetSectionId);
      if (target != null) {
        // target.changeAmendmentCount += 1;
      }
      return this.OrderAmendments(resolution);
    }
  }

  public OrderAmendments(resolution: Resolution): AbstractAmendment[] {
    const arr = [];
    //All Sections
    resolution.operativeSection.paragraphs.forEach(oa => {
      //Delete Amendments
      resolution.operativeSection.deleteAmendments.forEach(n => { if (n.targetSectionId === oa.operativeParagraphId) arr.push(n) });

      //Change Amendments
      resolution.operativeSection.changeAmendments.forEach(n => { if (n.targetSectionId === oa.operativeParagraphId) arr.push(n) });

      //Move Amendments
      resolution.operativeSection.moveAmendments.forEach(n => { if (n.targetSectionId === oa.operativeParagraphId) arr.push(n) });


    });
    //Add Amendments
    resolution.operativeSection.addAmendments.forEach(n => arr.push(n));
    return arr;
  }

  public getTestResolution(): Resolution {
    let resolution = new Resolution();
    resolution.resolutionId = 'test';
    resolution.date = new Date(Date.now());
    resolution.header = new ResolutionHeader();
    resolution.header.topic = 'Thema'
    resolution.header.committeeName = 'die Generalversammlung';
    resolution.header.submitterName = 'Deutschland';
    resolution.header.supporters = [];
    resolution.header.supporters.push('Frankreich', 'Österreich');
    resolution.preamble = new Preamble();
    resolution.preamble.paragraphs = [];
    resolution.operativeSection = new OperativeSection();
    resolution.operativeSection.addAmendments = [];
    resolution.operativeSection.changeAmendments = [];
    resolution.operativeSection.deleteAmendments = [];
    resolution.operativeSection.moveAmendments = [];
    resolution.operativeSection.paragraphs = [];

    let preaParagraphOne = new PreambleParagraph();
    preaParagraphOne.preambleParagraphId = 'prea1';
    preaParagraphOne.text = 'Dies ist ein Preambel Absatz.';
    resolution.preamble.paragraphs.push(preaParagraphOne);

    let operativeParagraphOne = new OperativeParagraph();
    operativeParagraphOne.operativeParagraphId = 'op1';
    operativeParagraphOne.text = 'Dies ist ein Operativer Absatz. Er ist markdown fähig und kann daher mit einem Unterstrich _ markierte Worte _krusiv_ machen und mit zwei Worte __dick__';
    operativeParagraphOne.isVirtual = false;
    operativeParagraphOne.notices = [];
    operativeParagraphOne.children = [];
    operativeParagraphOne.isLocked = false;
    operativeParagraphOne.name = 'Test Paragraph';
    operativeParagraphOne.visible = true;
    resolution.operativeSection.paragraphs.push(operativeParagraphOne);

    let operativeParagraphTwo = new OperativeParagraph();
    operativeParagraphTwo.operativeParagraphId = 'op2';
    operativeParagraphTwo.text = 'Dies ist ein weiterer Operative Absatz';
    operativeParagraphTwo.isVirtual = false;
    operativeParagraphTwo.notices = [];
    operativeParagraphTwo.children = [];
    operativeParagraphTwo.isLocked = false;
    operativeParagraphTwo.name = 'Test Paragraph';
    operativeParagraphTwo.visible = true;
    resolution.operativeSection.paragraphs.push(operativeParagraphTwo);

    let subParagraphOne = new OperativeParagraph();
    subParagraphOne.operativeParagraphId = 'op2';
    subParagraphOne.text = 'Dies ist ein weiterer Operativer Absatz und ich habe das Gefühl, dass in der mobilen Ansicht an sehr merkwürdigen stellen Umbrüche gemacht werden.';
    subParagraphOne.isVirtual = false;
    subParagraphOne.notices = [];
    subParagraphOne.children = [];
    subParagraphOne.isLocked = false;
    subParagraphOne.name = 'Test Paragraph';
    subParagraphOne.visible = true;
    operativeParagraphOne.children.push(subParagraphOne);

    return resolution;
  }

  public getPathOfOperativeParagraph(paragraph: OperativeParagraph, resolution: Resolution): string {
    let result = '';
    let firstLevel = resolution.operativeSection.paragraphs.indexOf(paragraph);
    if (firstLevel !== - 1) return (firstLevel + 1).toString();

    let path: OperativeParagraph[] = [];

    resolution.operativeSection.paragraphs.forEach(n => {
      this.resolveOperativeParagraphPath(paragraph, n, path);
    });

    let reversedList = path.reverse();
    let index: number = 0;
    let lastParagraph: OperativeParagraph;
    for (let p of reversedList) {
      if (index === 0) {
        let pIndex = resolution.operativeSection.paragraphs.indexOf(p);
        result += (pIndex + 1).toString() + '.';
        lastParagraph = p;

      }
      else {
        let pIndex = lastParagraph.children.indexOf(p);
        result += (pIndex + 1).toString() + '.'
        lastParagraph = p;
      }
      index += 1;
    }
    if (result.endsWith('.')) result = result.substr(0, result.length - 1);

    return result;
  }

  public resolveOperativeParagraphPath(paragraph: OperativeParagraph, currentParagraph: OperativeParagraph, path: OperativeParagraph[]): boolean {
    // go recursive inside
    if (currentParagraph == paragraph) {
      path.push(currentParagraph);
      return true;
    }

    if (currentParagraph.children === null || currentParagraph.children.length === 0) {
      // dead end
      return false;
    }

    // Go deeper
    for (let p of currentParagraph.children) {
      // If this path found some object add this paragraph
      if (this.resolveOperativeParagraphPath(paragraph, p, path)) {
        path.push(currentParagraph);
      }
    }

    return false;

  }

}

export class stackElement {
  methodName: string;
  args: any;
}
