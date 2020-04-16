import { Component, OnInit, TemplateRef } from '@angular/core';
import { Speakerlist } from '../../../models/speakerlist.model';
import { TimeSpan } from '../../../models/TimeSpan';
import { ActivatedRoute } from '@angular/router';
import { Simulation } from '../../../models/simulation.model';
import { SimulatorService } from '../../../services/simulator.service';
import { BsModalRef, BsModalService } from 'ngx-bootstrap';
import { FormBuilder, FormGroup } from '@angular/forms';
import { Delegation } from '../../../models/delegation.model';
import { ConferenceServiceService } from '../../../services/conference-service.service';

@Component({
  selector: 'app-sim-sim-view',
  templateUrl: './sim-sim-view.component.html',
  styleUrls: ['./sim-sim-view.component.css']
})
export class SimSimViewComponent implements OnInit {

  modalRef: BsModalRef;

  speakerlist: Speakerlist;

  simulation: Simulation;

  currentId: string;

  chatMessageForm: FormGroup;

  changeRoleForm: FormGroup;

  possibleDelegations: Delegation[];

  constructor(private route: ActivatedRoute, private simulationService: SimulatorService,
    private modalService: BsModalService, private formBuilder: FormBuilder,
  private conferenceService: ConferenceServiceService) { }

  ngOnInit() {
    this.chatMessageForm = this.formBuilder.group({
      message: ''
    });

    this.changeRoleForm = this.formBuilder.group({
      roletype: '',
      rolevalue: ''
    });

    this.route.params.subscribe(n => {
      this.currentId = n.id;
      this.simulationService.getSimulation(n.id).subscribe(a => {
        this.simulation = a;
        console.log(a);
        this.simulationService.joinSocket(a.SimSimId);
        this.simulationService.addSocketListener(this.simulation);
      });
    });

    let slist = new Speakerlist();
    slist.Speakertime = new TimeSpan(0, 0, 3, 0, 0);
    slist.Questiontime = new TimeSpan(0, 30, 0, 0, 0);
    slist.RemainingSpeakerTime = new TimeSpan(0, 0, 3, 0, 0);
    slist.RemainingQuestionTime = new TimeSpan(0, 30, 0, 0, 0);
    slist.Status = 0;
    this.speakerlist = slist;

    this.conferenceService.getAllDelegations().subscribe(n => this.possibleDelegations = n);
  }

  openModal(template: TemplateRef<any>) {
    this.modalRef = this.modalService.show(template);
  }

  postToChat(val) {
    this.simulationService.sendMessage(this.simulation.SimSimId.toString(), val.message).subscribe(n => {
      this.chatMessageForm.reset();
    });
  }

  changeRole(val) {
    console.log(val);
    if (val.roletype == 'Delegation' && val.rolevalue != null) {
      this.simulationService.setDelegation(this.simulation.SimSimId.toString(), val.rolevalue).subscribe();
    }
  }

}
