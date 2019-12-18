import { Injectable, Inject } from '@angular/core';
import * as signalR from "@aspnet/signalr";

@Injectable({
  providedIn: 'root'
})
export class SignalrtestService {
  private _hubConnection: signalR.HubConnection;
  msgs: Message[] = [];

  private baseUrl: string;

  constructor(@Inject('BASE_URL') baseUrl: string) {
    this.baseUrl = baseUrl;
  }

  public startConnection = () => {
    this._hubConnection = new signalR.HubConnectionBuilder().withUrl(this.baseUrl + 'signalrtest').build();
    this._hubConnection
      .start()
      .then(() => console.log('Connection started!'))
      .catch(err => console.log('Error while establishing connection :('));
  }

  public addMessageDataListener = () => {
    this._hubConnection.on('sendToAll', (name: string, message: string) => {
      let msg: string = message + " from " + name;
      this.msgs.push({name: name, content: message});
      console.log(msg);
    });
  }

  public subscribeToHub() {
    this._hubConnection.send("Subscribe")
  }

}

interface Message {
  name: string,
  content: string;
}
