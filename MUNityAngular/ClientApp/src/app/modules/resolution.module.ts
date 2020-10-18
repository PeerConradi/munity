import { SimulationMobileComponent } from '../pages/simulation/simulation-mobile/simulation-mobile.component';

import { TabsModule } from 'ngx-bootstrap/tabs';
import { ResViewComponent } from "../components/resedit/res-view/res-view.component";
import { NgModule } from '@angular/core';
import { AmendmentControllerComponent } from '../components/resedit/amendment-controller/amendment-controller.component';
import { EditorComponent } from '../components/resedit/editor/editor.component';
import { MyresolutionsComponent } from '../components/resedit/myresolutions/myresolutions.component';
import { OperativeParagraphComponent } from '../components/resedit/operative-paragraph/operative-paragraph.component';
import { PreambleParagraphComponent } from '../components/resedit/preamble-paragraph/preamble-paragraph.component';
import { ResOptionsComponent } from '../components/resedit/res-options/res-options.component';
import { ResolutionHomeComponent } from '../components/resedit/resolution-home/resolution-home.component';
import { WorkWithResolutionComponent } from '../components/resedit/work-with-resolution/work-with-resolution.component';
import { OperativeParagraphViewComponent } from '../components/resolution/view/operative-paragraph-view/operative-paragraph-view.component';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MarkdownModule } from 'ngx-markdown';
import { MunityControlsModule } from './munitycontrols.module';
import { MunityDefaultsModule } from './munitydefaults.module';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';

// ng-bootstrap
import { ModalModule } from 'ngx-bootstrap/modal';
import { TypeaheadModule } from 'ngx-bootstrap/typeahead';
import { PopoverModule } from 'ngx-bootstrap/popover';
import { BrowserModule } from '@angular/platform-browser';
import { BsDropdownModule } from 'ngx-bootstrap';

@NgModule({
    imports: [
        CommonModule,
        //BrowserModule,
        FormsModule,
        ReactiveFormsModule,
        BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
        MarkdownModule.forRoot(),
        MunityControlsModule,
        MunityDefaultsModule,
        FontAwesomeModule,
        ModalModule,
        TypeaheadModule,
        PopoverModule,
        BsDropdownModule
    ],
    declarations: [
        ResViewComponent,
        AmendmentControllerComponent,
        EditorComponent,
        MyresolutionsComponent,
        OperativeParagraphComponent,
        PreambleParagraphComponent,
        ResOptionsComponent,
        ResolutionHomeComponent,
        WorkWithResolutionComponent,
        OperativeParagraphViewComponent
    ],
    exports: [ResViewComponent]
})

export class ResolutionModule {
    constructor() {

    }
}
