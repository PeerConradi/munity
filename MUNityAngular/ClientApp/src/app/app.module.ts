import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { RouterModule } from '@angular/router';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { AppComponent } from './app.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { HomeComponent } from './home/home.component';
import { EditorComponent } from './components/resedit/editor/editor.component';
import { OperativeParagraphComponent } from './components/resedit/operative-paragraph/operative-paragraph.component';
import { PreambleParagraphComponent } from './components/resedit/preamble-paragraph/preamble-paragraph.component';
import { ResOptionsComponent } from './components/resedit/res-options/res-options.component';
import { AmendmentControllerComponent } from './components/resedit/amendment-controller/amendment-controller.component';
import { ResolutionHomeComponent } from './components/resedit/resolution-home/resolution-home.component';
import { CreateConferenceComponent } from './components/conference/create-conference/create-conference.component';
import { ConferenceListComponent } from './components/admin/conference-list/conference-list.component';
import { FooterComponent } from './components/default/footer/footer.component';
import { SignalrtestComponent } from './components/signalr/signalrtest/signalrtest.component';
import { ResViewComponent } from './components/resedit/res-view/res-view.component';
import { RegisterComponent } from './components/account/register/register.component';
import { LoginComponent } from './components/account/login/login.component';
import { LogoutComponent } from './components/account/logout/logout.component';
import { MyConferencesOverviewComponent } from './components/conference/my-conferences-overview/my-conferences-overview.component';
import { ConferenceDetailsComponent } from './components/conference/conference-details/conference-details.component';
import { MyresolutionsComponent } from './components/resedit/myresolutions/myresolutions.component';
import { EditConferenceComponent } from './components/conference/edit-conference/edit-conference.component';
import { MunityBoxComponent } from './components/components/munity-box/munity-box.component';
import { AllComponentsComponent } from './components/components/all-components/all-components.component';
import { NotifierModule } from "angular-notifier";
import { DefaultLayoutComponent } from './layouts/default-layout/default-layout.component';
import { AppRoutingModule } from './app-routing.module';
import { SpeakerlistStartupComponent } from './components/speakerlist/speakerlist-startup/speakerlist-startup.component';
import { SpeakerlistControllerComponent } from './components/speakerlist/speakerlist-controller/speakerlist-controller.component';
import { SpeakerlistViewComponent } from './components/speakerlist/speakerlist-view/speakerlist-view.component';

import { WorkWithResolutionComponent } from './components/resedit/work-with-resolution/work-with-resolution.component';
import { AccountSettingsComponent } from './components/account/account-settings/account-settings.component';
import { FontAwesomeModule, FaIconLibrary } from '@fortawesome/angular-fontawesome';
import { faFile, faFlag, faUser } from '@fortawesome/free-solid-svg-icons';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { AlertModule } from 'ngx-bootstrap/alert';
import { SortableModule } from 'ngx-bootstrap/sortable';
import { TypeaheadModule } from 'ngx-bootstrap/typeahead';
import { UserManagementComponent } from './components/admin/user-management/user-management.component';
import { ResolutionsManagementComponent } from './components/admin/resolutions-management/resolutions-management.component';
import { ModalModule } from 'ngx-bootstrap/modal';
import { ImpressumComponent } from './components/default/impressum/impressum.component';

import { MarkdownModule } from 'ngx-markdown';
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';
import { ExploreConferencesComponent } from './components/conference/explore-conferences/explore-conferences.component';
import { SpeakerlistPanelComponent } from './components/speakerlist/speakerlist-panel/speakerlist-panel.component';
import { PresentsCheckComponent } from './components/presents/presents-check/presents-check.component';
import { CommitteeDetailsComponent } from './components/conference/committee-details/committee-details.component';
import { DelegationDetailsComponent } from './components/conference/delegation-details/delegation-details.component';
import { BsDatepickerModule } from 'ngx-bootstrap/datepicker';
import { ProfileComponent } from './components/account/profile/profile.component';
import { TabsModule } from 'ngx-bootstrap/tabs';
import { ManageConferenceCommitteesComponent } from './components/conference/manage/manage-conference-committees/manage-conference-committees.component';
import { ManageConferenceTeamComponent } from './components/conference/manage/manage-conference-team/manage-conference-team.component';
import { ManageConferenceTeamRolesComponent } from './components/conference/manage/manage-conference-team-roles/manage-conference-team-roles.component';
import { ManageConferenceWebsiteComponent } from './components/conference/manage/manage-conference-website/manage-conference-website.component';
import { ManageConferenceDelegationsComponent } from './components/conference/manage/manage-conference-delegations/manage-conference-delegations.component';
import { AdminDashboardComponent } from './components/admin/admin-dashboard/admin-dashboard.component';


@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    HomeComponent,
    EditorComponent,
    //Todo: Auslagerung der ohnehin nur mit der EditorComponent genutzten Components in ein eigenes Module.
    OperativeParagraphComponent,
    PreambleParagraphComponent,
    ResOptionsComponent,
    AmendmentControllerComponent,
    ResolutionHomeComponent,
    CreateConferenceComponent,
    ConferenceListComponent,
    FooterComponent,
    SignalrtestComponent,
    ResViewComponent,
    RegisterComponent,
    LoginComponent,
    LogoutComponent,
    MyConferencesOverviewComponent,
    ConferenceDetailsComponent,
    MyresolutionsComponent,
    EditConferenceComponent,
    MunityBoxComponent,
    AllComponentsComponent,
    DefaultLayoutComponent,
    SpeakerlistStartupComponent,
    SpeakerlistControllerComponent,
    SpeakerlistViewComponent,
    WorkWithResolutionComponent,
    AccountSettingsComponent,
    UserManagementComponent,
    ResolutionsManagementComponent,
    ImpressumComponent,
    ExploreConferencesComponent,
    SpeakerlistPanelComponent,
    PresentsCheckComponent,
    CommitteeDetailsComponent,
    DelegationDetailsComponent,
    ProfileComponent,
    ManageConferenceCommitteesComponent,
    ManageConferenceTeamComponent,
    ManageConferenceTeamRolesComponent,
    ManageConferenceWebsiteComponent,
    ManageConferenceDelegationsComponent,
    AdminDashboardComponent
  ],
  imports: [
    RouterModule,
    NgbModule,
    FontAwesomeModule,
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule,
    NotifierModule,
    AppRoutingModule,
    BrowserAnimationsModule,
    AlertModule.forRoot(),
    SortableModule.forRoot(),
    TypeaheadModule.forRoot(),
    ModalModule.forRoot(),
    MarkdownModule.forRoot(),
    BsDropdownModule.forRoot(),
    BsDatepickerModule.forRoot(),
    TabsModule.forRoot()
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule {
  constructor(library: FaIconLibrary) {
    // Add an icon to the library for convenient access in other components
    library.addIcons(faFile, faFlag, faUser);

  }
}
