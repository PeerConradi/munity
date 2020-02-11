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
    headers = headers.set('auth', this.userService.getAuthOrDefault());
    const options = { headers: headers };
    return this.http.get<Conference[]>(this.baseUrl + 'api/conference/GetConferences', options);
  }

  public createConference(conference: Conference): Observable<Conference> {
    let headers = new HttpHeaders();
    headers = headers.set('auth', this.userService.getAuthOrDefault())
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
    headers = headers.set('auth', this.userService.getAuthOrDefault())
    headers = headers.set('id', id);
    let options = { headers: headers };
    return this.http.get<Conference>(this.baseUrl + 'api/Conference/GetConference',
      options);
  }

  public addCommittee(conferenceid: string, name: string, fullname: string,
    abbreviation: string, article: string, resolutlycommittee: string) {
    let headers = new HttpHeaders();

    headers = headers.set('auth', this.userService.getAuthOrDefault());
    headers = headers.set('conferenceid', conferenceid);
    headers = headers.set('name', name);
    headers = headers.set('fullname', fullname);
    headers = headers.set('abbreviation', abbreviation);
    headers = headers.set('article', article);
    headers = headers.set('resolutlycommittee', resolutlycommittee);
    let options = { headers: headers };
    return this.http.get<Committee>(this.baseUrl + 'api/Conference/AddCommittee',
      options);
  }

  public createDelegation(name: string, fullname: string,
    abbreviation: string) {
    let headers = new HttpHeaders();

    headers = headers.set('auth', this.userService.getAuthOrDefault());
    headers = headers.set('name', encodeURI(name + '|'));
    headers = headers.set('fullname', encodeURI(fullname + '|'));
    headers = headers.set('abbreviation', encodeURI(abbreviation + '|'));
    let options = { headers: headers };
    return this.http.get<Delegation>(this.baseUrl + 'api/Conference/CreateDelegation',
      options);
  }

  public addDelegationToConference(conferenceid: string, delegationid: string, mincount: number, maxcount: number) {
    let headers = new HttpHeaders();

    headers = headers.set('auth', this.userService.getAuthOrDefault());
    headers = headers.set('conferenceid', conferenceid);
    headers = headers.set('delegationid', delegationid);
    headers = headers.set('mincount', mincount.toString());
    headers = headers.set('maxcount', maxcount.toString());
    let options = { headers: headers };
    return this.http.get<Delegation>(this.baseUrl + 'api/Conference/AddDelegationToConference',
      options);
  }

  public changeConferenceName(conferenceid: string, newname: string) {
    let headers = new HttpHeaders();
    headers = headers.set('auth', this.userService.getAuthOrDefault());
    headers = headers.set('conferenceid', conferenceid);
    headers = headers.set('newname', newname);
    let options = { headers: headers };
    return this.http.get<Conference>(this.baseUrl + 'api/Conference/ChangeConferenceName',
      options);
  }

  public getAllDelegations() {
    return this.http.get<Delegation[]>(this.baseUrl + 'api/Conference/AllDelegations');
  }

  public getDelegationsOfCommittee(committeeid: string) {
    let headers = new HttpHeaders();
    headers = headers.set('auth', this.userService.getAuthOrDefault());
    headers = headers.set('committeeid', committeeid);
    let options = { headers: headers };
    return this.http.get<Delegation[]>(this.baseUrl + 'api/Conference/GetDelegationsOfCommittee',
      options);
  }

  public addDelegationToCommittee(committeeid: string, delegationid: string, mincount: number, maxcount: number) {
    let headers = new HttpHeaders();
    headers = headers.set('auth', this.userService.getAuthOrDefault());
    headers = headers.set('committeeid', committeeid);
    headers = headers.set('delegationid', delegationid);
    headers = headers.set('mincount', mincount.toString());
    headers = headers.set('maxcount', maxcount.toString());
    let options = { headers: headers };
    return this.http.get<Committee>(this.baseUrl + 'api/Conference/AddDelegationToCommittee',
      options);
  }
}
