import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Component, Inject } from '@angular/core';
import { Observable, of } from 'rxjs';
import { Conference } from '../models/conference/conference.model';
import { UserService } from './user.service';
import { Committee } from '../models/conference/committee.model';
import { Delegation } from '../models/conference/delegation.model';
import { AddCommitteeRequest } from '../models/requests/add-committee-request';
import { CreateDelegationRequest } from '../models/requests/create-delegation-request';
import { ChangeConferenceNameRequest } from '../models/requests/change-conference-name-request';
import { User } from '../models/user.model';
import { UserConferenceRole } from '../models/user-conference-role.model';
import { CommitteeStatus } from '../models/conference/committee-status.model';
import * as r from '../models/conference/roles';
import { Project } from '../models/conference/project.model';

// Some day this thing should be renamed into ConferenceService!
@Injectable({
  providedIn: 'root'
})
export class ConferenceService {

  hasError: boolean = false;

  public committeeContext: Committee = null;

  private _currentConference: Conference = null;
  public get currentConference(): Conference {
    return this._currentConference;
  }

  public set currentConference(val: Conference) {
    this._currentConference = val;
  }

  private baseUrl: string;
  constructor(private http: HttpClient, private userService: UserService, @Inject('BASE_URL') baseUrl: string) {
    this.baseUrl = baseUrl;
  }

  public knownAbbreviations: string[] = ['ad', 'ae', 'af', 'ag', 'al', 'am', 'ao', 'ar', 'at', 'au', 'az', 'ba', 'bb', 'bd', 'be', 'bf', 'bg', 'bh', 'bi', 'bj', 'bn', 'bo', 'br', 'bs', 'bt', 'bw', 'by', 'bz', 'ca', 'cd', 'cf', 'cg', 'ch', 'ci', 'ck', 'cl', 'cm', 'cn', 'co', 'cr', 'cu', 'cv', 'cy', 'cz', 'de', 'dj', 'dk', 'dm', 'do', 'dz', 'ec', 'ee', 'eg', 'eh', 'er', 'es', 'et', 'fi', 'fj', 'fm', 'fr', 'ga', 'gb', 'gd', 'ge', 'gh', 'gm', 'gn', 'gq', 'gr', 'gt', 'gw', 'gy', 'hn', 'hr', 'ht', 'hu', 'id', 'ie', 'il', 'in', 'iq', 'ir', 'is', 'it', 'jm', 'jo', 'jp', 'ke', 'kg', 'kh', 'ki', 'km', 'kn', 'kp', 'kr', 'kw', 'kz', 'la', 'lb', 'lc', 'li', 'lk', 'lr', 'ls', 'lt', 'lu', 'lv', 'ly', 'ma', 'mc', 'md', 'me', 'mg', 'mh', 'mk', 'ml', 'mm', 'mn', 'mr', 'mt', 'mu', 'mv', 'mw', 'mx', 'my', 'mz', 'na', 'ne', 'ng', 'ni', 'nl', 'no', 'np', 'nr', 'nu', 'nz', 'om', 'pa', 'pe', 'pg', 'ph', 'pk', 'pl', 'ps', 'pt', 'pw', 'py', 'qa', 'ro', 'rs', 'ru', 'rw', 'sa', 'sb', 'sc', 'sd', 'se', 'sg', 'si', 'sk', 'sl', 'sm', 'sn', 'so', 'sr', 'ss', 'st', 'sv', 'sy', 'sz', 'td', 'tg', 'th', 'tj', 'tl', 'tm', 'tn', 'to', 'tr', 'tt', 'tv', 'tw', 'tz', 'ua', 'ug', 'un', 'us', 'uy', 'uz', 'va', 'vc', 've', 'vn', 'vu', 'ws', 'xk', 'ye', 'za', 'zm', 'zw'];

  public getAllConferences(): Observable<Conference[]> {
    return this.http.get<Conference[]>(this.baseUrl + 'api/conference/GetConferences');
  }

  public createProject(organisationId: string, name: string, short: string) {
    let body = {
      organisationId: organisationId,
      name: name,
      abbreviation: short
    };
    return this.http.post<Project>(this.baseUrl + 'api/Conference/CreateProject', body);
  }

  public createConference(projectId: string, name: string, fullName: string, short: string, startDate: Date, endDate: Date) {
    let body: any = {
      projectId: projectId,
      name: name,
      fullName: fullName,
      abbreviation: short,
      startDate: startDate.toDateString(),
      endDate: endDate.toDateString()
    }
    console.log('Body:');
    console.log(body);
    return this.http.post<Conference>(this.baseUrl + 'api/Conference/CreateConference', body);
  }

  public getConference(id: string) {
    if (id === 'test') return of(this.getTestConference());
    return this.http.get<Conference>(this.baseUrl + 'api/Conference/GetConference?id=' + id);
  }

  public addCommittee(conferenceid: string, committee: Committee) {

  }

  public createDelegation(delegation: Delegation) {

  }

  public getTypeNumberByName(name: string): number {
    if (name == 'COUNTRY') return 0;

    return -1;
  }

  public getAllDelegations() {
    return this.http.get<Delegation[]>(this.baseUrl + 'api/Conference/AllDelegations');
  }

  public getDelegationsOfConference(conferenceId: string) {
    if (conferenceId == 'test') return of(this.getTestDelegations());
    return this.http.get<Delegation[]>(this.baseUrl + 'api/Conference/DelegationsOfConference?id=' + conferenceId);
  }

  public getDelegateRolesOfConferece(conferenceId: string) {
    if (conferenceId == 'test') return of(this.getTestDelegateRoles());
    return this.http.get<r.Roles.DelegateRole[]>(this.baseUrl + 'api/Conference/DelegateRolesOfConferece?id=' + conferenceId);
  }

