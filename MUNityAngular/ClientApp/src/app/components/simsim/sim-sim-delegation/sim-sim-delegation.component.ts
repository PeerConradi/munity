import { Component, OnInit, Input } from '@angular/core';
import { SimulationUser } from '../../../models/simulation-user.model';

@Component({
  selector: 'app-sim-sim-delegation',
  templateUrl: './sim-sim-delegation.component.html',
  styleUrls: ['./sim-sim-delegation.component.css']
})
export class SimSimDelegationComponent implements OnInit {

  @Input() hasRequest: boolean;

  @Input() user: SimulationUser;

  constructor() { }

  ngOnInit() {
  }

}
