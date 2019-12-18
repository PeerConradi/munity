import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
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


}

interface Conference {
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

interface Committee {
  ID: string;
  Abbreviation: string;
  Article: string;
  ConferenceID: string;
  DelegationList: string[];
  FullName: string;
  Name: string;
}

interface Delegation {
  Abbreviation: string;
  CountryId: string;
  ID: string;
  ISO: string;
  Name: string;
  TypeName: string;
}
