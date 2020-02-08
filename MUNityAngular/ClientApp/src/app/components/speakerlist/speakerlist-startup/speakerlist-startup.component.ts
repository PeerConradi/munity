import { Component, OnInit } from '@angular/core';
import { SpeakerListService } from '../../../services/speaker-list.service';
import { Router } from '@angular/router';
import { NotifierService } from 'angular-notifier';

@Component({
  selector: 'app-speakerlist-startup',
  templateUrl: './speakerlist-startup.component.html',
  styleUrls: ['./speakerlist-startup.component.css']
})
export class SpeakerlistStartupComponent implements OnInit {

  constructor(private speakerlistService: SpeakerListService, private router: Router, private notifier: NotifierService) { }

  ngOnInit() {
  }

  createSpeakerlist() {
    this.speakerlistService.createSpeakerlist('', '').subscribe(n => {
      this.router.navigate(['/s/edit', n.ID])
    }, err => {
        this.notifier.notify('error', 'Fehler beim Erstellen einer neuen Redeliste!');
        console.log(err);
    });
  }

}
