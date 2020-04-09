import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { RouterModule } from '@angular/router';
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
import { UserManagementComponent } from './components/admin/user-management/user-management.component';
import { ResolutionsManagementComponent } from './components/admin/resolutions-management/resolutions-management.component';
import { ImpressumComponent } from './components/default/impressum/impressum.component';

//ngx-Boostrap
import { AlertModule } from 'ngx-bootstrap/alert';
import { SortableModule } from 'ngx-bootstrap/sortable';
import { TypeaheadModule } from 'ngx-bootstrap/typeahead';
import { PopoverModule } from 'ngx-bootstrap/popover';
import { ModalModule } from 'ngx-bootstrap/modal';
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';
import { TabsModule } from 'ngx-bootstrap/tabs';
import { TooltipModule } from 'ngx-bootstrap/tooltip';
import { BsDatepickerModule } from 'ngx-bootstrap/datepicker';
import { CollapseModule } from 'ngx-bootstrap/collapse';

import { MarkdownModule } from 'ngx-markdown';
import { ExploreConferencesComponent } from './components/conference/explore-conferences/explore-conferences.component';
import { SpeakerlistPanelComponent } from './components/speakerlist/speakerlist-panel/speakerlist-panel.component';
import { PresentsCheckComponent } from './components/presents/presents-check/presents-check.component';
import { CommitteeDetailsComponent } from './components/conference/committee-details/committee-details.component';
import { DelegationDetailsComponent } from './components/conference/delegation-details/delegation-details.component';
import { ProfileComponent } from './components/account/profile/profile.component';
import { ManageConferenceCommitteesComponent } from './components/conference/manage/manage-conference-committees/manage-conference-committees.component';
import { ManageConferenceTeamComponent } from './components/conference/manage/manage-conference-team/manage-conference-team.component';
import { ManageConferenceTeamRolesComponent } from './components/conference/manage/manage-conference-team-roles/manage-conference-team-roles.component';
import { ManageConferenceWebsiteComponent } from './components/conference/manage/manage-conference-website/manage-conference-website.component';
import { ManageConferenceDelegationsComponent } from './components/conference/manage/manage-conference-delegations/manage-conference-delegations.component';
import { AdminDashboardComponent } from './components/admin/admin-dashboard/admin-dashboard.component';
import { MunityWindowComponent } from './components/components/munity-window/munity-window.component';
import { NoticeComponent } from './components/components/notice/notice.component';

import { NgxChartsModule } from '@swimlane/ngx-charts';
import { PrivacyTermsComponent } from './components/default/privacy-terms/privacy-terms.component';
import { CommitteeCardComponent } from './components/conference/committee-card/committee-card.component';
import { ConferenceMenuComponent } from './components/conference/conference-menu/conference-menu.component';
import { EditConferenceLayoutComponent } from './layouts/edit-conference-layout/edit-conference-layout.component';
import { ConferenceOptionsComponent } from './components/conference/conference-options/conference-options.component';

// X-Controls
import { XlabelComponent } from './components/xlabel/xlabel.component';

//SimSim
import { CreateSimSimComponent } from './components/simsim/create-sim-sim/create-sim-sim.component';
import { JoinSimSimComponent } from './components/simsim/join-sim-sim/join-sim-sim.component';
import { SimSimChatComponent } from './components/simsim/sim-sim-chat/sim-sim-chat.component';
import { SimSimViewComponent } from './components/simsim/sim-sim-view/sim-sim-view.component';
import { SimSimControlsComponent } from './components/simsim/sim-sim-controls/sim-sim-controls.component';
import { SimSimDelegationComponent } from './components/simsim/sim-sim-delegation/sim-sim-delegation.component';
import { SimSimStartupComponent } from './components/simsim/sim-sim-startup/sim-sim-startup.component';
import { SimSimOverviewListComponent } from './components/simsim/sim-sim-overview-list/sim-sim-overview-list.component';
import { SimSimLobbyElementComponent } from './components/simsim/sim-sim-lobby-element/sim-sim-lobby-element.component';
import { SimSimCreateComponent } from './components/simsim/sim-sim-create/sim-sim-create.component'

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
    AdminDashboardComponent,
    MunityWindowComponent,
    NoticeComponent,
    PrivacyTermsComponent,
    CommitteeCardComponent,
    ConferenceMenuComponent,
    EditConferenceLayoutComponent,
    ConferenceOptionsComponent,
    XlabelComponent,
    CreateSimSimComponent,
    JoinSimSimComponent,
    SimSimChatComponent,
    SimSimViewComponent,
    SimSimControlsComponent,
    SimSimDelegationComponent,
    SimSimStartupComponent,
    SimSimOverviewListComponent,
    SimSimLobbyElementComponent,
    SimSimCreateComponent
  ],
  imports: [
    RouterModule,
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
    TabsModule.forRoot(),
    NgxChartsModule,
    TooltipModule.forRoot(),
    CollapseModule.forRoot(),
    PopoverModule.forRoot()
    
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
