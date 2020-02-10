import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-explore-conferences',
  templateUrl: './explore-conferences.component.html',
  styleUrls: ['./explore-conferences.component.css']
})
export class ExploreConferencesComponent implements OnInit {

  pageDescription: string = '';

  constructor() { }

  ngOnInit() {
    this.pageDescription += '# MUN-SH';
    this.pageDescription += '\n\n';
    this.pageDescription += 'Model United Nations Schleswig-Holstein, kurz MUN-SH, ist ein __Planspiel für Schüler*innen sowie Studierende zwischen 15 und 21 Jahren__, bei dem die __Vereinten Nationen__ simuliert werden.\n\n';
    this.pageDescription += '__MUN-SH 2020__ findet vom 12. bis zum 16. März im Schleswig-Holsteinischen Landtag statt. Die Anmeldung ist geschlossen. Falls Sie sich auf die Warteliste setzen lassen wollen, um freiwerdende Plätze zu übernehmen, melden Sie sich bitte bei der Teilnehmendenbetreuung!\n\n';
    this.pageDescription += 'Auf den folgenden Seiten können Sie sich über das Projekt, Ihre Möglichkeiten zur Teilnahme, Förderungsmöglichkeiten und vieles mehr informieren!';
  }

}
