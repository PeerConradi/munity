import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { RouterModule } from '@angular/router';

import { AppComponent } from './app.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { HomeComponent } from './home/home.component';
import { CounterComponent } from './counter/counter.component';
import { FetchDataComponent } from './fetch-data/fetch-data.component';
import { EditorComponent } from './resedit/editor/editor.component';
import { OperativeParagraphComponent } from './resedit/operative-paragraph/operative-paragraph.component';
import { PreambleParagraphComponent } from './resedit/preamble-paragraph/preamble-paragraph.component';
import { ResOptionsComponent } from './resedit/res-options/res-options.component';
import { AmendmentControllerComponent } from './resedit/amendment-controller/amendment-controller.component';
import { ResolutionHomeComponent } from './resedit/resolution-home/resolution-home.component';
import { SecondaryNavComponent } from './secondary-nav/secondary-nav.component';
import { DashboardComponent } from './loggedIn/dashboard/dashboard.component';
import { ConferenceBoxComponent } from './loggedIn/conference-box/conference-box.component';
import { CreateConferenceComponent } from './loggedIn/conference/create-conference/create-conference.component';

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
    CreateConferenceComponent
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    RouterModule.forRoot([
      { path: '', component: HomeComponent, pathMatch: 'full' },
      { path: 'counter', component: CounterComponent },
      { path: 'fetch-data', component: FetchDataComponent },
      { path: 'resedit', component: EditorComponent },
      { path: 'reshome', component: ResolutionHomeComponent },
      { path: 'dashboard', component: DashboardComponent },
      { path: 'conference/create', component: CreateConferenceComponent }
    ])
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
