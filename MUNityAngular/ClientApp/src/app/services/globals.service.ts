import { Injectable } from '@angular/core';
import { AuthenticationResponse } from '../models/authentication-response.model';

@Injectable({
  providedIn: 'root'
})
export class GlobalsService {

  public session: AuthenticationResponse = null;

  constructor() { }
}
