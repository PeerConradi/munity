import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Routes } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { EditorComponent } from './components/resedit/editor/editor.component';
import { MyresolutionsComponent } from './components/resedit/myresolutions/myresolutions.component';
import { ResolutionHomeComponent } from './components/resedit/resolution-home/resolution-home.component';
import { CreateConferenceComponent } from './components/conference/create-conference/create-conference.component';
import { ConferenceListComponent } from './components/admin/conference-list/conference-list.component';
import { SignalrtestComponent } from './components/signalr/signalrtest/signalrtest.component';
import { RegisterComponent } from './components/account/register/register.component';
import { LoginComponent } from './components/account/login/login.component';
import { LogoutComponent } from './components/account/logout/logout.component';
import { MyConferencesOverviewComponent } from './components/conference/my-conferences-overview/my-conferences-overview.component';
import { EditConferenceComponent } from './components/conference/edit-conference/edit-conference.component';
import { ConferenceDetailsComponent } from './components/conference/conference-details/conference-details.component';
import { AllComponentsComponent } from './components/components/all-components/all-components.component';
import { ResViewComponent } from './components/resedit/res-view/res-view.component';
import { DefaultLayoutComponent } from './layouts/default-layout/default-layout.component';
import { SpeakerlistStartupComponent } from './components/speakerlist/speakerlist-startup/speakerlist-startup.component';
import { SpeakerlistControllerComponent } from './components/speakerlist/speakerlist-controller/speakerlist-controller.component';
import { SpeakerlistViewComponent } from './components/speakerlist/speakerlist-view/speakerlist-view.component';
import { WorkWithResolutionComponent } from './components/resedit/work-with-resolution/work-with-resolution.component';
import { AccountSettingsComponent } from './components/account/account-settings/account-settings.component';
import { UserManagementComponent } from './components/admin/user-management/user-management.component';
import { ImpressumComponent } from './components/default/impressum/impressum.component';
import { ExploreConferencesComponent } from './components/conference/explore-conferences/explore-conferences.component';
import { PresentsCheckComponent } from './components/presents/presents-check/presents-check.component';
import { ProfileComponent } from './components/account/profile/profile.component';
import { ManageConferenceTeamComponent } from './components/conference/manage/manage-conference-team/manage-conference-team.component';
import { ManageConferenceCommitteesComponent } from './components/conference/manage/manage-conference-committees/manage-conference-committees.component';
import { ManageConferenceTeamRolesComponent } from './components/conference/manage/manage-conference-team-roles/manage-conference-team-roles.component';
import { AdminDashboardComponent } from './components/admin/admin-dashboard/admin-dashboard.component';
import { ResolutionsManagementComponent } from './components/admin/resolutions-management/resolutions-management.component';
import { CommitteeDetailsComponent } from './components/conference/committee-details/committee-details.component';
import { PrivacyTermsComponent } from './components/default/privacy-terms/privacy-terms.component';
import { EditConferenceLayoutComponent } from './layouts/edit-conference-layout/edit-conference-layout.component';
import { ConferenceOptionsComponent } from './components/conference/conference-options/conference-options.component';

const routes: Routes = [
  { path: 'login', component: LoginComponent },
  { path: 'resa/read/:id', component: ResViewComponent },
  { path: 's/view/:id', component: SpeakerlistViewComponent },
  {
    path: '',
    component: DefaultLayoutComponent,
    children: [
      { path: '', component: HomeComponent, pathMatch: 'full' },
      { path: 'impressum', component: ImpressumComponent },
      { path: 'privacy', component: PrivacyTermsComponent },

      //Example
      { path: 'components', component: AllComponentsComponent },
      { path: 'test/signalr', component: SignalrtestComponent },

      //Resolutionen
      { path: 'resedit/:id', component: EditorComponent },
      { path: 'resa/live/:id', component: WorkWithResolutionComponent },
      { path: 'mydocs', component: MyresolutionsComponent },
      { path: 'reshome', component: ResolutionHomeComponent },
      
      //Account
      { path: 'register', component: RegisterComponent },
      { path: 'logout', component: LogoutComponent },
      { path: 'account/settings', component: AccountSettingsComponent },
      { path: 'u/:id', component: ProfileComponent },

      //// Login is a custom form and not in this layout, register could also be moved someday

      //Konferenz
      { path: 'createconference', component: CreateConferenceComponent },
      { path: 'conference/my', component: MyConferencesOverviewComponent },
      
      { path: 'conferences/edit/:id', component: EditConferenceComponent },
      { path: 'exploreconferences', component: ExploreConferencesComponent },
      { path: 'committee/:id', component: CommitteeDetailsComponent },
      

      //Admin
      { path: 'admin/dashboard', component: AdminDashboardComponent },
      { path: 'admin/conferences', component: ConferenceListComponent },
      { path: 'admin/users', component: UserManagementComponent },
      { path: 'admin/resolutions', component: ResolutionsManagementComponent },

      //Speakerlist
      { path: 's/start', component: SpeakerlistStartupComponent },
      { path: 's/edit/:id', component: SpeakerlistControllerComponent },

      { path: 'p/check/:id', component: PresentsCheckComponent }
      
    ]
  },
  {
    path: '',
    component: EditConferenceLayoutComponent,
    children: [
      { path: 'mc/dashboard/:id', component: ConferenceDetailsComponent },
      { path: 'mc/general/:id', component: ConferenceOptionsComponent },
      // mc for manage conference
      { path: 'mc/overview/:id', component: EditConferenceComponent },
      { path: 'mc/team/:id', component: ManageConferenceTeamComponent },
      { path: 'mc/TeamRoles/:id', component: ManageConferenceTeamRolesComponent },
      { path: 'mc/committees/:id', component: ManageConferenceCommitteesComponent }
    ]
  },

]

@NgModule({
  declarations: [],
  imports: [
    CommonModule, RouterModule.forRoot(routes)
  ],
  exports: [RouterModule],
})
export class AppRoutingModule { }
