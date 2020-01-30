import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { RouterModule } from '@angular/router';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { AppComponent } from './app.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { HomeComponent } from './home/home.component';
import { CounterComponent } from './counter/counter.component';
import { FetchDataComponent } from './fetch-data/fetch-data.component';
import { EditorComponent } from './components/resedit/editor/editor.component';
import { OperativeParagraphComponent } from './components/resedit/operative-paragraph/operative-paragraph.component';
import { PreambleParagraphComponent } from './components/resedit/preamble-paragraph/preamble-paragraph.component';
import { ResOptionsComponent } from './components/resedit/res-options/res-options.component';
import { AmendmentControllerComponent } from './components/resedit/amendment-controller/amendment-controller.component';
import { ResolutionHomeComponent } from './components/resedit/resolution-home/resolution-home.component';
import { SecondaryNavComponent } from './secondary-nav/secondary-nav.component';
import { DashboardComponent } from './loggedIn/dashboard/dashboard.component';
import { ConferenceBoxComponent } from './loggedIn/conference-box/conference-box.component';
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

import { NgxSortableModule } from 'ngx-sortable';
import { WorkWithResolutionComponent } from './components/resedit/work-with-resolution/work-with-resolution.component'

@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    HomeComponent,
    CounterComponent,
    FetchDataComponent,
    EditorComponent,
    //Todo: Auslagerung der ohnehin nur mit der EditorComponent genutzten Components in ein eigenes Module.
    OperativeParagraphComponent,
    PreambleParagraphComponent,
    ResOptionsComponent,
    AmendmentControllerComponent,
    ResolutionHomeComponent,
    SecondaryNavComponent,
    DashboardComponent,
    ConferenceBoxComponent,
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
    WorkWithResolutionComponent
  ],
  imports: [
    RouterModule,
    NgbModule,
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule,
    NotifierModule,
    AppRoutingModule,
    NgxSortableModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
