import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Component, Inject } from '@angular/core';
import { Observable } from 'rxjs';
import { Conference } from '../models/conference.model';
import { UserService } from './user.service';

@Injectable({
  providedIn: 'root'
})
export class ConferenceServiceService {

  private baseUrl: string;
  constructor(private http: HttpClient, private userSerice: UserService, @Inject('BASE_URL') baseUrl: string) {
    this.baseUrl = baseUrl;
  }

  public getAllConferences(): Observable<Conference[]> {
    let headers = new HttpHeaders();
    headers = headers.set('auth', this.userSerice.sessionkey())
    const options = { headers: headers };
    return this.http.get<Conference[]>(this.baseUrl + 'api/conference/GetConferences', options);
  }

  public createConference(conference: Conference, password: string): Observable<Conference> {
    let headers = new HttpHeaders();
    headers = headers.set('auth', this.userSerice.sessionkey())
    headers = headers.set('Name', conference.Name);
    headers = headers.set('FullName', conference.FullName);
    headers = headers.set('Abbreviation', conference.Abbreviation);
    headers = headers.set('StartDate', conference.StartDate.toUTCString());
    headers = headers.set('EndDate', conference.EndDate.toUTCString());
    headers = headers.set('Password', password);
    let options = { headers: headers };
    return this.http.get<Conference>(this.baseUrl + 'api/Conference/Create',
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
