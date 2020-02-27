import { Component, OnInit } from '@angular/core';
import { ResolutionService } from '../../../services/resolution.service';
import { Router } from '@angular/router';
import { NotifierService } from 'angular-notifier';
import { UserService } from 'src/app/services/user.service';

@Component({
  selector: 'app-resolution-home',
  templateUrl: './resolution-home.component.html',
  styleUrls: ['./resolution-home.component.css']
})
export class ResolutionHomeComponent implements OnInit {

  public workOnId: string = "default";

  public displayId: string = "";

  constructor(public service: ResolutionService, private router: Router,
    private readonly nofitier: NotifierService, public userService: UserService) { }

  ngOnInit() {
  }

  createResolution() {
    this.service.createResolution().subscribe(n => {
      if (n != null) {
        if (n.ID != null) {
          this.router.navigate(['/resedit', n.ID])
        }
        
      } else {
        this.nofitier.notify('error', 'Fehler beim Erstellen der Resolution.');
      }
    },
      err => {
        this.nofitier.notify('error', 'Fehler beim Erstellen der Resolution.');
      }
    );
  }

  updateWorkTogetherKey(val) {
    this.workOnId = val;
  }

  workTogether(val) {
    this.workOnId = val;
    this.workTogetherClick();
  }

  workTogetherClick() {
    this.router.navigate(['/resedit/' + this.workOnId]);
  }

  updateDisplayKey(val) {
    this.displayId = val;
  }

  displayResolution(val) {
    this.displayId = val;
    this.displayResolutionClick();
  }

  displayResolutionClick() {
    this.router.navigate(['/resa/live/' + this.displayId]);
  }
}
