import { Injectable, Inject } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { UserService } from './user.service';
import { Speakerlist } from '../models/speakerlist.model';
import * as signalR from '@aspnet/signalr';
import { TimeSpan } from '../models/TimeSpan';
import { Delegation } from '../models/delegation.model';

@Injectable({
  providedIn: 'root'
})
export class SpeakerListService {

  private baseUrl: string;

  public hasError: boolean = false;

  private connectionid: string;

  public currentList: Speakerlist;

  private _hubConnection: signalR.HubConnection;

  constructor(private http: HttpClient, @Inject('BASE_URL') baseUrl: string, private userService: UserService) {
    this.baseUrl = baseUrl;
  }

  public createSpeakerlist(conferenceid: string, committeeid: string) {
    let headers = new HttpHeaders();
    headers = headers.set('conferenceid', conferenceid);
    headers = headers.set('committeeid', committeeid);
    let options = { headers: headers };
    return this.http.get<Speakerlist>(this.baseUrl + 'api/Speakerlist/CreateSpeakerlist',
      options);
  }

  public getSpeakerlistById(id: string) {
    let headers = new HttpHeaders();
    headers = headers.set('id', id);
    let options = { headers: headers };
    return this.http.get<Speakerlist>(this.baseUrl + 'api/Speakerlist/GetSpeakerlist');
  }

  public getSpeakerlistByPublicId(id: string) {
    return this.http.get<Speakerlist>(this.baseUrl + 'api/Speakerlist/ReadSpeakerlist');
  }

  public setSpeakertime(listid: string, time: string) {
    let headers = new HttpHeaders();
    headers = headers.set('listid', listid);
    headers = headers.set('time', time);
    let options = { headers: headers };
    return this.http.patch(this.baseUrl + 'api/Speakerlist/SetSpeakertime', null,
      options);
  }

  public setQuestionTime(listid: string, time: string) {
    let headers = new HttpHeaders();
    headers = headers.set('listid', listid);
    headers = headers.set('time', time);
    let options = { headers: headers };
    return this.http.patch(this.baseUrl + 'api/Speakerlist/SetQuestiontime', null,
      options);
  }

  public addSpeaker(listid: string, delegationid: string) {
    let headers = new HttpHeaders();
    headers = headers.set('listid', listid);
    headers = headers.set('delegationid', delegationid);
    let options = { headers: headers };
    return this.http.post<Speakerlist>(this.baseUrl + 'api/Speakerlist/AddSpeakerToList', null,
      options);
  }

  public addSpeakerModel(listid: string, model: Delegation) {
    let headers = new HttpHeaders();
    headers = headers.set('listid', listid);
    let options = { headers: headers };
    return this.http.post<Speakerlist>(this.baseUrl + 'api/Speakerlist/AddSpeakerModelToList', model,
      options);
  }

  public addQuestionModel(listid: string, model: Delegation) {
    let headers = new HttpHeaders();
    headers = headers.set('listid', listid);
    let options = { headers: headers };
    return this.http.post<Speakerlist>(this.baseUrl + 'api/Speakerlist/AddQuestionModelToList', model,
      options);
  }

  public addQuestion(listid: string, delegationid: string) {
    let headers = new HttpHeaders();
    headers = headers.set('listid', listid);
    headers = headers.set('delegationid', delegationid);
    let options = { headers: headers };
    return this.http.post<Speakerlist>(this.baseUrl + 'api/Speakerlist/AddQuestionToList', null,
      options);
  }

  public nextSpeaker(listid: string) {
    let headers = new HttpHeaders();
    headers = headers.set('listid', listid);
    let options = { headers: headers };
    return this.http.post(this.baseUrl + 'api/Speakerlist/NextSpeaker', null,
      options);
  }

  public nextQuestion(listid: string) {
    let headers = new HttpHeaders();
    headers = headers.set('listid', listid);
    let options = { headers: headers };
    return this.http.post(this.baseUrl + 'api/Speakerlist/NextQuestion', null,
      options);
  }

  public startSpeaker(listid: string) {
    let headers = new HttpHeaders();
    headers = headers.set('listid', listid);
    let options = { headers: headers };
    return this.http.post(this.baseUrl + 'api/Speakerlist/StartSpeaker', null,
      options);
  }

  public clearSpeaker(listid: string) {
    let headers = new HttpHeaders();
    headers = headers.set('listid', listid);
    let options = { headers: headers };
    return this.http.put(this.baseUrl + 'api/Speakerlist/ClearSpeaker', null,
      options);
  }

  public clearQuestion(listid: string) {
    let headers = new HttpHeaders();
    headers = headers.set('listid', listid);
    let options = { headers: headers };
    return this.http.put(this.baseUrl + 'api/Speakerlist/ClearQuestion', null,
      options);
  }

