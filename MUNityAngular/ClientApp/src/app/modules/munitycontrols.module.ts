import { NgModule } from "@angular/core";
import { CommonModule } from "@angular/common";
import { XlabelComponent } from "../components/xlabel/xlabel.component";
import { MunityBoxComponent } from "../components/components/munity-box/munity-box.component";
import { MunityWindowComponent } from "../components/components/munity-window/munity-window.component";
import { NoticeComponent } from "../components/components/notice/notice.component";
import { RouterModule } from "@angular/router";

@NgModule({
    imports: [CommonModule,
        RouterModule],
    declarations: [
        XlabelComponent,
        MunityBoxComponent,
        MunityWindowComponent,
        NoticeComponent
    ],
    exports: [XlabelComponent,
        MunityBoxComponent,
        MunityWindowComponent,
        NoticeComponent]
}) export class MunityControlsModule { }