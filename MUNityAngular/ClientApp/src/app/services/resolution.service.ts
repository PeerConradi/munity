import { Injectable, Inject } from '@angular/core';
import * as signalR from "@aspnet/signalr";
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { UserService } from './user.service';
import { Resolution } from '../models/resolution.model';
import { PreambleParagraph } from '../models/preamble-paragraph.model';
import { ResolutionInformation } from '../models/resolution-information.model';
import { OperativeSection } from '../models/operative-section.model';
import { DeleteAmendment } from '../models/delete-amendment.model';
import { AbstractAmendment } from '../models/abstract-amendment.model';
import { AmendmentInspector } from '../models/amendment-inspector';
import { ChangeAmendment } from '../models/change-amendment.model';

@Injectable({
  providedIn: 'root'
})
export class ResolutionService {
    
  private _hubConnection: signalR.HubConnection;

  private baseUrl: string;

  public hasError: boolean = false;

  //public resolution: Resolution;

  public orderedAmendments: AbstractAmendment[] = [];

  public connectionReady: boolean = false;

  public stack: stackElement[] = [];

  constructor(private httpClient: HttpClient, private userService: UserService, @Inject('BASE_URL') baseUrl: string) {
    this.baseUrl = baseUrl;
    this._hubConnection = new signalR.HubConnectionBuilder().withUrl(baseUrl + 'resasocket').build();
    this._hubConnection
      .start()
      .then(() => {
        this.connectionReady = true;
        this.stack.forEach(n => {
          this._hubConnection.send(n.methodName, n.args);
        });
        this.stack = [];
      })
      .catch(err => {
        this.hasError = true;
      });
  }

  public createResolution() {
    let authString: string = 'default';
    if (this.userService.isLoggedIn)
      authString = this.userService.sessionkey();
    let headers = new HttpHeaders();
    headers = headers.set('auth', authString);
    let options = { headers: headers };
    return this.httpClient.get<Resolution>(this.baseUrl + 'api/Resolution/Create', options);
  }


  public getResolution(id: string) {
    let authString: string = 'default';
    if (this.userService.isLoggedIn)
      authString = this.userService.sessionkey();
    let headers = new HttpHeaders();
    headers = headers.set('auth', authString);
    headers = headers.set('id', id);
    let options = { headers: headers };
    return this.httpClient.get<Resolution>(this.baseUrl + 'api/Resolution/Get', options);
  }

  public getMyResolutions() {
    let authString = this.userService.sessionkey();
    let headers = new HttpHeaders();
    headers = headers.set('auth', authString);
    let options = { headers: headers };
    return this.httpClient.get<ResolutionInformation[]>(this.baseUrl + 'api/Resolution/MyResolutions', options);
  }

