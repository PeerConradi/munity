import { Injectable } from '@angular/core';
import * as signalR from "@aspnet/signalr";
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ResolutionService {
  private _hubConnection: signalR.HubConnection;

  constructor(private httpClient: HttpClient) {
    this._hubConnection = new signalR.HubConnectionBuilder().withUrl('https://localhost:44395/resasocket').build();
    this._hubConnection
      .start()
      .then(() => console.log('Connection started!'))
      .catch(err => console.log('Error while establishing connection :('));
  }

  public getResolution(id: string) {
    return this.httpClient.get<Resolution>('https://localhost:44395/api/Resolution/Get?auth=default&id=' + id);
  }

}

interface Resolution {

}
