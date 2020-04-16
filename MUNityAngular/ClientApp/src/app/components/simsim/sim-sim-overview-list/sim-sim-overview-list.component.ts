import { Component, OnInit } from '@angular/core';
import { SimulationLobbyInfo } from '../../../models/simulation-lobby-info.model';
import { SimulatorService } from '../../../services/simulator.service';

@Component({
  selector: 'app-sim-sim-overview-list',
  templateUrl: './sim-sim-overview-list.component.html',
  styleUrls: ['./sim-sim-overview-list.component.css']
})
export class SimSimOverviewListComponent implements OnInit {

  lobbies: SimulationLobbyInfo[] = [];

  constructor(private service: SimulatorService) { }

  ngOnInit(): void {
    this.service.getLobbies().subscribe(l => {
      this.lobbies = l;
    });
  }

}
