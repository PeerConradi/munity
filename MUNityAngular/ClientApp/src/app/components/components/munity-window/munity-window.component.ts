import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';

@Component({
  selector: 'app-munity-window',
  templateUrl: './munity-window.component.html',
  styleUrls: ['./munity-window.component.css']
})
export class MunityWindowComponent implements OnInit {
  @Input() leftVal: number = 20;
  @Input() topVal: number = 20;

  @Input() title: string;

  @Output() onClose: EventEmitter<any> = new EventEmitter<any>();

  isMoving: boolean = false;
  startMoveMousePositionX: number;
  startMoveMousePositionY: number;

  constructor() { }

  ngOnInit() {
  }

  windowMouseDown(val) {
    this.startMoveMousePositionX = val.offsetX;
    this.startMoveMousePositionY = val.offsetY;
    this.isMoving = true;
  }

  windowMouseMove(val) {
    if (this.isMoving) {
      this.leftVal = this.leftVal + (val.offsetX - this.startMoveMousePositionX);
      this.topVal = this.topVal + (val.offsetY - this.startMoveMousePositionY);
    }
  }

  windowMouseLeave() {
    this.isMoving = false;
  }

  windowMouseUp(val) {
    this.isMoving = false;
  }

  closeClicked() {
    this.onClose.emit("");
  }
}
