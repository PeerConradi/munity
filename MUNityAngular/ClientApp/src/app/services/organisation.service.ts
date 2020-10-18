import { Inject, Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Organisation } from '../models/orgnaisation.model';
import { UserService } from './user.service';
import { CreateOrganisationRequest } from '../models/requests/organisationRequests';
import { Project } from '../models/conference/project.model';

@Injectable({
  providedIn: 'root'
})
export class OrganisationService {

  private baseUrl: string;
  constructor(private http: HttpClient, private userService: UserService, @Inject('BASE_URL') baseUrl: string) {
    this.baseUrl = baseUrl;
  }

  public getOrganisation(id: string) {
    return this.http.get<Organisation>(this.baseUrl + 'api/Organisation/GetOrganisation?id=' + id);
  }

  public createOrganisation(name: string, abbreviation: string) {
    let body = new CreateOrganisationRequest(name, abbreviation);
    return this.http.post<Organisation>(this.baseUrl + 'api/Organisation/CreateOrganisation', body);
  }

  public getProjectsOfOrganisation(id: string) {
    return this.http.get<Project[]>(this.baseUrl + 'api/Conference/GetOrganisationProjects?organisationId=' + id);
  }

  public getProjectWithConferences(id: string) {
    return this.http.get<Project>(this.baseUrl + 'api/Conference/GetProjectWithConferences?id=' + id);
  }

  public getOrganisationsOfUser(username: string) {

  }

  public getMyOrganisations() {
    return this.http.get<Organisation[]>(this.baseUrl + 'api/Organisation/GetMyOrganisations');
  }
}
