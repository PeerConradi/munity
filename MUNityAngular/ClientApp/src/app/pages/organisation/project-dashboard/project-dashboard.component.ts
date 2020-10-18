import { Component, OnInit, TemplateRef } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { BsModalRef, BsModalService } from 'ngx-bootstrap';
import { Project } from 'src/app/models/conference/project.model';
import { ConferenceService } from 'src/app/services/conference-service.service';
import { OrganisationService } from 'src/app/services/organisation.service';

@Component({
  selector: 'app-project-dashboard',
  templateUrl: './project-dashboard.component.html',
  styleUrls: ['./project-dashboard.component.css']
})
export class ProjectDashboardComponent implements OnInit {


  private _project: Project;
  public get project(): Project {
    return this._project;
  }
  public set project(v: Project) {
    this._project = v;
  }

  modalRef: BsModalRef;
  newConfererenceForm: FormGroup;


  constructor(private route: ActivatedRoute, private orgaService: OrganisationService,
    private conferenceService: ConferenceService, private modalService: BsModalService,
    private formBuilder: FormBuilder) {
    this.newConfererenceForm = this.formBuilder.group({
      name: '',
      fullname: '',
      abbreviation: '',
      timespan: null
    });
  }

  openModal(template: TemplateRef<any>) {
    this.modalRef = this.modalService.show(template);
  }

  ngOnInit() {
    let id: string = null;
    this.route.params.subscribe(params => {
      id = params.id;
      console.log("Load project with id: " + id);
      if (id != null) {
        this.orgaService.getProjectWithConferences(id).subscribe(n => {
          console.log("result")
          console.log(n);
          this.project = n;
        })
      }
    });
  }

  createConference(data) {
    let name: string = data.name;
    let fullName: string = data.fullname;
    let abbreviation: string = data.abbreviation;
    let startDate: Date = data.timespan[0];
    let endDate: Date = data.timespan[1];
    this.conferenceService.createConference(this.project.projectId, name, fullName, abbreviation, startDate, endDate).subscribe(n => {
      if (n != null) {
        this.project.conferences.push(n);
        this.modalRef.hide();
      }

    });
  }

}
