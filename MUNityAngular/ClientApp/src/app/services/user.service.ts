import { Injectable, Inject } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class UserService {

  private baseUrl: string;
  constructor(private http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    this.baseUrl = baseUrl;
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

  public login(username: string, password: string) {
    let headers = new HttpHeaders();
    headers = headers.set('username', username);
    headers = headers.set('password', password);
    let options = { headers: headers };
    return this.http.get(this.baseUrl + 'api/User/Login',
      options);
  }


}
