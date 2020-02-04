import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Component, Inject } from '@angular/core';
import { Observable } from 'rxjs';
import { Conference } from '../models/conference.model';
import { UserService } from './user.service';
import { Committee } from '../models/committee.model';
import { Delegation } from '../models/delegation.model';

@Injectable({
  providedIn: 'root'
})
export class ConferenceServiceService {

  hasError: boolean = false;

  private baseUrl: string;
  constructor(private http: HttpClient, private userService: UserService, @Inject('BASE_URL') baseUrl: string) {
    this.baseUrl = baseUrl;
  }

  public getAllConferences(): Observable<Conference[]> {
    let headers = new HttpHeaders();
    let auth = 'default';
    if (this.userService.isLoggedIn)
      auth = this.userService.sessionkey();
    headers = headers.set('auth', auth);

    const options = { headers: headers };
    return this.http.get<Conference[]>(this.baseUrl + 'api/conference/GetConferences', options);
  }

  public createConference(conference: Conference): Observable<Conference> {
    let headers = new HttpHeaders();
    headers = headers.set('auth', this.userService.sessionkey())
    headers = headers.set('Name', conference.Name);
    headers = headers.set('FullName', conference.FullName);
    headers = headers.set('Abbreviation', conference.Abbreviation);
    headers = headers.set('StartDate', conference.StartDate.toUTCString());
    headers = headers.set('EndDate', conference.EndDate.toUTCString());
    let options = { headers: headers };
    return this.http.get<Conference>(this.baseUrl + 'api/Conference/Create',
      options);
  }

  public getConference(id: string) {
    let headers = new HttpHeaders();
    let authcode = 'default';
    if (this.userService.isLoggedIn)
      authcode = this.userService.sessionkey();
    headers = headers.set('auth', authcode)
    headers = headers.set('id', id);
    let options = { headers: headers };
    return this.http.get<Conference>(this.baseUrl + 'api/Conference/GetConference',
      options);
  }

  public addCommittee(conferenceid: string, name: string, fullname: string,
    abbreviation: string, article: string, resolutlycommittee: string) {
    let authcode = 'default';
    if (this.userService.isLoggedIn)
      authcode = this.userService.sessionkey();

    let headers = new HttpHeaders();

    headers = headers.set('auth', authcode);
    headers = headers.set('conferenceid', conferenceid);
    headers = headers.set('name', name);
    headers = headers.set('fullname', fullname);
    headers = headers.set('abbreviation', abbreviation);
    headers = headers.set('article', article);
    headers = headers.set('resolutlycommittee', resolutlycommittee);
    console.log(headers);
    let options = { headers: headers };
    return this.http.get<Committee>(this.baseUrl + 'api/Conference/AddCommittee',
      options);
  }

  public addDelegation(conferenceid: string, name: string, fullname: string,
    abbreviation: string, mincount: number, maxcount: number) {
    let authcode = 'default';
    if (this.userService.isLoggedIn)
      authcode = this.userService.sessionkey();

    let headers = new HttpHeaders();

    headers = headers.set('auth', authcode);
    headers = headers.set('conferenceid', conferenceid);
    headers = headers.set('name', encodeURI(name + '|'));
    headers = headers.set('fullname', encodeURI(fullname + '|'));
    headers = headers.set('abbreviation', encodeURI(abbreviation + '|'));
    headers = headers.set('mincount', mincount.toString());
    headers = headers.set('maxcount', maxcount.toString());
    console.log(headers);
    let options = { headers: headers };
    return this.http.get<Committee>(this.baseUrl + 'api/Conference/AddDelegation',
      options);
  }

  public changeConferenceName(conferenceid: string, password: string, newname: string) {
    let headers = new HttpHeaders();
    headers = headers.set('auth', 'default');
    headers = headers.set('conferenceid', conferenceid);
    headers = headers.set('password', password);
    headers = headers.set('newname', newname);
    let options = { headers: headers };
    return this.http.get<Conference>(this.baseUrl + 'api/Conference/ChangeConferenceName',
      options);
  }

  public getAllDelegations() {
    return this.http.get<Delegation[]>(this.baseUrl + '/api/Conference/AllDelegations');
  }
}
