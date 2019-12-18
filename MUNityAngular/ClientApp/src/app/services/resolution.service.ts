import { Injectable, Inject } from '@angular/core';
import * as signalR from "@aspnet/signalr";
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ResolutionService {
  private _hubConnection: signalR.HubConnection;

  private baseUrl: string;

  public resolution: Resolution;

  public connectionReady: boolean = false;

  public stack: stackElement[] = [];

  constructor(private httpClient: HttpClient, @Inject('BASE_URL') baseUrl: string) {
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
    return this.httpClient.get<Resolution>(this.baseUrl + 'api/Resolution/Create?auth=default');
  }


  public getResolution(id: string) {
    return this.httpClient.get<Resolution>(this.baseUrl + 'api/Resolution/Get?auth=default&id=' + id);
  }

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
        console.log('Update target text!')
        target.Text = newtext;
      }
      
    });
  }

  public addPreambleParagraph(resolutionid: string) {
    console.log('Requesting new PreambleParagraphFromServer: ')
    this.httpClient.get(this.baseUrl + 'api/Resolution/AddPreambleParagraph?auth=default&resolutionid=' + resolutionid).subscribe(data => { });
  }

  public changePreambleParagraph(resolutionid: string, paragraphid: string, newtext: string) {
    console.log('Want to change');
    let headers = new HttpHeaders();
    headers = headers.set('newtext', newtext);
    let options = { headers: headers };
    this.httpClient.get(this.baseUrl + 'api/Resolution/ChangePreambleParagraph?auth=default&resolutionid=' + resolutionid + '&paragraphid=' + paragraphid,
      options).subscribe(data => console.log(data));
  }

  public subscribeToResolution(id: string) {
    if (this.connectionReady == true)
      this._hubConnection.send('SubscribeToResolution', id);
    else {
      console.log('going to push the subscribtion to stack!')
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
