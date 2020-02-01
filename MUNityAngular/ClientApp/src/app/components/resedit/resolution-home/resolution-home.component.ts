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

  constructor(private service: ResolutionService, private router: Router,
    private readonly nofitier: NotifierService, private userService: UserService) { }

  ngOnInit() {
  }

  createResolution() {
    this.service.createResolution().subscribe(n => {
      console.log(n);
      if (n != null) {
        if (n.ID != null) {
          this.router.navigate(['/resedit', n.ID])
        }
        this.nofitier.notify('error', 'Fehler beim Erstellen der Resolution.');
      } else {

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
