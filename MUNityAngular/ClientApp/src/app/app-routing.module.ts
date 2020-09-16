import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Routes } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { EditorComponent } from './components/resedit/editor/editor.component';
import { MyresolutionsComponent } from './components/resedit/myresolutions/myresolutions.component';
import { ResolutionHomeComponent } from './components/resedit/resolution-home/resolution-home.component';
import { ConferenceListComponent } from './components/admin/conference-list/conference-list.component';
import { SignalrtestComponent } from './components/signalr/signalrtest/signalrtest.component';
import { RegisterComponent } from './pages/account/register/register.component';
import { LoginComponent } from './pages/account/login/login.component';
import { LogoutComponent } from './pages/account/logout/logout.component';
//import { AllComponentsComponent } from './components/components/all-components/all-components.component';
import { ResViewComponent } from './components/resedit/res-view/res-view.component';
import { DefaultLayoutComponent } from './layouts/default-layout/default-layout.component';
import { SpeakerlistStartupComponent } from './components/speakerlist/speakerlist-startup/speakerlist-startup.component';
import { SpeakerlistControllerComponent } from './components/speakerlist/speakerlist-controller/speakerlist-controller.component';
import { SpeakerlistViewComponent } from './components/speakerlist/speakerlist-view/speakerlist-view.component';
import { WorkWithResolutionComponent } from './components/resedit/work-with-resolution/work-with-resolution.component';
import { AccountSettingsComponent } from './components/account/account-settings/account-settings.component';
import { UserManagementComponent } from './components/admin/user-management/user-management.component';
import { ImpressumComponent } from './pages/default/impressum/impressum.component';
import { PresentsCheckComponent } from './components/presents/presents-check/presents-check.component';
import { ProfileComponent } from './components/account/profile/profile.component';
import { AdminDashboardComponent } from './components/admin/admin-dashboard/admin-dashboard.component';
import { ResolutionsManagementComponent } from './components/admin/resolutions-management/resolutions-management.component';
import { PrivacyTermsComponent } from './pages/default/privacy-terms/privacy-terms.component';
import { SimSimViewComponent } from './components/simsim/sim-sim-view/sim-sim-view.component';
import { SimSimStartupComponent } from './components/simsim/sim-sim-startup/sim-sim-startup.component';
import { SimSimCreateComponent } from './components/simsim/sim-sim-create/sim-sim-create.component';

import { AuthGuard } from "./shared/auth.guard";
import { SimulationMobileComponent } from './pages/simulation/simulation-mobile/simulation-mobile.component';

const routes: Routes = [
  // Routes that have no default theme!

  { path: 'resa/read/:id', component: ResViewComponent },
  { path: 's/view/:id', component: SpeakerlistViewComponent },
  { path: 'sim', component: SimSimViewComponent },
  { path: 'msim', component: SimulationMobileComponent },
  {
    path: '',
    component: DefaultLayoutComponent,
    children: [
      { path: '', component: HomeComponent, pathMatch: 'full' },
      { path: 'impressum', component: ImpressumComponent },
      { path: 'privacy', component: PrivacyTermsComponent },

      //Example
      //{ path: 'components', component: AllComponentsComponent },
      { path: 'test/signalr', component: SignalrtestComponent },

      //Resolutionen
      { path: 'resedit/:id', component: EditorComponent },
      { path: 'resa/live/:id', component: WorkWithResolutionComponent },
      { path: 'mydocs', component: MyresolutionsComponent },
      { path: 'reshome', component: ResolutionHomeComponent },

      //Account
      { path: 'login', component: LoginComponent },
      { path: 'register', component: RegisterComponent },
      { path: 'logout', component: LogoutComponent },
      { path: 'account/settings', component: AccountSettingsComponent, canActivate: [AuthGuard] },
      { path: 'u/:id', component: ProfileComponent },

      //Admin
      { path: 'admin/dashboard', component: AdminDashboardComponent },
      { path: 'admin/conferences', component: ConferenceListComponent },
      { path: 'admin/users', component: UserManagementComponent },
      { path: 'admin/resolutions', component: ResolutionsManagementComponent },

      //Speakerlist
      { path: 's/start', component: SpeakerlistStartupComponent },
      { path: 's/edit/:id', component: SpeakerlistControllerComponent },

      { path: 'p/check/:id', component: PresentsCheckComponent },

      // SimSim
      { path: 'simulator', component: SimSimStartupComponent },
      { path: 'simulator/create', component: SimSimCreateComponent }
    ],

  },
  {
    path: 'conference',
    loadChildren: () => import('./modules/conference.module').then(m => m.ConferenceModule)
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
