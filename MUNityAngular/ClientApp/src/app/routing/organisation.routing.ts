import { ModuleWithProviders } from "@angular/core"
import { RouterModule, Routes } from "@angular/router"
import { DefaultLayoutComponent } from "../layouts/default-layout/default-layout.component"
import { CreateOrganisationComponent } from "../pages/organisation/create-organisation/create-organisation.component"
import { MyOrganisationsComponent } from "../pages/organisation/my-organisations/my-organisations.component"
import { OrganisationDashboardComponent } from "../pages/organisation/organisation-dashboard/organisation-dashboard.component"
import { ProjectDashboardComponent } from "../pages/organisation/project-dashboard/project-dashboard.component"

export const routes: Routes = [

    {
        path: '',
        component: DefaultLayoutComponent,
        children: [
            //Konferenz
            { path: 'orga/create', component: CreateOrganisationComponent },
            { path: 'orga/myorgas', component: MyOrganisationsComponent },
            { path: 'orga/dashboard/:id', component: OrganisationDashboardComponent },
            { path: 'project/:id', component: ProjectDashboardComponent }
        ]

    },

]

export const OrganisationRouting: ModuleWithProviders<RouterModule> = RouterModule.forChild(routes)