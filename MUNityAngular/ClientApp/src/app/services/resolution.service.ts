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
import { NotifierService } from 'angular-notifier';
import { ResolutionAdvancedInfo } from '../models/resolution-advanced-info.model';

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

    this._hubConnection.on('SubmitterChanged', (newsubmittername: string) => {
      model.SubmitterName = newsubmittername;
    })

    this._hubConnection.on('PreambleSectionOrderChanged', (paragraphs: PreambleParagraph[]) => {
      model.Preamble.Paragraphs = paragraphs;
    });

    this._hubConnection.on('OperativeSectionOrderChanged', (paragraphs: OperativeSection[]) => {
      model.OperativeSections = paragraphs;
    });

    this._hubConnection.on('PreambleParaghraphRemoved', (paragraphs: PreambleParagraph[]) => {
      model.Preamble.Paragraphs = paragraphs;
    });

    this._hubConnection.on('OperativeSectionRemoved', (resolution: Resolution) => {
      model.OperativeSections = resolution.OperativeSections;
      model.ChangeAmendments = resolution.ChangeAmendments;
      model.MoveAmendments = resolution.MoveAmendments;
      model.DeleteAmendments = resolution.DeleteAmendments;
      model.AddAmendmentsSave = resolution.AddAmendmentsSave;
    });

    /**
     * One Amendment was deleted (maybe because of a typo or it was wrong not because it was Denied!)
     **/
    this._hubConnection.on('AmendmentRemoved', (resolution: Resolution, amendment: AbstractAmendment) => {
      //Fuck it! we update everything who cares about traffic?!
      model.OperativeSections = resolution.OperativeSections;
      model.ChangeAmendments = resolution.ChangeAmendments;
      model.DeleteAmendments = resolution.DeleteAmendments;
      model.MoveAmendments = resolution.MoveAmendments;
      //this.OnAmendmentRemoved(model, amendment);
      inspector.allAmendments = this.OrderAmendments(model);
      //inspector.allAmendments = this.OnAmendmentRemoved(model, amendment);
    });

    this._hubConnection.on('AmendmentDenied', (resolution: Resolution, amendment: AbstractAmendment) => {
      //Fuck it! we update everything who cares about traffic?!
      model.OperativeSections = resolution.OperativeSections;
      model.ChangeAmendments = resolution.ChangeAmendments;
      model.DeleteAmendments = resolution.DeleteAmendments;
      model.MoveAmendments = resolution.MoveAmendments;
      //this.OnAmendmentRemoved(model, amendment);
      inspector.allAmendments = this.OrderAmendments(model);
      //inspector.allAmendments = this.OnAmendmentRemoved(model, amendment);
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

    this._hubConnection.on('MoveAmendmentAdded', (resolution: Resolution, amendment: ChangeAmendment) => {
      model.OperativeSections = resolution.OperativeSections;
      model.MoveAmendments = resolution.MoveAmendments;
      model.DeleteAmendments = resolution.DeleteAmendments;
      model.ChangeAmendments = resolution.ChangeAmendments;
      model.AddAmendmentsSave = resolution.AddAmendmentsSave;
      inspector.allAmendments = this.OrderAmendments(model);
    });

    this._hubConnection.on('AddAmendmentAdded', (resolution: Resolution, amendment: ChangeAmendment) => {
      model.OperativeSections = resolution.OperativeSections;
      model.MoveAmendments = resolution.MoveAmendments;
      model.DeleteAmendments = resolution.DeleteAmendments;
      model.ChangeAmendments = resolution.ChangeAmendments;
      model.AddAmendmentsSave = resolution.AddAmendmentsSave;
      inspector.allAmendments = this.OrderAmendments(model);
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

    this._hubConnection.on('AmendmentDeactivated', (resolution: Resolution, amendment: AbstractAmendment) => {
      if (amendment.Type === 'move' || amendment.Type === 'add') {
        model.OperativeSections = resolution.OperativeSections;
      }
      const a = this.findAmendment(model, amendment.ID);
      a.Activated = false;
    });

    this._hubConnection.on('AmendmentSubmitted', (resolution: Resolution) => {
      model.OperativeSections = resolution.OperativeSections;
      model.DeleteAmendments = resolution.DeleteAmendments;
      model.ChangeAmendments = resolution.ChangeAmendments;
      model.MoveAmendments = resolution.MoveAmendments;
      model.AddAmendmentsSave = resolution.AddAmendmentsSave;
      
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
      a = resolution.AddAmendmentsSave.find(n => n.ID === amendmentid);
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
    this.httpClient.get<Resolution>(this.baseUrl + 'api/Resolution/ChangeTitle', options).subscribe(data => { }, err => { this.notifyService.notify('error', 'Das hat nicht geklappt :('); });
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
    this.httpClient.get<Resolution>(this.baseUrl + 'api/Resolution/ChangeCommittee', options).subscribe(data => { }, err => { this.notifyService.notify('error', 'Das hat nicht geklappt :('); });
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
      options).subscribe(data => { }, err => { this.notifyService.notify('error', 'Das hat nicht geklappt :('); });
  }

  public movePrembleParagraphUp(resolutionid: string, paragraphid: string) {
    let authString: string = 'default';
    if (this.userService.isLoggedIn)
      authString = this.userService.sessionkey();

    let headers = new HttpHeaders();
    headers = headers.set('Content-Type', 'application/json; charset=utf-8');
    headers = headers.set('auth', authString);
    headers = headers.set('resolutionid', resolutionid);
    headers = headers.set('paragraphid', paragraphid);
    let options = { headers: headers };
    this.httpClient.get(this.baseUrl + 'api/Resolution/MovePreambleParagraphUp',
      options).subscribe(data => { }, err => { this.notifyService.notify('error', 'Das hat nicht geklappt :('); });
  }

  public movePrembleParagraphDown(resolutionid: string, paragraphid: string) {
    let authString: string = 'default';
    if (this.userService.isLoggedIn)
      authString = this.userService.sessionkey();

    let headers = new HttpHeaders();
    headers = headers.set('Content-Type', 'application/json; charset=utf-8');
    headers = headers.set('auth', authString);
    headers = headers.set('resolutionid', resolutionid);
    headers = headers.set('paragraphid', paragraphid);
    let options = { headers: headers };
    this.httpClient.get(this.baseUrl + 'api/Resolution/MovePreambleParahraphDown',
      options).subscribe(data => { }, err => { this.notifyService.notify('error', 'Das hat nicht geklappt :('); });
  }

  public removePrembleParagraph(resolutionid: string, paragraphid: string) {
    let authString: string = 'default';
    if (this.userService.isLoggedIn)
      authString = this.userService.sessionkey();

    let headers = new HttpHeaders();
    headers = headers.set('Content-Type', 'application/json; charset=utf-8');
    headers = headers.set('auth', authString);
    headers = headers.set('resolutionid', resolutionid);
    headers = headers.set('paragraphid', paragraphid);
    let options = { headers: headers };
    this.httpClient.get(this.baseUrl + 'api/Resolution/RemovePreambleParagraph',
      options).subscribe(data => { }, err => { this.notifyService.notify('error', 'Das hat nicht geklappt :('); });
  }

  public getAdvancedInfos(resolutionid: string) {
    let authString: string = 'default';
    if (this.userService.isLoggedIn)
      authString = this.userService.sessionkey();

    let headers = new HttpHeaders();
    headers = headers.set('Content-Type', 'application/json; charset=utf-8');
    headers = headers.set('auth', authString);
    headers = headers.set('id', resolutionid);
    let options = { headers: headers };
    return this.httpClient.get<ResolutionAdvancedInfo>(this.baseUrl + 'api/Resolution/GetResolutionInfos',
      options);
  }

  public canIEditResolution(resolutionid: string) {
    let authString: string = 'default';
    if (this.userService.isLoggedIn)
      authString = this.userService.sessionkey();

    let headers = new HttpHeaders();
    headers = headers.set('Content-Type', 'application/json; charset=utf-8');
    headers = headers.set('auth', authString);
    headers = headers.set('id', resolutionid);
    let options = { headers: headers };
    return this.httpClient.get<boolean>(this.baseUrl + 'api/Resolution/CanAuthEditResolution',
      options);
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
      options).subscribe(data => { }, err => { this.notifyService.notify('error', 'Das hat nicht geklappt :('); });
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

        console.log('subscribte to: ' + id, ' with my id: ' + this.connectionid);
        let authString: string = 'default';
        if (this.userService.isLoggedIn)
          authString = this.userService.sessionkey();

        let headers = new HttpHeaders();
        headers = headers.set('content-type', 'application/json; charset=utf-8');
        headers = headers.set('auth', authString);
        headers = headers.set('id', id);
        headers = headers.set('connectionid', this.connectionid);
        let options = { headers: headers };
        this.httpClient.get(this.baseUrl + 'api/Resolution/SubscribeToResolution',
          options).subscribe(data => { }, err => { this.notifyService.notify('error', 'Das hat nicht geklappt :('); });
      })
      .catch(err => {
        this.hasError = true;
      });
    
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
      options).subscribe(data => { }, err => { this.notifyService.notify('error', 'Das hat nicht geklappt :('); });
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
      options).subscribe(data => { }, err => { this.notifyService.notify('error', 'Das hat nicht geklappt :('); });
  }

  public addMoveAmendment(resolutionid: string, paragraphid: string, submitter: string, newPosition: number) {
    let authString: string = 'default';
    if (this.userService.isLoggedIn)
      authString = this.userService.sessionkey();

    let headers = new HttpHeaders();
    headers = headers.set('content-type', 'application/json; charset=utf-8');
    headers = headers.set('auth', authString);
    headers = headers.set('resolutionid', resolutionid);
    headers = headers.set('sectionid', paragraphid);
    headers = headers.set('sumbittername', submitter);
    headers = headers.set('newposition', newPosition.toString());
    let options = { headers: headers };
    this.httpClient.get(this.baseUrl + 'api/Resolution/AddMoveAmendment',
      options).subscribe(data => { }, err => { this.notifyService.notify('error', 'Das hat nicht geklappt :('); });
  }

  public addAddAmendment(resolutionid: string, submitter: string, newPosition: number, newText: string) {
    let authString: string = 'default';
    if (this.userService.isLoggedIn)
      authString = this.userService.sessionkey();

    let headers = new HttpHeaders();
    headers = headers.set('content-type', 'application/json; charset=utf-8');
    headers = headers.set('auth', authString);
    headers = headers.set('resolutionid', resolutionid);
    headers = headers.set('sumbittername', submitter);
    headers = headers.set('newposition', newPosition.toString());
    headers = headers.set('newtext', encodeURI(newText + '|'));
    let options = { headers: headers };
    this.httpClient.get(this.baseUrl + 'api/Resolution/AddAddAmendment',
      options).subscribe(data => { }, err => { this.notifyService.notify('error', 'Das hat nicht geklappt :('); });
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
      options).subscribe(data => { }, err => { this.notifyService.notify('error', 'Das hat nicht geklappt :('); });
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
      options).subscribe(data => { }, err => { this.notifyService.notify('error', 'Das hat nicht geklappt :('); });
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
      options).subscribe(data => { }, err => { this.notifyService.notify('error', 'Das hat nicht geklappt :('); });
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
      options).subscribe(data => { }, err => { this.notifyService.notify('error', 'Das hat nicht geklappt :('); });
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

      
    });
    //Add Amendments
    resolution.AddAmendmentsSave.forEach(n => arr.push(n))
    return arr;
  }

  
}

export class stackElement {
  methodName: string;
  args: any;
}
