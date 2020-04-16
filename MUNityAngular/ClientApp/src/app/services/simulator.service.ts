import { Injectable, Inject } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Simulation } from '../models/simulation.model';
import { SimulationUser } from '../models/simulation-user.model';
import { CreateSimulationRequest } from './requestSchema/create-simulation-request';
import * as signalR from '@aspnet/signalr';
import { Session } from 'inspector';
import { SimulationLobbyInfo } from '../models/simulation-lobby-info.model';
import { of } from 'rxjs';
import { SimulationMessage } from '../models/simulation-message.model';
import { AddSimulationChatMessageRequest } from './requestSchema/add-simulation-chat-message-request';
import { Delegation } from '../models/delegation.model';

@Injectable({
  providedIn: 'root'
})
export class SimulatorService {

  private baseUrl: string;

  private myToken: string;

  private _currentSimulation: Simulation;

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

  public setCurrentSimulation(simulation: Simulation) {
    this._currentSimulation = simulation;
  }

  public get currentSimulation() {
    return this._currentSimulation;
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

  public getLobbies() {
    return this.http.get<SimulationLobbyInfo[]>(this.baseUrl + 'api/simsim/GetLobbies');
  }

  public checkHiddenToken(simulationid: string) {
    let headers = new HttpHeaders();
    headers = headers.set('simulationid', simulationid);
    headers = headers.set('hiddentoken', this.myToken);
    if (this.myToken == null)
      return of(null);
    return this.http.get<boolean>(this.baseUrl + 'api/simsim/IsHiddenTokenInside', { headers: headers });
  }

  public tryJoin(simulationId: string, name: string) {
    let headers = new HttpHeaders();
    headers = headers.set('id', simulationId);
    headers = headers.set('name', name);
    return this.http.get<SimulationUser>(this.baseUrl + 'api/simsim/TryJoin', { headers: headers });
  }

  public sendMessage(simulationid: string, message: string) {
    let request = new AddSimulationChatMessageRequest();
    request.SimulationId = simulationid;
    request.UserToken = this.myToken;
    request.Text = message;
    return this.http.post(this.baseUrl + 'api/simsim/AddChatMessage', request);
  }

  public setDelegation(simulationid: string, delegationid: string) {
    return this.http.post(this.baseUrl + 'api/simsim/SetDelegation', {
      'simulationid': simulationid,
      'token': this.myToken,
      'delegationid': delegationid
    });
  }

  // Signal R for the Socket connection

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

  public addSocketListener = (simulation: Simulation) => {
    // Call this function when a User joined the game
    this._hubConnection.on('UserJoined', (user: SimulationUser) => {
      if (!simulation.Users.find(n => n.UserToken == user.UserToken))
        simulation.Users.push(user);
    });

    this._hubConnection.on('UserLeft', (user: SimulationUser) => {
      const inList = simulation.Users.findIndex(n => n.UserToken == user.UserToken);
      if (inList != -1)
        simulation.Users = simulation.Users.splice(inList, 1);
    });

    this._hubConnection.on('ChatMessageAdded', (message: SimulationMessage) => {
      simulation.AllChat.push(message);
    });

    this._hubConnection.on('UserChangedDelegation', (usertoken: string, delegation: Delegation) => {
      let user = simulation.Users.find(n => n.UserToken == usertoken);
      if (user != null) {
        user.Delegation = delegation;
      }
    });
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
