import { Component, OnInit, TemplateRef } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { idLocale } from 'ngx-bootstrap';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { Organisation } from 'src/app/models/orgnaisation.model';
import { ConferenceService } from 'src/app/services/conference-service.service';
import { OrganisationService } from 'src/app/services/organisation.service';

@Component({
  selector: 'app-organisation-dashboard',
  templateUrl: './organisation-dashboard.component.html',
  styleUrls: ['./organisation-dashboard.component.css']
})
export class OrganisationDashboardComponent implements OnInit {

  organisation: Organisation;
  modalRef: BsModalRef;
  newProjectForm: FormGroup;
  errorCreatingProject: boolean = false;

  constructor(private route: ActivatedRoute, private orgaService: OrganisationService,
    private conferenceService: ConferenceService, private modalService: BsModalService,
    private formBuilder: FormBuilder) {
    this.newProjectForm = this.formBuilder.group({
      name: '',
      short: ''
    })
  }

  openModal(template: TemplateRef<any>) {
    this.modalRef = this.modalService.show(template);
  }

  async ngOnInit() {
    let id: string = null;
    this.route.params.subscribe(params => {
      id = params.id;
    });

    if (id != null) {
      this.organisation = await this.orgaService.getOrganisation(id).toPromise();
      // Load the projects in the background
      this.orgaService.getProjectsOfOrganisation(this.organisation.organisationId).subscribe(n => {
        this.organisation.projects = n;
      });
    }
  }

  async createProject(data) {
    console.log('Erstelle Projekt');
    console.log(data)
    this.errorCreatingProject = false;
    if (data == null)
      return;
    var name: string = data.name;
    var short: string = data.short;
    if (name == null || name == '' || short == null || short == '')
      return;

    var project = await this.conferenceService.createProject(this.organisation.organisationId, name, short).toPromise()

    if (project == null)
      this.errorCreatingProject = true;

    if (this.organisation.projects == null)
      this.organisation.projects = [];

    this.organisation.projects.push(project);
    this.modalRef.hide();
  }
}