  //SignalR Part
  public addResolutionListener = (model: Resolution, inspector: AmendmentInspector) => {
    this._hubConnection.on('PreambleParagraphAdded', (position: number, id: string, text: string) => {
      let paragraph: PreambleParagraph = new PreambleParagraph();
      paragraph.ID = id;
      paragraph.Text = text;
      model.Preamble.Paragraphs.push(paragraph);
      //this.resolution.OperativeSections.filter(n => n.ID == id)[0].Text = text;
    });

    this._hubConnection.on('OperativeParagraphAdded', (position: number, sectionModel: OperativeSection) => {
      model.OperativeSections.push(sectionModel);
      //this.resolution.OperativeSections.filter(n => n.ID == id)[0].Text = text;
    });

    this._hubConnection.on('PreambleParagraphChanged', (id: string, newtext: string) => {

      let target = model.Preamble.Paragraphs.filter(n => n.ID == id)[0];
      if (target != null && target.Text != newtext) {
        target.Text = newtext;
      }
    });

    this._hubConnection.on('OperativeParagraphChanged', (id: string, newtext: string) => {

      let target = model.OperativeSections.filter(n => n.ID == id)[0];
      if (target != null && target.Text != newtext) {
        target.Text = newtext;
      }
    });

    this._hubConnection.on('ResolutionSaved', (date: Date) => {
      model.lastSaved = date;
    });

    this._hubConnection.on('TitleChanged', (title: string) => {
      model.Topic = title;
    });

    this._hubConnection.on('CommitteeChanged', (newcommitteename: string) => {
      model.CommitteeName = newcommitteename;
    });

    this._hubConnection.on('AmendmentRemoved', (resolution: Resolution, amendment: AbstractAmendment) => {
      //model.OperativeSections = resolution.OperativeSections;
      //model.ChangeAmendments = resolution.ChangeAmendments;
      //model.DeleteAmendments = resolution.DeleteAmendments;
      //model.MoveAmendments = resolution.MoveAmendments;
      this.OnAmendmentRemoved(model, amendment);
      inspector.allAmendments = this.OnAmendmentRemoved(model, amendment);
    });

    /**
     * Wenn ein Änderungsantrag eingeht wird eine OnAmendmendmentAdded aufgerufen, welcher
     * diesen in eine der vier Listen einsortiert. Danach wird die Gesamtliste aller
     * Änderungsanträge neu sortiert und zurückgegeben
     **/
    this._hubConnection.on('DeleteAmendmentAdded', (amendment: DeleteAmendment) => {
      inspector.allAmendments = this.OnDeleteAmendmentAdded(model, amendment);
    });

    this._hubConnection.on('ChangeAmendmentAdded', (amendment: ChangeAmendment) => {
      inspector.allAmendments = this.onChangeAmendmentAdded(model, amendment);
    });

    this._hubConnection.on('AmendmentActivated', (resolution: Resolution, amendment: AbstractAmendment) => {
      //Diese beiden Änderungsanträge sind etwas komplexer und erfordern, dass die Struktur der
      //Operativen Abschnitte neu geladen werden, da sich dort etwas in der Vorschau ändert
      if (amendment.Type === 'move' || amendment.Type === 'add') {
        model.OperativeSections = resolution.OperativeSections;
      }

      const a = this.findAmendment(model, amendment.ID);
      a.Activated = true;
    });

    this._hubConnection.on('AmendmentDeactivated', (amendment: AbstractAmendment) => {
      const a = this.findAmendment(model, amendment.ID);
      a.Activated = false;
    });

    this._hubConnection.on('AmendmentSubmitted', (resolution: Resolution) => {
      model.OperativeSections = resolution.OperativeSections;
      model.DeleteAmendments = resolution.DeleteAmendments;
      model.ChangeAmendments = resolution.ChangeAmendments;
      model.MoveAmendments = resolution.MoveAmendments;
      //Anderen Amendments ebenfalls ersetzen!
      inspector.allAmendments = this.OrderAmendments(model);
    });
    
  }

  public findAmendment(resolution: Resolution, amendmentid: string): AbstractAmendment {
    var a = resolution.DeleteAmendments.find(n => n.ID === amendmentid);
    if (a == null) {
      //Search inside Change Amendments
      a = resolution.ChangeAmendments.find(n => n.ID === amendmentid);
    }

    if (a == null) {
      //Search inside Move Amendments
      a = resolution.MoveAmendments.find(n => n.ID === amendmentid);
    }

    if (a == null) {
      //Search inside Add Amendments
    }

    return a;
  }

  public addPreambleParagraph(resolutionid: string) {
    let authString: string = 'default';
    if (this.userService.isLoggedIn)
      authString = this.userService.sessionkey();
    let headers = new HttpHeaders();
    headers = headers.set('auth', authString);
    headers = headers.set('resolutionid', resolutionid);
    let options = { headers: headers };
    return this.httpClient.get(this.baseUrl + 'api/Resolution/AddPreambleParagraph', options);
  }

  public addOperativeParagraph(resolutionid: string) {
    let authString: string = 'default';
    if (this.userService.isLoggedIn)
      authString = this.userService.sessionkey();
    let headers = new HttpHeaders();
    headers = headers.set('auth', authString);
    headers = headers.set('resolutionid', resolutionid);
    let options = { headers: headers };
    return this.httpClient.get(this.baseUrl + 'api/Resolution/AddOperativeParagraph', options);
  }

  public changeTitle(resolutionid: string, newtitle: string) {
    let authString: string = 'default';
    if (this.userService.isLoggedIn)
      authString = this.userService.sessionkey();
    let headers = new HttpHeaders();
    headers = headers.set('auth', authString);
    headers = headers.set('resolutionid', resolutionid);
    headers = headers.set('newtitle', encodeURI(newtitle + "|"));
    let options = { headers: headers };
    this.httpClient.get<Resolution>(this.baseUrl + 'api/Resolution/ChangeTitle', options).subscribe(data => { }, err => { console.log('eror while changing title'); });
  }

