import { Injectable, Inject } from '@angular/core';
import { UserService } from './user.service';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { User } from '../models/user.model';
import { Conference } from '../models/conference/conference.model';

@Injectable({
  providedIn: 'root'
})
export class AdminService {

  private baseUrl: string;

  constructor(private httpClient: HttpClient, private userService: UserService, @Inject('BASE_URL') baseUrl: string) {
    this.baseUrl = baseUrl;
  }

  //Get the Count of Resolutions that exist inside the MongoDb
  public getResolutionMongoDbCount() {
    return this.httpClient.get<number>(this.baseUrl + 'api/Admin/GetResolutionMongoCount');
  }

  //Get the Count of Resolutions that exist inside the MySQL Database.
  public getResolutionDatabaseCount() {
    return this.httpClient.get<number>(this.baseUrl + 'api/Admin/GetResolutionDatabaseCount');
  }

  public getUsers() {
    return this.httpClient.get<User[]>(this.baseUrl + 'api/Admin/GetAllUsers');
  }

  public getUserCount() {
    return this.httpClient.get<number>(this.baseUrl + 'api/Admin/GetUserCount');
  }

  public getConferenceCount() {
    return this.httpClient.get<number>(this.baseUrl + 'api/Admin/GetConferenceCount');
  }

  public getConferenceCacheCount() {
    return this.httpClient.get<number>(this.baseUrl + 'api/Admin/GetConferenceCacheCount');
  }

  public getConferences() {
    return this.httpClient.get<Conference[]>(this.baseUrl + 'api/Admin/GetConferences');
  }

  public restoreResolutions() {
    return this.httpClient.get<Conference[]>(this.baseUrl + 'api/Admin/RestoreResolutions');
  }

  public purgeResolutions() {
    return this.httpClient.get<Conference[]>(this.baseUrl + 'api/Admin/PurgeResolutions');
  }
}
