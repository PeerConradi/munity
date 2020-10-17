import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { Organisation } from 'src/app/models/orgnaisation.model';
import { OrganisationService } from 'src/app/services/organisation.service';

@Component({
  selector: 'app-create-organisation-form',
  templateUrl: './create-organisation-form.component.html',
  styleUrls: ['./create-organisation-form.component.css']
})
export class CreateOrganisationFormComponent implements OnInit {

  createForm: FormGroup;
  error: boolean = false;
  createdOrganisation: Organisation;

  constructor(private organisationService: OrganisationService, private formBuilder: FormBuilder) {
    this.createForm = this.formBuilder.group({
      name: '',
      short: ''
    })
  }

  ngOnInit(): void {
  }

  async onSubmit(formData) {
    this.error = false;
    let result = await this.organisationService.createOrganisation(formData.name, formData.short);
    if (result != null) {
      var orga = await result.toPromise();
      if (orga != null) {
        this.createForm.reset();
        this.createdOrganisation = orga;
      }
    }
    else {
      this.error = true;
    }
  }

}
