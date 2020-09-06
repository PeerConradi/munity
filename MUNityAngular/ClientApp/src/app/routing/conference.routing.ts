import { Routes, RouterModule } from "@angular/router";
import { ManagerStartupComponent } from "../components/conference/manager-startup/manager-startup.component";
import { CreateConferenceComponent } from "../components/conference/create-conference/create-conference.component";
import { MyConferencesOverviewComponent } from "../components/conference/my-conferences-overview/my-conferences-overview.component";
import { CommitteeDetailsComponent } from "../components/conference/committee-details/committee-details.component";
import { ModuleWithProviders } from "@angular/core";
import { ExploreConferencesComponent } from "../components/conference/explore-conferences/explore-conferences.component";
import { DefaultLayoutComponent } from "../layouts/default-layout/default-layout.component";
import { ConferenceDetailsComponent } from "../components/conference/conference-details/conference-details.component";
import { ManageConferenceTeamComponent } from "../components/conference/manage/manage-conference-team/manage-conference-team.component";
import { ManageConferenceTeamRolesComponent } from "../components/conference/manage/manage-conference-team-roles/manage-conference-team-roles.component";
import { EditConferenceLayoutComponent } from "../layouts/edit-conference-layout/edit-conference-layout.component";

export const routes: Routes = [

    {
        path: '',
        component: DefaultLayoutComponent,
        children: [
            //Konferenz
            { path: 'cm/start', component: ManagerStartupComponent },
            { path: 'createconference', component: CreateConferenceComponent },
            { path: 'conference/my', component: MyConferencesOverviewComponent },

            { path: 'conferences/edit/:id', component: CreateConferenceComponent },
            { path: 'exploreconferences', component: ExploreConferencesComponent },
            { path: 'committee/:id', component: CommitteeDetailsComponent },
        ]

    },
    {
        path: '',
        component: EditConferenceLayoutComponent,
        children: [
            { path: 'mc/dashboard/:id', component: ConferenceDetailsComponent },
            { path: 'mc/general/:id', component: ConferenceDetailsComponent },
            // mc for manage conference
            { path: 'mc/overview/:id', component: CreateConferenceComponent },
            { path: 'mc/team/:id', component: ManageConferenceTeamComponent },
            { path: 'mc/TeamRoles/:id', component: ManageConferenceTeamRolesComponent },
            { path: 'mc/committees/:id', component: ManageConferenceTeamRolesComponent }
        ]
    },

]

export const ConferenceRouting: ModuleWithProviders = RouterModule.forChild(routes)