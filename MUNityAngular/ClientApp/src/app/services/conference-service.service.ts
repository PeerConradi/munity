import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Component, Inject } from '@angular/core';
import { Observable } from 'rxjs';
import { Conference } from '../models/conference/conference.model';
import { UserService } from './user.service';
import { Committee } from '../models/conference/committee.model';
import { Delegation } from '../models/conference/delegation.model';
import { AddCommitteeRequest } from '../models/requests/add-committee-request';
import { CreateDelegationRequest } from '../models/requests/create-delegation-request';
import { ChangeConferenceNameRequest } from '../models/requests/change-conference-name-request';
import { User } from '../models/user.model';
import { TeamRole } from '../models/team-role.model';
import { UserConferenceRole } from '../models/user-conference-role.model';
import { CommitteeStatus } from '../models/conference/committee-status.model';


// Some day this thing should be renamed into ConferenceService!
@Injectable({
  providedIn: 'root'
})
export class ConferenceService {

  hasError: boolean = false;

  public committeeContext: Committee = null;

  public currentConference: Conference = null;

  private baseUrl: string;
  constructor(private http: HttpClient, private userService: UserService, @Inject('BASE_URL') baseUrl: string) {
    this.baseUrl = baseUrl;
  }

  public getAllConferences(): Observable<Conference[]> {
    return this.http.get<Conference[]>(this.baseUrl + 'api/conference/GetConferences');
  }

  public createConference(conference: Conference): Observable<Conference> {
    return this.http.post<Conference>(this.baseUrl + 'api/Conference/Create', conference);
  }

  public getConference(id: string) {
    let headers = new HttpHeaders();
    headers = headers.set('id', id);
    let options = { headers: headers };
    return this.http.get<Conference>(this.baseUrl + 'api/Conference/GetConference',
      options);
  }

  public addCommittee(conferenceid: string, committee: Committee) {

  }

  public createDelegation(delegation: Delegation) {

  }

  public getTypeNumberByName(name: string): number {
    if (name == 'COUNTRY') return 0;

    return -1;
  }

  public addDelegationToConference(conferenceid: string, delegationid: string, mincount: number, maxcount: number) {

  }

  public changeConferenceName(conferenceid: string, newname: string) {

  }

  public getAllDelegations() {
    return this.http.get<Delegation[]>(this.baseUrl + 'api/Conference/AllDelegations');
  }

  public getDelegationsOfCommittee(committeeid: string) {
    let headers = new HttpHeaders();
    headers = headers.set('committeeid', committeeid);
    let options = { headers: headers };
    return this.http.get<Delegation[]>(this.baseUrl + 'api/Conference/GetDelegationsOfCommittee',
      options);
  }

  public getTeam(conferenceid: string) {
    let headers = new HttpHeaders();
    headers = headers.set('conferenceid', conferenceid);
    let options = { headers: headers };
    return this.http.get<UserConferenceRole[]>(this.baseUrl + 'api/Conference/GetTeam',
      options);
  }

  public getTeamRoles(conferenceid: string) {
    let headers = new HttpHeaders();
    headers = headers.set('conferenceid', conferenceid);
    let options = { headers: headers };
    return this.http.get<TeamRole[]>(this.baseUrl + 'api/Conference/GetTeamRoles',
      options);
  }

  public addTeamRole(role: TeamRole) {
    return this.http.put(this.baseUrl + 'api/Conference/AddTeamRole', role);
  }

  public addTeamMember(username: string, role: TeamRole) {
    return this.http.put(this.baseUrl + 'api/Conference/AddUserToTeam', { Username: username, Role: role });
  }

  public addDelegationToCommittee(committeeid: string, delegationid: string, mincount: number, maxcount: number) {
    let headers = new HttpHeaders();
    headers = headers.set('committeeid', committeeid);
    headers = headers.set('delegationid', delegationid);
    headers = headers.set('mincount', mincount.toString());
    headers = headers.set('maxcount', maxcount.toString());
    let options = { headers: headers };
    return this.http.post<Committee>(this.baseUrl + 'api/Conference/AddDelegationToCommittee', null,
      options);
  }

  public getCommitteesOfConferece(conferenceid: string) {
    let headers = new HttpHeaders();
    headers = headers.set('conferenceid', conferenceid);
    return this.http.get<Committee[]>(this.baseUrl + 'api/Conference/GetCommitteesOfConference');
  }

  public getCommittee(committeeid: string) {
    let headers = new HttpHeaders();
    headers = headers.set('committeeid', committeeid);
    return this.http.get<Committee>(this.baseUrl + 'api/Conference/GetCommittee');
  }

  public setCommitteeStatus(status: CommitteeStatus) {
    return this.http.put(this.baseUrl + 'api/Conference/SetCommitteeStatus', status);
  }

  public getCommitteeStatus(committeeid: string) {
    let headers = new HttpHeaders();
    headers = headers.set('committeeid', committeeid);
    return this.http.get<CommitteeStatus>(this.baseUrl + 'api/Conference/GetCommitteeStatus', { headers: headers });
  }

  public getTestConference(): Conference {
    let conference = new Conference();
    conference.name = 'Test Conference';
    conference.fullName = 'Conference to Test Everything';
    conference.conferenceId = 'test';
    conference.committees = [];
    conference.roles = [];
    return conference;
  }
}