  public startAnswer(listid: string) {
    let headers = new HttpHeaders();
    headers = headers.set('listid', listid);
    let options = { headers: headers };
    return this.http.post(this.baseUrl + 'api/Speakerlist/StartAnswer', null,
      options);
  }

  public startQuestion(listid: string) {
    let headers = new HttpHeaders();
    headers = headers.set('listid', listid);
    let options = { headers: headers };
    return this.http.post(this.baseUrl + 'api/Speakerlist/StartQuestion', null,
      options);
  }

  public stopTimer(listid: string) {
    let headers = new HttpHeaders();
    headers = headers.set('listid', listid);
    let options = { headers: headers };
    return this.http.post(this.baseUrl + 'api/Speakerlist/PauseTimer', null,
      options);
  }

  public removeSpeaker(listid: string, delegationid: string) {
    let headers = new HttpHeaders();
    headers = headers.set('listid', listid);
    headers = headers.set('delegationid', delegationid);
    let options = { headers: headers };
    return this.http.delete(this.baseUrl + 'api/Speakerlist/RemoveSpeakerFromList',
      options);
  }

  public reorderSpeaker(listid: string, items: Delegation[]) {
    let headers = new HttpHeaders();
    headers = headers.set('listid', listid);
    let options = { headers: headers };
    return this.http.patch(this.baseUrl + 'api/Speakerlist/SpeakersOrderChanged', items,
      options);
  }

  public reorderQuestion(listid: string, items: Delegation[]) {
    let headers = new HttpHeaders();
    headers = headers.set('listid', listid);
    let options = { headers: headers };
    return this.http.patch(this.baseUrl + 'api/Speakerlist/QuestionsOrderChanged', items,
      options);
  }

  public subscribeToSpeakerlist(publicid: string) {
    const builder = new signalR.HubConnectionBuilder().withUrl(this.baseUrl + 'slsocket');
    this._hubConnection = builder.build();
    this._hubConnection.serverTimeoutInMilliseconds = 9600000;
    this._hubConnection
      .start()
      .then(() => {
        var hub = this._hubConnection;

        var connectionUrl: string = hub["connection"].transport.webSocket.url;
        this.connectionid = connectionUrl.split('=')[1];

        let headers = new HttpHeaders();
        headers = headers.set('content-type', 'application/json; charset=utf-8');
        headers = headers.set('publicid', publicid);
        headers = headers.set('connectionid', this.connectionid);
        let options = { headers: headers };
        this.http.post(this.baseUrl + 'api/Speakerlist/SubscribeToList', null,
          options).subscribe(data => {  }, err => { this.hasError = true; console.log(err); });
      })
      .catch(err => {
        this.hasError = true;
      });
  }

  public timeSpanFromString(timespan: string): TimeSpan {
    const ts = new TimeSpan(0, 0, 0, 0, 0);
    ts.fromString(timespan);
    return ts;
  }

  public addSpeakerlistListener = (model: Speakerlist) => {
    this._hubConnection.on('SpeakerListChanged', (speakerlist: Speakerlist) => {
      model.Speakers = speakerlist.Speakers;
      model.Questions = speakerlist.Questions;
      model.ListClosed = speakerlist.ListClosed;
      model.QuestionsClosed = speakerlist.QuestionsClosed;
      model.CurrentSpeaker = speakerlist.CurrentSpeaker;
      model.CurrentQuestion = speakerlist.CurrentQuestion;
      model.Status = speakerlist.Status;

      //Sync the times in a better way!
      model.RemainingSpeakerTime.reset();
      model.RemainingSpeakerTime.addSeconds(speakerlist.RemainingSpeakerTime.TotalSeconds);

      model.RemainingQuestionTime.reset();
      model.RemainingQuestionTime.addSeconds(speakerlist.RemainingQuestionTime.TotalSeconds);
    });
    this._hubConnection.on('SpeakerTimerStarted', (secs: number) => {
      model.RemainingSpeakerTime = new TimeSpan(0, 0, 0, 0, 0);
      model.RemainingSpeakerTime.addSeconds(secs);
      model.Status = 1;
    });
    this._hubConnection.on('QuestionTimerStarted', (secs: number) => {
      model.RemainingQuestionTime = new TimeSpan(0, 0, 0, 0, 0);
      model.RemainingQuestionTime.addSeconds(secs);
      model.Status = 2;
    });
    this._hubConnection.on('TimerStopped', () => {
      model.Status = 0;
    });
    this._hubConnection.on('SpeakerTimerSynced', (secs: number) => {
      model.RemainingSpeakerTime = new TimeSpan(0, 0, 0, 0, 0);
      model.RemainingSpeakerTime.addSeconds(secs);
    });
  };
}
