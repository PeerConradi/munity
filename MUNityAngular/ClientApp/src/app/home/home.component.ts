import { Component } from '@angular/core';
import { ConferenceService } from '../services/conference-service.service';
import { FormBuilder } from '@angular/forms';
import { Router } from '@angular/router';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent {

  public conferences;

  checkResolutionForm;

  constructor(private formBuilder: FormBuilder, private router: Router) {
    this.checkResolutionForm = this.formBuilder.group({
      resolutionkey: ''
    });
  }

  onCheckResolution(val) {
    this.router.navigate(['/resa/live/' + val.resolutionkey]);
  }

}
