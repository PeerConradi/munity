import { BrowserModule, HammerGestureConfig, HammerModule, HAMMER_GESTURE_CONFIG } from '@angular/platform-browser';
import { NgModule, Injectable } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { RouterModule } from '@angular/router';
import { AppComponent } from './app.component';
import { HomeComponent } from './home/home.component';
import { ConferenceListComponent } from './components/admin/conference-list/conference-list.component';
import { SignalrtestComponent } from './components/signalr/signalrtest/signalrtest.component';
import { RegisterComponent } from './pages/account/register/register.component';
import { LoginComponent } from './pages/account/login/login.component';
import { LogoutComponent } from './pages/account/logout/logout.component';
//import { AllComponentsComponent } from './components/components/all-components/all-components.component';
import { NotifierModule } from "angular-notifier";
import { DefaultLayoutComponent } from './layouts/default-layout/default-layout.component';
import { AppRoutingModule } from './app-routing.module';
import { SpeakerlistStartupComponent } from './components/speakerlist/speakerlist-startup/speakerlist-startup.component';
import { SpeakerlistControllerComponent } from './components/speakerlist/speakerlist-controller/speakerlist-controller.component';
import { SpeakerlistViewComponent } from './components/speakerlist/speakerlist-view/speakerlist-view.component';

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

import { AuthInterceptor } from './interceptor/AuthInterceptor';
import { LoginFormComponent } from './components/account/login-form/login-form.component';
import { RegisterFormComponent } from './components/account/register-form/register-form.component';
import { ConferenceRouting } from './routing/conference.routing';
import { MunityControlsModule } from './modules/munitycontrols.module';
import { MunityDefaultsModule } from './modules/munitydefaults.module';
import { OperativeParagraphViewComponent } from './components/resolution/view/operative-paragraph-view/operative-paragraph-view.component';
import { SimulationModule } from './modules/simulation.module';
import { ResolutionModule } from './modules/resolution.module';


import * as Hammer from 'hammerjs';

// making hammer config (3)
@Injectable()
export class MyHammerConfig extends HammerGestureConfig {
  overrides = <any>{
    swipe: { direction: Hammer.DIRECTION_ALL },
  };
}

@NgModule({
  declarations: [
    AppComponent,
    HomeComponent,

    ConferenceListComponent,
    SignalrtestComponent,
    RegisterComponent,
    LoginComponent,
    LogoutComponent,
    //AllComponentsComponent,
    DefaultLayoutComponent,
    SpeakerlistStartupComponent,
    SpeakerlistControllerComponent,
    SpeakerlistViewComponent,
    AccountSettingsComponent,
    UserManagementComponent,
    ResolutionsManagementComponent,
    ImpressumComponent,
    SpeakerlistPanelComponent,
    PresentsCheckComponent,
    ProfileComponent,
    AdminDashboardComponent,
    PrivacyTermsComponent,
    LoginFormComponent,
    RegisterFormComponent
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
    HammerModule,
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
    ResolutionModule,
    ConferenceRouting,
    SimulationModule,
  ],
  providers: [
    {
      provide: HTTP_INTERCEPTORS,
      useClass: AuthInterceptor,
      multi: true
    },
    { provide: HAMMER_GESTURE_CONFIG, useClass: MyHammerConfig }
  ],
  bootstrap: [AppComponent]
})
export class AppModule {
  constructor(library: FaIconLibrary) {
    // Add an icon to the library for convenient access in other components
    library.addIcons(faFile, faFlag, faUser, faCircle);

  }
}
