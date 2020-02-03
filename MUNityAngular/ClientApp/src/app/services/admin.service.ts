import { Injectable, Inject } from '@angular/core';
import { UserService } from './user.service';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { User } from '../models/user.model';

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
    let authString: string = 'default';
    if (this.userService.isLoggedIn)
      authString = this.userService.sessionkey();
    let headers = new HttpHeaders();
    headers = headers.set('auth', authString);
    let options = { headers: headers };
    return this.httpClient.get<number>(this.baseUrl + 'api/Admin/GetResolutionMongoCount', options);
  }

  //Get the Count of Resolutions that exist inside the MySQL Database.
  public getResolutionDatabaseCount() {
    let authString: string = 'default';
    if (this.userService.isLoggedIn)
      authString = this.userService.sessionkey();
    let headers = new HttpHeaders();
    headers = headers.set('auth', authString);
    let options = { headers: headers };
    return this.httpClient.get<number>(this.baseUrl + 'api/Admin/GetResolutionDatabaseCount', options);
  }

  public getUsers() {
    let authString: string = 'default';
    authString = this.userService.sessionkey();
    let headers = new HttpHeaders();
    headers = headers.set('auth', authString);
    let options = { headers: headers };
    return this.httpClient.get<User[]>(this.baseUrl + 'api/Admin/GetAllUsers', options);
  }
}
