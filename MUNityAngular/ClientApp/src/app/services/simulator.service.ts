import { Injectable, Inject } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Simulation } from '../models/simulation/simulation.model';
import { SimulationUser } from '../models/simulation/simulation-user.model';
import { CreateSimulationRequest } from './requestSchema/create-simulation-request';
import * as signalR from '@aspnet/signalr';
import { Session } from 'inspector';
import { SimulationLobbyInfo } from '../models/simulation/simulation-lobby-info.model';
import { of, Observable } from 'rxjs';
import { SimulationMessage } from '../models/simulation/simulation-message.model';
import { AddSimulationChatMessageRequest } from './requestSchema/add-simulation-chat-message-request';
import { Delegation } from '../models/conference/delegation.model';
import { SimulationRequest } from '../models/simulation/simulation-request.model';

@Injectable({
  providedIn: 'root'
})
export class SimulationService {

  private baseUrl: string;

  private _currentUserToken: string;

  private _currentSimulation: Simulation;

  private _currentUser: SimulationUser;

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

  public set userToken(token: string) {
    this._currentUserToken = token;
    localStorage.setItem('lastSimToken', token);
  }

  public get userToken() {
    if (this._currentUserToken == null)
      this._currentUserToken = localStorage.getItem('lastSimToken');
    return this._currentUserToken;
  }


  public set currentSimulation(simulation: Simulation) {
    this._currentSimulation = simulation;
    if (simulation == null) {
      localStorage.removeItem('lastsimulation');
    } else {
      this.joinSocket(simulation.SimSimId);
      this.addSocketListener(simulation);
      localStorage.setItem('lastsimulation', simulation.SimSimId);
    }

  }

  public get currentSimulation(): Simulation {
    return this._currentSimulation;
  }

  public set currentUser(user: SimulationUser) {
    this._currentUser = user;
  }

  public get currentUser(): SimulationUser {
    return this._currentUser;
  }

  public getMe() {
    return this.getUserByHiddenToken(this.currentSimulation.SimSimId, this.userToken);
  }

  public async createSimulation(lobbyName: string, creatorName: string, password: string): Promise<Simulation> {
    const requestBody = new CreateSimulationRequest();
    requestBody.LobbyName = lobbyName;
    requestBody.CreatorName = creatorName;
    requestBody.Password = password;
    console.log('CreateSimulation called')
    let request = this.http.post<simSimCreationResponse>(this.baseUrl + "api/simsim/CreateSimSim", requestBody);
    let response = await request.toPromise();
    if (response != null) {
      console.log(response);
      this.userToken = response.HiddenToken;
      this.getUserByHiddenToken(response.SimulationId, response.HiddenToken).subscribe(a => {
        this.currentUser = a;
      });
      let simulation = this.getSimulation(response.SimulationId).toPromise();
      return simulation;
    }
    return null;
  }

  public getSimulation(id: string) {
    let rquestHeaders = new HttpHeaders();
    rquestHeaders = rquestHeaders.set('simulationid', id.toString());
    return this.http.get<Simulation>(this.baseUrl + 'api/simsim/GetSimSim', { headers: rquestHeaders });
  }

  public reloadCurrent(): Promise<Simulation> {
    let storage = localStorage.getItem('lastsimulation');
    if (storage != null) {
      return this.getSimulation(storage).toPromise();
    }

  }

  public getLobbies() {
    return this.http.get<SimulationLobbyInfo[]>(this.baseUrl + 'api/simsim/GetLobbies');
  }

  public checkHiddenToken(simulationid: string) {
    let headers = new HttpHeaders();
    headers = headers.set('simulationid', simulationid);
    headers = headers.set('hiddentoken', this.userToken);
    if (this.userToken == null)
      return of(null);
    return this.http.get<boolean>(this.baseUrl + 'api/simsim/IsHiddenTokenInside', { headers: headers });
  }

  public tryJoin(simulationId: string, name: string) {
    let headers = new HttpHeaders();
    headers = headers.set('id', simulationId.toString());
    headers = headers.set('name', name);
    return this.http.get<SimulationUser>(this.baseUrl + 'api/simsim/TryJoin', { headers: headers });
  }


  public sendMessage(message: string) {
    let request = new AddSimulationChatMessageRequest();
    request.SimulationId = this.currentSimulation.SimSimId.toString();
    request.UserToken = this.userToken;
    request.Text = message;
    return this.http.post(this.baseUrl + 'api/simsim/AddChatMessage', request);
  }

