import { Component, OnInit, TemplateRef } from '@angular/core';
import { Speakerlist } from '../../../models/speakerlist.model';
import { TimeSpan } from '../../../models/TimeSpan';
import { ActivatedRoute } from '@angular/router';
import { Simulation } from '../../../models/simulation.model';
import { SimulatorService } from '../../../services/simulator.service';
import { BsModalRef, BsModalService } from 'ngx-bootstrap';

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

  constructor(private route: ActivatedRoute, private simulationService: SimulatorService,
    private modalService: BsModalService) { }

  ngOnInit() {
    this.route.params.subscribe(n => {
      this.currentId = n.id;
      this.simulationService.getSimulation(n.id).subscribe(a => {
        this.simulation = a;
        console.log(a);
        this.simulationService.joinSocket(a.SimSimId);
      });
    });

    let slist = new Speakerlist();
    slist.Speakertime = new TimeSpan(0, 0, 3, 0, 0);
    slist.Questiontime = new TimeSpan(0, 30, 0, 0, 0);
    slist.RemainingSpeakerTime = new TimeSpan(0, 0, 3, 0, 0);
    slist.RemainingQuestionTime = new TimeSpan(0, 30, 0, 0, 0);
    slist.Status = 0;
    this.speakerlist = slist;
  }

  openModal(template: TemplateRef<any>) {
    this.modalRef = this.modalService.show(template);
  }

}
