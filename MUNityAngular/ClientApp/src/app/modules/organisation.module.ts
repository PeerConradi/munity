import { CommonModule } from "@angular/common";
import { NgModule, ModuleWithProviders } from "@angular/core";
import { FormsModule, ReactiveFormsModule } from "@angular/forms";
import { FontAwesomeModule } from "@fortawesome/angular-fontawesome";
import { BsDatepickerModule, ModalModule, PopoverModule, TypeaheadModule } from "ngx-bootstrap";
import { MarkdownModule } from "ngx-markdown";
import { MunityControlsModule } from "./munitycontrols.module";
import { MunityDefaultsModule } from "./munitydefaults.module";
import { CreateOrganisationComponent } from '../pages/organisation/create-organisation/create-organisation.component';
import { CreateOrganisationFormComponent } from '../components/organisation/create-organisation-form/create-organisation-form.component';
import { MyOrganisationsComponent } from '../pages/organisation/my-organisations/my-organisations.component';
import { RouterModule } from "@angular/router";
import { OrganisationDashboardComponent } from '../pages/organisation/organisation-dashboard/organisation-dashboard.component';
import { ProjectDashboardComponent } from '../pages/organisation/project-dashboard/project-dashboard.component';

@NgModule({
  imports: [
    CommonModule,
    RouterModule,
    FormsModule,
    ReactiveFormsModule,
    //BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    MarkdownModule.forRoot(),
    MunityControlsModule,
    MunityDefaultsModule,
    FontAwesomeModule,
    ModalModule,
    TypeaheadModule,
    PopoverModule,
    BsDatepickerModule,
  ],
  declarations: [
    CreateOrganisationFormComponent,
    CreateOrganisationComponent,
    MyOrganisationsComponent,
    OrganisationDashboardComponent,
    ProjectDashboardComponent
  ],
  exports: [
    CreateOrganisationFormComponent,
    CreateOrganisationComponent,
    MyOrganisationsComponent,
    OrganisationDashboardComponent

  ]
})

export class OrganisationModule {

}
