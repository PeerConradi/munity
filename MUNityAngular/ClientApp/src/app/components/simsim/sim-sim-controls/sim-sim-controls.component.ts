import { Component, OnInit } from '@angular/core';
import { SimulationService } from '../../../services/simulator.service';
import { SimulationUser } from '../../../models/simulation/simulation-user.model';
import { SpeakerListService } from '../../../services/speaker-list.service';

@Component({
  selector: 'app-sim-sim-controls',
  templateUrl: './sim-sim-controls.component.html',
  styleUrls: ['./sim-sim-controls.component.css']
})
export class SimSimControlsComponent implements OnInit {

  currentUser: SimulationUser;

  constructor(private simulationService: SimulationService, private speakerlistService: SpeakerListService) { }

  ngOnInit() {
    this.simulationService.getMe().subscribe(n => {
      this.currentUser = n;
      this.simulationService.currentUser = n;
    });
  }

  controlSpeakerlist() {
    if (this.simulationService.currentSimulation != null) {
      if (this.speakerlistService.currentList != this.simulationService.currentSimulation.Speakerlist) {
        this.speakerlistService.currentList = this.simulationService.currentSimulation.Speakerlist
      } else {
        this.speakerlistService.currentList = null;
      }
    }
  }

  requestAddToSpeakerlist() {
    this.simulationService.sendRequest('addToSpeakerlist', '').subscribe(n => {
    });
  }

  requestAddToQuestions() {
    this.simulationService.sendRequest('addToQuestions', '').subscribe();
  }

  requestGO() {
    this.simulationService.sendRequest('GO', '').subscribe();
  }

  requestPP() {
    this.simulationService.sendRequest('PP', '').subscribe();
  }

  deleteRequests() {
    this.simulationService.removeAllMyRequests().subscribe();
  }
}
