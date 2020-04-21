import { Component, OnInit, Input } from '@angular/core';
import { SimulationUser } from '../../../models/simulation-user.model';
import { SimulationService } from '../../../services/simulator.service';
import { SimulationRequest } from '../../../models/simulation-request.model';

@Component({
  selector: 'app-sim-sim-delegation',
  templateUrl: './sim-sim-delegation.component.html',
  styleUrls: ['./sim-sim-delegation.component.css']
})
export class SimSimDelegationComponent implements OnInit {

  public get hasRequest(): boolean {
    return this.simulationService.currentSimulation.Requests.find(n => n.UserToken == this.user.UserToken) != null;
  }

  @Input() user: SimulationUser;

  isMe: boolean = false;

  public get currentRequests(): SimulationRequest {
    return this.simulationService.currentSimulation.Requests.find(n => n.UserToken == this.user.UserToken);
  }

  constructor(private simulationService: SimulationService) { }

  ngOnInit() {
    this.simulationService.getMe().subscribe(n => {
      this.isMe = (n.UserToken == this.user.UserToken);
    }); 
  }

}
