import { Injectable, Inject } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Session } from 'inspector';
import { resolve } from 'dns';
import { Registration } from '../models/registration.model';
import { User } from '../models/user.model';
import { UserAuths } from '../models/user-auths.model';
import { AuthenticationResponse } from '../models/authentication-response.model';
import { GlobalsService } from './globals.service';

@Injectable({
  providedIn: 'root'
})
export class UserService {

  public currentUser: User = null;

  private baseUrl: string;


  private _isLoggedIn: boolean;
  public get isLoggedIn(): boolean {
    return this._isLoggedIn;
  }
  public set isLoggedIn(v: boolean) {
    this._isLoggedIn = v;
  }

  public get session(): AuthenticationResponse {
    return this.globalsService.session;
  }

  public sessionkey(): AuthenticationResponse {
    const val = localStorage.getItem('munity_session_key');
    console.log(val);
    if (val == null || val === "" || val === "undefined")
      return null;
    return JSON.parse(val);
  }

  public setSessionkey(val: AuthenticationResponse) {
    console.log(val);
    const str = JSON.stringify(val);
    console.log(str);
    localStorage.setItem('munity_session_key', str);
  }

  public register(model: Registration) {
    return this.http.post<User>(this.baseUrl + 'api/User/Register', model);
  }

  public async login(username: string, password: string): Promise<boolean> {
    const auth = new AuthenticateRequest(username, password);
    const result = await this.http.post<AuthenticationResponse>(this.baseUrl + 'api/user/login', auth).toPromise();
    if (result != null) {
      this.globalsService.session = result;
      this.setSessionkey(result);
      return true;
    } else {
      this.setSessionkey(null);
      return false;
    }
  }

  public justForVIP() {
    return this.http.get<string>(this.baseUrl + "api/user/justforvip");
  }

  public checkUsername(username: string) {
    return this.http.get<boolean>(this.baseUrl + 'api/User/CheckUsername?username=' + username);
  }

  public checkMail(username: string) {
    return this.http.get<boolean>(this.baseUrl + 'api/User/CheckMail?mail=' + username);
  }

  public logout() {
    this.globalsService.session = null;
    this.currentUser = null;
    this.setSessionkey(null);
  }

  public getMe() {
    return this.http.get<User>(this.baseUrl + 'api/User/WhoAmI');
  }

  constructor(private globalsService: GlobalsService, private http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    this.baseUrl = baseUrl;
    this.globalsService.session = this.sessionkey();
    if (this.globalsService.session != null) {
      this.isLoggedIn = true;
      this.getMe().subscribe(n => {
        this.currentUser = n;
      })
    }
  }
}

class AuthenticateRequest {

  constructor(public username: string, public password: string) {

  }
}

interface Login {
  Key: string;
  User: User;
}
