import { Injectable, Inject } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Session } from 'inspector';
import { resolve } from 'dns';

@Injectable({
  providedIn: 'root'
})
export class UserService {

  private _loggedIn: boolean;

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


  constructor(private http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    this.baseUrl = baseUrl;
  }

  public sessionkey(): string {
    return localStorage.getItem('munity_session_key');
  }

  public setSessionkey(val: string) {
    localStorage.setItem('munity_session_key', val);
  }

  public register(username: string, password: string, email: string) {
    let headers = new HttpHeaders();
    headers = headers.set('username', username);
    headers = headers.set('password', password);
    headers = headers.set('email', email);
    let options = { headers: headers };
    return this.http.get(this.baseUrl + 'api/User/Register',
      options);
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
    return this.http.get(this.baseUrl + 'api/User/ValidateKey',
      options);
  }


  public async logout() {
    let headers = new HttpHeaders();
    headers = headers.set('auth', this.sessionkey());
    let error = false;
    let options = { headers: headers };
    await this.http.get(this.baseUrl + 'api/User/Logout', options).toPromise().then(msg => {
      this.setSessionkey('');
      this._loggedIn = false;
    }).catch(err => error = true);
    if (error == false)
      return true;
    else
      return false;
  }

  public async login(username: string, password: string) {
    let headers = new HttpHeaders();
    headers = headers.set('username', username);
    headers = headers.set('password', password);
    let error = false;
    let options = { headers: headers };
    await this.http.get<Login>(this.baseUrl + 'api/User/Login', options).toPromise().then(msg => {
      localStorage.setItem('munity_session_key', msg.key);
      this._loggedIn = true;
    }).catch(err => error = true);
    if (error == false)
      return true;
    else
      return false;
  }
}

interface Login {
  key: string;
}