  public changeCommittee(resolutionid: string, newcommitteename: string) {
    let authString: string = 'default';
    if (this.userService.isLoggedIn)
      authString = this.userService.sessionkey();
    let headers = new HttpHeaders();
    headers = headers.set('auth', authString);
    headers = headers.set('resolutionid', resolutionid);
    headers = headers.set('newcommitteename', encodeURI(newcommitteename + "|"));
    let options = { headers: headers };
    this.httpClient.get<Resolution>(this.baseUrl + 'api/Resolution/ChangeCommittee', options).subscribe(data => { }, err => { console.log('eror while changing title'); });
  }

  public changePreambleParagraph(resolutionid: string, paragraphid: string, newtext: string) {
    let authString: string = 'default';
    if (this.userService.isLoggedIn)
      authString = this.userService.sessionkey();

    let headers = new HttpHeaders();
    headers = headers.set('Content-Type', 'application/json; charset=utf-8');
    headers = headers.set('auth', authString);
    headers = headers.set('resolutionid', resolutionid);
    headers = headers.set('paragraphid', paragraphid);
    headers = headers.set('newtext', encodeURI(newtext + '|'));
    let options = { headers: headers };
    this.httpClient.get(this.baseUrl + 'api/Resolution/ChangePreambleParagraph',
      options).subscribe(data => { });
  }

  public changeOperativeParagraph(resolutionid: string, paragraphid: string, newtext: string) {
    let authString: string = 'default';
    if (this.userService.isLoggedIn)
      authString = this.userService.sessionkey();

    let headers = new HttpHeaders();
    headers = headers.set('content-type', 'application/json; charset=utf-8');
    headers = headers.set('auth', authString);
    headers = headers.set('resolutionid', resolutionid);
    headers = headers.set('paragraphid', paragraphid);
    headers = headers.set('newtext', encodeURI(newtext + '|'));
    let options = { headers: headers };
    this.httpClient.get(this.baseUrl + 'api/Resolution/ChangeOperativeParagraph',
      options).subscribe(data => { });
  }

  public subscribeToResolution(id: string) {
    if (this.connectionReady == true)
      this._hubConnection.send('SubscribeToResolution', id);
    else {
      let element = new stackElement();
      element.methodName = 'SubscribeToResolution';
      element.args = id;
      this.stack.push(element);
    }
  }

  public addDeleteAmendment(resolutionid: string, paragraphid: string, submitter: string) {
    let authString: string = 'default';
    if (this.userService.isLoggedIn)
      authString = this.userService.sessionkey();

    let headers = new HttpHeaders();
    headers = headers.set('content-type', 'application/json; charset=utf-8');
    headers = headers.set('auth', authString);
    headers = headers.set('resolutionid', resolutionid);
    headers = headers.set('sectionid', paragraphid);
    headers = headers.set('sumbittername', submitter);
    let options = { headers: headers };
    this.httpClient.get(this.baseUrl + 'api/Resolution/AddDeleteAmendment',
      options).subscribe(data => { });
  }

  public addChangeAmendment(resolutionid: string, paragraphid: string, submitter: string, newtext: string) {
    let authString: string = 'default';
    if (this.userService.isLoggedIn)
      authString = this.userService.sessionkey();

    let headers = new HttpHeaders();
    headers = headers.set('content-type', 'application/json; charset=utf-8');
    headers = headers.set('auth', authString);
    headers = headers.set('resolutionid', resolutionid);
    headers = headers.set('sectionid', paragraphid);
    headers = headers.set('sumbittername', submitter);
    headers = headers.set('newtext', encodeURI(newtext + '|'));
    let options = { headers: headers };
    this.httpClient.get(this.baseUrl + 'api/Resolution/AddChangeAmendment',
      options).subscribe(data => { });
  }

  removeAmendment(resolutionid: string, amendmentid: string) {
    let authString: string = 'default';
    if (this.userService.isLoggedIn)
      authString = this.userService.sessionkey();

    let headers = new HttpHeaders();
    headers = headers.set('content-type', 'application/json; charset=utf-8');
    headers = headers.set('auth', authString);
    headers = headers.set('resolutionid', resolutionid);
    headers = headers.set('amendmentid', amendmentid);
    let options = { headers: headers };
    this.httpClient.get(this.baseUrl + 'api/Resolution/RemoveAmendment',
      options).subscribe(data => { });
  }

