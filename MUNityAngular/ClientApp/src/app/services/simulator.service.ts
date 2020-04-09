import { Injectable, Inject } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Simulation } from '../models/simulation.model';
import { SimulationUser } from '../models/simulation-user.model';
import { CreateSimulationRequest } from './requestSchema/create-simulation-request';
import * as signalR from '@aspnet/signalr';
import { Session } from 'inspector';

@Injectable({
  providedIn: 'root'
})
export class SimulatorService {

  private baseUrl: string;

  private myToken: string;

  private _hubConnection: signalR.HubConnection;

  constructor(private http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    this.baseUrl = baseUrl;
  }

  private connectSocket() {
    const builder = new signalR.HubConnectionBuilder().withUrl(this.baseUrl + 'simsocket');
    this._hubConnection = builder.build();
    this._hubConnection.serverTimeoutInMilliseconds = 9600000;
    return this._hubConnection.start();
  }

  public setMyToken(token: string) {
    this.myToken = token;
    localStorage.setItem('lastSimToken', token);
  }

  public get activeToken() {
    if (this.myToken == null)
      this.myToken = localStorage.getItem('lastSimToken');
    return this.myToken;
  }

  public createSimulation(lobbyName: string, creatorName: string, password: string) {
    const request = new CreateSimulationRequest();
    request.LobbyName = lobbyName;
    request.CreatorName = creatorName;
    request.Password = password;
    return this.http.post<simSimCreationResponse>(this.baseUrl + "api/simsim/CreateSimSim", request);
  }

  public getSimulation(id: string) {
    let headers = new HttpHeaders();
    headers = headers.set('id', id);
    return this.http.get<Simulation>(this.baseUrl + 'api/simsim/GetSimSim', { headers: headers });
  }

  public joinSocket(simulationId: string) {
    if (this._hubConnection == null || this._hubConnection.state != signalR.HubConnectionState.Connected) {
      console.log('let me create the connection first');
      this.connectSocket().then(() => {
        console.log('connection done let me now join');
        console.log(simulationId + ', ' + this.activeToken);
        this._hubConnection.invoke('join', simulationId.toString(), this.activeToken);
      });
    } else {
      console.log('I am connected let me join');
      this._hubConnection.invoke('join', simulationId, this.activeToken);
    }
  }

  public tryJoin(simulationId: string, name: string) {
    let headers = new HttpHeaders();
    headers = headers.set('id', simulationId);
    headers = headers.set('name', name);
    
    return this.http.get<SimulationUser>(this.baseUrl + 'api/simsim/GetSimSim');
  }


}

interface simSimCreationResponse {
  SimulationId: string;
  HiddenToken: string;
}

interface SessionToken {
  SessionId: string;
  Token: string;
}
