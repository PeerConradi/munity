import { NgModel, FormsModule, ReactiveFormsModule } from "@angular/forms";
import { NgModule } from "@angular/core";
import { CommitteeCardComponent } from "../components/conference/committee-card/committee-card.component";
import { CommitteeDetailsComponent } from "../components/conference/committee-details/committee-details.component";
import { ConferenceDetailsComponent } from "../components/conference/conference-details/conference-details.component";
import { ConferenceMenuComponent } from "../components/conference/conference-menu/conference-menu.component";
import { ConferenceOptionsComponent } from "../components/conference/conference-options/conference-options.component";
import { CreateConferenceComponent } from "../components/conference/create-conference/create-conference.component";
import { DelegationDetailsComponent } from "../components/conference/delegation-details/delegation-details.component";
import { EditConferenceComponent } from "../components/conference/edit-conference/edit-conference.component";
import { ExploreConferencesComponent } from "../components/conference/explore-conferences/explore-conferences.component";
import { ManageConferenceCommitteesComponent } from "../pages/conference/manage/manage-conference-committees/manage-conference-committees.component";
import { ManageConferenceDelegationsComponent } from "../pages/conference/manage/manage-conference-delegations/manage-conference-delegations.component";
import { ManageConferenceTeamComponent } from "../components/conference/manage/manage-conference-team/manage-conference-team.component";
import { ManageConferenceTeamRolesComponent } from "../components/conference/manage/manage-conference-team-roles/manage-conference-team-roles.component";
import { ManageConferenceWebsiteComponent } from "../components/conference/manage/manage-conference-website/manage-conference-website.component";
import { ManagerStartupComponent } from "../components/conference/manager-startup/manager-startup.component";
import { MyConferencesOverviewComponent } from "../components/conference/my-conferences-overview/my-conferences-overview.component";
import { CommonModule } from "@angular/common";
import { ConferenceRouting } from "../routing/conference.routing";
import { BrowserModule } from "@angular/platform-browser";
import { MarkdownModule } from "ngx-markdown";
import { MunityControlsModule } from "./munitycontrols.module";
import { EditConferenceLayoutComponent } from "../layouts/edit-conference-layout/edit-conference-layout.component";
import { MunityDefaultsModule } from "./munitydefaults.module";
import { ManageConferenceDashboardComponent } from '../pages/conference/manage/manage-conference-dashboard/manage-conference-dashboard.component';
import { FontAwesomeModule, FaIconLibrary } from "@fortawesome/angular-fontawesome";

import { faFile, faFlag, faUser, faCircle, fas } from '@fortawesome/free-solid-svg-icons';
import { ConferenceTodoListWidgetComponent } from '../components/conference/manage/conference-todo-list-widget/conference-todo-list-widget.component';
import { CommitteeListComponent } from '../components/conference/committee-list/committee-list.component';

@NgModule({
    imports: [
        CommonModule,
        FormsModule,
        ReactiveFormsModule,
        //BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
        MarkdownModule.forRoot(),
        MunityControlsModule,
        MunityDefaultsModule,
        FontAwesomeModule,
        ConferenceRouting,
    ],
    declarations: [
        ConferenceMenuComponent,
        CommitteeCardComponent,
        CommitteeDetailsComponent,
        ConferenceDetailsComponent,
        ConferenceOptionsComponent,
        CreateConferenceComponent,
        DelegationDetailsComponent,
        EditConferenceComponent,
        ExploreConferencesComponent,
        ManageConferenceCommitteesComponent,
        ManageConferenceDelegationsComponent,
        ManageConferenceTeamComponent,
        ManageConferenceTeamRolesComponent,
        ManageConferenceWebsiteComponent,
        ManagerStartupComponent,
        MyConferencesOverviewComponent,
        EditConferenceLayoutComponent,
        ManageConferenceDashboardComponent,
        ConferenceTodoListWidgetComponent,
        CommitteeListComponent
    ],
    exports: [ConferenceMenuComponent]
})
export class ConferenceModule {
    constructor(library: FaIconLibrary) {
        // Add an icon to the library for convenient access in other components
        library.addIconPacks(fas);
        library.addIcons(faFile, faFlag, faUser, faCircle);

    }
}