  public getTeam(conferenceid: string) {
    let headers = new HttpHeaders();
    headers = headers.set('conferenceid', conferenceid);
    let options = { headers: headers };
    return this.http.get<UserConferenceRole[]>(this.baseUrl + 'api/Conference/GetTeam',
      options);
  }

  public getTeamRoles(conferenceid: string) {
    if (conferenceid === 'test') return of(this.getTestTeamRoles());
    let headers = new HttpHeaders();
    headers = headers.set('conferenceid', conferenceid);
    let options = { headers: headers };
    return this.http.get<r.Roles.TeamRole[]>(this.baseUrl + 'api/Conference/GetTeamRoles',
      options);
  }

  public addTeamRole(role: r.Roles.TeamRole) {
    return this.http.put(this.baseUrl + 'api/Conference/AddTeamRole', role);
  }

  public addTeamMember(username: string, role: r.Roles.TeamRole) {
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

  public generateId(len: number): string {
    let allowedChars: string = "abcdefghijklmnopqrstuvwxyz0123456789";
    let returnVal: string = "";
    for (let i = 0; i < len; i++) {
      let charIndex = Math.random();
      charIndex = charIndex * allowedChars.length;
      charIndex = Math.round(charIndex);
      returnVal += allowedChars.charAt(charIndex);
    }
    return returnVal;
  }

  public getTestConference(): Conference {
    let conference = new Conference();
    conference.name = 'Test Conference';
    conference.fullName = 'Conference to Test Everything';
    conference.conferenceId = 'test';
    conference.roles = [];
    conference.committees = [];
    conference.teamRoleGroups = [];

    let committeeOne = new Committee();
    committeeOne.committeeId = 'gv';
    committeeOne.name = 'Generalversammlung';
    committeeOne.fullName = 'die Generalversammlung der Vereinten Nationen';
    committeeOne.abbreviation = 'GV';
    committeeOne.article = 'die';
    conference.committees.push(committeeOne);

    let committeeTwo = new Committee();
    committeeTwo.committeeId = 'sr';
    committeeTwo.name = 'Sicherheitsrat';
    committeeTwo.fullName = 'der UN Sicherheitsrat';
    committeeTwo.abbreviation = 'SR';
    committeeTwo.article = 'der';
    conference.committees.push(committeeTwo);


    this.getTestTeamRoleGroups().forEach(n => conference.teamRoleGroups.push(n));
    this.getTestTeamRoles().forEach(n => conference.roles.push(n));
    return conference;
  }

  public getTestTeamRoleGroups(): r.Roles.TeamRoleGroup[] {
    let groups: r.Roles.TeamRoleGroup[] = [];
    let plGroup = new r.Roles.TeamRoleGroup();
    plGroup.teamRoleGroupId = 0;
    plGroup.name = 'Projektleitung';
    plGroup.fullName = 'die Projektleitung';
    plGroup.groupLevel = 1;
    plGroup.abbreviation = 'PL';

    let gsGroup = new r.Roles.TeamRoleGroup();
    gsGroup.teamRoleGroupId = 1;
    gsGroup.name = 'Generalsekretariat';
    gsGroup.fullName = 'Generalsekretariat';
    gsGroup.groupLevel = 2;
    gsGroup.abbreviation = 'GS';

    groups.push(plGroup);
    groups.push(gsGroup);
    return groups;
  }

  public getTestTeamRoles(): r.Roles.TeamRole[] {
    let roles: r.Roles.TeamRole[] = [];

    let leaderRole: r.Roles.TeamRole = new r.Roles.TeamRole();
    leaderRole.roleId = 1;
    leaderRole.roleName = 'Projektleiter';
    leaderRole.teamRoleLevel = 1;
    leaderRole.roleFullName = 'Leiter des Projekts';
    leaderRole.teamRoleGroupId = 0;
    leaderRole.applicationState = r.Roles.EApplicationStates.CLOSED;

    let leaderRole2: r.Roles.TeamRole = new r.Roles.TeamRole();
    leaderRole2.roleId = 2;
    leaderRole2.roleName = 'Projektleiterin';
    leaderRole2.teamRoleLevel = 1;
    leaderRole2.roleFullName = 'Leiterin des Projekts';
    leaderRole2.teamRoleGroupId = 0;
    leaderRole.applicationState = r.Roles.EApplicationStates.CLOSED;

    let gsRole: r.Roles.TeamRole = new r.Roles.TeamRole();
    gsRole.roleId = 2;
    gsRole.roleName = 'Generalsekretär';
    gsRole.teamRoleLevel = 1;
    gsRole.roleFullName = 'Generalsekretär';
    gsRole.teamRoleGroupId = 1;
    gsRole.applicationState = r.Roles.EApplicationStates.CLOSED_TO_PUBLIC;

    roles.push(leaderRole);
    roles.push(leaderRole2);
    roles.push(gsRole);

    return roles;
  }

  public getTestDelegations(): Delegation[] {
    let delegations: Delegation[] = [];

    let delGermany: Delegation = new Delegation();
    delGermany.delegationId = 'delGer';
    delGermany.name = 'Deutschland';
    delGermany.fullName = 'Bundesrepublik Deutschland';
    delGermany.abbreviation = 'DE';

    delegations.push(delGermany);

    return delegations;
  }

  public getTestDelegateRoles(): r.Roles.DelegateRole[] {
    let roles: r.Roles.DelegateRole[] = [];

    let delDe = new r.Roles.DelegateRole();
    delDe.roleId = 1;
    delDe.roleName = "Abgeordneter Deutschland"
    delDe.committeeId = 'gv';

    roles.push(delDe);

    return roles;
  }

}
