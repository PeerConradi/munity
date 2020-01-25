import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Component, Inject } from '@angular/core';
import { Observable } from 'rxjs';
import { Conference } from '../models/conference.model';
import { UserService } from './user.service';
import { Committee } from '../models/committee.model';

@Injectable({
  providedIn: 'root'
})
export class ConferenceServiceService {

  private baseUrl: string;
  constructor(private http: HttpClient, private userService: UserService, @Inject('BASE_URL') baseUrl: string) {
    this.baseUrl = baseUrl;
  }

  public getAllConferences(): Observable<Conference[]> {
    let headers = new HttpHeaders();
    headers = headers.set('auth', this.userService.sessionkey())
    const options = { headers: headers };
    return this.http.get<Conference[]>(this.baseUrl + 'api/conference/GetConferences', options);
  }

  public createConference(conference: Conference, password: string): Observable<Conference> {
    let headers = new HttpHeaders();
    headers = headers.set('auth', this.userService.sessionkey())
    headers = headers.set('Name', conference.name);
    headers = headers.set('FullName', conference.fullName);
    headers = headers.set('Abbreviation', conference.abbreviation);
    headers = headers.set('StartDate', conference.startDate.toUTCString());
    headers = headers.set('EndDate', conference.endDate.toUTCString());
    headers = headers.set('Password', password);
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
}