  activateAmendment(resolutionid: string, amendmentid: string) {
    let authString: string = 'default';
    if (this.userService.isLoggedIn)
      authString = this.userService.sessionkey();

    let headers = new HttpHeaders();
    headers = headers.set('content-type', 'application/json; charset=utf-8');
    headers = headers.set('auth', authString);
    headers = headers.set('resolutionid', resolutionid);
    headers = headers.set('amendmentid', amendmentid);
    let options = { headers: headers };
    this.httpClient.get(this.baseUrl + 'api/Resolution/ActivateAmendment',
      options).subscribe(data => { });
  }

  deactivateAmendment(resolutionid: string, amendmentid: string) {
    let authString: string = 'default';
    if (this.userService.isLoggedIn)
      authString = this.userService.sessionkey();

    let headers = new HttpHeaders();
    headers = headers.set('content-type', 'application/json; charset=utf-8');
    headers = headers.set('auth', authString);
    headers = headers.set('resolutionid', resolutionid);
    headers = headers.set('amendmentid', amendmentid);
    let options = { headers: headers };
    this.httpClient.get(this.baseUrl + 'api/Resolution/DeactivateAmendment',
      options).subscribe(data => { });
  }

  submitAmendment(resolutionid: string, amendmentid: string) {
    let authString: string = 'default';
    if (this.userService.isLoggedIn)
      authString = this.userService.sessionkey();

    let headers = new HttpHeaders();
    headers = headers.set('content-type', 'application/json; charset=utf-8');
    headers = headers.set('auth', authString);
    headers = headers.set('resolutionid', resolutionid);
    headers = headers.set('amendmentid', amendmentid);
    let options = { headers: headers };
    this.httpClient.get(this.baseUrl + 'api/Resolution/SubmitAmendment',
      options).subscribe(data => { });
  }

  public OnDeleteAmendmentAdded(resolution: Resolution, amendment: DeleteAmendment): AbstractAmendment[] {
    if (resolution.DeleteAmendments.find(n => n.ID == amendment.ID) == null) {

      resolution.DeleteAmendments.push(amendment);
      const target = resolution.OperativeSections.find(n => n.ID == amendment.TargetSectionID);
      if (target != null) {
        target.DeleteAmendmentCount += 1;
      }
      return this.OrderAmendments(resolution);
    }
  }

  public onChangeAmendmentAdded(resolution: Resolution, amendment: ChangeAmendment): AbstractAmendment[] {
    if (resolution.ChangeAmendments.find(n => n.ID == amendment.ID) == null) {
      resolution.ChangeAmendments.push(amendment);
      const target = resolution.OperativeSections.find(n => n.ID == amendment.TargetSectionID);
      if (target != null) {
        target.ChangeAmendmentCount += 1;
      }
      return this.OrderAmendments(resolution);
    }
  }

  public OnAmendmentRemoved(resolution: Resolution, amendment: AbstractAmendment): AbstractAmendment[] {
    const amendmentElement = this.findAmendment(resolution, amendment.ID);
    if (amendmentElement != null) {
      const target = resolution.OperativeSections.find(n => n.ID == amendment.TargetSectionID);
      if (target != null) {
        if (amendment.Type === 'delete') {
          target.DeleteAmendmentCount -= 1;
        } else if (amendment.Type === 'change') {
          target.ChangeAmendmentCount -= 1;
        }
      }
      if (amendment.Type === 'delete') {
        const index: number = resolution.DeleteAmendments.indexOf(amendmentElement);
        if (index !== -1) {
          resolution.DeleteAmendments.splice(index, 1);
        }
      } else if (amendment.Type === 'change') {
        const element = resolution.ChangeAmendments.find(n => n.ID === amendment.ID);
        const index: number = resolution.ChangeAmendments.indexOf(element);
        if (index !== -1) {
          resolution.ChangeAmendments.splice(index, 1);
        }
      } else if
    }
    return this.OrderAmendments(resolution);
  }

  public OrderAmendments(resolution: Resolution): AbstractAmendment[] {
    const arr = [];
    //All Sections
    resolution.OperativeSections.forEach(oa => {
      //Delete Amendments
      console.log(resolution.DeleteAmendments);
      console.log(oa.ID);
      resolution.DeleteAmendments.forEach(n => { if (n.TargetSectionID == oa.ID) arr.push(n) });

      //Change Amendments
      resolution.ChangeAmendments.forEach(n => { if (n.TargetSectionID == oa.ID) arr.push(n) });

      //Move Amendments
      resolution.MoveAmendments.forEach(n => { if (n.TargetSectionID == oa.ID) arr.push(n) });

      //Add Amendments
    });
    return arr;
  }

  
}

export class stackElement {
  methodName: string;
  args: any;
}
