import { Injectable, Inject } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Simulation } from '../models/simulation.model';
import { SimulationUser } from '../models/simulation-user.model';

@Injectable({
  providedIn: 'root'
})
export class SimulatorService {

  private baseUrl: string;

  constructor(private http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    this.baseUrl = baseUrl ;
  }

  public createSimulation() {
    return this.http.get<Simulation>(this.baseUrl + "api/simsim/CreateSimSim");
  }

  public getSimulation(id: string) {
    let headers = new HttpHeaders();
    headers = headers.set('id', id);
    return this.http.get<Simulation>(this.baseUrl + 'api/simsim/GetSimSim');
  }

  public tryJoin(simulationId: string, name: string) {
    let headers = new HttpHeaders();
    headers = headers.set('id', simulationId);
    headers = headers.set('name', name);
    return this.http.get<SimulationUser>(this.baseUrl + 'api/simsim/GetSimSim');
  }
}
