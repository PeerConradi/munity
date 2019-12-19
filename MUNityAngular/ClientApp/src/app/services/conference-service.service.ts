import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Component, Inject } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ConferenceServiceService {

  private baseUrl: string;
  constructor(private http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    this.baseUrl = baseUrl;
  }

  public getAllConferences() : Observable<Conference[]> {
    return this.http.get<Conference[]>(this.baseUrl + 'api/conference/GetConferences');
  }

  public createConference(conference: Conference, password: string): Observable<Conference> {
    let headers = new HttpHeaders();
    headers = headers.set('Name', conference.Name);
    headers = headers.set('FullName', conference.FullName);
    headers = headers.set('Abbreviation', conference.Abbreviation);
    headers = headers.set('StartDate', conference.StartDate.toUTCString());
    headers = headers.set('EndDate', conference.EndDate.toUTCString());
    headers = headers.set('Password', password);
    let options = { headers: headers };
    return this.http.get<Conference>(this.baseUrl + 'api/Conference/Create?auth=default',
      options);
  }
}

export class Conference {
  ID: string;
  Name: string;
  FullName: string;
  Abbreviation: string;
  Committees: Committee[];
  CreationDate: Date;
  StartDate: Date;
  EndDate: Date;
  SecretaryGeneralTitle: string;
  SecretaryGeneralName: string;
}

export class Committee {
  ID: string;
  Abbreviation: string;
  Article: string;
  ConferenceID: string;
  DelegationList: string[];
  FullName: string;
  Name: string;
}

export class Delegation {
  Abbreviation: string;
  CountryId: string;
  ID: string;
  ISO: string;
  Name: string;
  TypeName: string;
}
