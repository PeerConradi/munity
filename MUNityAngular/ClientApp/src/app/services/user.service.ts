import { Injectable, Inject } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Session } from 'inspector';
import { resolve } from 'dns';
import { Registration } from '../models/registration.model';
import { User } from '../models/user.model';
import { UserAuths } from '../models/user-auths.model';

@Injectable({
  providedIn: 'root'
})
export class UserService {

  private _loggedIn: boolean;

  public currentUser: User = null;

  public get isLoggedIn(): boolean {
    if (this.sessionkey() != '' && this.sessionkey() != null) {
      return true;
    } else {
      return false;
    }
  }

  public getAuthOrDefault(): string {
    let key = this.sessionkey();
    if (key == null || key == '') {
      key = 'default';
    }
    return key;
  }

  private baseUrl: string;


  

  public sessionkey(): string {
    return localStorage.getItem('munity_session_key');
  }

  public setSessionkey(val: string) {
    localStorage.setItem('munity_session_key', val);
  }

  public register(model: Registration) {
    return this.http.put(this.baseUrl + 'api/User/Register', model);
  }

  public checkUsername(username: string) {
    return this.http.get<boolean>(this.baseUrl + 'api/User/CheckUsername?username=' + username);
  }

  public changePassword(oldpassword: string, newpassword: string) {
    if (this.isLoggedIn == false) {
      return;
    }

    let headers = new HttpHeaders();
    headers = headers.set('auth', this.sessionkey());
    headers = headers.set('oldpassword', encodeURI(oldpassword + '|'));
    headers = headers.set('newpassword', encodeURI(newpassword + '|'));
    let options = { headers: headers };
    return this.http.get<string>(this.baseUrl + 'api/User/ChangePassword',
      options);
  }

  public validateKey(key: string) {
    let headers = new HttpHeaders();
    headers = headers.set('auth', this.sessionkey());
    let options = { headers: headers };
    var cmd = this.http.get(this.baseUrl + 'api/User/ValidateKey',
      options);
    //Whit every Auth-Key validation also update the user!
    if (this.sessionkey() !== '') {
      cmd.subscribe(n => {
        this.getMe().subscribe(user => this.currentUser = user);
      });
    }

    return cmd;
  }

  public logout() {
    
    let headers = new HttpHeaders();
    headers = headers.set('auth', this.sessionkey());
    let options = { headers: headers };
    this.http.post(this.baseUrl + 'api/User/Logout',null, options).subscribe(msg => {
      this.setSessionkey('');
      this._loggedIn = false;
    }, err => {
        console.log('Fehler beim abmelden')
    });
  }

  public async login(username: string, password: string) {
    let headers = new HttpHeaders();
    headers = headers.set('username', username);
    headers = headers.set('password', password);
    let error = false;
    let options = { headers: headers };
    await this.http.get<Login>(this.baseUrl + 'api/User/Login', options).toPromise().then(msg => {
      this.setSessionkey(msg.Key);
      this.currentUser = msg.User;
      this._loggedIn = true;
    }).catch(err => error = true);
    if (error == false)
      return true;
    else
      return false;
  }

  public getUser(username: string) {
    let headers = new HttpHeaders();
    headers = headers.set('auth', this.getAuthOrDefault());
    headers = headers.set('username', username);
    return this.http.get<User>(this.baseUrl + 'api/User/GetUserByUsername', { headers: headers });
  }

  public getUserAuths() {
    //CanUserCreateConference
    let headers = new HttpHeaders();
    headers = headers.set('auth', this.getAuthOrDefault());
    return this.http.get<UserAuths>(this.baseUrl + 'api/User/GetKeyAuths', { headers: headers });
  }

  public getIsAdmin() {
    let headers = new HttpHeaders();
    headers = headers.set('auth', this.getAuthOrDefault());
    return this.http.get<boolean>(this.baseUrl + 'api/User/IsAdmin', { headers: headers });
  }

  public getCurrentUser() {
    let headers = new HttpHeaders();
    headers = headers.set('auth', this.getAuthOrDefault());
    return this.http.get<User>(this.baseUrl + 'api/User/GetAuthUser', { headers: headers });
  }

  public updateUserinfo(model: User) {
    let headers = new HttpHeaders();
    headers = headers.set('auth', this.getAuthOrDefault());
    return this.http.patch(this.baseUrl + 'api/User/UpdateUserinfo', model, { headers: headers });
  }

  //Updates the Auth-Key
  //you can check if current user is not null!
  public getMe() {
    let headers = new HttpHeaders();
    headers = headers.set('auth', this.getAuthOrDefault());
    return this.http.get<User>(this.baseUrl + 'api/User/GetUserOfAuth', { headers: headers });
  }

  constructor(private http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    this.baseUrl = baseUrl;
    //Beim laden den letzten Auth-Key überprüfen

    if (this.sessionkey() != '') {
      this.validateKey(this.sessionkey()).subscribe(valid => {
      },
        err => {
          this.setSessionkey('');
        });
    }
    
  }
}

interface Login {
  Key: string;
  User: User;
}
