import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-explore-conferences',
  templateUrl: './explore-conferences.component.html',
  styleUrls: ['./explore-conferences.component.css']
})
export class ExploreConferencesComponent implements OnInit {

  pageDescription: string = '';

  munbwDescription: string = '';

  munbbDescription: string = '';

  constructor() { }

  ngOnInit() {
    this.pageDescription += '# MUN-SH';
    this.pageDescription += '\n\n';
    this.pageDescription += 'Model United Nations Schleswig-Holstein, kurz MUN-SH, ist ein __Planspiel für Schüler*innen sowie Studierende zwischen 15 und 21 Jahren__, bei dem die __Vereinten Nationen__ simuliert werden.\n\n';
    this.pageDescription += '__MUN-SH 2020__ findet vom 12. bis zum 16. März im Schleswig-Holsteinischen Landtag statt. Die Anmeldung ist geschlossen. Falls Sie sich auf die Warteliste setzen lassen wollen, um freiwerdende Plätze zu übernehmen, melden Sie sich bitte bei der Teilnehmendenbetreuung!\n\n';
    this.pageDescription += 'Auf den folgenden Seiten können Sie sich über das Projekt, Ihre Möglichkeiten zur Teilnahme, Förderungsmöglichkeiten und vieles mehr informieren!';

    this.munbwDescription += '# MUNBW\n\n';
    this.munbwDescription += 'Model United Nations Baden-Württemberg, kurz MUNBW, ist ein __Planspiel für Schülerinnen und Schüler sowie Studierende zwischen 15 und 21 Jahren__, bei dem die __Vereinten Nationen__ simuliert werden. In realitätsnahen Gremien der Vereinten Nationen debattierst Du als Delegierte*r eines Mitgliedstaates der Vereinten Nationen aktuelle, weltpolitische Themen.\n\n'
    this.munbwDescription += 'MUNBW findet 2020 __vom 21. bis 25. Mai im Hospitalhof in Stuttgart__ statt.\n\n';
    this.munbwDescription += 'Egal, ob Du selbst an einer __Teilnahme__ bei MUNBW interessiert bist, Sie als __Lehrkraft__ mit einer Gruppe von Schülerinnen und Schülern an MUNBW teilnehmen möchten und nach Möglichkeiten der Einbindung des Themas Vereinte Nationen in den Schulunterricht suchen oder Sie an einer __Förderung__ des Projektes interessiert sind: Auf diesen Seiten finden Sie die nötigen Informationen und die richtigen Ansprechpartner für Ihre und Eure Fragen.';

    this.munbbDescription += '# MUNBB\n\n'
    this.munbbDescription += 'Model United Nations Brandenburg, kurz MUNBB, ist ein __Planspiel für Schülerinnen und Schüler sowie Studierende zwischen 15 und 21 Jahren__, bei dem die __Vereinten Nationen__ simuliert werden\n\n';
    this.munbbDescription += 'Zum ersten Mal sind vom __15. bis 19. August 2019__ rund 200 Jugendliche im Landtag Brandenburg zu Potsdam in die Rolle von Diplomatinnen und Diplomaten geschlüpft. Bald werden Sie an dieser Stelle die Dokumentation der Konferenz, sowie alle Informationen zur nächsten Ausgabe von MUNBB im Jahr 2020 finden. Bis dahin können Sie weiterhin alle Infos zur Konferenz 2019 abrufen.\n\n';
    this.munbbDescription += 'Egal, ob Sie selber an einer __Teilnahme__ an MUNBB interessiert sind, Sie als __Lehrkraft__ mit einer Gruppe von Schülerinnen und Schülern an MUNBB teilnehmen möchten und nach Möglichkeiten der Einbindung des Themas Vereinte Nationen in den Schulunterricht suchen oder Sie an einer __Förderung__ des Projektes interessiert sind: Auf diesen Seiten finden Sie die nötigen Informationen sowie die richtigen Ansprechpartner für Rückfragen. '
  }

}
