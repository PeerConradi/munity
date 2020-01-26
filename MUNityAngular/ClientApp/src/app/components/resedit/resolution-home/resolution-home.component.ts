import { Component, OnInit } from '@angular/core';
import { ResolutionService } from '../../../services/resolution.service';
import { Router } from '@angular/router';
import { NotifierService } from 'angular-notifier';

@Component({
  selector: 'app-resolution-home',
  templateUrl: './resolution-home.component.html',
  styleUrls: ['./resolution-home.component.css']
})
export class ResolutionHomeComponent implements OnInit {

  constructor(private service: ResolutionService, private router: Router, private readonly nofitier: NotifierService) { }

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

}
