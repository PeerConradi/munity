import { Component, OnInit, TemplateRef } from '@angular/core';
import { Speakerlist } from '../../../models/speakerlist.model';
import { TimeSpan } from '../../../models/TimeSpan';
import { ActivatedRoute } from '@angular/router';
import { Simulation } from '../../../models/simulation/simulation.model';
import { SimulationService } from '../../../services/simulator.service';
import { BsModalRef, BsModalService } from 'ngx-bootstrap';
import { FormBuilder, FormGroup } from '@angular/forms';
import { Delegation } from '../../../models/conference/delegation.model';
import { ConferenceService } from '../../../services/conference-service.service';
import { SpeakerListService } from '../../../services/speaker-list.service';
import { SimulationUser } from '../../../models/simulation/simulation-user.model';
import { Observable, of } from 'rxjs';
import { map, switchMap } from 'rxjs/operators';

@Component({
  selector: 'app-sim-sim-view',
  templateUrl: './sim-sim-view.component.html',
  styleUrls: ['./sim-sim-view.component.css']
})
export class SimSimViewComponent implements OnInit {

  modalRef: BsModalRef;

  public get speakerlist(): Speakerlist {
    if (this.simulation) {
      return this.simulation.Speakerlist;
    }
    return null;
  }

  private _simulation: Simulation;

  public get simulation(): Simulation {
    return this._simulation;
  }

  currentId: string;

  chatMessageForm: FormGroup;

  changeRoleForm: FormGroup;

  possibleDelegations: Delegation[];

  showSpeakerlist: boolean = false;

  constructor(private route: ActivatedRoute, private simulationService: SimulationService,
    private modalService: BsModalService, private formBuilder: FormBuilder,
    private conferenceService: ConferenceService,
    private speakerlistService: SpeakerListService) { }

  ngOnInit() {
    // Load a Speakerlist Template!

    this.chatMessageForm = this.formBuilder.group({
      message: ''
    });

    this.changeRoleForm = this.formBuilder.group({
      roletype: '',
      rolevalue: ''
    });

    //Testing with trivial Simuatlion right now
    //this._simulation = this.simulationService.currentSimulation;
    //this.simulationService.reloadCurrent().then(n => {
    //  this.speakerlist = n.Speakerlist;
    //  this.simulationService.currentSimulation = n;
    //  this._simulation = this.simulationService.currentSimulation;
    //});

    this._simulation = this.testSimulation;

    this.conferenceService.getAllDelegations().subscribe(n => {
      this.possibleDelegations = n
    });
    //this.simulation.pipe(map(r => this.speakerlist = r.Speakerlist));
  }

  get chairmen(): SimulationUser[] {
    if (this.simulation != null) {
      return this.simulation.Users.filter(u => u.Role == 'Chairman');
    } else {
      return [];
    }
  }

  get delegations(): SimulationUser[] {
    if (this.simulation != null) {
      return this.simulation.Users.filter(u => u.Role == 'Delegation');
    } else {
      return [];
    }
  }

  openModal(template: TemplateRef<any>) {
    this.modalRef = this.modalService.show(template);
  }

  postToChat(val) {
    this.simulationService.sendMessage(val.message).subscribe(n => {
      this.chatMessageForm.reset();
    });
  }

  changeRole(val) {
    console.log(val);
    if (val.roletype == 'Delegation' && val.rolevalue != null) {
      this.simulationService.setDelegation(val.rolevalue).subscribe(s => {
        this.modalRef.hide();
      });
    } else {
      this.simulationService.setRole(val.roletype).subscribe(s => {
        this.modalRef.hide();
      });
    }
  }

  test() {
    console.log(this.simulationService.currentSimulation);
    this.simulationService.getMe().subscribe(n => console.log(n));
  }

  private _testSim: Simulation;

  get testSimulation(): Simulation {
    if (this._testSim == null) {
      this._testSim = new Simulation();
      this._testSim.Speakerlist = new Speakerlist();
      this._testSim.Speakerlist.id = 'testliste';
      this._testSim.Speakerlist.remainingQuestionTime = new TimeSpan(0, 30, 0, 0, 0);
      this._testSim.Speakerlist.remainingSpeakerTime = new TimeSpan(0, 30, 0, 0, 0);
      this._testSim.Speakerlist.questiontime = new TimeSpan(0, 30, 0, 0, 0);
      this._testSim.Speakerlist.speakertime = new TimeSpan(0, 30, 0, 0, 0);
      this._testSim.Name = 'Test';

      let chairmanOne = new SimulationUser();
      chairmanOne.DisplayName = 'Vorsitzender 1';
      chairmanOne.Role = 'Chairman';
      this._testSim.Users.push(chairmanOne);
    }
    return this._testSim;
  }
}
