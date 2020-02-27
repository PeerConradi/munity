import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Component, Inject } from '@angular/core';
import { Observable } from 'rxjs';
import { Conference } from '../models/conference.model';
import { UserService } from './user.service';
import { Committee } from '../models/committee.model';
import { Delegation } from '../models/delegation.model';
import { AddCommitteeRequest } from '../models/requests/add-committee-request';
import { CreateDelegationRequest } from '../models/requests/create-delegation-request';
import { ChangeConferenceNameRequest } from '../models/requests/change-conference-name-request';
import { User } from '../models/user.model';
import { TeamRole } from '../models/team-role.model';
import { UserConferenceRole } from '../models/user-conference-role.model';
import { CommitteeStatus } from '../models/committee-status.model';


// Some day this thing should be renamed into ConferenceService!
@Injectable({
  providedIn: 'root'
})
export class ConferenceServiceService {

  hasError: boolean = false;

  public committeeContext: Committee = null;

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
    return this.http.post<Conference>(this.baseUrl + 'api/Conference/Create', conference, { headers: headers });
  }

  public getConference(id: string) {
    let headers = new HttpHeaders();
    headers = headers.set('auth', this.userService.getAuthOrDefault())
    headers = headers.set('id', id);
    let options = { headers: headers };
    return this.http.get<Conference>(this.baseUrl + 'api/Conference/GetConference',
      options);
  }

  public addCommittee(conferenceid: string, committee: Committee) {
    let headers = new HttpHeaders();

    headers = headers.set('auth', this.userService.getAuthOrDefault());
    const request = new AddCommitteeRequest();
    request.name = committee.Name;
    request.fullName = committee.FullName;
    request.abbreviation = committee.Abbreviation;
    request.article = committee.Article;
    request.conferenceId = conferenceid;
    request.resolutlyCommittee = committee.ResolutlyCommittee;
    return this.http.post<Committee>(this.baseUrl + 'api/Conference/AddCommittee', request, { headers: headers });
  }

  public createDelegation(delegation: Delegation) {
    let headers = new HttpHeaders();

    headers = headers.set('auth', this.userService.getAuthOrDefault());
    const request = new CreateDelegationRequest();
    request.name = delegation.Name;
    request.fullName = delegation.FullName;
    request.abbreviation = delegation.Abbreviation;
    request.type = this.getTypeNumberByName(delegation.TypeName);
    return this.http.post<Delegation>(this.baseUrl + 'api/Conference/CreateDelegation', request, { headers: headers });
  }

  public getTypeNumberByName(name: string): number {
    if (name == 'COUNTRY') return 0;

    return -1;
  }

  public addDelegationToConference(conferenceid: string, delegationid: string, mincount: number, maxcount: number) {
    let headers = new HttpHeaders();

    headers = headers.set('auth', this.userService.getAuthOrDefault());
    headers = headers.set('conferenceid', conferenceid);
    headers = headers.set('delegationid', delegationid);
    headers = headers.set('mincount', mincount.toString());
    headers = headers.set('maxcount', maxcount.toString());
    let options = { headers: headers };
    return this.http.post<Delegation>(this.baseUrl + 'api/Conference/AddDelegationToConference', null,
      options);
  }

  public changeConferenceName(conferenceid: string, newname: string) {
    let headers = new HttpHeaders();
    headers = headers.set('auth', this.userService.getAuthOrDefault());
    const request = new ChangeConferenceNameRequest();
    request.conferenceID = conferenceid;
    request.newName = newname;
    return this.http.patch<Conference>(this.baseUrl + 'api/Conference/ChangeConferenceName', request, { headers: headers });
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

  public getTeam(conferenceid: string) {
    let headers = new HttpHeaders();
    headers = headers.set('auth', this.userService.getAuthOrDefault());
    headers = headers.set('conferenceid', conferenceid);
    let options = { headers: headers };
    return this.http.get<UserConferenceRole[]>(this.baseUrl + 'api/Conference/GetTeam',
      options);
  }

  public getTeamRoles(conferenceid: string) {
    let headers = new HttpHeaders();
    headers = headers.set('auth', this.userService.getAuthOrDefault());
    headers = headers.set('conferenceid', conferenceid);
    let options = { headers: headers };
    return this.http.get<TeamRole[]>(this.baseUrl + 'api/Conference/GetTeamRoles',
      options);
  }

  public addTeamRole(role: TeamRole) {
    let headers = new HttpHeaders();
    headers = headers.set('auth', this.userService.getAuthOrDefault());
    return this.http.put(this.baseUrl + 'api/Conference/AddTeamRole', role, { headers: headers });
  }

  public addTeamMember(username: string, role: TeamRole) {
    let headers = new HttpHeaders();
    headers = headers.set('auth', this.userService.getAuthOrDefault());
    return this.http.put(this.baseUrl + 'api/Conference/AddUserToTeam', {Username: username, Role: role}, { headers: headers });
  }

  public addDelegationToCommittee(committeeid: string, delegationid: string, mincount: number, maxcount: number) {
    let headers = new HttpHeaders();
    headers = headers.set('auth', this.userService.getAuthOrDefault());
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
    headers = headers.set('auth', this.userService.getAuthOrDefault());
    headers = headers.set('conferenceid', conferenceid);
    return this.http.get<Committee[]>(this.baseUrl + 'api/Conference/GetCommitteesOfConference', { headers: headers });
  }

  public getCommittee(committeeid: string) {
    let headers = new HttpHeaders();
    headers = headers.set('auth', this.userService.getAuthOrDefault());
    headers = headers.set('committeeid', committeeid);
    return this.http.get<Committee>(this.baseUrl + 'api/Conference/GetCommittee', { headers: headers });
  }

  public setCommitteeStatus(status: CommitteeStatus) {
    let headers = new HttpHeaders();
    headers = headers.set('auth', this.userService.getAuthOrDefault());
    return this.http.put(this.baseUrl + 'api/Conference/SetCommitteeStatus',status, { headers: headers });
  }

  public getCommitteeStatus(committeeid: string) {
    let headers = new HttpHeaders();
    headers = headers.set('committeeid', committeeid);
    return this.http.get<CommitteeStatus>(this.baseUrl + 'api/Conference/GetCommitteeStatus', { headers: headers });
  }
}
