import { Injectable, Inject } from '@angular/core';
import * as signalR from "@aspnet/signalr";
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { UserService } from './user.service';

@Injectable({
  providedIn: 'root'
})
export class ResolutionService {
  private _hubConnection: signalR.HubConnection;

  private baseUrl: string;

  public resolution: Resolution;

  public connectionReady: boolean = false;

  public stack: stackElement[] = [];

  constructor(private httpClient: HttpClient, private userService: UserService, @Inject('BASE_URL') baseUrl: string) {
    this.baseUrl = baseUrl;
    this._hubConnection = new signalR.HubConnectionBuilder().withUrl(baseUrl + 'resasocket').build();
    this._hubConnection
      .start()
      .then(() => {
        console.log('Connection started!');
        this.connectionReady = true;
        this.stack.forEach(n => {
          console.log('work: "' + n.methodName + '" from stack Args: "' + n.args + '"');
          this._hubConnection.send(n.methodName, n.args);
        });
        this.stack = [];
      })
      .catch(err => console.log('Error while establishing connection :('));
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

  //SignalR Part
  public addResolutionListener = (model: Resolution) => {
    this._hubConnection.on('PreambleParagraphAdded', (position: number, id: string, text: string) => {
      let paragraph: PreambleParagraph = new PreambleParagraph();
      paragraph.ID = id;
      paragraph.Text = text;
      model.Preamble.Paragraphs.push(paragraph);
      //this.resolution.OperativeSections.filter(n => n.ID == id)[0].Text = text;
    });

    this._hubConnection.on('PreambleParagraphChanged', (id: string, newtext: string) => {

      let target = model.Preamble.Paragraphs.filter(n => n.ID == id)[0];
      if (target != null && target.Text != newtext) {
        target.Text = newtext;
      }
    });

    this._hubConnection.on('ResolutionSaved', (date: Date) => {
      console.log('Resolution has been saved!' + date);
      model.lastSaved = date;
    })
  }

  public addPreambleParagraph(resolutionid: string) {
    let authString: string = 'default';
    if (this.userService.isLoggedIn)
      authString = this.userService.sessionkey();
    let headers = new HttpHeaders();
    headers = headers.set('auth', authString);
    headers = headers.set('resolutionid', resolutionid);
    let options = { headers: headers };
    this.httpClient.get(this.baseUrl + 'api/Resolution/AddPreambleParagraph', options).subscribe(data => { });
  }

  public changePreambleParagraph(resolutionid: string, paragraphid: string, newtext: string) {
    let authString: string = 'default';
    if (this.userService.isLoggedIn)
      authString = this.userService.sessionkey();

    let headers = new HttpHeaders();
    headers = headers.set('content-type', 'application/json; charset=utf-8');
    headers = headers.set('auth', authString);
    headers = headers.set('resolutionid', resolutionid);
    headers = headers.set('paragraphid', paragraphid);
    headers = headers.set('newtext', newtext + '|');
    console.log('NEUERTEXT:' + newtext + ':TEXTEND');
    let options = { headers: headers };
    this.httpClient.get(this.baseUrl + 'api/Resolution/ChangePreambleParagraph',
      options).subscribe(data => console.log(data));
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

  public deleteOperativeParagraph(resolutionid: string, paragraphid: string) {

  }

}

export class stackElement {
  methodName: string;
  args: any;
}

export class Resolution {
  ID: string;
  OperativeSections: OperativeSection[];
  Preamble: Preamble;
  lastSaved: Date;
}

export class OperativeSection {
  ID: string;
  Text: string;
}

export class Preamble {
  Paragraphs: PreambleParagraph[];
}

export class PreambleParagraph {
  Text: string;
  ID: string;
}
