import { Component, OnInit } from '@angular/core';
import { ResolutionService } from '../../../services/resolution.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-resolution-home',
  templateUrl: './resolution-home.component.html',
  styleUrls: ['./resolution-home.component.css']
})
export class ResolutionHomeComponent implements OnInit {

  constructor(private service: ResolutionService, private router: Router) { }

  ngOnInit() {
  }

  createResolution() {
    this.service.createResolution().subscribe(n => this.router.navigateByUrl('/resedit?id=' + n.ID));
  }

}
