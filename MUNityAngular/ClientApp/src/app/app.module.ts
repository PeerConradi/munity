import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { RouterModule } from '@angular/router';
import { AppComponent } from './app.component';
import { HomeComponent } from './home/home.component';
import { EditorComponent } from './components/resedit/editor/editor.component';
import { OperativeParagraphComponent } from './components/resedit/operative-paragraph/operative-paragraph.component';
import { PreambleParagraphComponent } from './components/resedit/preamble-paragraph/preamble-paragraph.component';
import { ResOptionsComponent } from './components/resedit/res-options/res-options.component';
import { AmendmentControllerComponent } from './components/resedit/amendment-controller/amendment-controller.component';
import { ResolutionHomeComponent } from './components/resedit/resolution-home/resolution-home.component';
import { ConferenceListComponent } from './components/admin/conference-list/conference-list.component';
import { SignalrtestComponent } from './components/signalr/signalrtest/signalrtest.component';
import { ResViewComponent } from './components/resedit/res-view/res-view.component';
import { RegisterComponent } from './pages/account/register/register.component';
import { LoginComponent } from './pages/account/login/login.component';
import { LogoutComponent } from './pages/account/logout/logout.component';
import { MyresolutionsComponent } from './components/resedit/myresolutions/myresolutions.component';
//import { AllComponentsComponent } from './components/components/all-components/all-components.component';
import { NotifierModule } from "angular-notifier";
import { DefaultLayoutComponent } from './layouts/default-layout/default-layout.component';
import { AppRoutingModule } from './app-routing.module';
import { SpeakerlistStartupComponent } from './components/speakerlist/speakerlist-startup/speakerlist-startup.component';
import { SpeakerlistControllerComponent } from './components/speakerlist/speakerlist-controller/speakerlist-controller.component';
import { SpeakerlistViewComponent } from './components/speakerlist/speakerlist-view/speakerlist-view.component';

import { WorkWithResolutionComponent } from './components/resedit/work-with-resolution/work-with-resolution.component';
import { AccountSettingsComponent } from './components/account/account-settings/account-settings.component';
import { FontAwesomeModule, FaIconLibrary } from '@fortawesome/angular-fontawesome';
import { faFile, faFlag, faUser, faCircle } from '@fortawesome/free-solid-svg-icons';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { UserManagementComponent } from './components/admin/user-management/user-management.component';
import { ResolutionsManagementComponent } from './components/admin/resolutions-management/resolutions-management.component';
import { ImpressumComponent } from './pages/default/impressum/impressum.component';

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
import { SpeakerlistPanelComponent } from './components/speakerlist/speakerlist-panel/speakerlist-panel.component';
import { PresentsCheckComponent } from './components/presents/presents-check/presents-check.component';
import { ProfileComponent } from './components/account/profile/profile.component';
import { AdminDashboardComponent } from './components/admin/admin-dashboard/admin-dashboard.component';

import { NgxChartsModule } from '@swimlane/ngx-charts';
import { PrivacyTermsComponent } from './pages/default/privacy-terms/privacy-terms.component';

//SimSim
import { JoinSimSimComponent } from './components/simsim/join-sim-sim/join-sim-sim.component';
import { SimSimChatComponent } from './components/simsim/sim-sim-chat/sim-sim-chat.component';
import { SimSimViewComponent } from './components/simsim/sim-sim-view/sim-sim-view.component';
import { SimSimControlsComponent } from './components/simsim/sim-sim-controls/sim-sim-controls.component';
import { SimSimDelegationComponent } from './components/simsim/sim-sim-delegation/sim-sim-delegation.component';
import { SimSimStartupComponent } from './components/simsim/sim-sim-startup/sim-sim-startup.component';
import { SimSimOverviewListComponent } from './components/simsim/sim-sim-overview-list/sim-sim-overview-list.component';
import { SimSimLobbyElementComponent } from './components/simsim/sim-sim-lobby-element/sim-sim-lobby-element.component';
import { SimSimCreateComponent } from './components/simsim/sim-sim-create/sim-sim-create.component';

import { AuthInterceptor } from './interceptor/AuthInterceptor';
import { LoginFormComponent } from './components/account/login-form/login-form.component';
import { RegisterFormComponent } from './components/account/register-form/register-form.component';
import { ConferenceRouting } from './routing/conference.routing';
import { MunityControlsModule } from './modules/munitycontrols.module';
import { MunityDefaultsModule } from './modules/munitydefaults.module';
import { OperativeParagraphViewComponent } from './components/resolution/view/operative-paragraph-view/operative-paragraph-view.component';

@NgModule({
  declarations: [
    AppComponent,
    HomeComponent,
    EditorComponent,
    //Todo: Auslagerung der ohnehin nur mit der EditorComponent genutzten Components in ein eigenes Module.
    OperativeParagraphComponent,
    PreambleParagraphComponent,
    ResOptionsComponent,
    AmendmentControllerComponent,
    ResolutionHomeComponent,
    ConferenceListComponent,
    SignalrtestComponent,
    ResViewComponent,
    RegisterComponent,
    LoginComponent,
    LogoutComponent,
    MyresolutionsComponent,
    //AllComponentsComponent,
    DefaultLayoutComponent,
    SpeakerlistStartupComponent,
    SpeakerlistControllerComponent,
    SpeakerlistViewComponent,
    WorkWithResolutionComponent,
    AccountSettingsComponent,
    UserManagementComponent,
    ResolutionsManagementComponent,
    ImpressumComponent,
    SpeakerlistPanelComponent,
    PresentsCheckComponent,
    ProfileComponent,
    AdminDashboardComponent,
    PrivacyTermsComponent,
    JoinSimSimComponent,
    SimSimChatComponent,
    SimSimViewComponent,
    SimSimControlsComponent,
    SimSimDelegationComponent,
    SimSimStartupComponent,
    SimSimOverviewListComponent,
    SimSimLobbyElementComponent,
    SimSimCreateComponent,
    LoginFormComponent,
    RegisterFormComponent,
    OperativeParagraphViewComponent
  ],
  imports: [
    RouterModule,
    MunityDefaultsModule,
    FontAwesomeModule,
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule,
    NotifierModule,
    AppRoutingModule,
    BrowserAnimationsModule,
    MunityControlsModule,

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
    PopoverModule.forRoot(),
    ConferenceRouting,
  ],
  providers: [
    {
      provide: HTTP_INTERCEPTORS,
      useClass: AuthInterceptor,
      multi: true
    }
  ],
  bootstrap: [AppComponent]
})
export class AppModule {
  constructor(library: FaIconLibrary) {
    // Add an icon to the library for convenient access in other components
    library.addIcons(faFile, faFlag, faUser, faCircle);

  }
}
