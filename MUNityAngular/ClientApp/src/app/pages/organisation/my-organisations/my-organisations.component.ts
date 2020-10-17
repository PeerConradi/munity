import { Component, OnInit } from '@angular/core';
import { Organisation } from 'src/app/models/orgnaisation.model';
import { OrganisationService } from 'src/app/services/organisation.service';

@Component({
  selector: 'app-my-organisations',
  templateUrl: './my-organisations.component.html',
  styleUrls: ['./my-organisations.component.css']
})
export class MyOrganisationsComponent implements OnInit {

  organisations: Organisation[];

  constructor(private orgaService: OrganisationService) {
    this.orgaService.getMyOrganisations().subscribe(n => this.organisations = n);
  }

  ngOnInit(): void {
  }

}
