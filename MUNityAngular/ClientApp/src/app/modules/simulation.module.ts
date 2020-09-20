import { NgModule } from "@angular/core";
import { SimulationMobileComponent } from '../pages/simulation/simulation-mobile/simulation-mobile.component';

import { TabsModule } from 'ngx-bootstrap/tabs';
import { ResViewComponent } from "../components/resedit/res-view/res-view.component";
import { JoinSimSimComponent } from "../components/simsim/join-sim-sim/join-sim-sim.component";
import { SimSimChatComponent } from "../components/simsim/sim-sim-chat/sim-sim-chat.component";
import { SimSimViewComponent } from "../components/simsim/sim-sim-view/sim-sim-view.component";
import { SimSimControlsComponent } from "../components/simsim/sim-sim-controls/sim-sim-controls.component";
import { SimSimDelegationComponent } from "../components/simsim/sim-sim-delegation/sim-sim-delegation.component";
import { SimSimStartupComponent } from "../components/simsim/sim-sim-startup/sim-sim-startup.component";
import { SimSimOverviewListComponent } from "../components/simsim/sim-sim-overview-list/sim-sim-overview-list.component";
import { SimSimLobbyElementComponent } from "../components/simsim/sim-sim-lobby-element/sim-sim-lobby-element.component";
import { SimSimCreateComponent } from "../components/simsim/sim-sim-create/sim-sim-create.component";
import { ResolutionModule } from "./resolution.module";
import { FormsModule, ReactiveFormsModule } from "@angular/forms";
import { MunityDefaultsModule } from "./munitydefaults.module";
import { RouterModule } from "@angular/router";
import { HammerModule } from "@angular/platform-browser";

// TODO
@NgModule({
    imports: [
        RouterModule,
        TabsModule.forRoot(),
        FormsModule,
        ReactiveFormsModule,
        ResolutionModule,
        HammerModule,
    ],
    declarations: [
        JoinSimSimComponent,
        SimSimChatComponent,
        SimSimViewComponent,
        SimSimControlsComponent,
        SimSimDelegationComponent,
        SimSimStartupComponent,
        SimSimOverviewListComponent,
        SimSimLobbyElementComponent,
        SimSimCreateComponent,
        SimulationMobileComponent],
    exports: [RouterModule]
})

export class SimulationModule {
    constructor() {

    }
}