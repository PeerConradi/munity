import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { SimulatorService } from '../../../services/simulator.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-sim-sim-create',
  templateUrl: './sim-sim-create.component.html',
  styleUrls: ['./sim-sim-create.component.css']
})
export class SimSimCreateComponent implements OnInit {

  createForm: FormGroup;

  constructor(private formBuilder: FormBuilder, private simulatorService: SimulatorService, private router: Router) { }

  public get f() { return this.createForm.controls; }

  ngOnInit(): void {
    this.createForm = this.formBuilder.group({
      lobbyname: ['', Validators.required],
      username: ['', Validators.required],
      password: ''
    })
  }

  onSubmit(val) {
    if (this.createForm.valid == false)
      return;

    this.simulatorService.createSimulation(val.lobbyname, val.username, val.password).subscribe(n => {
      console.log(n);
      this.simulatorService.setMyToken(n.HiddenToken);
      this.router.navigate(['/sim/' + n.SimulationId]);
    });
    console.log(val);
  }

}
