import { CommonModule } from "@angular/common";
import { NgModule } from "@angular/core";
import { FormsModule, ReactiveFormsModule } from "@angular/forms";
import { FontAwesomeModule } from "@fortawesome/angular-fontawesome";
import { ModalModule, PopoverModule, TypeaheadModule } from "ngx-bootstrap";
import { MarkdownModule } from "ngx-markdown";
import { MunityControlsModule } from "./munitycontrols.module";
import { MunityDefaultsModule } from "./munitydefaults.module";
import { CreateOrganisationComponent } from '../pages/organisation/create-organisation/create-organisation.component';
import { CreateOrganisationFormComponent } from '../components/organisation/create-organisation-form/create-organisation-form.component';
import { MyOrganisationsComponent } from '../pages/organisation/my-organisations/my-organisations.component';
import { RouterModule } from "@angular/router";

@NgModule({
    imports: [

        CommonModule,
        //RouterModule,
        FormsModule,
        ReactiveFormsModule,
        //BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
        MarkdownModule.forRoot(),
        MunityDefaultsModule, ,
        MunityControlsModule,
        FontAwesomeModule,
        ModalModule,
        TypeaheadModule,
        PopoverModule
    ],
    declarations: [
        CreateOrganisationComponent,
        CreateOrganisationFormComponent,
        MyOrganisationsComponent],
    exports: [CreateOrganisationComponent,
        CreateOrganisationFormComponent]
})

export class OrganisationModule {
    constructor() {

    }
}