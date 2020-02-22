import { Injectable, Inject } from '@angular/core';
import { UserService } from './user.service';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { User } from '../models/user.model';
import { Conference } from '../models/conference.model';

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
    let headers = new HttpHeaders();
    headers = headers.set('auth', this.userService.getAuthOrDefault());
    let options = { headers: headers };
    return this.httpClient.get<number>(this.baseUrl + 'api/Admin/GetResolutionMongoCount', options);
  }

  //Get the Count of Resolutions that exist inside the MySQL Database.
  public getResolutionDatabaseCount() {
    let headers = new HttpHeaders();
    headers = headers.set('auth', this.userService.getAuthOrDefault());
    let options = { headers: headers };
    return this.httpClient.get<number>(this.baseUrl + 'api/Admin/GetResolutionDatabaseCount', options);
  }

  public getUsers() {
    let headers = new HttpHeaders();
    headers = headers.set('auth', this.userService.getAuthOrDefault());
    let options = { headers: headers };
    return this.httpClient.get<User[]>(this.baseUrl + 'api/Admin/GetAllUsers', options);
  }

  public getUserCount() {
    let headers = new HttpHeaders();
    headers = headers.set('auth', this.userService.getAuthOrDefault());
    return this.httpClient.get<number>(this.baseUrl + 'api/Admin/GetUserCount', { headers: headers });
  }

  public getConferenceCount() {
    let headers = new HttpHeaders();
    headers = headers.set('auth', this.userService.getAuthOrDefault());
    return this.httpClient.get<number>(this.baseUrl + 'api/Admin/GetConferenceCount', { headers: headers });
  }

  public getConferenceCacheCount() {
    let headers = new HttpHeaders();
    headers = headers.set('auth', this.userService.getAuthOrDefault());
    return this.httpClient.get<number>(this.baseUrl + 'api/Admin/GetConferenceCacheCount', { headers: headers });
  }

  public getConferences() {
    let headers = new HttpHeaders();
    headers = headers.set('auth', this.userService.getAuthOrDefault());
    return this.httpClient.get<Conference[]>(this.baseUrl + 'api/Admin/GetConferences', { headers: headers });
  }

  public restoreResolutions() {
    let headers = new HttpHeaders();
    headers = headers.set('auth', this.userService.getAuthOrDefault());
    return this.httpClient.get<Conference[]>(this.baseUrl + 'api/Admin/RestoreResolutions', { headers: headers });
  }

  public purgeResolutions() {
    let headers = new HttpHeaders();
    headers = headers.set('auth', this.userService.getAuthOrDefault());
    return this.httpClient.get<Conference[]>(this.baseUrl + 'api/Admin/PurgeResolutions', { headers: headers });
  }
}
