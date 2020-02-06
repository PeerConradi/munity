import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Routes } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { CounterComponent } from './counter/counter.component';
import { FetchDataComponent } from './fetch-data/fetch-data.component';
import { EditorComponent } from './components/resedit/editor/editor.component';
import { MyresolutionsComponent } from './components/resedit/myresolutions/myresolutions.component';
import { ResolutionHomeComponent } from './components/resedit/resolution-home/resolution-home.component';
import { DashboardComponent } from './loggedIn/dashboard/dashboard.component';
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

const routes: Routes = [
  { path: 'login', component: LoginComponent },
  { path: 'resa/read/:id', component: ResViewComponent },
  {
    path: '',
    component: DefaultLayoutComponent,
    children: [
      { path: '', component: HomeComponent, pathMatch: 'full' },
      { path: 'impressum', component: ImpressumComponent},

      //Example
      { path: 'counter', component: CounterComponent },
      { path: 'fetch-data', component: FetchDataComponent },
      { path: 'components', component: AllComponentsComponent },
      { path: 'test/signalr', component: SignalrtestComponent },

      //Resolutionen
      { path: 'resedit/:id', component: EditorComponent },
      { path: 'resa/live/:id', component: WorkWithResolutionComponent },
      { path: 'mydocs', component: MyresolutionsComponent },
      { path: 'reshome', component: ResolutionHomeComponent },

      //User Special Areas
      { path: 'dashboard', component: DashboardComponent },
      
      //Account
      { path: 'register', component: RegisterComponent },
      { path: 'logout', component: LogoutComponent },
      { path: 'account/settings', component: AccountSettingsComponent },
      //// Login is a custom form and not in this layout, register could also be moved someday

      //Konferenz
      { path: 'conference/create', component: CreateConferenceComponent },
      { path: 'conference/my', component: MyConferencesOverviewComponent },
      { path: 'conferences/:id', component: ConferenceDetailsComponent },
      { path: 'conferences/edit/:id', component: EditConferenceComponent },

      //Admin
      { path: 'admin/conferences', component: ConferenceListComponent },
      { path: 'admin/users', component: UserManagementComponent },

      //Speakerlist
      { path: 's/start', component: SpeakerlistStartupComponent },
      { path: 's/edit/:id', component: SpeakerlistControllerComponent },
      { path: 's/view/:id', component: SpeakerlistViewComponent }
    ]
  }

]

@NgModule({
  declarations: [],
  imports: [
    CommonModule, RouterModule.forRoot(routes)
  ],
  exports: [RouterModule],
})
export class AppRoutingModule { }
