import { ModuleWithProviders } from "@angular/core"
import { RouterModule, Routes } from "@angular/router"
import { DefaultLayoutComponent } from "../layouts/default-layout/default-layout.component"
import { CreateOrganisationComponent } from "../pages/organisation/create-organisation/create-organisation.component"
import { MyOrganisationsComponent } from "../pages/organisation/my-organisations/my-organisations.component"

export const routes: Routes = [

    {
        path: '',
        component: DefaultLayoutComponent,
        children: [
            //Konferenz
            { path: 'orga/create', component: CreateOrganisationComponent },
            { path: 'orga/myorgas', component: MyOrganisationsComponent }
        ]

    },

]

export const OrganisationRouting: ModuleWithProviders<RouterModule> = RouterModule.forChild(routes)