  public setDelegation(delegationid: string) {
    let request = this.http.post<Delegation>(this.baseUrl + 'api/simsim/SetDelegation', {
      'simulationid': this.currentSimulation.SimSimId.toString(),
      'token': this.userToken,
      'delegationid': delegationid
    });

    request.subscribe(n => {
      if (this.currentUser != null) {
        this.currentUser.Role = "Delegation";
        this.currentUser.Delegation = n;
      }
    });

    return request;
  }

  public setRole(role: string) {
    console.log(this.currentSimulation.SimSimId);
    console.log(this.userToken);
    let request = this.http.post(this.baseUrl + 'api/simsim/SetRole', {
      'simulationid': this.currentSimulation.SimSimId.toString(),
      'token': this.userToken,
      'role': role
    });
    request.subscribe(success => {
      if (this.currentUser != null) {
        this.currentUser.Role = role;
      }
    });
    return request;
  }

  public sendRequest(requestype: string, message: string) {
    return this.http.post(this.baseUrl + 'api/simsim/PostRequest', {
      simulationid: this.currentSimulation.SimSimId.toString(),
      hiddentoken: this.userToken,
      token: this.currentUser.UserToken,
      type: requestype,
      message: message
    });
  }

  public removeRequest(requestid: string) {
    return this.http.patch(this.baseUrl + 'api/simsim/PostRequest', {
      simulationid: this.currentSimulation.SimSimId.toString(),
      hiddentoken: this.userToken,
      requestid: requestid,
    });
  }

  public removeAllMyRequests() {
    return this.http.patch(this.baseUrl + 'api/simsim/DeleteAllRequest', {
      simulationid: this.currentSimulation.SimSimId.toString(),
      hiddentoken: this.userToken,
      requestid: '',
    });
  }

  public getUserByHiddenToken(simulationid: string, token: string) {
    let headers = new HttpHeaders();
    headers = headers.set('simulationid', simulationid.toString());
    headers = headers.set('token', token);
    return this.http.get<SimulationUser>(this.baseUrl + 'api/simsim/GetUserByHiddenToken', { headers: headers });
  }

  // Signal R for the Socket connection

  public joinSocket(simulationId: string) {
    if (this._hubConnection == null || this._hubConnection.state != signalR.HubConnectionState.Connected) {
      console.log('let me create the connection first');
      this.connectSocket().then(() => {
        console.log('connection done let me now join');
        console.log(simulationId + ', ' + this.userToken);
        this._hubConnection.invoke('join', simulationId.toString(), this.userToken);
      });
    } else {
      console.log('I am connected let me join');
      this._hubConnection.invoke('join', simulationId, this.userToken);
    }
  }

  public addSocketListener = (simulation: Simulation) => {
    // Call this function when a User joined the game
    this._hubConnection.on('UserJoined', (user: SimulationUser) => {
      if (!simulation.Users.find(n => n.UserToken == user.UserToken))
        simulation.Users.push(user);
    });

    this._hubConnection.on('UserLeft', (user: SimulationUser) => {
      const inList = simulation.Users.findIndex((n => n.UserToken === user.UserToken) as any);
      if (inList !== -1)
        simulation.Users = simulation.Users.splice(inList, 1);
    });

    this._hubConnection.on('ChatMessageAdded', (message: SimulationMessage) => {
      simulation.AllChat.push(message);
    });

    this._hubConnection.on('UserChangedDelegation', (usertoken: string, delegation: Delegation) => {
      let user = simulation.Users.find(n => n.UserToken == usertoken);
      if (user != null) {
        user.Delegation = delegation;
        user.Role = "Delegation";
      }
    });

    this._hubConnection.on('UserChangedRole', (usertoken: string, role: string) => {
      let user = simulation.Users.find(n => n.UserToken == usertoken);
      if (user != null) {
        user.Role = role;
      }
    });

    this._hubConnection.on('RequestAdded', (request: SimulationRequest) => {
      simulation.Requests.push(request);
    });

    this._hubConnection.on('RequestRemoved', (request: SimulationRequest) => {
      let index = simulation.Requests.findIndex((n => n.SimSImRequestModelId === request.SimSImRequestModelId) as any);
      if (index !== -1) {
        simulation.Requests = simulation.Requests.splice(index, 1);
      }
    });

    this._hubConnection.on('RequestsChanged', (requests: SimulationRequest[]) => {
      simulation.Requests = requests;
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
