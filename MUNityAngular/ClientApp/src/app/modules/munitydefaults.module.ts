import { NgModule } from "@angular/core";

import { FooterComponent } from "../pages/default/footer/footer.component";
import { CommonModule } from "@angular/common";
import { BrowserModule } from "@angular/platform-browser";
import { RouterModule } from "@angular/router";
import { NavMenuComponent } from "../nav-menu/nav-menu.component";

@NgModule({
    imports: [
        RouterModule,
        CommonModule
    ],
    declarations: [
        FooterComponent,
        NavMenuComponent
    ],
    exports: [
        FooterComponent,
        NavMenuComponent,
        RouterModule,
        CommonModule
    ],

}) export class MunityDefaultsModule { }